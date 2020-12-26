import { AccountService } from './../../_services/account.service';
import { Fight } from './../../_models/Fight';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-fight',
  templateUrl: './fight.component.html',
  styleUrls: ['./fight.component.css']
})
export class FightComponent implements OnInit {
  @Input()
  fight: Fight;

  constructor(private accountService: AccountService) { 
    
  }

  ngOnInit(): void {
    var pa = true;
    var t = setInterval(() => {
      if(pa){
        this.fight.enemy.health -= 10;
      }
      else{
        this.fight.player.health -= 10;
      }
      if (this.fight.enemy.health <= 0 || this.fight.player.health <= 0) {
        clearInterval(t);
      }
      pa = !pa;
    }, 1000);
    console.log(this.fight);
  }

  FinishMission(): void {
    this.accountService.currentFightSubject.next(null);
  }

}
