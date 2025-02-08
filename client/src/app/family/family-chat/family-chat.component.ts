import { CommonModule } from '@angular/common';
import { Component, OnDestroy, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
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

export class FamilyChatComponent implements OnDestroy, AfterViewChecked {
  @ViewChild('messageForm') messageForm?: NgForm
  @ViewChild('scrollMe') private chatContainer!: ElementRef;
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
  
  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  private scrollToBottom(): void {
    try {
      this.chatContainer.nativeElement.scrollTop = this.chatContainer.nativeElement.scrollHeight;
    } catch (err) {
      console.log('Scroll error:', err);
    }
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
