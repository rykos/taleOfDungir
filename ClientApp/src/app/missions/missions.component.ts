import { Fight } from './../../_models/Fight';
import { AppComponent } from './../app.component';
import { AccountService } from './../../_services/account.service';
import { Mission } from './../../_models/Mission';
import { Component, EventEmitter, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { interval } from 'rxjs';

@Component({
  selector: 'app-missions',
  templateUrl: './missions.component.html',
  styleUrls: ['./missions.component.css']
})
export class MissionsComponent implements OnInit {
  fight: Fight;

  constructor(private accountService: AccountService) {
    this.accountService.currentFightSubject.subscribe((val) => {
      this.fight = this.fight;
    });
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
