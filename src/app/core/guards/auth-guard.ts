import {CanActivateFn, Router} from '@angular/router';
import {AccountService} from '../services/account.service';
import {inject} from '@angular/core';
import {map, of} from 'rxjs';

export const authGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  if(accountService.currentUser()) {
    return of(true);
  } else {
    return accountService.getAuthStatus().pipe(
      map(auth => {
        if(auth.isAuthenticated) {
          return true;
        } else {
          router.navigateByUrl('/auth');
          return false;
        }
      })
    );
  }
};
