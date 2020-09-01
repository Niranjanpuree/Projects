import {Routes} from '@angular/router';
import { AuthorizeService } from "../authorize.service";
import { BedTypesComponent, BedTypeListComponent } from './bedTypes';

export const BedTypeRoutes: Routes = [
    {
        path: 'bedtype', component: BedTypesComponent,
        children: [
            {
                path: '', component: BedTypeListComponent, data: { activeSubMenuTab: 'Bed Type', activeSubMenuGroup: 'Bed'}
            },
        ],
        data: { activeTab: 'Bed' },
        canActivate: [AuthorizeService]
    }
]