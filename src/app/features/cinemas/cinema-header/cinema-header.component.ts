import {Component, inject} from '@angular/core';
import {IonicModule, ModalController} from '@ionic/angular';
import {CityModalComponent} from '../../../shared/components/city-modal/city-modal.component';
import {CityService} from '../../../core/services/city.service';

@Component({
  selector: 'app-cinema-header',
  templateUrl: './cinema-header.component.html',
  styleUrls: ['./cinema-header.component.css'],
  imports: [
    IonicModule
  ],
  standalone: true
})
export class CinemaHeaderComponent {
  modalCtrl = inject(ModalController);
  cityService = inject(CityService)

  async openCityModal() {
    const modal = await this.modalCtrl.create({
      component: CityModalComponent,
      breakpoints: [0, 0.5, 0.8],
      initialBreakpoint: 0.5
    });
    await modal.present();
  }
}
