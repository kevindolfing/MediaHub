import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MediaGridComponent } from './components/media-grid/media-grid.component';
import {MediaComponent} from "./components/media/media/media.component";
import {HeaderComponent} from "./components/header/header/header.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MediaComponent, HeaderComponent],
  templateUrl: './app.component.html',
})
export class AppComponent {
  title = 'MediaHub.Web';
}
