import { Entity } from './../../_models/Entity';
import { Character } from './../../_models/Character';
import { MissionFinishedObject } from './../../_models/MissionFinishedObject';
import { MissionEvents } from './../../_models/MissionEvents';
import { Fight } from './../../_models/Fight';
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
  missionFinished: MissionFinishedObject;
  currentCharacter: Character;

  currentFight: Subscription;
  currentMissionEvents: Subscription;
  currentMissionFinished: Subscription;
  currentCharacterSubcription: Subscription;

  constructor(private accountService: AccountService) {
    this.currentFight = this.accountService.currentFightSubject.subscribe((val) => {
      this.fight = val;
    });
    this.currentMissionEvents = this.accountService.currentMissionEvents.subscribe((val) => {
      this.missionEvents = val;
    });
    this.currentMissionFinished = this.accountService.currentMissionFinishedSubject.subscribe((val) => {
      this.missionFinished = val;
    });
    this.currentCharacterSubcription = this.accountService.currentCharacterSubject.subscribe((val) => {
      this.currentCharacter = val;
    });
  }

  ngOnDestroy(): void {
    this.currentFight.unsubscribe();
    this.currentMissionEvents.unsubscribe();
    this.currentMissionFinished.unsubscribe();
    this.currentCharacterSubcription.unsubscribe();
  }

  ngOnInit(): void {
    this.accountService.UpdateMissions();
    let x = new Fight();
    let c = new Entity();
    let m = new Entity();
    c.health = 100;
    m.health = 50;
    x.enemyHealth = m;
    x.playerHealth = c;
    this.accountService.currentFightSubject.next(x);
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
    this.accountService.currentMissionEvents.next(null);
    this.accountService.currentMissionFinishedSubject.next(null);
  }
}
