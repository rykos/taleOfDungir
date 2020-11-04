import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/_models/User';
import { AuthenticationService } from 'src/_services/authentication.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent {
  currentUser: User;

  constructor(private router: Router, private authenticationService: AuthenticationService) {
    this.authenticationService.currentUser.subscribe(u => { this.currentUser = u; });
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }
}
