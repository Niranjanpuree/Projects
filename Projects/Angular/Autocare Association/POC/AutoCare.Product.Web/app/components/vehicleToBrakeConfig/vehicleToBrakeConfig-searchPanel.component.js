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
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var shared_service_1 = require("../shared/shared.service");
var constants_warehouse_1 = require("../constants-warehouse");
var vehicleToBrakeConfig_search_model_1 = require("../vehicleToBrakeConfig/vehicleToBrakeConfig-search.model");
var vehicleToBrakeConfig_service_1 = require("../vehicleToBrakeConfig/vehicleToBrakeConfig.service");
var VehicleToBrakeConfigSearchPanel = (function () {
    function VehicleToBrakeConfigSearchPanel(sharedService, toastr, vehicleToBrakeConfigService) {
        this.sharedService = sharedService;
        this.toastr = toastr;
        this.vehicleToBrakeConfigService = vehicleToBrakeConfigService;
        this.vehicleTypeGroupFacet = [];
        this.vehicleToBrakeConfigsRetrieved = [];
        this.showLoadingGif = false;
        this.onSearchEvent = new core_1.EventEmitter();
    }
    VehicleToBrakeConfigSearchPanel.prototype.ngOnInit = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.brakeConfigId = "";
        if (this.sharedService.vehicleToBrakeConfigSearchViewModel != null) {
            this.vehicleToBrakeConfigSearchViewModel.facets = this.sharedService.vehicleToBrakeConfigSearchViewModel.facets;
            this.frontBrakeTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.slice();
            this.rearBrakeTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.slice();
            this.brakeAbsFacet = this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.slice();
            this.brakeSystemFacet = this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.slice();
            this.regionFacet = this.vehicleToBrakeConfigSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleToBrakeConfigSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleToBrakeConfigSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.slice();
            if (this.sharedService.vehicleToBrakeConfigSearchViewModel.searchType == vehicleToBrakeConfig_search_model_1.SearchType.SearchByBrakeConfigId) {
                this.searchByBrakeConfigId();
            }
            else if (this.sharedService.vehicleToBrakeConfigSearchViewModel.searchType == vehicleToBrakeConfig_search_model_1.SearchType.GeneralSearch) {
                this.search();
            }
            else {
                this.showLoadingGif = false;
            }
        }
        else {
            this.refreshFacets();
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.onClearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.brakeConfigId = "";
        if (this.vehicleToBrakeConfigSearchViewModel.facets.regions) {
            this.vehicleToBrakeConfigSearchViewModel.facets.regions.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.makes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.makes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.models) {
            this.vehicleToBrakeConfigSearchViewModel.facets.models.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.subModels) {
            this.vehicleToBrakeConfigSearchViewModel.facets.subModels.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs) {
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems) {
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.forEach(function (item) { return item.isSelected = false; });
        }
        this.refreshFacets();
    };
    VehicleToBrakeConfigSearchPanel.prototype.getDefaultInputModel = function () {
        return {
            startYear: "0",
            endYear: "0",
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
        };
    };
    VehicleToBrakeConfigSearchPanel.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToBrakeConfigSearchViewModel.facets.regions) {
            this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.makes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.models) {
            this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.subModels) {
            this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.subModels.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.frontBrakeTypes.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.rearBrakeTypes.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs) {
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.brakeAbs.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems) {
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.brakeSystems.push(m.name); });
        }
        this.showLoadingGif = true;
        this.vehicleToBrakeConfigService.refreshFacets(inputModel).subscribe(function (data) {
            _this.updateRegionFacet(data.facets.regions);
            _this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
            _this.updateVehicleTypeFacet(data.facets.vehicleTypes);
            _this.updateYearFacet(data.facets.years);
            _this.updateMakeFacet(data.facets.makes);
            _this.updateModelFacet(data.facets.models, "");
            _this.updateSubModelFacet(data.facets.subModels, "");
            _this.updateFrontBrakeTypeFacet(data.facets.frontBrakeTypes);
            _this.updateRearBrakeTypeFacet(data.facets.rearBrakeTypes);
            _this.updateBrakeAbsFacet(data.facets.brakeAbs);
            _this.updateBrakeSystemFacet(data.facets.brakeSystems);
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToBrakeConfigSearchPanel.prototype.filterMakes = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.filterModels = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.filterSubModels = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.filterRegions = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.filterFrontBrakeTypes = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.filterRearBrakeTypes = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.filterBrakeAbs = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.filterBrakeSystems = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    VehicleToBrakeConfigSearchPanel.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    VehicleToBrakeConfigSearchPanel.prototype.updateRegionFacet = function (regions) {
        var existingSelectedRegions = this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBrakeConfigSearchViewModel.facets.regions = [];
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
            this.vehicleToBrakeConfigSearchViewModel.facets.regions.push(newItem);
        }
        this.regionFacet = this.vehicleToBrakeConfigSearchViewModel.facets.regions.slice();
    };
    VehicleToBrakeConfigSearchPanel.prototype.updateYearFacet = function (years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    VehicleToBrakeConfigSearchPanel.prototype.updateMakeFacet = function (makes) {
        var existingSelectedMakes = this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(function (make) { return make.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBrakeConfigSearchViewModel.facets.makes = [];
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
            this.vehicleToBrakeConfigSearchViewModel.facets.makes.push(newMake);
        }
        this.makeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.makes.slice();
    };
    //TODO: makeName is not used
    VehicleToBrakeConfigSearchPanel.prototype.updateModelFacet = function (models, makeName) {
        var existingSelectedModels = this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(function (model) { return model.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToBrakeConfigSearchViewModel.facets.models = [];
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
            this.vehicleToBrakeConfigSearchViewModel.facets.models.push(newModel);
        }
        this.modelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.models.slice();
    };
    VehicleToBrakeConfigSearchPanel.prototype.updateSubModelFacet = function (subModels, modelName) {
        var existingSelectedSubModels = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(function (submodel) { return submodel.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToBrakeConfigSearchViewModel.facets.subModels = [];
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
            this.vehicleToBrakeConfigSearchViewModel.facets.subModels.push(newSubModel);
        }
        this.subModelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.slice();
    };
    VehicleToBrakeConfigSearchPanel.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
        var existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups = [];
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
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }
        this.vehicleTypeGroupFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.slice();
    };
    VehicleToBrakeConfigSearchPanel.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
        var existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes = [];
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
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.push(newItem);
        }
        this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.slice();
    };
    VehicleToBrakeConfigSearchPanel.prototype.updateFrontBrakeTypeFacet = function (frontBrakeTypes) {
        var existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes = [];
        for (var _i = 0, frontBrakeTypes_1 = frontBrakeTypes; _i < frontBrakeTypes_1.length; _i++) {
            var item = frontBrakeTypes_1[_i];
            var newItem = {
                name: item,
                isSelected: false
            };
            for (var _a = 0, existingSelectedItems_3 = existingSelectedItems; _a < existingSelectedItems_3.length; _a++) {
                var existingSelectedItem = existingSelectedItems_3[_a];
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }
            this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.push(newItem);
        }
        this.frontBrakeTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.slice();
    };
    VehicleToBrakeConfigSearchPanel.prototype.updateRearBrakeTypeFacet = function (rearBrakeTypes) {
        var existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes = [];
        for (var _i = 0, rearBrakeTypes_1 = rearBrakeTypes; _i < rearBrakeTypes_1.length; _i++) {
            var item = rearBrakeTypes_1[_i];
            var newItem = {
                name: item,
                isSelected: false
            };
            for (var _a = 0, existingSelectedItems_4 = existingSelectedItems; _a < existingSelectedItems_4.length; _a++) {
                var existingSelectedItem = existingSelectedItems_4[_a];
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }
            this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.push(newItem);
        }
        this.rearBrakeTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.slice();
    };
    VehicleToBrakeConfigSearchPanel.prototype.updateBrakeAbsFacet = function (brakeAbs) {
        var existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs = [];
        for (var _i = 0, brakeAbs_1 = brakeAbs; _i < brakeAbs_1.length; _i++) {
            var item = brakeAbs_1[_i];
            var newItem = {
                name: item,
                isSelected: false
            };
            for (var _a = 0, existingSelectedItems_5 = existingSelectedItems; _a < existingSelectedItems_5.length; _a++) {
                var existingSelectedItem = existingSelectedItems_5[_a];
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.push(newItem);
        }
        this.brakeAbsFacet = this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.slice();
    };
    VehicleToBrakeConfigSearchPanel.prototype.updateBrakeSystemFacet = function (brakeSystems) {
        var existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems = [];
        for (var _i = 0, brakeSystems_1 = brakeSystems; _i < brakeSystems_1.length; _i++) {
            var item = brakeSystems_1[_i];
            var newItem = {
                name: item,
                isSelected: false
            };
            for (var _a = 0, existingSelectedItems_6 = existingSelectedItems; _a < existingSelectedItems_6.length; _a++) {
                var existingSelectedItem = existingSelectedItems_6[_a];
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.push(newItem);
        }
        this.brakeSystemFacet = this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.slice();
    };
    VehicleToBrakeConfigSearchPanel.prototype.searchByBrakeConfigId = function () {
        var _this = this;
        var brakeConfigId = Number(this.brakeConfigId);
        if (isNaN(brakeConfigId)) {
            this.toastr.warning("Invalid Break Config Id", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.vehicleToBrakeConfigSearchViewModel.searchType = vehicleToBrakeConfig_search_model_1.SearchType.SearchByBrakeConfigId;
        this.showLoadingGif = true;
        this.vehicleToBrakeConfigService.searchByBrakeConfigId(brakeConfigId).subscribe(function (m) {
            if (m.result.brakeConfigs.length > 0) {
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
    VehicleToBrakeConfigSearchPanel.prototype.onBrakeConfigKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.searchByBrakeConfigId();
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.getSearchResult = function (m) {
        var _this = this;
        this.vehicleToBrakeConfigSearchViewModel.result = m.result;
        this.vehicleToBrakeConfigSearchViewModel.totalCount = m.totalCount;
        this.vehicleToBrakeConfigsForSelectedBrake = [];
        this.isSelectAllBrakeSystems = false;
        // note: select all when totalRecords <= threshold
        if (this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.length <= this.thresholdRecordCount) {
            this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.forEach(function (item) {
                item.isSelected = true;
                _this.refreshAssociationWithBrakeConfigId(item.id, item.isSelected);
            });
            this.isSelectAllBrakeSystems = true;
        }
        // callback emitter
        this.onSearchEvent.emit(this.vehicleToBrakeConfigsForSelectedBrake);
    };
    // todo: make is modular, this method is at two location
    VehicleToBrakeConfigSearchPanel.prototype.refreshAssociationWithBrakeConfigId = function (brakeConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToBrakeConfigsRetrieved = this.getVehicleToBrakeConfigsByBrakeConfigId(brakeConfigId);
            //TODO: number of associations which may be useful in add brake association screen?
            var temp = this.vehicleToBrakeConfigsForSelectedBrake || [];
            for (var _i = 0, _a = this.vehicleToBrakeConfigsRetrieved; _i < _a.length; _i++) {
                var vehicleToBrakeConfig = _a[_i];
                temp.push(vehicleToBrakeConfig);
            }
            this.vehicleToBrakeConfigsForSelectedBrake = temp;
        }
        else {
            var m = this.vehicleToBrakeConfigsForSelectedBrake.filter(function (x) { return x.brakeConfig.id != brakeConfigId; });
            this.vehicleToBrakeConfigsForSelectedBrake = m;
        }
    };
    VehicleToBrakeConfigSearchPanel.prototype.getVehicleToBrakeConfigsByBrakeConfigId = function (id) {
        return this.vehicleToBrakeConfigSearchViewModel.result.vehicleToBrakeConfigs.filter(function (v) { return v.brakeConfig.id == id; });
    };
    VehicleToBrakeConfigSearchPanel.prototype.search = function () {
        var _this = this;
        this.vehicleToBrakeConfigSearchViewModel.searchType = vehicleToBrakeConfig_search_model_1.SearchType.GeneralSearch;
        this.showLoadingGif = true;
        var inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.frontBrakeTypes.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.rearBrakeTypes.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs) {
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.brakeAbs.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems) {
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.brakeSystems.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.makes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.models) {
            this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.subModels) {
            this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (s) { return inputModel.subModels.push(s.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.regions) {
            this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        this.vehicleToBrakeConfigService.search(inputModel).subscribe(function (m) {
            if (m.result.brakeConfigs.length > 0 || m.result.vehicleToBrakeConfigs.length > 0) {
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
    VehicleToBrakeConfigSearchPanel.prototype.clearFacet = function (facet, refresh) {
        if (refresh === void 0) { refresh = true; }
        if (facet) {
            facet.forEach(function (item) { return item.isSelected = false; });
        }
        if (refresh) {
            this.refreshFacets();
        }
    };
    __decorate([
        core_1.Input("thresholdRecordCount"), 
        __metadata('design:type', Number)
    ], VehicleToBrakeConfigSearchPanel.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.Input("vehicleToBrakeConfigSearchViewModel"), 
        __metadata('design:type', Object)
    ], VehicleToBrakeConfigSearchPanel.prototype, "vehicleToBrakeConfigSearchViewModel", void 0);
    __decorate([
        core_1.Input("vehicleToBrakeConfigsForSelectedBrake"), 
        __metadata('design:type', Array)
    ], VehicleToBrakeConfigSearchPanel.prototype, "vehicleToBrakeConfigsForSelectedBrake", void 0);
    __decorate([
        core_1.Output("onSearchEvent"), 
        __metadata('design:type', Object)
    ], VehicleToBrakeConfigSearchPanel.prototype, "onSearchEvent", void 0);
    VehicleToBrakeConfigSearchPanel = __decorate([
        core_1.Component({
            selector: "vehicletobrakeconfig-searchpanel",
            templateUrl: "app/templates/vehicleToBrakeConfig/vehicleToBrakeConfig-searchPanel.component.html",
            providers: [vehicleToBrakeConfig_service_1.VehicleToBrakeConfigService, shared_service_1.SharedService],
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, ng2_toastr_1.ToastsManager, vehicleToBrakeConfig_service_1.VehicleToBrakeConfigService])
    ], VehicleToBrakeConfigSearchPanel);
    return VehicleToBrakeConfigSearchPanel;
}());
exports.VehicleToBrakeConfigSearchPanel = VehicleToBrakeConfigSearchPanel;
