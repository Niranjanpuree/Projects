import {Component} from '@angular/core';
import {SubModelService} from './subModel.service';
import {BaseVehicleService} from '../baseVehicle/baseVehicle.service';

@Component({
    selector: 'sub-models-component',
    template: `<router-outlet></router-outlet>`,
    providers: [SubModelService, BaseVehicleService]
})

export class SubModelsComponent {
}