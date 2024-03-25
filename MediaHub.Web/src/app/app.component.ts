import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MediaComponent} from "./components/media/media/media.component";
import {HeaderComponent} from "./components/header/header/header.component";
import {AuthService} from "@auth0/auth0-angular";
import {AsyncPipe} from "@angular/common";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MediaComponent, HeaderComponent, AsyncPipe],
  templateUrl: './app.component.html',
})
export class AppComponent {
  constructor(public authService: AuthService) {
  }
}
