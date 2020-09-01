import {Component} from '@angular/core';
import {EngineVinService} from './engineVin.service';

@Component({
    selector: 'engineVins-component',
    template: `<router-outlet></router-outlet>`,
    providers: [EngineVinService]
})

export class EngineVinsComponent {
}