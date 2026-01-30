import {Component, inject, OnInit, signal} from '@angular/core';
import {MovieService} from '../../../core/services/movie.service';
import {catchError, debounceTime, EMPTY, of, Subject, switchMap, tap} from 'rxjs';
import {Movie, MoviesParams} from '../../../shared/models/types';
import {InfiniteScrollCustomEvent, IonicModule, ModalController} from '@ionic/angular';
import {MovieItemComponent} from '../../../shared/components/movie-item/movie-item.component';
import {RouterLink} from '@angular/router';
import {GenreFilterComponent} from '../home/movies/genre-filter/genre-filter.component';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  imports: [
    IonicModule,
    MovieItemComponent,
    RouterLink
  ],
  standalone: true
})
export class SearchComponent  implements OnInit {
  private movieService = inject(MovieService);
  private filterTrigger$ = new Subject<void>();
  private modalCtrl = inject(ModalController);
  private isModalLoading = false;
  private moviesParams = new MoviesParams();

  movies = signal<Movie[]>([]);
  isLoading = signal(false);
  selectedGenres = signal<string[]>([]);
  hasMorePages = true;

  ngOnInit() {
    this.movieService.getGenres();

    this.filterTrigger$.pipe(
      debounceTime(400),
      switchMap(() => {
        const hasSearch = this.moviesParams.search && this.moviesParams.search.trim().length > 0;
        const hasGenres = this.selectedGenres().length > 0;

        if (!hasSearch && !hasGenres) {
          this.isLoading.set(false);
          this.movies.set([]);
          this.hasMorePages = false;
          return of({ data: [], totalItems: 0, pageNumber: 1, pageSize: 10 }); // Повертаємо пустий результат
        }

        this.isLoading.set(true);
        this.moviesParams.genres = this.selectedGenres().join(',');
        return this.getMovies();
      })
    ).subscribe();
  }

  loadMore($event: InfiniteScrollCustomEvent) {
    if (this.hasMorePages) {
      this.moviesParams.pageNumber++;
      this.getMovies($event).subscribe();
    } else {
      $event.target.disabled = true;
    }
  }

  onSearchChange(event: any) {
    this.moviesParams.search = event.detail.value;
    this.moviesParams.pageNumber = 1;
    this.filterTrigger$.next();
  }

  async openFilters() {
    if (this.isModalLoading) return;
    this.isModalLoading = true;

    try {
      const modal = await this.modalCtrl.create({
        component: GenreFilterComponent,
        initialBreakpoint: 0.5,
        breakpoints: [0, 0.5, 0.8],
        handle: true,
        componentProps: {
          allGenres: this.movieService.genres,
          initialSelected: this.selectedGenres()
        }
      });

      await modal.present();

      this.isModalLoading = false;

      const { data } = await modal.onWillDismiss();

      if (data) {
        this.selectedGenres.set(data);
        this.applyFilters();
      }
    } catch (error) {
      this.isModalLoading = false;
    }
  }

  private applyFilters() {
    this.moviesParams.pageNumber = 1;
    this.filterTrigger$.next();
  }

  private getMovies($event?: InfiniteScrollCustomEvent) {
    return this.movieService.getMovies(this.moviesParams).pipe(
      tap(data => {
        if (this.moviesParams.pageNumber === 1) {
          this.movies.set(data.data);
          this.hasMorePages = true;
        } else {
          this.movies.update(prev => [...prev, ...data.data]);
        }

        const totalPages = Math.ceil(data.totalItems / data.pageSize);
        if (data.pageNumber >= totalPages || data.data.length === 0) {
          this.hasMorePages = false;
        }

        if ($event) $event.target.complete();
        this.isLoading.set(false);
      }),
      catchError(err => {
        console.error(err);
        if ($event) $event.target.complete();
        this.isLoading.set(false);
        return EMPTY;
      })
    );
  }
}
