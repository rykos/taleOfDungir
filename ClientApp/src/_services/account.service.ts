import { MissionFinishedObject } from './../_models/MissionFinishedObject';
import { MissionResoult } from './../_models/MissionResoult';
import { MissionEvents } from './../_models/MissionEvents';
import { MissionEvent } from './../_models/MissionEvent';
import { Fight } from './../_models/Fight';
import { first } from 'rxjs/operators';
import { WebVars } from '../_models/WebVars';
import { Title } from '@angular/platform-browser';
import { Mission } from './../_models/Mission';
import { Character } from './../_models/Character';
import { observable, Observable, of, BehaviorSubject } from 'rxjs';
import { environment } from './../environments/environment';
import { AuthenticationService } from './authentication.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  public static MissionTimeLeft;
  public static MissionTimeLeftPercent;
  public static availableMissions: Mission[];
  public static activeMission: Mission;
  public currentMissionEvents: BehaviorSubject<MissionEvents>;
  public currentFightSubject: BehaviorSubject<Fight>;
  public currentMissionFinishedSubject: BehaviorSubject<MissionFinishedObject> = new BehaviorSubject<MissionFinishedObject>(null);
  public currentCharacterSubject: BehaviorSubject<Character> = new BehaviorSubject<Character>(null);

  constructor(private httpClient: HttpClient, private titleService: Title) {
    this.currentFightSubject = new BehaviorSubject<Fight>(null);
    this.currentMissionEvents = new BehaviorSubject<MissionEvents>(null);
  }

  public UpdateMissions() {
    this.GetMissions().subscribe(missions => {
      if (!missions) { this.UpdateMissions(); }
      else if (missions.length > 1) {
        this.onMissionsSelect(missions);
      }
      else {
        this.onMissionActive();
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

  private onMissionActive() {
    //Mission is already active
    this.GetActiveMission().subscribe(m => {
      if (m.state == "mission") {
        AccountService.availableMissions = null;
        AccountService.activeMission = <Mission>m.value;
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
      else if (m.state == "fight") {
        this.onMissionFinished(<Fight>m.value);
      }
      else if (m.state == "event") {
        this.currentMissionEvents.next(<MissionEvents>m.value);
      }
      else if (m.state == "finished") {
        console.log("mission got finished");
        console.log(m.value);
      }
      else {
        console.warn("unknown response");
        this.UpdateMissions();
      }
    });
  }

  private onMissionsSelect(missions: Mission[]) {
    AccountService.activeMission = null;
    AccountService.availableMissions = missions;
  }

  private onMissionFinished(fight: Fight) {
    // AccountService.fight = fight;
    this.currentFightSubject.next(fight);
  }

  Details(): Observable<Character> {
    return this.httpClient.get<Character>(environment.apiUrl + "/character/details");
  }

  RefreshCharacter(): void {
    this.Details().subscribe(c => this.currentCharacterSubject.next(c));
  }

  GetMissions(): Observable<Mission[]> {
    return this.httpClient.get<Mission[]>(`${environment.apiUrl}/missions`);
  }

  GetActiveMission(): Observable<MissionResoult> {
    return this.httpClient.get<MissionResoult>(`${environment.apiUrl}/missions/active`);
  }

  StartMission(missionId: number): Observable<any> {
    return this.httpClient.get(`${environment.apiUrl}/missions/start/${missionId}`);
  }

  PickMissionEventAction(eventActionId: number): Observable<MissionResoult> {
    return this.httpClient.get<MissionResoult>(`${environment.apiUrl}/missions/active/event/${eventActionId}`);
  }
}
