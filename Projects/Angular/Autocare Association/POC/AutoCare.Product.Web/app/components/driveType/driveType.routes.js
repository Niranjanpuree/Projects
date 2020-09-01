"use strict";
var driveTypes_1 = require("./driveTypes");
var driveType_replace_component_1 = require('./driveType-replace.component');
var driveType_replaceConfirm_component_1 = require("./driveType-replaceConfirm.component");
var authorize_service_1 = require("../authorize.service");
exports.DriveTypeRoutes = [
    {
        path: 'drivetype', component: driveTypes_1.DriveTypesComponent,
        children: [
            { path: '', redirectTo: 'add', pathMatch: 'full' },
            { path: 'replace/:id', component: driveType_replace_component_1.DriveTypeReplaceComponent },
            { path: "replace/confirm/:id", component: driveType_replaceConfirm_component_1.DriveTypeReplaceConfirmComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
