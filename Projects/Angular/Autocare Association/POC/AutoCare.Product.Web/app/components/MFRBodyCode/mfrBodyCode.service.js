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
var MfrBodyCodeService = (function () {
    function MfrBodyCodeService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    MfrBodyCodeService.prototype.getMfrBodyCodes = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.mfrBodyCode);
    };
    MfrBodyCodeService.prototype.getMfrBodyCode = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.mfrBodyCode + "/" + id);
    };
    MfrBodyCodeService.prototype.addMfrBodyCode = function (data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.mfrBodyCode, data);
    };
    MfrBodyCodeService.prototype.updateMfrBodyCode = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.mfrBodyCode + '/' + id, data);
    };
    MfrBodyCodeService.prototype.replaceMfrBodyCode = function (id, data) {
        return this.httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.mfrBodyCode + "/replace/" + id, data);
    };
    MfrBodyCodeService.prototype.getPendingChangeRequests = function () {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.mfrBodyCode + "/pendingChangeRequests");
    };
    MfrBodyCodeService.prototype.deleteMfrBodyCode = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.mfrBodyCode + "/delete/" + id, data);
    };
    MfrBodyCodeService.prototype.getChangeRequestStaging = function (id) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.mfrBodyCode + "/changeRequestStaging/" + id);
    };
    MfrBodyCodeService.prototype.submitChangeRequestReview = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.mfrBodyCode + '/changeRequestStaging/' + id, data);
    };
    MfrBodyCodeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], MfrBodyCodeService);
    return MfrBodyCodeService;
}());
exports.MfrBodyCodeService = MfrBodyCodeService;
