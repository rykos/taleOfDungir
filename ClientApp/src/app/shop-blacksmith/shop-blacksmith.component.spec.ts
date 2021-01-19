import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShopBlacksmithComponent } from './shop-blacksmith.component';

describe('ShopBlacksmithComponent', () => {
  let component: ShopBlacksmithComponent;
  let fixture: ComponentFixture<ShopBlacksmithComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShopBlacksmithComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShopBlacksmithComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
