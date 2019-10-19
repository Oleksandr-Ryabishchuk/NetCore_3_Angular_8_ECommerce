import { Component, OnInit, OnDestroy, ViewChild, TemplateRef, ChangeDetectorRef } from '@angular/core';
import {FormGroup, FormControl, FormBuilder, Validators, ReactiveFormsModule} from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Product } from 'src/app/interfaces/product';
import { Observable, Subject } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { ProductService } from 'src/app/services/product.service';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';


@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit, OnDestroy {
  constructor(private productService: ProductService,
              private modalService: BsModalService,
              private formBuilder: FormBuilder,
              private changeRef: ChangeDetectorRef,
              private router: Router,
              private account: AccountService) { }
 // For the FormControl - Adding products
 insertForm: FormGroup;
 name: FormControl;
 price: FormControl;
 description: FormControl;
 // outOfStock: FormControl;
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
      pageLength: 3,
      autoWidth: true,
      order: [[0, 'desc']]
    };
    // Add products to datatable
    this.products$ = this.productService.getProducts();

    this.products$.subscribe(result => {
      this.products = result;

      this.changeRef.detectChanges();

      this.dtTrigger.next();
    });

    this.account.currentUserRole.subscribe(result => {
      this.userRoleStatus = result;
    });

    // Modal message
    this.modalMessage = 'Всі поля є обов`язкові';

    // Add product properties

    this.name = new FormControl('', [Validators.required, Validators.maxLength(50)]);
    this.description = new FormControl('', [Validators.required, Validators.maxLength(150)]);
    this.price = new FormControl('', [Validators.required, Validators.min(0), Validators.max(100000)]);
    this.imageUrl = new FormControl('');

    this.insertForm = this.formBuilder.group({
     name: this.name,
     description: this.description,
     price: this.price,
     outOfStock: true,
     imageUrl: this.imageUrl
    });
    // Add update properties

    this._name = new FormControl('', [Validators.required, Validators.maxLength(50)]);
    this._price = new FormControl('', [Validators.required, Validators.min(0), Validators.max(100000)]);
    this._description = new FormControl('', [Validators.required, Validators.maxLength(150)]);
    this._imageUrl = new FormControl('');
    this._id = new FormControl();

    this.updateForm = this.formBuilder.group({
      name: this._name,
      description: this._description,
      price: this._price,
      imageUrl: this._imageUrl,
      outOfStock: true,
      id: this._id
    });
  }
  onSubmit() {
    const newProduct = this.insertForm.value;
    this.productService.insertProduct(newProduct).subscribe(
      result => {
        this.productService.clearCache();
        this.products$ = this.productService.getProducts();
        this.products$.subscribe(newList => {
          this.products = newList;
          this.modalRef.hide();
          this.insertForm.reset();
          this.rerender();
        });
        console.log('New product added');
      },
      error => console.log('Adding new product failed')
    );
  }

  onAddProduct() {
    this.modalRef = this.modalService.show(this.modal);
  }

  onUpdate() {
    const editProduct = this.updateForm.value;
    this.productService.updateProduct(editProduct.id, editProduct).subscribe(
      result => {
        console.log('Product updated');
        this.productService.clearCache();
        this.products$ = this.productService.getProducts();
        this.products$.subscribe(updatedList => {
          this.products = updatedList;
          this.modalRef.hide();
          this.rerender();
        });
      },
      error => console.log('Could not Update Product', error)
    );
  }
  onSelect(product: Product): void {
    this.selectedProduct = product;
    this.router.navigateByUrl('/products/' + product.productId);
  }
  onDelete(product: Product): void {
    this.productService.deleteProduct(product.productId).subscribe(result => {
      this.productService.clearCache();
      this.products$ = this.productService.getProducts();
      this.products$.subscribe(newList => {
        this.products = newList;
        this.rerender();
      });
    });
  }

  onModalUpdate(productEdit: Product): void {
    this._id.setValue(productEdit.productId);
    this._name.setValue(productEdit.name);
    this._description.setValue(productEdit.description);
    this._price.setValue(productEdit.price);
    this._imageUrl.setValue(productEdit.imageUrl);

    this.updateForm.setValue({
      id: this._id.value,
      name: this._name.value,
      outOfStock: true,
      description: this._description.value,
      price: this._price.value,
      imageUrl: this._imageUrl.value
    });
    this.modalRef = this.modalService.show(this.editmodal);
  }
  rerender() {
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.destroy();
      this.dtTrigger.next();
    });
  }
  ngOnDestroy() {
    this.dtTrigger.unsubscribe();
  }
}
