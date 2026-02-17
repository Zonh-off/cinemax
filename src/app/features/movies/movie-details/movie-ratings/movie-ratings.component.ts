import {Component, computed, input} from '@angular/core';
import {MovieDetails} from '../../../../shared/models/types';
import {IonicModule} from '@ionic/angular';
import {SectionHeaderComponent} from '../../../../shared/components/section-header/section-header.component';
import {StarRatingComponent} from '../../../../shared/components/star-rating/star-rating.component';

@Component({
  selector: 'app-movie-ratings',
  templateUrl: './movie-ratings.component.html',
  styleUrls: ['./movie-ratings.component.css'],
  imports: [
    IonicModule,
    SectionHeaderComponent,
    StarRatingComponent
  ],
  standalone: true
})
export class MovieRatingsComponent {
  movie = input.required<MovieDetails>();

  getVoteHeight(score: number): number {
    const average = this.movie()?.voteAverage || 0;
    const total = this.movie()?.voteCount || 0;

    if (total === 0) return 0;

    const sigma = 1.5;
    const height = Math.exp(-Math.pow(score - average, 2) / (2 * Math.pow(sigma, 2)));

    return height * 100;
  }
}
