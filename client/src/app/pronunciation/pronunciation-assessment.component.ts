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

interface PronunciationResult {
  recognizedText: string;
  pronunciationScore: number;
  accuracyScore: number;
  fluencyScore: number;
  completenessScore: number;
  words: WordResult[];
  weakPhonemes: PhonemeResult[];
}

@Component({
  selector: 'app-pronunciation-assessment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './pronunciation-assessment.component.html',
  styleUrl: './pronunciation-assessment.component.css'
})
export class PronunciationAssessmentComponent implements OnInit, OnDestroy {

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
  result        = signal<PronunciationResult | null>(null);
  lastRawMessage = signal('');

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

    const url = `${this.WS_BASE}?ayahId=${this.selectedAyahId}`;

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
      const raw = event.data as string;
      this.lastRawMessage.set(raw);
      console.log('[WS] received:', raw);

      try {
        const msg = JSON.parse(raw);
        if (msg.type === 'pronunciation') {
          this.result.set(msg as PronunciationResult);
        } else if (msg.type === 'error') {
          this.errorMessage.set(msg.message ?? 'Server error');
        }
      } catch {
        console.warn('[WS] unparseable message:', raw);
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
      this.statusMessage.set('Recording stopped — result above');
    }
  }

  clearResult(): void {
    this.result.set(null);
  }

  // ── Score helpers ─────────────────────────────────────────────────────

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

  isPerfect(r: PronunciationResult): boolean {
    return r.words.length > 0
        && r.words.filter(w => !w.isCorrect).length === 0
        && r.weakPhonemes.length === 0;
  }

  ngOnDestroy(): void {
    this.disconnect();
  }
}
