import { Routes } from '@angular/router';
import { SubModelsComponent, SubModelListComponent } from './submodels';
import { AuthorizeService } from "../authorize.service";

export const SubModelRoutes: Routes = [
    {
        path: 'submodel', component: SubModelsComponent,
        children: [
            { path: '', component: SubModelListComponent, data: { activeSubMenuTab: 'SubModel', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'ReferenceData' },
        canActivate: [AuthorizeService]
    }
]