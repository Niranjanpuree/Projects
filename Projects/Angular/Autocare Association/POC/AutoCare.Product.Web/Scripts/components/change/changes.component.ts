import {Component, OnInit} from '@angular/core';
import {ChangeService} from './change.service';
import {ChangeSearchComponent} from './change-search.component';

@Component({
    selector: 'changes-component',
    template: '<router-outlet></router-outlet>',
    providers: [ChangeService]
})

export class ChangesComponent {
}