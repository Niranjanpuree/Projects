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
var RecentChangeService = (function () {
    function RecentChangeService() {
        this.recentChanges = [];
    }
    RecentChangeService.prototype.getRecentChanges = function () {
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Add"
        });
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Update"
        });
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Delete"
        });
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Update"
        });
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Delete"
        });
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Add"
        });
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Delete"
        });
        return this.recentChanges;
    };
    RecentChangeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], RecentChangeService);
    return RecentChangeService;
}());
exports.RecentChangeService = RecentChangeService;
