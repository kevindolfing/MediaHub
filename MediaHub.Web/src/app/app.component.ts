import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MediaGridComponent } from './components/media-grid/media-grid.component';
import {MediaComponent} from "./components/media/media/media.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MediaComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'MediaHub.Web';
}
