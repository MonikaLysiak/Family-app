import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ListItem } from 'src/app/_models/list-item';

@Component({
  selector: 'app-edit-shopping-list-modal',
  templateUrl: './edit-shopping-list-modal.component.html',
  styleUrls: ['./edit-shopping-list-modal.component.css']
})
export class EditShoppingListModalComponent {
  title = '';
  message = '';
  btnOkText = '';
  btnCancelText = '';
  result = false;
  editShoppingListForm: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;
  items: ListItem[] = [];
  shoppingListName: string = '';
  shoppingListId: number | null = null;

  constructor(public bsModalRef: BsModalRef, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  save() {
    this.result = true;
    this.bsModalRef.hide();
  }

  cancel() {
    this.bsModalRef.hide();
  }

  initializeForm() {
    this.editShoppingListForm = this.fb.group({
      item: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(20)]]
    });
  }

  addItem() {
    var newItem: ListItem = {} as ListItem;
    newItem.content = this.editShoppingListForm.controls['item'].value;
    newItem.familyListId = this.shoppingListId;
    newItem.isChecked = false;
    this.items.push(newItem);
    this.editShoppingListForm.controls['item'].reset();
  }

  deleteItem(item: ListItem) {
    const index = this.items.indexOf(item);
    if (index > -1) {
      this.items.splice(index, 1);
    }
  }

  checkItem(item: ListItem) {
    const index = this.items.indexOf(item);
    if (index > -1) {
      item.isChecked = !item.isChecked;
      this.items[index] = item;
    }
  }
}
