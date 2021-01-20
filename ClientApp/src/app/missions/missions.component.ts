import { Item } from 'src/_models/Item';
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
import { ItemDescriptionBoxComponent } from '../item-description-box/item-description-box.component';

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

  SelectedItem: Item;
  context: string;

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
  }

  get AS() {
    return AccountService;
  }

  Activate(id: number) {
    this.accountService.StartMission(id).subscribe(x => {
      this.accountService.UpdateMissions();
    });
  }

  FinishMission() {
    this.accountService.FinishMission();
  }

  itemClick(itemWidget: HTMLElement, item: Item, context: string) {
    this.context = context;
    this.SelectedItem = item;
    ItemDescriptionBoxComponent.ShowDescriptionBox(itemWidget, this.SelectedItem);
  }
}
