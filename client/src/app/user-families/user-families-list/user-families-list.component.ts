import { Component, OnInit } from '@angular/core';
import { Family } from 'src/app/_models/family';
import { FamilyParams } from 'src/app/_models/familyParams';
import { Pagination } from 'src/app/_models/pagination';
import { AccountService } from 'src/app/_services/account.service';
import { UserFamiliesService } from 'src/app/_services/user-families.service';

@Component({
  selector: 'app-user-families-list',
  templateUrl: './user-families-list.component.html',
  styleUrls: ['./user-families-list.component.css']
})
export class UserFamiliesListComponent implements OnInit {
  families: Family[] = [];
  pagination: Pagination | undefined;
  familyParams: FamilyParams | undefined;
  addFamilyMode = false;

  constructor(private userFamiliesService: UserFamiliesService, public accountService: AccountService) {
    this.familyParams = this.userFamiliesService.getFamilyParams();
  }

  ngOnInit(): void {
    this.loadUserFamilies();
  }

  loadUserFamilies() {
    if (this.familyParams) {
      this.userFamiliesService.setFamilyParams(this.familyParams);

      this.userFamiliesService.getFamilies(this.familyParams).subscribe({
        next: response => {
          if (response.result && response.pagination) {
            this.families = response.result;
            this.pagination = response.pagination;
          }
        }
      })
    }
  }
  
  pageChanged(event: any) {
    if (this.familyParams && this.familyParams?.pageNumber !== event.page) {
      this.familyParams.pageNumber = event.page;
      this.userFamiliesService.setFamilyParams(this.familyParams);
      this.loadUserFamilies();
    }
  }

  addFamilyToggle() {
    this.addFamilyMode = !this.addFamilyMode;
  }

  cancelAddFamilyMode(event: boolean) {
    this.addFamilyMode = event;
  }
}
