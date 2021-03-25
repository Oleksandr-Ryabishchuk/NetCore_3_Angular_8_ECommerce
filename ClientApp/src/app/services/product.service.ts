import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../interfaces/product';
import { shareReplay, flatMap, first } from 'rxjs/operators';
import { CartItem } from '../interfaces/CartItem';
import { Order } from '../interfaces/order';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http: HttpClient) { }

  private getProductsUrl = '/api/product/getallproducts';

  private addProductUrl = '/api/product/addproduct';

  private deleteProductUrl = '/api/product/deleteproduct/';

  private updateProductUrl = '/api/product/updateproduct/';

  private product$: Observable<Product[]>;

  private products: Product[];

  getProducts(): Observable<Product[]> {
    if (!this.product$) {
      this.product$ = this.http.get<Product[]>(this.getProductsUrl).pipe(shareReplay());
    }
    return this.product$;
  }
  getProductById(id: number): Observable<Product>  {
      return this.getProducts().pipe(flatMap(result => result), first(product => product.productId === id));
  }
  insertProduct(newProduct: Product ): Observable<Product> {
    return this.http.post<Product>(this.addProductUrl, newProduct);
  }
  updateProduct(id: number, editProduct: Product): Observable<Product> {
    return this.http.put<Product>(this.updateProductUrl + id, editProduct);
  }
  deleteProduct(id: number): Observable<any> {
    return this.http.delete(this.deleteProductUrl + id);
  }
  
  addItemToCart(item: CartItem){
    localStorage.setItem('cartItem', item.item.name);
  }

  clearCache() {
    this.product$ = null;
  }
}
