import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';
import { Family } from './_models/family';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Family app';

  constructor(private accountService: AccountService, public translateService: TranslateService) {
    translateService.addLangs(['en', 'pl']);
    translateService.setDefaultLang('pl');
  }

  ngOnInit(): void {
      this.setCurrentUser();
      this.setCurrentUserFamily();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }

  setCurrentUserFamily() {
    const familyString = localStorage.getItem('family');
    if(!familyString) return;
    const family: Family = JSON.parse(familyString);
    this.accountService.setCurrentFamily(family);
  }
}
