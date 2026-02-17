import {Component, input} from '@angular/core';
import { MovieDetails } from '../../../../shared/models/types';
import {SectionHeaderComponent} from '../../../../shared/components/section-header/section-header.component';

@Component({
  selector: 'app-movie-synopsis',
  templateUrl: './movie-synopsis.component.html',
  styleUrls: ['./movie-synopsis.component.css'],
  imports: [
    SectionHeaderComponent
  ],
  standalone: true
})
export class MovieSynopsisComponent {
  movie = input.required<MovieDetails>();
}
