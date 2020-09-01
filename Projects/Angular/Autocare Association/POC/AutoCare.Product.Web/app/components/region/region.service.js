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
require('rxjs/Rx');
var constants_warehouse_1 = require('../constants-warehouse');
var httpHelper_1 = require('../httpHelper');
var RegionService = (function () {
    function RegionService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    RegionService.prototype.getRegion = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.region);
    };
    RegionService.prototype.getRegionDetail = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.region + '/' + id);
    };
    RegionService.prototype.getRegionByNameFilter = function (regionNameFilter) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.region + '/search', regionNameFilter);
    };
    RegionService.prototype.getRegionsByBaseVehicleIdAndSubModelId = function (baseVehicleId, subModelId) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle + '/' + baseVehicleId + '/subModels/' + subModelId + '/regions');
    };
    RegionService.prototype.addRegion = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.region, data);
    };
    RegionService.prototype.updateRegion = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.region + '/' + id, data);
    };
    RegionService.prototype.deleteRegionPost = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.region + '/delete/' + id, data);
    };
    RegionService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.region + '/changeRequestStaging/' + id);
    };
    RegionService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.region + '/changeRequestStaging/' + id, data);
    };
    RegionService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], RegionService);
    return RegionService;
}());
exports.RegionService = RegionService;
