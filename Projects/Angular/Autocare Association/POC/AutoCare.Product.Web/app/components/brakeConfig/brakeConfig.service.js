"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var constants_warehouse_1 = require("../constants-warehouse");
var httpHelper_1 = require("../httpHelper");
var BrakeConfigService = (function () {
    function BrakeConfigService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    BrakeConfigService.prototype.getBrakeConfigs = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeConfig);
    };
    BrakeConfigService.prototype.getBrakeConfig = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeConfig + "/" + id);
    };
    BrakeConfigService.prototype.addBrakeConfig = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeConfig, data);
    };
    BrakeConfigService.prototype.updateBrakeConfig = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.brakeConfig + '/' + id, data);
    };
    BrakeConfigService.prototype.replaceBrakeConfig = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.brakeConfig + "/replace/" + id, data);
    };
    BrakeConfigService.prototype.getPendingChangeRequests = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeConfig + "/pendingChangeRequests");
    };
    BrakeConfigService.prototype.getByChildIds = function (frontBrakeTypeId, rearBrakeTypeId, brakeABSId, brakeSystemId) {
        var urlSearch = "/frontBrakeType/" + frontBrakeTypeId + "/rearBrakeType/" + rearBrakeTypeId + "/brakeABS/" + brakeABSId + "/brakeSystem/" + brakeSystemId;
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeConfig + urlSearch);
    };
    BrakeConfigService.prototype.deleteBrakeConfig = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeConfig + "/delete/" + id, data);
    };
    BrakeConfigService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeConfig + "/changeRequestStaging/" + id);
    };
    BrakeConfigService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeConfig + '/changeRequestStaging/' + id, data);
    };
    BrakeConfigService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], BrakeConfigService);
    return BrakeConfigService;
}());
exports.BrakeConfigService = BrakeConfigService;
