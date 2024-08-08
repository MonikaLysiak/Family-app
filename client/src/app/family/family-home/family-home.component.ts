import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { take } from 'rxjs';
import { Family } from 'src/app/_models/family';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-family-home',
  templateUrl: './family-home.component.html',
  styleUrls: ['./family-home.component.css']
})
export class FamilyHomeComponent implements OnInit {
  family: Family | null = null;
  constructor(private route: ActivatedRoute, private accountService: AccountService, ) {
    this.route.data.subscribe({
      next: data => {
        this.accountService.setCurrentFamily(data['family']);
      }
    })
  }

  ngOnInit(): void {
    this.accountService.currentFamily$.pipe(take(1)).subscribe({
      next: family => this.family = family
    });
  }
}
