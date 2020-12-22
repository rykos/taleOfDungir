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
  
  constructor() { }

  ngOnInit(): void {
  }

}
