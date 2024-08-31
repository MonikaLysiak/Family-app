import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, take } from 'rxjs';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { PresenceService } from './presence.service';
import { Family } from '../_models/family';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  private currentFamilySource = new BehaviorSubject<Family | null>(null);
  currentFamily$ = this.currentFamilySource.asObservable();

  private currentLanguageSource = new BehaviorSubject<string | null>(null);
  currentLanguage$ = this.currentLanguageSource.asObservable();

  constructor(private http: HttpClient, private presenceService: PresenceService, public translateService: TranslateService) { }
  
  setCurrentLanguage(language: string){
    this.translateService.use(language);
    this.currentLanguageSource.next(language);
    localStorage.setItem('language', JSON.stringify(language));
  }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    )
  }

  setCurrentUser(user: User){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user))
    this.currentUserSource.next(user);
    this.presenceService.createHubConnection(user);
  }
  
  getCurrentUser() : User | undefined{
    var user;
    this.currentUser$.pipe(take(1)).subscribe(user => 
      {
        if (user)
          user = user;
      });
    return user;
  }

  // check if cant be , model, and then not from Query??
  // check the options and how to best recieve data in controller
  addFamily(familyName: string) {
    return this.http.post<Family>(this.baseUrl + 'family/' + familyName, {}).pipe(
      map(family => {
        if (family) {
          this.setCurrentFamily(family);
        }
      })
    )
  }

  //why this way? should i chnge it to simple property??
  setCurrentFamily(family: Family | null){
    localStorage.setItem('family', JSON.stringify(family))
    this.currentFamilySource.next(family);
  }

  getCurrentFamilyId() : number{
    var familyId = -1;
    this.currentFamily$.pipe(take(1)).subscribe(family => 
      {
        if (family)
          familyId = family?.id
      });
    return familyId;
  }

  getCurrentFamily() : Family | undefined{
    var family;
    this.currentFamily$.pipe(take(1)).subscribe(family => 
      {
        if (family)
          family = family;
      });
    return family;
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    localStorage.removeItem('family');
    this.currentFamilySource.next(null);
    this.presenceService.stopHubConnection();
  }

  getDecodedToken(token: string){
    return JSON.parse(atob(token.split('.')[1]))
  }
  
  inviteFamilyMember(username: string) {
    return this.http.post<Family>(this.baseUrl + 'invitations/' + username + '/' + this.getCurrentFamilyId(), {}).pipe(
      map(family => {
        if (family) {
          this.setCurrentFamily(family);
        }
      })
    )
  }
}
