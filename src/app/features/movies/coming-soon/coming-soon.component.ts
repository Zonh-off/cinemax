import {Component, inject, OnInit, signal} from '@angular/core';
import {MovieService} from '../../../core/services/movie.service';
import {Movie, MoviesParams} from '../../../shared/models/types';
import {InfiniteScrollCustomEvent, IonicModule} from '@ionic/angular';
import {MovieItemComponent} from '../../../shared/components/movie-item/movie-item.component';
import {RouterLink} from '@angular/router';
import {CityService} from '../../../core/services/city.service';

@Component({
  selector: 'app-now-playing',
  templateUrl: './coming-soon.component.html',
  styleUrls: ['./coming-soon.component.css'],
  imports: [
    IonicModule,
    MovieItemComponent,
    RouterLink
  ],
  standalone: true
})
export class ComingSoonComponent implements OnInit {
  movieService = inject(MovieService);
  cityService = inject(CityService);
  movies = signal<Movie[]>([]);
  moviesParams = new MoviesParams();
  hasMorePages = true;

  ngOnInit(): void {
    this.moviesParams.status = "coming-soon";
    this.moviesParams.cityId = this.cityService.selectedCity()?.id;
    this.getMovies();
  }

  getMovies($event?: InfiniteScrollCustomEvent) {
    this.movieService.getMovies(this.moviesParams).subscribe({
      next: data => {
        this.movies.update(prev => [...prev, ...data.data]);

        const totalPages = Math.ceil(data.totalItems / data.pageSize);
        if (data.pageNumber >= totalPages || data.data.length === 0) {
          this.hasMorePages = false;
        }

        if ($event) {
          $event.target.complete();
        }
      },
      error: err => {
        console.log(err);
        if ($event) $event.target.complete();
      }
    });
  }

  loadMore($event: InfiniteScrollCustomEvent) {
    if (this.hasMorePages) {
      this.moviesParams.pageNumber++;
      this.getMovies($event);
    } else {
      $event.target.disabled = true;
    }
  }
}
