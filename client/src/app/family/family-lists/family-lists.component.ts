import { Component } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from 'src/app/_models/pagination';
import { FamilyService } from 'src/app/_services/family.service';
import { CreateShoppingListModalComponent } from 'src/app/modals/create-shopping-list-modal/create-shopping-list-modal.component';

@Component({
  selector: 'app-family-lists',
  templateUrl: './family-lists.component.html',
  styleUrls: ['./family-lists.component.css']
})
export class FamilyListsComponent {
  familyLists: any[] = [];
  pagination?: Pagination;
  pageNumber = 1;
  pageSize = 5;
  orderBy = 'created'
  loading = false;
  bsModalRef: BsModalRef<CreateShoppingListModalComponent> = new BsModalRef<CreateShoppingListModalComponent>();
  
  constructor(private familyService: FamilyService, private modalService: BsModalService, private toastr: ToastrService) {}

  loadFamilyLists() {
    // this.loading = true
    // this.familyService.getFamilyLists(this.pageNumber, this.pageSize, this.orderBy).subscribe({
    //   next: response => {
    //     if (!response.result) return;
    //     this.familyLists = response.result;
    //     this.pagination = response.pagination;
    //     this.loading = false;
    //   }
    // })
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadFamilyLists();
    }
  }

  createList() {
    this.openRolesModal();
  }

  openRolesModal() {
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        btnOkText: 'Save shoping list items',
        btnCancelText: 'Cancel'
      }
    }
    this.bsModalRef = this.modalService.show(CreateShoppingListModalComponent, config);
    this.bsModalRef.onHide?.subscribe({
      next: () => {
        const items = this.bsModalRef.content?.items;
        if (items && items.length === 0) {
          this.familyService.updateFamilyList(items!);
        }
      }
    })
  }
}
