import {Routes} from '@angular/router';
import { RegionsComponent, RegionListComponent } from './regions';
import { AuthorizeService } from "../authorize.service";

export const RegionRoutes: Routes = [
    {
        path: 'region', component: RegionsComponent,
        children: [
            { path: '', component: RegionListComponent, data: { activeSubMenuTab: 'Region', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [AuthorizeService]
    }
]