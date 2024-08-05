import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { Member } from 'src/app/_models/member';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';
import { PresenceService } from 'src/app/_services/presence.service';
import { AccountService } from 'src/app/_services/account.service';
import { take } from 'rxjs';
import { User } from 'src/app/_models/user';
import { MemberMessagesComponent } from 'src/app/members/member-messages/member-messages.component';
import { FamilyMember } from 'src/app/_models/family-member';

@Component({
  selector: 'app-family-member-details',
  templateUrl: './family-member-details.component.html',
  styleUrls: ['./family-member-details.component.css'],
  standalone: true,
  imports: [CommonModule, TabsModule, GalleryModule, TimeagoModule, MemberMessagesComponent]
})
export class FamilyMemberDetailsComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent;
  familyMember: FamilyMember = {} as FamilyMember;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  messages: Message[] = [];
  user?: User;

  constructor(private accountService: AccountService, private route: ActivatedRoute, private messageService: MessageService, public presenceService: PresenceService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user;
      }
    })
  }

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => this.familyMember = data['familyMember']
    })
    
    this.getImages();
  }
  
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      this.memberTabs.tabs.find(x => x.heading === heading)!.active = true;
    }
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.user) {
      this.messageService.createHubConnection(this.user, this.familyMember.userName)
    } else {
      this.messageService.stopHubConnection();
    }
  }

  getImages() {
    if (!this.familyMember) return;
    for (const photo of this.familyMember?.photos) {
      this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
    }
  }

  loadMessages() {
    if (this.familyMember) {
      this.messageService.getMessageThread(this.familyMember.userName).subscribe({
        next: messages => this.messages = messages
      })
    }
  }
}
