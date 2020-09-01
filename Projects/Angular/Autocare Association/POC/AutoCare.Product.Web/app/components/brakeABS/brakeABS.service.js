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
var BrakeABSService = (function () {
    function BrakeABSService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    BrakeABSService.prototype.getAllBrakeABSes = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeABS);
    };
    BrakeABSService.prototype.getBrakeABSDetail = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeABS + '/' + id);
    };
    BrakeABSService.prototype.getBrakeABSs = function (brakeABSNameFilter) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeABS + "/search", brakeABSNameFilter);
    };
    BrakeABSService.prototype.getByFrontBrakeTypeIdRearBrakeTypeId = function (frontBrakeTypeId, rearBrakeTypeId) {
        var urlSearch = "/frontBrakeType/" + frontBrakeTypeId + "/rearBrakeType/" + rearBrakeTypeId;
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeABS + urlSearch);
    };
    BrakeABSService.prototype.addBrakeABS = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeABS, data);
    };
    BrakeABSService.prototype.updateBrakeABS = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.brakeABS + '/' + id, data);
    };
    BrakeABSService.prototype.deleteBrakeABS = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeABS + '/delete/' + id, data);
    };
    BrakeABSService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeABS + '/changeRequestStaging/' + id);
    };
    BrakeABSService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeABS + '/changeRequestStaging/' + id, data);
    };
    BrakeABSService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], BrakeABSService);
    return BrakeABSService;
}());
exports.BrakeABSService = BrakeABSService;
