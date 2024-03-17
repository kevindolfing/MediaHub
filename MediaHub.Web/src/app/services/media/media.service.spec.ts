import {TestBed} from '@angular/core/testing';
import {HttpClientTestingModule, HttpTestingController} from '@angular/common/http/testing';

import {MediaService} from './media.service';
import {MediaFolder} from '../../types/media.type';

describe('MediaService', () => {
  let service: MediaService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [MediaService]
    });

    service = TestBed.inject(MediaService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should retrieve media folders successfully', () => {
    const mockFolders: MediaFolder[] = [
      {name: 'Folder 1', path: '/folder1', children: []},
      {name: 'Folder 2', path: '/folder2', children: []}
    ];

    service.getMediaFolders().subscribe(folders => {
      expect(folders.length).toBe(2);
      expect(folders).toEqual(mockFolders);
    });

    const req = httpMock.expectOne((service as any).endpoint + "/media");
    expect(req.request.method).toBe('GET');
    req.flush(mockFolders);
  });

  it('should handle error when retrieving media folders', () => {
    service.getMediaFolders().subscribe(
      () => fail('should have failed with 404 error'),
      (error) => {
        expect(error.status).toEqual(404);
        expect(error.statusText).toEqual('Not Found');
      }
    );

    const req = httpMock.expectOne((service as any).endpoint + "/media");
    req.flush('Not Found', {status: 404, statusText: 'Not Found'});
  });
});
