"use strict";
var dashboard_component_1 = require('./dashboard.component');
var authorize_service_1 = require('../authorize.service');
exports.DashboardRoutes = [
    { path: 'dashboard', component: dashboard_component_1.DashboardComponent, canActivate: [authorize_service_1.AuthorizeService] },
];
