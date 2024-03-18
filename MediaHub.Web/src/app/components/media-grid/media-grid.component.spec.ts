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
      providers: [{ provide: MediaService, useValue: mediaService }]
    })
      .compileComponents();

    fixture = TestBed.createComponent(MediaGridComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
