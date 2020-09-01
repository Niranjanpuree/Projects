"use strict";
var regions_1 = require('./regions');
var authorize_service_1 = require("../authorize.service");
exports.RegionRoutes = [
    {
        path: 'region', component: regions_1.RegionsComponent,
        children: [
            { path: '', component: regions_1.RegionListComponent, data: { activeSubMenuTab: 'Region', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
