import {Routes} from '@angular/router';
import { AuthorizeService } from "../authorize.service";
import { EngineVersionsComponent, EngineVersionListComponent } from './engineVersions';

export const EngineVersionRoutes: Routes = [
    {
        path: 'engineversion', component: EngineVersionsComponent,
        children: [
            {
                path: '', component: EngineVersionListComponent, data: { activeSubMenuTab: 'Version', activeSubMenuGroup: 'Engine'}
            },
        ],
        data: { activeTab: 'Engine' },
        canActivate: [AuthorizeService]
    }
]