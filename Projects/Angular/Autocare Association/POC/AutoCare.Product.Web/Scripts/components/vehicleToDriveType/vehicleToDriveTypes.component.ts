import {Component, OnInit} from '@angular/core';
import {VehicleToDriveTypeService} from './vehicleToDriveType.service';
import {DriveTypeService} from '../driveType/driveType.service';
import {VehicleService} from '../vehicle/vehicle.service';

@Component({
    selector: 'vehicleToDriveTypes-component',
    template: `<router-outlet></router-outlet>`,
    providers: [VehicleToDriveTypeService, VehicleService, DriveTypeService]
})

export class VehicleToDriveTypesComponent {
}