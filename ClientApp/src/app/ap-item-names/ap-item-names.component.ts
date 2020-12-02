import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { ComResponse } from 'src/_models/ComResponse';
import { Item } from 'src/_models/Item';
import { ItemType } from 'src/_models/ItemType';

@Component({
  selector: 'app-ap-item-names',
  templateUrl: './ap-item-names.component.html',
  styleUrls: ['./ap-item-names.component.css']
})
export class ApItemNamesComponent implements OnInit {

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
    this.httpClient.get<Item[]>(`${environment.apiUrl}/admin/items/names`).subscribe(x => {
      this.foundItems = x;
    });
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

  Delete(id: number) {
    this.httpClient.delete<ComResponse>(`${environment.apiUrl}/admin/items/names/${id}`).subscribe(x => {
      if (x && x.type == "Success") {
        let index = this.foundItems.findIndex(x => x.id === id);
        if (index > -1) {
          this.foundItems.splice(index, 1);
        }
      }
    });
  }

  ItemTypeToString(id: number): string {
    return ItemType[id];
  }
}