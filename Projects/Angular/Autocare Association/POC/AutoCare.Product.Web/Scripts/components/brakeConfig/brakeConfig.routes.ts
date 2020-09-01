import { Routes } from '@angular/router';
import { BrakeConfigsComponent } from './brakeConfigs';
import { BrakeConfigAddComponent }            from "./brakeConfig-add.component";
import { BrakeConfigReplaceComponent }        from "./brakeConfig-replace.component";
import { BrakeConfigReplaceConfirmComponent } from "./brakeConfig-replaceConfirm.component";
import { BrakeConfigModifyComponent }         from "./brakeConfig-modify.component";
import { BrakeConfigDeleteComponent }         from "./brakeConfig-delete.component";

import { AuthorizeService } from "../authorize.service";

export const BrakeConfigRoutes: Routes = [
    {
        path: 'brakeconfig', component: BrakeConfigsComponent,
        children: [
            { path: '', redirectTo: 'add', pathMatch: 'full' },
            { path: 'add', component: BrakeConfigAddComponent },
            { path: 'modify/:id', component: BrakeConfigModifyComponent },
            { path: 'delete/:id', component: BrakeConfigDeleteComponent },
            { path: 'replace/:id', component: BrakeConfigReplaceComponent },
            { path: "replace/confirm/:id", component: BrakeConfigReplaceConfirmComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];