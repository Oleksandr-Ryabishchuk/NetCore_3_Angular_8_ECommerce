import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable, BehaviorSubject} from 'rxjs';
import {map} from 'rxjs/operators';
import { Router } from '@angular/router';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient, private route: Router) { }

  private baseUrlLogin = '/api/account/login';

  private baseUrlRegister = '/api/account/register';

  private loginStatus = new BehaviorSubject<boolean>(this.checkLoginStatus());
  private Email = new BehaviorSubject<string>(localStorage.getItem('email'));
  private UserName = new BehaviorSubject<string>(localStorage.getItem('username'));
  private UserRole = new BehaviorSubject<string>(localStorage.getItem('userRole'));

  register(username: string, phonenumber: string, password: string,  email: string) {
    return this.http.post<any>(this.baseUrlRegister, {username, phonenumber, password, email}).pipe(map(result => {
      return result;
    }, error => {
      return error;
    }));
  }
  login(email: string, password: string) {

    return this.http.post<any>(this.baseUrlLogin, {email, password}).pipe(
      map(result => {
        if (result && result.token) {
          this.loginStatus.next(true);
          localStorage.setItem('loginStatus', '1');
          localStorage.setItem('jwt', result.token);
          localStorage.setItem('email', result.email);
          localStorage.setItem('username', result.userName);
          localStorage.setItem('expiration', result.expiration);
          localStorage.setItem('userRole', result.userRole);
          this.UserName.next(localStorage.getItem('username'));
          this.UserRole.next(localStorage.getItem('userRole'));
        }
        return result;
      })
    );
    }
    logout() {
      this.loginStatus.next(false);
      localStorage.removeItem('jwt');
      localStorage.removeItem('email');
      localStorage.removeItem('username');
      localStorage.removeItem('expiration');
      localStorage.removeItem('userRole');
      localStorage.setItem('loginStatus', '0');
      this.route.navigateByUrl('home');
    }
  checkLoginStatus(): boolean {

    const loginCookie = localStorage.getItem('loginStatus');

    if (loginCookie === '1') {

      if (localStorage.getItem('jwt') === null || localStorage.getItem('jwt') === undefined) {
          return false;
      }

      const token = localStorage.getItem('jwt');
      const decoded = jwt_decode(token);

      if (decoded.exp === undefined) {
        return false;
      }

      const date = new Date(0);

      const tokenExpDate = date.setUTCSeconds(decoded.exp);

      if (tokenExpDate.valueOf() > new Date().valueOf()) {
        return true;
      }

      return false;
    }

  }
  get isLoggedIn() {
    return this.loginStatus.asObservable();
  }

  get currentUserEmail() {
    return this.Email.asObservable();
  }

  get currentUserName() {
    return this.UserName.asObservable();
  }

  get currentUserRole() {
    return this.UserRole.asObservable();
  }


}
