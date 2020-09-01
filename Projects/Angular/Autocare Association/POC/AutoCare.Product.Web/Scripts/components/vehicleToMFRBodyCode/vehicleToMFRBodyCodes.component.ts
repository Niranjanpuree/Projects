import {Component, OnInit} from '@angular/core';
import {VehicleToMfrBodyCodeService} from './vehicleToMfrBodyCode.service';
import {MfrBodyCodeService} from '../mfrBodyCode/mfrBodyCode.service';
import {VehicleService} from '../vehicle/vehicle.service';

@Component({
    selector: 'vehicleToMfrBodyCodes-component',
    template: `<router-outlet></router-outlet>`,
    providers: [VehicleToMfrBodyCodeService, VehicleService, MfrBodyCodeService]
})

export class VehicleToMfrBodyCodesComponent {
}