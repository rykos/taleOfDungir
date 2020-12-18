import { AccountService } from './../../_services/account.service';
import { MissionEvents } from './../../_models/MissionEvents';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-mission-event',
  templateUrl: './mission-event.component.html',
  styleUrls: ['./mission-event.component.css']
})
export class MissionEventComponent implements OnInit {

  @Input()
  missionEvents: MissionEvents;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  EventActionClick(eventActionId: number) {
    this.accountService.PickMissionEventAction(eventActionId).subscribe(x => {
      console.log(x);
    });
  }

  eventActionIdToValue(eventActionId: number): number {
    return this.missionEvents.msr.eventActionIdToValue.find(x => x.Key == eventActionId).Value;
  }
}
