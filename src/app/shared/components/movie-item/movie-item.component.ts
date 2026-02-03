import {Component, input} from '@angular/core';
import {IonicModule} from '@ionic/angular';
import {Movie} from '../../models/types';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-movie-item',
  templateUrl: './movie-item.component.html',
  styleUrls: ['./movie-item.component.css'],
  imports: [
    IonicModule,
    RouterLink
  ],
  standalone: true
})
export class MovieItemComponent {
  movie = input.required<Movie>()
}
