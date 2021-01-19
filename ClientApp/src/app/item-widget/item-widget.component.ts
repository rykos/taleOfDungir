import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { AccountService } from './../../_services/account.service';
import { Component, OnInit, Input } from '@angular/core';
import { Item } from 'src/_models/Item';

@Component({
  selector: 'app-item-widget',
  templateUrl: './item-widget.component.html',
  styleUrls: ['./item-widget.component.css']
})
export class ItemWidgetComponent implements OnInit {
  @Input()
  item: Item;
  @Input()
  placeholder: string;

  constructor(private domSanitizer: DomSanitizer) { }

  ngOnInit(): void { }

  ImgLink(imageId: string): string {
    return AccountService.GetImageLink(imageId);
  }

  PlaceholderLink(type: string) {
    return this.domSanitizer.bypassSecurityTrustResourceUrl(`http://localhost:4200/assets/${type}.png`);
  }

}
