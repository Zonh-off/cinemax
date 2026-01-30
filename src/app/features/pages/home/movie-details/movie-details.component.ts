import {Component, computed, inject, OnInit, signal} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {IonicModule} from '@ionic/angular';
import {MovieService} from '../../../../core/services/movie.service';
import {MovieDetails} from '../../../../shared/models/types';

@Component({
  selector: 'app-movie-details',
  templateUrl: './movie-details.component.html',
  styleUrls: ['./movie-details.component.css'],
  imports: [
    IonicModule
  ],
  standalone: true
})
export class MovieDetailsComponent  implements OnInit {
  private route = inject(ActivatedRoute);
  private movieService = inject(MovieService);
  movie = signal<MovieDetails | null>(null);
  stars = computed(() => {
    const rating = this.movie()?.voteAverage || 0;
    const scaledRating = rating / 2;
    const starsArray = [];

    for (let i = 1; i <= 5; i++) {
      if (i <= scaledRating) {
        starsArray.push('star');
      } else if (i - 0.5 <= scaledRating) {
        starsArray.push('star-half');
      } else {
        starsArray.push('star-outline');
      }
    }
    return starsArray;
  });

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if(!id) return;
    this.movieService.getMovie(+id).subscribe({
      next: movie => this.movie.set(movie),
      error: err => console.log(err)
    });
  }

  getVoteHeight(score: number): number {
    const average = this.movie()?.voteAverage || 0;
    const total = this.movie()?.voteCount || 0;

    if (total === 0) return 0;

    const sigma = 1.5;
    const height = Math.exp(-Math.pow(score - average, 2) / (2 * Math.pow(sigma, 2)));

    return height * 100;
  }
}
