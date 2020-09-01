"use strict";
var vehicleToMfrBodyCodes_1 = require('./vehicleToMfrBodyCodes');
var vehicleToMfrBodyCode_add_component_1 = require('./vehicleToMfrBodyCode-add.component');
var vehicleToMfrBodyCode_search_component_1 = require("./vehicleToMfrBodyCode-search.component");
var authorize_service_1 = require("../authorize.service");
exports.VehicleToMfrBodyCodeRoutes = [
    {
        path: 'vehicletomfrbodycode', component: vehicleToMfrBodyCodes_1.VehicleToMfrBodyCodesComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: vehicleToMfrBodyCode_search_component_1.VehicleToMfrBodyCodeSearchComponent },
            { path: 'add', component: vehicleToMfrBodyCode_add_component_1.VehicleToMfrBodyCodeAddComponent },
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
