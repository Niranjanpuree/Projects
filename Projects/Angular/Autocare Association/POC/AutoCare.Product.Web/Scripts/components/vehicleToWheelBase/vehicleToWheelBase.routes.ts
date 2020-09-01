import { Routes } from '@angular/router';
import { VehicleToWheelBasesComponent } from './vehicleToWheelBases.component';
import { VehicleToWheelBaseAddComponent} from './vehicleToWheelBase-add.component';
import { VehicleToWheelBaseSearchComponent }         from "./vehicleToWheelBase-search.component";
import { AuthorizeService } from "../authorize.service";

export const VehicleToWheelBaseRoutes: Routes = [
    {
        path: 'vehicletowheelbase', component: VehicleToWheelBasesComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: VehicleToWheelBaseSearchComponent },
            { path: 'add', component: VehicleToWheelBaseAddComponent },
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];
