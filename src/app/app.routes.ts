import { Routes } from '@angular/router';
import {TabsComponent} from './features/pages/tabs/tabs.component';
import {authGuard} from './core/guards/auth-guard';
import {authPageGuard} from './core/guards/auth-page-guard';

export const routes: Routes = [
  {
    path: 'auth',
    canActivate: [authPageGuard],
    loadComponent: () => import('./features/pages/auth/auth/auth.component').then(m => m.AuthComponent)
  },
  {
    path: 'auth/login',
    canActivate: [authPageGuard],
    loadComponent: () => import('./features/pages/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'auth/register',
    canActivate: [authPageGuard],
    loadComponent: () => import('./features/pages/auth/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'tabs',
    component: TabsComponent,
    canActivateChild: [authGuard],
    children: [
      {
        path: 'home',
        loadComponent: () => import('./features/pages/home/home.component').then(m => m.HomeComponent),
      },
      {
        path: 'explore',
        loadComponent: () => import('./features/pages/explore/explore.component').then(m => m.ExploreComponent)
      },
      {
        path: 'explore/movie-details/:id',
        loadComponent: () => import('./features/pages/explore/movie-details/movie-details.component').then(m => m.MovieDetailsComponent)
      },
      {
        path: 'cinemas',
        loadComponent: () => import('./features/pages/cinemas/cinemas.component').then(m => m.CinemasComponent),
      },
      {
        path: 'account',
        loadComponent: () => import('./features/pages/account/account.component').then(m => m.AccountComponent),
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
