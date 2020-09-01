"use strict";
var vehicleTypeGroups_1 = require('./vehicleTypeGroups');
var authorize_service_1 = require("../authorize.service");
exports.VehicleTypeGroupRoutes = [
    {
        path: 'vehicletypegroup', component: vehicleTypeGroups_1.VehicleTypeGroupsComponent,
        children: [
            { path: '', component: vehicleTypeGroups_1.VehicleTypeGroupListComponent, data: { activeSubMenuTab: 'VehicleTypeGroup', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
