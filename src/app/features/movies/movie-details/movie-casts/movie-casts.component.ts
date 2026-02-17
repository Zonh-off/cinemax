import {Component, input} from '@angular/core';
import {MovieDetails} from '../../../../shared/models/types';
import {
  HorizontalSectionComponent
} from '../../../../shared/components/horizontal-section/horizontal-section.component';

@Component({
  selector: 'app-movie-casts',
  templateUrl: './movie-casts.component.html',
  styleUrls: ['./movie-casts.component.css'],
  imports: [
    HorizontalSectionComponent
  ],
  standalone: true
})
export class MovieCastsComponent {
  movie = input.required<MovieDetails>();
}
