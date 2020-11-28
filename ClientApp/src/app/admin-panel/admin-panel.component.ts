import { ItemType } from './../../_models/ItemType';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  itemTypes = Object.values(ItemType).filter(value => isNaN(Number(value)));
  selectedItemType = this.itemTypes[0];

  constructor() { }

  ngOnInit(): void {
    
  }

  public Changed(newValue) {
    console.log(newValue);
  }

}
