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
var VehicleToBrakeConfigService = (function () {
    function VehicleToBrakeConfigService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    VehicleToBrakeConfigService.prototype.getVehicleToBrakeConfigs = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfig);
    };
    VehicleToBrakeConfigService.prototype.getVehicleToBrakeConfig = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfig + "/" + id);
    };
    VehicleToBrakeConfigService.prototype.getByVehicleId = function (vehicleId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + "/" + vehicleId + "/vehicletobrakeconfigs");
    };
    VehicleToBrakeConfigService.prototype.getVehicleToBrakeConfigsByVehicleIdsAndBrakeConfigIds = function (vehicleIds, brakeConfigIds) {
        if (vehicleIds == null && brakeConfigIds == null) {
            return null;
        }
        var vehicleIdFilter = '/,';
        var brakeConfigIdFilter = '/,';
        if (vehicleIds != null) {
            vehicleIds.forEach(function (item) { return vehicleIdFilter += item + ','; });
        }
        if (brakeConfigIds != null) {
            brakeConfigIds.forEach(function (item) { return brakeConfigIdFilter += item + ','; });
        }
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + '/' + vehicleIdFilter + '/brakeConfigs/' + brakeConfigIdFilter + '/vehicleToBrakeConfigs');
    };
    VehicleToBrakeConfigService.prototype.addVehicleToBrakeConfig = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfig, data);
    };
    //TODO: use getAssociations() which calls azure search
    VehicleToBrakeConfigService.prototype.getVehicleToBrakeConfigByBrakeConfigId = function (brakeConfigId) {
        var urlSearch = "/brakeConfig/" + brakeConfigId;
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfig + urlSearch);
    };
    VehicleToBrakeConfigService.prototype.updateVehicleToBrakeConfig = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfig + "/" + id, data);
    };
    VehicleToBrakeConfigService.prototype.deleteVehicleToBrakeConfig = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfig + "/delete/" + id, data);
    };
    VehicleToBrakeConfigService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfig + '/changeRequestStaging/' + id);
    };
    VehicleToBrakeConfigService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfig + '/changeRequestStaging/' + id, data);
    };
    VehicleToBrakeConfigService.prototype.search = function (vehicleToBrakeConfigSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfigSearch, vehicleToBrakeConfigSearchInputModel);
    };
    VehicleToBrakeConfigService.prototype.searchByVehicleIds = function (vehicleIds) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfigSearch + '/vehicle/', vehicleIds);
    };
    VehicleToBrakeConfigService.prototype.searchByBrakeConfigId = function (brakeConfigId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfigSearch + "/brakeConfig/" + brakeConfigId);
    };
    VehicleToBrakeConfigService.prototype.getAssociations = function (vehicleToBrakeConfigSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfigSearch + "/associations", vehicleToBrakeConfigSearchInputModel);
    };
    VehicleToBrakeConfigService.prototype.refreshFacets = function (vehicleToBrakeConfigSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBrakeConfigSearchFacets, vehicleToBrakeConfigSearchInputModel);
    };
    VehicleToBrakeConfigService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], VehicleToBrakeConfigService);
    return VehicleToBrakeConfigService;
}());
exports.VehicleToBrakeConfigService = VehicleToBrakeConfigService;
