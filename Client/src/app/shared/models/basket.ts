export interface IBasket {
  userName: string;
  items: IBasketItem[];
  totalPrice: number;
}

export interface IBasketItem {
  quantity: number;
  imageFile: string;
  price: number;
  productId: string;
  productName: string;
}

export class Basket implements IBasket {
  userName: string = 'hoang';
  totalPrice: number = 0;
  items: IBasketItem[] = [];
}

export interface IBasketTotal {
  total: number;
}