import {Component, effect, inject, signal} from '@angular/core';
import {IonicModule} from '@ionic/angular';
import {Router} from '@angular/router';
import {Pagination, Showtime, ShowtimesParams} from '../../shared/models/types';
import {CityService} from '../../core/services/city.service';
import {MoviesCarouselComponent} from './movies-carousel/movies-carousel.component';
import {HomeHeaderComponent} from './home-header/home-header.component';
import {ShowtimeService} from '../../core/services/showtime.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  imports: [
    IonicModule,
    MoviesCarouselComponent,
    HomeHeaderComponent,
  ],
  standalone: true
})
export class HomeComponent {
  showtimeService = inject(ShowtimeService);
  cityService = inject(CityService);

  nowMovies = signal<Showtime[]>([]);
  upcomingMovies = signal<Showtime[]>([]);

  nowMoviesParams = signal(new ShowtimesParams());
  upcomingMoviesParams = signal(new ShowtimesParams());

  router = inject(Router);

  constructor() {
    effect(() => {
      const city = this.cityService.selectedCity();
      if (city) {
        this.nowMoviesParams.update(prev => ({ ...prev, cityId: city.id }));
        this.upcomingMoviesParams.update(prev => ({ ...prev, cityId: city.id }));
        this.getNowPlayingMovies();
        this.getUpcomingMovies()
      }
    });
  }

  getNowPlayingMovies() {
    this.showtimeService.getShowtimesPaged(this.nowMoviesParams()).subscribe({
      next: (data: Pagination<Showtime>) => {
        this.nowMovies.set(data.data)
      },
      error: err => console.log(err)
    });
  }

  getUpcomingMovies() {
    console.log(this.upcomingMoviesParams());
    this.showtimeService.getShowtimesPaged(this.upcomingMoviesParams()).subscribe({
      next: data => {
        console.log(data.data)
        this.upcomingMovies.set(data.data)
      },
      error: err => console.log(err)
    });
  }
}
