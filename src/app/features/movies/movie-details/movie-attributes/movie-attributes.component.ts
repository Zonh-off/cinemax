import {Component, input} from '@angular/core';
import {MovieDetails} from '../../../../shared/models/types';

@Component({
  selector: 'app-movie-attributes',
  templateUrl: './movie-attributes.component.html',
  styleUrls: ['./movie-attributes.component.css'],
  standalone: true
})
export class MovieAttributesComponent{
  movie = input.required<MovieDetails>();
}
