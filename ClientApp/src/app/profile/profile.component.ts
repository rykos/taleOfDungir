import { Subscription } from 'rxjs';
import { Equipment } from './../../_models/Equipment';
import { Item } from './../../_models/Item';
import { Character } from './../../_models/Character';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AccountService } from './../../_services/account.service';
import { User } from './../../_models/User';
import { AuthenticationService } from './../../_services/authentication.service';
import { Component, ElementRef, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy {
  user: User;
  character: Character;
  ItemDescriptionBox: Item;
  characterSub: Subscription;
  constructor(private authenticationService: AuthenticationService, private accountService: AccountService, private httpClient: HttpClient) {
    this.user = authenticationService.currentUserValue;
    accountService.RefreshCharacter();
    this.characterSub = accountService.currentCharacterSubject.subscribe((c) => {
      this.character = c;
    });
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.characterSub.unsubscribe();
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
    this.accountService.EquipItem(this.ItemDescriptionBox).subscribe(x => {
      console.log(x);
      this.accountService.RefreshCharacter();
    });
  }

}
