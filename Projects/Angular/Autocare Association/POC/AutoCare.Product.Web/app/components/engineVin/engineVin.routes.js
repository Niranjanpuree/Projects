"use strict";
var authorize_service_1 = require("../authorize.service");
var engineVins_1 = require('./engineVins');
exports.EngineVinRoutes = [
    {
        path: 'enginevin', component: engineVins_1.EngineVinsComponent,
        children: [
            {
                path: '', component: engineVins_1.EngineVinListComponent, data: { activeSubMenuTab: 'Vin', activeSubMenuGroup: 'Engine' }
            },
        ],
        data: { activeTab: 'Engine' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
