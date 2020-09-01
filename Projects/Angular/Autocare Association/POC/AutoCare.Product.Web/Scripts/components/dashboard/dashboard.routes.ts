import {Routes} from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { AuthorizeService } from '../authorize.service';

export const DashboardRoutes: Routes = [
    { path: 'dashboard', component: DashboardComponent, canActivate: [AuthorizeService] },
]