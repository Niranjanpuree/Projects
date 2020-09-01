import {Routes} from '@angular/router';
import { MakesComponent, MakeListComponent } from './makes';
import { AuthorizeService } from '../authorize.service';

export const MakeRoutes: Routes = [
    {
        path: 'make', component: MakesComponent,
        children: [
            { path: '', component: MakeListComponent, data: { activeSubMenuTab: 'Make', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'ReferenceData' },
        canActivate: [AuthorizeService]
    },
]