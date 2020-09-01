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
var ModelService = (function () {
    function ModelService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    ModelService.prototype.getAllModels = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.model);
    };
    ModelService.prototype.getModels = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.model);
    };
    ModelService.prototype.getFilteredModels = function (modelNameFilter) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.model + '/search', modelNameFilter);
    };
    ModelService.prototype.getModelsByMakeIds = function (makeIds) {
        if (makeIds != null) {
            var makeIdFilter_1 = '/,'; //note: if makeIds is empty then send a comma (,) so that /api/makes//models/, is hit
            makeIds.forEach(function (item) { return makeIdFilter_1 += item + ','; });
            return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.make + makeIdFilter_1 + '/models');
        }
    };
    ModelService.prototype.getModelsByYearIdAndMakeId = function (yearId, makeId) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.year + "/" + yearId + "/makes" + makeId + '/models');
    };
    ModelService.prototype.getModelDetail = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.model + '/' + id);
    };
    ModelService.prototype.addModel = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.model, data);
    };
    ModelService.prototype.updateModel = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.model + '/' + id, data);
    };
    ModelService.prototype.deleteModel = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.model + '/delete/' + id, data);
    };
    ModelService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.model + '/changeRequestStaging/' + id);
    };
    ModelService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.model + '/changeRequestStaging/' + id, data);
    };
    ModelService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], ModelService);
    return ModelService;
}());
exports.ModelService = ModelService;
