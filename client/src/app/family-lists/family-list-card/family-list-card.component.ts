import { Component, Input } from '@angular/core';
import { FamilyList } from 'src/app/_models/family-list';

@Component({
  selector: 'app-family-list-card',
  templateUrl: './family-list-card.component.html',
  styleUrls: ['./family-list-card.component.css']
})
export class FamilyListCardComponent {
  @Input() familyList: FamilyList | undefined;

}
