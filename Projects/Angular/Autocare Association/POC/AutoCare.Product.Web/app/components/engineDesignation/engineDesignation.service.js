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
var EngineDesignationService = (function () {
    function EngineDesignationService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    EngineDesignationService.prototype.getAllEngineDesignations = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineDesignation);
    };
    EngineDesignationService.prototype.getEngineDesignationDetail = function (id) {
        debugger;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineDesignation + '/' + id);
    };
    EngineDesignationService.prototype.getEngineDesignations = function (engineDesignationNameFilter) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.engineDesignation + "/search", engineDesignationNameFilter);
    };
    EngineDesignationService.prototype.getByEngineDesignationId = function (engineDesignationId) {
        var urlSearch = "/engineDesignation/" + engineDesignationId;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineDesignation + urlSearch);
    };
    EngineDesignationService.prototype.getByBedLengthId = function (engineDesignatioId) {
        var urlSearch = "/engineDesignationByBedLength/" + engineDesignatioId;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineDesignation + urlSearch);
    };
    EngineDesignationService.prototype.addEngineDesignation = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.engineDesignation, data);
    };
    EngineDesignationService.prototype.updateEngineDesignation = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.engineDesignation + '/' + id, data);
    };
    EngineDesignationService.prototype.deleteEngineDesignation = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.engineDesignation + '/delete/' + id, data);
    };
    EngineDesignationService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.engineDesignation + '/changeRequestStaging/' + id);
    };
    EngineDesignationService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.engineDesignation + '/changeRequestStaging/' + id, data);
    };
    EngineDesignationService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], EngineDesignationService);
    return EngineDesignationService;
}());
exports.EngineDesignationService = EngineDesignationService;
