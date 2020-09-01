import {Component} from '@angular/core';
import {ModelService} from './model.service';
import {VehicleTypeService} from '../vehicleType/vehicleType.service';
import {BaseVehicleService} from '../baseVehicle/baseVehicle.service';
import { HttpHelper } from '../httpHelper';

@Component({
    selector: 'models-component',
    template: `<router-outlet></router-outlet>`,
    providers: [ModelService, VehicleTypeService, BaseVehicleService, HttpHelper]
})

export class ModelsComponent {
}