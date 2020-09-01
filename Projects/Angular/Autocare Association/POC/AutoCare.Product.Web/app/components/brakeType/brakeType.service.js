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
var BrakeTypeService = (function () {
    function BrakeTypeService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    BrakeTypeService.prototype.getAllBrakeTypes = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeType);
    };
    BrakeTypeService.prototype.getBrakeTypeDetail = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeType + '/' + id);
    };
    BrakeTypeService.prototype.getBrakeTypes = function (brakeTypeNameFilter) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeType + "/search", brakeTypeNameFilter);
    };
    BrakeTypeService.prototype.getByFrontBrakeTypeId = function (frontBrakeTypeId) {
        var urlSearch = "/frontBrakeType/" + frontBrakeTypeId;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeType + urlSearch);
    };
    BrakeTypeService.prototype.addBrakeType = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeType, data);
    };
    BrakeTypeService.prototype.updateBrakeType = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.brakeType + '/' + id, data);
    };
    BrakeTypeService.prototype.deleteBrakeType = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeType + '/delete/' + id, data);
    };
    BrakeTypeService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.brakeType + '/changeRequestStaging/' + id);
    };
    BrakeTypeService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.brakeType + '/changeRequestStaging/' + id, data);
    };
    BrakeTypeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], BrakeTypeService);
    return BrakeTypeService;
}());
exports.BrakeTypeService = BrakeTypeService;
