import {Component} from '@angular/core';
import {EngineVersionService} from './engineVersion.service';

@Component({
    selector: 'engineVersions-component',
    template: `<router-outlet></router-outlet>`,
    providers: [EngineVersionService]
})

export class EngineVersionsComponent {
}