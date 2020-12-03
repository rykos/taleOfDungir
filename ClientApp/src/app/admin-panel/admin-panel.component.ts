import { ComResponse } from './../../_models/ComResponse';
import { environment } from './../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ItemType } from './../../_models/ItemType';
import { Component, OnInit } from '@angular/core';
import { Item } from 'src/_models/Item';
import { NgForm, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { combineAll } from 'rxjs/operators';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  selected: string = "players";

  constructor() { }
  ngOnInit() { }

  IsActive(name: string): boolean {
    if (name.toLowerCase().replace(/\s/g, "") == this.selected.toLowerCase().replace(/\s/g, "")) {
      return true;
    }
    return false;
  }

  MenuItemClick(elem: Element) {
    this.selected = elem.innerHTML;
  }
}
