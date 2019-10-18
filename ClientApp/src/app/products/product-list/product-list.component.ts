import { Component, OnInit, OnDestroy, ViewChild, TemplateRef } from '@angular/core';
import {FormGroup, FormControl, FormBuilder, Validators, ReactiveFormsModule} from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Product } from 'src/app/interfaces/product';
import { Observable, Subject } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { ProductService } from 'src/app/services/product.service';


@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  constructor(private productService: ProductService,
              private modalService: BsModalService,
              private formBuilder: FormBuilder) { }
 // For the FormControl - Adding products
 insertForm: FormGroup;
 name: FormControl;
 price: FormControl;
 description: FormControl;
 outOfStock: FormControl;
 imageUrl: FormControl;

 // Updating the Product
  updateForm: FormGroup;
 // tslint:disable-next-line: variable-name
 _name: FormControl;
 // tslint:disable-next-line: variable-name
 _price: FormControl;
 // tslint:disable-next-line: variable-name
 _description: FormControl;
 // tslint:disable-next-line: variable-name
 _imageUrl: FormControl;
 // tslint:disable-next-line: variable-name
 _id: FormControl;


 // Add modal
  @ViewChild('template', {static: false}) modal: TemplateRef<any>;

 // Update modal
 @ViewChild('editTemplate', {static: false}) editmodal: TemplateRef<any>;

 // Modal properties
 modalMessage: string;
 modalRef: BsModalRef;
 selectedProduct: Product;
 products$: Observable<Product[]>;
 products: Product[] = [];
 userRoleStatus: string;


 // Datatables props
 dtOptions: DataTables.Settings = {};
 dtTrigger: Subject<any> = new Subject();

 @ViewChild(DataTableDirective, {static: false}) dtElement: DataTableDirective;


  ngOnInit() {
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 5,
      autoWidth: true,
      order: [[0, 'desc']]
    };

    this.products$ = this.productService.getProducts();

    this.products$.subscribe(result => {
      this.products = result;
      this.dtTrigger.next();
    });

    // Modal message
    this.modalMessage = 'Всі поля є обов`язкові';

    // Add product properties
    const validateImageUrl = '^(https?:\/\/.*\.(?:png|jpg))$';

    this.name = new FormControl('', [Validators.required, Validators.maxLength(50)]);
    this.price = new FormControl('', [Validators.required, Validators.min(0), Validators.max(100000)]);
    this.description = new FormControl('', [Validators.required, Validators.maxLength(150)]);
    this.imageUrl = new FormControl('', [Validators.pattern(validateImageUrl)]);

    this.insertForm = this.formBuilder.group({
     name: this.name,
     price: this.price,
     description: this.description,
     imageUrl: this.imageUrl,
     outOfStock: true
    });
  }
  onAddProduct() {
    this.modalRef = this.modalService.show(this.modal);
  }
}
