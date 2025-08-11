import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class StoreService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getProducts() {
    return this.http.get<IPagination<IProduct[]>>(this.baseUrl + "Catalog/GetAllProducts");
  }
}
