import {Component} from '@angular/core';
import {VehicleTypeListComponent} from './vehicleType-list.component';
import {VehicleTypeService} from './vehicleType.service';
import {VehicleTypeGroupService} from '../vehicleTypeGroup/vehicleTypeGroup.service';

@Component({
    selector: 'vehicleTypes-component',
    template: `<router-outlet></router-outlet>`,
    providers: [VehicleTypeService, VehicleTypeGroupService]
})

export class VehicleTypesComponent {
}