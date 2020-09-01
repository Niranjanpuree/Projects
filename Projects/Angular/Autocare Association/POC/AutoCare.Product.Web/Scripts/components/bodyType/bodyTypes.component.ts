import {Component} from '@angular/core';
import {BodyTypeService} from './bodyType.service';

@Component({
    selector: 'bodyTypes-component',
    template: `<router-outlet></router-outlet>`,
    providers: [BodyTypeService]
})

export class BodyTypesComponent {
}