import { Component, OnInit } from '@angular/core';
import { Invitation } from 'src/app/_models/invitation';
import { Pagination } from 'src/app/_models/pagination';
import { InvitationService } from 'src/app/_services/invitation.service';

@Component({
  selector: 'app-invitations',
  templateUrl: './invitations.component.html',
  styleUrls: ['./invitations.component.css']
})
export class InvitationsComponent implements OnInit {
  invitations: Invitation[] | undefined;
  predicate = 'received';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination | undefined;

  constructor(private invitationService: InvitationService) { }

  ngOnInit(): void {
    this.loadInvitations();
  }

  loadInvitations() {
    this.invitationService.getInvitations(this.predicate, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.invitations = response.result;
        this.pagination = response.pagination;
      }
    })
  }
  
  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadInvitations();
    }
  }
}
