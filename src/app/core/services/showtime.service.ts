import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Pagination, Showtime, ShowtimeDay, ShowtimesParams} from '../../shared/models/types';

@Injectable({
  providedIn: 'root',
})
export class ShowtimeService {
  private http = inject(HttpClient);

  getShowtimesPaged(showtimeParams: ShowtimesParams) {
    let params = new HttpParams();

    params = params.append("pageSize", showtimeParams.pageSize)
    params = params.append("pageNumber", showtimeParams.pageNumber)

    if(showtimeParams.cityId) {
      params = params.append("cityId", showtimeParams.cityId);
    }

    if(showtimeParams.cinemaId) {
      params = params.append("cinemaId", showtimeParams.cinemaId);
    }

    return this.http.get<Pagination<Showtime>>('showtimes/paged', { params });
  }

  getShowtimes(showtimeParams: ShowtimesParams) {
    let params = new HttpParams();

    if(showtimeParams.cityId) {
      params = params.append("cityId", showtimeParams.cityId);
    }

    if(showtimeParams.cinemaId) {
      params = params.append("cinemaId", showtimeParams.cinemaId);
    }

    if(showtimeParams.from) {
      params = params.append("from", showtimeParams.from);
    }

    if(showtimeParams.to) {
      params = params.append("from", showtimeParams.to);
    }

    return this.http.get<ShowtimeDay[]>('showtimes', { params });
  }

  getShowtime(id: string) {
    return this.http.get<ShowtimeDay>('showtimes/' + id);
  }
}
