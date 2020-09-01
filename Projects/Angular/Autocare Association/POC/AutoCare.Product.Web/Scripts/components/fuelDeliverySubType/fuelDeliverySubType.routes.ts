import {Routes} from '@angular/router';
import { FuelDeliverySubTypesComponent, FuelDeliverySubTypeListComponent } from './fuelDeliverySubTypes'
import { AuthorizeService } from '../authorize.service';

export const FuelDeliverySubTypeRoutes: Routes = [
    {
        path: 'fuelDeliverySubType', component: FuelDeliverySubTypesComponent,
        children: [
            { path: '', component: FuelDeliverySubTypeListComponent, data: { activeSubMenuTab: 'FuelDeliverySubType', activeSubMenuGroup: 'Engine' } },
        ],
        data: { activeTab: 'ReferenceData' },
        canActivate: [AuthorizeService]
    },
]