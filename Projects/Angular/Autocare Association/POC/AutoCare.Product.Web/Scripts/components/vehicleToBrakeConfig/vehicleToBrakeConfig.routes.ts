import {Routes} from '@angular/router';
import { VehicleToBrakeConfigsComponent } from './vehicleToBrakeConfigs';
import {VehicleToBrakeConfigAddComponent} from './vehicleToBrakeConfig-add.component';
import { VehicleToBrakeConfigSearchComponent }         from "./vehicleToBrakeConfig-search.component";
import { AuthorizeService } from "../authorize.service";

export const VehicleToBrakeConfigRoutes: Routes = [
    {
        path: 'vehicletobrakeconfig', component: VehicleToBrakeConfigsComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: VehicleToBrakeConfigSearchComponent },
            { path: 'add', component: VehicleToBrakeConfigAddComponent },
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];