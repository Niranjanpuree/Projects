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
var VehicleToMfrBodyCodeService = (function () {
    function VehicleToMfrBodyCodeService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    VehicleToMfrBodyCodeService.prototype.getVehicleToMfrBodyCodes = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCode);
    };
    VehicleToMfrBodyCodeService.prototype.getVehicleToMfrBodyCode = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCode + "/" + id);
    };
    VehicleToMfrBodyCodeService.prototype.getByVehicleId = function (vehicleId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + "/" + vehicleId + "/vehicletobrakeconfigs");
    };
    VehicleToMfrBodyCodeService.prototype.getVehicleToMfrBodyCodesByVehicleIdsAndMfrBodyCodeIds = function (vehicleIds, mfrBodyCodeIds) {
        if (vehicleIds == null && mfrBodyCodeIds == null) {
            return null;
        }
        var vehicleIdFilter = '/,';
        var mfrBodyCodeIdFilter = '/,';
        if (vehicleIds != null) {
            vehicleIds.forEach(function (item) { return vehicleIdFilter += item + ','; });
        }
        if (mfrBodyCodeIds != null) {
            mfrBodyCodeIds.forEach(function (item) { return mfrBodyCodeIdFilter += item + ','; });
        }
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicle + '/' + vehicleIdFilter + '/mfrBodyCodes/' + mfrBodyCodeIdFilter + '/vehicleToMfrBodyCodes');
    };
    VehicleToMfrBodyCodeService.prototype.addVehicleToMfrBodyCode = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCode, data);
    };
    //TODO: use getAssociations() which calls azure search
    VehicleToMfrBodyCodeService.prototype.getVehicleToMfrBodyCodeByMfrBodyCodeId = function (mfrBodyCodeId) {
        var urlSearch = "/mfrBodyCode/" + mfrBodyCodeId;
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCode + urlSearch);
    };
    VehicleToMfrBodyCodeService.prototype.updateVehicleToMfrBodyCode = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCode + "/" + id, data);
    };
    VehicleToMfrBodyCodeService.prototype.deleteVehicleToMfrBodyCode = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCode + "/delete/" + id, data);
    };
    VehicleToMfrBodyCodeService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCode + '/changeRequestStaging/' + id);
    };
    VehicleToMfrBodyCodeService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCode + '/changeRequestStaging/' + id, data);
    };
    VehicleToMfrBodyCodeService.prototype.search = function (vehicleToMfrBodyCodeSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCodeSearch, vehicleToMfrBodyCodeSearchInputModel);
    };
    VehicleToMfrBodyCodeService.prototype.searchByVehicleIds = function (vehicleIds) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCodeSearch + '/vehicle/', vehicleIds);
    };
    VehicleToMfrBodyCodeService.prototype.searchByMfrBodyCodeId = function (mfrBodyCodeId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCodeSearch + "/mfrBodyCode/" + mfrBodyCodeId);
    };
    VehicleToMfrBodyCodeService.prototype.getAssociations = function (vehicleToMfrBodyCodeSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCodeSearch + "/associations", vehicleToMfrBodyCodeSearchInputModel);
    };
    VehicleToMfrBodyCodeService.prototype.refreshFacets = function (vehicleToMfrBodyCodeSearchInputModel) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.vehicleToMfrBodyCodeSearchFacets, vehicleToMfrBodyCodeSearchInputModel);
    };
    VehicleToMfrBodyCodeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], VehicleToMfrBodyCodeService);
    return VehicleToMfrBodyCodeService;
}());
exports.VehicleToMfrBodyCodeService = VehicleToMfrBodyCodeService;
