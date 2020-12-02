import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApItemNamesComponent } from './ap-item-names.component';

describe('ApItemNamesComponent', () => {
  let component: ApItemNamesComponent;
  let fixture: ComponentFixture<ApItemNamesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApItemNamesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApItemNamesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
