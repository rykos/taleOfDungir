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

  constructor() { }

  ngOnInit(): void {
  }

  EventActionClick(eventActionId: number) {
    console.log(eventActionId);
  }

  eventActionIdToValue(eventActionId: number): number {
    return this.missionEvents.msr.eventActionIdToValue.find(x => x.Key == eventActionId).Value;
  }
}
