import { ItemDescriptionBoxComponent } from './../item-description-box/item-description-box.component';
import { Item } from 'src/_models/Item';
import { Subscription, interval } from 'rxjs';
import { Equipment } from './../../_models/Equipment';
import { Character } from './../../_models/Character';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AccountService } from './../../_services/account.service';
import { User } from './../../_models/User';
import { AuthenticationService } from './../../_services/authentication.service';
import { Component, ElementRef, OnInit, OnDestroy } from '@angular/core';
import { ItemType } from 'src/_models/ItemType';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy {
  user: User;
  character: Character;
  SelectedItem: Item;
  characterSub: Subscription;
  context: string;

  constructor(private authenticationService: AuthenticationService, private accountService: AccountService, private httpClient: HttpClient) {
    this.user = authenticationService.currentUserValue;
    accountService.RefreshCharacter();
    this.characterSub = accountService.currentCharacterSubject.subscribe((c) => {
      if (c) {
        this.character = c;
        this.character.inventory = c.inventory.filter(i => !i?.worn);
        for (let i = this.character.inventory.length; i < 21; i++) {
          this.character.inventory.push(null);
        }
      }
    });
  }

  ngOnInit(): void { }

  ngOnDestroy(): void {
    this.characterSub.unsubscribe();
  }

  ImgLink(imageId: string): string {
    return AccountService.GetImageLink(imageId);
  }

  itemClick(itemWidget: HTMLElement, item: Item, context: string) {
    this.context = context;
    this.SelectedItem = item;
    ItemDescriptionBoxComponent.ShowDescriptionBox(itemWidget, this.SelectedItem);
  }

}
