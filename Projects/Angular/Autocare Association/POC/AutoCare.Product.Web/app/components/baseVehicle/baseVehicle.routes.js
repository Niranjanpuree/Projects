"use strict";
var baseVehicles_1 = require('./baseVehicles');
var baseVehicle_add_component_1 = require('./baseVehicle-add.component');
var baseVehicle_replace_component_1 = require('./baseVehicle-replace.component');
var baseVehicle_replaceConfirm_component_1 = require('./baseVehicle-replaceConfirm.component');
var baseVehicle_modify_component_1 = require("./baseVehicle-modify.component");
var baseVehicle_delete_component_1 = require("./baseVehicle-delete.component");
var authorize_service_1 = require("../authorize.service");
exports.BaseVehicleRoutes = [
    {
        path: 'basevehicle', component: baseVehicles_1.BaseVehiclesComponent,
        children: [
            { path: '', redirectTo: 'add', pathMatch: 'full' },
            { path: 'add', component: baseVehicle_add_component_1.BaseVehicleAddComponent },
            { path: 'modify/:id', component: baseVehicle_modify_component_1.BaseVehicleModifyComponent },
            { path: 'delete/:id', component: baseVehicle_delete_component_1.BaseVehicleDeleteComponent },
            { path: 'replace/:id', component: baseVehicle_replace_component_1.BaseVehicleReplaceComponent },
            { path: 'replace/:id/confirm', component: baseVehicle_replaceConfirm_component_1.BaseVehicleReplaceConfirmComponent },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
