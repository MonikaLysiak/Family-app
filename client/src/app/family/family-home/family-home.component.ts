import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs';
import { Family } from 'src/app/_models/family';
import { AccountService } from 'src/app/_services/account.service';
import {faPeopleRoof} from'@fortawesome/free-solid-svg-icons'
import {faRectangleList} from'@fortawesome/free-solid-svg-icons'
import {faImages} from'@fortawesome/free-solid-svg-icons'
import {faComments} from'@fortawesome/free-solid-svg-icons'
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FamilyService } from 'src/app/_services/family.service';

@Component({
  selector: 'app-family-home',
  templateUrl: './family-home.component.html',
  styleUrls: ['./family-home.component.css']
})
export class FamilyHomeComponent implements OnInit {
  family: Family | null = null;

  faPeopleRoof = faPeopleRoof;
  faRectangleList = faRectangleList;
  faImages = faImages;
  faComments = faComments;
  
  changeNicknameForm: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;

  constructor(private familyService: FamilyService, private route: ActivatedRoute, public accountService: AccountService,  private fb: FormBuilder, private router: Router) {
    this.route.data.subscribe({
      next: data => {
        this.accountService.setCurrentFamily(data['family']);
        this.family = data['family'];
      }
    })
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.changeNicknameForm = this.fb.group({
      nickname: [this.family?.userNickname, [Validators.required]]
    });
  }

  changeNickname() {
    var nickname = this.changeNicknameForm.controls['nickname'].value;
    if (nickname == this.family?.userNickname)
      return;
    this.familyService.changeNickname(nickname).subscribe({
      next: () => {
        if (this.family) {
          this.family.userNickname = nickname;
          this.accountService.setCurrentFamily(this.family);
        }
      },
      error: error => {
        this.validationErrors = error
      }
    })
  }
}
