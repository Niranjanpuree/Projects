import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FuelDeliverySubTypeService } from './fuelDeliverySubType.service';

@Component({
    selector: 'fuelDeliverySubTypes-comp',
    template: `<router-outlet></router-outlet>`,
    providers: [FuelDeliverySubTypeService]
})
export class FuelDeliverySubTypesComponent {
}