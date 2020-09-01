import {Component} from '@angular/core';
import {VehicleTypeGroupListComponent} from './vehicleTypeGroup-list.component';
import {VehicleTypeGroupService} from './vehicleTypeGroup.service';

@Component({
    selector: 'vehicleTypeGroups-component',
    template: `<router-outlet></router-outlet>`,
    providers: [VehicleTypeGroupService]
})

export class VehicleTypeGroupsComponent {
}