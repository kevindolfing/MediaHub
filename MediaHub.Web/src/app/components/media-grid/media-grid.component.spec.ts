import { of } from 'rxjs';
import {MediaFolder} from "../../types/media.type";
import {MediaGridComponent} from "./media-grid.component";
import {ComponentFixture, TestBed} from "@angular/core/testing";
import {MediaService} from "../../services/media/media.service";

describe('MediaGridComponent', () => {
  let component: MediaGridComponent;
  let fixture: ComponentFixture<MediaGridComponent>;
  let mediaService: MediaService;

  beforeEach(async () => {
    mediaService = jasmine.createSpyObj('MediaService', ['getMediaFolders']);

    await TestBed.configureTestingModule({
      imports: [MediaGridComponent],
      providers: [
        { provide: MediaService, useValue: mediaService }
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(MediaGridComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should populate media on init when media folders are returned', () => {
    const mediaFolders: MediaFolder[] = [{name: 'Folder 1', path: '/folder1', children: []}];
    (mediaService.getMediaFolders as jasmine.Spy).and.returnValue(of(mediaFolders));

    component.ngOnInit();
    fixture.detectChanges();

    expect(component.media).toEqual(mediaFolders);
  });

  it('should have empty media on init when no media folders are returned', () => {
    (mediaService.getMediaFolders as jasmine.Spy).and.returnValue(of([]));

    component.ngOnInit();
    fixture.detectChanges();

    expect(component.media).toEqual([]);
  });
});
