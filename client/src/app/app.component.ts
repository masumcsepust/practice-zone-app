import { Component } from '@angular/core';
import { PronunciationAssessmentComponent } from './pronunciation/pronunciation-assessment.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [PronunciationAssessmentComponent],
  template: '<app-pronunciation-assessment />'
})
export class AppComponent {}
