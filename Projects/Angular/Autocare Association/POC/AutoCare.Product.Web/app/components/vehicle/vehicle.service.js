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
var VehicleService = (function () {
    function VehicleService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    VehicleService.prototype.getVehicles = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle);
    };
    VehicleService.prototype.getVehicle = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + "/" + id);
    };
    VehicleService.prototype.getVehicleByBaseVehicleIdSubModelIdAndRegionId = function (baseVehicleId, subModelId, regionId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle + "/" + baseVehicleId + "/subModels/" + subModelId + "/regions/" + regionId + "/vehicles");
    };
    VehicleService.prototype.getVehiclesByBaseVehicleId = function (baseVehicleId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle + "/" + baseVehicleId + "/vehicles");
    };
    VehicleService.prototype.updateVehicle = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.vehicle + "/" + id, data);
    };
    VehicleService.prototype.deleteVehicle = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicle + "/delete/" + id, data);
    };
    VehicleService.prototype.getPendingChangeRequest = function (baseVehicleId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehiclePendingChangeRequest + "/" + baseVehicleId);
    };
    VehicleService.prototype.createVehicleChangeRequests = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicle, data);
    };
    VehicleService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + "/changeRequestStaging/" + id);
    };
    VehicleService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicle + '/changeRequestStaging/' + id, data);
    };
    VehicleService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], VehicleService);
    return VehicleService;
}());
exports.VehicleService = VehicleService;
