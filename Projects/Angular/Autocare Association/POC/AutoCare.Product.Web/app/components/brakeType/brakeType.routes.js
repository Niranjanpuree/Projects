"use strict";
var brakeTypes_1 = require('./brakeTypes');
var authorize_service_1 = require("../authorize.service");
exports.BrakeTypeRoutes = [
    {
        path: 'braketype', component: brakeTypes_1.BrakeTypesComponent,
        children: [
            { path: '', component: brakeTypes_1.BrakeTypeListComponent, data: { activeSubMenuTab: 'BrakeType', activeSubMenuGroup: 'Brake' } },
        ],
        data: { activeTab: 'Brake' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
