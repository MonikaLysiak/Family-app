import { Injectable } from '@angular/core';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { environment } from 'src/environments/environment';
import { Family } from '../_models/family';
import { User } from '../_models/user';
import { FamilyParams } from '../_models/familyParams';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AccountService } from './account.service';
import { map, of, take } from 'rxjs';
import { FamilyMember } from '../_models/family-member';

@Injectable({
  providedIn: 'root'
})
export class UserFamiliesService {
  baseUrl = environment.apiUrl;
  families: Family[] = [];
  familyCache = new Map();
  user: User | undefined;
  familyParams: FamilyParams | undefined;

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.familyParams = new FamilyParams();
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.user = user;
        }
      }
    })
  }

  getFamilies(familyParams: FamilyParams) {
    const response = this.familyCache.get(Object.values(familyParams).join('-'));

    if (response) return of(response);

    let params = getPaginationHeaders(familyParams.pageNumber, familyParams.pageSize);

    return getPaginatedResult<Family[]>(this.baseUrl + 'family', params, this.http).pipe(
      map(response => {
        this.familyCache.set(Object.values(familyParams).join('-'), response);
        return response;
      })
    );
  }

  getFamilyParams() {
    return this.familyParams;
  }

  setFamilyParams(params: FamilyParams) {
    this.familyParams = params;
  }

  resetFamilyParams() {
    if (this.user) {
      this.familyParams = new FamilyParams();
      return this.familyParams;
    }
    return;
  }

  getFamily(familyId: number) {
    return this.http.get<Family>(this.baseUrl + 'family/' + familyId);
  }

  getFamilyMemberDetails(familyMemberId: string) {
    var familyId = this.accountService.getCurrentFamilyId();

    return this.http.get<FamilyMember>(this.baseUrl + 'family/' + familyId  + '/' + familyMemberId);
  }
}
