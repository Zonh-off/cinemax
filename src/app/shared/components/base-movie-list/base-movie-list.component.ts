import {Component, effect, inject, signal} from '@angular/core';
import {CityService} from '../../../core/services/city.service';
import {Showtime, ShowtimesParams} from '../../models/types';
import {InfiniteScrollCustomEvent} from '@ionic/angular';
import {ShowtimeService} from '../../../core/services/showtime.service';

@Component({
  selector: 'app-base-movie-list',
  templateUrl: './base-movie-list.component.html',
  styleUrls: ['./base-movie-list.component.css'],
})
export abstract class BaseMovieList {
  protected showtimeService = inject(ShowtimeService);
  protected cityService = inject(CityService);

  showtimes = signal<Showtime[]>([]);
  showtimesParams = new ShowtimesParams();
  hasMorePages = true;

  constructor() {
    effect(() => {
      const city = this.cityService.selectedCity();
      if (city) {
        this.showtimes.set([]);
        this.showtimesParams.pageNumber = 1;
        this.showtimesParams.cityId = city.id;
        this.getMovies();
      }
    });
  }

  abstract initParams(): void;

  protected getMovies($event?: InfiniteScrollCustomEvent) {
    this.showtimeService.getShowtimesPaged(this.showtimesParams).subscribe({
      next: data => {
        this.showtimes.update(prev => [...prev, ...data.data]);

        const totalPages = Math.ceil(data.totalItems / data.pageSize);
        this.hasMorePages = data.pageNumber < totalPages;

        $event?.target.complete();
      },
      error: () => $event?.target.complete()
    });
  }

  protected refreshMovies() {
    this.showtimes.set([]);
    this.showtimesParams.pageNumber = 1;
    this.initParams();
    this.getMovies();
  }

  loadMore($event: InfiniteScrollCustomEvent) {
    if (this.hasMorePages) {
      this.showtimesParams.pageNumber++;
      this.getMovies($event);
    } else {
      $event.target.disabled = true;
    }
  }
}
