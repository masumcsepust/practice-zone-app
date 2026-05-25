import { Component } from '@angular/core';
import { RecitationComponent } from './recitation/recitation.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RecitationComponent],
  template: '<app-recitation />'
})
export class AppComponent {}
