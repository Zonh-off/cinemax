import {inject, Injectable} from '@angular/core';
import {of, switchMap} from 'rxjs';
import {AccountService} from './account.service';

@Injectable({
  providedIn: 'root',
})
export class InitService {
  private accountService = inject(AccountService);

  init() {
    return this.accountService.getAuthStatus().pipe(
      switchMap(auth => {
        if (auth.isAuthenticated) {
          return this.accountService.getUserInfo();
        }
        return of(null);
      })
    );
  }
}
