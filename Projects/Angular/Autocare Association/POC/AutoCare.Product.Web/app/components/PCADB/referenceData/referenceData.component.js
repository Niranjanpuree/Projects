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
var PCADBReferenceDataComponent = (function () {
    function PCADBReferenceDataComponent(route, router) {
        var _this = this;
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
            }
        });
    }
    PCADBReferenceDataComponent.prototype.toggleSubMenuGroupActive = function (subMenuGroup, activeSubMenuGroup) {
        if (this.activeSubMenuGroup == subMenuGroup && this.isChildClicked == false) {
            this.activeSubMenuGroup = '';
        }
        else {
            this.activeSubMenuGroup = subMenuGroup;
        }
        this.isChildClicked = false;
    };
    PCADBReferenceDataComponent.prototype.toggleLeftMenu = function () {
        if (this.isMenuExpanded) {
            this.isMenuExpanded = false;
        }
        else {
            this.isMenuExpanded = true;
        }
    };
    PCADBReferenceDataComponent = __decorate([
        core_1.Component({
            selector: 'pcadb-reference-data',
            templateUrl: 'app/templates/PCADB/referenceData/referenceData.component.html',
        }), 
        __metadata('design:paramtypes', [router_1.ActivatedRoute, router_1.Router])
    ], PCADBReferenceDataComponent);
    return PCADBReferenceDataComponent;
}());
exports.PCADBReferenceDataComponent = PCADBReferenceDataComponent;
