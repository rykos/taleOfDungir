import { ErrorInterceptor } from './../_helpers/error.interceptor';
import { JwtInterceptor } from './../_helpers/jwt.interceptor';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainComponent } from './main/main.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { NavigationComponent } from './navigation/navigation.component';
import { AccountComponent } from './account/account.component';
import { ProfileComponent } from './profile/profile.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { ApItemNamesComponent } from './ap-item-names/ap-item-names.component';
import { ApItemsComponent } from './ap-items/ap-items.component';
import { ApPlayersComponent } from './ap-players/ap-players.component';
import { MissionsComponent } from './missions/missions.component';
import { RarityPipe } from './rarity.pipe';
import { MissionEventComponent } from './mission-event/mission-event.component';
import { FightComponent } from './fight/fight.component';
import { EntityCardComponent } from './entity-card/entity-card.component';
import { ImagesCrudComponent } from './images-crud/images-crud.component';
import { ItemWidgetComponent } from './item-widget/item-widget.component';
import { ItemDescriptionInteractButtonComponent } from './item-description-interact-button/item-description-interact-button.component';
import { ItemDescriptionBoxComponent } from './item-description-box/item-description-box.component';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    LoginComponent,
    RegisterComponent,
    NavigationComponent,
    AccountComponent,
    ProfileComponent,
    AdminPanelComponent,
    ApItemNamesComponent,
    ApItemsComponent,
    ApPlayersComponent,
    MissionsComponent,
    RarityPipe,
    MissionEventComponent,
    FightComponent,
    EntityCardComponent,
    ImagesCrudComponent,
    ItemWidgetComponent,
    ItemDescriptionInteractButtonComponent,
    ItemDescriptionBoxComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
