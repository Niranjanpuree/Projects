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
var router_1 = require('@angular/router');
var shared_service_1 = require('../shared/shared.service');
var ReferenceDataComponent = (function () {
    function ReferenceDataComponent(route, router, sharedService) {
        var _this = this;
        this.sharedService = sharedService;
        this.activeSubMenu = '';
        this.activeSubMenuGroup = '';
        this.isChildClicked = false;
        this.isMenuExpanded = true;
        this.route = route;
        this.router = router;
        this.router.events.subscribe(function (event) {
            if (event.constructor.name === 'NavigationEnd') {
                var navEndEvent = event;
                _this.finalUrl = navEndEvent.urlAfterRedirects;
                var referenceDataRoute = _this.route.firstChild;
                if (referenceDataRoute) {
                    var activeRouteConfig = referenceDataRoute.firstChild;
                    referenceDataRoute.firstChild.data.subscribe(function (data) {
                        _this.activeSubMenu = data['activeSubMenuTab'];
                        if (sharedService.referenceDataActiveSubMenuGroupSelected == '') {
                            _this.activeSubMenuGroup = data['activeSubMenuGroup'];
                            _this.sharedService.referenceDataActiveSubMenuGroupSelected = _this.activeSubMenuGroup;
                        }
                        else {
                            _this.activeSubMenuGroup = _this.sharedService.referenceDataActiveSubMenuGroupSelected;
                        }
                    });
                    _this.isChildClicked = true;
                }
            }
        });
    }
    ReferenceDataComponent.prototype.toggleSubMenuGroupActive = function (subMenuGroup, event) {
        this.sharedService.referenceDataActiveSubMenuGroupSelected = subMenuGroup;
        if (this.activeSubMenuGroup == subMenuGroup && this.isChildClicked == false) {
            if (event.target.innerText == subMenuGroup) {
                this.activeSubMenuGroup = '';
            }
        }
        else {
            this.activeSubMenuGroup = subMenuGroup;
        }
        this.isChildClicked = false;
    };
    ReferenceDataComponent.prototype.toggleLeftMenu = function () {
        if (this.isMenuExpanded) {
            this.isMenuExpanded = false;
        }
        else {
            this.isMenuExpanded = true;
        }
    };
    ReferenceDataComponent.prototype.redirectToSystem = function (menuName) {
        if (menuName != null) {
            this.sharedService.systemMenubarSelected = menuName; //Assumes that same name in system and reference data
            this.router.navigate(["/system/search"]);
        }
    };
    ReferenceDataComponent = __decorate([
        core_1.Component({
            selector: 'reference-data',
            templateUrl: 'app/templates/referenceData/referenceData.component.html',
        }), 
        __metadata('design:paramtypes', [router_1.ActivatedRoute, router_1.Router, shared_service_1.SharedService])
    ], ReferenceDataComponent);
    return ReferenceDataComponent;
}());
exports.ReferenceDataComponent = ReferenceDataComponent;
