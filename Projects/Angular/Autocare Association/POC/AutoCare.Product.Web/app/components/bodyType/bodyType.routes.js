"use strict";
var bodyTypes_1 = require('./bodyTypes');
var authorize_service_1 = require("../authorize.service");
exports.BodyTypeRoutes = [
    {
        path: 'bodytype', component: bodyTypes_1.BodyTypesComponent,
        children: [
            { path: '', component: bodyTypes_1.BodyTypeListComponent, data: { activeSubMenuTab: 'BodyType', activeSubMenuGroup: 'Body' } },
        ],
        data: { activeTab: 'Body' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
