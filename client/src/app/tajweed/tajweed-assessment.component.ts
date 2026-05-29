import { Component, OnDestroy, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { QuranApiService, SurahDto, AyahDto } from '../services/quran-api.service';

type Status = 'disconnected' | 'connecting' | 'connected' | 'error';

interface WordResult {
  word: string;
  accuracyScore: number;
  isCorrect: boolean;
  errorType: string;
}

interface PhonemeResult {
  phoneme: string;
  accuracyScore: number;
  duration: number;
  isWeak: boolean;
}

interface TajweedIssue {
  ruleType: string;
  word: string;
  affectedLetter: string;
  feedback: string;
  severity: 'low' | 'medium' | 'high';
  actualDurationMs: number | null;
  expectedDurationMs: number | null;
}

interface TajweedResult {
  recognizedText: string;
  pronunciationScore: number;
  accuracyScore: number;
  fluencyScore: number;
  completenessScore: number;
  tajweedScore: number;
  words: WordResult[];
  weakPhonemes: PhonemeResult[];
  tajweedIssues: TajweedIssue[];
}

const ARABIC_NAMES: Record<string, string> = {
  Madd:     'مَدّ',
  Ghunnah:  'غُنَّة',
  Qalqalah: 'قَلْقَلَة',
  Ikhfa:    'إِخْفَاء',
  Idgham:   'إِدْغَام',
};

@Component({
  selector: 'app-tajweed-assessment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tajweed-assessment.component.html',
  styleUrl: './tajweed-assessment.component.css'
})
export class TajweedAssessmentComponent implements OnInit, OnDestroy {

  private readonly WS_BASE  = 'ws://localhost:5092/ws/recitation';
  private readonly CHUNK_MS = 250;
  private readonly api      = inject(QuranApiService);

  private ws: WebSocket | null              = null;
  private mediaRecorder: MediaRecorder | null = null;
  private stream: MediaStream | null        = null;

  // ── Reactive state ────────────────────────────────────────────────────

  status        = signal<Status>('disconnected');
  statusMessage = signal('Select a surah and ayah, then click Connect');
  isRecording   = signal(false);
  errorMessage  = signal('');
  result        = signal<TajweedResult | null>(null);

  surahs = signal<SurahDto[]>([]);
  ayahs  = signal<AyahDto[]>([]);

  selectedSurahId = 1;
  selectedAyahId  = 1;

  get selectedAyah(): AyahDto | undefined {
    return this.ayahs().find(a => a.id === this.selectedAyahId);
  }

  get canConnect()    { return this.status() === 'disconnected' || this.status() === 'error'; }
  get canDisconnect() { return this.status() === 'connected'; }
  get canRecord()     { return this.status() === 'connected' && !this.isRecording(); }
  get canStop()       { return this.isRecording(); }

  // ── Lifecycle ─────────────────────────────────────────────────────────

  ngOnInit(): void {
    this.api.getSurahs().subscribe(list => {
      this.surahs.set(list);
      if (list.length > 0) this.loadAyahs(list[0].id);
    });
  }

  loadAyahs(surahId: number): void {
    this.selectedSurahId = surahId;
    this.api.getAyahs(surahId).subscribe(list => {
      this.ayahs.set(list);
      if (list.length > 0) this.selectedAyahId = list[0].id;
    });
  }

  // ── WebSocket ─────────────────────────────────────────────────────────

  connect(): void {
    if (this.ws) return;

    const url = `${this.WS_BASE}?ayahId=${this.selectedAyahId}&tajweed=true`;

    this.status.set('connecting');
    this.statusMessage.set('Connecting…');
    this.errorMessage.set('');
    this.result.set(null);

    this.ws = new WebSocket(url);
    this.ws.binaryType = 'arraybuffer';

    this.ws.onopen = () => {
      this.status.set('connected');
      this.statusMessage.set('Connected — ready to record');
    };

    this.ws.onmessage = (event: MessageEvent) => {
      try {
        const msg = JSON.parse(event.data as string);
        if (msg.type === 'tajweed') {
          this.result.set(msg as TajweedResult);
        } else if (msg.type === 'error') {
          this.errorMessage.set(msg.message ?? 'Server error');
        }
      } catch {
        console.warn('[WS] unparseable message:', event.data);
      }
    };

    this.ws.onclose = () => {
      this.status.set('disconnected');
      this.statusMessage.set('Disconnected');
      this.ws = null;
      this.result.set(null);
      this.stopRecording();
    };

    this.ws.onerror = () => {
      this.status.set('error');
      this.statusMessage.set('Connection error');
      this.errorMessage.set(`Could not connect to ${this.WS_BASE}`);
    };
  }

  disconnect(): void {
    this.stopRecording();
    this.ws?.close();
    this.ws = null;
    this.status.set('disconnected');
    this.statusMessage.set('Disconnected');
  }

  // ── Microphone ────────────────────────────────────────────────────────

  async startRecording(): Promise<void> {
    if (!this.ws || this.ws.readyState !== WebSocket.OPEN) return;

    try {
      this.stream = await navigator.mediaDevices.getUserMedia({ audio: true, video: false });

      const mimeType = MediaRecorder.isTypeSupported('audio/webm;codecs=opus')
        ? 'audio/webm;codecs=opus'
        : 'audio/webm';

      this.mediaRecorder = new MediaRecorder(this.stream, { mimeType });

      this.mediaRecorder.ondataavailable = (event: BlobEvent) => {
        if (event.data.size > 0 && this.ws?.readyState === WebSocket.OPEN) {
          event.data.arrayBuffer().then(buf => this.ws!.send(buf));
        }
      };

      this.mediaRecorder.start(this.CHUNK_MS);
      this.isRecording.set(true);
      this.statusMessage.set('Recording… recite now');

    } catch (err) {
      this.errorMessage.set('Microphone access denied or unavailable.');
      console.error('getUserMedia error:', err);
    }
  }

  stopRecording(): void {
    this.mediaRecorder?.stop();
    this.mediaRecorder = null;
    this.stream?.getTracks().forEach(t => t.stop());
    this.stream = null;
    this.isRecording.set(false);
    if (this.status() === 'connected') {
      this.statusMessage.set('Analysing… result will appear above');
      if (this.ws?.readyState === WebSocket.OPEN) {
        this.ws.send(JSON.stringify({ type: 'end-of-audio' }));
      }
    }
  }

  clearResult(): void {
    this.result.set(null);
  }

  // ── Helpers ───────────────────────────────────────────────────────────

  scoreColor(score: number): string {
    if (score >= 90) return '#22c55e';
    if (score >= 75) return '#84cc16';
    if (score >= 60) return '#f59e0b';
    return '#ef4444';
  }

  scoreGrade(score: number): string {
    if (score >= 90) return 'Excellent';
    if (score >= 75) return 'Good';
    if (score >= 60) return 'Needs Work';
    return 'Weak';
  }

  tajweedColor(score: number): string {
    if (score >= 90) return '#d97706';
    if (score >= 75) return '#f59e0b';
    if (score >= 60) return '#fb923c';
    return '#ef4444';
  }

  severityColor(severity: string): string {
    if (severity === 'high')   return '#ef4444';
    if (severity === 'medium') return '#f59e0b';
    return '#22c55e';
  }

  arabicName(ruleType: string): string {
    return ARABIC_NAMES[ruleType] ?? ruleType;
  }

  isPerfect(r: TajweedResult): boolean {
    return r.words.length > 0
        && r.words.filter(w => !w.isCorrect).length === 0
        && r.weakPhonemes.length === 0
        && r.tajweedIssues.length === 0;
  }

  ngOnDestroy(): void {
    this.disconnect();
  }
}
