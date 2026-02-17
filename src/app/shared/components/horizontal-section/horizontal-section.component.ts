import {Component, input} from '@angular/core';
import {IonicModule} from '@ionic/angular';
import {SectionHeaderComponent} from '../section-header/section-header.component';

@Component({
  selector: 'app-horizontal-section',
  templateUrl: './horizontal-section.component.html',
  styleUrls: ['./horizontal-section.component.css'],
  imports: [
    IonicModule,
    SectionHeaderComponent
  ],
  standalone: true
})
export class HorizontalSectionComponent {
  title = input.required<string>();
  routerUrl = input<string | undefined>();
}
