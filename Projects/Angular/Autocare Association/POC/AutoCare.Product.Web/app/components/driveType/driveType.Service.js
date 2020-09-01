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
var DriveTypeService = (function () {
    function DriveTypeService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    DriveTypeService.prototype.getDriveTypes = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.driveType);
    };
    DriveTypeService.prototype.getDriveType = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.driveType + "/" + id);
    };
    DriveTypeService.prototype.addDriveType = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.driveType, data);
    };
    DriveTypeService.prototype.updateDriveType = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.driveType + '/' + id, data);
    };
    DriveTypeService.prototype.replaceDriveTypeConfig = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.driveType + "/replace/" + id, data);
    };
    DriveTypeService.prototype.getPendingChangeRequests = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.driveType + "/pendingChangeRequests");
    };
    DriveTypeService.prototype.deleteDriveType = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.driveType + "/delete/" + id, data);
    };
    DriveTypeService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.driveType + "/changeRequestStaging/" + id);
    };
    DriveTypeService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.driveType + '/changeRequestStaging/' + id, data);
    };
    DriveTypeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], DriveTypeService);
    return DriveTypeService;
}());
exports.DriveTypeService = DriveTypeService;
