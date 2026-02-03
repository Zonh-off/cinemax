import {Component} from '@angular/core';
import {IonicModule} from '@ionic/angular';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-auth',
  imports: [
    IonicModule,
    RouterLink
  ],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
  standalone: true
})
export class AuthComponent {
}
