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
var vehicleToDriveType_search_model_1 = require("../vehicleToDriveType/vehicleToDriveType-search.model");
var vehicleToDriveType_service_1 = require("../vehicleToDriveType/vehicleToDriveType.service");
var VehicleToDriveTypeSearchPanel = (function () {
    function VehicleToDriveTypeSearchPanel(sharedService, toastr, vehicleToDriveTypesService) {
        this.sharedService = sharedService;
        this.toastr = toastr;
        this.vehicleToDriveTypesService = vehicleToDriveTypesService;
        this.vehicleTypeGroupFacet = [];
        this.vehicleToDriveTypesRetrieved = [];
        this.showLoadingGif = false;
        this.onSearchEvent = new core_1.EventEmitter();
    }
    VehicleToDriveTypeSearchPanel.prototype.ngOnInit = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.driveTypeId = "";
        if (this.sharedService.vehicleToDriveTypeSearchViewModel != null) {
            this.vehicleToDriveTypeSearchViewModel.facets = this.sharedService.vehicleToDriveTypeSearchViewModel.facets;
            this.regionFacet = this.vehicleToDriveTypeSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleToDriveTypeSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleToDriveTypeSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleToDriveTypeSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleToDriveTypeSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleToDriveTypeSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.slice();
            this.driveTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.slice();
            if (this.sharedService.vehicleToDriveTypeSearchViewModel.searchType == vehicleToDriveType_search_model_1.SearchType.SearchByDriveTypeId) {
                this.searchByDriveTypeId();
            }
            else if (this.sharedService.vehicleToDriveTypeSearchViewModel.searchType == vehicleToDriveType_search_model_1.SearchType.GeneralSearch) {
            }
            else {
                this.showLoadingGif = false;
            }
        }
        else {
            this.refreshFacets();
        }
    };
    VehicleToDriveTypeSearchPanel.prototype.onClearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.driveTypeId = "";
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
        if (this.vehicleToDriveTypeSearchViewModel.facets.driveTypes) {
            this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.forEach(function (item) { return item.isSelected = false; });
        }
        this.refreshFacets();
    };
    VehicleToDriveTypeSearchPanel.prototype.getDefaultInputModel = function () {
        return {
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
    VehicleToDriveTypeSearchPanel.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
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
        if (this.vehicleToDriveTypeSearchViewModel.facets.driveTypes) {
            this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.driveTypes.push(m.name); });
        }
        this.showLoadingGif = true;
        this.vehicleToDriveTypesService.refreshFacets(inputModel).subscribe(function (data) {
            _this.updateRegionFacet(data.facets.regions);
            _this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
            _this.updateVehicleTypeFacet(data.facets.vehicleTypes);
            _this.updateYearFacet(data.facets.years);
            _this.updateMakeFacet(data.facets.makes);
            _this.updateModelFacet(data.facets.models, "");
            _this.updateSubModelFacet(data.facets.subModels, "");
            _this.updateDriveTypeFacet(data.facets.driveTypes);
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToDriveTypeSearchPanel.prototype.filterMakes = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleToDriveTypeSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToDriveTypeSearchPanel.prototype.filterModels = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleToDriveTypeSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToDriveTypeSearchPanel.prototype.filterSubModels = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleToDriveTypeSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToDriveTypeSearchPanel.prototype.filterRegions = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleToDriveTypeSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToDriveTypeSearchPanel.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToDriveTypeSearchPanel.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToDriveTypeSearchPanel.prototype.filterDriveType = function ($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.driveTypes != null) {
            var inputElement = $event.target;
            this.driveTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToDriveTypeSearchPanel.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    VehicleToDriveTypeSearchPanel.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    VehicleToDriveTypeSearchPanel.prototype.updateRegionFacet = function (regions) {
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
    VehicleToDriveTypeSearchPanel.prototype.updateYearFacet = function (years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    VehicleToDriveTypeSearchPanel.prototype.updateMakeFacet = function (makes) {
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
    VehicleToDriveTypeSearchPanel.prototype.updateModelFacet = function (models, makeName) {
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
    VehicleToDriveTypeSearchPanel.prototype.updateSubModelFacet = function (subModels, modelName) {
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
    VehicleToDriveTypeSearchPanel.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
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
    VehicleToDriveTypeSearchPanel.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
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
    VehicleToDriveTypeSearchPanel.prototype.updateDriveTypeFacet = function (frontBrakeTypes) {
        var existingSelectedItems = this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToDriveTypeSearchViewModel.facets.driveTypes = [];
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
            this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.push(newItem);
        }
        this.driveTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.slice();
    };
    VehicleToDriveTypeSearchPanel.prototype.searchByDriveTypeId = function () {
        var _this = this;
        var driveTypeId = Number(this.driveTypeId);
        if (isNaN(driveTypeId)) {
            this.toastr.warning("Invalid Drive TypeIdId", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.vehicleToDriveTypeSearchViewModel.searchType = vehicleToDriveType_search_model_1.SearchType.SearchByDriveTypeId;
        this.showLoadingGif = true;
        this.vehicleToDriveTypesService.searchByDriveTypeId(driveTypeId).subscribe(function (m) {
            if (m.result.driveTypes.length > 0) {
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
    VehicleToDriveTypeSearchPanel.prototype.onDriveTypesKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.searchByDriveTypeId();
        }
    };
    VehicleToDriveTypeSearchPanel.prototype.getSearchResult = function (m) {
        var _this = this;
        this.vehicleToDriveTypeSearchViewModel.result = m.result;
        this.vehicleToDriveTypeSearchViewModel.totalCount = m.totalCount;
        this.vehicleToDriveTypeForSelectedDriveType = [];
        this.isSelectAllDriveTypes = false;
        // note: select all when totalRecords <= threshold
        if (this.vehicleToDriveTypeSearchViewModel.result.driveTypes.length <= this.thresholdRecordCount) {
            this.vehicleToDriveTypeSearchViewModel.result.driveTypes.forEach(function (item) {
                item.isSelected = true;
                _this.refreshAssociationWithDriveTypesId(item.id, item.isSelected);
            });
            this.isSelectAllDriveTypes = true;
        }
        // callback emitter
        this.onSearchEvent.emit(this.vehicleToDriveTypeForSelectedDriveType);
    };
    // todo: make is modular, this method is at two location
    VehicleToDriveTypeSearchPanel.prototype.refreshAssociationWithDriveTypesId = function (brakeConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToDriveTypesRetrieved = this.getVehicleToDriveTypesByDriveTypesId(brakeConfigId);
            //TODO: number of associations which may be useful in add brake association screen?
            var temp = this.vehicleToDriveTypeForSelectedDriveType || [];
            for (var _i = 0, _a = this.vehicleToDriveTypesRetrieved; _i < _a.length; _i++) {
                var vehicleToDriveTypes = _a[_i];
                temp.push(vehicleToDriveTypes);
            }
            this.vehicleToDriveTypeForSelectedDriveType = temp;
        }
        else {
            var m = this.vehicleToDriveTypeForSelectedDriveType.filter(function (x) { return x.driveType.id != brakeConfigId; });
            this.vehicleToDriveTypeForSelectedDriveType = m;
        }
    };
    VehicleToDriveTypeSearchPanel.prototype.getVehicleToDriveTypesByDriveTypesId = function (id) {
        return this.vehicleToDriveTypeSearchViewModel.result.vehicleToDriveTypes.filter(function (v) { return v.driveType.id == id; });
    };
    VehicleToDriveTypeSearchPanel.prototype.search = function () {
        var _this = this;
        this.vehicleToDriveTypeSearchViewModel.searchType = vehicleToDriveType_search_model_1.SearchType.GeneralSearch;
        this.showLoadingGif = true;
        var inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToDriveTypeSearchViewModel.facets.driveTypes) {
            this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.driveTypes.push(m.name); });
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
        this.vehicleToDriveTypesService.search(inputModel).subscribe(function (m) {
            if (m.result.driveTypes.length > 0 || m.result.vehicleToDriveTypes.length > 0) {
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
    VehicleToDriveTypeSearchPanel.prototype.clearFacet = function (facet, refresh) {
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
    ], VehicleToDriveTypeSearchPanel.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.Input("vehicleToDriveTypesSearchViewModel"), 
        __metadata('design:type', Object)
    ], VehicleToDriveTypeSearchPanel.prototype, "vehicleToDriveTypeSearchViewModel", void 0);
    __decorate([
        core_1.Input("vehicleToDriveTypesForSelectedBrake"), 
        __metadata('design:type', Array)
    ], VehicleToDriveTypeSearchPanel.prototype, "vehicleToDriveTypeForSelectedDriveType", void 0);
    __decorate([
        core_1.Output("onSearchEvent"), 
        __metadata('design:type', Object)
    ], VehicleToDriveTypeSearchPanel.prototype, "onSearchEvent", void 0);
    VehicleToDriveTypeSearchPanel = __decorate([
        core_1.Component({
            selector: "vehicletodrivetype-searchpanel",
            templateUrl: "app/templates/vehicleToDriveType/vehicleToDriveType-searchPanel.component.html",
            providers: [shared_service_1.SharedService, vehicleToDriveType_service_1.VehicleToDriveTypeService],
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, ng2_toastr_1.ToastsManager, vehicleToDriveType_service_1.VehicleToDriveTypeService])
    ], VehicleToDriveTypeSearchPanel);
    return VehicleToDriveTypeSearchPanel;
}());
exports.VehicleToDriveTypeSearchPanel = VehicleToDriveTypeSearchPanel;
