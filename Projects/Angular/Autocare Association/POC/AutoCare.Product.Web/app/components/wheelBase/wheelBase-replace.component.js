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
var wheelBase_service_1 = require("./wheelBase.service");
var constants_warehouse_1 = require("../constants-warehouse");
var vehicleToWheelBase_service_1 = require("../vehicleToWheelBase/vehicleToWheelBase.service");
var ac_grid_1 = require('../../lib/aclibs/ac-grid/ac-grid');
var WheelBaseReplaceComponent = (function () {
    function WheelBaseReplaceComponent(wheelBaseService, router, route, toastr, vehicleToWheelBaseService) {
        this.wheelBaseService = wheelBaseService;
        this.router = router;
        this.route = route;
        this.toastr = toastr;
        this.vehicleToWheelBaseService = vehicleToWheelBaseService;
        this.showLoadingGif = false;
        this.vehicleTypeGroupFacet = [];
        this.thresholdRecordCount = 100000; //NOTE: keep this number large so that "select all" checkbox always appears
        this.replacementWheelBase = {
            id: -1,
        };
    }
    WheelBaseReplaceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.isSelectAllVehicleToWheelBase = false;
        // Load existing wheel base with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        this.wheelBaseService.getWheelBaseDetail(id).subscribe(function (result) {
            _this.existingWheelBase = result;
            _this.wheelBaseService.getAllWheelBase().subscribe(function (m) {
                _this.wheelBases = m;
                _this.refreshFacets();
            });
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
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
            result: { wheelBases: [], vehicleToWheelBases: [] }
        };
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        // Drawer right start
        var headerht = $('header').innerHeight();
        var navht = $('nav').innerHeight();
        var winht = $(window).height();
        var winwt = 800;
        $(".drawer-left").css('min-height', winht - headerht - navht);
        $(".drawer-left").css('width', 15); //start is collapsed state
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
        // Drawer right end
    };
    // validation
    WheelBaseReplaceComponent.prototype.validatereplacementWheelBase = function () {
        var isValid = true;
        // check required fields
        if (Number(this.replacementWheelBase.id) < 1) {
            this.toastr.warning("Please select replacement Wheel Base.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingWheelBase.id) === Number(this.replacementWheelBase.id)) {
            this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingWheelBase.vehicleToWheelBases != undefined) {
            if (this.existingWheelBase.vehicleToWheelBases.filter(function (item) { return item.isSelected; }).length <= 0) {
                this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                isValid = false;
            }
        }
        else if (this.existingWheelBase.vehicleToWheelBases == undefined) {
            this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    WheelBaseReplaceComponent.prototype.onWheelBaseIdKeyPress = function (event) {
        if (Number(event.keyCode) === 13) {
            this.onWheelBaseIdSearch();
        }
    };
    WheelBaseReplaceComponent.prototype.onWheelBaseIdSearch = function () {
        var _this = this;
        var wheelBaseId = Number(this.wheelBaseIdSearchText);
        if (isNaN(wheelBaseId) || wheelBaseId <= 0) {
            this.toastr.warning("Invalid Wheel Base Id.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.replacementWheelBase.id = -1;
        this.showLoadingGif = true;
        this.wheelBaseService.getWheelBaseDetail(Number(this.wheelBaseIdSearchText)).subscribe(function (result) {
            _this.replacementWheelBase = result;
            _this.showLoadingGif = false;
        }, function (error) {
            var errorMessage = JSON.parse(String(error)).message;
            _this.toastr.warning(errorMessage, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    WheelBaseReplaceComponent.prototype.onSelectAllVehicleToWheelBase = function (isSelected) {
        this.isSelectAllVehicleToWheelBase = isSelected;
        if (this.existingWheelBase.vehicleToWheelBases == null) {
            return;
        }
        this.existingWheelBase.vehicleToWheelBases.forEach(function (item) { return item.isSelected = isSelected; });
    };
    WheelBaseReplaceComponent.prototype.onClearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.refreshFacets();
    };
    WheelBaseReplaceComponent.prototype.getDefaultInputModel = function () {
        return {
            wheelBaseId: 0,
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: []
        };
    };
    WheelBaseReplaceComponent.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
        inputModel.wheelBaseId = this.existingWheelBase.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToWheelBaseSearchViewModel.facets.regions) {
            this.vehicleToWheelBaseSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.makes) {
            this.vehicleToWheelBaseSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.models) {
            this.vehicleToWheelBaseSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.subModels) {
            this.vehicleToWheelBaseSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.subModels.push(m.name); });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes) {
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        this.showLoadingGif = true;
        this.vehicleToWheelBaseService.refreshFacets(inputModel).subscribe(function (data) {
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
    WheelBaseReplaceComponent.prototype.filterMakes = function ($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleToWheelBaseSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    WheelBaseReplaceComponent.prototype.filterModels = function ($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleToWheelBaseSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    WheelBaseReplaceComponent.prototype.filterSubModels = function ($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleToWheelBaseSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    WheelBaseReplaceComponent.prototype.filterRegions = function ($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleToWheelBaseSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    WheelBaseReplaceComponent.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    WheelBaseReplaceComponent.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    WheelBaseReplaceComponent.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    WheelBaseReplaceComponent.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    WheelBaseReplaceComponent.prototype.updateRegionFacet = function (regions) {
        var existingSelectedRegions = this.vehicleToWheelBaseSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToWheelBaseSearchViewModel.facets.regions = [];
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
            this.vehicleToWheelBaseSearchViewModel.facets.regions.push(newItem);
        }
        this.regionFacet = this.vehicleToWheelBaseSearchViewModel.facets.regions.slice();
    };
    WheelBaseReplaceComponent.prototype.updateYearFacet = function (years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    WheelBaseReplaceComponent.prototype.updateMakeFacet = function (makes) {
        var existingSelectedMakes = this.vehicleToWheelBaseSearchViewModel.facets.makes.filter(function (make) { return make.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToWheelBaseSearchViewModel.facets.makes = [];
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
            this.vehicleToWheelBaseSearchViewModel.facets.makes.push(newMake);
        }
        this.makeFacet = this.vehicleToWheelBaseSearchViewModel.facets.makes.slice();
    };
    //TODO: makeName is not used
    WheelBaseReplaceComponent.prototype.updateModelFacet = function (models, makeName) {
        var existingSelectedModels = this.vehicleToWheelBaseSearchViewModel.facets.models.filter(function (model) { return model.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToWheelBaseSearchViewModel.facets.models = [];
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
            this.vehicleToWheelBaseSearchViewModel.facets.models.push(newModel);
        }
        this.modelFacet = this.vehicleToWheelBaseSearchViewModel.facets.models.slice();
    };
    WheelBaseReplaceComponent.prototype.updateSubModelFacet = function (subModels, modelName) {
        var existingSelectedSubModels = this.vehicleToWheelBaseSearchViewModel.facets.subModels.filter(function (submodel) { return submodel.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToWheelBaseSearchViewModel.facets.subModels = [];
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
            this.vehicleToWheelBaseSearchViewModel.facets.subModels.push(newSubModel);
        }
        this.subModelFacet = this.vehicleToWheelBaseSearchViewModel.facets.subModels.slice();
    };
    WheelBaseReplaceComponent.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
        var existingSelectedItems = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups = [];
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
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }
        this.vehicleTypeGroupFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.slice();
    };
    WheelBaseReplaceComponent.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
        var existingSelectedItems = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes = [];
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
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.push(newItem);
        }
        this.vehicleTypeFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.slice();
    };
    WheelBaseReplaceComponent.prototype.searchVehicleToWheelBases = function () {
        var _this = this;
        this.showLoadingGif = true;
        var inputModel = this.getDefaultInputModel();
        inputModel.wheelBaseId = this.existingWheelBase.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        //NOTE: inputModel.wheelBaseId = this.existingWheelBase.id; should be sufficient here
        if (this.vehicleToWheelBaseSearchViewModel.facets.makes) {
            this.vehicleToWheelBaseSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.models) {
            this.vehicleToWheelBaseSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.subModels) {
            this.vehicleToWheelBaseSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (s) { return inputModel.subModels.push(s.name); });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes) {
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.regions) {
            this.vehicleToWheelBaseSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        this.vehicleToWheelBaseService.getAssociations(inputModel).subscribe(function (result) {
            if (result.length > 0) {
                _this.existingWheelBase.vehicleToWheelBases = result;
                _this.existingWheelBase.vehicleToWheelBaseCount = result.length;
                _this.isSelectAllVehicleToWheelBase = false;
                if (_this.vehicleToWheelBaseGrid)
                    _this.vehicleToWheelBaseGrid.refresh();
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
    // event on view affected vehicle associations
    WheelBaseReplaceComponent.prototype.onViewAffectedAssociations = function () {
        $(".drawer-left").css('width', 800); //show the filter panel to enable search
    };
    WheelBaseReplaceComponent.prototype.clearAllFacets = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        if (this.vehicleToWheelBaseSearchViewModel.facets.regions) {
            this.vehicleToWheelBaseSearchViewModel.facets.regions.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.makes) {
            this.vehicleToWheelBaseSearchViewModel.facets.makes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.models) {
            this.vehicleToWheelBaseSearchViewModel.facets.models.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.subModels) {
            this.vehicleToWheelBaseSearchViewModel.facets.subModels.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes) {
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.forEach(function (item) { return item.isSelected = false; });
        }
        this.refreshFacets();
    };
    WheelBaseReplaceComponent.prototype.clearFacet = function (facet, refresh) {
        if (refresh === void 0) { refresh = true; }
        if (facet) {
            facet.forEach(function (item) { return item.isSelected = false; });
        }
        if (refresh) {
            this.refreshFacets();
        }
    };
    WheelBaseReplaceComponent.prototype.onVehicleToWheelBaseSelected = function (vechicleToWheelBase) {
        if (vechicleToWheelBase.isSelected) {
            //unchecked
            this.isSelectAllVehicleToWheelBase = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingWheelBase.vehicleToWheelBases.filter(function (item) { return item.id != vechicleToWheelBase.id; });
            if (excludedVehicle.every(function (item) { return item.isSelected; })) {
                this.isSelectAllVehicleToWheelBase = true;
            }
        }
    };
    // event on continue
    WheelBaseReplaceComponent.prototype.onContinue = function () {
        var _this = this;
        // validate
        if (this.validatereplacementWheelBase()) {
            // set data in factory/ service
            this.wheelBaseService.existingWheelBase = this.existingWheelBase;
            this.wheelBaseService.existingWheelBase.vehicleToWheelBases = this.existingWheelBase.vehicleToWheelBases.filter(function (item) { return item.isSelected; });
            this.replacementWheelBase.base = this.wheelBases.filter(function (item) { return item.id === Number(_this.replacementWheelBase.id); })[0].base;
            this.replacementWheelBase.wheelBaseMetric = this.wheelBases.filter(function (item) { return item.id === Number(_this.replacementWheelBase.id); })[0].wheelBaseMetric;
            this.wheelBaseService.replacementWheelBase = this.replacementWheelBase;
            // redirect to Confirm page.
            this.router.navigateByUrl("/wheelbase/replace/confirm/" + this.existingWheelBase.id);
        }
        else {
        }
    };
    __decorate([
        //NOTE: keep this number large so that "select all" checkbox always appears
        core_1.ViewChild("vehicleToWheelBaseGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], WheelBaseReplaceComponent.prototype, "vehicleToWheelBaseGrid", void 0);
    WheelBaseReplaceComponent = __decorate([
        core_1.Component({
            selector: "wheelBase-replace-component",
            templateUrl: "app/templates/wheelBase/wheelBase-replace.component.html",
            providers: [vehicleToWheelBase_service_1.VehicleToWheelBaseService],
        }), 
        __metadata('design:paramtypes', [wheelBase_service_1.WheelBaseService, router_1.Router, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager, vehicleToWheelBase_service_1.VehicleToWheelBaseService])
    ], WheelBaseReplaceComponent);
    return WheelBaseReplaceComponent;
}());
exports.WheelBaseReplaceComponent = WheelBaseReplaceComponent;
