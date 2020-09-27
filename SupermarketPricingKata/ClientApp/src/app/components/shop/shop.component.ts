import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from "rxjs";
import { CheckoutService } from "../../services/checkout.service";
import { FormGroup } from '@angular/forms';
import { FormControlService } from '../../services/form-control.service';
import { Product, DiscountRule, Checkout, CheckoutItem } from 'src/app/models/models';
enum MeasurmentUnits { "UNIT", "POUND", "LITRE", "METRE", "GRAM" }

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent implements OnInit {

  public products: Product[];
  public productForms: FormGroup[] = [];
  public checkout: Checkout;
  public discountRules: DiscountRule[];
  public update = new BehaviorSubject<boolean>(false); // Needed to auto-update the checkout object
  units = MeasurmentUnits;

  constructor(private chechkoutService: CheckoutService, private formService: FormControlService) {
    // Initialize lists
    this.checkout = { checkoutItems: [], totalPrice: 0 } as Checkout;
    this.discountRules = [{ id: 0, description: 'None', price: 0, quantity: 0 }];
  }

  ngOnInit() {
    this.chechkoutService.getProducts().subscribe(result => {
      this.products = result;
      this.initializeForms();
    });

    this.setDiscountRules();
    this.getCheckout();
    if (this.update) {
      // Reload getCheckout() request each time an item is added or removed from the checkout
      this.update.subscribe(update => update ? this.getCheckout() : "");
    }
  }

  // Initializes the forms for each checkout item through form-control service
  initializeForms() {
    this.products.forEach(p => {
      let productForm: FormGroup = new FormGroup({});
      productForm = this.formService.createProductForm();
      this.productForms.push(productForm);
    })
  }

  addItemToCheckout(product: Product, form: FormGroup) {

    let checkoutItem = {} as CheckoutItem;

    checkoutItem.product = product;
    checkoutItem.quantity = form.get('quantity').value;

    this.discountRules.forEach(rule => {
      if (rule.description === form.get('discount').value) {
        checkoutItem.discountRule = rule;
        // Set the discount price based on the product's unit price for each type of rule
        switch (rule.id) {
          case 2:
            checkoutItem.discountRule.price = checkoutItem.product.unitPrice * 2;
            break;
          case 3:
            checkoutItem.discountRule.price = checkoutItem.product.unitPrice / 5;
            break;
          default:
            break;
        }
      }
    });
    // Perform adding the created item to the checkout through HTTP
    this.chechkoutService.addItemToCheckout(checkoutItem).subscribe(() => this.update.next(true));
  }

  getCheckout() {
    this.chechkoutService.getCheckout().subscribe(result => this.checkout = result);
  }

  setDiscountRules() {
    // Create example discount rules to be applied on any product
    let rule1 = { id: 1, description: "Three for a dollar", quantity: 3, price: 1 } as DiscountRule;
    this.discountRules.push(rule1);

    let rule2 = { id: 2, description: "Buy two, get one free!", quantity: 3 } as DiscountRule;
    this.discountRules.push(rule2);

    let rule3 = { id: 3, description: "80% Off !", quantity: 1 } as DiscountRule;
    this.discountRules.push(rule3);
  }

  deleteFromCheckout(sku: number) {
    this.chechkoutService.deleteItemFromCheckout(sku).subscribe(() => this.update.next(true));
  }

}
