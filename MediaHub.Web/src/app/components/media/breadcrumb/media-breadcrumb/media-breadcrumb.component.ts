import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-media-breadcrumb',
  standalone: true,
  imports: [],
  templateUrl: './media-breadcrumb.component.html',
})
export class MediaBreadcrumbComponent {
  @Input() currentPath: string = "";
  @Input() public changePathCallBack: (path: string) => void = () => {
  };

  pathParts: { title: string, path: string }[] = [];

  constructor() {
  }

  ngOnChanges() {
    this.pathParts = this.currentPath.split('/').filter(p => p != "").map((part, index, parts) => {
      return {
        title: part,
        path: parts.slice(0, index + 1).join('/')
      };
    });
  }
}
