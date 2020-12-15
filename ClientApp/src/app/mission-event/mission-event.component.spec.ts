import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MissionEventComponent } from './mission-event.component';

describe('MissionEventComponent', () => {
  let component: MissionEventComponent;
  let fixture: ComponentFixture<MissionEventComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MissionEventComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MissionEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
