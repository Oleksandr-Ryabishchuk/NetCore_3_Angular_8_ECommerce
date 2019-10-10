import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import {FormGroup, FormControl, FormBuilder, Validators, ReactiveFormsModule} from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

@Component ({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  insertForm: FormGroup;
  Email: FormControl;
  Password: FormControl;
  returnUrl: string;
  ErrorMessage: string;
  invalidLogin: boolean;
  constructor(private account: AccountService,
              private router: Router,
              private route: ActivatedRoute,
              private formBuilder: FormBuilder
              ) { }
  onSubmit() {
    const userLogin = this.insertForm.value;
    this.account.login(userLogin.Email, userLogin.Password).subscribe(res => {
      const token = (res as any).token;
      this.invalidLogin = false;
      console.log(token);
      console.log(res.userRole);
      console.log(this.returnUrl);
      this.router.navigateByUrl(this.returnUrl);
    },
    error => {
      this.invalidLogin = true;
      this.ErrorMessage = 'Ви ввели невірні дані';
      console.log(this.ErrorMessage);
    });
  }

  ngOnInit() {
   this.Email = new FormControl('', [Validators.required]);
   this.Password = new FormControl('', [Validators.required]);


   this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/';

   this.insertForm = this.formBuilder.group({
    Email: this.Email,
    Password:  this.Password
 });
  }

}
