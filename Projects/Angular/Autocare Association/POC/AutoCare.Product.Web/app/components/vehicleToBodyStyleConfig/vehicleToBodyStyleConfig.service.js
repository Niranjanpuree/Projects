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
var VehicleToBodyStyleConfigService = (function () {
    function VehicleToBodyStyleConfigService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    VehicleToBodyStyleConfigService.prototype.getVehicleToBodyStyleConfigs = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfig);
    };
    VehicleToBodyStyleConfigService.prototype.getVehicleToBodyStyleConfig = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfig + "/" + id);
    };
    VehicleToBodyStyleConfigService.prototype.getByVehicleId = function (vehicleId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + "/" + vehicleId + "/vehicletobodyStyleconfigs");
    };
    VehicleToBodyStyleConfigService.prototype.getVehicleToBodyStyleConfigsByVehicleIdsAndBodyStyleConfigIds = function (vehicleIds, bodyStyleConfigIds) {
        if (vehicleIds == null && bodyStyleConfigIds == null) {
            return null;
        }
        var vehicleIdFilter = '/,';
        var bedConfigIdFilter = '/,';
        if (vehicleIds != null) {
            vehicleIds.forEach(function (item) { return vehicleIdFilter += item + ','; });
        }
        if (bodyStyleConfigIds != null) {
            bodyStyleConfigIds.forEach(function (item) { return bedConfigIdFilter += item + ','; });
        }
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + '/' + vehicleIdFilter + '/bodyStyleConfigs/' + bedConfigIdFilter + '/vehicleToBodyStyleConfigs');
    };
    VehicleToBodyStyleConfigService.prototype.addVehicleToBodyStyleConfig = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfig, data);
    };
    //TODO: use getAssociations() which calls azure search
    VehicleToBodyStyleConfigService.prototype.getVehicleToBodyStyleConfigByBodyStyleConfigId = function (bodyStyleConfigId) {
        var urlSearch = "/bodyStyleConfig/" + bodyStyleConfigId;
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfig + urlSearch);
    };
    VehicleToBodyStyleConfigService.prototype.updateVehicleToBodyStyleConfig = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfig + "/" + id, data);
    };
    VehicleToBodyStyleConfigService.prototype.deleteVehicleToBodyStyleConfig = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfig + "/delete/" + id, data);
    };
    VehicleToBodyStyleConfigService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfig + '/changeRequestStaging/' + id);
    };
    VehicleToBodyStyleConfigService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfig + '/changeRequestStaging/' + id, data);
    };
    VehicleToBodyStyleConfigService.prototype.search = function (vehicleToBodyStyleConfigSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfigSearch, vehicleToBodyStyleConfigSearchInputModel);
    };
    VehicleToBodyStyleConfigService.prototype.searchByVehicleIds = function (vehicleIds) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfigSearch + '/vehicle/', vehicleIds);
    };
    VehicleToBodyStyleConfigService.prototype.searchByBodyStyleConfigId = function (bodyStyleConfigId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfigSearch + "/bodyStyleConfig/" + bodyStyleConfigId);
    };
    VehicleToBodyStyleConfigService.prototype.getAssociations = function (vehicleToBodyStyleConfigSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfigSearch + "/associations", vehicleToBodyStyleConfigSearchInputModel);
    };
    VehicleToBodyStyleConfigService.prototype.refreshFacets = function (vehicleToBodyStyleConfigSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToBodyStyleConfigSearch + "/facets", vehicleToBodyStyleConfigSearchInputModel);
    };
    VehicleToBodyStyleConfigService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], VehicleToBodyStyleConfigService);
    return VehicleToBodyStyleConfigService;
}());
exports.VehicleToBodyStyleConfigService = VehicleToBodyStyleConfigService;
