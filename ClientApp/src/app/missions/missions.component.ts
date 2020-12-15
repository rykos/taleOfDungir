import { MissionEvents } from './../../_models/MissionEvents';
import { Fight } from './../../_models/Fight';
import { AppComponent } from './../app.component';
import { AccountService } from './../../_services/account.service';
import { Mission } from './../../_models/Mission';
import { Component, EventEmitter, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { interval, observable, Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-missions',
  templateUrl: './missions.component.html',
  styleUrls: ['./missions.component.css']
})
export class MissionsComponent implements OnInit, OnDestroy {
  fight: Fight;
  missionEvents: MissionEvents;

  currentFight: Subscription;
  currentMissionEvents: Subscription;

  constructor(private accountService: AccountService) {
    this.currentFight = this.accountService.currentFightSubject.subscribe((val) => {
      this.fight = val;
    });
    this.currentMissionEvents = this.accountService.currentMissionEvents.subscribe((val) => {
      this.missionEvents = val;
      console.log(val);
    });
  }

  ngOnDestroy(): void {
    this.currentFight.unsubscribe();
    this.currentMissionEvents.unsubscribe();
  }

  ngOnInit(): void {
    this.accountService.UpdateMissions();
  }

  get AS() {
    return AccountService;
  }

  Activate(id: number) {
    this.accountService.StartMission(id).subscribe(x => {
      this.accountService.UpdateMissions();
    });
  }

  StartFight(fight: Fight) {
    this.fight = fight;
    let i = 0;
    let timer = setInterval(() => {
      if (i > this.fight.turns.length)
        clearInterval(timer);
      console.log(this.fight[i]);
      i++;
    }, 400);
  }

  FinishMission() {
    this.accountService.UpdateMissions();
    this.accountService.currentFightSubject.next(null);
  }
}
