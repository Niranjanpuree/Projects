import {Routes} from '@angular/router';
import {PCADBReferenceDataComponent} from './referenceData.component';
import { AuthorizeService } from "../../authorize.service";

export const PCADBReferenceDataRoutes: Routes = [
    {
        path: 'pcadb/referencedata', component: PCADBReferenceDataComponent,
        data: { activeTab: 'ReferenceData' },
        canActivate: [AuthorizeService]
    }
];