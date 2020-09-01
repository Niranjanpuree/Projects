"use strict";
var years_1 = require('./years');
var authorize_service_1 = require("../authorize.service");
exports.YearRoutes = [
    {
        path: 'year', component: years_1.YearsComponent,
        children: [
            { path: '', component: years_1.YearListComponent, data: { activeSubMenuTab: 'Year', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [authorize_service_1.AuthorizeService]
    },
];
