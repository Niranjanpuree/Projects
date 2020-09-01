import {Routes} from '@angular/router';
import { BrakeSystemsComponent, BrakeSystemListComponent } from './brakesystems';
import { AuthorizeService } from "../authorize.service";

export const BrakeSystemRoutes: Routes = [
    {
        path: 'brakesystem', component: BrakeSystemsComponent,
        children: [
            { path: '' , component: BrakeSystemListComponent, data: { activeSubMenuTab: 'BrakeSystem', activeSubMenuGroup: 'Brake' } },
],
    data: { activeTab: 'Brake' },
    canActivate: [AuthorizeService]
    }
]