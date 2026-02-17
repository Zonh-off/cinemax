import {Component, input} from '@angular/core';
import {MovieDetails} from '../../../../shared/models/types';
import {
  HorizontalSectionComponent
} from '../../../../shared/components/horizontal-section/horizontal-section.component';

@Component({
  selector: 'app-movie-photos',
  templateUrl: './movie-photos.component.html',
  styleUrls: ['./movie-photos.component.css'],
  imports: [
    HorizontalSectionComponent
  ],
  standalone: true
})
export class MoviePhotosComponent {
  movie = input.required<MovieDetails>();
}
