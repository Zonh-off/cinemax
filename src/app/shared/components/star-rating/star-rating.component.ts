import {Component, computed, input} from '@angular/core';
import {IonicModule} from '@ionic/angular';
import {addIcons} from 'ionicons';
import { star, starHalf, starOutline } from 'ionicons/icons';

@Component({
  selector: 'app-star-rating',
  templateUrl: './star-rating.component.html',
  styleUrls: ['./star-rating.component.css'],
  imports: [
    IonicModule
  ],
  standalone: true
})
export class StarRatingComponent {
  constructor() {
    addIcons({ star, starHalf, starOutline });
  }

  rating = input.required<number>();
  stars = computed(() => {
    const scaledRating = this.rating() / 2;
    const starsArray = [];

    for (let i = 1; i <= 5; i++) {
      if (i <= scaledRating) {
        starsArray.push('star');
      } else if (i - 0.5 <= scaledRating) {
        starsArray.push('star-half');
      } else {
        starsArray.push('star-outline');
      }
    }
    return starsArray;
  });
}
