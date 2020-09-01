"use strict";
var authorize_service_1 = require("../authorize.service");
var bedTypes_1 = require('./bedTypes');
exports.BedTypeRoutes = [
    {
        path: 'bedtype', component: bedTypes_1.BedTypesComponent,
        children: [
            {
                path: '', component: bedTypes_1.BedTypeListComponent, data: { activeSubMenuTab: 'Bed Type', activeSubMenuGroup: 'Bed' }
            },
        ],
        data: { activeTab: 'Bed' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
