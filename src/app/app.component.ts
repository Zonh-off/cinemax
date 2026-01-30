import {Component} from '@angular/core';
import {IonicModule} from '@ionic/angular';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [
    IonicModule,
    CommonModule
  ],
  templateUrl: './app.component.html',
  standalone: true,
  styleUrl: './app.component.css'
})
export class AppComponent {

}
