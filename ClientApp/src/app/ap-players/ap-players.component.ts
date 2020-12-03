import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-ap-players',
  templateUrl: './ap-players.component.html',
  styleUrls: ['./ap-players.component.css']
})
export class ApPlayersComponent implements OnInit {
  registeredPlayers: number;
  foundPlayers: any[] = [];
  // createForm: FormGroup;

  constructor(private httpClient: HttpClient) {
    // this.createForm = this.formBuilder.group({
    //   itemType: ['Trash', Validators.required],
    //   itemName: ['', [Validators.required, Validators.minLength(3)]]
    // });
  }

  ngOnInit(): void {
    this.httpClient.get<any[]>(`${environment.apiUrl}/admin/players`).subscribe(x => {
      this.foundPlayers = x;
    });
    this.httpClient.get<number>(`${environment.apiUrl}/admin/players/amount`).subscribe(x => {
      this.registeredPlayers = x;
    });
  }                                           

  SearchBoxChanged(value) {
    let pattern = value ? `?pattern=${value}` : '';
    this.httpClient.get<any[]>(`${environment.apiUrl}/admin/players${pattern}`).subscribe(x => {
      this.foundPlayers = x;
    });
  }

}
