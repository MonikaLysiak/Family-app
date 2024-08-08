import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Invitation } from 'src/app/_models/invitation';
import { InvitationService } from 'src/app/_services/invitation.service';

@Component({
  selector: 'app-invitation-received-card',
  templateUrl: './invitation-received-card.component.html',
  styleUrls: ['./invitation-received-card.component.css']
})
export class InvitationReceivedCardComponent {
  @Input() invitation: Invitation | undefined;

  constructor(private invitationService: InvitationService, private router: Router, private toastr: ToastrService) {}

  ngOnInit(): void { }

  acceptInvitation() {
    if (!this.invitation) return;
    this.invitationService.acceptInvitation(this.invitation.id).subscribe({
      next: () => this.router.navigateByUrl('/families'),
      error: error => this.toastr.error(error)
    })
  }

  deleteInvitation() {
    if (!this.invitation) return;
    this.invitationService.deleteInvitation(this.invitation.id).subscribe({
      error: error => this.toastr.error(error)
    })
  }
}
