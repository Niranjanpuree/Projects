"use strict";
var models_1 = require('./models');
var authorize_service_1 = require("../authorize.service");
exports.ModelRoutes = [
    {
        path: 'model', component: models_1.ModelsComponent,
        children: [
            { path: '', component: models_1.ModelListComponent, data: { activeSubMenuTab: 'Model', activeSubMenuGroup: 'Vehicles' } },
        ],
        data: { activeTab: 'Vehicles' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
