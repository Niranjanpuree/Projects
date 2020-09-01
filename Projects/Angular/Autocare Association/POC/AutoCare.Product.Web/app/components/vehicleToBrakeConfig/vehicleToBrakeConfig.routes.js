"use strict";
var vehicleToBrakeConfigs_1 = require('./vehicleToBrakeConfigs');
var vehicleToBrakeConfig_add_component_1 = require('./vehicleToBrakeConfig-add.component');
var vehicleToBrakeConfig_search_component_1 = require("./vehicleToBrakeConfig-search.component");
var authorize_service_1 = require("../authorize.service");
exports.VehicleToBrakeConfigRoutes = [
    {
        path: 'vehicletobrakeconfig', component: vehicleToBrakeConfigs_1.VehicleToBrakeConfigsComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: vehicleToBrakeConfig_search_component_1.VehicleToBrakeConfigSearchComponent },
            { path: 'add', component: vehicleToBrakeConfig_add_component_1.VehicleToBrakeConfigAddComponent },
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
