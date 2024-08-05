import { Component } from '@angular/core';
import { FamilyMember } from 'src/app/_models/family-member';
import { Pagination } from 'src/app/_models/pagination';
import { FamilyService } from 'src/app/_services/family.service';

@Component({
  selector: 'app-family-member-list',
  templateUrl: './family-member-list.component.html',
  styleUrls: ['./family-member-list.component.css']
})
export class FamilyMemberListComponent {
  familyMembers?: FamilyMember[];
  pagination?: Pagination;
  pageNumber = 1;
  pageSize = 5;
  orderBy = 'created'
  loading = false;

  constructor(private familyService: FamilyService) {}

  ngOnInit(): void {
    this.loadFamilyMembers();
  }

  loadFamilyMembers() {
    this.loading = true
    this.familyService.getFamilyMembers(this.pageNumber, this.pageSize, this.orderBy).subscribe({
      next: response => {
        this.familyMembers = response.result;
        this.pagination = response.pagination;
        this.loading = false;
      }
    })
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadFamilyMembers();
    }
  }
}
