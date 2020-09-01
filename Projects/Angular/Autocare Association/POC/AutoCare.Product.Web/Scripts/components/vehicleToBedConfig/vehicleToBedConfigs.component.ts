import {Component, OnInit} from '@angular/core';
import {VehicleToBedConfigService} from './vehicleToBedConfig.service';
import {BedConfigService} from '../bedConfig/bedConfig.service';
import {VehicleService} from '../vehicle/vehicle.service';

@Component({
    selector: 'vehicleToBedConfigs-component',
    template: `<router-outlet></router-outlet>`,
    providers: [VehicleToBedConfigService, VehicleService, BedConfigService]
})

export class VehicleToBedConfigsComponent {
}