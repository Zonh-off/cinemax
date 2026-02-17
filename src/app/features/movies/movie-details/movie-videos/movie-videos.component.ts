import {Component, input} from '@angular/core';
import {MovieDetails} from '../../../../shared/models/types';
import {
  HorizontalSectionComponent
} from '../../../../shared/components/horizontal-section/horizontal-section.component';
import {IonicModule} from '@ionic/angular';

@Component({
  selector: 'app-movie-videos',
  templateUrl: './movie-videos.component.html',
  styleUrls: ['./movie-videos.component.css'],
  imports: [
    HorizontalSectionComponent,
    IonicModule
  ],
  standalone: true
})
export class MovieVideosComponent {
  movie = input.required<MovieDetails>();
}
