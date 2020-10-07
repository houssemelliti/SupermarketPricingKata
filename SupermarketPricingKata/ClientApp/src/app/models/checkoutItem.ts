/**
 * Supermarket Pricing Kata API
 * Visualize and interact with the API's resources
 *
 * OpenAPI spec version: v1
 *
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
import { DiscountRule } from './discountRule';
import { Product } from './product';
import { MeasurmentUnits } from './measurmentUnits';

export interface CheckoutItem {
    id?: number;
    product?: Product;
    quantity?: number;
    buyUnit?: MeasurmentUnits;
    discountRule?: DiscountRule;
}
