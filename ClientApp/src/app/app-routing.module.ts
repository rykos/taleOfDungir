import { AdminAuthGuard } from './../_helpers/admin-auth.guard';
import { ProfileComponent } from './profile/profile.component';
import { AccountComponent } from './account/account.component';
import { AuthGuard } from './../_helpers/auth.guard';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { MainComponent } from './main/main.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';

const routes: Routes = [
  { path: '', component: MainComponent, canActivate: [AuthGuard] },
  { path: 'account', component: AccountComponent, canActivate: [AuthGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'admin', component: AdminPanelComponent, canActivate: [AdminAuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
