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
var LikeStagingService = (function () {
    function LikeStagingService(httpHelper) {
        this.httpHelper = httpHelper;
    }
    LikeStagingService.prototype.submitLike = function (id, data) {
        return this.httpHelper.post(constants_warehouse_1.ConstantsWarehouse.api.likeStaging + '/' + id, data);
    };
    LikeStagingService.prototype.getLikeDetails = function (changeRequestId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.likeStaging + '/' + changeRequestId);
    };
    LikeStagingService.prototype.getAllLikedBy = function (changeRequestId) {
        return this.httpHelper.get(constants_warehouse_1.ConstantsWarehouse.api.likeStaging + "/allLikedBy/" + changeRequestId);
    };
    LikeStagingService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper])
    ], LikeStagingService);
    return LikeStagingService;
}());
exports.LikeStagingService = LikeStagingService;
