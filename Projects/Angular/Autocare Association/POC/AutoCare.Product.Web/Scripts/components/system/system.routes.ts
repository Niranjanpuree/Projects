import { Routes }             from "@angular/router";
import { SystemsComponent }         from "./systems.component";
import { SystemSearchComponent }    from "./system-search.component";
import { AuthorizeService }         from "../authorize.service";

export const SystemRoutes: Routes = [
    {
        path: 'system', component: SystemsComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: SystemSearchComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];