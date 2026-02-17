import {ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners} from '@angular/core';
import {provideRouter, withComponentInputBinding} from '@angular/router';

import { routes } from './app.routes';
import { provideIonicAngular } from '@ionic/angular/standalone';
import {apiInterceptor} from './core/interceptors/api-interceptor';
import {provideHttpClient, withInterceptors } from '@angular/common/http';
import {authInterceptor} from './core/interceptors/auth-interceptor';
import {InitService} from './core/services/init.service';
import {lastValueFrom} from 'rxjs';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withComponentInputBinding()), provideIonicAngular({}),
    provideHttpClient(withInterceptors([apiInterceptor, authInterceptor])),
    provideAppInitializer(async () => {
      const initService = inject(InitService)
      return lastValueFrom(initService.init());
    }),
  ]
};
