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
var EngineVersionService = (function () {
    function EngineVersionService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    EngineVersionService.prototype.getAllEngineVersions = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineVersion);
    };
    EngineVersionService.prototype.getEngineVersionDetail = function (id) {
        debugger;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineVersion + '/' + id);
    };
    EngineVersionService.prototype.getEngineVersions = function (engineVersionNameFilter) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.engineVersion + "/search", engineVersionNameFilter);
    };
    EngineVersionService.prototype.getByEngineVersionId = function (engineVersionId) {
        var urlSearch = "/engineVersion/" + engineVersionId;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineVersion + urlSearch);
    };
    EngineVersionService.prototype.getByBedLengthId = function (engineDesignatioId) {
        var urlSearch = "/engineVersionByBedLength/" + engineDesignatioId;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineVersion + urlSearch);
    };
    EngineVersionService.prototype.addEngineVersion = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.engineVersion, data);
    };
    EngineVersionService.prototype.updateEngineVersion = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.engineVersion + '/' + id, data);
    };
    EngineVersionService.prototype.deleteEngineVersion = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.engineVersion + '/delete/' + id, data);
    };
    EngineVersionService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineVersion + '/changeRequestStaging/' + id);
    };
    EngineVersionService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.engineVersion + '/changeRequestStaging/' + id, data);
    };
    EngineVersionService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], EngineVersionService);
    return EngineVersionService;
}());
exports.EngineVersionService = EngineVersionService;
