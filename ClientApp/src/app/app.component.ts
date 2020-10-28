import { AuthenticationService } from './../_services/authentication.service';
import { User } from './../_models/User';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Tale of Dungir';
  currentUser: User;

  constructor(private router: Router, private authenticationService: AuthenticationService) {
    this.authenticationService.currentUser.subscribe(u => { this.currentUser = u; });
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }
}
