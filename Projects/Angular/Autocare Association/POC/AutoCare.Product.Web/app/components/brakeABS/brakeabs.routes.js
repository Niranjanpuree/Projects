"use strict";
var brakeABSes_1 = require('./brakeABSes');
var authorize_service_1 = require("../authorize.service");
exports.BrakeAbsRoutes = [
    {
        path: 'brakeabs', component: brakeABSes_1.BrakeABSesComponent,
        children: [
            { path: '', component: brakeABSes_1.BrakeABSListComponent, data: { activeSubMenuTab: 'BrakeABS', activeSubMenuGroup: 'Brake' } },
        ],
        data: { activeTab: 'Brake' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
