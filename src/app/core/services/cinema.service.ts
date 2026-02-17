import {inject, Injectable, signal} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Cinema} from '../../shared/models/types';

@Injectable({
  providedIn: 'root',
})
export class CinemaService {
  private http = inject(HttpClient);

  getCinemas(cityId?: number) {
    let params = new HttpParams();

    if(cityId) {
      params = params.append("cityId", cityId);
    }

    return this.http.get<Cinema[]>('cinemas', { params });
  }

  getCinema(cinemaId?: number) {
    return this.http.get<Cinema>('cinemas?=' + cinemaId);
  }
}
