import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApItemsComponent } from './ap-items.component';

describe('ApItemsComponent', () => {
  let component: ApItemsComponent;
  let fixture: ComponentFixture<ApItemsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApItemsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
