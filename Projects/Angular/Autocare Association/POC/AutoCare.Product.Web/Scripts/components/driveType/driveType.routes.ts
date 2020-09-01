import {Routes} from '@angular/router';
import { DriveTypesComponent } from "./driveTypes";
import { DriveTypeReplaceComponent }        from './driveType-replace.component';
import { DriveTypeReplaceConfirmComponent } from "./driveType-replaceConfirm.component";

import { AuthorizeService } from "../authorize.service";

export const DriveTypeRoutes: Routes = [
    {
        path: 'drivetype', component: DriveTypesComponent,
        children: [
            { path: '', redirectTo: 'add', pathMatch: 'full' },
            { path: 'replace/:id', component: DriveTypeReplaceComponent },
            { path: "replace/confirm/:id", component: DriveTypeReplaceConfirmComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];