import {Routes} from '@angular/router';
import { AuthorizeService } from "../authorize.service";
import { EngineVinsComponent, EngineVinListComponent } from './engineVins';

export const EngineVinRoutes: Routes = [
    {
        path: 'enginevin', component: EngineVinsComponent,
        children: [
            {
                path: '', component: EngineVinListComponent, data: { activeSubMenuTab: 'Vin', activeSubMenuGroup: 'Engine'}
            },
        ],
        data: { activeTab: 'Engine' },
        canActivate: [AuthorizeService]
    }
]