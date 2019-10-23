import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '../services/account.service';
import { take, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {


  constructor(private account: AccountService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.account.isLoggedIn.pipe(take(1), map((loginStatus: boolean) => {
      const destination = state.url;
      const productId = route.params.id;


      switch (destination) {
        case '/products':
         case '/products/' + productId: {

          return true;
         }
        // tslint:disable-next-line: no-switch-case-fall-through
        case '/product/update': {
          if (localStorage.getItem('userRole') === 'Customer' ||
          localStorage.getItem('userRole') === 'Moderator') {
            this.router.navigate(['/access-denied']);
            return false;
          }
          if (localStorage.getItem('userRole') === 'Admin') {
            return true;
          }
        }
        // tslint:disable-next-line: no-switch-case-fall-through
        default: return false;
      }
    }));
  }
}
