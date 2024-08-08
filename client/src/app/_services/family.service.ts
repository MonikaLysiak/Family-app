import { Injectable } from '@angular/core';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { environment } from 'src/environments/environment';
import { FamilyMember } from '../_models/family-member';
import { HttpClient } from '@angular/common/http';
import { AccountService } from './account.service';
import { take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FamilyService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private accountService: AccountService) { }

  getFamilyMembers(pageNumber: number, pageSize: number, orderBy: string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('OrderBy', orderBy);
    params = params.append('FamilyId', this.accountService.getCurrentFamilyId());
    return getPaginatedResult<FamilyMember[]>(this.baseUrl + 'family/members', params, this.http);
  }
  
  setMainPhoto(familyId: number, photoId: number) {
    return this.http.put(this.baseUrl + 'family/set-main-photo/' + familyId + '/' + photoId, {});
  }

  deletePhoto(familyId: number, photoId: number) {
    return this.http.delete(this.baseUrl + 'family/delete-photo/' + familyId + '/' + photoId);
  }
}
