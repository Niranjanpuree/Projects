"use strict";
var referenceData_component_1 = require('./referenceData.component');
var authorize_service_1 = require("../../authorize.service");
exports.QDBReferenceDataRoutes = [
    {
        path: 'qdb/referencedata', component: referenceData_component_1.QDBReferenceDataComponent,
        data: { activeTab: 'ReferenceData' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
