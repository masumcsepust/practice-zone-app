import { Component, OnDestroy, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuranApiService, ArabicLetterDto } from '../services/quran-api.service';

type Status = 'disconnected' | 'connecting' | 'connected' | 'error';

interface LetterResult {
  letter: string;
  letterName: string;
  recognizedText: string;
  pronunciationScore: number;
  accuracyScore: number;
  isCorrect: boolean;
  feedback: string;
}

@Component({
  selector: 'app-letter-practice',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './letter-practice.component.html',
  styleUrl: './letter-practice.component.css'
})
export class LetterPracticeComponent implements OnInit, OnDestroy {

  private readonly WS_BASE  = 'ws://localhost:5092/ws/letter';
  private readonly CHUNK_MS = 250;
  private readonly api      = inject(QuranApiService);

  private ws: WebSocket | null              = null;
  private mediaRecorder: MediaRecorder | null = null;
  private stream: MediaStream | null        = null;

  // ── Reactive state ────────────────────────────────────────────────────

  status        = signal<Status>('disconnected');
  statusMessage = signal('Pick a letter, then tap Connect');
  isRecording   = signal(false);
  errorMessage  = signal('');
  result        = signal<LetterResult | null>(null);

  letters        = signal<ArabicLetterDto[]>([]);
  selectedLetter = signal<ArabicLetterDto | null>(null);

  get canConnect()    { return this.status() === 'disconnected' || this.status() === 'error'; }
  get canDisconnect() { return this.status() === 'connected'; }
  get canRecord()     { return this.status() === 'connected' && !this.isRecording(); }
  get canStop()       { return this.isRecording(); }

  // ── Lifecycle ─────────────────────────────────────────────────────────

  ngOnInit(): void {
    this.api.getLetters().subscribe(list => {
      this.letters.set(list);
      if (list.length > 0) this.selectedLetter.set(list[0]);
    });
  }

  selectLetter(letter: ArabicLetterDto): void {
    if (this.status() !== 'disconnected' && this.status() !== 'error') return;
    this.selectedLetter.set(letter);
    this.result.set(null);
    this.errorMessage.set('');
  }

  // ── WebSocket ─────────────────────────────────────────────────────────

  connect(): void {
    const letter = this.selectedLetter();
    if (!letter || this.ws) return;

    const url = `${this.WS_BASE}?letterId=${letter.id}`;

    this.status.set('connecting');
    this.statusMessage.set('Connecting…');
    this.errorMessage.set('');
    this.result.set(null);

    this.ws = new WebSocket(url);
    this.ws.binaryType = 'arraybuffer';

    this.ws.onopen = () => {
      this.status.set('connected');
      this.statusMessage.set('Connected — start recording and say the letter');
    };

    this.ws.onmessage = (event: MessageEvent) => {
      try {
        const msg = JSON.parse(event.data as string);
        if (msg.type === 'letter-result') {
          this.result.set(msg as LetterResult);
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
      this.statusMessage.set('Recording… say the letter now');

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

  playAudio(letter: ArabicLetterDto): void {
    const audio = new Audio(`http://localhost:5092${letter.audioUrl}`);
    audio.play().catch(err => console.error('Audio play failed:', err));
  }

  ngOnDestroy(): void {
    this.disconnect();
  }
}
