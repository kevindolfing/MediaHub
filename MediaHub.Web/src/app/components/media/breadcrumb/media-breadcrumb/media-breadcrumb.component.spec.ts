import {ComponentFixture, TestBed} from "@angular/core/testing";
import { MediaBreadcrumbComponent } from "./media-breadcrumb.component";

describe('MediaBreadcrumbComponent', () => {
  let component: MediaBreadcrumbComponent;
  let fixture: ComponentFixture<MediaBreadcrumbComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MediaBreadcrumbComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(MediaBreadcrumbComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should split currentPath into pathParts on ngOnChanges', () => {
    component.currentPath = 'path/to/media';
    component.ngOnChanges();
    expect(component.pathParts.length).toBe(3);
    expect(component.pathParts[0].title).toBe('path');
    expect(component.pathParts[0].path).toBe('path');
    expect(component.pathParts[1].title).toBe('to');
    expect(component.pathParts[1].path).toBe('path/to');
    expect(component.pathParts[2].title).toBe('media');
    expect(component.pathParts[2].path).toBe('path/to/media');
  });

  it('should handle empty currentPath on ngOnChanges', () => {
    component.currentPath = '';
    component.ngOnChanges();
    expect(component.pathParts.length).toBe(0);
  });

  it('should handle currentPath with only slashes on ngOnChanges', () => {
    component.currentPath = '////';
    component.ngOnChanges();
    expect(component.pathParts.length).toBe(0);
  });
});
