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
var httpHelper_1 = require("../httpHelper");
var constants_warehouse_1 = require("../constants-warehouse");
var ChangeSearchService = (function () {
    function ChangeSearchService(_httpHelper) {
        this._httpHelper = _httpHelper;
        this.bulkApprovalMessage = [];
    }
    ChangeSearchService.prototype.search = function (changeRequestSearchInputModel) {
        //NOTE: <IChangeRequestSearchViewModel> return type would require Observable<T> in post() in httphelper.ts
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.changeRequestSearch, changeRequestSearchInputModel);
    };
    ChangeSearchService.prototype.searchbyChangeRequestId = function (changeRequestId) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.changeRequestSearch + "/changeRequest/" + changeRequestId);
    };
    ChangeSearchService.prototype.getRequestorComment = function (changeRequestId, status) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.changeRequestSearch + "/changeRequestId/" + changeRequestId + "/status/" + status);
    };
    ChangeSearchService.prototype.getAllLikedBy = function (changeRequestId) {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.likeStaging + "/allLikedBy/" + changeRequestId);
    };
    ChangeSearchService.prototype.getApprovedStatus = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.changeRequestSearch + "/getStatus");
    };
    ChangeSearchService.prototype.getChangeTypes = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.changeRequestSearch + "/getChangeTypes");
    };
    ChangeSearchService.prototype.getChangeEntities = function () {
        return this._httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.changeRequestSearch + "/getChangeEntities");
    };
    ChangeSearchService.prototype.refreshFacets = function (changeRequestSearchInputModel) {
        return this._httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.changeRequestSearchFacets, changeRequestSearchInputModel);
    };
    ChangeSearchService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], ChangeSearchService);
    return ChangeSearchService;
}());
exports.ChangeSearchService = ChangeSearchService;
