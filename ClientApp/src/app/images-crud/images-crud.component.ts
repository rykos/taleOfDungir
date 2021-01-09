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
  fileListUrls: SafeUrl[] = [];

  constructor(private formBuilder: FormBuilder, private sanitezer: DomSanitizer) {
    this.createForm = this.formBuilder.group({
      imageType: ['item', Validators.required],
      itemType: ['Trash', Validators.required],
      avatarType: ['Player', Validators.required],
      images: ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  onSubmit(data) {
    this.createForm.value.images = this.fileList;
    console.log(data);
    console.log(this.createForm.value.imageType);
  }

  get form(){
    return this.createForm.value;
  }

  inputChanged(files: FileList) {
    this.fileList = files;
    //Clear previous urls
    this.fileListUrls = [];
    Array.prototype.forEach.call(this.fileList, (file) => {
      let url = URL.createObjectURL(file)
      this.fileListUrls.push(this.sanitezer.bypassSecurityTrustUrl(url));
    });
  }

}
