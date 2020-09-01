import { Routes } from '@angular/router';
import { YearsComponent, YearListComponent } from './years';
import { AuthorizeService } from "../authorize.service";

export const YearRoutes: Routes = [
    {
        path: 'year', component: YearsComponent,
        children: [
            { path: '', component: YearListComponent, data: { activeSubMenuTab: 'Year', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [AuthorizeService]
    },
]