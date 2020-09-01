import {Routes} from '@angular/router';
import { BedLengthComponent, BedLengthListComponent } from './bedLengths';
import { AuthorizeService } from "../authorize.service";

export const BedLengthRoutes: Routes = [
    {
        path: 'bedlength', component: BedLengthComponent,
        children: [
            { path: '', component: BedLengthListComponent, data: { activeSubMenuTab: 'Bed Length', activeSubMenuGroup: 'Bed' } },
        ],
        data: { activeTab: 'Bed Length' },
        canActivate: [AuthorizeService]
    }
]