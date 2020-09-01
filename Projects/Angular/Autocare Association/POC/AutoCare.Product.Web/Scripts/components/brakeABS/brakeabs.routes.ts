import { Routes } from '@angular/router';
import { BrakeABSesComponent, BrakeABSListComponent } from './brakeABSes';
import { AuthorizeService } from "../authorize.service";

export const BrakeAbsRoutes: Routes = [
    {
        path: 'brakeabs', component: BrakeABSesComponent,
        children: [
            { path: '' , component: BrakeABSListComponent, data: { activeSubMenuTab: 'BrakeABS', activeSubMenuGroup: 'Brake' } },
        ],
        data: { activeTab: 'Brake' },
        canActivate: [AuthorizeService]
    }
]