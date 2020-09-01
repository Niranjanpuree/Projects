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
var MakeService = (function () {
    function MakeService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    MakeService.prototype.getAllMakes = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.make);
    };
    MakeService.prototype.get = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.make /*+ '/count/20'*/);
    };
    MakeService.prototype.getByFilter = function (makeNameFilter) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.make + '/search', makeNameFilter);
    };
    MakeService.prototype.getMakesByYearId = function (yearId) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.year + "/" + yearId + '/makes');
    };
    MakeService.prototype.add = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.make, data);
    };
    MakeService.prototype.update = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.make + '/' + id, data);
    };
    MakeService.prototype.delete = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.make + '/delete/' + id, data);
    };
    MakeService.prototype.getById = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.make + '/' + id);
    };
    MakeService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.make + '/changeRequestStaging/' + id);
    };
    MakeService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.make + '/changeRequestStaging/' + id, data);
    };
    MakeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], MakeService);
    return MakeService;
}());
exports.MakeService = MakeService;
