import { Routes } from '@angular/router';
import { ModelsComponent, ModelListComponent } from './models';
import { AuthorizeService } from "../authorize.service";

export const ModelRoutes: Routes = [
    {
        path: 'model', component: ModelsComponent,
        children: [
            { path: '', component: ModelListComponent, data: { activeSubMenuTab: 'Model', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [AuthorizeService]
    }
]