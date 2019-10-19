import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/interfaces/product';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  @Input() product: Product;
  constructor(private route: ActivatedRoute,
              private productService: ProductService) { }

  ngOnInit() {
    const id = + this.route.snapshot.params.id;

    this.productService.getProductById(id).subscribe(result => this.product = result);
  }

}
