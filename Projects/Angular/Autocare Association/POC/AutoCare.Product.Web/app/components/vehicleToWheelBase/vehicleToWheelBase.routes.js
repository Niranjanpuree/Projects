"use strict";
var vehicleToWheelBases_component_1 = require('./vehicleToWheelBases.component');
var vehicleToWheelBase_add_component_1 = require('./vehicleToWheelBase-add.component');
var vehicleToWheelBase_search_component_1 = require("./vehicleToWheelBase-search.component");
var authorize_service_1 = require("../authorize.service");
exports.VehicleToWheelBaseRoutes = [
    {
        path: 'vehicletowheelbase', component: vehicleToWheelBases_component_1.VehicleToWheelBasesComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: vehicleToWheelBase_search_component_1.VehicleToWheelBaseSearchComponent },
            { path: 'add', component: vehicleToWheelBase_add_component_1.VehicleToWheelBaseAddComponent },
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
