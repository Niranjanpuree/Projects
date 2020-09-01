import {Routes} from '@angular/router';
import { BodyStyleConfigsComponent } from './bodyStyleConfig';
import { BodyStyleConfigAddComponent }            from "./bodyStyleConfig-add.component";
import { BodyStyleConfigReplaceComponent }        from "./bodyStyleConfig-replace.component";
import { BodyStyleConfigReplaceConfirmComponent } from "./bodyStyleConfig-replaceConfirm.component";
import { BodyStyleConfigModifyComponent }         from "./bodyStyleConfig-modify.component";
import { BodyStyleConfigDeleteComponent }         from "./bodyStyleConfig-delete.component";

import { AuthorizeService } from "../authorize.service";

export const BodyStyleConfigRoutes: Routes = [
    {
        path: 'bodystyleconfig', component: BodyStyleConfigsComponent,
        children: [
            { path: '', redirectTo: 'add', pathMatch: 'full' },
            { path: 'add', component: BodyStyleConfigAddComponent },
            { path: 'modify/:id', component: BodyStyleConfigModifyComponent },
            { path: 'delete/:id', component: BodyStyleConfigDeleteComponent },
            { path: 'replace/:id', component: BodyStyleConfigReplaceComponent },
            { path: "replace/confirm/:id", component: BodyStyleConfigReplaceConfirmComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];