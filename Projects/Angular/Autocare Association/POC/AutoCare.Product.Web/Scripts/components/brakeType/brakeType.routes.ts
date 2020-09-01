import {Routes} from '@angular/router';
import { BrakeTypesComponent, BrakeTypeListComponent } from './brakeTypes';
import { AuthorizeService } from "../authorize.service";

export const BrakeTypeRoutes: Routes = [
    {
        path: 'braketype', component: BrakeTypesComponent,
        children: [
            { path: '', component: BrakeTypeListComponent, data: { activeSubMenuTab: 'BrakeType', activeSubMenuGroup: 'Brake' } },
        ],
        data: { activeTab: 'Brake' },
        canActivate: [AuthorizeService]
    }
]