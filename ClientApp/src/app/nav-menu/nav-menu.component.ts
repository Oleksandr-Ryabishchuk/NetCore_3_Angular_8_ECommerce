import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Observable } from 'rxjs';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  constructor(private account: AccountService, private productService: ProductService) { }
    loginStatus$: Observable<boolean>;

    UserName$: Observable<string>;

  ngOnInit() {
    this.loginStatus$ = this.account.isLoggedIn;

    this.UserName$ = this.account.currentUserName;
  }
  onLogout() {
    this.productService.clearCache();
    this.account.logout();
  }
}
