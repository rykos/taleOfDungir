import { FightTurn } from './../../_models/FightTurn';
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
  fight: FightTurn[];

  constructor(private accountService: AccountService) {
    this.accountService.currentFightSubject.subscribe((val) => {
      console.log(`I received val = ${val}`);
    });
    // this.accountService.currentFight.subscribe((val) => {
    //   console.log(`I received val = ${val}`);
    // });
  }

  ngOnInit(): void {
    this.accountService.UpdateMissions();
    if (AccountService.fight) {
      this.StartFight(AccountService.fight);
    }
  }

  get AS() {
    return AccountService;
  }

  Activate(id: number) {
    this.accountService.StartMission(id).subscribe(x => {
      this.accountService.UpdateMissions();
    });
  }

  StartFight(fight: FightTurn[]) {
    this.fight = fight;
    let i = 0;
    let timer = setInterval(() => {
      if (i > this.fight.length)
        clearInterval(timer);
      console.log(this.fight[i]);
      i++;
    }, 400);
  }

  FinishMission() {
    this.accountService.UpdateMissions();
    this.AS.fight = null;
  }
}
