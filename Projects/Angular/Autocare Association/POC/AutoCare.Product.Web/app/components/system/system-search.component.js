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
var router_1 = require("@angular/router");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var system_menubar_component_1 = require("./system-menubar.component");
var vehicleToBrakeConfig_search_model_1 = require("../vehicleToBrakeConfig/vehicleToBrakeConfig-search.model");
var vehicleToBrakeConfig_search_component_1 = require("../vehicleToBrakeConfig/vehicleToBrakeConfig-search.component");
var vehicleToBedConfig_search_component_1 = require("../vehicleToBedConfig/vehicleToBedConfig-search.component");
var vehicleToBodyStyleConfig_search_component_1 = require("../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-search.component");
var vehicleToWheelBase_search_component_1 = require("../vehicleToWheelBase/vehicleToWheelBase-search.component");
var vehicleToMfrBodyCode_search_component_1 = require("../vehicleToMfrBodyCode/vehicleToMfrBodyCode-search.component");
var vehicleToDriveType_search_component_1 = require("../vehicleToDriveType/vehicleToDriveType-search.component");
var SystemSearchComponent = (function () {
    function SystemSearchComponent(sharedService, router, toastr, navigationService) {
        this.sharedService = sharedService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.thresholdRecordCount = 100;
    }
    SystemSearchComponent.prototype.ngOnInit = function () {
        //this.activeSubMenuGroup = this.systemMenubar.activeSubMenuGroup;
        if (this.sharedService.systemMenubarSelected != null) {
            this.activeSubMenuGroup = this.sharedService.systemMenubarSelected;
        }
        else {
            this.activeSubMenuGroup = "Brake"; //load brake system first
        }
        this.isSystemsMenuExpanded = true;
        // initialize vehicleToBrakeConfigSearchModel. this property will be passed to searchPanel component.
        this.vehicleToBrakeConfigSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: [],
                frontBrakeTypes: [],
                rearBrakeTypes: [],
                brakeAbs: [],
                brakeSystems: [],
            },
            result: { brakeConfigs: [], vehicleToBrakeConfigs: [] },
            searchType: vehicleToBrakeConfig_search_model_1.SearchType.None
        };
        // vehicle to bed config
        this.vehicleToBedConfigSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: [],
                bedLengths: [],
                bedTypes: [],
            },
            result: { bedConfigs: [], vehicleToBedConfigs: [] }
        };
        //this.vehicleToBedConfigsForSelectedBed = [];
        // vehicle to body style config
        this.vehicleToBodyStyleConfigSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: [],
                bodyNumDoors: [],
                bodyTypes: []
            },
            result: { bodyStyleConfigs: [], vehicleToBodyStyleConfigs: [] },
        };
        //vehicle to wheelbase 
        this.vehicleToWheelBaseSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: []
            },
            result: { wheelBases: [], vehicleToWheelBases: [] },
        };
        this.vehicleToMfrBodyCodeSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: [],
                mfrBodyCodes: []
            },
            result: { mfrBodyCodes: [], vehicleToMfrBodyCodes: [] },
        };
        this.vehicleToDriveTypeSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: [],
                driveTypes: []
            },
            result: { driveTypes: [], vehicleToDriveTypes: [] },
        };
    };
    SystemSearchComponent.prototype.onToggleMenuBarEvent = function (isSystemsMenuExpanded) {
        this.isSystemsMenuExpanded = isSystemsMenuExpanded;
    };
    SystemSearchComponent.prototype.onSearchEvent = function (vehicleToBrakeConfigsForSelectedBrake) {
        this.vehicleToBrakeConfigsForSelectedBrake = vehicleToBrakeConfigsForSelectedBrake;
        this.vehicletobrakeconfigsearch.refreshGrids();
    };
    SystemSearchComponent.prototype.onBedSearchEvent = function (vehicleToBedConfigsForSelectedBed) {
        this.vehicleToBedConfigsForSelectedBed = vehicleToBedConfigsForSelectedBed;
        this.vehicletobedconfigsearch.refreshGrids();
    };
    SystemSearchComponent.prototype.onSelectedSubMenuGroupEvent = function (subMenuGroup) {
        this.activeSubMenuGroup = subMenuGroup;
        this.systemMenubarGrid.activeSubMenuGroup = subMenuGroup;
        this.systemMenubarPanel.activeSubMenuGroup = subMenuGroup;
        this.sharedService.systemMenubarSelected = subMenuGroup;
    };
    SystemSearchComponent.prototype.onBodyStyleConfigSearchEvent = function (vehicleToBodyStyleConfigsForSelectedBodyStyle) {
        this.vehicleToBodyStyleConfigsForSelectedBodyStyle = vehicleToBodyStyleConfigsForSelectedBodyStyle;
        this.vehicletobodystyleconfigsearch.refreshGrids();
    };
    SystemSearchComponent.prototype.onWheelBaseSearchEvent = function (vehicleToWheelBaseForSelectedWheelBase) {
        this.vehicleToWheelBaseForSelectedWheelBase = vehicleToWheelBaseForSelectedWheelBase;
        this.vehicletowheelbasesearch.refreshGrids();
    };
    SystemSearchComponent.prototype.onMfrBodyCodeSearchEvent = function (vehicleToMfrBodyCodesForSelectedMfrBodyCode) {
        this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = vehicleToMfrBodyCodesForSelectedMfrBodyCode;
        this.vehicletomfrbodycodesearch.refreshGrids();
    };
    SystemSearchComponent.prototype.onDriveTypeSearchEvent = function (vehicleToDriveTypesForSelectedDriveType) {
        this.vehicleToDriveTypesForSelectedDriveType = vehicleToDriveTypesForSelectedDriveType;
        this.vehicletodrivetypesearch.refreshGrids();
    };
    __decorate([
        core_1.ViewChild("systemMenubarPanel"), 
        __metadata('design:type', system_menubar_component_1.SystemMenuBar)
    ], SystemSearchComponent.prototype, "systemMenubarPanel", void 0);
    __decorate([
        core_1.ViewChild("systemMenubarGrid"), 
        __metadata('design:type', system_menubar_component_1.SystemMenuBar)
    ], SystemSearchComponent.prototype, "systemMenubarGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicletobrakeconfigsearch"), 
        __metadata('design:type', vehicleToBrakeConfig_search_component_1.VehicleToBrakeConfigSearchComponent)
    ], SystemSearchComponent.prototype, "vehicletobrakeconfigsearch", void 0);
    __decorate([
        core_1.ViewChild("vehicletobedconfigsearch"), 
        __metadata('design:type', vehicleToBedConfig_search_component_1.VehicleToBedConfigSearchComponent)
    ], SystemSearchComponent.prototype, "vehicletobedconfigsearch", void 0);
    __decorate([
        core_1.ViewChild("vehicletobodystyleconfigsearch"), 
        __metadata('design:type', vehicleToBodyStyleConfig_search_component_1.VehicleToBodyStyleConfigSearchComponent)
    ], SystemSearchComponent.prototype, "vehicletobodystyleconfigsearch", void 0);
    __decorate([
        core_1.ViewChild("vehicletowheelbasesearch"), 
        __metadata('design:type', vehicleToWheelBase_search_component_1.VehicleToWheelBaseSearchComponent)
    ], SystemSearchComponent.prototype, "vehicletowheelbasesearch", void 0);
    __decorate([
        core_1.ViewChild("vehicletomfrbodycodesearch"), 
        __metadata('design:type', vehicleToMfrBodyCode_search_component_1.VehicleToMfrBodyCodeSearchComponent)
    ], SystemSearchComponent.prototype, "vehicletomfrbodycodesearch", void 0);
    __decorate([
        core_1.ViewChild("vehicletodrivetypesearch"), 
        __metadata('design:type', vehicleToDriveType_search_component_1.VehicleToDriveTypeSearchComponent)
    ], SystemSearchComponent.prototype, "vehicletodrivetypesearch", void 0);
    SystemSearchComponent = __decorate([
        core_1.Component({
            selector: "system-search",
            templateUrl: "app/templates/system/system-search.component.html",
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], SystemSearchComponent);
    return SystemSearchComponent;
}());
exports.SystemSearchComponent = SystemSearchComponent;
