import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { AccountService } from '../services/account.service';
import { Router } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private account: AccountService,
    private router: Router,
    private modalService: BsModalService
  ) { }

  insertForm: FormGroup;
  username: FormControl;
  phonenumber: FormControl;
  password: FormControl;
  cpassword: FormControl;
  email: FormControl;
  modalRef: BsModalRef;
  errorList: string[];
  modalMessage: string;

  @ViewChild('template', {static: false}) modal: TemplateRef<any>;

  onSubmit() {
    const userDetails = this.insertForm.value;

    this.account.register(userDetails.username, userDetails.phonenumber, userDetails.password, userDetails.email).subscribe(result => {

      this.router.navigate(['/login']);
    // tslint:disable-next-line: no-shadowed-variable
    }, error => {
      this.errorList = [];
      // tslint:disable-next-line: prefer-for-of
      for (let i = 0; i < error.error.value.length; i++) {
        this.errorList.push(error.error.value[i]);
        console.log(error.error.value[i]);
      }
      console.log(this.errorList);
      this.modalMessage = 'Введене ім`я або електронна адреса вже зареєстровані';
      this.modalRef = this.modalService.show(this.modal);
    });
  }

  MustMatch(passwordControl: AbstractControl ): ValidatorFn {
    return (cpasswordControl: AbstractControl): {[key: string]: boolean} | null => {
      if (!passwordControl && !cpasswordControl) {
        return null;
      }
      if (cpasswordControl.hasError && !passwordControl.hasError) {
        return null;
      }
      if (passwordControl.value !== cpasswordControl.value) {
        return {mustMatch: true};
      } else {
        return null;
      }
    };
 }

  ngOnInit() {
    this.username = new FormControl('', [Validators.required, Validators.maxLength(15), Validators.minLength(5)]);
    this.phonenumber = new FormControl('', [Validators.required]);
    this.password = new FormControl('', [Validators.required, Validators.maxLength(15), Validators.minLength(5)]);
    this.cpassword = new FormControl('', [Validators.required, this.MustMatch(this.password)]);
    this.email = new FormControl('', [Validators.required, Validators.email]);
    this.errorList = [];

    this.insertForm = this.formBuilder.group({
      username: this.username,
      phonenumber: this.phonenumber,
      password: this.password,
      cpassword: this.cpassword,
      email: this.email
    });
  }

}
