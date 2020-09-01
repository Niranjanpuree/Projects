import {Routes} from '@angular/router';
import { VehiclesComponent } from './vehicles';
import {VehicleSearchComponent} from './vehicle-search.component';
import {VehicleModifyComponent} from './vehicle-modify.component';
import {VehicleDeleteComponent} from './vehicle-delete.component';
import {VehicleAddComponent} from './vehicle-add.component';
import { AuthorizeService } from "../authorize.service";

export const VehicleRoutes: Routes = [
    {
        path: 'vehicle', component: VehiclesComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: VehicleSearchComponent },
            { path: 'add/:basevid', component: VehicleAddComponent },
            { path: 'modify/:id', component: VehicleModifyComponent },
            { path: 'delete/:id', component: VehicleDeleteComponent },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [AuthorizeService]
    }
]