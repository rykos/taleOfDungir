import { AccountService } from './../../_services/account.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  selected: string = "";
  constructor() { }

  ngOnInit(): void { }


  IsActive(name: string): boolean {
    if (name.toLowerCase().replace(/\s/g, "") == this.selected.toLowerCase().replace(/\s/g, "")) {
      return true;
    }
    return false;
  }

  MenuItemClick(elem: Element) {
    this.selected = elem.textContent;
  }

  get staticAccountService() {
    return AccountService;
  }

}
