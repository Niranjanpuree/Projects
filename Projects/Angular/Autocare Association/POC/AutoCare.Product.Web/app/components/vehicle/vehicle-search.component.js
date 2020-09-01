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
var vehicle_search_model_1 = require("./vehicle-search.model");
var vehicle_search_service_1 = require("./vehicle-search.service");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var vehicleToBrakeConfig_service_1 = require("../vehicleToBrakeConfig/vehicleToBrakeConfig.service");
var vehicleToWheelBase_service_1 = require("../vehicleToWheelBase/vehicleToWheelBase.service");
var vehicleToBedConfig_service_1 = require("../vehicleToBedConfig/vehicleToBedConfig.service");
var vehicleToBodyStyleConfig_service_1 = require("../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.service");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_grid_1 = require('../../lib/aclibs/ac-grid/ac-grid');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var vehicleToDriveType_service_1 = require("../vehicleToDriveType/vehicleToDriveType.service");
var vehicleToMfrBodyCode_service_1 = require("../vehicleToMfrBodyCode/vehicleToMfrBodyCode.service");
var VehicleSearchComponent = (function () {
    function VehicleSearchComponent(vehicleToBrakeConfigService, vehicleSearchService, sharedService, router, toastr, navigationService, vehicleToBedConfigService, vehicleToBodyStyleConfigService, vehicleToDriveTypeService, vehicleToWheelBaseService, vehicleToMfrBodyCodeService) {
        this.vehicleToBrakeConfigService = vehicleToBrakeConfigService;
        this.vehicleSearchService = vehicleSearchService;
        this.sharedService = sharedService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.vehicleToBedConfigService = vehicleToBedConfigService;
        this.vehicleToBodyStyleConfigService = vehicleToBodyStyleConfigService;
        this.vehicleToDriveTypeService = vehicleToDriveTypeService;
        this.vehicleToWheelBaseService = vehicleToWheelBaseService;
        this.vehicleToMfrBodyCodeService = vehicleToMfrBodyCodeService;
        this.isHide = false;
        this.vehicleTypeGroupFacet = [];
        this.vehicles = [];
        this.vehicleToBrakeConfigs = [];
        this.vehicleToBedConfigs = [];
        this.vehicleToBodyStyleConfigs = [];
        this.vehicleToDriveTypes = [];
        this.vehicleToWheelBases = [];
        this.vehicleToMfrBodyCodes = [];
        this.isBaseVehiclesExpanded = true;
        this.isVehiclesExpanded = true;
        this.isSystemExpanded = true;
        this.showSystemSelect = false;
        this.showLoadingGif = false;
        this.thresholdRecordCount = 1000; //NOTE: keep this number large so that "select all" checkbox for base vehicles always appears
        this.thresholdRecordCountVehicle = 100;
    }
    VehicleSearchComponent.prototype.ngOnInit = function () {
        this.vehicleSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: [],
            },
            result: { baseVehicles: [], vehicles: [] },
            searchType: vehicle_search_model_1.SearchType.None
        };
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.baseVehicleId = "";
        this.vehicleId = "";
        if (this.sharedService.vehicleSearchViewModel != null) {
            this.vehicleSearchViewModel.facets = this.sharedService.vehicleSearchViewModel.facets;
            this.regionFacet = this.vehicleSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleSearchViewModel.facets.vehicleTypes.slice();
            if (this.sharedService.vehicleSearchViewModel.searchType == vehicle_search_model_1.SearchType.SearchByBaseVehicleId) {
                this.baseVehicleId = this.sharedService.vehicleSearchViewModel.result.baseVehicles[0].id.toString();
                this.searchByBaseVehicleId();
            }
            else if (this.sharedService.vehicleSearchViewModel.searchType == vehicle_search_model_1.SearchType.SearchByVehicleId) {
                this.searchByVehicleId();
            }
            else if (this.sharedService.vehicleSearchViewModel.searchType == vehicle_search_model_1.SearchType.GeneralSearch) {
                this.searchVehicle();
            }
            else {
                this.showLoadingGif = false;
            }
        }
        else {
            this.refreshFacets();
        }
        //this.isSelectAllBaseVehicles = true;
        this.isSelectAllVehicles = false;
        this.selectedSystem = 0;
        this.sharedService.vehicles = null; //clear old selections
        this.sharedService.brakeConfigs = null; //clear old selections
        // Drawer right start
        var headerht = $('header').innerHeight();
        var navht = $('nav').innerHeight();
        var winht = $(window).height();
        var winwt = 800;
        $(".drawer-left").css('min-height', winht - headerht - navht);
        $(".drawer-left").css('width', winwt);
        $(document).on('click', '.drawer-show', function (event) {
            $(".drawer-left").css('width', winwt);
        });
        $(".drawer-left span").on('click', function () {
            var drawerwt = $(".drawer-left").width();
            if (drawerwt == 15) {
                $(".drawer-left").css('width', winwt);
            }
            else {
                $(".drawer-left").css('width', 15);
            }
        });
        $(document).on('click', function (event) {
            if (!$(event.target).closest(".drawer-left").length) {
                // Hide the menus.
                var drawerwt = $(".drawer-left").width();
                if (drawerwt > 15) {
                    $(".drawer-left").css('width', 15);
                }
            }
        });
        //$("#main").on('click', function() {
        //    var drawerwt = $(".drawer-left").width();
        //    if (drawerwt > 15) {
        //        $(".drawer-left").css('width', 15);
        //    } 
        //});
        // Drawer right end
        this.configSystems = vehicle_search_model_1.ConfigurationSystems;
    };
    VehicleSearchComponent.prototype.clearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.baseVehicleId = "";
        this.vehicleId = "";
        if (this.vehicleSearchViewModel.facets.regions) {
            this.vehicleSearchViewModel.facets.regions.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleSearchViewModel.facets.makes) {
            this.vehicleSearchViewModel.facets.makes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleSearchViewModel.facets.models) {
            this.vehicleSearchViewModel.facets.models.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleSearchViewModel.facets.subModels) {
            this.vehicleSearchViewModel.facets.subModels.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleSearchViewModel.facets.vehicleTypeGroups.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleSearchViewModel.facets.vehicleTypes) {
            this.vehicleSearchViewModel.facets.vehicleTypes.forEach(function (item) { return item.isSelected = false; });
        }
        this.refreshFacets();
    };
    VehicleSearchComponent.prototype.getDefaultInputModel = function () {
        return {
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
        };
    };
    VehicleSearchComponent.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleSearchViewModel.facets.regions) {
            this.vehicleSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        if (this.vehicleSearchViewModel.facets.makes) {
            this.vehicleSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleSearchViewModel.facets.models) {
            this.vehicleSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleSearchViewModel.facets.subModels) {
            this.vehicleSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.subModels.push(m.name); });
        }
        if (this.vehicleSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleSearchViewModel.facets.vehicleTypes) {
            this.vehicleSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        this.showLoadingGif = true;
        this.vehicleSearchService.refreshFacets(inputModel).subscribe(function (data) {
            _this.updateRegionFacet(data.facets.regions);
            _this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
            _this.updateVehicleTypeFacet(data.facets.vehicleTypes);
            _this.updateYearFacet(data.facets.years);
            _this.updateMakeFacet(data.facets.makes);
            _this.updateModelFacet(data.facets.models, "");
            _this.updateSubModelFacet(data.facets.subModels, "");
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleSearchComponent.prototype.filterMakes = function ($event) {
        if (this.vehicleSearchViewModel != null &&
            this.vehicleSearchViewModel.facets != null &&
            this.vehicleSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleSearchComponent.prototype.filterModels = function ($event) {
        if (this.vehicleSearchViewModel != null &&
            this.vehicleSearchViewModel.facets != null &&
            this.vehicleSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleSearchComponent.prototype.filterSubModels = function ($event) {
        if (this.vehicleSearchViewModel != null &&
            this.vehicleSearchViewModel.facets != null &&
            this.vehicleSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleSearchComponent.prototype.filterRegions = function ($event) {
        if (this.vehicleSearchViewModel != null &&
            this.vehicleSearchViewModel.facets != null &&
            this.vehicleSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleSearchComponent.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleSearchViewModel != null &&
            this.vehicleSearchViewModel.facets != null &&
            this.vehicleSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleSearchComponent.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleSearchViewModel != null &&
            this.vehicleSearchViewModel.facets != null &&
            this.vehicleSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleSearchComponent.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    VehicleSearchComponent.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    VehicleSearchComponent.prototype.updateRegionFacet = function (regions) {
        var existingSelectedRegions = this.vehicleSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleSearchViewModel.facets.regions = [];
        for (var _i = 0, regions_1 = regions; _i < regions_1.length; _i++) {
            var item = regions_1[_i];
            var newItem = {
                name: item,
                isSelected: false
            };
            for (var _a = 0, existingSelectedRegions_1 = existingSelectedRegions; _a < existingSelectedRegions_1.length; _a++) {
                var existingSelectedRegion = existingSelectedRegions_1[_a];
                if (item === existingSelectedRegion) {
                    newItem.isSelected = true;
                }
            }
            this.vehicleSearchViewModel.facets.regions.push(newItem);
        }
        this.regionFacet = this.vehicleSearchViewModel.facets.regions.slice();
    };
    VehicleSearchComponent.prototype.updateYearFacet = function (years) {
        this.vehicleSearchViewModel.facets.startYears = years.slice();
        this.vehicleSearchViewModel.facets.endYears = years.slice();
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    VehicleSearchComponent.prototype.updateMakeFacet = function (makes) {
        var existingSelectedMakes = this.vehicleSearchViewModel.facets.makes.filter(function (make) { return make.isSelected; }).map(function (item) { return item.name; });
        this.vehicleSearchViewModel.facets.makes = [];
        for (var _i = 0, makes_1 = makes; _i < makes_1.length; _i++) {
            var make = makes_1[_i];
            var newMake = {
                name: make,
                isSelected: false
            };
            for (var _a = 0, existingSelectedMakes_1 = existingSelectedMakes; _a < existingSelectedMakes_1.length; _a++) {
                var existingSelectedMake = existingSelectedMakes_1[_a];
                if (make === existingSelectedMake) {
                    newMake.isSelected = true;
                }
            }
            this.vehicleSearchViewModel.facets.makes.push(newMake);
        }
        this.makeFacet = this.vehicleSearchViewModel.facets.makes.slice();
    };
    VehicleSearchComponent.prototype.updateModelFacet = function (models, makeName) {
        var existingSelectedModels = this.vehicleSearchViewModel.facets.models.filter(function (model) { return model.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleSearchViewModel.facets.models = [];
        for (var _i = 0, models_1 = models; _i < models_1.length; _i++) {
            var model = models_1[_i];
            var newModel = {
                name: model,
                isSelected: false,
            };
            for (var _a = 0, existingSelectedModels_1 = existingSelectedModels; _a < existingSelectedModels_1.length; _a++) {
                var existingSelectedModel = existingSelectedModels_1[_a];
                if (model === existingSelectedModel) {
                    newModel.isSelected = true;
                }
            }
            this.vehicleSearchViewModel.facets.models.push(newModel);
        }
        this.modelFacet = this.vehicleSearchViewModel.facets.models.slice();
    };
    VehicleSearchComponent.prototype.updateSubModelFacet = function (subModels, modelName) {
        var existingSelectedSubModels = this.vehicleSearchViewModel.facets.subModels.filter(function (submodel) { return submodel.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleSearchViewModel.facets.subModels = [];
        for (var _i = 0, subModels_1 = subModels; _i < subModels_1.length; _i++) {
            var subModel = subModels_1[_i];
            var newSubModel = {
                name: subModel,
                isSelected: false,
            };
            for (var _a = 0, existingSelectedSubModels_1 = existingSelectedSubModels; _a < existingSelectedSubModels_1.length; _a++) {
                var existingSelectedSubModel = existingSelectedSubModels_1[_a];
                if (subModel === existingSelectedSubModel) {
                    newSubModel.isSelected = true;
                }
            }
            this.vehicleSearchViewModel.facets.subModels.push(newSubModel);
        }
        this.subModelFacet = this.vehicleSearchViewModel.facets.subModels.slice();
    };
    VehicleSearchComponent.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
        var existingSelectedItems = this.vehicleSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleSearchViewModel.facets.vehicleTypeGroups = [];
        for (var _i = 0, vehicleTypeGroups_1 = vehicleTypeGroups; _i < vehicleTypeGroups_1.length; _i++) {
            var item = vehicleTypeGroups_1[_i];
            var newItem = {
                name: item,
                isSelected: false
            };
            for (var _a = 0, existingSelectedItems_1 = existingSelectedItems; _a < existingSelectedItems_1.length; _a++) {
                var existingSelectedItem = existingSelectedItems_1[_a];
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }
            this.vehicleSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }
        this.vehicleTypeGroupFacet = this.vehicleSearchViewModel.facets.vehicleTypeGroups.slice();
    };
    VehicleSearchComponent.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
        var existingSelectedItems = this.vehicleSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleSearchViewModel.facets.vehicleTypes = [];
        for (var _i = 0, vehicleTypes_1 = vehicleTypes; _i < vehicleTypes_1.length; _i++) {
            var item = vehicleTypes_1[_i];
            var newItem = {
                name: item,
                isSelected: false
            };
            for (var _a = 0, existingSelectedItems_2 = existingSelectedItems; _a < existingSelectedItems_2.length; _a++) {
                var existingSelectedItem = existingSelectedItems_2[_a];
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }
            this.vehicleSearchViewModel.facets.vehicleTypes.push(newItem);
        }
        this.vehicleTypeFacet = this.vehicleSearchViewModel.facets.vehicleTypes.slice();
    };
    VehicleSearchComponent.prototype.searchVehicle = function () {
        var _this = this;
        this.vehicleSearchViewModel.searchType = vehicle_search_model_1.SearchType.GeneralSearch;
        this.vehicles = [];
        this.vehicleToBrakeConfigs = [];
        this.isSelectAllVehicles = false;
        var inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleSearchViewModel.facets.regions) {
            this.vehicleSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        if (this.vehicleSearchViewModel.facets.makes) {
            this.vehicleSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleSearchViewModel.facets.models) {
            this.vehicleSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleSearchViewModel.facets.subModels) {
            this.vehicleSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.subModels.push(m.name); });
        }
        if (this.vehicleSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleSearchViewModel.facets.vehicleTypes) {
            this.vehicleSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        this.showLoadingGif = true;
        this.vehicleSearchService.search(inputModel).subscribe(function (m) {
            if (m.result.baseVehicles.length > 0 || m.result.vehicles.length > 0) {
                _this.getSearchResult(m);
                _this.showLoadingGif = false;
                $(".drawer-left").css('width', 15);
            }
            else {
                _this.toastr.warning("The search yeilded no result", "No Record Found!!");
                _this.showLoadingGif = false;
            }
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleSearchComponent.prototype.onBaseIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.searchByBaseVehicleId();
        }
    };
    VehicleSearchComponent.prototype.searchByBaseVehicleId = function () {
        var _this = this;
        var baseId = Number(this.baseVehicleId);
        if (isNaN(baseId)) {
            this.toastr.warning("Invalid Base ID", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.vehicleSearchViewModel.searchType = vehicle_search_model_1.SearchType.SearchByBaseVehicleId;
        this.showLoadingGif = true;
        this.vehicles = [];
        this.vehicleToBrakeConfigs = [];
        this.isSelectAllVehicles = false;
        this.vehicleSearchService.searchByBaseVehicleId(baseId).subscribe(function (m) {
            if (m.result.baseVehicles.length > 0 || m.result.vehicles.length > 0) {
                _this.getSearchResult(m);
                _this.showLoadingGif = false;
                $(".drawer-left").css('width', 15);
            }
            else {
                _this.toastr.warning("The search yeilded no result", "No Record Found!!");
                _this.showLoadingGif = false;
            }
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleSearchComponent.prototype.onVehicleIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.searchByVehicleId();
        }
    };
    VehicleSearchComponent.prototype.searchByVehicleId = function () {
        var _this = this;
        var vehicleId = Number(this.vehicleId);
        if (isNaN(vehicleId)) {
            this.toastr.warning("Invalid Vehicle ID", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.vehicleSearchViewModel.searchType = vehicle_search_model_1.SearchType.SearchByVehicleId;
        this.showLoadingGif = true;
        this.vehicles = [];
        this.vehicleToBrakeConfigs = [];
        this.isSelectAllVehicles = false;
        this.vehicleSearchService.searchByVehicleId(vehicleId).subscribe(function (m) {
            if (m.result.baseVehicles.length > 0 || m.result.vehicles.length > 0) {
                _this.getSearchResult(m);
                _this.showLoadingGif = false;
                $(".drawer-left").css('width', 15);
            }
            else {
                _this.toastr.warning("The search yeilded no result", "No Record Found!!");
                _this.showLoadingGif = false;
            }
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    // wrapper privates
    VehicleSearchComponent.prototype.onSelectAllBaseVehicle = function (selected) {
        var _this = this;
        this.isSelectAllBaseVehicles = selected;
        if (!selected) {
            this.isSelectAllVehicles = false; // uncheck slect all vehicle from vehicles when select all base vehicle unchecked
            this.vehicleSearchViewModel.result.vehicles.forEach(function (x) { return x.isSelected = false; });
        }
        if (this.vehicleSearchViewModel == null) {
            return;
        }
        this.vehicles = [];
        this.vehicleToBrakeConfigs = [];
        this.vehicleSearchViewModel.result.baseVehicles.forEach(function (item) {
            item.isSelected = selected;
            _this.refreshVehiclesWithBaseVehicleId(item.id, item.isSelected);
        });
        // refresh grids
        if (this.vehicleGrid)
            this.vehicleGrid.refresh();
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
    };
    VehicleSearchComponent.prototype.onBaseVehicleSelected = function (baseVehicle) {
        this.refreshVehiclesWithBaseVehicleId(baseVehicle.id, !baseVehicle.isSelected);
        if (baseVehicle.isSelected) {
            //unchecked
            this.isSelectAllBaseVehicles = false;
        }
        else {
            //checked
            var excludedBaseVehicle = this.vehicleSearchViewModel.result.baseVehicles.filter(function (item) { return item.id != baseVehicle.id; });
            if (excludedBaseVehicle.every(function (item) { return item.isSelected; })) {
                this.isSelectAllBaseVehicles = true;
            }
            this.isSelectAllVehicles = false;
        }
        // check if all vehicles are selected for less then equal to threshold of vehicle.
        if (this.vehicles && this.vehicles.length > 0 &&
            this.vehicles.length <= this.thresholdRecordCountVehicle && this.vehicles.every(function (item) { return item.isSelected; })) {
            this.isSelectAllVehicles = true;
        }
        else {
            this.isSelectAllVehicles = false;
        }
        // refresh grids
        if (this.vehicleGrid)
            this.vehicleGrid.refresh();
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
    };
    VehicleSearchComponent.prototype.refreshVehiclesWithBaseVehicleId = function (baseVehicleId, isSelected) {
        if (isSelected) {
            var vehiclesRetrieved = this.getVehiclesByBaseVehicleId(baseVehicleId);
            //add this vehicles to vehiclesGrid array
            for (var _i = 0, vehiclesRetrieved_1 = vehiclesRetrieved; _i < vehiclesRetrieved_1.length; _i++) {
                var vehicle = vehiclesRetrieved_1[_i];
                this.vehicles.push(vehicle);
            }
        }
        else {
            var removedVehicles = this.vehicles.filter(function (x) { return x.baseVehicleId == baseVehicleId; });
            removedVehicles.forEach(function (item) { return item.isSelected = false; });
            //refresh brakes
            this.vehicleToBrakeConfigs = this.vehicleToBrakeConfigs.filter(function (item) { return removedVehicles.map(function (v) { return v.id; }).indexOf(item.vehicleId) < 0; });
            this.vehicles = this.vehicles.filter(function (x) { return x.baseVehicleId != baseVehicleId; });
        }
    };
    VehicleSearchComponent.prototype.getVehiclesByBaseVehicleId = function (id) {
        return this.vehicleSearchViewModel.result.vehicles.filter(function (v) { return v.baseVehicleId == id; });
    };
    VehicleSearchComponent.prototype.getSystemInfoByVehicleId = function (id) {
        this.selectedSystemType = "brake";
        if (this.selectedSystemType == "brake") {
        }
        else if (this.selectedSystemType == "engine") {
        }
        else {
        }
    };
    VehicleSearchComponent.prototype.filterMenu = function () {
        if (this.isHide) {
            return "";
        }
        else {
            return "activate";
        }
    };
    VehicleSearchComponent.prototype.onSelectAllVehicle = function (selected) {
        var _this = this;
        this.isSelectAllVehicles = selected;
        if (this.vehicles == null) {
            return;
        }
        this.vehicles.forEach(function (item) { return item.isSelected = selected; });
        if (this.selectedSystem === vehicle_search_model_1.ConfigurationSystems.Brake) {
            this.vehicleToBrakeConfigs = [];
            if (selected) {
                this.showLoadingGif = true;
                this.vehicleToBrakeConfigService.searchByVehicleIds(this.vehicles.filter(function (item) { return item.isSelected; }).map(function (item) { return item.id; })).subscribe(function (m) {
                    _this.vehicles.forEach(function (v) {
                        v.vehicleToBrakeConfigs = m.filter(function (item) { return item.vehicle.id == v.id; });
                        _this.vehicleToBrakeConfigs = _this.vehicleToBrakeConfigs.concat(v.vehicleToBrakeConfigs);
                    });
                    _this.showLoadingGif = false;
                }, function (error) {
                    _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                    _this.showLoadingGif = false;
                });
            }
        }
        // refresh grids
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
    };
    VehicleSearchComponent.prototype.onAddVehicleToBrakeConfigs = function () {
        this.sharedService.vehicles = this.vehicles.filter(function (item) { return item.isSelected; });
        this.sharedService.vehicles.forEach(function (item) {
            if (item.vehicleToBrakeConfigs) {
                item.vehicleToBrakeConfigCount = item.vehicleToBrakeConfigs.length;
            }
            else {
                item.vehicleToBrakeConfigCount = 0;
            }
        });
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletobrakeconfig/add"]);
    };
    VehicleSearchComponent.prototype.onAddVehicleToBedConfigs = function () {
        this.sharedService.vehicles = this.vehicles.filter(function (item) { return item.isSelected; });
        this.sharedService.vehicles.forEach(function (item) {
            if (item.vehicleToBedConfigs) {
                item.vehicleToBedConfigCount = item.vehicleToBedConfigs.length;
            }
            else {
                item.vehicleToBedConfigCount = 0;
            }
        });
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletobedconfig/add"]);
    };
    VehicleSearchComponent.prototype.onAddVehicleToBodyStyleConfigs = function () {
        this.sharedService.vehicles = this.vehicles.filter(function (item) { return item.isSelected; });
        this.sharedService.vehicles.forEach(function (item) {
            if (item.vehicleToBodyStyleConfigs) {
                item.vehicleToBodyStyleConfigCount = item.vehicleToBodyStyleConfigs.length;
            }
            else {
                item.vehicleToBodyStyleConfigCount = 0;
            }
        });
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletobodystyleconfig/add"]);
    };
    VehicleSearchComponent.prototype.onAddVehicleToDriveTypes = function () {
        this.sharedService.vehicles = this.vehicles.filter(function (item) { return item.isSelected; });
        this.sharedService.vehicles.forEach(function (item) {
            if (item.vehicleToDriveTypes) {
                item.vehicleToDriveTypeCount = item.vehicleToDriveTypes.length;
            }
            else {
                item.vehicleToDriveTypeCount = 0;
            }
        });
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletodrivetype/add"]);
    };
    VehicleSearchComponent.prototype.onAddVehicleToMfrBodyCodes = function () {
        this.sharedService.vehicles = this.vehicles.filter(function (item) { return item.isSelected; });
        this.sharedService.vehicles.forEach(function (item) {
            if (item.vehicleToMfrBodyCodes) {
                item.vehicleToMfrBodyCodeCount = item.vehicleToMfrBodyCodes.length;
            }
            else {
                item.vehicleToMfrBodyCodeCount = 0;
            }
        });
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletomfrbodycode/add"]);
    };
    VehicleSearchComponent.prototype.getSearchResult = function (m) {
        var _this = this;
        //NOTE: do not replace this.vehicleSearchViewModel.facets = m.facets;
        //NOTE: do not replace this.vehicleSearchViewModel.filters
        this.vehicleSearchViewModel.result = m.result;
        this.vehicleSearchViewModel.totalCount = m.totalCount;
        this.vehicles = [];
        this.vehicleToBrakeConfigs = [];
        this.isSelectAllBaseVehicles = false;
        // note: select all only if totalRecords <= thresholdRecordCount
        // else no selection.
        if (this.vehicleSearchViewModel.result.baseVehicles.length <= this.thresholdRecordCount) {
            this.vehicleSearchViewModel.result.baseVehicles.forEach(function (item) {
                item.isSelected = true;
                _this.refreshVehiclesWithBaseVehicleId(item.id, item.isSelected);
            });
            this.isSelectAllBaseVehicles = true;
        }
        // refresh grid
        if (this.baseVehicleGrid)
            this.baseVehicleGrid.refresh();
        if (this.vehicleGrid)
            this.vehicleGrid.refresh();
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
        if (this.vehicleToMfrBodyCodeGrid)
            this.vehicleToMfrBodyCodeGrid.refresh();
    };
    VehicleSearchComponent.prototype.onVehicleSelected = function (vehicle, event) {
        if (event.target.checked) {
            switch (this.selectedSystem) {
                case vehicle_search_model_1.ConfigurationSystems.Brake:
                    this.loadVehicleToBrakeSystem(vehicle);
                    break;
                case vehicle_search_model_1.ConfigurationSystems.Bed:
                    this.loadVehicleToBedSystem(vehicle);
                    break;
                case vehicle_search_model_1.ConfigurationSystems.Body:
                    this.loadVehicleToBodyStyleSystem(vehicle);
                    break;
                case vehicle_search_model_1.ConfigurationSystems.Drive:
                    this.loadVehicleToDriveSystem(vehicle);
                    break;
                case vehicle_search_model_1.ConfigurationSystems.Wheel:
                    this.loadVehicleToWheelBaseSystem(vehicle);
                    break;
                case vehicle_search_model_1.ConfigurationSystems.MFR:
                    this.loadVehicleToMfrBodyCodeSystem(vehicle);
                    break;
                default:
                    break;
            }
        }
        else {
            this.vehicleToBrakeConfigs = this.vehicleToBrakeConfigs.filter(function (item) { return item.vehicleId != vehicle.id; });
            this.vehicleToBedConfigs = this.vehicleToBedConfigs.filter(function (item) { return item.vehicleId != vehicle.id; });
            this.vehicleToBodyStyleConfigs = this.vehicleToBodyStyleConfigs.filter(function (item) { return item.vehicleId != vehicle.id; });
            this.vehicleToDriveTypes = this.vehicleToDriveTypes.filter(function (item) { return item.vehicleId != vehicle.id; });
            this.vehicleToWheelBases = this.vehicleToWheelBases.filter(function (item) { return item.vehicleId != vehicle.id; });
            this.vehicleToMfrBodyCodes = this.vehicleToMfrBodyCodes.filter(function (item) { return item.vehicleId != vehicle.id; });
        }
        if (vehicle.isSelected) {
            //unchecked
            this.isSelectAllVehicles = false;
        }
        else {
            //checked
            var excludedVehicle = this.vehicles.filter(function (item) { return item.id != vehicle.id; });
            if (excludedVehicle.every(function (item) { return item.isSelected; })) {
                this.isSelectAllVehicles = true;
            }
        }
        // refresh grids
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
        if (this.vehicleToMfrBodyCodeGrid)
            this.vehicleToMfrBodyCodeGrid.refresh();
    };
    VehicleSearchComponent.prototype.loadVehicleToBrakeSystem = function (vehicle) {
        var _this = this;
        if (vehicle.vehicleToBrakeConfigs == null || vehicle.vehicleToBrakeConfigs.length == 0) {
            this.showLoadingGif = true;
            this.vehicleToBrakeConfigService.searchByVehicleIds([vehicle.id]).subscribe(function (m) {
                vehicle.vehicleToBrakeConfigs = m;
                _this.vehicleToBrakeConfigs = _this.vehicleToBrakeConfigs.concat(m);
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }
        else {
            this.vehicleToBrakeConfigs = this.vehicleToBrakeConfigs.concat(vehicle.vehicleToBrakeConfigs);
        }
    };
    VehicleSearchComponent.prototype.loadVehicleToBedSystem = function (vehicle) {
        var _this = this;
        if (vehicle.vehicleToBedConfigs == null || vehicle.vehicleToBedConfigs.length == 0) {
            this.showLoadingGif = true;
            this.vehicleToBedConfigService.searchByVehicleIds([vehicle.id]).subscribe(function (m) {
                vehicle.vehicleToBedConfigs = m;
                _this.vehicleToBedConfigs = _this.vehicleToBedConfigs.concat(m);
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }
        else {
            this.vehicleToBedConfigs = this.vehicleToBedConfigs.concat(vehicle.vehicleToBedConfigs);
        }
    };
    VehicleSearchComponent.prototype.loadVehicleToBodyStyleSystem = function (vehicle) {
        var _this = this;
        if (vehicle.vehicleToBodyStyleConfigs == null || vehicle.vehicleToBodyStyleConfigs.length == 0) {
            this.showLoadingGif = true;
            this.vehicleToBodyStyleConfigService.searchByVehicleIds([vehicle.id]).subscribe(function (m) {
                vehicle.vehicleToBodyStyleConfigs = m;
                _this.vehicleToBodyStyleConfigs = _this.vehicleToBodyStyleConfigs.concat(m);
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }
        else {
            this.vehicleToBodyStyleConfigs = this.vehicleToBodyStyleConfigs.concat(vehicle.vehicleToBodyStyleConfigs);
        }
    };
    VehicleSearchComponent.prototype.loadVehicleToDriveSystem = function (vehicle) {
        var _this = this;
        if (vehicle.vehicleToDriveTypes == null || vehicle.vehicleToDriveTypes.length == 0) {
            this.showLoadingGif = true;
            this.vehicleToDriveTypeService.searchByVehicleIds([vehicle.id]).subscribe(function (m) {
                vehicle.vehicleToDriveTypes = m;
                _this.vehicleToDriveTypes = _this.vehicleToDriveTypes.concat(m);
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }
        else {
            this.vehicleToDriveTypes = this.vehicleToDriveTypes.concat(vehicle.vehicleToDriveTypes);
        }
    };
    VehicleSearchComponent.prototype.loadVehicleToWheelBaseSystem = function (vehicle) {
        var _this = this;
        if (vehicle.vehicleToWheelBases == null || vehicle.vehicleToWheelBases.length == 0) {
            this.showLoadingGif = true;
            this.vehicleToWheelBaseService.searchByVehicleIds([vehicle.id]).subscribe(function (m) {
                vehicle.vehicleToWheelBases = m;
                _this.vehicleToWheelBases = _this.vehicleToWheelBases.concat(m);
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }
        else {
            this.vehicleToWheelBases = this.vehicleToWheelBases.concat(vehicle.vehicleToWheelBases);
        }
    };
    VehicleSearchComponent.prototype.loadVehicleToMfrBodyCodeSystem = function (vehicle) {
        var _this = this;
        if (vehicle.vehicleToMfrBodyCodes == null || vehicle.vehicleToMfrBodyCodes.length == 0) {
            this.showLoadingGif = true;
            this.vehicleToMfrBodyCodeService.searchByVehicleIds([vehicle.id]).subscribe(function (m) {
                vehicle.vehicleToMfrBodyCodes = m;
                _this.vehicleToMfrBodyCodes = _this.vehicleToMfrBodyCodes.concat(m);
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }
        else {
            this.vehicleToMfrBodyCodes = this.vehicleToMfrBodyCodes.concat(vehicle.vehicleToMfrBodyCodes);
        }
    };
    VehicleSearchComponent.prototype.onDeleteVehicleToBrakeConfig = function (vehicleToBrakeConfig) {
        this.deleteVehicleToBrakeConfig = vehicleToBrakeConfig;
        this.deleteBrakeAssociationPopup.open("sm");
    };
    VehicleSearchComponent.prototype.onDeleteVehicleToBedConfig = function (vehicleToBedConfig) {
        this.deleteVehicleToBedConfig = vehicleToBedConfig;
        this.deleteBedAssociationPopup.open("sm");
    };
    VehicleSearchComponent.prototype.onDeleteVehicleToBodyStyleConfig = function (vehicleToBodyStyleConfig) {
        this.deleteVehicleToBodyStyleConfig = vehicleToBodyStyleConfig;
        this.deleteBodyStyleAssociationPopup.open("sm");
    };
    VehicleSearchComponent.prototype.onDeleteVehicleToDriveType = function (vehicleToDriveType) {
        this.deleteVehicleToDriveType = vehicleToDriveType;
        this.deleteDriveTypeAssociationPopup.open("sm");
    };
    VehicleSearchComponent.prototype.onDeleteVehicleToWheelBase = function (vehicleToWheelBase) {
        this.deleteVehicleToWheelBase = vehicleToWheelBase;
        this.deleteWheelBaseAssociationPopup.open("sm");
    };
    VehicleSearchComponent.prototype.onDeleteVehicleToMfrBodyCode = function (vehicleToMfrBodyCode) {
        this.deleteVehicleToMfrBodyCode = vehicleToMfrBodyCode;
        this.deleteMfrBodyCodeAssociationPopup.open("sm");
    };
    VehicleSearchComponent.prototype.onDeleteVehicleToBrakeConfigSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteVehicleToBrakeConfig.attachments = uploadedFiles;
            }
            if (_this.deleteVehicleToBrakeConfig.attachments) {
                _this.deleteVehicleToBrakeConfig.attachments = _this.deleteVehicleToBrakeConfig.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleToBrakeConfigService.deleteVehicleToBrakeConfig(_this.deleteVehicleToBrakeConfig.id, _this.deleteVehicleToBrakeConfig).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Brake Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBrakeConfig.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.deleteVehicleToBrakeConfig.id + "\" Brake Association change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBrakeConfig.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBrakeConfig.id);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.acFileUploader.reset(true);
                _this.deleteBrakeAssociationPopup.close();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.acFileUploader.reset(true);
            _this.deleteBrakeAssociationPopup.close();
            _this.showLoadingGif = false;
        });
    };
    VehicleSearchComponent.prototype.onDeleteVehicleToBedConfigSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteVehicleToBedConfig.attachments = uploadedFiles;
            }
            if (_this.deleteVehicleToBedConfig.attachments) {
                _this.deleteVehicleToBedConfig.attachments = _this.deleteVehicleToBedConfig.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleToBedConfigService.deleteVehicleToBedConfig(_this.deleteVehicleToBedConfig.id, _this.deleteVehicleToBedConfig).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Bed Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBedConfig.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.deleteVehicleToBedConfig.id + "\" Bed Association change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBedConfig.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBedConfig.id);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.acFileUploader.reset(true);
                _this.deleteBedAssociationPopup.close();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.acFileUploader.reset(true);
            _this.deleteBedAssociationPopup.close();
            _this.showLoadingGif = false;
        });
    };
    VehicleSearchComponent.prototype.onDeleteVehicleToBodyStyleConfigSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteVehicleToBodyStyleConfig.attachments = uploadedFiles;
            }
            if (_this.deleteVehicleToBodyStyleConfig.attachments) {
                _this.deleteVehicleToBodyStyleConfig.attachments = _this.deleteVehicleToBodyStyleConfig.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleToBodyStyleConfigService.deleteVehicleToBodyStyleConfig(_this.deleteVehicleToBodyStyleConfig.id, _this.deleteVehicleToBodyStyleConfig).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Body Style Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBodyStyleConfig.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.deleteVehicleToBodyStyleConfig.id + "\" Body Style Association change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body Style Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBodyStyleConfig.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body Style Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBodyStyleConfig.id);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.acFileUploader.reset(true);
                _this.deleteBodyStyleAssociationPopup.close();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.acFileUploader.reset(true);
            _this.deleteBodyStyleAssociationPopup.close();
            _this.showLoadingGif = false;
        });
    };
    VehicleSearchComponent.prototype.onDeleteVehicleToDriveTypeSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteVehicleToDriveType.attachments = uploadedFiles;
            }
            if (_this.deleteVehicleToDriveType.attachments) {
                _this.deleteVehicleToDriveType.attachments = _this.deleteVehicleToDriveType.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleToDriveTypeService.deleteVehicleToDriveType(_this.deleteVehicleToDriveType.id, _this.deleteVehicleToDriveType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Drive Type Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToDriveType.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.deleteVehicleToDriveType.id + "\" Drive Type Association change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Drive Type Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToDriveType.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Drive Type Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToDriveType.id);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.acFileUploader.reset(true);
                _this.deleteDriveTypeAssociationPopup.close();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.acFileUploader.reset(true);
            _this.deleteDriveTypeAssociationPopup.close();
            _this.showLoadingGif = false;
        });
    };
    VehicleSearchComponent.prototype.onDeleteVehicleToWheelBaseSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteVehicleToWheelBase.attachments = uploadedFiles;
            }
            if (_this.deleteVehicleToWheelBase.attachments) {
                _this.deleteVehicleToWheelBase.attachments = _this.deleteVehicleToWheelBase.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleToWheelBaseService.deleteVehicleToWheelBase(_this.deleteVehicleToWheelBase.id, _this.deleteVehicleToWheelBase).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Wheel Base Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToWheelBase.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.deleteVehicleToWheelBase.id + "\" Wheel Base Association change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Wheel Base Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToWheelBase.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Wheel Base Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToWheelBase.id);
                _this.toastr.warning(error, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.acFileUploader.reset(true);
                _this.deleteWheelBaseAssociationPopup.close();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.acFileUploader.reset(true);
            _this.deleteWheelBaseAssociationPopup.close();
            _this.showLoadingGif = false;
        });
    };
    VehicleSearchComponent.prototype.onDeleteVehicleToMfrBodyCodeSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteVehicleToMfrBodyCode.attachments = uploadedFiles;
            }
            if (_this.deleteVehicleToMfrBodyCode.attachments) {
                _this.deleteVehicleToMfrBodyCode.attachments = _this.deleteVehicleToMfrBodyCode.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleToMfrBodyCodeService.deleteVehicleToMfrBodyCode(_this.deleteVehicleToMfrBodyCode.id, _this.deleteVehicleToMfrBodyCode).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("MFR Body Code Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToMfrBodyCode.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.deleteVehicleToMfrBodyCode.id + "\" MFR Body Code Association change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("MFR Body Code Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToMfrBodyCode.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("MFR Body Code Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToMfrBodyCode.id);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.acFileUploader.reset(true);
                _this.deleteMfrBodyCodeAssociationPopup.close();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.acFileUploader.reset(true);
            _this.deleteMfrBodyCodeAssociationPopup.close();
            _this.showLoadingGif = false;
        });
    };
    VehicleSearchComponent.prototype.systemSelect = function (selectedSystem) {
        this.showSystemSelect = !this.showSystemSelect;
        this.selectedSystem = selectedSystem;
        this.vehicleToBrakeConfigs = [];
        switch (this.selectedSystem) {
            case vehicle_search_model_1.ConfigurationSystems.Brake:
                this.loadVehicleToBrakeSystemOnSystemSelect();
                break;
            case vehicle_search_model_1.ConfigurationSystems.Bed:
                this.loadVehicleToBedSystemOnSystemSelect();
                break;
            case vehicle_search_model_1.ConfigurationSystems.Body:
                this.loadVehicleToBodyStyleSystemOnSystemSelect();
                break;
            case vehicle_search_model_1.ConfigurationSystems.Drive:
                this.loadVehicleToDriveTypeSystemOnSystemSelect();
                break;
            case vehicle_search_model_1.ConfigurationSystems.Wheel:
                this.vehicleToWheelBases = [];
                this.loadVehicleToWheelSystemOnSystemSelect();
                break;
            case vehicle_search_model_1.ConfigurationSystems.MFR:
                this.loadVehicleToMFRSystemOnSystemSelect();
                break;
            default:
                break;
        }
    };
    VehicleSearchComponent.prototype.loadVehicleToBrakeSystemOnSystemSelect = function () {
        var _this = this;
        if (this.vehicles.filter(function (x) { return x.isSelected; }).length) {
            var selectedTemp = this.vehicles.filter(function (x) { return x.isSelected; });
            this.vehicles.filter(function (x) { return x.isSelected; }).forEach(function (vehicle) {
                if (vehicle.vehicleToBrakeConfigs) {
                    _this.vehicleToBrakeConfigs = _this.vehicleToBrakeConfigs.concat(vehicle.vehicleToBrakeConfigs);
                    selectedTemp = selectedTemp.filter(function (x) { return x.id !== vehicle.id; });
                }
            });
            if (selectedTemp.length > 0) {
                this.showLoadingGif = true;
                this.vehicleToBrakeConfigService.searchByVehicleIds(selectedTemp.map(function (item) { return item.id; })).subscribe(function (m) {
                    _this.vehicles.forEach(function (v) {
                        v.vehicleToBrakeConfigs = m.filter(function (item) { return item.vehicle.id === v.id; });
                        _this.vehicleToBrakeConfigs = _this.vehicleToBrakeConfigs.concat(v.vehicleToBrakeConfigs);
                    });
                    _this.showLoadingGif = false;
                }, function (error) {
                    _this.showLoadingGif = false;
                    _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                });
            }
        }
    };
    VehicleSearchComponent.prototype.loadVehicleToBedSystemOnSystemSelect = function () {
        var _this = this;
        if (this.vehicles.filter(function (x) { return x.isSelected; }).length) {
            var selectedTemp = this.vehicles.filter(function (x) { return x.isSelected; });
            this.vehicles.filter(function (x) { return x.isSelected; }).forEach(function (vehicle) {
                if (vehicle.vehicleToBedConfigs) {
                    _this.vehicleToBedConfigs = _this.vehicleToBedConfigs.concat(vehicle.vehicleToBedConfigs);
                    selectedTemp = selectedTemp.filter(function (x) { return x.id !== vehicle.id; });
                }
            });
            if (selectedTemp.length > 0) {
                this.showLoadingGif = true;
                this.vehicleToBedConfigService.searchByVehicleIds(selectedTemp.map(function (item) { return item.id; })).subscribe(function (m) {
                    _this.vehicles.forEach(function (v) {
                        v.vehicleToBedConfigs = m.filter(function (item) { return item.vehicle.id === v.id; });
                        _this.vehicleToBedConfigs = _this.vehicleToBedConfigs.concat(v.vehicleToBedConfigs);
                    });
                    _this.showLoadingGif = false;
                }, function (error) {
                    _this.showLoadingGif = false;
                    _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                });
            }
        }
    };
    VehicleSearchComponent.prototype.loadVehicleToBodyStyleSystemOnSystemSelect = function () {
        var _this = this;
        if (this.vehicles.filter(function (x) { return x.isSelected; }).length) {
            var selectedTemp = this.vehicles.filter(function (x) { return x.isSelected; });
            this.vehicles.filter(function (x) { return x.isSelected; }).forEach(function (vehicle) {
                if (vehicle.vehicleToBodyStyleConfigs) {
                    _this.vehicleToBodyStyleConfigs = _this.vehicleToBodyStyleConfigs.concat(vehicle.vehicleToBodyStyleConfigs);
                    selectedTemp = selectedTemp.filter(function (x) { return x.id !== vehicle.id; });
                }
            });
            if (selectedTemp.length > 0) {
                this.showLoadingGif = true;
                this.vehicleToBodyStyleConfigService.searchByVehicleIds(selectedTemp.map(function (item) { return item.id; })).subscribe(function (m) {
                    _this.vehicles.forEach(function (v) {
                        v.vehicleToBodyStyleConfigs = m.filter(function (item) { return item.vehicle.id === v.id; });
                        _this.vehicleToBodyStyleConfigs = _this.vehicleToBodyStyleConfigs.concat(v.vehicleToBodyStyleConfigs);
                    });
                    _this.showLoadingGif = false;
                }, function (error) {
                    _this.showLoadingGif = false;
                    _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                });
            }
        }
    };
    VehicleSearchComponent.prototype.loadVehicleToDriveTypeSystemOnSystemSelect = function () {
        var _this = this;
        if (this.vehicles.filter(function (x) { return x.isSelected; }).length) {
            var selectedTemp = this.vehicles.filter(function (x) { return x.isSelected; });
            this.vehicleToDriveTypes = [];
            this.vehicles.filter(function (x) { return x.isSelected; }).forEach(function (vehicle) {
                if (vehicle.vehicleToDriveTypes) {
                    _this.vehicleToDriveTypes = _this.vehicleToDriveTypes.concat(vehicle.vehicleToDriveTypes);
                    selectedTemp = selectedTemp.filter(function (x) { return x.id !== vehicle.id; });
                }
            });
            if (selectedTemp.length > 0) {
                this.showLoadingGif = true;
                this.vehicleToDriveTypeService.searchByVehicleIds(selectedTemp.map(function (item) { return item.id; })).subscribe(function (m) {
                    _this.vehicles.forEach(function (v) {
                        v.vehicleToDriveTypes = m.filter(function (item) { return item.vehicle.id === v.id; });
                        _this.vehicleToDriveTypes = _this.vehicleToDriveTypes.concat(v.vehicleToDriveTypes);
                    });
                    _this.showLoadingGif = false;
                }, function (error) {
                    _this.showLoadingGif = false;
                    _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                });
            }
        }
    };
    VehicleSearchComponent.prototype.loadVehicleToWheelSystemOnSystemSelect = function () {
        var _this = this;
        if (this.vehicles.filter(function (x) { return x.isSelected; }).length) {
            var selectedTemp = this.vehicles.filter(function (x) { return x.isSelected; });
            this.vehicles.filter(function (x) { return x.isSelected; }).forEach(function (vehicle) {
                if (vehicle.vehicleToWheelBases) {
                    _this.vehicleToWheelBases = _this.vehicleToWheelBases.concat(vehicle.vehicleToWheelBases);
                    selectedTemp = selectedTemp.filter(function (x) { return x.id !== vehicle.id; });
                }
            });
            if (selectedTemp.length > 0) {
                this.showLoadingGif = true;
                this.vehicleToWheelBaseService.searchByVehicleIds(selectedTemp.map(function (item) { return item.id; })).subscribe(function (m) {
                    _this.vehicles.forEach(function (v) {
                        v.vehicleToWheelBases = m.filter(function (item) { return item.vehicle.id === v.id; });
                        _this.vehicleToWheelBases = _this.vehicleToWheelBases.concat(v.vehicleToWheelBases);
                    });
                    _this.showLoadingGif = false;
                }, function (error) {
                    _this.showLoadingGif = false;
                    _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                });
            }
        }
    };
    VehicleSearchComponent.prototype.loadVehicleToMFRSystemOnSystemSelect = function () {
        var _this = this;
        if (this.vehicles.filter(function (x) { return x.isSelected; }).length) {
            var selectedTemp = this.vehicles.filter(function (x) { return x.isSelected; });
            this.vehicles.filter(function (x) { return x.isSelected; }).forEach(function (vehicle) {
                if (vehicle.vehicleToMfrBodyCodes) {
                    _this.vehicleToMfrBodyCodes = _this.vehicleToMfrBodyCodes.concat(vehicle.vehicleToMfrBodyCodes);
                    selectedTemp = selectedTemp.filter(function (x) { return x.id !== vehicle.id; });
                }
            });
            if (selectedTemp.length > 0) {
                this.showLoadingGif = true;
                this.vehicleToMfrBodyCodeService.searchByVehicleIds(selectedTemp.map(function (item) { return item.id; })).subscribe(function (m) {
                    _this.vehicles.forEach(function (v) {
                        v.vehicleToMfrBodyCodes = m.filter(function (item) { return item.vehicle.id === v.id; });
                        _this.vehicleToMfrBodyCodes = _this.vehicleToMfrBodyCodes.concat(v.vehicleToMfrBodyCodes);
                    });
                    _this.showLoadingGif = false;
                }, function (error) {
                    _this.showLoadingGif = false;
                    _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                });
            }
        }
    };
    VehicleSearchComponent.prototype.iconClassReturn = function () {
        var returnclass = "";
        if (this.selectedSystem === vehicle_search_model_1.ConfigurationSystems.Brake)
            returnclass = "brake-system";
        else if (this.selectedSystem === vehicle_search_model_1.ConfigurationSystems.Engine)
            returnclass = "engine-system";
        else if (this.selectedSystem === vehicle_search_model_1.ConfigurationSystems.Wheel)
            returnclass = "wheel-system";
        return returnclass;
    };
    VehicleSearchComponent.prototype.routerLinkRedirect = function (route, id) {
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    };
    VehicleSearchComponent.prototype.onViewBaseVehicleChangeRequest = function (baseVehicleVm) {
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/basevehicle/" + baseVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    VehicleSearchComponent.prototype.onViewVehicleCr = function (vehicleVm) {
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicle/" + vehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    VehicleSearchComponent.prototype.onViewAssociatedVehiclesCr = function (associatedVehicleVm) {
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        switch (this.selectedSystem) {
            case vehicle_search_model_1.ConfigurationSystems.Brake:
                var changeRequestLink = "/change/review/vehicletobrakeconfig/" + associatedVehicleVm.changeRequestId;
                break;
            case vehicle_search_model_1.ConfigurationSystems.Bed:
                var changeRequestLink = "/change/review/vehicletobedconfig/" + associatedVehicleVm.changeRequestId;
                break;
            case vehicle_search_model_1.ConfigurationSystems.Body:
                var changeRequestLink = "/change/review/vehicletobodystyleconfig/" + associatedVehicleVm.changeRequestId;
                break;
            case vehicle_search_model_1.ConfigurationSystems.Drive:
                var changeRequestLink = "/change/review/vehicletodrivetype/" + associatedVehicleVm.changeRequestId;
                break;
            case vehicle_search_model_1.ConfigurationSystems.MFR:
                var changeRequestLink = "/change/review/vehicletomfrbodycode/" + associatedVehicleVm.changeRequestId;
                break;
            default:
                break;
        }
        this.router.navigateByUrl(changeRequestLink);
    };
    VehicleSearchComponent.prototype.clearFacet = function (facet, refresh) {
        if (refresh === void 0) { refresh = true; }
        if (facet) {
            facet.forEach(function (item) { return item.isSelected = false; });
        }
        if (refresh) {
            this.refreshFacets();
        }
    };
    VehicleSearchComponent.prototype.onSystemAssociationClick = function () {
        switch (this.selectedSystem) {
            case vehicle_search_model_1.ConfigurationSystems.Select:
                this.toastr.warning("Please select an assoication to add.", "No association selected!!");
                break;
            case vehicle_search_model_1.ConfigurationSystems.Brake:
                this.onAddVehicleToBrakeConfigs();
                break;
            case vehicle_search_model_1.ConfigurationSystems.Bed:
                this.onAddVehicleToBedConfigs();
                break;
            case vehicle_search_model_1.ConfigurationSystems.Body:
                this.onAddVehicleToBodyStyleConfigs();
                break;
            case vehicle_search_model_1.ConfigurationSystems.Drive:
                this.onAddVehicleToDriveTypes();
                break;
            case vehicle_search_model_1.ConfigurationSystems.MFR:
                this.onAddVehicleToMfrBodyCodes();
                break;
            default:
                this.toastr.warning("", "No Method for this Vehicle to System Association");
                break;
        }
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleSearchComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("baseVehicleGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleSearchComponent.prototype, "baseVehicleGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleSearchComponent.prototype, "vehicleGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleToBrakeConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleSearchComponent.prototype, "vehicleToBrakeConfigGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleToBedConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleSearchComponent.prototype, "vehicleToBedConfigGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleToBodyStyleConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleSearchComponent.prototype, "vehicleToBodyStyleConfigGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleToDriveTypeGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleSearchComponent.prototype, "vehicleToDriveTypeGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleToWheelBaseGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleSearchComponent.prototype, "vehicleToWheelBaseGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleToMfrBodyCodeGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleSearchComponent.prototype, "vehicleToMfrBodyCodeGrid", void 0);
    __decorate([
        core_1.ViewChild('deleteBrakeAssociationPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleSearchComponent.prototype, "deleteBrakeAssociationPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteBedAssociationPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleSearchComponent.prototype, "deleteBedAssociationPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteBodyStyleAssociationPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleSearchComponent.prototype, "deleteBodyStyleAssociationPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteDriveTypeAssociationPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleSearchComponent.prototype, "deleteDriveTypeAssociationPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteWheelBaseAssociationPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleSearchComponent.prototype, "deleteWheelBaseAssociationPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteMfrBodyCodeAssociationPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleSearchComponent.prototype, "deleteMfrBodyCodeAssociationPopup", void 0);
    VehicleSearchComponent = __decorate([
        core_1.Component({
            selector: 'vehicleSearch-comp',
            templateUrl: 'app/templates/vehicle/vehicle-search.component.html',
            providers: [vehicleToBrakeConfig_service_1.VehicleToBrakeConfigService, vehicleToBedConfig_service_1.VehicleToBedConfigService, vehicleToWheelBase_service_1.VehicleToWheelBaseService, vehicleToBodyStyleConfig_service_1.VehicleToBodyStyleConfigService, vehicleToDriveType_service_1.VehicleToDriveTypeService, vehicleToMfrBodyCode_service_1.VehicleToMfrBodyCodeService, vehicle_search_service_1.VehicleSearchService, ac_fileuploader_1.AcFileUploader]
        }), 
        __metadata('design:paramtypes', [vehicleToBrakeConfig_service_1.VehicleToBrakeConfigService, vehicle_search_service_1.VehicleSearchService, shared_service_1.SharedService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService, vehicleToBedConfig_service_1.VehicleToBedConfigService, vehicleToBodyStyleConfig_service_1.VehicleToBodyStyleConfigService, vehicleToDriveType_service_1.VehicleToDriveTypeService, vehicleToWheelBase_service_1.VehicleToWheelBaseService, vehicleToMfrBodyCode_service_1.VehicleToMfrBodyCodeService])
    ], VehicleSearchComponent);
    return VehicleSearchComponent;
}());
exports.VehicleSearchComponent = VehicleSearchComponent;
