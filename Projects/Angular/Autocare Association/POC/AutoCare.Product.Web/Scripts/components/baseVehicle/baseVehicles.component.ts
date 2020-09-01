import {Component, OnInit} from '@angular/core';
import {BaseVehicleService} from './baseVehicle.service';
import {ModelService} from '../model/model.service';
import {MakeService} from '../make/make.service';
import {YearService} from '../year/year.service';

@Component({
    selector: 'baseVehicles-component',
    template: `<router-outlet></router-outlet>`,
    providers: [BaseVehicleService, ModelService, MakeService, YearService]
})

export class BaseVehiclesComponent {
}