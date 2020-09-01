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
var recentChange_service_1 = require('./recentChange.service');
var RecentChangesComponent = (function () {
    function RecentChangesComponent(_recentChangeService) {
        this._recentChangeService = _recentChangeService;
        this.recentChanges = [];
        //isumm: ISummary = {
        //    countAdd: 0,
        //    countUpdate: 0,
        //    countDelete: 0
        //}
        this.changeTypeAddCount = 0;
        this.changeTypeUpdateCount = 0;
        this.changeTypeDeleteCount = 0;
    }
    RecentChangesComponent.prototype.ngOnInit = function () {
        this.getRecentChanges();
    };
    RecentChangesComponent.prototype.getRecentChanges = function () {
        this.recentChanges = this._recentChangeService.getRecentChanges();
        for (var i = 0; i < this.recentChanges.length; i++) {
            if (this.recentChanges[i].changeType == "Add") {
                this.changeTypeAddCount++;
            }
            else if (this.recentChanges[i].changeType == "Update") {
                this.changeTypeUpdateCount++;
            }
            else if (this.recentChanges[i].changeType == "Delete") {
                this.changeTypeDeleteCount++;
            }
        }
        //this.recentChanges.forEach(function (recentChange, index) {
        //    debugger;
        //    if (recentChange.changeType == "Add") {
        //        //this.changeTypeAddCount++;
        //    }
        //    else if (recentChange.changeType == "Update") {
        //        this.changeTypeEditCount++;
        //    }
        //    else if (recentChange.changeType == "Delete") {
        //        this.changeTypeDeleteCount++;
        //    }
        //})
    };
    RecentChangesComponent = __decorate([
        core_1.Component({
            selector: 'recent-changes',
            templateUrl: 'app/templates/recentChanges/recentChanges.html',
            providers: [recentChange_service_1.RecentChangeService]
        }), 
        __metadata('design:paramtypes', [recentChange_service_1.RecentChangeService])
    ], RecentChangesComponent);
    return RecentChangesComponent;
}());
exports.RecentChangesComponent = RecentChangesComponent;
//export interface ISummary {
//    countAdd: number;
//    countUpdate: number;
//    countDelete: number;
//} 
