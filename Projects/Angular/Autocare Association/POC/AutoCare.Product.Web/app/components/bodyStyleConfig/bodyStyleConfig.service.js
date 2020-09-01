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
var BodyStyleConfigService = (function () {
    function BodyStyleConfigService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    BodyStyleConfigService.prototype.getBodyStyleConfigs = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bodyStyleConfig);
    };
    BodyStyleConfigService.prototype.getBodyStyleConfig = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bodyStyleConfig + "/" + id);
    };
    BodyStyleConfigService.prototype.addBodyStyleConfig = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bodyStyleConfig, data);
    };
    BodyStyleConfigService.prototype.updateBodyStyleConfig = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.bodyStyleConfig + '/' + id, data);
    };
    BodyStyleConfigService.prototype.replaceBodyStyleConfig = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.bodyStyleConfig + "/replace/" + id, data);
    };
    BodyStyleConfigService.prototype.getPendingChangeRequests = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bodyStyleConfig + "/pendingChangeRequests");
    };
    BodyStyleConfigService.prototype.getByChildIds = function (bodyNumberDoorsId, bodyTypeId) {
        var urlSearch = "/bodyNumDoors/" + bodyNumberDoorsId + "/bodyType/" + bodyTypeId;
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bodyStyleConfig + urlSearch);
    };
    BodyStyleConfigService.prototype.deleteBodyStyleConfig = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bodyStyleConfig + "/delete/" + id, data);
    };
    BodyStyleConfigService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bodyStyleConfig + "/changeRequestStaging/" + id);
    };
    BodyStyleConfigService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bodyStyleConfig + '/changeRequestStaging/' + id, data);
    };
    BodyStyleConfigService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], BodyStyleConfigService);
    return BodyStyleConfigService;
}());
exports.BodyStyleConfigService = BodyStyleConfigService;
