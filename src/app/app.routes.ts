import { Routes } from '@angular/router';
import {TabsComponent} from './features/tabs/tabs.component';
import {authGuard} from './core/guards/auth-guard';
import {authPageGuard} from './core/guards/auth-page-guard';

export const routes: Routes = [
  {
    path: 'auth',
    canActivate: [authPageGuard],
    loadComponent: () => import('./features/auth/auth/auth.component').then(m => m.AuthComponent)
  },
  {
    path: 'auth/login',
    canActivate: [authPageGuard],
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'auth/register',
    canActivate: [authPageGuard],
    loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'movies/now-playing',
    loadComponent: () => import('./features/movies/now-playing/now-playing.component').then(m => m.NowPlayingComponent),
  },
  {
    path: 'movies/coming-soon',
    loadComponent: () => import('./features/movies/coming-soon/coming-soon.component').then(m => m.ComingSoonComponent),
  },
  {
    path: 'movies/movie-details/:id',
    loadComponent: () => import('./features/movies/movie-details/movie-details.component').then(m => m.MovieDetailsComponent)
  },
  {
    path: 'tabs',
    component: TabsComponent,
    canActivateChild: [authGuard],
    children: [
      {
        path: 'home',
        loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent),
      },
      {
        path: 'cinemas',
        loadComponent: () => import('./features/cinemas/cinemas.component').then(m => m.CinemasComponent),
      },
      {
        path: 'account',
        loadComponent: () => import('./features/account/account.component').then(m => m.AccountComponent),
      },
      {
        path: '',
        redirectTo: '/tabs/home',
        pathMatch: 'full'
      }
    ]
  },
  {
    path: '',
    redirectTo: '/tabs/home',
    pathMatch: 'full'
  }
];
