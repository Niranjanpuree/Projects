import { Routes } from '@angular/router';
import { WheelBasesComponent } from './wheelBases';
import { WheelBaseReplaceComponent }        from "./wheelBase-replace.component";
import { WheelBaseReplaceConfirmComponent } from "./wheelBase-replaceConfirm.component";
import { AuthorizeService } from "../authorize.service";

export const WheelBaseRoutes: Routes = [
    {
        path: 'wheelbase', component: WheelBasesComponent,
        children: [
            { path: '', redirectTo: 'replace', pathMatch: 'full' },
            { path: 'replace/:id', component: WheelBaseReplaceComponent },
            { path: "replace/confirm/:id", component: WheelBaseReplaceConfirmComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [AuthorizeService]
    }
];