"use strict";
var makes_1 = require('./makes');
var authorize_service_1 = require('../authorize.service');
exports.MakeRoutes = [
    {
        path: 'make', component: makes_1.MakesComponent,
        children: [
            { path: '', component: makes_1.MakeListComponent, data: { activeSubMenuTab: 'Make', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'ReferenceData' },
        canActivate: [authorize_service_1.AuthorizeService]
    },
];
