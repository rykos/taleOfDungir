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
  //
  skillPrice$: Observable<any>;


  constructor(private accountService: AccountService) { }
  ngOnChanges(changes: SimpleChanges): void {
    this.skillPrice$ = this.accountService.EnchanceSkillPrice(this.skillLevel);
  }

  ngOnInit(): void {
    
  }

  EnchanceSkill() {
    this.accountService.EnchanceSKill(this.skillName.toLocaleLowerCase()).subscribe(x => {
      this.accountService.RefreshCharacter();
    });
  }

}
