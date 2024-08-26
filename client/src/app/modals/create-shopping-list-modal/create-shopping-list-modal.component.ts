import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ListService } from 'src/app/_services/list.service';

@Component({
  selector: 'app-create-shopping-list-modal',
  templateUrl: './create-shopping-list-modal.component.html',
  styleUrls: ['./create-shopping-list-modal.component.css']
})
export class CreateShoppingListModalComponent implements OnInit {
  title = '';
  message = '';
  btnOkText = '';
  btnCancelText = '';
  result = false;
  addShoppingListForm: FormGroup = new FormGroup({});
  addItemForm: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;
  items: string[] = [];
  addItemsMode = false;
  shoppingListName: string = '';
  categoryId = 1;

  constructor(public bsModalRef: BsModalRef, private listService: ListService, private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  confirm() {
    this.result = true;
    this.bsModalRef.hide();
  }

  decline() {
    this.bsModalRef.hide();
  }

  initializeForm() {
    this.addShoppingListForm = this.fb.group({
      shoppingListName: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(25)]]
    });
  }

  initializeAddItemsForm() {
    this.addItemForm = this.fb.group({
      item: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]]
    });
  }
  
  addShoppingList() {
    this.shoppingListName = this.addShoppingListForm.controls['shoppingListName'].value;
    this.addItemsMode = true;
    this.initializeAddItemsForm();
  }

  addItem() {
    this.items.push(this.addItemForm.controls['item'].value);
  }

  deleteItem(item: string) {
    const index = this.items.indexOf(item);
    if (index > -1) {
      this.items.splice(index, 1);
    }
  }
}
