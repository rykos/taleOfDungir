import { AuthenticationService } from './../../_services/authentication.service';
import { User } from './../../_models/User';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  user: User;

  constructor(private authenticationService: AuthenticationService) {
    this.user = authenticationService.currentUserValue;
    console.log(this.user);
  }

  ngOnInit(): void {
  }

}
