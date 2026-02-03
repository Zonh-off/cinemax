import {inject, Injectable, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {City} from '../../shared/models/types';
import {of, tap} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CityService {
  private http = inject(HttpClient);

  selectedCity = signal<City | null>(null);
  cities: City[] = []

  getCities() {
    if (this.cities.length > 0) {
      return of(this.cities);
    }

    return this.http.get<City[]>('cities').pipe(
      tap(response => this.cities = response)
    );
  }

  getSavedCity() {
    const savedCityId=  localStorage.getItem('selectedCityId');

    if (savedCityId) {
      const city=this.cities.find(c => c.id === +savedCityId);
      if (city) {
        this.selectedCity.set(city);
        return;
      } else {
        if (this.cities.length > 0) {
          this.selectedCity.set(this.cities[0]);
          return;
        }
      }
    }
  }

  selectCity(city: City) {
    this.selectedCity.set(city);
    localStorage.setItem('selectedCityId', city.id.toString());
  }

  getDistance(lat1: number, lon1: number, lat2: number, lon2: number) {
    const R = 6371;
    const dLat = (lat2 - lat1) * Math.PI / 180;
    const dLon = (lon2 - lon1) * Math.PI / 180;
    const a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(lat1 * Math.PI / 180) * Math.cos(lat2 * Math.PI / 180) *
      Math.sin(dLon / 2) * Math.sin(dLon / 2);
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    return R * c;
  }
}
