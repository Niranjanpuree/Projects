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
var SubModelService = (function () {
    function SubModelService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    SubModelService.prototype.getAllSubModels = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.subModel);
    };
    SubModelService.prototype.getSubModels = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.subModel);
    };
    SubModelService.prototype.getSubModelDetail = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.subModel + "/" + id);
    };
    SubModelService.prototype.getFilteredSubModels = function (subModelNameFilter) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.subModel + "/search", subModelNameFilter);
    };
    SubModelService.prototype.getSubModelsByBaseVehicleId = function (baseVehicleId) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.baseVehicle + "/" + baseVehicleId + "/subModels");
    };
    SubModelService.prototype.getSubModelsByMakeIdsAndModelIds = function (makeIds, modelIds) {
        var makeIdFilter = ",";
        makeIds.forEach(function (item) { return makeIdFilter += item + ","; });
        var modelIdFilter = ",";
        modelIds.forEach(function (item) { return modelIdFilter += item + ","; });
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.make + "/" + makeIdFilter + "/models/" + modelIdFilter + "/subModels");
    };
    SubModelService.prototype.addSubModel = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.subModel, data);
    };
    SubModelService.prototype.updateSubModel = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.subModel + "/" + id, data);
    };
    SubModelService.prototype.deleteSubModel = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.subModel + "/delete/" + id, data);
    };
    SubModelService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.subModel + '/changeRequestStaging/' + id);
    };
    SubModelService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.subModel + '/changeRequestStaging/' + id, data);
    };
    SubModelService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], SubModelService);
    return SubModelService;
}());
exports.SubModelService = SubModelService;
