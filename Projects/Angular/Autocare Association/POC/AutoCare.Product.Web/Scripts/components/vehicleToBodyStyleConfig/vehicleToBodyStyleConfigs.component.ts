import {Component, OnInit} from '@angular/core';
import {VehicleToBodyStyleConfigService} from './vehicleToBodyStyleConfig.service';
import {BodyStyleConfigService} from '../bodyStyleConfig/bodyStyleConfig.service';
import {VehicleService} from '../vehicle/vehicle.service';

@Component({
    selector: 'vehicleToBodyStyleConfigs-component',
    template: `<router-outlet></router-outlet>`,
    providers: [VehicleToBodyStyleConfigService, VehicleService, BodyStyleConfigService]
})

export class VehicleToBodyStyleConfigsComponent {
}