import { Item } from 'src/_models/Item';
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ItemType } from 'src/_models/ItemType';
import { flatMap } from 'rxjs/operators';

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

  //Needs rework
  ngOnInit(): void {
    if (this.context == "none") {
      this.text = null;
    }
    //forced context
    else if (this.context) {
      if (this.context == "buy" || this.context == "sell") {
        this.text = this.context
        this.color = "green";
      }
      else if (!this.equipable()) {
        this.text = null;
      }
    }
    else if (this.item.itemType == ItemType.None || this.item.itemType == ItemType.Trash) {
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
      this.equipable();
    }

  }

  equipable(): boolean {
    if (<number>this.item.itemType > 3) {
      if (this.item.worn) {
        this.text = 'unequip';
        this.color = 'red';
      }
      else {
        this.text = 'equip'
        this.color = 'green';
      }
      return true;
    }
    return false;
  }

}
