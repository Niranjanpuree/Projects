"use strict";
var vehicletypes_1 = require('./vehicletypes');
var authorize_service_1 = require("../authorize.service");
exports.VehicleTypeRoutes = [
    {
        path: 'vehicletype', component: vehicletypes_1.VehicleTypesComponent,
        children: [
            { path: '', component: vehicletypes_1.VehicleTypeListComponent, data: { activeSubMenuTab: 'VehicleType', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
