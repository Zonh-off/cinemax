import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MovieDetails} from '../../shared/models/types';

@Injectable({
  providedIn: 'root',
})
export class MovieService {
  private http = inject(HttpClient);

  getMovie(id: number) {
    return this.http.get<MovieDetails>('movies/' + id);
  }
}
