import {Component, OnInit} from '@angular/core';
import {BrakeTypeService} from './brakeType.service';

@Component({
    selector: 'brakeTypes-component',
    template: '<router-outlet></router-outlet>',
    providers: [BrakeTypeService]
})

export class BrakeTypesComponent {
}