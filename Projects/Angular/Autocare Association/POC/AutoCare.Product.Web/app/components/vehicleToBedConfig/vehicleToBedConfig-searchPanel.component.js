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
var vehicleToBedConfig_search_model_1 = require("../vehicleToBedConfig/vehicleToBedConfig-search.model");
var vehicleTobedConfig_service_1 = require("../vehicleTobedConfig/vehicleTobedConfig.service");
var VehicleToBedConfigSearchPanel = (function () {
    function VehicleToBedConfigSearchPanel(sharedService, toastr, vehicleToBedConfigService) {
        this.sharedService = sharedService;
        this.toastr = toastr;
        this.vehicleToBedConfigService = vehicleToBedConfigService;
        this.vehicleTypeGroupFacet = [];
        this.vehicleToBedConfigsRetrieved = [];
        this.showLoadingGif = false;
        this.onSearchEvent = new core_1.EventEmitter();
    }
    VehicleToBedConfigSearchPanel.prototype.ngOnInit = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        if (this.sharedService.vehicleToBedConfigSearchViewModel != null) {
            this.vehicleToBedConfigSearchViewModel.facets = this.sharedService.vehicleToBedConfigSearchViewModel.facets;
            this.bedLengthFacet = this.vehicleToBedConfigSearchViewModel.facets.bedLengths.slice();
            this.bedTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.bedTypes.slice();
            this.regionFacet = this.vehicleToBedConfigSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleToBedConfigSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleToBedConfigSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleToBedConfigSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleToBedConfigSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleToBedConfigSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.slice();
            if (this.sharedService.vehicleToBedConfigSearchViewModel.searchType == vehicleToBedConfig_search_model_1.SearchType.SearchByBedConfigId) {
                this.searchByBedConfigId();
            }
            else if (this.sharedService.vehicleToBedConfigSearchViewModel.searchType == vehicleToBedConfig_search_model_1.SearchType.GeneralSearch) {
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
    VehicleToBedConfigSearchPanel.prototype.onClearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        if (this.vehicleToBedConfigSearchViewModel.facets.regions) {
            this.vehicleToBedConfigSearchViewModel.facets.regions.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.makes) {
            this.vehicleToBedConfigSearchViewModel.facets.makes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.models) {
            this.vehicleToBedConfigSearchViewModel.facets.models.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.subModels) {
            this.vehicleToBedConfigSearchViewModel.facets.subModels.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.bedTypes) {
            this.vehicleToBedConfigSearchViewModel.facets.bedTypes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.bedLengths) {
            this.vehicleToBedConfigSearchViewModel.facets.bedLengths.forEach(function (item) { return item.isSelected = false; });
        }
        this.refreshFacets();
    };
    VehicleToBedConfigSearchPanel.prototype.getDefaultInputModel = function () {
        return {
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
            bedTypes: [],
            bedLengths: []
        };
    };
    VehicleToBedConfigSearchPanel.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToBedConfigSearchViewModel.facets.regions) {
            this.vehicleToBedConfigSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.makes) {
            this.vehicleToBedConfigSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.models) {
            this.vehicleToBedConfigSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.subModels) {
            this.vehicleToBedConfigSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.subModels.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.bedTypes) {
            this.vehicleToBedConfigSearchViewModel.facets.bedTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.bedTypes.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.bedLengths) {
            this.vehicleToBedConfigSearchViewModel.facets.bedLengths.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.bedLengths.push(m.name); });
        }
        this.showLoadingGif = true;
        this.vehicleToBedConfigService.refreshFacets(inputModel).subscribe(function (data) {
            _this.updateRegionFacet(data.facets.regions);
            _this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
            _this.updateVehicleTypeFacet(data.facets.vehicleTypes);
            _this.updateYearFacet(data.facets.years);
            _this.updateMakeFacet(data.facets.makes);
            _this.updateModelFacet(data.facets.models, "");
            _this.updateSubModelFacet(data.facets.subModels, "");
            _this.updateBedTypeFacet(data.facets.bedTypes);
            _this.updateBedLengthFacet(data.facets.bedLengths);
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToBedConfigSearchPanel.prototype.filterMakes = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleToBedConfigSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBedConfigSearchPanel.prototype.filterModels = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleToBedConfigSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBedConfigSearchPanel.prototype.filterSubModels = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleToBedConfigSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBedConfigSearchPanel.prototype.filterRegions = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleToBedConfigSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBedConfigSearchPanel.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBedConfigSearchPanel.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBedConfigSearchPanel.prototype.filterBedTypes = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.bedTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.bedTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBedConfigSearchPanel.prototype.filterBedLengths = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.bedLengths != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.bedLengths.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBedConfigSearchPanel.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    VehicleToBedConfigSearchPanel.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    VehicleToBedConfigSearchPanel.prototype.updateRegionFacet = function (regions) {
        var existingSelectedRegions = this.vehicleToBedConfigSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBedConfigSearchViewModel.facets.regions = [];
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
            this.vehicleToBedConfigSearchViewModel.facets.regions.push(newItem);
        }
        this.regionFacet = this.vehicleToBedConfigSearchViewModel.facets.regions.slice();
    };
    VehicleToBedConfigSearchPanel.prototype.updateYearFacet = function (years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    VehicleToBedConfigSearchPanel.prototype.updateMakeFacet = function (makes) {
        var existingSelectedMakes = this.vehicleToBedConfigSearchViewModel.facets.makes.filter(function (make) { return make.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBedConfigSearchViewModel.facets.makes = [];
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
            this.vehicleToBedConfigSearchViewModel.facets.makes.push(newMake);
        }
        this.makeFacet = this.vehicleToBedConfigSearchViewModel.facets.makes.slice();
    };
    //TODO: makeName is not used
    VehicleToBedConfigSearchPanel.prototype.updateModelFacet = function (models, makeName) {
        var existingSelectedModels = this.vehicleToBedConfigSearchViewModel.facets.models.filter(function (model) { return model.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToBedConfigSearchViewModel.facets.models = [];
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
            this.vehicleToBedConfigSearchViewModel.facets.models.push(newModel);
        }
        this.modelFacet = this.vehicleToBedConfigSearchViewModel.facets.models.slice();
    };
    VehicleToBedConfigSearchPanel.prototype.updateSubModelFacet = function (subModels, modelName) {
        var existingSelectedSubModels = this.vehicleToBedConfigSearchViewModel.facets.subModels.filter(function (submodel) { return submodel.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToBedConfigSearchViewModel.facets.subModels = [];
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
            this.vehicleToBedConfigSearchViewModel.facets.subModels.push(newSubModel);
        }
        this.subModelFacet = this.vehicleToBedConfigSearchViewModel.facets.subModels.slice();
    };
    VehicleToBedConfigSearchPanel.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
        var existingSelectedItems = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups = [];
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
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }
        this.vehicleTypeGroupFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.slice();
    };
    VehicleToBedConfigSearchPanel.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
        var existingSelectedItems = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes = [];
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
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.push(newItem);
        }
        this.vehicleTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.slice();
    };
    VehicleToBedConfigSearchPanel.prototype.updateBedTypeFacet = function (bedTypes) {
        var existingSelectedItems = this.vehicleToBedConfigSearchViewModel.facets.bedTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBedConfigSearchViewModel.facets.bedTypes = [];
        for (var _i = 0, bedTypes_1 = bedTypes; _i < bedTypes_1.length; _i++) {
            var item = bedTypes_1[_i];
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
            this.vehicleToBedConfigSearchViewModel.facets.bedTypes.push(newItem);
        }
        this.bedTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.bedTypes.slice();
    };
    VehicleToBedConfigSearchPanel.prototype.updateBedLengthFacet = function (bedLengths) {
        var existingSelectedItems = this.vehicleToBedConfigSearchViewModel.facets.bedLengths.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBedConfigSearchViewModel.facets.bedLengths = [];
        for (var _i = 0, bedLengths_1 = bedLengths; _i < bedLengths_1.length; _i++) {
            var item = bedLengths_1[_i];
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
            this.vehicleToBedConfigSearchViewModel.facets.bedLengths.push(newItem);
        }
        this.bedLengthFacet = this.vehicleToBedConfigSearchViewModel.facets.bedLengths.slice();
    };
    VehicleToBedConfigSearchPanel.prototype.searchByBedConfigId = function () {
        var _this = this;
        var bedConfigId = Number(this.bedConfigId);
        if (isNaN(bedConfigId)) {
            this.toastr.warning("Invalid Bed Config Id", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.vehicleToBedConfigSearchViewModel.searchType = vehicleToBedConfig_search_model_1.SearchType.SearchByBedConfigId;
        this.showLoadingGif = true;
        this.vehicleToBedConfigService.searchByBedConfigId(bedConfigId).subscribe(function (m) {
            if (m.result.bedConfigs.length > 0) {
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
    VehicleToBedConfigSearchPanel.prototype.onBedConfigKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.searchByBedConfigId();
        }
    };
    VehicleToBedConfigSearchPanel.prototype.getSearchResult = function (m) {
        var _this = this;
        this.vehicleToBedConfigSearchViewModel.result = m.result;
        this.vehicleToBedConfigSearchViewModel.totalCount = m.totalCount;
        this.vehicleToBedConfigsForSelectedBed = [];
        this.isSelectAllBedSystems = false;
        if (this.vehicleToBedConfigSearchViewModel.result.bedConfigs.length <= this.thresholdRecordCount) {
            this.vehicleToBedConfigSearchViewModel.result.bedConfigs.forEach(function (item) {
                item.isSelected = true;
                _this.refreshAssociationWithBedConfigId(item.id, item.isSelected);
            });
            this.isSelectAllBedSystems = true;
        }
        // callback emitter
        this.onSearchEvent.emit(this.vehicleToBedConfigsForSelectedBed);
    };
    // todo: make is modular, this method is at two location
    VehicleToBedConfigSearchPanel.prototype.refreshAssociationWithBedConfigId = function (bedConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToBedConfigsRetrieved = this.getVehicleToBedConfigsByBedConfigId(bedConfigId);
            //TODO: number of associations which may be useful in add bed association screen?
            var temp = this.vehicleToBedConfigsForSelectedBed || [];
            for (var _i = 0, _a = this.vehicleToBedConfigsRetrieved; _i < _a.length; _i++) {
                var vehicleToBedConfig = _a[_i];
                temp.push(vehicleToBedConfig);
            }
            this.vehicleToBedConfigsForSelectedBed = temp;
        }
        else {
            var m = this.vehicleToBedConfigsForSelectedBed.filter(function (x) { return x.bedConfig.id != bedConfigId; });
            this.vehicleToBedConfigsForSelectedBed = m;
        }
    };
    VehicleToBedConfigSearchPanel.prototype.getVehicleToBedConfigsByBedConfigId = function (id) {
        return this.vehicleToBedConfigSearchViewModel.result.vehicleToBedConfigs.filter(function (v) { return v.bedConfig.id == id; });
    };
    VehicleToBedConfigSearchPanel.prototype.search = function () {
        var _this = this;
        this.vehicleToBedConfigSearchViewModel.searchType = vehicleToBedConfig_search_model_1.SearchType.GeneralSearch;
        this.showLoadingGif = true;
        var inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToBedConfigSearchViewModel.facets.bedTypes) {
            this.vehicleToBedConfigSearchViewModel.facets.bedTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.bedTypes.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.bedLengths) {
            this.vehicleToBedConfigSearchViewModel.facets.bedLengths.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.bedLengths.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.makes) {
            this.vehicleToBedConfigSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.models) {
            this.vehicleToBedConfigSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.subModels) {
            this.vehicleToBedConfigSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (s) { return inputModel.subModels.push(s.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.regions) {
            this.vehicleToBedConfigSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        this.vehicleToBedConfigService.search(inputModel).subscribe(function (m) {
            if (m.result.bedConfigs.length > 0 || m.result.vehicleToBedConfigs.length > 0) {
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
    VehicleToBedConfigSearchPanel.prototype.clearFacet = function (facet, refresh) {
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
    ], VehicleToBedConfigSearchPanel.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.Input("vehicleToBedConfigSearchViewModel"), 
        __metadata('design:type', Object)
    ], VehicleToBedConfigSearchPanel.prototype, "vehicleToBedConfigSearchViewModel", void 0);
    __decorate([
        core_1.Input("vehicleToBedConfigsForSelectedBed"), 
        __metadata('design:type', Array)
    ], VehicleToBedConfigSearchPanel.prototype, "vehicleToBedConfigsForSelectedBed", void 0);
    __decorate([
        core_1.Output("onSearchEvent"), 
        __metadata('design:type', Object)
    ], VehicleToBedConfigSearchPanel.prototype, "onSearchEvent", void 0);
    VehicleToBedConfigSearchPanel = __decorate([
        core_1.Component({
            selector: "vehicletobedconfig-searchpanel",
            templateUrl: "app/templates/vehicleToBedConfig/vehicleToBedConfig-searchPanel.component.html",
            providers: [vehicleTobedConfig_service_1.VehicleToBedConfigService],
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, ng2_toastr_1.ToastsManager, vehicleTobedConfig_service_1.VehicleToBedConfigService])
    ], VehicleToBedConfigSearchPanel);
    return VehicleToBedConfigSearchPanel;
}());
exports.VehicleToBedConfigSearchPanel = VehicleToBedConfigSearchPanel;
