import { CartItem } from "./CartItem";

export class Order {
    cartItems: CartItem[];
    totalSum: number;
    email: string;
    phoneNumber: string;
    userName: string;
    address: string;
}
