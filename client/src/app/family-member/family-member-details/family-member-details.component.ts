import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
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

export class FamilyMemberDetailsComponent implements OnInit {
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent;
  familyMember: FamilyMember = {} as FamilyMember;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  messages: Message[] = [];
  user?: User;

  constructor(private accountService: AccountService, private route: ActivatedRoute, public presenceService: PresenceService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user;
      }
    })
  }

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => this.familyMember = data['familyMember']
    });
    this.getImages();
  }

  getImages() {
    if (!this.familyMember) return;
    for (const photo of this.familyMember?.userPhotos) {
      this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
    }
  }
}
