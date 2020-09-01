import {Routes} from '@angular/router';
import { BodyTypesComponent, BodyTypeListComponent } from './bodyTypes';
import { AuthorizeService } from "../authorize.service";

export const BodyTypeRoutes: Routes = [
    {
        path: 'bodytype', component: BodyTypesComponent,
        children: [
            { path: '', component: BodyTypeListComponent, data: { activeSubMenuTab: 'BodyType', activeSubMenuGroup: 'Body' } },
        ],
        data: { activeTab: 'Body' },
        canActivate: [AuthorizeService]
    }
]