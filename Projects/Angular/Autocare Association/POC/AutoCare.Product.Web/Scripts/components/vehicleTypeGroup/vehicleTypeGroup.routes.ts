import { Routes} from '@angular/router';
import { VehicleTypeGroupsComponent, VehicleTypeGroupListComponent } from './vehicleTypeGroups';
import { AuthorizeService } from "../authorize.service";

export const VehicleTypeGroupRoutes: Routes = [
    {
        path: 'vehicletypegroup', component: VehicleTypeGroupsComponent,
        children: [
            { path: '', component: VehicleTypeGroupListComponent, data: { activeSubMenuTab: 'VehicleTypeGroup', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [AuthorizeService]
    }
]