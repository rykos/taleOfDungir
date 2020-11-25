import { Observable } from 'rxjs';
import { environment } from './../environments/environment';
import { AuthenticationService } from './authentication.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private authenticationService: AuthenticationService, private httpClient: HttpClient) {

  }

  Details(): Observable<string> {
    return this.httpClient.get<string>(environment.apiUrl + "/account/details");
  }
}
