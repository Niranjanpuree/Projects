import {Component} from '@angular/core';
import {BrakeSystemService} from './brakeSystem.service';

@Component({
    selector: 'brakeSystems-component',
    template: `<router-outlet></router-outlet>`,
    providers: [BrakeSystemService]
})

export class BrakeSystemsComponent {
}