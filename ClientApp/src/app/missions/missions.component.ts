import { AccountService } from './../../_services/account.service';
import { Mission } from './../../_models/Mission';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-missions',
  templateUrl: './missions.component.html',
  styleUrls: ['./missions.component.css']
})
export class MissionsComponent implements OnInit {
  availableMissions: Mission[];
  activeMission: Mission;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.accountService.GetMissions().subscribe(m => {
      if (!m) { }
      else if (m.length > 1) {
        console.log("I should present you with those missions");
        console.log(m);
        this.activeMission = null;
        this.availableMissions = m;
      }
      else {
        //Mission is already active
        this.accountService.GetActiveMission().subscribe(m => {
          if (m) {
            console.log("This is active mission");
            console.log(m);
            this.availableMissions = null;
            this.activeMission = m;
          }
        });
      }
    });
  }

}
