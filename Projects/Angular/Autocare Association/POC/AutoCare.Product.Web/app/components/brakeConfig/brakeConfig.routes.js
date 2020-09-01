"use strict";
var brakeConfigs_1 = require('./brakeConfigs');
var brakeConfig_add_component_1 = require("./brakeConfig-add.component");
var brakeConfig_replace_component_1 = require("./brakeConfig-replace.component");
var brakeConfig_replaceConfirm_component_1 = require("./brakeConfig-replaceConfirm.component");
var brakeConfig_modify_component_1 = require("./brakeConfig-modify.component");
var brakeConfig_delete_component_1 = require("./brakeConfig-delete.component");
var authorize_service_1 = require("../authorize.service");
exports.BrakeConfigRoutes = [
    {
        path: 'brakeconfig', component: brakeConfigs_1.BrakeConfigsComponent,
        children: [
            { path: '', redirectTo: 'add', pathMatch: 'full' },
            { path: 'add', component: brakeConfig_add_component_1.BrakeConfigAddComponent },
            { path: 'modify/:id', component: brakeConfig_modify_component_1.BrakeConfigModifyComponent },
            { path: 'delete/:id', component: brakeConfig_delete_component_1.BrakeConfigDeleteComponent },
            { path: 'replace/:id', component: brakeConfig_replace_component_1.BrakeConfigReplaceComponent },
            { path: "replace/confirm/:id", component: brakeConfig_replaceConfirm_component_1.BrakeConfigReplaceConfirmComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
