import {Routes} from '@angular/router';
import { BodyNumDoorsComponent, BodyNumDoorsListComponent } from './bodyNumDoors';
import { AuthorizeService } from "../authorize.service";

export const BodyNumDoorRoutes: Routes = [
    {
        path: 'bodynumdoor', component: BodyNumDoorsComponent,
        children: [
            { path: '', component: BodyNumDoorsListComponent, data: { activeSubMenuTab: 'BodyNumDoor', activeSubMenuGroup: 'Body' } },
        ],
        data: { activeTab: 'Body' },
        canActivate: [AuthorizeService]
    }
]