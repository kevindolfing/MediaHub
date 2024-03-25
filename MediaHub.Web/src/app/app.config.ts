import {ApplicationConfig} from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import {provideHttpClient, withInterceptors} from "@angular/common/http";
import {authHttpInterceptorFn, provideAuth0} from "@auth0/auth0-angular";
import {environment} from "../environments/environment";

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), provideHttpClient(withInterceptors([authHttpInterceptorFn])),
    provideAuth0({
      domain: environment.OAUTH_DOMAIN,
      clientId: environment.OAUTH_CLIENT_ID,
      authorizationParams: {
        redirect_uri: environment.OAUTH_CALLBACK_URL,
        audience: environment.OAUTH_AUDIENCE,
      },
      httpInterceptor: {
        allowedList: [
          {
            uri: `${environment.BACKEND_URL}/*`,
            tokenOptions: {
              authorizationParams: {
                audience: environment.OAUTH_AUDIENCE,
              }
            },
          },
        ],
      },
    })],
};
