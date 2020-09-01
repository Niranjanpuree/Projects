import {Component} from '@angular/core';
import { YearService } from './year.service';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'years-component',
    template: `<router-outlet></router-outlet>`,
    providers: [YearService]
})

export class YearsComponent { }