import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { Login} from '../_models/login';
import { AuthStatus } from '../_enums/auth-status';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
  model: Login = {} as Login;
  authStatus = AuthStatus;
  currentStatus: AuthStatus = AuthStatus.NotLoggedIn;

  constructor(public accountService: AccountService, private router: Router) {
    this.accountService.currentAuthStatus$.subscribe(status => {
      this.currentStatus = status;
    });
  }

  ngOnInit(): void {
  }
 
  showLogin(): boolean {
    return this.currentStatus != AuthStatus.LoggedIn && this.currentStatus != AuthStatus.TwoFactorRequired && this.currentStatus != AuthStatus.InvalidTwoFactorCode;
  }

  showTwoFactorLogin(): boolean {
    return this.currentStatus === AuthStatus.TwoFactorRequired || this.currentStatus === AuthStatus.InvalidTwoFactorCode;
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => {
        this.router.navigateByUrl('');
        this.model = {} as Login;
      }
    });
  }

  twoFactorLogin() {
    this.accountService.twoFactorLogin(this.model).subscribe({
      next: _ => {
        this.router.navigateByUrl('');
        this.model = {} as Login;
      }
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

  switchLang(lang: string) {
    this.accountService.setCurrentLanguage(lang);
  }
}
