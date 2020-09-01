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
var shared_service_1 = require("../shared/shared.service");
var SystemMenuBar = (function () {
    function SystemMenuBar(sharedService) {
        this.sharedService = sharedService;
        //private isSystemsMenuExpanded: boolean = true;
        this.activeSubMenu = "";
        this.isChildClicked = false;
        this.onToggleMenuBarEvent = new core_1.EventEmitter();
        this.onSelectedSubMenuGroupEvent = new core_1.EventEmitter();
    }
    SystemMenuBar.prototype.ngOnInit = function () {
        if (this.sharedService.systemMenubarSelected != null) {
            this.activeSubMenuGroup = this.sharedService.systemMenubarSelected;
        }
        else {
            this.activeSubMenuGroup = "Brake"; //load brake system first
        }
    };
    SystemMenuBar.prototype.toggleSubMenuGroupActive = function (subMenuGroup, activeSubMenuGroup) {
        if (this.activeSubMenuGroup == subMenuGroup && this.isChildClicked == false) {
            this.activeSubMenuGroup = subMenuGroup;
        }
        else {
            this.activeSubMenuGroup = subMenuGroup;
            this.onSelectedSubMenuGroupEvent.emit(subMenuGroup);
        }
        this.isChildClicked = false;
        var headerht = $('header').innerHeight();
        var navht = $('nav').innerHeight();
        var winht = $(window).height();
        var winwt = 960;
        $(".drawer-left").css('min-height', winht - headerht - navht);
        $(".drawer-left").css('width', winwt);
    };
    SystemMenuBar.prototype.ontoggleMenuBar = function () {
        this.isSystemsMenuExpanded = !this.isSystemsMenuExpanded;
        // call back
        this.onToggleMenuBarEvent.emit(this.isSystemsMenuExpanded);
    };
    __decorate([
        core_1.Input("style"), 
        __metadata('design:type', Array)
    ], SystemMenuBar.prototype, "style", void 0);
    __decorate([
        core_1.Input("isSystemsMenuExpanded"), 
        __metadata('design:type', Boolean)
    ], SystemMenuBar.prototype, "isSystemsMenuExpanded", void 0);
    __decorate([
        core_1.Output("onToggleMenuBarEvent"), 
        __metadata('design:type', Object)
    ], SystemMenuBar.prototype, "onToggleMenuBarEvent", void 0);
    __decorate([
        core_1.Output("onSelectedSubMenuGroupEvent"), 
        __metadata('design:type', Object)
    ], SystemMenuBar.prototype, "onSelectedSubMenuGroupEvent", void 0);
    SystemMenuBar = __decorate([
        core_1.Component({
            selector: "system-menubar",
            templateUrl: "app/templates/system/system-menubar.component.html",
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService])
    ], SystemMenuBar);
    return SystemMenuBar;
}());
exports.SystemMenuBar = SystemMenuBar;
