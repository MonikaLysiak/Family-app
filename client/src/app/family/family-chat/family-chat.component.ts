import { CommonModule } from '@angular/common';
import { Component, Input, OnDestroy, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { take } from 'rxjs';
import { Message } from 'src/app/_models/message';
import { AccountService } from 'src/app/_services/account.service';
import { MessageService } from 'src/app/_services/message.service';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-family-chat',
  standalone: true,
  templateUrl: './family-chat.component.html',
  styleUrls: ['./family-chat.component.css'],
  imports: [CommonModule, TimeagoModule, FormsModule, TranslateModule]
})

export class FamilyChatComponent implements OnDestroy {
  @ViewChild('messageForm') messageForm?: NgForm
  username = '';
  messageContent = '';
  messages: Message[] = [];

  constructor(public messageService: MessageService, public accountService: AccountService) { }

  ngOnInit(): void {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user){
          this.messageService.createHubConnection(user, this.accountService.getCurrentFamilyId());
          this.username = user.username;
        }
      }
    })
  }

  sendMessage() {
    this.messageService.sendMessage(this.accountService.getCurrentFamilyId(), this.messageContent).then(() => {
      this.messageForm?.reset();
    })
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
}
