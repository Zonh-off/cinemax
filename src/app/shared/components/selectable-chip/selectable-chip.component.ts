import {Component, input, output} from '@angular/core';
import {IonicModule} from '@ionic/angular';

@Component({
  selector: 'app-selectable-chip',
  templateUrl: './selectable-chip.component.html',
  styleUrls: ['./selectable-chip.component.css'],
  imports: [
    IonicModule
  ],
  standalone: true
})
export class SelectableChipComponent {
  isSelected = input<boolean>(false);
  select = output<void>();
}
