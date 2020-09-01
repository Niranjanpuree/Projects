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
var VehicleToWheelBaseService = (function () {
    function VehicleToWheelBaseService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    VehicleToWheelBaseService.prototype.getVehicleToWheelBases = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBase);
    };
    VehicleToWheelBaseService.prototype.getVehicleToWheelBase = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBase + "/" + id);
    };
    VehicleToWheelBaseService.prototype.getByVehicleId = function (vehicleId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + "/" + vehicleId + "/vehicleToWheelBases");
    };
    VehicleToWheelBaseService.prototype.getVehicleToWheelBasesByVehicleIdsAndWheelBaseIds = function (vehicleIds, wheelBaseIds) {
        if (vehicleIds == null && wheelBaseIds == null) {
            return null;
        }
        var vehicleIdFilter = '/,';
        var wheelBaseIdsFilter = '/,';
        if (vehicleIds != null) {
            vehicleIds.forEach(function (item) { return vehicleIdFilter += item + ','; });
        }
        if (wheelBaseIds != null) {
            wheelBaseIds.forEach(function (item) { return wheelBaseIdsFilter += item + ','; });
        }
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + '/' + vehicleIdFilter + '/wheelBases/' + wheelBaseIdsFilter + '/vehicleToWheelBases');
    };
    VehicleToWheelBaseService.prototype.addVehicleToWheelBase = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBase, data);
    };
    //TODO: use getAssociations() which calls azure search
    VehicleToWheelBaseService.prototype.getVehicleToWheelBaseByWheelBaseId = function (wheelBaseId) {
        var urlSearch = "/wheelBase/" + wheelBaseId;
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBase + urlSearch);
    };
    VehicleToWheelBaseService.prototype.updateVehicleToWheelBase = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBase + "/" + id, data);
    };
    VehicleToWheelBaseService.prototype.deleteVehicleToWheelBase = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBase + "/delete/" + id, data);
    };
    VehicleToWheelBaseService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBase + '/changeRequestStaging/' + id);
    };
    VehicleToWheelBaseService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBase + '/changeRequestStaging/' + id, data);
    };
    VehicleToWheelBaseService.prototype.search = function (vehicleToWheelBaseSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBaseSearch, vehicleToWheelBaseSearchInputModel);
    };
    VehicleToWheelBaseService.prototype.searchByVehicleIds = function (vehicleIds) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBaseSearch + '/vehicle/', vehicleIds);
    };
    VehicleToWheelBaseService.prototype.searchByWheelBaseId = function (wheelBaseId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBaseSearch + "/wheelBase/" + wheelBaseId);
    };
    VehicleToWheelBaseService.prototype.getAssociations = function (vehicleToWheelBaseSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBaseSearch + "/associations", vehicleToWheelBaseSearchInputModel);
    };
    VehicleToWheelBaseService.prototype.refreshFacets = function (vehicleToWheelBaseSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToWheelBaseSearch + "/facets", vehicleToWheelBaseSearchInputModel);
    };
    VehicleToWheelBaseService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], VehicleToWheelBaseService);
    return VehicleToWheelBaseService;
}());
exports.VehicleToWheelBaseService = VehicleToWheelBaseService;
