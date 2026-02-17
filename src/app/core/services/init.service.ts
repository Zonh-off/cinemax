import {inject, Injectable} from '@angular/core';
import {forkJoin, of, switchMap, tap} from 'rxjs';
import {AccountService} from './account.service';
import {CityService} from './city.service';

@Injectable({
  providedIn: 'root',
})
export class InitService {
  private accountService = inject(AccountService);
  private cityService = inject(CityService);

  init() {
    return forkJoin({
      user: this.accountService.getAuthStatus().pipe(
        tap(auth => {
          if (auth.isAuthenticated) {
            return this.accountService.getUserInfo();
          }
          return of(null);
        })),
      city: this.cityService.getCities().pipe(
        tap(() => this.cityService.getSavedCity())
      )
    });
  }
}
