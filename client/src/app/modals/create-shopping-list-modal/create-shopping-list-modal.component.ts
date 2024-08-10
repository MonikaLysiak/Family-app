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
  items: any[] = [];
  addItemsMode = false;

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
      shoppingListName: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]]
    });
  }

  initializeAddItemsForm() {
    this.addItemForm = this.fb.group({
      item: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]]
    });
  }
  
  addShoppingList() {
    this.listService.addShoppingList(this.addShoppingListForm.controls['shoppingListName'].value);
    this.addItemsMode = true;
    this.initializeAddItemsForm();
    // .subscribe({
    //   next: () => {
    //     this.router.navigateByUrl('/familyMembers')
    //   },
    //   error: error => {
    //     this.validationErrors = error
    //   }
    // })
  }

  addItem() {
    this.items.push(this.addItemForm.controls['item'].value);
  }
}
