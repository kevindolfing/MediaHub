import {Component, OnInit} from '@angular/core';
import {MediaGridComponent} from "../../media-grid/media-grid.component";
import {MediaFolder} from "../../../types/media.type";
import {MediaService} from "../../../services/media/media.service";
import {MediaBreadcrumbComponent} from "../breadcrumb/media-breadcrumb/media-breadcrumb.component";

@Component({
  selector: 'app-media',
  standalone: true,
  imports: [MediaGridComponent, MediaBreadcrumbComponent],
  templateUrl: './media.component.html',
})
export class MediaComponent implements OnInit{
  currentPath: string = "";

  media: MediaFolder[] = [];

  changedPathCallback = (path: string) => {
    this.pathChanged(path);
  }

  constructor(private mediaService: MediaService) {}

  ngOnInit(): void {
    this.mediaService.getMediaFolders().subscribe(folders => this.media = folders);
  }


  pathChanged(path: string) {
    this.currentPath = path;
    this.mediaService.getMediaFolders(path).subscribe(folders => this.media = folders);
  }
}
