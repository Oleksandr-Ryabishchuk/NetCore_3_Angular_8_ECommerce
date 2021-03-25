import { Component, OnInit } from '@angular/core';
import { CartItem } from '../interfaces/CartItem';
import { Order } from '../interfaces/order';
import { Product } from '../interfaces/product';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  
}
