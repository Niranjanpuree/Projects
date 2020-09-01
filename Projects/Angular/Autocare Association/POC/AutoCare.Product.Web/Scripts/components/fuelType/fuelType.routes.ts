import {Routes} from '@angular/router';
import { AuthorizeService } from "../authorize.service";
import { FuelTypesComponent, FuelTypeListComponent } from './fuelTypes';

export const FuelTypeRoutes: Routes = [
    {
        path: 'fueltype', component: FuelTypesComponent,
        children: [
            {
                path: '', component: FuelTypeListComponent, data: { activeSubMenuTab: 'FuelType', activeSubMenuGroup: 'Engine'}
            },
        ],
        data: { activeTab: 'Engine' },
        canActivate: [AuthorizeService]
    }
]