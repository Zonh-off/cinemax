import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import {map} from 'rxjs';

export const authPageGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  if (accountService.currentUser()) {
    router.navigateByUrl('/tabs/home');
    return false;
  } else {
    return accountService.getAuthStatus().pipe(
      map(res => {
        if (res.isAuthenticated) {
          router.navigateByUrl('/tabs/home');
          return false;
        }
        return true;
      }))
  }
};
