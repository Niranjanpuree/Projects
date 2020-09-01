"use strict";
var fuelDeliverySubTypes_1 = require('./fuelDeliverySubTypes');
var authorize_service_1 = require('../authorize.service');
exports.FuelDeliverySubTypeRoutes = [
    {
        path: 'fuelDeliverySubType', component: fuelDeliverySubTypes_1.FuelDeliverySubTypesComponent,
        children: [
            { path: '', component: fuelDeliverySubTypes_1.FuelDeliverySubTypeListComponent, data: { activeSubMenuTab: 'FuelDeliverySubType', activeSubMenuGroup: 'Engine' } },
        ],
        data: { activeTab: 'ReferenceData' },
        canActivate: [authorize_service_1.AuthorizeService]
    },
];
