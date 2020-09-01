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
var downloadRequest_service_1 = require('./downloadRequest.service');
var DownloadRequestComponent = (function () {
    function DownloadRequestComponent(_downloadRequestService) {
        this._downloadRequestService = _downloadRequestService;
    }
    DownloadRequestComponent.prototype.ngOnInit = function () {
        this.getDownloadRequests();
    };
    DownloadRequestComponent.prototype.getDownloadRequests = function () {
        this.downloadRequests = this._downloadRequestService.getDownloadRequests();
        //this._downloadRequestService.getDownloadRequests()
        //    .subscribe(
        //    downloadRequests => this.downloadRequests = downloadRequests,
        //    error => this.errorMessage = <any>error
        //);
    };
    DownloadRequestComponent = __decorate([
        core_1.Component({
            selector: 'download-requests',
            templateUrl: 'app/templates/downloadRequests/downloadRequests.html',
            providers: [downloadRequest_service_1.DownloadRequestService]
        }), 
        __metadata('design:paramtypes', [downloadRequest_service_1.DownloadRequestService])
    ], DownloadRequestComponent);
    return DownloadRequestComponent;
}());
exports.DownloadRequestComponent = DownloadRequestComponent;
