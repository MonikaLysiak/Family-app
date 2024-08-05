import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-add-family',
  templateUrl: './add-family.component.html',
  styleUrls: ['./add-family.component.css']
})
export class AddFamilyComponent {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;
  
  constructor(private accountService: AccountService, private fb: FormBuilder, private router: Router) {}
  
  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      familyName: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]]
    });
  }
  
  addFamily() {
    this.accountService.addFamily(this.registerForm.controls['familyName'].value).subscribe({
      next: () => {
        this.router.navigateByUrl('/familyMembers')
      },
      error: error => {
        this.validationErrors = error
      }
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
