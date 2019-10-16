import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  constructor(private account: AccountService) { }
    loginStatus$: Observable<boolean>;

    UserName$: Observable<string>;

  ngOnInit() {
    this.loginStatus$ = this.account.isLoggedIn;

    this.UserName$ = this.account.currentUserName;
  }
  onLogout() {
    this.account.logout();
  }
}
