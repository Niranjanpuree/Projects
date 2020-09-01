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
var driveType_service_1 = require("./driveType.service");
var vehicleToDriveType_service_1 = require("../vehicleToDriveType/vehicleToDriveType.service");
var ac_grid_1 = require('../../lib/aclibs/ac-grid/ac-grid');
var constants_warehouse_1 = require("../constants-warehouse");
var DriveTypeReplaceComponent = (function () {
    function DriveTypeReplaceComponent(driveTypeService, router, route, toastr, vehicleToDriveTypeService) {
        this.driveTypeService = driveTypeService;
        this.router = router;
        this.route = route;
        this.toastr = toastr;
        this.vehicleToDriveTypeService = vehicleToDriveTypeService;
        this.showLoadingGif = false;
        this.vehicleTypeGroupFacet = [];
        this.driveTypes = [];
        this.thresholdRecordCount = 100000; //NOTE: keep this number large so that "select all" checkbox always appears
        // initialize empty DriveType
        this.replaceDriveType = {
            id: 0,
            name: "",
            isSelected: false
        };
    }
    DriveTypeReplaceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.isSelectAllVehicleToDriveType = false;
        // Load existing DriveType config with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        this.driveTypes = [];
        this.driveTypeService.getDriveType(id).subscribe(function (result) {
            _this.existingDriveType = result;
            _this.driveTypeService.getDriveTypes().subscribe(function (mbc) {
                _this.driveTypes = mbc;
                _this.refreshFacets();
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
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
                driveTypes: [],
            },
            result: { driveTypes: [], vehicleToDriveTypes: [] }
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
    DriveTypeReplaceComponent.prototype.validateReplaceDriveType = function () {
        var isValid = true;
        // check required fields
        if (Number(this.replaceDriveType.id) === -1) {
            this.toastr.warning("Please select Drive Type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceDriveType.id) < 1) {
            this.toastr.warning("Please select replacement Drive Type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingDriveType.id) === Number(this.replaceDriveType.id)) {
            this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingDriveType.vehicleToDriveTypes.filter(function (item) { return item.isSelected; }).length <= 0) {
            this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    // event on model change in selection of Drive Type
    DriveTypeReplaceComponent.prototype.onChangeDriveType = function (driveTypeId) {
        if (driveTypeId > 0)
            this.replaceDriveType.id = driveTypeId;
        else
            this.replaceDriveType.id = 0;
    };
    DriveTypeReplaceComponent.prototype.onDriveTypeConfigIdKeyPress = function (event) {
        if (Number(event.keyCode) === 13) {
            this.onDriveTypeIdSearch();
        }
    };
    // event on DriveType config id search
    DriveTypeReplaceComponent.prototype.onDriveTypeIdSearch = function () {
        var _this = this;
        var driveTypeId = Number(this.driveTypeIdSearchText);
        if (isNaN(driveTypeId) || driveTypeId <= 0) {
            this.toastr.warning("Invalid Drive Type Id.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        // empty replace DriveType config
        this.replaceDriveType = {
            id: 0,
            name: "",
            isSelected: false
        };
        this.driveTypes = null;
        this.showLoadingGif = true;
        this.driveTypeService.getDriveType(Number(this.driveTypeIdSearchText)).subscribe(function (result) {
            _this.replaceDriveType = result;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    DriveTypeReplaceComponent.prototype.onSelectAllVehicleToDriveType = function (isSelected) {
        this.isSelectAllVehicleToDriveType = isSelected;
        if (this.existingDriveType.vehicleToDriveTypes == null) {
            return;
        }
        this.existingDriveType.vehicleToDriveTypes.forEach(function (item) { return item.isSelected = isSelected; });
    };
    DriveTypeReplaceComponent.prototype.onClearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.refreshFacets();
    };
    DriveTypeReplaceComponent.prototype.getDefaultInputModel = function () {
        return {
            driveTypeId: 0,
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
            driveTypes: []
        };
    };
    DriveTypeReplaceComponent.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
        inputModel.driveTypeId = this.existingDriveType.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToDriveTypeSearchViewModel.facets.regions) {
            this.vehicleToDriveTypeSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.makes) {
            this.vehicleToDriveTypeSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.models) {
            this.vehicleToDriveTypeSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.subModels) {
            this.vehicleToDriveTypeSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.subModels.push(m.name); });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes) {
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        this.showLoadingGif = true;
        this.vehicleToDriveTypeService.refreshFacets(inputModel).subscribe(function (data) {
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
    DriveTypeReplaceComponent.prototype.filterMakes = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleToDriveTypeSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    DriveTypeReplaceComponent.prototype.filterModels = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleToDriveTypeSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    DriveTypeReplaceComponent.prototype.filterSubModels = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleToDriveTypeSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    DriveTypeReplaceComponent.prototype.filterRegions = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleToDriveTypeSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    DriveTypeReplaceComponent.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    DriveTypeReplaceComponent.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    DriveTypeReplaceComponent.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    DriveTypeReplaceComponent.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    DriveTypeReplaceComponent.prototype.updateRegionFacet = function (regions) {
        var existingSelectedRegions = this.vehicleToDriveTypeSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToDriveTypeSearchViewModel.facets.regions = [];
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
            this.vehicleToDriveTypeSearchViewModel.facets.regions.push(newItem);
        }
        this.regionFacet = this.vehicleToDriveTypeSearchViewModel.facets.regions.slice();
    };
    DriveTypeReplaceComponent.prototype.updateYearFacet = function (years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    DriveTypeReplaceComponent.prototype.updateMakeFacet = function (makes) {
        var existingSelectedMakes = this.vehicleToDriveTypeSearchViewModel.facets.makes.filter(function (make) { return make.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToDriveTypeSearchViewModel.facets.makes = [];
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
            this.vehicleToDriveTypeSearchViewModel.facets.makes.push(newMake);
        }
        this.makeFacet = this.vehicleToDriveTypeSearchViewModel.facets.makes.slice();
    };
    //TODO: makeName is not used
    DriveTypeReplaceComponent.prototype.updateModelFacet = function (models, makeName) {
        var existingSelectedModels = this.vehicleToDriveTypeSearchViewModel.facets.models.filter(function (model) { return model.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToDriveTypeSearchViewModel.facets.models = [];
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
            this.vehicleToDriveTypeSearchViewModel.facets.models.push(newModel);
        }
        this.modelFacet = this.vehicleToDriveTypeSearchViewModel.facets.models.slice();
    };
    DriveTypeReplaceComponent.prototype.updateSubModelFacet = function (subModels, modelName) {
        var existingSelectedSubModels = this.vehicleToDriveTypeSearchViewModel.facets.subModels.filter(function (submodel) { return submodel.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToDriveTypeSearchViewModel.facets.subModels = [];
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
            this.vehicleToDriveTypeSearchViewModel.facets.subModels.push(newSubModel);
        }
        this.subModelFacet = this.vehicleToDriveTypeSearchViewModel.facets.subModels.slice();
    };
    DriveTypeReplaceComponent.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
        var existingSelectedItems = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups = [];
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
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }
        this.vehicleTypeGroupFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.slice();
    };
    DriveTypeReplaceComponent.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
        var existingSelectedItems = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes = [];
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
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.push(newItem);
        }
        this.vehicleTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.slice();
    };
    DriveTypeReplaceComponent.prototype.searchVehicleToDriveTypes = function () {
        var _this = this;
        this.showLoadingGif = true;
        var inputModel = this.getDefaultInputModel();
        inputModel.driveTypeId = this.existingDriveType.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToDriveTypeSearchViewModel.facets.makes) {
            this.vehicleToDriveTypeSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.models) {
            this.vehicleToDriveTypeSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.subModels) {
            this.vehicleToDriveTypeSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (s) { return inputModel.subModels.push(s.name); });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes) {
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.regions) {
            this.vehicleToDriveTypeSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        this.vehicleToDriveTypeService.getAssociations(inputModel).subscribe(function (result) {
            if (result.length > 0) {
                _this.existingDriveType.vehicleToDriveTypes = result;
                _this.existingDriveType.vehicleToDriveTypeCount = result.length;
                _this.isSelectAllVehicleToDriveType = false;
                if (_this.vehicleToDriveTypeGrid)
                    _this.vehicleToDriveTypeGrid.refresh();
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
    DriveTypeReplaceComponent.prototype.onViewAffectedAssociations = function () {
        $(".drawer-left").css('width', 800); //show the filter panel to enable search
    };
    DriveTypeReplaceComponent.prototype.clearAllFacets = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        if (this.vehicleToDriveTypeSearchViewModel.facets.regions) {
            this.vehicleToDriveTypeSearchViewModel.facets.regions.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.makes) {
            this.vehicleToDriveTypeSearchViewModel.facets.makes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.models) {
            this.vehicleToDriveTypeSearchViewModel.facets.models.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.subModels) {
            this.vehicleToDriveTypeSearchViewModel.facets.subModels.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes) {
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.forEach(function (item) { return item.isSelected = false; });
        }
        this.refreshFacets();
    };
    DriveTypeReplaceComponent.prototype.clearFacet = function (facet, refresh) {
        if (refresh === void 0) { refresh = true; }
        if (facet) {
            facet.forEach(function (item) { return item.isSelected = false; });
        }
        if (refresh) {
            this.refreshFacets();
        }
    };
    DriveTypeReplaceComponent.prototype.onVehicleToDriveTypeSelected = function (vechicleToDriveType) {
        if (vechicleToDriveType.isSelected) {
            //unchecked
            this.isSelectAllVehicleToDriveType = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingDriveType.vehicleToDriveTypes.filter(function (item) { return item.id != vechicleToDriveType.id; });
            if (excludedVehicle.every(function (item) { return item.isSelected; })) {
                this.isSelectAllVehicleToDriveType = true;
            }
        }
    };
    // event on continue
    DriveTypeReplaceComponent.prototype.onContinue = function () {
        var _this = this;
        if (this.validateReplaceDriveType()) {
            this.driveTypeService.existingDriveType = this.existingDriveType;
            this.driveTypeService.existingDriveType.vehicleToDriveTypes = this.existingDriveType.vehicleToDriveTypes.filter(function (item) { return item.isSelected; });
            this.replaceDriveType.name = this.driveTypes.filter(function (item) { return item.id === Number(_this.replaceDriveType.id); })[0].name;
            this.driveTypeService.replacementDriveType = this.replaceDriveType;
            this.router.navigateByUrl("/drivetype/replace/confirm/" + this.existingDriveType.id);
        }
    };
    __decorate([
        //NOTE: keep this number large so that "select all" checkbox always appears
        core_1.ViewChild("vehicleToDriveTypeGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], DriveTypeReplaceComponent.prototype, "vehicleToDriveTypeGrid", void 0);
    DriveTypeReplaceComponent = __decorate([
        core_1.Component({
            selector: "driveType-replace-component",
            templateUrl: "app/templates/driveType/driveType-replace.component.html",
            providers: [vehicleToDriveType_service_1.VehicleToDriveTypeService]
        }), 
        __metadata('design:paramtypes', [driveType_service_1.DriveTypeService, router_1.Router, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager, vehicleToDriveType_service_1.VehicleToDriveTypeService])
    ], DriveTypeReplaceComponent);
    return DriveTypeReplaceComponent;
}());
exports.DriveTypeReplaceComponent = DriveTypeReplaceComponent;
//import { Component, OnInit, ViewChild }           from "@angular/core";
//import { Router, ActivatedRoute } from "@angular/router";
//import { ToastsManager }                          from "../../lib/aclibs/ng2-toastr/ng2-toastr";
//import { IDriveType }                           from "./driveType.model";
//import { DriveTypeService }                           from "./driveType.service";
//import { AcGridComponent }                        from '../../lib/aclibs/ac-grid/ac-grid';     
//import { VehicleToDriveTypeService }            from "../vehicleToDriveType/vehicleToDriveType.service";
//import {IVehicleToDriveType }                   from"../vehicleToDriveType/vehicleToDriveType.model"
//import { ConstantsWarehouse }                     from "../constants-warehouse";
//@Component({
//    selector: "driveType-replace-component",
//    templateUrl: "app/templates/driveType/driveType-replace.component.html",
//    providers: [DriveTypeService, VehicleToDriveTypeService],
//})
//export class DriveTypeReplaceComponent implements OnInit {
//    public existingDriveType: IDriveType;
//    public replaceDriveType: IDriveType;
//    public driveTypeNames: IDriveType[];
//    public driveTypeIdSearchText: string;
//    showLoadingGif: boolean = false;
//    constructor(private driveTypeService: DriveTypeService, private router: Router, private route: ActivatedRoute,
//        private toastr: ToastsManager,
//        private vehicleToDriveTypeService: VehicleToDriveTypeService) {
//        this.replaceDriveType = {
//            id: null,
//            name: "",
//            isSelected: false
//        };
//    }
//    ngOnInit() {
//        debugger;
//        this.showLoadingGif = true;
//        let id = Number(this.route.snapshot.params["id"]);
//        this.driveTypeService.getDriveType(id).subscribe(result => {
//            debugger;
//            this.existingDriveType = result;
//            this.driveTypeService.getDriveTypes().subscribe(bt => {
//                debugger;
//                this.driveTypeNames = bt;
//                //this.refreshFacets();
//            }, error => {
//                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
//                this.showLoadingGif = false;
//            }); // front brake types
//        }, error => {
//            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
//            this.showLoadingGif = false;
//        });
//        this.showLoadingGif = false;
//        // Drawer right start
//        var headerht = $('header').innerHeight();
//        var navht = $('nav').innerHeight();
//        var winht = $(window).height();
//        var winwt = 800;
//        $(".drawer-left").css('min-height', winht - headerht - navht);
//        $(".drawer-left").css('width', 15); //start is collapsed state
//        $(document).on('click', '.drawer-show', function (event) {
//            $(".drawer-left").css('width', winwt);
//        });
//        $(".drawer-left span").on('click', function () {
//            var drawerwt = $(".drawer-left").width();
//            if (drawerwt == 15) {
//                $(".drawer-left").css('width', winwt);
//            }
//            else {
//                $(".drawer-left").css('width', 15);
//            }
//        });
//        $(document).on('click', function (event) {
//            if (!$(event.target).closest(".drawer-left").length) {
//                // Hide the menus.
//                var drawerwt = $(".drawer-left").width();
//                if (drawerwt > 15) {
//                    $(".drawer-left").css('width', 15);
//                }
//            }
//        });
//        // Drawer right end
//    }
//    onChangeDriveTypeName(driveType: IDriveType) {
//        this.replaceDriveType.id = null;
//        this.replaceDriveType = driveType;
//        // get brake config system info
//        //if (driveTypeName !== "") {
//        //    this.driveTypeService.getByChildIds(frontBrakeTypeId, rearBrakeTypeId, brakeABSId, brakeSystemId).subscribe(result => {
//        //        this.replaceDriveType = result;
//        //    }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
//        //}
//    }
//    // event on view affected vehicle associations
//    onViewAffectedAssociations() {
//        $(".drawer-left").css('width', 800); //show the filter panel to enable search
//    }
////import { Component, OnInit, ViewChild }           from "@angular/core";
////import { Router, ROUTER_DIRECTIVES, ActivatedRoute } from "@angular/router";
////import { ToastsManager }                          from "../../lib/aclibs/ng2-toastr/ng2-toastr";
////import { MODAL_DIRECTIVES, ModalComponent }       from "ng2-bs3-modal/ng2-bs3-modal";
////import { IBrakeConfig }                           from "./brakeConfig.model";
////import { DriveTypeService }                       from "../driveType/driveType.service";
////import { IDriveType }                             from "../driveType/driveType.model";
////import { ConstantsWarehouse }                     from "../constants-warehouse";
////import { VehicleToDriveTypeService }            from "../vehicleToDriveType/vehicleToDriveType.service";
////import { LoadingGifComponent } from '../shared/loadingGif.component';
////import { AcGridComponent }                        from '../../lib/aclibs/ac-grid/ac-grid';
////import {IVehicleToDriveType }                   from"../vehicleToDriveType/vehicleToDriveType.model"
////import {
////    IVehicleToDriveTypeSearchInputModel,
////    IVehicleToDriveTypeSearchViewModel,
////    IFacet }                                    from "../vehicleToDriveType/vehicleToDriveType-search.model";
////@Component({
////    selector: "driveType-replace-component",
////    templateUrl: "app/templates/driveType/driveType-replace.component.html",
////    providers: [VehicleToDriveTypeService],
////    directives: [MODAL_DIRECTIVES, ROUTER_DIRECTIVES, LoadingGifComponent, AcGridComponent]
////})
////export class DriveTypeReplaceComponent implements OnInit {
////    public existingDriveType: IDriveType;
////    public replaceDriveType: IDriveType;
////    public driveTypeNames: IDriveType[];
////    public driveTypeIdSearchText: string;
////    showLoadingGif: boolean = false;
////    isSelectAllVehicleToDriveType: boolean;
////    vehicleToDriveTypeSearchViewModel: IVehicleToDriveTypeSearchViewModel;
////    regionFacet: IFacet[];
////    vehicleTypeFacet: IFacet[];
////    vehicleTypeGroupFacet: IFacet[] = [];
////    makeFacet: IFacet[];
////    modelFacet: IFacet[];
////    subModelFacet: IFacet[];
////    startYearFacet: string[];
////    endYearFacet: string[];
////    selectedStartYear: string;
////    selectedEndYear: string;
////    private thresholdRecordCount: number = 100000;  //NOTE: keep this number large so that "select all" checkbox always appears
////    @ViewChild("vehicleToDriveTypeGrid") vehicleToDriveTypeGrid: AcGridComponent;
////    constructor(private driveTypeService: DriveTypeService, private router: Router,
////        private route: ActivatedRoute, private toastr: ToastsManager,
////        private vehicleToDriveTypeService: VehicleToDriveTypeService) {
////        // initialize empty brake config
////        this.replaceDriveType = {
////            id: null,
////            name: "",
////            isSelected: false
////        };
////    }
////    ngOnInit() {
////        debugger;
////        this.showLoadingGif = true;
////        this.isSelectAllVehicleToDriveType = false;
////        // Load existing brake config with reference from RouteParams
////        let id = Number(this.route.snapshot.params["id"]);
////        this.driveTypeService.getDriveType(id).subscribe(result => {
////            debugger;
////            this.existingDriveType = result;
////            // Load select options for replace.
////            this.driveTypeService.getDriveTypes().subscribe(bt => {
////                debugger;
////                this.driveTypeNames = bt;
////                //this.refreshFacets();
////            }, error => {
////                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
////                this.showLoadingGif = false;
////            }); // front brake types
////        }, error => {
////            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
////            this.showLoadingGif = false;
////        });
////        this.vehicleToDriveTypeSearchViewModel = {
////            facets: {
////                startYears: [],
////                endYears: [],
////                regions: [],
////                vehicleTypeGroups: [],
////                vehicleTypes: [],
////                makes: [],
////                models: [],
////                subModels: [],
////                driveTypes: [],
////            }
////            , result: { driveTypes: [], vehicleToDriveTypes: [] }
////        };
////        this.selectedStartYear = "0";
////        this.selectedEndYear = "0";
////        // Drawer right start
////        var headerht = $('header').innerHeight();
////        var navht = $('nav').innerHeight();
////        var winht = $(window).height();
////        var winwt = 800;
////        $(".drawer-left").css('min-height', winht - headerht - navht);
////        $(".drawer-left").css('width', 15); //start is collapsed state
////        $(document).on('click', '.drawer-show', function (event) {
////            $(".drawer-left").css('width', winwt);
////        });
////        $(".drawer-left span").on('click', function () {
////            var drawerwt = $(".drawer-left").width();
////            if (drawerwt == 15) {
////                $(".drawer-left").css('width', winwt);
////            }
////            else {
////                $(".drawer-left").css('width', 15);
////            }
////        });
////        $(document).on('click', function (event) {
////            if (!$(event.target).closest(".drawer-left").length) {
////                // Hide the menus.
////                var drawerwt = $(".drawer-left").width();
////                if (drawerwt > 15) {
////                    $(".drawer-left").css('width', 15);
////                }
////            }
////        });
////        // Drawer right end
////    }
//    // validation
//    //private validateReplaceDriveType(): Boolean {
//    //    let isValid: Boolean = true;
//    //    // check required fields
//    //    if (String(this.replaceDriveType.name) === "") {
//    //        this.toastr.warning("Please select Front brake type.", ConstantsWarehouse.validationTitle);
//    //        isValid = false;
//    //    }
//    //    else if (Number(this.replaceDriveType.id) < 1) {
//    //        this.toastr.warning("Please select replacement Drive Type system.", ConstantsWarehouse.validationTitle);
//    //        isValid = false;
//    //    }
//    //    else if (Number(this.existingDriveType.id) === Number(this.replaceDriveType.id)) {
//    //        this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
//    //        isValid = false;
//    //    }
//    //    else if (this.existingDriveType.vehicleToDriveTypes != undefined) {
//    //        if (this.existingDriveType.vehicleToDriveTypes.filter(item => item.isSelected).length <= 0) {
//    //            this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
//    //            isValid = false;
//    //        }
//    //    }
//    //    else if (this.existingDriveType.vehicleToDriveTypes == undefined) {
//    //        this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
//    //        isValid = false;
//    //    }
//    //    return isValid;
//    //}
//    //// event on model change in selection of Front brake type
//    //onChangeDriveTypeName(driveTypeName: String) {
//    //    this.replaceDriveType.name = "";
//    //    this.driveTypeNames = [];
//    //    this.replaceDriveType.id = null;
//    //    this.driveTypeService.getByDriveTypeName(driveTypeName).subscribe(result => {
//    //        this.replaceDriveType = result;
//    //    }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
//    //}
//    //// event on model change in selection of Rear brake type
//    ////onChangeRearBrakeType(frontBrakeTypeId: Number, rearBrakeTypeId: Number) {
//    ////    this.replaceBrakeConfig.brakeSystemId = -1;
//    ////    this.brakeSystems = [];
//    ////    this.replaceBrakeConfig.brakeABSId = -1;
//    ////    if (this.replaceBrakeConfig.rearBrakeTypeId == -1) {
//    ////        this.brakeABSes = [];
//    ////        return;
//    ////    }
//    ////    this.brakeABSes = null;
//    ////    this.brakeABSService.getByFrontBrakeTypeIdRearBrakeTypeId(frontBrakeTypeId, rearBrakeTypeId).subscribe(babs => {
//    ////        this.brakeABSes = babs;
//    ////    },
//    ////        error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle)); // brake ABSes
//    ////}
//    //// event on model change in selection of Brake Abs
//    ////onChangeBrakeABS(frontBrakeTypeId: Number, rearBrakeTypeId: Number, brakeABSId: Number) {
//    ////    this.replaceBrakeConfig.brakeSystemId = -1;
//    ////    if (this.replaceBrakeConfig.brakeABSId == -1) {
//    ////        this.brakeSystems = [];
//    ////        return;
//    ////    }
//    ////    this.brakeSystems = null;
//    ////    this.brakeSystemService.getByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId(frontBrakeTypeId, rearBrakeTypeId, brakeABSId).subscribe(bs => {
//    ////        this.brakeSystems = bs;
//    ////    },
//    ////        error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle)); // brake ABSes
//    ////}
//    //// event on model change in selection of Brake system
//    ////onChangeBrakeSystem(frontBrakeTypeId: Number, rearBrakeTypeId: Number, brakeABSId: Number, brakeSystemId: Number) {
//    ////    this.replaceBrakeConfig.id = null;
//    ////    // get brake config system info
//    ////    if (frontBrakeTypeId > 0 && rearBrakeTypeId > 0 && brakeABSId > 0 && brakeSystemId > 0) {
//    ////        this.brakeConfigService.getByChildIds(frontBrakeTypeId, rearBrakeTypeId, brakeABSId, brakeSystemId).subscribe(result => {
//    ////            this.replaceBrakeConfig = result;
//    ////        }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
//    ////    }
//    ////}
//    //// event on brake config id quick search
//    //onDriveTypeIdKeyPress(event) {
//    //    if (Number(event.keyCode) === 13) {
//    //        this.onDriveTypeIdSearch();
//    //    }
//    //}
//    //// event on brake config id search
//    //onBrakeConfigIdSearch() {
//    //    let driveTypeId: number = Number(this.driveTypeIdSearchText);
//    //    if (isNaN(driveTypeId) || driveTypeId <= 0) {
//    //        this.toastr.warning("Invalid Drive Type Id.", ConstantsWarehouse.validationTitle);
//    //        return;
//    //    }
//    //    // empty replace brake config
//    //    this.replaceDriveType = {
//    //        id: 0,
//    //        name: "",
//    //        isSelected: false
//    //    };
//    //    this.showLoadingGif = true;
//    //    this.driveTypeService.getBrakeConfig(Number(this.driveTypeIdSearchText)).subscribe(result => {
//    //        this.brakeTypeSerivce.getByFrontBrakeTypeId(result.frontBrakeTypeId).subscribe(bt => {
//    //            this.rearBrakeTypes = bt;
//    //            this.brakeABSService.getByFrontBrakeTypeIdRearBrakeTypeId(result.frontBrakeTypeId, result.rearBrakeTypeId).subscribe(babs => {
//    //                this.brakeABSes = babs;
//    //                this.brakeSystemService.getByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId(result.frontBrakeTypeId, result.rearBrakeTypeId, result.brakeABSId).subscribe(bs => {
//    //                    this.brakeSystems = bs;
//    //                    this.brakeConfigService.getByChildIds(result.frontBrakeTypeId, result.rearBrakeTypeId, result.brakeABSId, result.brakeSystemId).subscribe(result => {
//    //                        this.replaceBrakeConfig = result;
//    //                        this.showLoadingGif = false;
//    //                    }, error => {
//    //                        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
//    //                        this.showLoadingGif = false;
//    //                    });
//    //                }, error => {
//    //                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
//    //                    this.showLoadingGif = false;
//    //                });
//    //            }, error => {
//    //                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
//    //                this.showLoadingGif = false;
//    //            });
//    //        }, error => {
//    //            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
//    //            this.showLoadingGif = false;
//    //        });
//    //    }, error => {
//    //        let errorMessage: string = JSON.parse(String(error)).message;
//    //        this.toastr.warning(errorMessage, ConstantsWarehouse.errorTitle);
//    //        this.showLoadingGif = false;
//    //    });
//    //}
//    //onSelectAllVehicleToBrakeConfig(isSelected) {
//    //    this.isSelectAllVehicleToBrakeConfig = isSelected;
//    //    if (this.existingBrakeConfig.vehicleToBrakeConfigs == null) {
//    //        return;
//    //    }
//    //    this.existingBrakeConfig.vehicleToBrakeConfigs.forEach(item => item.isSelected = isSelected);
//    //}
//    //onClearFilters() {
//    //    this.selectedStartYear = "0";
//    //    this.selectedEndYear = "0";
//    //    this.refreshFacets();
//    //}
//    //getDefaultInputModel() {
//    //    return {
//    //        brakeConfigId: 0,
//    //        startYear: "0",
//    //        endYear: "0",
//    //        regions: [],
//    //        vehicleTypeGroups: [],
//    //        vehicleTypes: [],
//    //        makes: [],
//    //        models: [],
//    //        subModels: [],
//    //        frontBrakeTypes: [],
//    //        rearBrakeTypes: [],
//    //        brakeAbs: [],
//    //        brakeSystems: [],
//    //    };
//    //}
//    //refreshFacets() {
//    //    let inputModel = this.getDefaultInputModel();
//    //    inputModel.brakeConfigId = this.existingBrakeConfig.id;
//    //    inputModel.startYear = this.selectedStartYear;
//    //    inputModel.endYear = this.selectedEndYear;
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.regions) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(item => item.isSelected)
//    //            .forEach(m => inputModel.regions.push(m.name));
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.makes) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(item => item.isSelected)
//    //            .forEach(m => inputModel.makes.push(m.name));
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.models) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(item => item.isSelected)
//    //            .forEach(m => inputModel.models.push(m.name));
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.subModels) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(item => item.isSelected)
//    //            .forEach(m => inputModel.subModels.push(m.name));
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
//    //            .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
//    //            .forEach(m => inputModel.vehicleTypes.push(m.name));
//    //    }
//    //    this.showLoadingGif = true;
//    //    this.vehicleToBrakeConfigService.refreshFacets(inputModel).subscribe(data => {
//    //        this.updateRegionFacet(data.facets.regions);
//    //        this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
//    //        this.updateVehicleTypeFacet(data.facets.vehicleTypes);
//    //        this.updateYearFacet(data.facets.years);
//    //        this.updateMakeFacet(data.facets.makes);
//    //        this.updateModelFacet(data.facets.models, "");
//    //        this.updateSubModelFacet(data.facets.subModels, "");
//    //        this.showLoadingGif = false;
//    //    }, error => {
//    //        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
//    //        this.showLoadingGif = false;
//    //    });
//    //}
//    //filterMakes($event) {
//    //    if (this.vehicleToBrakeConfigSearchViewModel != null &&
//    //        this.vehicleToBrakeConfigSearchViewModel.facets != null &&
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.makes != null) {
//    //        var inputElement = <HTMLInputElement>$event.target;
//    //        this.makeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(
//    //            item => item.name.toLocaleLowerCase()
//    //                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
//    //    }
//    //}
//    //filterModels($event) {
//    //    if (this.vehicleToBrakeConfigSearchViewModel != null &&
//    //        this.vehicleToBrakeConfigSearchViewModel.facets != null &&
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.models != null) {
//    //        var inputElement = <HTMLInputElement>$event.target;
//    //        this.modelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(
//    //            item => item.name.toLocaleLowerCase()
//    //                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
//    //    }
//    //}
//    //filterSubModels($event) {
//    //    if (this.vehicleToBrakeConfigSearchViewModel != null &&
//    //        this.vehicleToBrakeConfigSearchViewModel.facets != null &&
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.subModels != null) {
//    //        var inputElement = <HTMLInputElement>$event.target;
//    //        this.subModelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(
//    //            item => item.name.toLocaleLowerCase()
//    //                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
//    //    }
//    //}
//    //filterRegions($event) {
//    //    if (this.vehicleToBrakeConfigSearchViewModel != null &&
//    //        this.vehicleToBrakeConfigSearchViewModel.facets != null &&
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.regions != null) {
//    //        var inputElement = <HTMLInputElement>$event.target;
//    //        this.regionFacet = this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(
//    //            item => item.name.toLocaleLowerCase()
//    //                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
//    //    }
//    //}
//    //filterVehicleTypeGroups($event) {
//    //    if (this.vehicleToBrakeConfigSearchViewModel != null &&
//    //        this.vehicleToBrakeConfigSearchViewModel.facets != null &&
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups != null) {
//    //        var inputElement = <HTMLInputElement>$event.target;
//    //        this.vehicleTypeGroupFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(
//    //            item => item.name.toLocaleLowerCase()
//    //                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
//    //    }
//    //}
//    //filterVehicleTypes($event) {
//    //    if (this.vehicleToBrakeConfigSearchViewModel != null &&
//    //        this.vehicleToBrakeConfigSearchViewModel.facets != null &&
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes != null) {
//    //        var inputElement = <HTMLInputElement>$event.target;
//    //        this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(
//    //            item => item.name.toLocaleLowerCase()
//    //                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
//    //    }
//    //}
//    //onYearSelected() {
//    //    this.refreshFacets();
//    //}
//    //onItemSelected(event, facet: IFacet[]) {
//    //    let isChecked = event.target.checked;
//    //    let selectedItem = facet.filter(item => item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase())[0];
//    //    selectedItem.isSelected = isChecked;
//    //    this.refreshFacets();
//    //}
//    //updateRegionFacet(regions) {
//    //    let existingSelectedRegions = this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(item => item.isSelected).map(item => item.name);
//    //    this.vehicleToBrakeConfigSearchViewModel.facets.regions = [];
//    //    for (let item of regions) {
//    //        let newItem = {
//    //            name: item,
//    //            isSelected: false
//    //        };
//    //        for (let existingSelectedRegion of existingSelectedRegions) {
//    //            if (item === existingSelectedRegion) {
//    //                newItem.isSelected = true;
//    //            }
//    //        }
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.regions.push(newItem);
//    //    }
//    //    this.regionFacet = this.vehicleToBrakeConfigSearchViewModel.facets.regions.slice();
//    //}
//    //updateYearFacet(years) {
//    //    this.startYearFacet = years.slice();
//    //    this.endYearFacet = years.slice();
//    //}
//    //updateMakeFacet(makes) {
//    //    let existingSelectedMakes = this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(make => make.isSelected).map(item => item.name);
//    //    this.vehicleToBrakeConfigSearchViewModel.facets.makes = [];
//    //    for (let make of makes) {
//    //        let newMake = {
//    //            name: make,
//    //            isSelected: false
//    //        };
//    //        for (let existingSelectedMake of existingSelectedMakes) {
//    //            if (make === existingSelectedMake) {
//    //                newMake.isSelected = true;
//    //            }
//    //        }
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.makes.push(newMake);
//    //    }
//    //    this.makeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.makes.slice();
//    //}
//    ////TODO: makeName is not used
//    //updateModelFacet(models, makeName) {
//    //    let existingSelectedModels = this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(model => model.isSelected)
//    //        .map(item => item.name);
//    //    this.vehicleToBrakeConfigSearchViewModel.facets.models = [];
//    //    for (let model of models) {
//    //        let newModel = {
//    //            name: model,
//    //            isSelected: false,
//    //        };
//    //        for (let existingSelectedModel of existingSelectedModels) {
//    //            if (model === existingSelectedModel) {
//    //                newModel.isSelected = true;
//    //            }
//    //        }
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.models.push(newModel);
//    //    }
//    //    this.modelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.models.slice();
//    //}
//    //updateSubModelFacet(subModels, modelName) {
//    //    let existingSelectedSubModels = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(submodel => submodel.isSelected)
//    //        .map(item => item.name);
//    //    this.vehicleToBrakeConfigSearchViewModel.facets.subModels = [];
//    //    for (let subModel of subModels) {
//    //        let newSubModel = {
//    //            name: subModel,
//    //            isSelected: false,
//    //        };
//    //        for (let existingSelectedSubModel of existingSelectedSubModels) {
//    //            if (subModel === existingSelectedSubModel) {
//    //                newSubModel.isSelected = true;
//    //            }
//    //        }
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.subModels.push(newSubModel);
//    //    }
//    //    this.subModelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.slice();
//    //}
//    //updateVehicleTypeGroupFacet(vehicleTypeGroups) {
//    //    let existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected).map(item => item.name);
//    //    this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups = [];
//    //    for (let item of vehicleTypeGroups) {
//    //        let newItem = {
//    //            name: item,
//    //            isSelected: false
//    //        };
//    //        for (let existingSelectedItem of existingSelectedItems) {
//    //            if (item === existingSelectedItem) {
//    //                newItem.isSelected = true;
//    //            }
//    //        }
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.push(newItem);
//    //    }
//    //    this.vehicleTypeGroupFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.slice();
//    //}
//    //updateVehicleTypeFacet(vehicleTypes) {
//    //    let existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected).map(item => item.name);
//    //    this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes = [];
//    //    for (let item of vehicleTypes) {
//    //        let newItem = {
//    //            name: item,
//    //            isSelected: false
//    //        };
//    //        for (let existingSelectedItem of existingSelectedItems) {
//    //            if (item === existingSelectedItem) {
//    //                newItem.isSelected = true;
//    //            }
//    //        }
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.push(newItem);
//    //    }
//    //    this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.slice();
//    //}
//    //searchVehicleToBrakeConfigs() {
//    //    this.showLoadingGif = true;
//    //    let inputModel = this.getDefaultInputModel();
//    //    inputModel.brakeConfigId = this.existingBrakeConfig.id;
//    //    inputModel.startYear = this.selectedStartYear;
//    //    inputModel.endYear = this.selectedEndYear;
//    //    //NOTE: inputModel.brakeConfigId = this.existingBrakeConfig.id; should be sufficient here
//    //    //inputModel.frontBrakeTypes.push(this.existingBrakeConfig.frontBrakeTypeName);
//    //    //inputModel.rearBrakeTypes.push(this.existingBrakeConfig.rearBrakeTypeName);
//    //    //inputModel.brakeAbs.push(this.existingBrakeConfig.brakeABSName);
//    //    //inputModel.brakeSystems.push(this.existingBrakeConfig.brakeSystemName);
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.makes) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(item => item.isSelected)
//    //            .forEach(m => inputModel.makes.push(m.name));
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.models) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(item => item.isSelected)
//    //            .forEach(m => inputModel.models.push(m.name));
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.subModels) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(item => item.isSelected)
//    //            .forEach(s => inputModel.subModels.push(s.name));
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
//    //            .forEach(m => inputModel.vehicleTypes.push(m.name));
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
//    //            .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.regions) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(item => item.isSelected)
//    //            .forEach(m => inputModel.regions.push(m.name));
//    //    }
//    //    this.vehicleToBrakeConfigService.getAssociations(inputModel).subscribe(result => {
//    //        if (result.length > 0) {
//    //            this.existingBrakeConfig.vehicleToBrakeConfigs = result;
//    //            this.existingBrakeConfig.vehicleToBrakeConfigCount = result.length;
//    //            this.isSelectAllVehicleToBrakeConfig = false;
//    //            if (this.vehicleToBrakeConfigGrid)
//    //                this.vehicleToBrakeConfigGrid.refresh();
//    //            this.showLoadingGif = false;
//    //            $(".drawer-left").css('width', 15);
//    //        }
//    //        else {
//    //            this.toastr.warning("The search yeilded no result", "No Record Found!!");
//    //            this.showLoadingGif = false;
//    //        }
//    //    }, error => {
//    //        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
//    //        this.showLoadingGif = false;
//    //    });
//    //}
//    //// event on view affected vehicle associations
//    //onViewAffectedAssociations() {
//    //    $(".drawer-left").css('width', 800); //show the filter panel to enable search
//    //}
//    //clearAllFacets() {
//    //    this.selectedStartYear = "0";
//    //    this.selectedEndYear = "0";
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.regions) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.regions.forEach(item => item.isSelected = false);
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.makes) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.makes.forEach(item => item.isSelected = false);
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.models) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.models.forEach(item => item.isSelected = false);
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.subModels) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.subModels.forEach(item => item.isSelected = false);
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.forEach(item => item.isSelected = false);
//    //    }
//    //    if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes) {
//    //        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.forEach(item => item.isSelected = false);
//    //    }
//    //    this.refreshFacets();
//    //}
//    //clearFacet(facet: IFacet[], refresh: boolean = true) {
//    //    if (facet) {
//    //        facet.forEach(item => item.isSelected = false);
//    //    }
//    //    if (refresh) {
//    //        this.refreshFacets();
//    //    }
//    //}
//    //onVehicleToBrakeConfigSelected(vechicleToBrakeConfig: IVehicleToBrakeConfig) {
//    //    if (vechicleToBrakeConfig.isSelected) {
//    //        //unchecked
//    //        this.isSelectAllVehicleToBrakeConfig = false;
//    //    }
//    //    else {
//    //        //checked
//    //        var excludedVehicle = this.existingBrakeConfig.vehicleToBrakeConfigs.filter(item => item.id != vechicleToBrakeConfig.id);
//    //        if (excludedVehicle.every(item => item.isSelected)) {
//    //            this.isSelectAllVehicleToBrakeConfig = true;
//    //        }
//    //    }
//    //}
//    //// event on continue
//    //onContinue() {
//    //    // validate
//    //    if (this.validateReplaceBrakeConfig()) {
//    //        // set data in factory/ service
//    //        this.brakeConfigService.existingBrakeConfig = this.existingBrakeConfig;
//    //        this.brakeConfigService.existingBrakeConfig.vehicleToBrakeConfigs = this.existingBrakeConfig.vehicleToBrakeConfigs.filter(item => item.isSelected);
//    //        this.replaceBrakeConfig.frontBrakeTypeName = this.frontBrakeTypes.filter(item => item.id === Number(this.replaceBrakeConfig.frontBrakeTypeId))[0].name;
//    //        this.replaceBrakeConfig.rearBrakeTypeName = this.rearBrakeTypes.filter(item => item.id === Number(this.replaceBrakeConfig.rearBrakeTypeId))[0].name;
//    //        this.replaceBrakeConfig.brakeABSName = this.brakeABSes.filter(item => item.id === Number(this.replaceBrakeConfig.brakeABSId))[0].name;
//    //        this.replaceBrakeConfig.brakeSystemName = this.brakeSystems.filter(item => item.id === Number(this.replaceBrakeConfig.brakeSystemId))[0].name;
//    //        this.brakeConfigService.replacementBrakeConfig = this.replaceBrakeConfig;
//    //        // redirect to Confirm page.
//    //        this.router.navigateByUrl("/brakeconfig/replace/confirm/" + this.existingBrakeConfig.id);
//    //    } else {
//    //        //console.log(this.errorMessage);
//    //    }
//    //}
//}
