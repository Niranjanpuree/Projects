import {Component, OnInit} from '@angular/core';
import {VehicleToBrakeConfigService} from './vehicleToBrakeConfig.service';
import {BrakeConfigService} from '../brakeConfig/brakeConfig.service';
import {BrakeTypeService} from '../brakeType/brakeType.service';
import {BrakeSystemService} from '../brakeSystem/brakeSystem.service';
import {BrakeABSService} from '../brakeABS/brakeABS.service';
import {VehicleService} from '../vehicle/vehicle.service';

@Component({
    selector: 'vehicleToBrakeConfigs-component',
    template: `<router-outlet></router-outlet>`,
    providers: [VehicleToBrakeConfigService, VehicleService, BrakeConfigService, BrakeTypeService, BrakeSystemService, BrakeABSService]
})

export class VehicleToBrakeConfigsComponent {
}