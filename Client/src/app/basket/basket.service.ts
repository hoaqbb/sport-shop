import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { BehaviorSubject } from 'rxjs';
import { Basket, IBasket, IBasketItem, IBasketTotal } from '../shared/models/basket';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<Basket | null>(null);
  basketSource$ = this.basketSource.asObservable();
  private basketTotal = new BehaviorSubject<IBasketTotal | null>(null);
  basketTotal$ = this.basketTotal.asObservable();

  constructor(private http: HttpClient) { }
  
  getBasket(username: string) {
    return this.http.get<IBasket>(this.baseUrl + "Basket/GetBasket/" + username).subscribe({
      next: basket => {
        this.basketSource.next(basket);
        this.calculateBasketTotal();
      }
    });
  }

  setBasket(basket: IBasket) {
    return this.http.post<IBasket>(this.baseUrl + "Basket/CreateBasket", basket).subscribe({
      next: basket => {
        this.basketSource.next(basket);
        this.calculateBasketTotal();
      }
    });
  }
  
  getCurrentBasket() {
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item);
    const basket = this.getCurrentBasket() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  incrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasket();
    if(!basket) return;
    const itemIndex = basket.items.findIndex(x => x.productId == item.productId);
    basket.items[itemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasket();
    if(!basket) return;
    const itemIndex = basket.items.findIndex(x => x.productId == item.productId);
    if(basket.items[itemIndex].quantity > 1) {
      basket.items[itemIndex].quantity--;
      this.setBasket(basket);
    } else {
      this.removeItemFromBasket(item);
    }
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasket();
    if(!basket) return;
    if(basket.items.some(x => x.productId == item.productId)) {
      basket.items = basket.items.filter(x => x.productId != item.productId);
      if(basket.items.length > 0) {
        this.setBasket(basket);
      } else {
        this.deleteBasket(basket.userName);
      }
    }
    const itemIndex = basket.items.findIndex(x => x.productId == item.productId);
    basket.items[itemIndex].quantity++;
    this.setBasket(basket);
  }

  deleteBasket(userName: string) {
    return this.http.delete(this.baseUrl + 'Basket/DeleteBasket/' + userName).subscribe({
      next: (response) => {
        this.basketSource.next(null);
        this.basketTotal.next(null);
        localStorage.removeItem('basket_username');
      },
      error: (err) => {
        console.log('Error while deleting basket');
        console.log(err);
      }
    });
  }

  addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const item = items.find(x => x.productId == itemToAdd.productId);
    if(item) {
      item.quantity += quantity;
    } else {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }

    return items;
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem("basket_username", "hoang");
    return basket;
  }

  private mapProductItemToBasketItem(item: IProduct): IBasketItem {
    return {
      productId: item.id,
      productName: item.name,
      price: item.price,
      imageFile: item.imageFile,
      quantity: 0
    }
  }

  private calculateBasketTotal() {
    const basket = this.getCurrentBasket();
    if(!basket) return;

    const total = basket.items.reduce((total, item) => (item.price * item.quantity) + total, 0);
    this.basketTotal.next({total});
  }
}
