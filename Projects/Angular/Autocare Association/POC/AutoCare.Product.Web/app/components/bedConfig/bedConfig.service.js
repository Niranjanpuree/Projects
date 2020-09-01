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
var BedConfigService = (function () {
    function BedConfigService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    BedConfigService.prototype.getBedConfigs = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bedConfig);
    };
    BedConfigService.prototype.getBedConfig = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bedConfig + "/" + id);
    };
    BedConfigService.prototype.addBedConfig = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bedConfig, data);
    };
    BedConfigService.prototype.updateBedConfig = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.bedConfig + '/' + id, data);
    };
    BedConfigService.prototype.replaceBedConfig = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.bedConfig + "/replace/" + id, data);
    };
    BedConfigService.prototype.getPendingChangeRequests = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bedConfig + "/pendingChangeRequests");
    };
    BedConfigService.prototype.getByChildIds = function (bedLengthId, bedTypeId) {
        var urlSearch = "/bedLength/" + bedLengthId + "/bedType/" + bedTypeId;
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bedConfig + urlSearch);
    };
    BedConfigService.prototype.deleteBedConfig = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bedConfig + "/delete/" + id, data);
    };
    BedConfigService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bedConfig + "/changeRequestStaging/" + id);
    };
    BedConfigService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bedConfig + '/changeRequestStaging/' + id, data);
    };
    BedConfigService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], BedConfigService);
    return BedConfigService;
}());
exports.BedConfigService = BedConfigService;
