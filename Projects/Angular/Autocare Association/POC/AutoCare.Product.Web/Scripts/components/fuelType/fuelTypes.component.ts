import {Component} from '@angular/core';
import {FuelTypeService} from './fuelType.service';

@Component({
    selector: 'fuelTypes-component',
    template: `<router-outlet></router-outlet>`,
    providers: [FuelTypeService]
})

export class FuelTypesComponent {
}