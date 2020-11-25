import { AccountService } from './../../_services/account.service';
import { User } from './../../_models/User';
import { AuthenticationService } from './../../_services/authentication.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user: User;
  details: string;
  constructor(private authenticationService: AuthenticationService, private accountService: AccountService) {
    this.user = authenticationService.currentUserValue;
    accountService.Details().subscribe(x => this.details = x);
  }

  ngOnInit(): void {
  }

}
