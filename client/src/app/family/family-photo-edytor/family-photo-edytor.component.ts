import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { take } from 'rxjs';
import { Family } from 'src/app/_models/family';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { FamilyService } from 'src/app/_services/family.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-family-photo-edytor',
  templateUrl: './family-photo-edytor.component.html',
  styleUrls: ['./family-photo-edytor.component.css'],
  standalone: true,
  imports: [CommonModule, FileUploadModule]
})
export class FamilyPhotoEdytorComponent {
  @Input() family: Family | undefined;
  uploader: FileUploader | undefined;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  user: User | undefined;

  constructor(private accountService: AccountService, private familyService: FamilyService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user)
          this.user = user;
      }
    });
   }

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  setMainPhoto(photo: Photo) {
    if (!this.family) return;
    this.familyService.setMainPhoto(this.family.id, photo.id).subscribe({
      next: () => {
        if (this.family) {
          this.family.photoUrl = photo.url;
          this.family.familyPhotos.forEach(x => {
            if (x.isMain) x.isMain = false;
            if (photo.id === photo.id) photo.isMain = true;
          })
          this.accountService.setCurrentFamily(this.family);
        }
      }
    })
  }

  // maby change family in account service to not have photos
  // and then have there currentFamily and here use both currentFamily and family
  // then updating currentFamily only when changing main photo

  deletePhoto(photoId: number) {
    if (!this.family) return;
    this.familyService.deletePhoto(this.family.id, photoId).subscribe({
      next: _ => {
        if (this.family) {
          this.family.familyPhotos = this.family.familyPhotos.filter(x => x.id !== photoId);
          this.accountService.setCurrentFamily(this.family);
        }
      }
    })
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'family/add-photo/' + this.family?.id,
      authToken: 'Bearer ' + this.user?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response){
        const photo = JSON.parse(response);
        if (!this.family) return;
        this.family.familyPhotos.push(photo);
        if(photo.isMain) {
          this.family.photoUrl = photo.url;
        }
        this.accountService.setCurrentFamily(this.family);
      }
    }
  }
}
