"use strict";
var vehicleToBedConfigs_1 = require('./vehicleToBedConfigs');
var vehicleToBedConfig_add_component_1 = require('./vehicleToBedConfig-add.component');
var vehicleToBedConfig_search_component_1 = require("./vehicleToBedConfig-search.component");
var authorize_service_1 = require("../authorize.service");
exports.VehicleToBedConfigRoutes = [
    {
        path: 'vehicletobedconfig', component: vehicleToBedConfigs_1.VehicleToBedConfigsComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: vehicleToBedConfig_search_component_1.VehicleToBedConfigSearchComponent },
            { path: 'add', component: vehicleToBedConfig_add_component_1.VehicleToBedConfigAddComponent },
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
