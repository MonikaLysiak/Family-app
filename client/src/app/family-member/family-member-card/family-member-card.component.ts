import { Component, Input } from '@angular/core';
import { FamilyMember } from 'src/app/_models/family-member';
import { PresenceService } from 'src/app/_services/presence.service';
import {faHeart} from'@fortawesome/free-solid-svg-icons'

@Component({
  selector: 'app-family-member-card',
  templateUrl: './family-member-card.component.html',
  styleUrls: ['./family-member-card.component.css']
})
export class FamilyMemberCardComponent {
  @Input() familyMember: FamilyMember | undefined;

  faHeartPulse = faHeart;

  constructor(public presenceService: PresenceService) {}

  ngOnInit(): void { }
}
