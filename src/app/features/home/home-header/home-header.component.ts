import {Component, inject} from '@angular/core';
import {CityModalComponent} from '../../../shared/components/city-modal/city-modal.component';
import {IonicModule, ModalController} from '@ionic/angular';
import {CityService} from '../../../core/services/city.service';

@Component({
  selector: 'app-home-header',
  templateUrl: './home-header.component.html',
  styleUrls: ['./home-header.component.css'],
  imports: [
    IonicModule
  ],
  standalone: true
})
export class HomeHeaderComponent {
  cityService = inject(CityService);
  modalCtrl = inject(ModalController);

  async openCityModal() {
    const modal = await this.modalCtrl.create({
      component: CityModalComponent,
      breakpoints: [0, 0.5, 0.8],
      initialBreakpoint: 0.5
    });
    await modal.present();
  }
}
