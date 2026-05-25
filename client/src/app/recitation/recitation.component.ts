import { Component, OnDestroy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

type ConnectionStatus = 'disconnected' | 'connecting' | 'connected' | 'error';

@Component({
  selector: 'app-recitation',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './recitation.component.html',
  styleUrl: './recitation.component.css'
})
export class RecitationComponent implements OnDestroy {

  private readonly WS_URL = 'ws://localhost:5092/ws/recitation';
  private readonly CHUNK_INTERVAL_MS = 250;

  private ws: WebSocket | null = null;
  private mediaRecorder: MediaRecorder | null = null;
  private stream: MediaStream | null = null;

  // Reactive state
  status        = signal<ConnectionStatus>('disconnected');
  isRecording   = signal(false);
  partialText   = signal('');
  finalText     = signal('');
  errorMessage  = signal('');
  statusMessage = signal('Click Connect to start');

  // ── WebSocket ───────────────────────────────────────────────────

  connect(): void {
    if (this.ws) return;

    this.status.set('connecting');
    this.statusMessage.set('Connecting…');
    this.errorMessage.set('');

    this.ws = new WebSocket(this.WS_URL);
    this.ws.binaryType = 'arraybuffer';

    this.ws.onopen = () => {
      this.status.set('connected');
      this.statusMessage.set('Connected — ready to record');
    };

    this.ws.onmessage = (event: MessageEvent) => {
      try {
        const msg: { type: string; text: string } = JSON.parse(event.data);
        if (msg.type === 'partial') {
          this.partialText.set(msg.text);
        } else if (msg.type === 'final') {
          this.finalText.set((this.finalText() + ' ' + msg.text).trim());
          this.partialText.set('');
        }
      } catch {
        console.warn('Could not parse WebSocket message:', event.data);
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
      this.errorMessage.set('Could not connect to ws://localhost:5092/ws/recitation');
    };
  }

  disconnect(): void {
    this.stopRecording();
    this.ws?.close();
    this.ws = null;
    this.status.set('disconnected');
    this.statusMessage.set('Disconnected');
  }

  // ── Microphone ──────────────────────────────────────────────────

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
          event.data.arrayBuffer().then(buffer => this.ws!.send(buffer));
        }
      };

      this.mediaRecorder.start(this.CHUNK_INTERVAL_MS);
      this.isRecording.set(true);
      this.statusMessage.set('Recording… speak now');
      this.partialText.set('');

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
      this.statusMessage.set('Recording stopped');
    }
  }

  clearText(): void {
    this.partialText.set('');
    this.finalText.set('');
  }

  // ── Helpers ─────────────────────────────────────────────────────

  get canConnect()    { return this.status() === 'disconnected' || this.status() === 'error'; }
  get canDisconnect() { return this.status() === 'connected'; }
  get canRecord()     { return this.status() === 'connected' && !this.isRecording(); }
  get canStop()       { return this.isRecording(); }

  ngOnDestroy(): void {
    this.disconnect();
  }
}
