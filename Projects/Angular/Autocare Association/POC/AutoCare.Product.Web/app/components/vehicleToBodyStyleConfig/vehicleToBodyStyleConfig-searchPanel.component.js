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
var vehicleToBodyStyleConfig_service_1 = require("../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.service");
var shared_model_1 = require("../shared/shared.model");
var VehicleToBodyStyleConfigSearchPanel = (function () {
    function VehicleToBodyStyleConfigSearchPanel(sharedService, toastr, vehicleToBodyStyleConfigService) {
        this.sharedService = sharedService;
        this.toastr = toastr;
        this.vehicleToBodyStyleConfigService = vehicleToBodyStyleConfigService;
        this.vehicleTypeGroupFacet = [];
        this.vehicleToBodyStyleConfigsRetrieved = [];
        this.showLoadingGif = false;
        this.onSearchEvent = new core_1.EventEmitter();
    }
    VehicleToBodyStyleConfigSearchPanel.prototype.ngOnInit = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.bodyStyleConfigId = "";
        if (this.sharedService.vehicleToBodyStyleConfigSearchViewModel != null) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets = this.sharedService.vehicleToBodyStyleConfigSearchViewModel.facets;
            this.bodyNumDoorsFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.slice();
            this.bodyTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.slice();
            this.regionFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.slice();
            if (this.sharedService.vehicleToBodyStyleConfigSearchViewModel.searchType == shared_model_1.SearchType.SearchByConfigId) {
                this.searchByBodyStyleConfigId();
            }
            else if (this.sharedService.vehicleToBodyStyleConfigSearchViewModel.searchType == shared_model_1.SearchType.GeneralSearch) {
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
    VehicleToBodyStyleConfigSearchPanel.prototype.onClearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.bodyStyleConfigId = "";
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.regions) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.makes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.models) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.forEach(function (item) { return item.isSelected = false; });
        }
        this.refreshFacets();
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.regions) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.makes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.models) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.subModels.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.bodyNumDoors.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.bodyTypes.push(m.name); });
        }
        this.showLoadingGif = true;
        this.vehicleToBodyStyleConfigService.refreshFacets(inputModel).subscribe(function (data) {
            _this.updateRegionFacet(data.facets.regions);
            _this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
            _this.updateVehicleTypeFacet(data.facets.vehicleTypes);
            _this.updateYearFacet(data.facets.years);
            _this.updateMakeFacet(data.facets.makes);
            _this.updateModelFacet(data.facets.models);
            _this.updateSubModelFacet(data.facets.subModels);
            _this.updateBodyNumDoorsFacet(data.facets.bodyNumDoors);
            _this.updateBodyTypeFacet(data.facets.bodyTypes);
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.getDefaultInputModel = function () {
        return {
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
            bodyNumDoors: [],
            bodyTypes: []
        };
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.updateRegionFacet = function (regions) {
        var existingSelectedRegions = this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBodyStyleConfigSearchViewModel.facets.regions = [];
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
            this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.push(newItem);
        }
        this.regionFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.slice();
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
        var existingSelectedItems = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups = [];
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
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }
        this.vehicleTypeGroupFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.slice();
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
        var existingSelectedItems = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes = [];
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
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.push(newItem);
        }
        this.vehicleTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.slice();
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.updateYearFacet = function (years) {
        this.vehicleToBodyStyleConfigSearchViewModel.facets.startYears = years.slice();
        this.vehicleToBodyStyleConfigSearchViewModel.facets.endYears = years.slice();
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.updateMakeFacet = function (makes) {
        var existingSelectedMakes = this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.filter(function (make) { return make.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBodyStyleConfigSearchViewModel.facets.makes = [];
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
            this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.push(newMake);
        }
        this.makeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.slice();
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.updateModelFacet = function (models) {
        var existingSelectedModels = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.filter(function (model) { return model.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToBodyStyleConfigSearchViewModel.facets.models = [];
        for (var _i = 0, models_1 = models; _i < models_1.length; _i++) {
            var model = models_1[_i];
            var newModel = {
                name: model,
                isSelected: false
            };
            for (var _a = 0, existingSelectedModels_1 = existingSelectedModels; _a < existingSelectedModels_1.length; _a++) {
                var existingSelectedModel = existingSelectedModels_1[_a];
                if (model === existingSelectedModel) {
                    newModel.isSelected = true;
                }
            }
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models.push(newModel);
        }
        this.modelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.slice();
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.updateSubModelFacet = function (subModels) {
        var existingSelectedSubModels = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.filter(function (submodel) { return submodel.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels = [];
        for (var _i = 0, subModels_1 = subModels; _i < subModels_1.length; _i++) {
            var subModel = subModels_1[_i];
            var newSubModel = {
                name: subModel,
                isSelected: false
            };
            for (var _a = 0, existingSelectedSubModels_1 = existingSelectedSubModels; _a < existingSelectedSubModels_1.length; _a++) {
                var existingSelectedSubModel = existingSelectedSubModels_1[_a];
                if (subModel === existingSelectedSubModel) {
                    newSubModel.isSelected = true;
                }
            }
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.push(newSubModel);
        }
        this.subModelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.slice();
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.updateBodyNumDoorsFacet = function (bodyNumDoors) {
        var existingSelectedItems = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors = [];
        for (var _i = 0, bodyNumDoors_1 = bodyNumDoors; _i < bodyNumDoors_1.length; _i++) {
            var item = bodyNumDoors_1[_i];
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
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.push(newItem);
        }
        this.bodyNumDoorsFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.slice();
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.updateBodyTypeFacet = function (bodyTypes) {
        var existingSelectedItems = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes = [];
        for (var _i = 0, bodyTypes_1 = bodyTypes; _i < bodyTypes_1.length; _i++) {
            var item = bodyTypes_1[_i];
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
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.push(newItem);
        }
        this.bodyTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.slice();
    };
    // filters
    VehicleToBodyStyleConfigSearchPanel.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.filterMakes = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.filterModels = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.filterSubModels = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.filterRegions = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.filterBodyNumDoors = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.filterBodyTypes = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.searchByBodyStyleConfigId = function () {
        var _this = this;
        var bodyStyleConfigId = Number(this.bodyStyleConfigId);
        if (isNaN(bodyStyleConfigId)) {
            this.toastr.warning("Invalid Body Style Config Id", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.vehicleToBodyStyleConfigSearchViewModel.searchType = shared_model_1.SearchType.SearchByConfigId;
        this.showLoadingGif = true;
        this.vehicleToBodyStyleConfigService.searchByBodyStyleConfigId(bodyStyleConfigId).subscribe(function (m) {
            if (m.result.bodyStyleConfigs.length > 0) {
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
    VehicleToBodyStyleConfigSearchPanel.prototype.getSearchResult = function (m) {
        var _this = this;
        this.vehicleToBodyStyleConfigSearchViewModel.result = m.result;
        this.vehicleToBodyStyleConfigSearchViewModel.totalCount = m.totalCount;
        this.vehicleToBodyStyleConfigsForSelectedBodyStyle = [];
        this.isSelectAllBodyStyleConfigs = false;
        // note: select all only if totalCount <= threshold
        if (this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.length <= this.thresholdRecordCount) {
            this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.forEach(function (item) {
                item.isSelected = true;
                _this.refreshAssociationWithBodyStyleConfigId(item.id, item.isSelected);
            });
            this.isSelectAllBodyStyleConfigs = true;
        }
        // callback emitter
        this.onSearchEvent.emit(this.vehicleToBodyStyleConfigsForSelectedBodyStyle);
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.refreshAssociationWithBodyStyleConfigId = function (bodyStyleConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToBodyStyleConfigsRetrieved = this.getVehicleToBodyStyleConfigsByBodyStyleConfigId(bodyStyleConfigId);
            //TODO: number of associations which may be useful in add BodyStyle association screen?
            var temp = this.vehicleToBodyStyleConfigsForSelectedBodyStyle || [];
            for (var _i = 0, _a = this.vehicleToBodyStyleConfigsRetrieved; _i < _a.length; _i++) {
                var vehicleToBodyStyleConfig = _a[_i];
                temp.push(vehicleToBodyStyleConfig);
            }
            this.vehicleToBodyStyleConfigsForSelectedBodyStyle = temp;
        }
        else {
            var m = this.vehicleToBodyStyleConfigsForSelectedBodyStyle.filter(function (x) { return x.bodyStyleConfig.id != bodyStyleConfigId; });
            this.vehicleToBodyStyleConfigsForSelectedBodyStyle = m;
        }
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.getVehicleToBodyStyleConfigsByBodyStyleConfigId = function (id) {
        return this.vehicleToBodyStyleConfigSearchViewModel.result.vehicleToBodyStyleConfigs.filter(function (v) { return v.bodyStyleConfig.id == id; });
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.onBodyStyleConfigKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.searchByBodyStyleConfigId();
        }
    };
    VehicleToBodyStyleConfigSearchPanel.prototype.search = function () {
        var _this = this;
        this.vehicleToBodyStyleConfigSearchViewModel.searchType = shared_model_1.SearchType.GeneralSearch;
        this.showLoadingGif = true;
        var inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.bodyNumDoors.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.bodyTypes.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.makes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.models) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (s) { return inputModel.subModels.push(s.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.regions) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        this.vehicleToBodyStyleConfigService.search(inputModel).subscribe(function (m) {
            if (m.result.bodyStyleConfigs.length > 0 || m.result.vehicleToBodyStyleConfigs.length > 0) {
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
    VehicleToBodyStyleConfigSearchPanel.prototype.clearFacet = function (facet, refresh) {
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
    ], VehicleToBodyStyleConfigSearchPanel.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.Input("vehicleToBodyStyleConfigSearchViewModel"), 
        __metadata('design:type', Object)
    ], VehicleToBodyStyleConfigSearchPanel.prototype, "vehicleToBodyStyleConfigSearchViewModel", void 0);
    __decorate([
        core_1.Input("vehicleToBodyStyleConfigsForSelectedBodyStyle"), 
        __metadata('design:type', Array)
    ], VehicleToBodyStyleConfigSearchPanel.prototype, "vehicleToBodyStyleConfigsForSelectedBodyStyle", void 0);
    __decorate([
        core_1.Output("onSearchEvent"), 
        __metadata('design:type', Object)
    ], VehicleToBodyStyleConfigSearchPanel.prototype, "onSearchEvent", void 0);
    VehicleToBodyStyleConfigSearchPanel = __decorate([
        core_1.Component({
            selector: "vehicletobodystyleconfig-searchpanel",
            templateUrl: "app/templates/vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-searchPanel.component.html",
            providers: [vehicleToBodyStyleConfig_service_1.VehicleToBodyStyleConfigService, shared_service_1.SharedService],
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, ng2_toastr_1.ToastsManager, vehicleToBodyStyleConfig_service_1.VehicleToBodyStyleConfigService])
    ], VehicleToBodyStyleConfigSearchPanel);
    return VehicleToBodyStyleConfigSearchPanel;
}());
exports.VehicleToBodyStyleConfigSearchPanel = VehicleToBodyStyleConfigSearchPanel;
