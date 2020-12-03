import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApPlayersComponent } from './ap-players.component';

describe('ApPlayersComponent', () => {
  let component: ApPlayersComponent;
  let fixture: ComponentFixture<ApPlayersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApPlayersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApPlayersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
