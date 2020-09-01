"use strict";
var bodyNumDoors_1 = require('./bodyNumDoors');
var authorize_service_1 = require("../authorize.service");
exports.BodyNumDoorRoutes = [
    {
        path: 'bodynumdoor', component: bodyNumDoors_1.BodyNumDoorsComponent,
        children: [
            { path: '', component: bodyNumDoors_1.BodyNumDoorsListComponent, data: { activeSubMenuTab: 'BodyNumDoor', activeSubMenuGroup: 'Body' } },
        ],
        data: { activeTab: 'Body' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
