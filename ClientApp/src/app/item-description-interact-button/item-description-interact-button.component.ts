import { Item } from 'src/_models/Item';
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ItemType } from 'src/_models/ItemType';

@Component({
  selector: 'app-item-description-interact-button',
  templateUrl: './item-description-interact-button.component.html',
  styleUrls: ['./item-description-interact-button.component.css']
})
export class ItemDescriptionInteractButtonComponent implements OnInit, OnChanges {
  @Input()
  item: Item;
  //Forced button text
  @Input()
  context: string;
  //Button text
  text: string;
  //Button color
  color: string;
  // context: string;

  constructor() { 
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.ngOnInit();
  }

  ngOnInit(): void {
    if (this.context) {
      this.text = this.context;
      this.color = 'green';
      return;
    }
    if (this.item.itemType == ItemType.None || this.item.itemType == ItemType.Trash) {
      this.text = '';
    }
    else if (this.item.itemType == ItemType.Consumable) {
      this.text = 'consume';
      this.color = 'green';
    }
    else if (this.item.itemType == ItemType.Resource) {
      this.text = '';
    }
    else {
      if (this.item.worn) {
        this.text = 'unequip';
        this.color = 'red';
      }
      else {
        this.text = 'equip'
        this.color = 'green';
      }
    }

  }

}
