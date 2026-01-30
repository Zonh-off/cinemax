import { Routes } from '@angular/router';
import {TabsComponent} from './features/pages/tabs/tabs.component';

export const routes: Routes = [
  {
    path: 'tabs',
    component: TabsComponent,
    children: [
      {
        path: 'home',
        loadComponent: () => import('./features/pages/home/home.component').then(m => m.HomeComponent),
      },
      {
        path: 'home/movies',
        loadComponent: () => import('./features/pages/home/movies/movies.component').then(m => m.MoviesComponent),
      },
      {
        path: 'home/movie-details/:id',
        loadComponent: () => import('./features/pages/home/movie-details/movie-details.component').then(m => m.MovieDetailsComponent)
      },
      {
        path: 'search', loadComponent: () => import('./features/pages/search/search.component').then(m => m.SearchComponent)
      },
      {
        path: 'cinemas',
        loadComponent: () => import('./features/pages/cinemas/cinemas.component').then(m => m.CinemasComponent),
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
