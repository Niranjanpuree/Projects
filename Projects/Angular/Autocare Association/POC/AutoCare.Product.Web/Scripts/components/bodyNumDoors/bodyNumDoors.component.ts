import {Component} from '@angular/core';
import {BodyNumDoorsService} from './bodyNumDoors.service';

@Component({
    selector: 'bodyNumDoors-component',
    template: `<router-outlet></router-outlet>`,
    providers: [BodyNumDoorsService]
})

export class BodyNumDoorsComponent {
}