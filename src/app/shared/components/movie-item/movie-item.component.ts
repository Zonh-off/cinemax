import {Component, input, Input} from '@angular/core';
import {IonicModule} from '@ionic/angular';
import {Movie} from '../../models/types';

@Component({
  selector: 'app-movie-item',
  templateUrl: './movie-item.component.html',
  styleUrls: ['./movie-item.component.css'],
  imports: [
    IonicModule
  ],
  standalone: true
})
export class MovieItemComponent {
  movie = input.required<Movie>()
  @Input() isWishlisted: boolean = false;
}
