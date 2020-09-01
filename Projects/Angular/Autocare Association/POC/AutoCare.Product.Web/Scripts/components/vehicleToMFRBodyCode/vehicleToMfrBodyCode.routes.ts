import {Routes} from '@angular/router';
import { VehicleToMfrBodyCodesComponent } from './vehicleToMfrBodyCodes';
import {VehicleToMfrBodyCodeAddComponent} from './vehicleToMfrBodyCode-add.component';
import { VehicleToMfrBodyCodeSearchComponent }         from "./vehicleToMfrBodyCode-search.component";
import { AuthorizeService } from "../authorize.service";

export const VehicleToMfrBodyCodeRoutes: Routes = [
    {
        path: 'vehicletomfrbodycode', component: VehicleToMfrBodyCodesComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: VehicleToMfrBodyCodeSearchComponent },
            { path: 'add', component: VehicleToMfrBodyCodeAddComponent },
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];