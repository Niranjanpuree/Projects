import {Component, OnInit} from '@angular/core';
import {VehicleToWheelBaseService} from './vehicleToWheelBase.service';
import {WheelBaseService} from '../wheelbase/wheelbase.service';
import {VehicleService} from '../vehicle/vehicle.service';

@Component({
    selector: 'vehicleToWheelBase-component',
    template: `<router-outlet></router-outlet>`,
    providers: [VehicleToWheelBaseService, VehicleService, WheelBaseService]
})

export class VehicleToWheelBasesComponent {
}