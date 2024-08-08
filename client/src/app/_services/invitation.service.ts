import { Injectable } from '@angular/core';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { Invitation } from '../_models/invitation';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/user';
import { AccountService } from './account.service';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InvitationService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private accountService: AccountService) { }
  
  getInvitations(predicate: string, pageNumber: number, pageSize: number) {
    let params = getPaginationHeaders(pageNumber, pageSize);

    params = params.append('predicate', predicate);

    return getPaginatedResult<Invitation[]>(this.baseUrl + 'invitations', params, this.http);
  }

  acceptInvitation(invitationsId: number){
    return this.http.post<User>(this.baseUrl + 'invitations/accept/' + invitationsId, {}).pipe(
      map(user => {
        if (user) {
          this.accountService.setCurrentUser(user);
        }
      })
    )
  }

  deleteInvitation(invitationsId: number){
    return this.http.post<User>(this.baseUrl + 'invitations/delete/' + invitationsId, {});
  }
}
