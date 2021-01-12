import { HttpClient } from '@angular/common/http';
import { ItemType } from './../../_models/ItemType';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-images-crud',
  templateUrl: './images-crud.component.html',
  styleUrls: ['./images-crud.component.css']
})
export class ImagesCrudComponent implements OnInit {
  createForm: FormGroup;
  itemTypes = Object.values(ItemType).filter(value => isNaN(Number(value)));
  selectedCategory = "item";
  types = [];
  fileList: FileList;
  fileListUrls: SafeUrl[] = [];

  constructor(private formBuilder: FormBuilder, private sanitezer: DomSanitizer, private httpClient: HttpClient) {
    this.createForm = this.formBuilder.group({
      category: ['item', Validators.required],
      type: ["None", Validators.required],
      images: ['', Validators.required]
    });
    this.categoryChanged("item");
  }

  categoryChanged(category: string) {
    this.selectedCategory = category[0].toUpperCase() + category.slice(1);
    if (category == "item") {
      this.types = Object.values(ItemType).filter(value => isNaN(Number(value)));
    }
    else {
      this.types = ["player", "monster"];
    }
    this.createForm.value.type = this.types[0];
  }

  ngOnInit(): void {
  }

  onSubmit(data) {
    this.createForm.value.images = this.fileList;

    var x = this.fileList[0];

    let fd = new FormData();
    fd.append('category', this.createForm.value.category);
    fd.append('type', this.createForm.value.type);
    for (let i = 0; i < this.fileList.length; i++) {
      fd.append("Files", this.fileList[i]);
    }
    //https://localhost:5001/images
    this.httpClient.post(`${environment.apiUrl}/images`, fd).subscribe(x => {
      console.log(x);
    });
  }

  get form() {
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
