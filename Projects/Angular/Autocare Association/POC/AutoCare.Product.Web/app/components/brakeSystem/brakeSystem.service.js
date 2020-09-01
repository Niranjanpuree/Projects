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
var core_1 = require('@angular/core');
var constants_warehouse_1 = require('../constants-warehouse');
var httpHelper_1 = require('../httpHelper');
var BrakeSystemService = (function () {
    function BrakeSystemService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    BrakeSystemService.prototype.getAllBrakeSystems = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeSystem);
    };
    BrakeSystemService.prototype.getBrakeSystemDetail = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeSystem + '/' + id);
    };
    BrakeSystemService.prototype.getBrakeSystems = function (brakeSystemNameFilter) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeSystem + "/search", brakeSystemNameFilter);
    };
    BrakeSystemService.prototype.getByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId = function (frontBrakeTypeId, rearBrakeTypeId, brakeABSId) {
        var urlSearch = "/frontBrakeType/" + frontBrakeTypeId + "/rearBrakeType/" + rearBrakeTypeId + "/brakeABS/" + brakeABSId;
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeSystem + urlSearch);
    };
    BrakeSystemService.prototype.addBrakeSystem = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeSystem, data);
    };
    BrakeSystemService.prototype.updateBrakeSystem = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.brakeSystem + '/' + id, data);
    };
    BrakeSystemService.prototype.deleteBrakeSystem = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeSystem + '/delete/' + id, data);
    };
    BrakeSystemService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeSystem + '/changeRequestStaging/' + id);
    };
    BrakeSystemService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeSystem + '/changeRequestStaging/' + id, data);
    };
    BrakeSystemService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], BrakeSystemService);
    return BrakeSystemService;
}());
exports.BrakeSystemService = BrakeSystemService;
