import {Component, OnInit} from '@angular/core';
import {RegionService} from './region.service';

@Component({
    selector: 'region-component',
    template: '<router-outlet></router-outlet>',
    providers: [RegionService]
})

export class RegionsComponent {
}