import {Routes} from '@angular/router';
import { MfrBodyCodesComponent } from "./mfrBodyCodes";
import { MfrBodyCodeReplaceComponent }        from './mfrBodyCode-replace.component';
import { MfrBodyCodeReplaceConfirmComponent } from "./mfrBodyCode-replaceConfirm.component";

import { AuthorizeService } from "../authorize.service";

export const MfrBodyCodeRoutes: Routes = [
    {
        path: 'mfrbodycode', component: MfrBodyCodesComponent,
        children: [
            { path: '', redirectTo: 'add', pathMatch: 'full' },
            { path: 'replace/:id', component: MfrBodyCodeReplaceComponent },
            { path: "replace/confirm/:id", component: MfrBodyCodeReplaceConfirmComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];