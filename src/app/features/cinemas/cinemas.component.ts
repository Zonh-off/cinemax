import {Component, effect, inject, OnInit, signal} from '@angular/core';
import {IonicModule} from '@ionic/angular';
import {Cinema} from '../../shared/models/types';
import {CinemaService} from '../../core/services/cinema.service';
import {CityService} from '../../core/services/city.service';
import {CinemaHeaderComponent} from './cinema-header/cinema-header.component';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-cinemas',
  templateUrl: './cinemas.component.html',
  styleUrls: ['./cinemas.component.css'],
  imports: [
    IonicModule,
    CinemaHeaderComponent,
    RouterLink
  ],
  standalone: true
})
export class CinemasComponent implements OnInit {
  cinemaService = inject(CinemaService)
  cityService = inject(CityService)

  cinemas = signal<Cinema[]>([])

  constructor() {
    effect(() => {
      const cityId = this.cityService.selectedCity()?.id;
      this.cinemaService.getCinemas(cityId).subscribe({
        next: data => this.cinemas.set(data)
      })
    });
  }

  ngOnInit() {
    const cityId = this.cityService.selectedCity()?.id;
    this.cinemaService.getCinemas(cityId).subscribe({
      next: data => this.cinemas.set(data)
    })
  }
}
