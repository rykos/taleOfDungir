import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemDescriptionInteractButtonComponent } from './item-description-interact-button.component';

describe('ItemDescriptionInteractButtonComponent', () => {
  let component: ItemDescriptionInteractButtonComponent;
  let fixture: ComponentFixture<ItemDescriptionInteractButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItemDescriptionInteractButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemDescriptionInteractButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
