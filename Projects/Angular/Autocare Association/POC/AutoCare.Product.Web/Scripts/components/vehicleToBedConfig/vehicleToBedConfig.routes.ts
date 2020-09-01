import {Routes} from '@angular/router';
import { VehicleToBedConfigsComponent } from './vehicleToBedConfigs';
import {VehicleToBedConfigAddComponent} from './vehicleToBedConfig-add.component';
import { VehicleToBedConfigSearchComponent }         from "./vehicleToBedConfig-search.component";
import { AuthorizeService } from "../authorize.service";

export const VehicleToBedConfigRoutes: Routes = [
    {
        path: 'vehicletobedconfig', component: VehicleToBedConfigsComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: VehicleToBedConfigSearchComponent },
            { path: 'add', component: VehicleToBedConfigAddComponent },
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];




