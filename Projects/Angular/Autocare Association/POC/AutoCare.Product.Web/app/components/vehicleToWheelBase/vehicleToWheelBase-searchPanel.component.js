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
var vehicleToWheelBase_search_model_1 = require("../vehicleToWheelBase/vehicleToWheelBase-search.model");
var vehicleToWheelBase_service_1 = require("../vehicleTowheelBase/vehicleToWheelBase.service");
var VehicleToWheelBaseSearchPanel = (function () {
    function VehicleToWheelBaseSearchPanel(sharedService, toastr, vehicleToWheelBaseService) {
        this.sharedService = sharedService;
        this.toastr = toastr;
        this.vehicleToWheelBaseService = vehicleToWheelBaseService;
        this.vehicleTypeGroupFacet = [];
        this.vehicleToWheelBaseRetrieved = [];
        this.showLoadingGif = false;
        this.onSearchEvent = new core_1.EventEmitter();
    }
    VehicleToWheelBaseSearchPanel.prototype.ngOnInit = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.wheelBaseId = "";
        if (this.sharedService.vehicleToWheelBaseSearchViewModel != null) {
            this.vehicleToWheelBaseSearchViewModel.facets = this.sharedService.vehicleToWheelBaseSearchViewModel.facets;
            this.regionFacet = this.vehicleToWheelBaseSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleToWheelBaseSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleToWheelBaseSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleToWheelBaseSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleToWheelBaseSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleToWheelBaseSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.slice();
            if (this.sharedService.vehicleToWheelBaseSearchViewModel.searchType == vehicleToWheelBase_search_model_1.SearchType.SearchByWheelBaseId) {
                this.searchByWheelBaseId();
            }
            else if (this.sharedService.vehicleToWheelBaseSearchViewModel.searchType == vehicleToWheelBase_search_model_1.SearchType.GeneralSearch) {
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
    VehicleToWheelBaseSearchPanel.prototype.onClearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.wheelBaseId = "";
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
    VehicleToWheelBaseSearchPanel.prototype.getDefaultInputModel = function () {
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
    VehicleToWheelBaseSearchPanel.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
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
    VehicleToWheelBaseSearchPanel.prototype.filterMakes = function ($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleToWheelBaseSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToWheelBaseSearchPanel.prototype.filterModels = function ($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleToWheelBaseSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToWheelBaseSearchPanel.prototype.filterSubModels = function ($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleToWheelBaseSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToWheelBaseSearchPanel.prototype.filterRegions = function ($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleToWheelBaseSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToWheelBaseSearchPanel.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToWheelBaseSearchPanel.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToWheelBaseSearchPanel.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    VehicleToWheelBaseSearchPanel.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    VehicleToWheelBaseSearchPanel.prototype.updateRegionFacet = function (regions) {
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
    VehicleToWheelBaseSearchPanel.prototype.updateYearFacet = function (years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    VehicleToWheelBaseSearchPanel.prototype.updateMakeFacet = function (makes) {
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
    VehicleToWheelBaseSearchPanel.prototype.updateModelFacet = function (models, makeName) {
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
    VehicleToWheelBaseSearchPanel.prototype.updateSubModelFacet = function (subModels, modelName) {
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
    VehicleToWheelBaseSearchPanel.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
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
    VehicleToWheelBaseSearchPanel.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
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
    VehicleToWheelBaseSearchPanel.prototype.searchByWheelBaseId = function () {
        var _this = this;
        var wheelBaseId = Number(this.wheelBaseId);
        if (isNaN(wheelBaseId)) {
            this.toastr.warning("Invalid Wheel Base Id", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.vehicleToWheelBaseSearchViewModel.searchType = vehicleToWheelBase_search_model_1.SearchType.SearchByWheelBaseId;
        this.showLoadingGif = true;
        this.vehicleToWheelBaseService.searchByWheelBaseId(wheelBaseId).subscribe(function (m) {
            if (m.result.wheelBases.length > 0) {
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
    VehicleToWheelBaseSearchPanel.prototype.onWheelBaseKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.searchByWheelBaseId();
        }
    };
    VehicleToWheelBaseSearchPanel.prototype.getSearchResult = function (m) {
        var _this = this;
        this.vehicleToWheelBaseSearchViewModel.result = m.result;
        this.vehicleToWheelBaseSearchViewModel.totalCount = m.totalCount;
        this.vehicleToWheelBaseForSelectedWheelBase = [];
        this.isSelectAllWheelBaseSystems = false;
        // note: select all when totalRecords <= threshold
        if (this.vehicleToWheelBaseSearchViewModel.result.wheelBases.length <= this.thresholdRecordCount) {
            this.vehicleToWheelBaseSearchViewModel.result.wheelBases.forEach(function (item) {
                item.isSelected = true;
                _this.refreshAssociationWithWheelBaseId(item.id, item.isSelected);
            });
            this.isSelectAllWheelBaseSystems = true;
        }
        // callback emitter
        this.onSearchEvent.emit(this.vehicleToWheelBaseForSelectedWheelBase);
    };
    // todo: make is modular, this method is at two location
    VehicleToWheelBaseSearchPanel.prototype.refreshAssociationWithWheelBaseId = function (wheelBaseConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToWheelBaseRetrieved = this.getVehicleToWheelBaseByWheelBaseId(wheelBaseConfigId);
            //TODO: number of associations which may be useful in add Wheel BAse association screen?
            var temp = this.vehicleToWheelBaseForSelectedWheelBase || [];
            for (var _i = 0, _a = this.vehicleToWheelBaseRetrieved; _i < _a.length; _i++) {
                var vehicleToWheelBase = _a[_i];
                temp.push(vehicleToWheelBase);
            }
            this.vehicleToWheelBaseForSelectedWheelBase = temp;
        }
        else {
            var m = this.vehicleToWheelBaseForSelectedWheelBase.filter(function (x) { return x.wheelBaseId != wheelBaseConfigId; });
            this.vehicleToWheelBaseForSelectedWheelBase = m;
        }
    };
    VehicleToWheelBaseSearchPanel.prototype.getVehicleToWheelBaseByWheelBaseId = function (id) {
        return this.vehicleToWheelBaseSearchViewModel.result.vehicleToWheelBases.filter(function (v) { return v.wheelBaseId == id; });
    };
    VehicleToWheelBaseSearchPanel.prototype.search = function () {
        var _this = this;
        this.vehicleToWheelBaseSearchViewModel.searchType = vehicleToWheelBase_search_model_1.SearchType.GeneralSearch;
        this.showLoadingGif = true;
        var inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
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
        this.vehicleToWheelBaseService.search(inputModel).subscribe(function (m) {
            if (m.result.wheelBases.length > 0 || m.result.vehicleToWheelBase.length > 0) {
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
    VehicleToWheelBaseSearchPanel.prototype.clearFacet = function (facet, refresh) {
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
    ], VehicleToWheelBaseSearchPanel.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.Input("vehicleToWheelBaseSearchViewModel"), 
        __metadata('design:type', Object)
    ], VehicleToWheelBaseSearchPanel.prototype, "vehicleToWheelBaseSearchViewModel", void 0);
    __decorate([
        core_1.Input("vehicleToWheelBaseForSelectedWheelBase"), 
        __metadata('design:type', Array)
    ], VehicleToWheelBaseSearchPanel.prototype, "vehicleToWheelBaseForSelectedWheelBase", void 0);
    __decorate([
        core_1.Output("onSearchEvent"), 
        __metadata('design:type', Object)
    ], VehicleToWheelBaseSearchPanel.prototype, "onSearchEvent", void 0);
    VehicleToWheelBaseSearchPanel = __decorate([
        core_1.Component({
            selector: "vehicletowheelbase-searchpanel",
            templateUrl: "app/templates/VehicleToWheelBase/vehicleToWheelBase-searchPanel.component.html",
            providers: [vehicleToWheelBase_service_1.VehicleToWheelBaseService],
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, ng2_toastr_1.ToastsManager, vehicleToWheelBase_service_1.VehicleToWheelBaseService])
    ], VehicleToWheelBaseSearchPanel);
    return VehicleToWheelBaseSearchPanel;
}());
exports.VehicleToWheelBaseSearchPanel = VehicleToWheelBaseSearchPanel;
