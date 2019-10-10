import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable, BehaviorSubject} from 'rxjs';
import {map} from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient, private route: Router) { }

  private baseUrlLogin = '/api/account/login';

  private loginStatus = new BehaviorSubject<boolean>(this.checkLoginStatus());
  private Email = new BehaviorSubject<string>(localStorage.getItem('email'));
  private UserRole = new BehaviorSubject<string>(localStorage.getItem('userRole'));

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
      this.route.navigateByUrl('login');
      console.log('Logged Out Successfully');
    }
  checkLoginStatus(): boolean {
    return false;
  }
  get isLoggedIn() {
    return this.loginStatus.asObservable();
  }

  get currentUserEmail() {
    return this.Email.asObservable();
  }

  get currentUserRole() {
    return this.UserRole.asObservable();
  }
}
