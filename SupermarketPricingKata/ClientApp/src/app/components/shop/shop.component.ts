import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BehaviorSubject } from "rxjs";
import { CheckoutService } from "../../services/checkout.service";
import { FormControlService } from '../../services/form-control.service';
import { Product, DiscountRule, Checkout, CheckoutItem } from 'src/app/models/models';
enum MeasurmentUnits { "PIECE", "POUND", "OUNCE", "LITRE", "MILLILITRE", "GALLON", "METRE", "KILOGRAM", "GRAM" }

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
  defaultDiscount = [{ id: 0, description: 'None', price: 0, quantity: 0 }];
  showError = false;

  constructor(private chechkoutService: CheckoutService, private formService: FormControlService) {
    // Initialize lists
    this.checkout = { checkoutItems: [], totalPrice: 0 } as Checkout;
  }

  ngOnInit() {
    // Get the list of available products in the strore
    this.chechkoutService.getProducts().subscribe(result => {
      this.products = result;
      this.initializeForms();
    });

    // Get the list of available discount rules for products
    this.chechkoutService.getDiscountRules().subscribe(result => {
      this.discountRules = result;
    });

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
      productForm.patchValue({ buyUnit: p.measurmentUnit }); // set default buy units
      if (p.measurmentUnit == MeasurmentUnits.PIECE) {
        // disable buy unit choice when product is sold by piece 
        productForm.controls.buyUnit.disable();
      }
      this.productForms.push(productForm);
    })
  }

  addItemToCheckout(product: Product, form: FormGroup) {

    let checkoutItem = {} as CheckoutItem;

    checkoutItem.product = product;
    checkoutItem.quantity = form.get('quantity').value;

    if (form.get('discount').value !== "default") {
      checkoutItem.discountRule = {} as DiscountRule;
      checkoutItem.discountRule.id = form.get('discount').value; // set the discount rule id
    }
   
    checkoutItem.buyUnit = form.get('buyUnit').value; // set the buy unit

    // Perform adding the created item to the checkout through HTTP
    this.chechkoutService.addItemToCheckout(checkoutItem).subscribe(() => {
      this.update.next(true);
      this.showError = false;
    }, error => this.showError = true);
  }

  getCheckout() {
    this.chechkoutService.getCheckout().subscribe(result => this.checkout = result);
  }

  deleteFromCheckout(id: number) {
    this.chechkoutService.deleteItemFromCheckout(id).subscribe(() => this.update.next(true));
  }

}
