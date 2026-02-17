import { Component, input } from '@angular/core';
import {IonicModule} from '@ionic/angular';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-section-header',
  templateUrl: './section-header.component.html',
  styleUrls: ['./section-header.component.css'],
  imports: [
    IonicModule,
    RouterLink
  ],
  standalone: true
})
export class SectionHeaderComponent {
  title = input.required<string>();
  routerUrl = input<string>();
}
