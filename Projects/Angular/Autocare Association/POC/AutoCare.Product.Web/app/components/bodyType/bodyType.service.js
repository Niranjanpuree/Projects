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
var BodyTypeService = (function () {
    function BodyTypeService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    BodyTypeService.prototype.getAllBodyTypes = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bodyType);
    };
    BodyTypeService.prototype.getByBodyTypeId = function (bodyTypeId) {
        var urlSearch = "/bodyType/" + bodyTypeId;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bodyType + urlSearch);
    };
    BodyTypeService.prototype.getByBodyNumDoorsId = function (bodyNumDoorsId) {
        var urlSearch = "/bodyNumDoors/" + bodyNumDoorsId;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bodyType + urlSearch);
    };
    BodyTypeService.prototype.getBodyTypeDetail = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bodyType + '/' + id);
    };
    BodyTypeService.prototype.getBodyTypes = function (bodyTypeNameFilter) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bodyType + "/search", bodyTypeNameFilter);
    };
    BodyTypeService.prototype.addBodyType = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bodyType, data);
    };
    BodyTypeService.prototype.updateBodyType = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.bodyType + '/' + id, data);
    };
    BodyTypeService.prototype.deleteBodyType = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bodyType + '/delete/' + id, data);
    };
    BodyTypeService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bodyType + '/changeRequestStaging/' + id);
    };
    BodyTypeService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bodyType + '/changeRequestStaging/' + id, data);
    };
    BodyTypeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], BodyTypeService);
    return BodyTypeService;
}());
exports.BodyTypeService = BodyTypeService;
