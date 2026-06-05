import { Component, signal } from '@angular/core';
import { PronunciationAssessmentComponent } from './pronunciation/pronunciation-assessment.component';
import { TajweedAssessmentComponent } from './tajweed/tajweed-assessment.component';
import { LetterPracticeComponent } from './arabic-letters/letter-practice.component';
import { SyllableSoundComponent } from './syllable-sound/syllable-sound.component';

type Mode = 'pronunciation' | 'tajweed' | 'letters' | 'syllables';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [PronunciationAssessmentComponent, TajweedAssessmentComponent, LetterPracticeComponent, SyllableSoundComponent],
  template: `
    <nav class="app-nav">
      <button [class.active]="mode() === 'pronunciation'" (click)="mode.set('pronunciation')">
        Pronunciation
      </button>
      <button [class.active]="mode() === 'tajweed'" (click)="mode.set('tajweed')">
        Tajweed
      </button>
      <button [class.active]="mode() === 'letters'" (click)="mode.set('letters')">
        Letters
      </button>
      <button [class.active]="mode() === 'syllables'" (click)="mode.set('syllables')">
        Syllables
      </button>
    </nav>

    <div class="app-body">
      @if (mode() === 'pronunciation') {
        <app-pronunciation-assessment />
      } @else if (mode() === 'tajweed') {
        <app-tajweed-assessment />
      } @else if (mode() === 'syllables') {
        <app-syllable-sound />
      } @else {
        <app-letter-practice />
      }
    </div>
  `,
  styles: [`
    .app-nav {
      position: sticky;
      top: 0;
      z-index: 50;
      display: flex;
      justify-content: center;
      gap: 0;
      background: #080e1a;
      border-bottom: 1px solid #1e293b;
      padding: 0.625rem 1rem;
    }

    .app-nav button {
      background: transparent;
      border: 1px solid #334155;
      color: #64748b;
      padding: 0.4rem 1.25rem;
      font-size: 0.8rem;
      font-weight: 600;
      cursor: pointer;
      transition: all 0.15s;
      letter-spacing: 0.03em;
    }

    .app-nav button:first-child {
      border-radius: 0.4rem 0 0 0.4rem;
    }

    .app-nav button:last-child {
      border-radius: 0 0.4rem 0.4rem 0;
      border-left: none;
    }

    .app-nav button:not(:first-child):not(:last-child) {
      border-left: none;
    }

    .app-nav button.active {
      background: #1e293b;
      color: #f59e0b;
      border-color: #f59e0b;
    }

    .app-body { }
  `]
})
export class AppComponent {
  mode = signal<Mode>('letters');
}
