"use strict";
var authorize_service_1 = require("../authorize.service");
var engineDesignations_1 = require('./engineDesignations');
exports.EngineDesignationRoutes = [
    {
        path: 'enginedesignation', component: engineDesignations_1.EngineDesignationsComponent,
        children: [
            {
                path: '', component: engineDesignations_1.EngineDesignationListComponent, data: { activeSubMenuTab: 'Designation', activeSubMenuGroup: 'Engine' }
            },
        ],
        data: { activeTab: 'Engine' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
