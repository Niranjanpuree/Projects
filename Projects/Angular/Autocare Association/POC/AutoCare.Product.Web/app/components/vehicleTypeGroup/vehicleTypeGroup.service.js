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
var VehicleTypeGroupService = (function () {
    function VehicleTypeGroupService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    VehicleTypeGroupService.prototype.getAllVehicleTypeGroups = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleTypeGroup);
    };
    VehicleTypeGroupService.prototype.getVehicleTypeGroupDetail = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleTypeGroup + '/' + id);
    };
    VehicleTypeGroupService.prototype.getVehicleTypeGroups = function (vehicleTypeGroupNameFilter) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleTypeGroup + '/search', vehicleTypeGroupNameFilter);
    };
    VehicleTypeGroupService.prototype.addVehicleTypeGroup = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleTypeGroup, data);
    };
    VehicleTypeGroupService.prototype.updateVehicleTypeGroup = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.vehicleTypeGroup + '/' + id, data);
    };
    VehicleTypeGroupService.prototype.deleteVehicleTypeGroupPost = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleTypeGroup + '/delete/' + id, data);
    };
    VehicleTypeGroupService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleTypeGroup + '/changeRequestStaging/' + id);
    };
    VehicleTypeGroupService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleTypeGroup + '/changeRequestStaging/' + id, data);
    };
    VehicleTypeGroupService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], VehicleTypeGroupService);
    return VehicleTypeGroupService;
}());
exports.VehicleTypeGroupService = VehicleTypeGroupService;
