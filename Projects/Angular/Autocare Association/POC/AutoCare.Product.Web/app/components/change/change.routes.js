"use strict";
var changes_1 = require('./changes');
var change_search_component_1 = require('./change-search.component');
var review_routes_1 = require("./review.routes");
var authorize_service_1 = require("../authorize.service");
exports.ChangeRoutes = [
    {
        path: 'change', component: changes_1.ChangesComponent,
        children: review_routes_1.ReviewRoutes.concat([
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'search', component: change_search_component_1.ChangeSearchComponent }
        ]),
        data: { activeTab: 'Changes' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
