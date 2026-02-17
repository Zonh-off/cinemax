import {Component, effect, input, OnInit} from '@angular/core';
import {BaseMovieList} from '../../../shared/components/base-movie-list/base-movie-list.component';
import {MovieListComponent} from '../movie-list/movie-list.component';
import {
  BaseModalPageLayoutComponent
} from '../../../shared/components/base-modal-page-layout/base-modal-page-layout.component';

@Component({
  selector: 'app-movie-list-modal',
  templateUrl: './movie-list-modal.component.html',
  styleUrls: ['./movie-list-modal.component.css'],
  imports: [
    MovieListComponent,
    BaseModalPageLayoutComponent
  ],
  standalone: true
})
export class MovieListModalComponent extends BaseMovieList implements OnInit {
  title = input.required<string>();
  status = input.required<string>();

  constructor() {
    super();
    effect(() => {
      const currentStatus = this.status();
      if (currentStatus) {
        this.refreshMovies();
      }
    });
  }

  ngOnInit(): void {}

  initParams(): void {
    this.showtimesParams.cityId = this.cityService.selectedCity()?.id;
  }
}
