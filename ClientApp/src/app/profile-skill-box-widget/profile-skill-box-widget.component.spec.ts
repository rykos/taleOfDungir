import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileSkillBoxWidgetComponent } from './profile-skill-box-widget.component';

describe('ProfileSkillBoxWidgetComponent', () => {
  let component: ProfileSkillBoxWidgetComponent;
  let fixture: ComponentFixture<ProfileSkillBoxWidgetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProfileSkillBoxWidgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileSkillBoxWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
