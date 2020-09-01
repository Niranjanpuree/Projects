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
var BedTypeService = (function () {
    function BedTypeService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    BedTypeService.prototype.getAllBedTypes = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bedType);
    };
    BedTypeService.prototype.getBedTypeDetail = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bedType + '/' + id);
    };
    BedTypeService.prototype.getBedTypes = function (bedTypeNameFilter) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bedType + "/search", bedTypeNameFilter);
    };
    BedTypeService.prototype.getByBedTypeId = function (bedTypeId) {
        var urlSearch = "/bedType/" + bedTypeId;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bedType + urlSearch);
    };
    BedTypeService.prototype.getByBedLengthId = function (bedLengthId) {
        var urlSearch = "/bedTypeByBedLength/" + bedLengthId;
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bedType + urlSearch);
    };
    BedTypeService.prototype.addBedType = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bedType, data);
    };
    BedTypeService.prototype.updateBedType = function (id, data) {
        return this._httpHelper.put(constants_warehouse_1.ConstantsWarehouse.api.bedType + '/' + id, data);
    };
    BedTypeService.prototype.deleteBedType = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bedType + '/delete/' + id, data);
    };
    BedTypeService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.bedType + '/changeRequestStaging/' + id);
    };
    BedTypeService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.bedType + '/changeRequestStaging/' + id, data);
    };
    BedTypeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], BedTypeService);
    return BedTypeService;
}());
exports.BedTypeService = BedTypeService;
