import { ItemDescriptionBoxComponent } from './../item-description-box/item-description-box.component';
import { MissionReward } from './../../_models/MissionReward';
import { AccountService } from './../../_services/account.service';
import { Fight } from './../../_models/Fight';
import { Component, Input, OnInit } from '@angular/core';
import { Item } from 'src/_models/Item';

@Component({
  selector: 'app-fight',
  templateUrl: './fight.component.html',
  styleUrls: ['./fight.component.css']
})
export class FightComponent implements OnInit {
  @Input()
  fight: Fight;
  fightFinished: boolean = false;
  fightTimer: number;
  ph: number;
  eh: number;
  reward: MissionReward;
  SelectedItem: Item;

  constructor(private accountService: AccountService) {
    this.reward = accountService.currentMissionRewardSubject.value;
  }

  ngOnInit(): void {
    this.CalculateFinalHealth();
    var i = 0;
    this.fightTimer = setInterval(() => {
      console.log(i);
      if (this.fight.turns[i].playerAttack) {
        this.fight.enemy.health -= this.fight.turns[i].damageDealt;
      }
      else {
        this.fight.player.health -= this.fight.turns[i].damageDealt;
      }
      i++;
      if (i >= this.fight.turns.length) {
        this.fightFinished = true;
        clearInterval(this.fightTimer);
      }
    }, 1000);
  }

  CalculateFinalHealth() {
    this.ph = this.fight.player.health;
    this.eh = this.fight.enemy.health;
    for (var i = 0; i < this.fight.turns.length; i++) {
      if (this.fight.turns[i].playerAttack) {
        this.eh -= this.fight.turns[i].damageDealt;
      }
      else {
        this.ph -= this.fight.turns[i].damageDealt;
      }
    }
  }

  FinishMission(): void {
    this.accountService.FinishMission();
  }

  SkipFight(): void {
    clearInterval(this.fightTimer);
    this.fight.player.health = this.ph;
    this.fight.enemy.health = this.eh;
    this.fightFinished = true;
  }

  itemClick(itemWidget: HTMLElement, item: Item) {
    this.SelectedItem = item;
    ItemDescriptionBoxComponent.ShowDescriptionBox(itemWidget, item);
  }

}
