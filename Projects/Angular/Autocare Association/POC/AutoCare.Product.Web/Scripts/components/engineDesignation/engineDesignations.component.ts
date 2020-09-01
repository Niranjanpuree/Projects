import {Component} from '@angular/core';
import {EngineDesignationService} from './engineDesignation.service';

@Component({
    selector: 'engineDesignations-component',
    template: `<router-outlet></router-outlet>`,
    providers: [EngineDesignationService]
})

export class EngineDesignationsComponent {
}