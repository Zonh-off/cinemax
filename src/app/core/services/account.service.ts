import {inject, Injectable, signal} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {map} from 'rxjs';
import {User} from '../../shared/models/types';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);

  currentUser = signal<User | null>(null);

  preRegister(data: any) {
    return this.http.post(`account/pre-register`, data);
  }

  verifyEmail(email: string, code: string) {
    return this.http.post(`account/verify-email?email=${email}&code=${code}`, {});
  }

  login(values: any) {
    let params = new HttpParams();
    params = params.append('useCookies', true);
    return this.http.post(`account/login`, values, { params });
  }

  logout() {
    return this.http.post(`account/logout`, {});
  }

  getAuthStatus() {
    return this.http.get<{isAuthenticated: boolean}>('account/auth-status')
  }

  getUserInfo() {
    return this.http.get<User>('account/user-info').pipe(
      map(user => {
        this.currentUser.set(user);
        return user;
      })
    );
  }

  setCurrentUser(user: User) {
    this.currentUser.set(user);
  }
}
