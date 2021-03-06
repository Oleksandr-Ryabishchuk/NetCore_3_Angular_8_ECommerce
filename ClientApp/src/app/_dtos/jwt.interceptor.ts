import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { AccountService } from '../services/account.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
  })

  export class JwtInterceptor implements HttpInterceptor {

    constructor(private accountService: AccountService) {}
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const currentUser = this.accountService.isLoggedIn;
        const token = localStorage.getItem('jwt');

        if (currentUser && token !== undefined) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`
                }
            });
            return next.handle(request);
        }
    }
  }
