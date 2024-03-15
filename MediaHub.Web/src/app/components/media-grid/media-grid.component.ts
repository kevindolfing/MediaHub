import { Component } from '@angular/core';
import { MediaService } from '../../services/media/media.service';
import { MediaFolder } from '../../types/media.type';

@Component({
  selector: 'app-media-grid',
  standalone: true,
  imports: [],
  templateUrl: './media-grid.component.html',
  styleUrl: './media-grid.component.scss'
})
export class MediaGridComponent {
  constructor(private mediaService: MediaService) { }

  public media: MediaFolder[] = [];

  ngOnInit() {
    this.mediaService.getMediaFolders().subscribe(folders => this.media = folders);
  }
}
