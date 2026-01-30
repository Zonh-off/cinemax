import { Component } from '@angular/core';
import { addIcons } from 'ionicons';
import * as allIcons from 'ionicons/icons';
import {IonicModule} from '@ionic/angular';

@Component({
  selector: 'app-tabs',
  templateUrl: './tabs.component.html',
  styleUrls: ['./tabs.component.css'],
  imports: [
    IonicModule
  ],
  standalone: true
})
export class TabsComponent{
  constructor() {
    addIcons(allIcons);
  }
}
