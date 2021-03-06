import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccessDeniedComponent } from './errors/access-denied/access-denied.component';
import { CartComponent } from './cart/cart.component';


const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forRoot([
    {path: 'home', component: HomeComponent},
    {path: '', component: HomeComponent, pathMatch: 'full'},
    {path: 'products', loadChildren: './products/products.module#ProductsModule'},
    {path: 'login', component: LoginComponent},
    {path: 'cart', component: CartComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'access-denied', component: AccessDeniedComponent},
    {path: '**', redirectTo: 'home'}
  ])],
  exports: [RouterModule]
})
export class AppRoutingModule { }
