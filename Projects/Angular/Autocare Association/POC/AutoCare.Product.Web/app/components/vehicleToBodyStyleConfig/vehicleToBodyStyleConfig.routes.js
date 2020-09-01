"use strict";
var vehicleToBodyStyleConfigs_1 = require('./vehicleToBodyStyleConfigs');
var vehicleToBodyStyleConfig_add_component_1 = require('./vehicleToBodyStyleConfig-add.component');
var vehicleToBodyStyleConfig_search_component_1 = require("./vehicleToBodyStyleConfig-search.component");
var authorize_service_1 = require("../authorize.service");
exports.VehicleToBodyStyleConfigRoutes = [
    {
        path: 'vehicletobodystyleconfig', component: vehicleToBodyStyleConfigs_1.VehicleToBodyStyleConfigsComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: vehicleToBodyStyleConfig_search_component_1.VehicleToBodyStyleConfigSearchComponent },
            { path: 'add', component: vehicleToBodyStyleConfig_add_component_1.VehicleToBodyStyleConfigAddComponent },
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
