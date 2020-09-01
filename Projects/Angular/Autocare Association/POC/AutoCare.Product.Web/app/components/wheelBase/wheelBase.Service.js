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
var WheelBaseService = (function () {
    function WheelBaseService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    WheelBaseService.prototype.getAllWheelBase = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.wheelBase);
    };
    WheelBaseService.prototype.getWheelBaseDetail = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.wheelBase + '/' + id);
    };
    WheelBaseService.prototype.getWheelBasebyid = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.wheelBase + "/" + id);
    };
    WheelBaseService.prototype.getByBase = function (data) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.wheelBase + "/getByWheelBaseName", data);
    };
    WheelBaseService.prototype.getByChildNames = function (baseName, wheelBaseMetric) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        var data = {
            base: baseName, wheelBaseMetric: wheelBaseMetric
        };
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.wheelBase + "/getByChildNames", data);
    };
    WheelBaseService.prototype.addWheelBase = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.wheelBase, data);
    };
    WheelBaseService.prototype.updateWheelBase = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.wheelBase + '/' + id, data);
    };
    WheelBaseService.prototype.replaceWheelBase = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.wheelBase + "/replace/" + id, data);
    };
    WheelBaseService.prototype.deleteWheelBase = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.wheelBase + '/delete/' + id, data);
    };
    WheelBaseService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.wheelBase + '/changeRequestStaging/' + id);
    };
    WheelBaseService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.wheelBase + '/changeRequestStaging/' + id, data);
    };
    WheelBaseService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], WheelBaseService);
    return WheelBaseService;
}());
exports.WheelBaseService = WheelBaseService;
