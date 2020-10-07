import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { ShopComponent } from './components/shop/shop.component';
import { CheckoutService } from './services/checkout.service';
import { FormControlService } from './services/form-control.service';
import { EnumToArrayPipe } from './pipes/enum-to-array.pipe';

@NgModule({
  declarations: [
    AppComponent,
    ShopComponent,
    NavMenuComponent,
    EnumToArrayPipe
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: ShopComponent, pathMatch: 'full' }
    ])
  ],
  providers: [CheckoutService, FormControlService],
  bootstrap: [AppComponent]
})
export class AppModule { }
