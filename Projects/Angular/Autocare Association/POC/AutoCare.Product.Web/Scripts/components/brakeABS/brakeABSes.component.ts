import {Component} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {BrakeABSService} from './brakeABS.service';

@Component({
    selector: 'brakeABSes-component',
    template: '<router-outlet></router-outlet>',
    providers: [BrakeABSService]
})

export class BrakeABSesComponent {
}