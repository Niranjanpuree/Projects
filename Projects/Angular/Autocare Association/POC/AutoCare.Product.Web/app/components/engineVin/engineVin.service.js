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
var EngineVinService = (function () {
    function EngineVinService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    EngineVinService.prototype.getAllEngineVins = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineVin);
    };
    EngineVinService.prototype.getEngineVinDetail = function (id) {
        debugger;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineVin + '/' + id);
    };
    EngineVinService.prototype.getEngineVins = function (engineVinNameFilter) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.engineVin + "/search", engineVinNameFilter);
    };
    EngineVinService.prototype.getByEngineVinId = function (engineVinId) {
        var urlSearch = "/engineVin/" + engineVinId;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineVin + urlSearch);
    };
    EngineVinService.prototype.getByBedLengthId = function (engineDesignatioId) {
        var urlSearch = "/engineVinByBedLength/" + engineDesignatioId;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineVin + urlSearch);
    };
    EngineVinService.prototype.addEngineVin = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.engineVin, data);
    };
    EngineVinService.prototype.updateEngineVin = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.engineVin + '/' + id, data);
    };
    EngineVinService.prototype.deleteEngineVin = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.engineVin + '/delete/' + id, data);
    };
    EngineVinService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineVin + '/changeRequestStaging/' + id);
    };
    EngineVinService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.engineVin + '/changeRequestStaging/' + id, data);
    };
    EngineVinService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], EngineVinService);
    return EngineVinService;
}());
exports.EngineVinService = EngineVinService;
