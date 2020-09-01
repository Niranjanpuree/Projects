import {Component} from '@angular/core';
import {BedLengthService} from './bedLength.service';

@Component({
    selector: 'bedLength-component',
    template: `<router-outlet></router-outlet>`,
    providers: [BedLengthService]
})

export class BedLengthComponent {
}