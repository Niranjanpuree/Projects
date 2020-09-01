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
var authentication_service_1 = require("../authentication.service");
var shared_service_1 = require("./shared.service");
var router_1 = require("@angular/router");
var MainHeaderComponent = (function () {
    function MainHeaderComponent(authenticationService, sharedService, route, router) {
        this.authenticationService = authenticationService;
        this.sharedService = sharedService;
        this.route = route;
        this.router = router;
        this.selectedMainHeaderMenu = [];
    }
    MainHeaderComponent.prototype.ngOnInit = function () {
        this.token = this.sharedService.getTokenModel();
        this.selectedMainHeaderMenu.selectedMainHeaderMenuItem = "VCDB";
    };
    MainHeaderComponent.prototype.onLogout = function () {
        this.authenticationService.logout();
    };
    MainHeaderComponent.prototype.onMainHeaderMenuClick = function (mainHeaderMenuItem) {
        this.selectedMainHeaderMenu.selectedMainHeaderMenuItem = mainHeaderMenuItem;
        this.sharedService.selectedMenuHeaderItem.selectedMainHeaderMenuItem = mainHeaderMenuItem;
        this.router.navigateByUrl('dashboard');
    };
    MainHeaderComponent = __decorate([
        core_1.Component({
            selector: 'mainHeader-comp',
            templateUrl: 'app/templates/shared/mainHeader.component.html',
            providers: [authentication_service_1.AuthenticationService],
        }), 
        __metadata('design:paramtypes', [authentication_service_1.AuthenticationService, shared_service_1.SharedService, router_1.ActivatedRoute, router_1.Router])
    ], MainHeaderComponent);
    return MainHeaderComponent;
}());
exports.MainHeaderComponent = MainHeaderComponent;
