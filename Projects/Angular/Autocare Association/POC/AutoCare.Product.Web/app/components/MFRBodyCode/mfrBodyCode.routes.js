"use strict";
var mfrBodyCodes_1 = require("./mfrBodyCodes");
var mfrBodyCode_replace_component_1 = require('./mfrBodyCode-replace.component');
var mfrBodyCode_replaceConfirm_component_1 = require("./mfrBodyCode-replaceConfirm.component");
var authorize_service_1 = require("../authorize.service");
exports.MfrBodyCodeRoutes = [
    {
        path: 'mfrbodycode', component: mfrBodyCodes_1.MfrBodyCodesComponent,
        children: [
            { path: '', redirectTo: 'add', pathMatch: 'full' },
            { path: 'replace/:id', component: mfrBodyCode_replace_component_1.MfrBodyCodeReplaceComponent },
            { path: "replace/confirm/:id", component: mfrBodyCode_replaceConfirm_component_1.MfrBodyCodeReplaceConfirmComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
