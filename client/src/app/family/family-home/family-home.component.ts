import { Component } from '@angular/core';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-family-home',
  templateUrl: './family-home.component.html',
  styleUrls: ['./family-home.component.css']
})
export class FamilyHomeComponent {

  constructor(public accountService: AccountService) {}

}
