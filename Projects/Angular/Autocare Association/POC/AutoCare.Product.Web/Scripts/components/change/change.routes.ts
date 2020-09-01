import {Routes} from '@angular/router';
import { ChangesComponent } from './changes';
import {ChangeSearchComponent} from './change-search.component';
import { ReviewRoutes } from "./review.routes"
import { AuthorizeService } from "../authorize.service";

export const ChangeRoutes: Routes = [
    {
        path: 'change', component: ChangesComponent,
        children: [
            ...ReviewRoutes,
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: ChangeSearchComponent }
        ],
        data: { activeTab: 'Changes' },
        canActivate: [AuthorizeService]
    }
];