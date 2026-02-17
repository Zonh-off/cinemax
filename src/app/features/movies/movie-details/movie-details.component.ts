import {Component, inject, OnInit, signal} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {IonicModule} from '@ionic/angular';
import {MovieService} from '../../../core/services/movie.service';
import {MovieDetails} from '../../../shared/models/types';
import {
  BaseModalPageLayoutComponent
} from '../../../shared/components/base-modal-page-layout/base-modal-page-layout.component';
import {MovieAttributesComponent} from './movie-attributes/movie-attributes.component';
import {MovieCastsComponent} from './movie-casts/movie-casts.component';
import {MoviePhotosComponent} from './movie-photos/movie-photos.component';
import {MovieVideosComponent} from './movie-videos/movie-videos.component';
import {MovieTrailerComponent} from './movie-trailer/movie-trailer.component';
import {MovieRatingsComponent} from './movie-ratings/movie-ratings.component';
import {MovieSynopsisComponent} from './movie-synopsis/movie-synopsis.component';

@Component({
  selector: 'app-movie-details',
  templateUrl: './movie-details.component.html',
  styleUrls: ['./movie-details.component.css'],
  imports: [
    IonicModule,
    BaseModalPageLayoutComponent,
    MovieAttributesComponent,
    MovieCastsComponent,
    MoviePhotosComponent,
    MovieVideosComponent,
    MovieTrailerComponent,
    MovieRatingsComponent,
    MovieSynopsisComponent
  ],
  standalone: true
})
export class MovieDetailsComponent  implements OnInit {
  private route = inject(ActivatedRoute);
  private movieService = inject(MovieService);
  movie = signal<MovieDetails | null>(null);


  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    console.log(id)
    if(!id) return;
    this.movieService.getMovie(+id).subscribe({
      next: movie => this.movie.set(movie),
      error: err => console.log(err)
    });
  }
}
