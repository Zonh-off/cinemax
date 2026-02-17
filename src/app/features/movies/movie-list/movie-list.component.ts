import {Component, input, output} from '@angular/core';
import {Showtime} from '../../../shared/models/types';
import {InfiniteScrollCustomEvent, IonicModule} from '@ionic/angular';
import {MovieItemComponent} from '../../../shared/components/movie-item/movie-item.component';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-movie-list',
  templateUrl: './movie-list.component.html',
  styleUrls: ['./movie-list.component.css'],
  imports: [
    MovieItemComponent,
    RouterLink,
    IonicModule
  ],
  standalone: true
})
export class MovieListComponent {
  movies = input.required<Showtime[]>();

  loadMore = output<InfiniteScrollCustomEvent>();
}
