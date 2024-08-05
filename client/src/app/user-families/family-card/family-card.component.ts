import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Family } from 'src/app/_models/family';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-family-card',
  templateUrl: './family-card.component.html',
  styleUrls: ['./family-card.component.css']
})
export class FamilyCardComponent {
  @Input() family: Family | null = null;

  constructor(private accountService: AccountService, private router: Router) { }

  goToFamily(family: Family) {
    this.accountService.setCurrentFamily(family);
    this.router.navigateByUrl('/families/' + family.id);
  }
}
