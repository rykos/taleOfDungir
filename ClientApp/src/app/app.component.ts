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
}
