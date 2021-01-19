import { AccountService } from './../../_services/account.service';
import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Item } from 'src/_models/Item';

@Component({
  selector: 'app-item-description-box',
  templateUrl: './item-description-box.component.html',
  styleUrls: ['./item-description-box.component.css']
})
export class ItemDescriptionBoxComponent implements OnInit {
  @Input()
  item: Item;
  @Input()
  context: string;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void { }

  itemClose() {
    document.getElementById("ItemDescriptionBox").style.visibility = "hidden";
  }

  useItem() {
    if (this.context == "equip") {
      this.accountService.EquipItem(this.item).subscribe(x => {
        this.accountService.RefreshCharacter();
        this.itemClose();
      });
    }
    else if (this.context == "eat") {
      console.log("eating some food");
    }
  }

  static ShowDescriptionBox(itemWidget: HTMLElement, item: Item, ctx?: string) {
    let itemDescriptionItem = document.getElementById("ItemDescriptionBox");
    itemDescriptionItem.style.visibility = "visible";
    itemDescriptionItem.style.top = itemWidget.offsetTop + "px";
    itemDescriptionItem.style.left = itemWidget.offsetLeft + "px";
  }
}
