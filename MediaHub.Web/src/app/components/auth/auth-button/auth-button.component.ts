import { Component } from '@angular/core';
import {AuthService} from "@auth0/auth0-angular";

@Component({
  selector: 'app-auth-button',
  standalone: true,
  imports: [],
  templateUrl: './auth-button.component.html'
})
export class AuthButtonComponent {
  constructor(public auth: AuthService) {
  }
}
