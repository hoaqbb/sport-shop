import { Component } from '@angular/core';
import { BasketService } from '../../basket/basket.service';
import { IBasketItem } from '../../shared/models/basket';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.css'
})
export class NavBarComponent {

  constructor(public basketService: BasketService) {}

  getBasketCount(items: IBasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }
}
