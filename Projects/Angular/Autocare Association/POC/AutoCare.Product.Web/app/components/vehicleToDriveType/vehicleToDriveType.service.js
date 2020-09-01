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
var VehicleToDriveTypeService = (function () {
    function VehicleToDriveTypeService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    VehicleToDriveTypeService.prototype.getVehicleToDriveTypes = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveType);
    };
    VehicleToDriveTypeService.prototype.getVehicleToDriveType = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveType + "/" + id);
    };
    VehicleToDriveTypeService.prototype.getByVehicleId = function (vehicleId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + "/" + vehicleId + "/vehicletodrivetypes");
    };
    VehicleToDriveTypeService.prototype.getVehicleToDriveTypesByVehicleIdsAndDriveTypeIds = function (vehicleIds, driveTypeIds) {
        if (vehicleIds == null && driveTypeIds == null) {
            return null;
        }
        var vehicleIdFilter = '/,';
        var driveTypeIdFilter = '/,';
        if (vehicleIds != null) {
            vehicleIds.forEach(function (item) { return vehicleIdFilter += item + ','; });
        }
        if (driveTypeIds != null) {
            driveTypeIds.forEach(function (item) { return driveTypeIdFilter += item + ','; });
        }
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + '/' + vehicleIdFilter + '/driveTypes/' + driveTypeIdFilter + '/vehicleToDriveTypes');
    };
    VehicleToDriveTypeService.prototype.addVehicleToDriveType = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveType, data);
    };
    //TODO: use getAssociations() which calls azure search
    VehicleToDriveTypeService.prototype.getVehicleToDriveTypeByDriveTypeId = function (driveTypeId) {
        var urlSearch = "/driveType/" + driveTypeId;
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveType + urlSearch);
    };
    VehicleToDriveTypeService.prototype.updateVehicleToDriveType = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveType + "/" + id, data);
    };
    VehicleToDriveTypeService.prototype.deleteVehicleToDriveType = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveType + "/delete/" + id, data);
    };
    VehicleToDriveTypeService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveType + '/changeRequestStaging/' + id);
    };
    VehicleToDriveTypeService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveType + '/changeRequestStaging/' + id, data);
    };
    VehicleToDriveTypeService.prototype.search = function (vehicleToDriveTypeSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveTypeSearch, vehicleToDriveTypeSearchInputModel);
    };
    VehicleToDriveTypeService.prototype.searchByVehicleIds = function (vehicleIds) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveTypeSearch + '/vehicle/', vehicleIds);
    };
    VehicleToDriveTypeService.prototype.searchByDriveTypeId = function (driveTypeId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveTypeSearch + "/driveType/" + driveTypeId);
    };
    VehicleToDriveTypeService.prototype.getAssociations = function (vehicleToDriveTypeSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveTypeSearch + "/associations", vehicleToDriveTypeSearchInputModel);
    };
    VehicleToDriveTypeService.prototype.refreshFacets = function (vehicleToDriveTypeSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToDriveTypeSearchFacets, vehicleToDriveTypeSearchInputModel);
    };
    VehicleToDriveTypeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], VehicleToDriveTypeService);
    return VehicleToDriveTypeService;
}());
exports.VehicleToDriveTypeService = VehicleToDriveTypeService;
