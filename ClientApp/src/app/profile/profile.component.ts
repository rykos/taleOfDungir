import { Equipment } from './../../_models/Equipment';
import { Item } from './../../_models/Item';
import { Character } from './../../_models/Character';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AccountService } from './../../_services/account.service';
import { User } from './../../_models/User';
import { AuthenticationService } from './../../_services/authentication.service';
import { Component, ElementRef, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user: User;
  character: Character;
  ItemDescriptionBox: Item;
  constructor(private authenticationService: AuthenticationService, private accountService: AccountService, private httpClient: HttpClient) {
    this.user = authenticationService.currentUserValue;
    accountService.Details().subscribe(x => {
      this.character = x;
      this.character.equipment = new Equipment();
      this.character.equipment.head = this.character.inventory[0];
    });
  }

  ngOnInit(): void {
  }

  ImgLink(imageId: string): string {
    return AccountService.GetImageLink(imageId);
  }

  itemClick(itemWidget: HTMLElement, item: Item) {
    this.ItemDescriptionBox = item;
    let itemDescriptionItem = document.getElementById("ItemDescriptionBox");
    itemDescriptionItem.style.visibility = "visible";
    itemDescriptionItem.style.top = itemWidget.offsetTop + "px";
    itemDescriptionItem.style.left = itemWidget.offsetLeft + "px";
  }

  itemClose() {
    document.getElementById("ItemDescriptionBox").style.visibility = "hidden";
    this.ItemDescriptionBox = null;
  }

  equipItem() {
    console.log(`equip ${this.ItemDescriptionBox.name}`);
  }

}
