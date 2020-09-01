import {Routes} from '@angular/router';
import { AuthorizeService } from "../authorize.service";
import { EngineDesignationsComponent, EngineDesignationListComponent } from './engineDesignations';

export const EngineDesignationRoutes: Routes = [
    {
        path: 'enginedesignation', component: EngineDesignationsComponent,
        children: [
            {
                path: '', component: EngineDesignationListComponent, data: { activeSubMenuTab: 'Designation', activeSubMenuGroup: 'Engine'}
            },
        ],
        data: { activeTab: 'Engine' },
        canActivate: [AuthorizeService]
    }
]