import {Routes} from '@angular/router';
import { VehicleToBodyStyleConfigsComponent } from './vehicleToBodyStyleConfigs';
import {VehicleToBodyStyleConfigAddComponent} from './vehicleToBodyStyleConfig-add.component';
import { VehicleToBodyStyleConfigSearchComponent }         from "./vehicleToBodyStyleConfig-search.component";
import { AuthorizeService } from "../authorize.service";

export const VehicleToBodyStyleConfigRoutes: Routes = [
    {
        path: 'vehicletobodystyleconfig', component: VehicleToBodyStyleConfigsComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: VehicleToBodyStyleConfigSearchComponent },
            { path: 'add', component: VehicleToBodyStyleConfigAddComponent },
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];




