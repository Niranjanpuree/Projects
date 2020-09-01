"use strict";
var vehicleToDriveTypes_1 = require('./vehicleToDriveTypes');
var authorize_service_1 = require("../authorize.service");
exports.VehicleToDriveTypeRoutes = [
    {
        path: 'vehicletodrivetype', component: vehicleToDriveTypes_1.VehicleToDriveTypesComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: vehicleToDriveTypes_1.VehicleToDriveTypeSearchComponent },
            { path: 'add', component: vehicleToDriveTypes_1.VehicleToDriveTypeAddComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
