import { AppComponent } from './../app.component';
import { AccountService } from './../../_services/account.service';
import { Mission } from './../../_models/Mission';
import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-missions',
  templateUrl: './missions.component.html',
  styleUrls: ['./missions.component.css']
})
export class MissionsComponent implements OnInit {

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.Update();
  }

  get AS() {
    return AccountService;
  }

  Update() {
    this.accountService.UpdateMissions();
  }

  Activate(id: number) {
    this.accountService.StartMission(id).subscribe(x => {
      this.Update();
    });
  }
}
