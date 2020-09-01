"use strict";
var vehicles_1 = require('./vehicles');
var vehicle_search_component_1 = require('./vehicle-search.component');
var vehicle_modify_component_1 = require('./vehicle-modify.component');
var vehicle_delete_component_1 = require('./vehicle-delete.component');
var vehicle_add_component_1 = require('./vehicle-add.component');
var authorize_service_1 = require("../authorize.service");
exports.VehicleRoutes = [
    {
        path: 'vehicle', component: vehicles_1.VehiclesComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: vehicle_search_component_1.VehicleSearchComponent },
            { path: 'add/:basevid', component: vehicle_add_component_1.VehicleAddComponent },
            { path: 'modify/:id', component: vehicle_modify_component_1.VehicleModifyComponent },
            { path: 'delete/:id', component: vehicle_delete_component_1.VehicleDeleteComponent },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
