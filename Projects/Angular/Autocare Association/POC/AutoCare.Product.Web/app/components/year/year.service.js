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
var YearService = (function () {
    function YearService(_httpHelper) {
        this._httpHelper = _httpHelper;
    }
    YearService.prototype.getYears = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.year);
    };
    YearService.prototype.addYear = function (data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.year, data);
    };
    YearService.prototype.getDependencies = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.year + '/dependencies/' + id);
    };
    YearService.prototype.delete = function (id) {
        return this._httpHelper.delete(constants_warehouse_1.ConstantsWarehouse.api.year + '/' + id);
    };
    YearService.prototype.deleteYear = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.year + '/delete/' + id, data);
    };
    YearService.prototype.getChangeRequestStaging = function (id) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.year + '/changeRequestStaging/' + id);
    };
    YearService.prototype.submitChangeRequestReview = function (id, data) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.year + '/changeRequestStaging/' + id, data);
    };
    YearService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], YearService);
    return YearService;
}());
exports.YearService = YearService;
