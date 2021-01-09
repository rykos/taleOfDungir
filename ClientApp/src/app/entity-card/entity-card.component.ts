import { Entity } from './../../_models/Entity';
import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-entity-card',
  templateUrl: './entity-card.component.html',
  styleUrls: ['./entity-card.component.css']
})
export class EntityCardComponent implements OnInit {
  @Input()
  entity: Entity;
  constructor() { }

  ngOnInit(): void {
  }

  ImgLink(imageId: string): string {
    return `${environment.apiUrl}/images/${imageId}`;
  }

}
