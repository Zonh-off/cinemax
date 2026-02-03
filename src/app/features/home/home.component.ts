import {Component, effect, inject, OnInit, signal} from '@angular/core';
import {IonicModule, ModalController} from '@ionic/angular';
import {MovieItemComponent} from '../../shared/components/movie-item/movie-item.component';
import * as allIcons from 'ionicons/icons';
import { addIcons } from 'ionicons';
import {Router, RouterLink} from '@angular/router';
import {Movie, MoviesParams, Pagination} from '../../shared/models/types';
import {MovieService} from '../../core/services/movie.service';
import {CityService} from '../../core/services/city.service';
import {CityModalComponent} from '../../shared/components/city-modal/city-modal.component';

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
  cityService = inject(CityService);
  movies = signal<Pagination<Movie> | null>(null);
  moviesParams = signal(new MoviesParams());
  router = inject(Router);
  modalCtrl = inject(ModalController);

  constructor() {
    addIcons(allIcons);

    effect(() => {
      const city = this.cityService.selectedCity();
      if (city) {
        this.moviesParams.update(prev => ({ ...prev, cityId: city.id }));
        this.getMovies();
      }
    });
  }

  ngOnInit(): void {
    this.cityService.getCities().subscribe({
      next: () => this.cityService.getSavedCity()
    });
  }

  async openCityModal() {
    const modal = await this.modalCtrl.create({
      component: CityModalComponent,
      breakpoints: [0, 0.5, 0.8],
      initialBreakpoint: 0.5
    });
    await modal.present();
  }

  getMovies() {
    this.movieService.getMovies(this.moviesParams()).subscribe({
      next: data => {
        this.movies.set(data)
      },
      error: err => console.log(err)
    });
  }
}
