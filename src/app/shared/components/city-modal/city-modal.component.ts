import {Component, inject} from '@angular/core';
import {IonicModule, ModalController} from '@ionic/angular';
import {addIcons} from 'ionicons';
import { checkmarkCircle } from 'ionicons/icons';
import {CityService} from '../../../core/services/city.service';
import {City} from '../../models/types';

@Component({
  selector: 'app-city-modal',
  templateUrl: './city-modal.component.html',
  styleUrls: ['./city-modal.component.css'],
  imports: [
    IonicModule
  ],
  standalone: true
})
export class CityModalComponent {
  cityService = inject(CityService);
  private modalCtrl = inject(ModalController);

  constructor() {
    addIcons({ checkmarkCircle });
  }

  selectCity(city: City) {
    this.cityService.selectCity(city);
    this.modalCtrl.dismiss();
  }

  close() {
    this.modalCtrl.dismiss();
  }
}
