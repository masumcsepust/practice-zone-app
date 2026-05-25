import { Component, OnDestroy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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

interface Ayah {
  id: number;
  number: number;
  arabic: string;
  translation: string;
}

@Component({
  selector: 'app-pronunciation-assessment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './pronunciation-assessment.component.html',
  styleUrl: './pronunciation-assessment.component.css'
})
export class PronunciationAssessmentComponent implements OnDestroy {

  private readonly WS_BASE       = 'ws://localhost:5092/ws/recitation';
  private readonly CHUNK_MS      = 250;

  private ws: WebSocket | null            = null;
  private mediaRecorder: MediaRecorder | null = null;
  private stream: MediaStream | null      = null;

  // ── Reactive state ────────────────────────────────────────────────────

  status        = signal<Status>('disconnected');
  statusMessage = signal('Select an ayah, then click Connect');
  isRecording   = signal(false);
  errorMessage  = signal('');
  result        = signal<PronunciationResult | null>(null);

  selectedAyahId = 1;   // plain property — works with ngModel

  // ── Al-Fatiha ayahs (IDs 1–7 from DB seed) ───────────────────────────

  readonly ayahs: Ayah[] = [
    { id: 1, number: 1,
      arabic: 'بِسْمِ اللَّهِ الرَّحْمَٰنِ الرَّحِيمِ',
      translation: 'In the name of Allah, the Most Gracious, the Most Merciful' },
    { id: 2, number: 2,
      arabic: 'الْحَمْدُ لِلَّهِ رَبِّ الْعَالَمِينَ',
      translation: 'Praise be to Allah, Lord of the Worlds' },
    { id: 3, number: 3,
      arabic: 'الرَّحْمَٰنِ الرَّحِيمِ',
      translation: 'The Most Gracious, the Most Merciful' },
    { id: 4, number: 4,
      arabic: 'مَالِكِ يَوْمِ الدِّينِ',
      translation: 'Master of the Day of Judgment' },
    { id: 5, number: 5,
      arabic: 'إِيَّاكَ نَعْبُدُ وَإِيَّاكَ نَسْتَعِينُ',
      translation: 'You alone we worship, and You alone we ask for help' },
    { id: 6, number: 6,
      arabic: 'اهْدِنَا الصِّرَاطَ الْمُسْتَقِيمَ',
      translation: 'Guide us to the straight path' },
    { id: 7, number: 7,
      arabic: 'صِرَاطَ الَّذِينَ أَنْعَمْتَ عَلَيْهِمْ غَيْرِ الْمَغْضُوبِ عَلَيْهِمْ وَلَا الضَّالِّينَ',
      translation: 'The path of those You have blessed, not those who earned anger or went astray' },
  ];

  get selectedAyah(): Ayah {
    return this.ayahs.find(a => a.id === this.selectedAyahId) ?? this.ayahs[0];
  }

  get canConnect()    { return this.status() === 'disconnected' || this.status() === 'error'; }
  get canDisconnect() { return this.status() === 'connected'; }
  get canRecord()     { return this.status() === 'connected' && !this.isRecording(); }
  get canStop()       { return this.isRecording(); }

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
      try {
        const msg = JSON.parse(event.data as string);
        if (msg.type === 'pronunciation') {
          this.result.set(msg as PronunciationResult);
        } else if (msg.type === 'error') {
          this.errorMessage.set(msg.message ?? 'Server error');
        }
      } catch {
        console.warn('Unparseable WS message:', event.data);
      }
    };

    this.ws.onclose = () => {
      this.status.set('disconnected');
      this.statusMessage.set('Disconnected');
      this.ws = null;
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
