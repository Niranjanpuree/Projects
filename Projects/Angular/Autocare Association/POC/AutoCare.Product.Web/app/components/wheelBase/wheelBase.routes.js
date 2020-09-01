"use strict";
var wheelBases_1 = require('./wheelBases');
var wheelBase_replace_component_1 = require("./wheelBase-replace.component");
var wheelBase_replaceConfirm_component_1 = require("./wheelBase-replaceConfirm.component");
var authorize_service_1 = require("../authorize.service");
exports.WheelBaseRoutes = [
    {
        path: 'wheelbase', component: wheelBases_1.WheelBasesComponent,
        children: [
            { path: '', redirectTo: 'replace', pathMatch: 'full' },
            { path: 'replace/:id', component: wheelBase_replace_component_1.WheelBaseReplaceComponent },
            { path: "replace/confirm/:id", component: wheelBase_replaceConfirm_component_1.WheelBaseReplaceConfirmComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
