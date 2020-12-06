import { AccountService } from './../_services/account.service';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public static title = 'Tale of Dungir';
  public constructor(private accountService: AccountService) { 
    accountService.UpdateMissions();
  }
}