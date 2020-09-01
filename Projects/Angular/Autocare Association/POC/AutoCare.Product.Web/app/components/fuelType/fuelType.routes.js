"use strict";
var authorize_service_1 = require("../authorize.service");
var fuelTypes_1 = require('./fuelTypes');
exports.FuelTypeRoutes = [
    {
        path: 'fueltype', component: fuelTypes_1.FuelTypesComponent,
        children: [
            {
                path: '', component: fuelTypes_1.FuelTypeListComponent, data: { activeSubMenuTab: 'FuelType', activeSubMenuGroup: 'Engine' }
            },
        ],
        data: { activeTab: 'Engine' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
