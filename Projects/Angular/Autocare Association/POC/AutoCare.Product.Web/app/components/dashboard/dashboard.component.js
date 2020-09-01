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
var downloadRequest_service_1 = require('../downloadRequests/downloadRequest.service');
var DashboardComponent = (function () {
    function DashboardComponent(_downloadRequestService) {
        this._downloadRequestService = _downloadRequestService;
        this.dashboardPreferences = [];
    }
    DashboardComponent.prototype.ngOnInit = function () {
        this.getDashboardPreferences();
        //debugger;
        //var p = document.getElementById('myDashboard');
        //for (var i = 0; i < this.dashboardPreferences.length; i++) {
        //   // var n = document.registerElement(this.dashboardPreferences[i].htmlSelector);
        //   // document.body.appendChild(n);
        //}
    };
    DashboardComponent.prototype.getDashboardPreferences = function () {
        //get dashboard preferences
        this.dashboardPreferences.push({
            name: 'Search',
            htmlSelector: 'search'
        });
        this.dashboardPreferences.push({
            name: 'Download Request',
            htmlSelector: 'download-requests'
        });
        this.dashboardPreferences.push({
            name: 'My Change Request',
            htmlSelector: 'my-change-requests'
        });
        this.dashboardPreferences.push({
            name: 'Recent Changes',
            htmlSelector: 'recent-changes'
        });
    };
    DashboardComponent = __decorate([
        core_1.Component({
            selector: 'dashboard',
            templateUrl: 'app/templates/dashboard/dashboard.html',
            providers: [downloadRequest_service_1.DownloadRequestService]
        }), 
        __metadata('design:paramtypes', [downloadRequest_service_1.DownloadRequestService])
    ], DashboardComponent);
    return DashboardComponent;
}());
exports.DashboardComponent = DashboardComponent;
