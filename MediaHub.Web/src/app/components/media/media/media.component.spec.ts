import {ComponentFixture, TestBed} from '@angular/core/testing';
import {MediaComponent} from './media.component';
import {MediaService} from '../../../services/media/media.service';
import {Observable} from "rxjs";

describe('MediaComponent', () => {
  let component: MediaComponent;
  let fixture: ComponentFixture<MediaComponent>;

  beforeEach(async () => {
    const mediaServiceSpy = jasmine.createSpyObj('MediaService', ['getMediaFolders']);
    mediaServiceSpy.getMediaFolders.and.returnValue(new Observable());
    await TestBed.configureTestingModule({
      imports: [MediaComponent],
      providers: [{provide: MediaService, useValue: mediaServiceSpy}]
    })
      .compileComponents();

    fixture = TestBed.createComponent(MediaComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call getMediaFolders on init', () => {
    component.ngOnInit();
    expect((component as any).mediaService.getMediaFolders).toHaveBeenCalled();
  });

  it('should set currentPath on pathChanged', () => {
    const path = 'test';
    component.pathChanged(path);
    expect(component.currentPath).toEqual(path);
  });

  it('should call getMediaFolders on pathChanged', () => {
    const path = 'test';
    component.pathChanged(path);
    expect((component as any).mediaService.getMediaFolders).toHaveBeenCalledWith(path);
  });
});
