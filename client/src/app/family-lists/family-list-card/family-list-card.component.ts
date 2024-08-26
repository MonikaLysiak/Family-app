import { Component, Input } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FamilyList } from 'src/app/_models/family-list';
import { ListService } from 'src/app/_services/list.service';
import { EditShoppingListModalComponent } from 'src/app/modals/edit-shopping-list-modal/edit-shopping-list-modal.component';

@Component({
  selector: 'app-family-list-card',
  templateUrl: './family-list-card.component.html',
  styleUrls: ['./family-list-card.component.css']
})
export class FamilyListCardComponent {
  @Input() familyList: FamilyList | undefined;

  bsModalRef: BsModalRef<EditShoppingListModalComponent> = new BsModalRef<EditShoppingListModalComponent>();

  constructor(private listService: ListService, private modalService: BsModalService) { }

  editList() {
    if (!this.familyList?.id) return;
    this.openEditListModal();
  }

  openEditListModal() {
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
