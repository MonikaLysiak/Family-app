import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-add-invitation',
  templateUrl: './add-invitation.component.html',
  styleUrls: ['./add-invitation.component.css']
})
export class AddInvitationComponent {
  @Output() cancelAddFamilyMember = new EventEmitter();
  addFamilyMemberForm: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;
  
  constructor(private accountService: AccountService, private fb: FormBuilder, private router: Router) {}
  
  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.addFamilyMemberForm = this.fb.group({
      username: ['', [Validators.required]]
    });
  }
  
  inviteFamilyMember() {
    this.accountService.inviteFamilyMember(this.addFamilyMemberForm.controls['username'].value).subscribe({
      next: () => {
        this.router.navigateByUrl('/familyMembers')
      },
      error: error => {
        this.validationErrors = error
      }
    })
  }

  cancel() {
    this.cancelAddFamilyMember.emit(false);
  }
}
