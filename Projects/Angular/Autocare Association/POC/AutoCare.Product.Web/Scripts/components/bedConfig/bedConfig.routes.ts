import {Routes} from '@angular/router';
import { BedConfigsComponent } from './bedConfigs';
import { BedConfigAddComponent }            from "./bedConfig-add.component";
import { BedConfigReplaceComponent }        from "./bedConfig-replace.component";
import { BedConfigReplaceConfirmComponent } from "./bedConfig-replaceConfirm.component";
import { BedConfigModifyComponent }         from "./bedConfig-modify.component";
import { BedConfigDeleteComponent }         from "./bedConfig-delete.component";

import { AuthorizeService } from "../authorize.service";

export const BedConfigRoutes: Routes = [
    {
        path: 'bedconfig', component: BedConfigsComponent,
        children: [
            { path: '', redirectTo: 'add', pathMatch: 'full' },
            { path: 'add', component: BedConfigAddComponent },
            { path: 'modify/:id', component: BedConfigModifyComponent },
            { path: 'delete/:id', component: BedConfigDeleteComponent },
            { path: 'replace/:id', component: BedConfigReplaceComponent },
            { path: "replace/confirm/:id", component: BedConfigReplaceConfirmComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];