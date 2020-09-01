import {Routes} from '@angular/router';
import { VehicleTypesComponent, VehicleTypeListComponent } from './vehicletypes';
import { AuthorizeService } from "../authorize.service";

export const VehicleTypeRoutes: Routes = [
    {
        path: 'vehicletype', component: VehicleTypesComponent,
        children: [
            { path: '', component: VehicleTypeListComponent, data: { activeSubMenuTab: 'VehicleType', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [AuthorizeService]
    }
]