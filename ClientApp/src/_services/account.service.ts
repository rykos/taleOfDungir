import { Mission } from './../_models/Mission';
import { Character } from './../_models/Character';
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

  Details(): Observable<Character> {
    return this.httpClient.get<Character>(environment.apiUrl + "/character/details");
  }

  GetMissions(): Observable<Mission[]> {
    return this.httpClient.get<Mission[]>(`${environment.apiUrl}/missions`);
  }

  GetActiveMission(): Observable<Mission> {
    return this.httpClient.get<Mission>(`${environment.apiUrl}/missions/active`);
  }

  StartMission(missionId: number): Observable<any> {
    return this.httpClient.get(`${environment.apiUrl}/missions/start/${missionId}`);
  }
}
