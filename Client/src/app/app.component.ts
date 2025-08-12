import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  title = 'Sport Shop';

  constructor(private basketService: BasketService) {}

  ngOnInit(): void {
    const basket_username = localStorage.getItem('basket_username');
    if (basket_username) {
      this.basketService.getBasket(basket_username);
    }
  }
}
