import { Component, OnDestroy, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuranApiService, ArabicLetterDto } from '../services/quran-api.service';

type PracticeState = 'idle' | 'connecting' | 'recording' | 'processing' | 'done' | 'error';

interface LetterResult {
  letter: string;
  letterName: string;
  recognizedText: string;
  pronunciationScore: number;
  accuracyScore: number;
  isCorrect: boolean;
  feedback: string;
  feedbackBn?: string;
  makhrajHint?: string;
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

  private ws: WebSocket | null               = null;
  private mediaRecorder: MediaRecorder | null = null;
  private stream: MediaStream | null          = null;
  private recordingMimeType                   = 'audio/webm';

  // ── Reactive state ─────────────────────────────────────────────────────

  state         = signal<PracticeState>('idle');
  statusMessage = signal('Select a letter and tap 🎤 Start');
  errorMessage  = signal('');
  result        = signal<LetterResult | null>(null);

  letters        = signal<ArabicLetterDto[]>([]);
  selectedLetter = signal<ArabicLetterDto | null>(null);

  get canStart() {
    const s = this.state();
    return !!this.selectedLetter() && (s === 'idle' || s === 'done' || s === 'error');
  }
  get canStop() { return this.state() === 'recording'; }

  // ── Lifecycle ──────────────────────────────────────────────────────────

  ngOnInit(): void {
    this.api.getLetters().subscribe(list => {
      this.letters.set(list);
      if (list.length > 0) this.selectedLetter.set(list[0]);
    });
  }

  selectLetter(letter: ArabicLetterDto): void {
    const s = this.state();
    if (s === 'connecting' || s === 'recording' || s === 'processing') return;
    this.selectedLetter.set(letter);
    this.result.set(null);
    this.errorMessage.set('');
    if (s !== 'idle') {
      this.state.set('idle');
      this.statusMessage.set('Select a letter and tap 🎤 Start');
    }
  }

  // ── WebSocket + Mic ────────────────────────────────────────────────────

  async startPractice(): Promise<void> {
    const letter = this.selectedLetter();
    if (!letter) return;

    this.state.set('connecting');
    this.statusMessage.set('Connecting…');
    this.errorMessage.set('');
    this.result.set(null);

    this.ws = new WebSocket(`${this.WS_BASE}?letterId=${letter.id}`);
    this.ws.binaryType = 'arraybuffer';

    this.ws.onopen = () => {
      this.startMic().catch(err => console.error('startMic:', err));
    };

    this.ws.onmessage = (event: MessageEvent) => {
      try {
        const msg = JSON.parse(event.data as string);
        if (msg.type === 'letter-result') {
          this.result.set(msg as LetterResult);
          this.state.set('done');
          this.statusMessage.set('Done — tap 🎤 Start to try again');
        } else if (msg.type === 'error') {
          this.state.set('error');
          this.statusMessage.set('Server error');
          this.errorMessage.set(msg.message ?? 'Server error');
        }
      } catch {
        console.warn('[WS] unparseable message:', event.data);
      }
    };

    this.ws.onclose = () => {
      this.stopMicTracks();
      const s = this.state();
      if (s !== 'done' && s !== 'error') {
        this.state.set('idle');
        this.statusMessage.set('Select a letter and tap 🎤 Start');
      }
      this.ws = null;
    };

    this.ws.onerror = () => {
      this.state.set('error');
      this.statusMessage.set('Connection error');
      this.errorMessage.set(`Could not connect to ${this.WS_BASE}`);
      this.stopMicTracks();
    };
  }

  private async startMic(): Promise<void> {
    try {
      this.stream = await navigator.mediaDevices.getUserMedia({ audio: true, video: false });

      this.recordingMimeType = MediaRecorder.isTypeSupported('audio/webm;codecs=opus')
        ? 'audio/webm;codecs=opus'
        : 'audio/webm';

      this.mediaRecorder = new MediaRecorder(this.stream, { mimeType: this.recordingMimeType });

      this.mediaRecorder.ondataavailable = (event: BlobEvent) => {
        if (event.data.size > 0 && this.ws?.readyState === WebSocket.OPEN) {
          event.data.arrayBuffer().then(buf => this.ws!.send(buf));
        }
      };

      // Send mime type + end-of-audio AFTER final ondataavailable fires (onstop ordering guarantee)
      this.mediaRecorder.onstop = () => {
        if (this.ws?.readyState === WebSocket.OPEN) {
          // Strip codec params: "audio/webm;codecs=opus" → "audio/webm"
          const baseMime = this.recordingMimeType.split(';')[0];
          this.ws.send(`mime:${baseMime}|end-of-audio`);
        }
      };

      this.mediaRecorder.start(this.CHUNK_MS);
      this.state.set('recording');
      this.statusMessage.set('Recording — say the letter clearly');

    } catch (err) {
      this.state.set('error');
      this.errorMessage.set('Microphone access denied or unavailable.');
      this.ws?.close();
      console.error('getUserMedia error:', err);
    }
  }

  stopAndSubmit(): void {
    this.state.set('processing');
    this.statusMessage.set('Analysing… result will appear shortly');
    this.mediaRecorder?.stop();
    this.mediaRecorder = null;
    this.stopMicTracks();
  }

  clearResult(): void {
    this.result.set(null);
    this.errorMessage.set('');
    const s = this.state();
    if (s === 'done' || s === 'error') {
      this.state.set('idle');
      this.statusMessage.set('Select a letter and tap 🎤 Start');
    }
  }

  // ── Audio playback ─────────────────────────────────────────────────────

  playAudio(letter: ArabicLetterDto): void {
    const audio = new Audio(`http://localhost:5092${letter.audioUrl}`);
    audio.play().catch(err => console.error('Audio play failed:', err));
  }

  // ── Helpers ────────────────────────────────────────────────────────────

  scoreColor(score: number): string {
    if (score >= 90) return '#22c55e';
    if (score >= 75) return '#84cc16';
    if (score >= 60) return '#f59e0b';
    return '#ef4444';
  }

  private stopMicTracks(): void {
    this.stream?.getTracks().forEach(t => t.stop());
    this.stream = null;
  }

  private cleanup(): void {
    this.mediaRecorder?.stop();
    this.mediaRecorder = null;
    this.stopMicTracks();
    this.ws?.close();
    this.ws = null;
  }

  ngOnDestroy(): void {
    this.cleanup();
  }
}
