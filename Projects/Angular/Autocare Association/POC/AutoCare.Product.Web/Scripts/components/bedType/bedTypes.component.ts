import {Component} from '@angular/core';
import {BedTypeService} from './bedType.service';

@Component({
    selector: 'bedTypes-component',
    template: `<router-outlet></router-outlet>`,
    providers: [BedTypeService]
})

export class BedTypesComponent {
}