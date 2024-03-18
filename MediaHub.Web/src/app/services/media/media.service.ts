import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {Observable} from 'rxjs';
import {Media} from '../../types/media.type';

@Injectable({
  providedIn: 'root'
})
export class MediaService {

  constructor(private httpClient: HttpClient) {
  }

  private endpoint = environment.BACKEND_URL;

  public getMediaFolders(path?: string): Observable<Media[]> {
    const url = this.endpoint + "/media" + (path ? "?path=" + encodeURIComponent(path) : "");

    return this.httpClient.get<Media[]>(url);
  }
}
