import {Component, input } from '@angular/core';
import {IonicModule} from '@ionic/angular';

@Component({
  selector: 'app-base-modal-page-layout',
  templateUrl: './base-modal-page-layout.component.html',
  styleUrls: ['./base-modal-page-layout.component.css'],
  imports: [
    IonicModule
  ],
  standalone: true
})
export class BaseModalPageLayoutComponent {
  title = input.required<string>();
  backRouterUrl = input.required<string>();
}
