import {Component, inject, OnInit, signal} from '@angular/core';
import {CinemaService} from '../../../core/services/cinema.service';
import {ActivatedRoute} from '@angular/router';
import {Cinema, MovieDetails} from '../../../shared/models/types';
import {
  BaseModalPageLayoutComponent
} from '../../../shared/components/base-modal-page-layout/base-modal-page-layout.component';
import {StarRatingComponent} from '../../../shared/components/star-rating/star-rating.component';
import {IonicModule} from '@ionic/angular';
import {SelectableChipComponent} from '../../../shared/components/selectable-chip/selectable-chip.component';
import {MovieService} from '../../../core/services/movie.service';
import {MovieAttributesComponent} from '../../movies/movie-details/movie-attributes/movie-attributes.component';

@Component({
  selector: 'app-cinema-details',
  templateUrl: './cinema-details.component.html',
  styleUrls: ['./cinema-details.component.css'],
  imports: [
    BaseModalPageLayoutComponent,
    StarRatingComponent,
    IonicModule,
    SelectableChipComponent,
    MovieAttributesComponent
  ],
  standalone: true
})
export class CinemaDetailsComponent  implements OnInit {
  cinemaService = inject(CinemaService)
  activatedRouter = inject(ActivatedRoute);
  movieService = inject(MovieService);

  selectedDate = signal('22');
  selectedTime = signal('17:30');

  days = [
    { dayNumber: '22', label: 'Today', value: '22' },
    { dayNumber: '23', label: 'Tue', value: '23' },
    { dayNumber: '24', label: 'Wed', value: '24' },
    { dayNumber: '25', label: 'Thu', value: '25' },
    { dayNumber: '26', label: 'Fri', value: '26' },
  ];

  sessions = ['15:00', '17:30', '19:00', '21:15', '23:00'];

  cinema = signal<Cinema | null>(null)

  movie = signal<MovieDetails | null>(null);

  ngOnInit(): void {
    const id = this.activatedRouter.snapshot.paramMap.get('id');
    if(!id) return;
    this.cinemaService.getCinema(+id).subscribe({
      next: cinema => this.cinema.set(cinema),
      error: err => console.log(err)
    });

    this.movieService.getMovie(0).subscribe({
      next: movie => this.movie.set(movie),
    })
  }
}
