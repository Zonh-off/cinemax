import {Component, input} from '@angular/core';
import {MovieDetails} from '../../../../shared/models/types';
import {IonicModule} from '@ionic/angular';
import {SectionHeaderComponent} from '../../../../shared/components/section-header/section-header.component';

@Component({
  selector: 'app-movie-trailer',
  templateUrl: './movie-trailer.component.html',
  styleUrls: ['./movie-trailer.component.css'],
  imports: [
    IonicModule,
    SectionHeaderComponent
  ],
  standalone: true
})
export class MovieTrailerComponent {
  movie = input.required<MovieDetails>();
}
