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
var FuelTypeService = (function () {
    function FuelTypeService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    FuelTypeService.prototype.getAllFuelTypes = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.fuelType);
    };
    FuelTypeService.prototype.getFuelTypeDetail = function (id) {
        debugger;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.fuelType + '/' + id);
    };
    FuelTypeService.prototype.getFuelTypes = function (fuelTypeNameFilter) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.fuelType + "/search", fuelTypeNameFilter);
    };
    FuelTypeService.prototype.getByFuelTypeId = function (fuelTypeId) {
        var urlSearch = "/fuelType/" + fuelTypeId;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.fuelType + urlSearch);
    };
    FuelTypeService.prototype.addFuelType = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.fuelType, data);
    };
    FuelTypeService.prototype.updateFuelType = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.fuelType + '/' + id, data);
    };
    FuelTypeService.prototype.deleteFuelType = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.fuelType + '/delete/' + id, data);
    };
    FuelTypeService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.fuelType + '/changeRequestStaging/' + id);
    };
    FuelTypeService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.fuelType + '/changeRequestStaging/' + id, data);
    };
    FuelTypeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], FuelTypeService);
    return FuelTypeService;
}());
exports.FuelTypeService = FuelTypeService;
