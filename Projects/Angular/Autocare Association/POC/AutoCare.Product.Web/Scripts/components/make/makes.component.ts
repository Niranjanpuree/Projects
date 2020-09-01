import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MakeService } from './make.service';

@Component({
    selector: 'makes-comp',
    template: `<router-outlet></router-outlet>`,
    providers: [MakeService]
})
export class MakesComponent {
}