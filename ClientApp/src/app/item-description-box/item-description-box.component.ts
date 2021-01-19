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

  constructor(private accountService: AccountService) { }

  ngOnInit(): void { }

  itemClose() {
    document.getElementById("ItemDescriptionBox").style.visibility = "hidden";
  }

  useItem() {
    console.log(`use item ${this.item.name}`);
    this.accountService.EquipItem(this.item).subscribe(x => {
      this.accountService.RefreshCharacter();
      this.itemClose();
    });
  }

  static ShowDescriptionBox(itemWidget: HTMLElement, item: Item) {
    let itemDescriptionItem = document.getElementById("ItemDescriptionBox");
    itemDescriptionItem.style.visibility = "visible";
    itemDescriptionItem.style.top = itemWidget.offsetTop + "px";
    itemDescriptionItem.style.left = itemWidget.offsetLeft + "px";
  }
}
