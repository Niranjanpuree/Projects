"use strict";
var submodels_1 = require('./submodels');
var authorize_service_1 = require("../authorize.service");
exports.SubModelRoutes = [
    {
        path: 'submodel', component: submodels_1.SubModelsComponent,
        children: [
            { path: '', component: submodels_1.SubModelListComponent, data: { activeSubMenuTab: 'SubModel', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'ReferenceData' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
