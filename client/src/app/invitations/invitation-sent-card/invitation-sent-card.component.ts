import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Invitation } from 'src/app/_models/invitation';
import { InvitationService } from 'src/app/_services/invitation.service';

@Component({
  selector: 'app-invitation-sent-card',
  templateUrl: './invitation-sent-card.component.html',
  styleUrls: ['./invitation-sent-card.component.css']
})
export class InvitationSentCardComponent {
  @Input() invitation: Invitation | undefined;

  constructor(private invitationService: InvitationService, private toastr: ToastrService) {}

  deleteInvitation() {
    if (!this.invitation) return;
    this.invitationService.deleteInvitation(this.invitation.id).subscribe({
      error: error => this.toastr.error(error)
    })
  }
}
