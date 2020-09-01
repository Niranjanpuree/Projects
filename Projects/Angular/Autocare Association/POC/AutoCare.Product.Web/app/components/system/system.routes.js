"use strict";
var systems_component_1 = require("./systems.component");
var system_search_component_1 = require("./system-search.component");
var authorize_service_1 = require("../authorize.service");
exports.SystemRoutes = [
    {
        path: 'system', component: systems_component_1.SystemsComponent,
        children: [
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: system_search_component_1.SystemSearchComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
