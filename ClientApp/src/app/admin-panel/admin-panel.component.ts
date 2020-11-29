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
  itemTypes = Object.values(ItemType).filter(value => isNaN(Number(value)));
  selectedItemType = this.itemTypes[0];

  foundItems: Item[] = [];

  createForm: FormGroup;
  constructor(private formBuilder: FormBuilder, private httpClient: HttpClient) {
    this.createForm = this.formBuilder.group({
      itemType: ['Trash', Validators.required],
      itemName: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  ngOnInit(): void {
    let x = new Item();
    x.id = 1;
    x.itemType = ItemType.Finger;
    x.level = 1;
    x.name = "Itemek";
    x.power = 1;
    this.foundItems = [x, x, x, x];
    console.log(this.createForm.get("itemName").errors);
  }

  public Changed(newValue) {
    console.log(newValue);
  }

  SearchBoxChanged(value) {
    let pattern = value ? `?pattern=${value}` : '';
    this.httpClient.get<Item[]>(`${environment.apiUrl}/admin/items/names${pattern}`).subscribe(x => {
      this.foundItems = x;
    });
  }

  onSubmit(data) {
    if (this.createForm.valid) {
      let params = `?itemType=${this.createForm.value.itemType}&itemName=${this.createForm.value.itemName}`;
      this.httpClient.post<ComResponse>(`${environment.apiUrl}/admin/items/names${params}`, null).subscribe(x => {
        if (x && x.type === "Error") {
          console.log(x.message);
        }
        else {
          //Success
        }
      });
    }
  }

}
