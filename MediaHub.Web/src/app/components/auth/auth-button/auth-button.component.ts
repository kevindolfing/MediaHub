import {Component} from '@angular/core';
import {AuthService} from "@auth0/auth0-angular";
import {AsyncPipe} from "@angular/common";

@Component({
  selector: 'app-auth-button',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './auth-button.component.html'
})
export class AuthButtonComponent {
  constructor(public auth: AuthService) {
  }
}
