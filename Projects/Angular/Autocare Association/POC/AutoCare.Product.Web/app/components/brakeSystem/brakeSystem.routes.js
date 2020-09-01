"use strict";
var brakesystems_1 = require('./brakesystems');
var authorize_service_1 = require("../authorize.service");
exports.BrakeSystemRoutes = [
    {
        path: 'brakesystem', component: brakesystems_1.BrakeSystemsComponent,
        children: [
            { path: '', component: brakesystems_1.BrakeSystemListComponent, data: { activeSubMenuTab: 'BrakeSystem', activeSubMenuGroup: 'Brake' } },
        ],
        data: { activeTab: 'Brake' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
