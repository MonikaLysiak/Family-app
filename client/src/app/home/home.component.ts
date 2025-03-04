import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthStatus } from '../_enums/auth-status';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {
  registerMode = false;
  emailSend = false;
  authStatus = AuthStatus;
  currentStatus: AuthStatus = AuthStatus.NotLoggedIn;

  constructor(public accountService: AccountService, private route: ActivatedRoute) {
    this.accountService.currentAuthStatus$.subscribe(status => {
      this.currentStatus = status;
    });
  }

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => {
        if(data['confirmEmail'])
          this.accountService.confirmEmail(this.route.queryParams);
      }
    });
  }

  register() {
    this.registerMode = true;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
    this.accountService.setCurrentAuthStatus(AuthStatus.NotLoggedIn);
  }

  showRegistrationSuccess() {
    this.registerMode = false;
    this.accountService.setCurrentAuthStatus(AuthStatus.EmailConfirmationSent);
  }

  resendEmail() {
    this.accountService.resendEmail().subscribe({
    });
  }

  backToHome() {
    this.registerMode = false;
    this.accountService.setCurrentAuthStatus(AuthStatus.NotLoggedIn);
  }
}
