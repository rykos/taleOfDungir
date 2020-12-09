import { FightTurn } from './../_models/FightTurn';
import { first } from 'rxjs/operators';
import { WebVars } from '../_models/WebVars';
import { Title } from '@angular/platform-browser';
import { Mission } from './../_models/Mission';
import { Character } from './../_models/Character';
import { Observable } from 'rxjs';
import { environment } from './../environments/environment';
import { AuthenticationService } from './authentication.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { typeofExpr } from '@angular/compiler/src/output/output_ast';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  public static MissionTimeLeft;
  public static MissionTimeLeftPercent;
  public static availableMissions: Mission[];
  public static activeMission: Mission;

  constructor(private httpClient: HttpClient, private titleService: Title) {

  }

  public UpdateMissions() {
    this.GetMissions().subscribe(missions => {
      if (!missions) { this.UpdateMissions(); }
      else if (missions.length > 1) {
        this.onMissionsSelect(missions);
      }
      else {
        this.onMissionActive(missions[0]);
      }
    });
  }

  private UpdateMissionTime(end) {
    if (AccountService.activeMission) {
      AccountService.MissionTimeLeft = Math.round((end.getTime() - Date.now().valueOf()) / 1000);
      AccountService.MissionTimeLeftPercent = Math.round((1 - AccountService.MissionTimeLeft / AccountService.activeMission.duration) * 100);
    }
    else {
      AccountService.MissionTimeLeft = 0;
    }
    if (AccountService.MissionTimeLeft > 0) {
      this.titleService.setTitle(`${WebVars.Title} (${AccountService.MissionTimeLeft}s)`);
    }
    else {
      this.titleService.setTitle(WebVars.Title);
    }
  }

  private onMissionActive(mission: Mission) {
    //Mission is already active
    this.GetActiveMission().subscribe(m => {
      if ((m as Mission).name) {
        AccountService.availableMissions = null;
        AccountService.activeMission = <Mission>m;
        let start = new Date(AccountService.activeMission.startTime);
        let end = new Date(start.getTime() + AccountService.activeMission.duration * 1000);
        this.UpdateMissionTime(end);
        let timer = setInterval(() => {
          this.UpdateMissionTime(end);
          if (AccountService.MissionTimeLeft <= 0) {
            AccountService.MissionTimeLeft = 0;
            this.UpdateMissions();
            clearInterval(timer);
          }
        }, 1000);
      }
      else if(Array.isArray(m) && (m[0] as FightTurn).damageDealt != null){
        console.log(<FightTurn[]>m);
        this.UpdateMissions();
      }
      else {
        this.UpdateMissions();
      }
    });
  }

  private onMissionsSelect(missions: Mission[]) {
    AccountService.activeMission = null;
    AccountService.availableMissions = missions;
  }

  Details(): Observable<Character> {
    return this.httpClient.get<Character>(environment.apiUrl + "/character/details");
  }

  GetMissions(): Observable<Mission[]> {
    return this.httpClient.get<Mission[]>(`${environment.apiUrl}/missions`);
  }

  GetActiveMission(): Observable<Mission | FightTurn[]> {
    return this.httpClient.get<Mission | FightTurn[]>(`${environment.apiUrl}/missions/active`);
  }

  StartMission(missionId: number): Observable<any> {
    return this.httpClient.get(`${environment.apiUrl}/missions/start/${missionId}`);
  }
}
