import {Routes} from '@angular/router';
import { BaseVehiclesComponent } from './baseVehicles';
import {BaseVehicleAddComponent} from './baseVehicle-add.component';
import {BaseVehicleReplaceComponent} from './baseVehicle-replace.component';
import {BaseVehicleReplaceConfirmComponent} from './baseVehicle-replaceConfirm.component';
import {BaseVehicleModifyComponent} from "./baseVehicle-modify.component";
import {BaseVehicleDeleteComponent} from "./baseVehicle-delete.component";
import { AuthorizeService } from "../authorize.service";
import { cleanupGuardService }    from '../cleanupGuard.service';

export const BaseVehicleRoutes: Routes = [
    {
        path: 'basevehicle', component: BaseVehiclesComponent,
        children: [
            { path: '', redirectTo: 'add', pathMatch: 'full' },
            { path: 'add', component: BaseVehicleAddComponent },
            { path: 'modify/:id', component: BaseVehicleModifyComponent },
            { path: 'delete/:id', component: BaseVehicleDeleteComponent },
            { path: 'replace/:id', component: BaseVehicleReplaceComponent },
            { path: 'replace/:id/confirm', component: BaseVehicleReplaceConfirmComponent },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [AuthorizeService]
    }
];