import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Genre, Movie, MovieDetails, MoviesParams, Pagination} from '../../shared/models/types';

@Injectable({
  providedIn: 'root',
})
export class MovieService {
  private http = inject(HttpClient);

  genres: string[] = [];

  getMovies(moviesParams: MoviesParams) {
    let params = new HttpParams();

    params = params.append("pageSize", moviesParams.pageSize)
    params = params.append("pageNumber", moviesParams.pageNumber)

    if(moviesParams.cityId) {
      params = params.append("cityId", moviesParams.cityId);
    }
    if(moviesParams.status.length > 0) {
      params = params.append("status", moviesParams.status)
    }

    return this.http.get<Pagination<Movie>>('movies', { params });
  }

  getMovie(id: number) {
    return this.http.get<MovieDetails>('movies/' + id);
  }

  getGenres() {
    if(this.genres.length > 0) return;
    return this.http.get<Genre[]>('movies/genres').subscribe({
      next: response => this.genres = response.map(v => v.name)
    })
  }
}
