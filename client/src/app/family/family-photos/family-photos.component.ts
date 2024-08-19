import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { Family } from 'src/app/_models/family';
import { AccountService } from 'src/app/_services/account.service';
import { FamilyPhotoEdytorComponent } from '../family-photo-edytor/family-photo-edytor.component';
import { take } from 'rxjs';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-family-photos',
  templateUrl: './family-photos.component.html',
  styleUrls: ['./family-photos.component.css'],
  standalone: true,
  imports: [CommonModule, GalleryModule, FamilyPhotoEdytorComponent, TabsModule, TranslateModule]
})
export class FamilyPhotosComponent {
  images: GalleryItem[] = [];
  family: Family = {} as Family;

  constructor(private accountService: AccountService) {
    this.accountService.currentFamily$.pipe(take(1)).subscribe({
      next: family => {
        if (family)
          this.family = family
      }
    });
   }

  ngOnInit(): void {
    this.getImages();
  }

  getImages() {
    if (!this.family) return;
    for (const photo of this.family?.familyPhotos) {
      this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
    }
  }
}
