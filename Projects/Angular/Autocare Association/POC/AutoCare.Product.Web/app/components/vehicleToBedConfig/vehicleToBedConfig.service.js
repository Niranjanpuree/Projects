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
var VehicleToBedConfigService = (function () {
    function VehicleToBedConfigService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    VehicleToBedConfigService.prototype.getVehicleToBedConfigs = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfig);
    };
    VehicleToBedConfigService.prototype.getVehicleToBedConfig = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfig + "/" + id);
    };
    VehicleToBedConfigService.prototype.getByVehicleId = function (vehicleId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + "/" + vehicleId + "/vehicletobedconfigs");
    };
    VehicleToBedConfigService.prototype.getVehicleToBedConfigsByVehicleIdsAndBedConfigIds = function (vehicleIds, bedConfigIds) {
        if (vehicleIds == null && bedConfigIds == null) {
            return null;
        }
        var vehicleIdFilter = '/,';
        var bedConfigIdFilter = '/,';
        if (vehicleIds != null) {
            vehicleIds.forEach(function (item) { return vehicleIdFilter += item + ','; });
        }
        if (bedConfigIds != null) {
            bedConfigIds.forEach(function (item) { return bedConfigIdFilter += item + ','; });
        }
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + '/' + vehicleIdFilter + '/bedConfigs/' + bedConfigIdFilter + '/vehicleToBedConfigs');
    };
    VehicleToBedConfigService.prototype.addVehicleToBedConfig = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfig, data);
    };
    //TODO: use getAssociations() which calls azure search
    VehicleToBedConfigService.prototype.getVehicleToBedConfigByBedConfigId = function (bedConfigId) {
        var urlSearch = "/bedConfig/" + bedConfigId;
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfig + urlSearch);
    };
    VehicleToBedConfigService.prototype.updateVehicleToBedConfig = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfig + "/" + id, data);
    };
    VehicleToBedConfigService.prototype.deleteVehicleToBedConfig = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfig + "/delete/" + id, data);
    };
    VehicleToBedConfigService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfig + '/changeRequestStaging/' + id);
    };
    VehicleToBedConfigService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfig + '/changeRequestStaging/' + id, data);
    };
    VehicleToBedConfigService.prototype.search = function (vehicleToBedConfigSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfigSearch, vehicleToBedConfigSearchInputModel);
    };
    VehicleToBedConfigService.prototype.searchByBaseVehicleId = function (baseVehicleId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfigSearch + "/baseVehicle/" + baseVehicleId);
    };
    VehicleToBedConfigService.prototype.searchByVehicleId = function (vehicleId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfigSearch + "/vehicle/" + vehicleId);
    };
    VehicleToBedConfigService.prototype.searchByVehicleIds = function (vehicleIds) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfigSearch + '/vehicle/', vehicleIds);
    };
    VehicleToBedConfigService.prototype.searchByBedConfigId = function (bedConfigId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfigSearch + "/bedConfig/" + bedConfigId);
    };
    VehicleToBedConfigService.prototype.getAssociations = function (vehicleToBedConfigSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfigSearch + "/associations", vehicleToBedConfigSearchInputModel);
    };
    VehicleToBedConfigService.prototype.refreshFacets = function (vehicleToBedConfigSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBedConfigSearchFacets, vehicleToBedConfigSearchInputModel);
    };
    VehicleToBedConfigService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], VehicleToBedConfigService);
    return VehicleToBedConfigService;
}());
exports.VehicleToBedConfigService = VehicleToBedConfigService;
