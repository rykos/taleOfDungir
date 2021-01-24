import { map } from 'rxjs/operators';
import { observable, Observable } from 'rxjs';
import { AccountService } from './../../_services/account.service';
import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-profile-skill-box-widget',
  templateUrl: './profile-skill-box-widget.component.html',
  styleUrls: ['./profile-skill-box-widget.component.css']
})
export class ProfileSkillBoxWidgetComponent implements OnChanges {

  @Input()
  skillName: string;
  @Input()
  skillLevel: number;
  @Input()
  skillBonus: number;
  //
  desc: string;
  skillPrice$: Observable<any>;


  constructor(private accountService: AccountService) { }
  ngOnChanges(changes: SimpleChanges): void {
    this.setDesc();
    this.skillPrice$ = this.accountService.EnchanceSkillPrice(this.skillLevel).pipe(map(res => this.desc + res + " gold"));
  }

  setDesc() {
    if (this.skillName == "Combat") {
      this.desc = `Increase attack power by ${this.skillLevel * 2}%\n`;
    }
    else if (this.skillName == "Luck") {
      this.desc = `Increse chance for better loot\n`;
    }
    else if (this.skillName == "Perception") {
      this.desc = `Increase chance for critical strikes by ${this.skillLevel * 0.5}\n`;
    }
    else if (this.skillName == "Vitality") {
      this.desc = `Increase max health by ${this.skillLevel * 2} points\n`;
    }
  }

  ngOnInit(): void {

  }

  EnchanceSkill() {
    this.accountService.EnchanceSKill(this.skillName.toLocaleLowerCase()).subscribe(x => {
      this.accountService.RefreshCharacter();
    });
  }

}
