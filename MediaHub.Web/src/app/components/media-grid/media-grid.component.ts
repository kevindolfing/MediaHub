import { Component, Input } from '@angular/core';
import { MediaService } from '../../services/media/media.service';
import { MediaFolder } from '../../types/media.type';

@Component({
  selector: 'app-media-grid',
  standalone: true,
  imports: [],
  templateUrl: './media-grid.component.html',
})
export class MediaGridComponent {
  constructor(private mediaService: MediaService) { }

  @Input()
  public media: MediaFolder[] = [];

  @Input()
  public changePathCallBack: (path: string) => void = () => {};

}
