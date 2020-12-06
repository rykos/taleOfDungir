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
  timeLeft;
  percentage?: number;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    // let end;
    // if ((end = localStorage.getItem('missionEnd'))) {
    //   console.log(new Date(JSON.parse(end)));
    // }
    this.Update();
  }

  Update() {
    this.accountService.GetMissions().subscribe(m => {
      if (!m) { }
      else if (m.length > 1) {
        this.activeMission = null;
        this.availableMissions = m;
      }
      else {
        //Mission is already active
        this.accountService.GetActiveMission().subscribe(m => {
          if (m) {
            this.availableMissions = null;
            this.activeMission = m;
            let start = new Date(this.activeMission.startTime);
            let end = new Date(start.getTime() + this.activeMission.duration * 1000);
            localStorage.setItem('missionEnd', JSON.stringify(end));
            this.UpdateMissionTime(end);
            let timer = setInterval(() => {
              this.UpdateMissionTime(end);
              if (this.timeLeft <= 0) {
                this.timeLeft = 0;
                this.Update();
                clearInterval(timer);
              }
            }, 1000);
          }
          else {
            this.Update();
          }
        });
      }
    });
  }

  UpdateMissionTime(end) {
    this.timeLeft = ((end.getTime() - Date.now().valueOf()) / 1000);
    this.timeLeft = Math.round(this.timeLeft);
    this.percentage = Math.round((1 - this.timeLeft / this.activeMission.duration) * 100);
    AccountService.MissionTimeLeft = this.timeLeft;
  }

  Activate(id: number) {
    this.accountService.StartMission(id).subscribe(x => {
      this.Update();
    });
  }
}
