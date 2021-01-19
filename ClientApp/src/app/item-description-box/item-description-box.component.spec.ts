import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemDescriptionBoxComponent } from './item-description-box.component';

describe('ItemDescriptionBoxComponent', () => {
  let component: ItemDescriptionBoxComponent;
  let fixture: ComponentFixture<ItemDescriptionBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItemDescriptionBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemDescriptionBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
