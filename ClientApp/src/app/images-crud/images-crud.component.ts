import { ItemType } from './../../_models/ItemType';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-images-crud',
  templateUrl: './images-crud.component.html',
  styleUrls: ['./images-crud.component.css']
})
export class ImagesCrudComponent implements OnInit {
  createForm: FormGroup;
  itemTypes = Object.values(ItemType).filter(value => isNaN(Number(value)));
  fileList: FileList;
  urls: SafeUrl[] = [];

  constructor(private formBuilder: FormBuilder, private sanitezer: DomSanitizer) {
    this.createForm = this.formBuilder.group({
      itemType: ['Trash', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  onSubmit(data) { }

  inputChanged(files: FileList) {
    this.fileList = files;
    Array.prototype.forEach.call(this.fileList, (file) => {
      let url = URL.createObjectURL(file)
      this.urls.push(this.sanitezer.bypassSecurityTrustUrl(url));
    });
    console.log(this.urls);
  }

}
