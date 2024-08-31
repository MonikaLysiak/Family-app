import { Component, Input, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FamilyList } from 'src/app/_models/family-list';
import { ListItem } from 'src/app/_models/list-item';
import { ListService } from 'src/app/_services/list.service';
import { EditShoppingListModalComponent } from 'src/app/modals/edit-shopping-list-modal/edit-shopping-list-modal.component';

@Component({
  selector: 'app-family-list-card',
  templateUrl: './family-list-card.component.html',
  styleUrls: ['./family-list-card.component.css']
})
export class FamilyListCardComponent implements OnInit{
  @Input() familyList: FamilyList | undefined;
  displayedItems: ListItem[] = [];

  bsModalRef: BsModalRef<EditShoppingListModalComponent> = new BsModalRef<EditShoppingListModalComponent>();

  constructor(private listService: ListService, private modalService: BsModalService) { }

  ngOnInit(): void {
    if (this.familyList?.listItems)
      this.displayedItems = this.familyList.listItems.slice(0, 5);
  }

  editList() {
    if (!this.familyList?.id) return;
    this.openEditListModal();
  }

  deleteList() {
    if (!this.familyList?.id) return;
    this.listService.deleteList(this.familyList.id);
  }

  private openEditListModal() {
    if (!this.familyList?.id) return;
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        btnOkText: 'Zapisz',
        btnCancelText: 'Anuluj',
        shoppingListId: this.familyList.id,
        items: this.familyList.listItems
      }
    }
    this.bsModalRef = this.modalService.show(EditShoppingListModalComponent, config);
    this.bsModalRef.onHide?.subscribe({
      next: () => {
        const items = this.bsModalRef.content?.items;
        const shoppingListId = this.bsModalRef.content?.shoppingListId;
        
        if (items && shoppingListId) {
          this.listService.editList(shoppingListId, items);
        }
      }
    })
  }
}
