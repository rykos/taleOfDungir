import { Character } from './../../_models/Character';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
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
  character: Character;
  constructor(private authenticationService: AuthenticationService, private accountService: AccountService, private httpClient: HttpClient) {
    this.user = authenticationService.currentUserValue;
    accountService.Details().subscribe(x => { this.character = x; });
  }

  ngOnInit(): void {
  }

  ImgLink(imageId: string): string {
    return this.accountService.GetImageLink(imageId);
  }

}
