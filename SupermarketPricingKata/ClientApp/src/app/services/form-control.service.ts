import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class FormControlService {

  createProductForm() {
    return new FormGroup({
      quantity: new FormControl({ value: 0 }, Validators.required),
      buyUnit: new FormControl(),
      discount: new FormControl("default")
    });
  }
}
