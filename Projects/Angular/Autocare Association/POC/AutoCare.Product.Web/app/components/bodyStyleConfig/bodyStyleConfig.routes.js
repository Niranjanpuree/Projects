"use strict";
var bodyStyleConfig_1 = require('./bodyStyleConfig');
var bodyStyleConfig_add_component_1 = require("./bodyStyleConfig-add.component");
var bodyStyleConfig_replace_component_1 = require("./bodyStyleConfig-replace.component");
var bodyStyleConfig_replaceConfirm_component_1 = require("./bodyStyleConfig-replaceConfirm.component");
var bodyStyleConfig_modify_component_1 = require("./bodyStyleConfig-modify.component");
var bodyStyleConfig_delete_component_1 = require("./bodyStyleConfig-delete.component");
var authorize_service_1 = require("../authorize.service");
exports.BodyStyleConfigRoutes = [
    {
        path: 'bodystyleconfig', component: bodyStyleConfig_1.BodyStyleConfigsComponent,
        children: [
            { path: '', redirectTo: 'add', pathMatch: 'full' },
            { path: 'add', component: bodyStyleConfig_add_component_1.BodyStyleConfigAddComponent },
            { path: 'modify/:id', component: bodyStyleConfig_modify_component_1.BodyStyleConfigModifyComponent },
            { path: 'delete/:id', component: bodyStyleConfig_delete_component_1.BodyStyleConfigDeleteComponent },
            { path: 'replace/:id', component: bodyStyleConfig_replace_component_1.BodyStyleConfigReplaceComponent },
            { path: "replace/confirm/:id", component: bodyStyleConfig_replaceConfirm_component_1.BodyStyleConfigReplaceConfirmComponent }
        ],
        data: { activeTab: 'Systems' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
