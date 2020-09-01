"use strict";
var authorize_service_1 = require("../authorize.service");
var engineVersions_1 = require('./engineVersions');
exports.EngineVersionRoutes = [
    {
        path: 'engineversion', component: engineVersions_1.EngineVersionsComponent,
        children: [
            {
                path: '', component: engineVersions_1.EngineVersionListComponent, data: { activeSubMenuTab: 'Version', activeSubMenuGroup: 'Engine' }
            },
        ],
        data: { activeTab: 'Engine' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
