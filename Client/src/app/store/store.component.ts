import { Component, OnInit } from '@angular/core';
import { StoreService } from './store.service';
import { IProduct } from '../shared/models/product';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrl: './store.component.css'
})
export class StoreComponent implements OnInit{
  products: IProduct[] = [];

  constructor(private storeService: StoreService) {}

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts() {
    this.storeService.getProducts().subscribe({
      next: (response) => this.products = response.data,
      error: (error) => console.log(error)
    });
  }
}
