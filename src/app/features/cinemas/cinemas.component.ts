import {Component, effect, inject, OnInit, signal} from '@angular/core';
import {IonicModule, ModalController} from '@ionic/angular';
import {CityModalComponent} from '../../shared/components/city-modal/city-modal.component';
import {Cinema} from '../../shared/models/types';
import {CinemaService} from '../../core/services/cinema.service';
import {CityService} from '../../core/services/city.service';

@Component({
  selector: 'app-cinemas',
  templateUrl: './cinemas.component.html',
  styleUrls: ['./cinemas.component.css'],
  imports: [
    IonicModule
  ],
  standalone: true
})
export class CinemasComponent implements OnInit {
  cinemaService = inject(CinemaService)
  cityService = inject(CityService)
  modalCtrl = inject(ModalController);

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

  async openCityModal() {
    const modal = await this.modalCtrl.create({
      component: CityModalComponent,
      breakpoints: [0, 0.5, 0.8],
      initialBreakpoint: 0.5
    });
    await modal.present();
  }
}
