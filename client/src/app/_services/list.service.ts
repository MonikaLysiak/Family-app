import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FamilyList } from '../_models/family-list';
import { environment } from 'src/environments/environment';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { ListItem } from '../_models/list-item';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class ListService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private listThreadSource = new BehaviorSubject<FamilyList[]>([]);
  listThread$ = this.listThreadSource.asObservable();

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

  // deleteList(id: number){
  //   return this.http.delete(this.baseUrl + 'lists/' + id).subscribe(r=>{});
  // }
  
  createHubConnection(user: User, familyId: number){
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'list?familyId=' + familyId, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('ReceiveListThread', messages => {
      this.listThreadSource.next(messages);
    })

    this.hubConnection.on('NewList', list => {
      this.listThread$.pipe(take(1)).subscribe({
        next: lists => {
          this.listThreadSource.next([...lists, list])
        }
      })
    })

    this.hubConnection.on('NewListItem', listItem => {
      if (!listItem.familyListId) return;
      this.listThread$.pipe(take(1)).subscribe({
        next: lists => {
          lists.forEach(x => {
            if(x.familyId == listItem.familyListId)
              x.listItems.push(listItem);
          });
          this.listThreadSource.next(lists);
        }
      })
    })

    this.hubConnection.on('ListItemChecked', listItem => {
      if (!listItem.familyListId) return;
      this.listThread$.pipe(take(1)).subscribe({
        next: lists => {
          lists.forEach(x => {
            if(x.familyId == listItem.familyListId){
              x.listItems.forEach(y => {
                // to be fixed must not update all that match content should not go through all items make it stop after finding the first one
                if(y.content == listItem.content){
                  y.isChecked = listItem.isChecked;
                }
              });
            }
          });
          this.listThreadSource.next(lists);
        }
      })
    })
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  getListThread(familyId: number) {
    return this.http.get<FamilyList[]>(this.baseUrl + 'lists/thread/' + familyId);
  }

  async addList(familyId : number, name: string, categoryId: number, listItems: string[]) {
    return this.hubConnection?.invoke('AddList', {
      familyId: familyId,
      name: name,
      categoryId: categoryId,
      listItems: listItems
    }).catch(error => console.log(error));
  }

  deleteList(id: number) {
    return this.http.delete(this.baseUrl + 'lists/' + id);
  }

  async addListItem(listId : number, item: string) {
    return this.hubConnection?.invoke('AddListItem', {
      listId: listId,
      item: item
    }).catch(error => console.log(error));
  }

  async checkListItem(listId : number, item: string) {
    return this.hubConnection?.invoke('CheckListItem', {
      listId: listId,
      item: item
    }).catch(error => console.log(error));
  }
  
  async deleteListItem(listId : number, item: string) {
    return this.hubConnection?.invoke('DeleteListItem', {
      listId: listId,
      item: item
    }).catch(error => console.log(error));
  }
}
