import {Routes} from '@angular/router';
import { VehicleToDriveTypesComponent,
         VehicleToDriveTypeAddComponent,
         VehicleToDriveTypeSearchComponent} from './vehicleToDriveTypes';
import { AuthorizeService } from "../authorize.service";

export const VehicleToDriveTypeRoutes: Routes = [
    {
        path: 'vehicletodrivetype', component: VehicleToDriveTypesComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: VehicleToDriveTypeSearchComponent },
            { path: 'add', component: VehicleToDriveTypeAddComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];