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
var FuelDeliverySubTypeService = (function () {
    function FuelDeliverySubTypeService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    FuelDeliverySubTypeService.prototype.getAllFuelDeliverySubTypes = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.fuelDeliverySubType);
    };
    FuelDeliverySubTypeService.prototype.get = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.fuelDeliverySubType /*+ '/count/20'*/);
    };
    FuelDeliverySubTypeService.prototype.getByFilter = function (fuelDeliverySubTypeNameFilter) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.fuelDeliverySubType + '/search', fuelDeliverySubTypeNameFilter);
    };
    FuelDeliverySubTypeService.prototype.add = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.fuelDeliverySubType, data);
    };
    FuelDeliverySubTypeService.prototype.update = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.fuelDeliverySubType + '/' + id, data);
    };
    FuelDeliverySubTypeService.prototype.delete = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.fuelDeliverySubType + '/delete/' + id, data);
    };
    FuelDeliverySubTypeService.prototype.getById = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.fuelDeliverySubType + '/' + id);
    };
    FuelDeliverySubTypeService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.fuelDeliverySubType + '/changeRequestStaging/' + id);
    };
    FuelDeliverySubTypeService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.fuelDeliverySubType + '/changeRequestStaging/' + id, data);
    };
    FuelDeliverySubTypeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], FuelDeliverySubTypeService);
    return FuelDeliverySubTypeService;
}());
exports.FuelDeliverySubTypeService = FuelDeliverySubTypeService;
