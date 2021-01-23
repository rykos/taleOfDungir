import { Time } from './../../_models/Time';
import { MerchantStock } from './../../_models/MerchantStock';
import { Subscription, Observable } from 'rxjs';
import { AccountService } from './../../_services/account.service';
import { Character } from './../../_models/Character';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Item } from 'src/_models/Item';
import { ItemDescriptionBoxComponent } from '../item-description-box/item-description-box.component';
import { retry, map, catchError } from 'rxjs/operators';

@Component({
  selector: 'app-shop-blacksmith',
  templateUrl: './shop-blacksmith.component.html',
  styleUrls: ['./shop-blacksmith.component.css']
})
export class ShopBlacksmithComponent implements OnInit, OnDestroy {
  character: Character;
  sub: Subscription;
  SelectedItem: Item;
  context: string;
  blacksmithItems$: Observable<MerchantStock>;

  constructor(private accountService: AccountService) {

  }

  ngOnInit(): void {
    this.accountService.RefreshCharacter();
    this.sub = this.accountService.currentCharacterSubject.subscribe(c => {
      if (c) {
        this.character = c;
        this.blacksmithItems$ = this.accountService.GetBlacksmithItems().pipe(
          retry(3),
          map(ms => {
            let remaingSeconds = new Date(new Date(ms.restockTime).getTime() - Date.now()).getSeconds() + 1;
            ms.restockTimeRemaining = new Time(remaingSeconds);
            let timerHandle = setInterval(() => {
              ms.restockTimeRemaining = new Time(ms.restockTimeRemaining.totalSeconds - 1);
              if(ms.restockTimeRemaining.totalSeconds <= 0){
                this.accountService.RefreshCharacter();
                clearInterval(timerHandle);
              }
            }, 1000);
            return ms;
          })
        );
      }
    });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  itemClick(itemWidget: HTMLElement, item: Item, context: string) {
    this.context = context;
    this.SelectedItem = item;
    ItemDescriptionBoxComponent.ShowDescriptionBox(itemWidget, this.SelectedItem);
  }
}
