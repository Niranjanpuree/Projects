"use strict";
var bedLengths_1 = require('./bedLengths');
var authorize_service_1 = require("../authorize.service");
exports.BedLengthRoutes = [
    {
        path: 'bedlength', component: bedLengths_1.BedLengthComponent,
        children: [
            { path: '', component: bedLengths_1.BedLengthListComponent, data: { activeSubMenuTab: 'Bed Length', activeSubMenuGroup: 'Bed' } },
        ],
        data: { activeTab: 'Bed Length' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
