import {ApplicationConfig} from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import {provideHttpClient} from "@angular/common/http";
import {provideAuth0} from "@auth0/auth0-angular";
import {environment} from "../environments/environment";

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), provideHttpClient(),
    provideAuth0({
      domain: environment.OAUTH_DOMAIN,
      clientId: environment.OAUTH_CLIENT_ID,
      authorizationParams: {
        redirect_uri: environment.OAUTH_CALLBACK_URL,
      }
    })]
};
