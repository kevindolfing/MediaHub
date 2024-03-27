import {Component, Input} from '@angular/core';
import {MediaService} from '../../services/media/media.service';
import {Media} from '../../types/media.type';
import {environment} from "../../../environments/environment";
import {HttpUrlEncodingCodec} from "@angular/common/http";

@Component({
  selector: 'app-media-grid',
  standalone: true,
  imports: [],
  templateUrl: './media-grid.component.html',
})
export class MediaGridComponent {
  constructor(private mediaService: MediaService) {
  }

  @Input()
  public media: Media[] = [];

  @Input()
  public changePathCallBack: (path: string) => void = () => {
  };

  public handleClick(item: Media) {
    if (item.type === 0) {
      this.changePathCallBack(item.path);
    } else {
      window.open(environment.BACKEND_URL + '/media/file?path=' + new HttpUrlEncodingCodec().encodeValue(item.path), '_blank');

    }
  }

}
