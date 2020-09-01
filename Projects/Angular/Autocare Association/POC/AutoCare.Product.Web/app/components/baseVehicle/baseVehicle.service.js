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
var BaseVehicleService = (function () {
    function BaseVehicleService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    BaseVehicleService.prototype.getBaseVehicles = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle);
    };
    BaseVehicleService.prototype.getBaseVehicle = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle + "/" + id);
    };
    BaseVehicleService.prototype.getModelsByYearIdAndMakeId = function (yearId, makeId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.year + "/" + yearId + "/makes/" + makeId + "/models");
    };
    BaseVehicleService.prototype.addBaseVehicle = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle, data);
    };
    BaseVehicleService.prototype.getPendingChangeRequests = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle + "/pendingChangeRequests"); //TODO: move to constants
    };
    BaseVehicleService.prototype.updateBaseVehicle = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle + "/" + id, data);
    };
    BaseVehicleService.prototype.replaceBaseVehicle = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle + "/replace/" + id, data);
    };
    BaseVehicleService.prototype.deleteBaseVehicle = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle + "/delete/" + id, data);
    };
    BaseVehicleService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle + "/changeRequestStaging/" + id);
    };
    BaseVehicleService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle + '/changeRequestStaging/' + id, data);
    };
    BaseVehicleService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], BaseVehicleService);
    return BaseVehicleService;
}());
exports.BaseVehicleService = BaseVehicleService;
