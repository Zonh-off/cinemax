import {Component, inject, OnInit, signal} from '@angular/core';
import {IonicModule} from '@ionic/angular';
import {MovieItemComponent} from '../../../shared/components/movie-item/movie-item.component';
import * as allIcons from 'ionicons/icons';
import { addIcons } from 'ionicons';
import {Router, RouterLink} from '@angular/router';
import {Movie, MoviesParams, Pagination} from '../../../shared/models/types';
import {MovieService} from '../../../core/services/movie.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  imports: [
    IonicModule,
    MovieItemComponent,
    RouterLink,
  ],
  standalone: true
})
export class HomeComponent implements OnInit {
  movieService = inject(MovieService);
  movies = signal<Pagination<Movie> | null>(null);
  moviesParams = new MoviesParams();
  router = inject(Router);

  constructor() {
    addIcons(allIcons);
  }

  ngOnInit(): void {
    this.getMovies();
  }

  getMovies() {
    this.movieService.getMovies(this.moviesParams).subscribe({
      next: data => {
        this.movies.set(data)
      },
      error: err => console.log(err)
    });
  }

  handleBooking(movieId: number) {
    console.log('Бронюємо фільм з ID:', movieId);
  }

  goToMovies() {
  }
}
