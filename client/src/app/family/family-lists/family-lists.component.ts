import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from 'src/app/_models/pagination';
import { AccountService } from 'src/app/_services/account.service';
import { ListService } from 'src/app/_services/list.service';
import { CreateShoppingListModalComponent } from 'src/app/modals/create-shopping-list-modal/create-shopping-list-modal.component';

@Component({
  selector: 'app-family-lists',
  templateUrl: './family-lists.component.html',
  styleUrls: ['./family-lists.component.css']
})
export class FamilyListsComponent implements OnInit {
  familyLists: any[] = [];
  pagination?: Pagination;
  pageNumber = 1;
  pageSize = 5;
  orderBy = 'created'
  loading = false;
  bsModalRef: BsModalRef<CreateShoppingListModalComponent> = new BsModalRef<CreateShoppingListModalComponent>();
  
  constructor(private accountService: AccountService, private listService: ListService, private modalService: BsModalService, private toastr: ToastrService) {}
  
  ngOnInit(): void {
    this.loadFamilyLists();
  }

  loadFamilyLists() {
    this.loading = true
    this.listService.getFamilyLists(this.accountService.getCurrentFamilyId(), this.pageNumber, this.pageSize, this.orderBy).subscribe({
      next: response => {
        if (!response.result) return;
        this.familyLists = response.result;
        this.pagination = response.pagination;
        this.loading = false;
      }
    })
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
        const shoppingListName = this.bsModalRef.content?.shoppingListName;
        const categoryId = this.bsModalRef.content?.categoryId;
        const items = this.bsModalRef.content?.items;
        
        if (shoppingListName && categoryId && items) {
          this.listService.addShoppingList(this.accountService.getCurrentFamilyId(), shoppingListName, categoryId, items);
        }
      }
    })
  }
}
