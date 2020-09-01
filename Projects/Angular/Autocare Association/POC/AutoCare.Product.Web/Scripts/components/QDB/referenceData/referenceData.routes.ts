import {Routes} from '@angular/router';
import {QDBReferenceDataComponent} from './referenceData.component';
import { AuthorizeService } from "../../authorize.service";

export const QDBReferenceDataRoutes: Routes = [
    {
        path: 'qdb/referencedata', component: QDBReferenceDataComponent,
        data: { activeTab: 'ReferenceData' },
        canActivate: [AuthorizeService]
    }
];