import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImagesCrudComponent } from './images-crud.component';

describe('ImagesCrudComponent', () => {
  let component: ImagesCrudComponent;
  let fixture: ComponentFixture<ImagesCrudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImagesCrudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImagesCrudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
