import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ListService {

  constructor(private http: HttpClient) { }

  addShoppingList(shoppingListName: string) {
    //fix it 
    // return this.http.post<ShoppingList>(this.baseUrl + 'family/' + familyName, {}).pipe(
    //   map(shoppingList => {
    //     if (shoppingList) {
    //       this.setCurrentFamily(shoppingList);
    //     }
    //   })
    // )
  }
}
