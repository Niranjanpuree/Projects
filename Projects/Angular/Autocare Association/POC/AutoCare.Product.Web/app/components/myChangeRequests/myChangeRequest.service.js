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
var MyChangeRequestService = (function () {
    function MyChangeRequestService() {
        this.myChangeRequests = [];
    }
    MyChangeRequestService.prototype.getMyChangeRequests = function () {
        this.myChangeRequests.push({
            id: 1592,
            date: "09-09-2016",
            table: "Make",
            rowId: "1321345589",
            status: "Pending",
            reviewed: "",
            changeType: "Add"
        });
        this.myChangeRequests.push({
            id: 1592,
            date: "09-09-2016",
            table: "Break Type",
            rowId: "1235813213",
            status: "Pending",
            reviewed: "",
            changeType: "Update"
        });
        this.myChangeRequests.push({
            id: 1592,
            date: "09-09-2016",
            table: "Break Config",
            rowId: "1321345589",
            status: "Pending",
            reviewed: "",
            changeType: "Delete"
        });
        return this.myChangeRequests;
    };
    MyChangeRequestService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], MyChangeRequestService);
    return MyChangeRequestService;
}());
exports.MyChangeRequestService = MyChangeRequestService;
