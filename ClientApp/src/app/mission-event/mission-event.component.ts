import { Character } from './../../_models/Character';
import { Subscription } from 'rxjs';
import { MissionFinishedObject } from './../../_models/MissionFinishedObject';
import { AccountService } from './../../_services/account.service';
import { MissionEvents } from './../../_models/MissionEvents';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-mission-event',
  templateUrl: './mission-event.component.html',
  styleUrls: ['./mission-event.component.css']
})
export class MissionEventComponent implements OnInit, OnDestroy {
  @Input()
  missionEvents: MissionEvents;
  currentCharacterSub: Subscription;
  currentCharacter: Character;

  constructor(private accountService: AccountService) {
    this.currentCharacterSub = this.accountService.currentCharacterSubject.subscribe(c => {
      this.currentCharacter = c;
      console.log(this.currentCharacter);
    });
  }

  ngOnInit(): void {
    this.accountService.RefreshCharacter();
  }

  ngOnDestroy(): void {
    this.currentCharacterSub.unsubscribe();
  }

  EventActionClick(eventActionId: number) {
    this.accountService.PickMissionEventAction(eventActionId).subscribe(x => {
      if (x.state == "finished") {
        this.accountService.currentMissionEvents.next(null);
        this.accountService.currentMissionFinishedSubject.next(<MissionFinishedObject>x.value);
        this.accountService.UpdateMissions();
      }
    });
  }

  eventActionIdToValue(eventActionId: number): number {
    return this.missionEvents.msr.eventActionIdToValue.find(x => x.Key == eventActionId).Value;
  }

  ViableOption(skillName: string, value: number) {
    if(!this.currentCharacter){
      return false;
    }
    skillName = skillName.toLocaleLowerCase();
    if (skillName == "vitality") {
      if (this.currentCharacter.skills.vitality >= value)
        return true;
    }
    else if (skillName == "combat") {
      if (this.currentCharacter.skills.combat >= value)
        return true;
    }
    else if (skillName == "luck") {
      if (this.currentCharacter.skills.luck >= value)
        return true;
    }
    else if (skillName == "perception") {
      if (this.currentCharacter.skills.perception >= value)
        return true;
    }
    return false;
  }
}
