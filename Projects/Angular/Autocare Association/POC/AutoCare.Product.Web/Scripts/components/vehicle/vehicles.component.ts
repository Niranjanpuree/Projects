import {Component, OnInit} from '@angular/core';
import {VehicleSearchService} from './vehicle-search.service';
import {VehicleService} from './vehicle.service';
import {SubModelService} from '../subModel/subModel.service';
import {RegionService} from '../region/region.service';

@Component({
    selector: 'vehicles-component',
    template: '<router-outlet></router-outlet>',
    providers: [VehicleSearchService, VehicleService, SubModelService, RegionService]
})

export class VehiclesComponent {
}