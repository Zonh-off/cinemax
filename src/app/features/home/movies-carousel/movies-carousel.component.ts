import {Component, input} from '@angular/core';
import {Showtime} from '../../../shared/models/types';
import {IonicModule} from '@ionic/angular';
import {MovieItemComponent} from '../../../shared/components/movie-item/movie-item.component';
import {RouterLink} from '@angular/router';
import {HorizontalSectionComponent} from '../../../shared/components/horizontal-section/horizontal-section.component';

@Component({
  selector: 'app-movies-carousel',
  templateUrl: './movies-carousel.component.html',
  styleUrls: ['./movies-carousel.component.css'],
  imports: [
    IonicModule,
    MovieItemComponent,
    RouterLink,
    HorizontalSectionComponent
  ],
  standalone: true
})
export class MoviesCarouselComponent {
  title = input.required<string>();
  routerUrl = input.required<string>();
  movies = input.required<Showtime[]>()
}
