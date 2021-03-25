import { Product } from "./product";

export class CartItem {
    id: number;
    item: Product;
    quantity: number;
    sum: number;
}
