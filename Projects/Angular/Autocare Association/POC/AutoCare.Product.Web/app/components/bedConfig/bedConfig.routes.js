"use strict";
var bedConfigs_1 = require('./bedConfigs');
var bedConfig_add_component_1 = require("./bedConfig-add.component");
var bedConfig_replace_component_1 = require("./bedConfig-replace.component");
var bedConfig_replaceConfirm_component_1 = require("./bedConfig-replaceConfirm.component");
var bedConfig_modify_component_1 = require("./bedConfig-modify.component");
var bedConfig_delete_component_1 = require("./bedConfig-delete.component");
var authorize_service_1 = require("../authorize.service");
exports.BedConfigRoutes = [
    {
        path: 'bedconfig', component: bedConfigs_1.BedConfigsComponent,
        children: [
            { path: '', redirectTo: 'add', pathMatch: 'full' },
            { path: 'add', component: bedConfig_add_component_1.BedConfigAddComponent },
            { path: 'modify/:id', component: bedConfig_modify_component_1.BedConfigModifyComponent },
            { path: 'delete/:id', component: bedConfig_delete_component_1.BedConfigDeleteComponent },
            { path: 'replace/:id', component: bedConfig_replace_component_1.BedConfigReplaceComponent },
            { path: "replace/confirm/:id", component: bedConfig_replaceConfirm_component_1.BedConfigReplaceConfirmComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
