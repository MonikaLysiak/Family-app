import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FamilyList } from '../_models/family-list';
import { environment } from 'src/environments/environment';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { ListItem } from '../_models/list-item';

@Injectable({
  providedIn: 'root'
})
export class ListService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addShoppingList(familyId : number, name: string, categoryId: number, listItems: string[]) {
    const requestBody = {
      familyId: familyId,
      name: name,
      categoryId: categoryId,
      listItems: listItems
    };
    return this.http.post<FamilyList>(this.baseUrl + 'lists', requestBody).subscribe(r=>{});
  }

  getFamilyLists(familyId: number, pageNumber: number, pageSize: number, orderBy: string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('OrderBy', orderBy);
    params = params.append('FamilyId', familyId);
    return getPaginatedResult<FamilyList[]>(this.baseUrl + 'lists', params, this.http);
  }

  editList(listId: number, listItems: ListItem[]){
    const requestBody = {
      id: listId,
      listItems: listItems
    };
    return this.http.post<FamilyList>(this.baseUrl + 'lists/edit', requestBody).subscribe(r=>{});
  }
}
