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
var vehicleToMfrBodyCode_search_model_1 = require("../vehicleToMfrBodyCode/vehicleToMfrBodyCode-search.model");
var vehicleToMfrBodyCode_service_1 = require("../vehicleToMfrBodyCode/vehicleToMfrBodyCode.service");
var VehicleToMfrBodyCodeSearchPanel = (function () {
    function VehicleToMfrBodyCodeSearchPanel(sharedService, toastr, vehicleToMfrBodyCodesService) {
        this.sharedService = sharedService;
        this.toastr = toastr;
        this.vehicleToMfrBodyCodesService = vehicleToMfrBodyCodesService;
        this.vehicleTypeGroupFacet = [];
        this.vehicleToMfrBodyCodesRetrieved = [];
        this.showLoadingGif = false;
        this.onSearchEvent = new core_1.EventEmitter();
    }
    VehicleToMfrBodyCodeSearchPanel.prototype.ngOnInit = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.mfrBodyCodeId = "";
        if (this.sharedService.vehicleToMfrBodyCodeSearchViewModel != null) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets = this.sharedService.vehicleToMfrBodyCodeSearchViewModel.facets;
            this.regionFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.slice();
            if (this.sharedService.vehicleToMfrBodyCodeSearchViewModel.searchType == vehicleToMfrBodyCode_search_model_1.SearchType.SearchByMfrBodyCodeId) {
                this.searchByMfrBodyCodeId();
            }
            else if (this.sharedService.vehicleToMfrBodyCodeSearchViewModel.searchType == vehicleToMfrBodyCode_search_model_1.SearchType.GeneralSearch) {
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
    VehicleToMfrBodyCodeSearchPanel.prototype.onClearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.mfrBodyCodeId = "";
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.regions) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.makes) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.models) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.models.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.forEach(function (item) { return item.isSelected = false; });
        }
        this.refreshFacets();
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.getDefaultInputModel = function () {
        return {
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
            mfrBodyCodes: [],
        };
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.regions) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.makes) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.models) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.subModels.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        this.showLoadingGif = true;
        this.vehicleToMfrBodyCodesService.refreshFacets(inputModel).subscribe(function (data) {
            _this.updateRegionFacet(data.facets.regions);
            _this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
            _this.updateVehicleTypeFacet(data.facets.vehicleTypes);
            _this.updateYearFacet(data.facets.years);
            _this.updateMakeFacet(data.facets.makes);
            _this.updateModelFacet(data.facets.models, "");
            _this.updateSubModelFacet(data.facets.subModels, "");
            //this.updateMfrBodyCodeFacet(data.facets.mfrBodyCodes);
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.filterMakes = function ($event) {
        if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.filterModels = function ($event) {
        if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.filterSubModels = function ($event) {
        if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.filterRegions = function ($event) {
        if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    //private filterMfrBodyCode($event) {
    //    if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
    //        this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
    //        this.vehicleToMfrBodyCodesSearchViewModel.facets.mfrBodyCodes != null) {
    //        var inputElement = <HTMLInputElement>$event.target;
    //        this.mfrBodyCodeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.mfrBodyCodes.filter(
    //            item => item.name.toLocaleLowerCase()
    //            .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
    //    }
    //}
    VehicleToMfrBodyCodeSearchPanel.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.updateRegionFacet = function (regions) {
        var existingSelectedRegions = this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToMfrBodyCodesSearchViewModel.facets.regions = [];
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
            this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.push(newItem);
        }
        this.regionFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.slice();
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.updateYearFacet = function (years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.updateMakeFacet = function (makes) {
        var existingSelectedMakes = this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.filter(function (make) { return make.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToMfrBodyCodesSearchViewModel.facets.makes = [];
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
            this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.push(newMake);
        }
        this.makeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.slice();
    };
    //TODO: makeName is not used
    VehicleToMfrBodyCodeSearchPanel.prototype.updateModelFacet = function (models, makeName) {
        var existingSelectedModels = this.vehicleToMfrBodyCodesSearchViewModel.facets.models.filter(function (model) { return model.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToMfrBodyCodesSearchViewModel.facets.models = [];
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
            this.vehicleToMfrBodyCodesSearchViewModel.facets.models.push(newModel);
        }
        this.modelFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.models.slice();
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.updateSubModelFacet = function (subModels, modelName) {
        var existingSelectedSubModels = this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.filter(function (submodel) { return submodel.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels = [];
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
            this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.push(newSubModel);
        }
        this.subModelFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.slice();
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
        var existingSelectedItems = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups = [];
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
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }
        this.vehicleTypeGroupFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.slice();
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
        var existingSelectedItems = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes = [];
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
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.push(newItem);
        }
        this.vehicleTypeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.slice();
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.searchByMfrBodyCodeId = function () {
        var _this = this;
        var mfrBodyCodeId = Number(this.mfrBodyCodeId);
        if (isNaN(mfrBodyCodeId)) {
            this.toastr.warning("Invalid Mfr Body Code Id", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.vehicleToMfrBodyCodesSearchViewModel.searchType = vehicleToMfrBodyCode_search_model_1.SearchType.SearchByMfrBodyCodeId;
        this.showLoadingGif = true;
        this.vehicleToMfrBodyCodesService.searchByMfrBodyCodeId(mfrBodyCodeId).subscribe(function (m) {
            if (m.result.mfrBodyCodes.length > 0) {
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
    VehicleToMfrBodyCodeSearchPanel.prototype.onMfrBodyCodesKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.searchByMfrBodyCodeId();
        }
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.getSearchResult = function (m) {
        var _this = this;
        this.vehicleToMfrBodyCodesSearchViewModel.result = m.result;
        this.vehicleToMfrBodyCodesSearchViewModel.totalCount = m.totalCount;
        this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = [];
        this.isSelectAllMfrBodyCodes = false;
        // note: select all when totalRecords <= threshold
        if (this.vehicleToMfrBodyCodesSearchViewModel.result.mfrBodyCodes.length <= this.thresholdRecordCount) {
            this.vehicleToMfrBodyCodesSearchViewModel.result.mfrBodyCodes.forEach(function (item) {
                item.isSelected = true;
                _this.refreshAssociationWithMfrBodyCodeId(item.id, item.isSelected);
            });
            this.isSelectAllMfrBodyCodes = true;
        }
        // callback emitter
        this.onSearchEvent.emit(this.vehicleToMfrBodyCodesForSelectedMfrBodyCode);
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.refreshAssociationWithMfrBodyCodeId = function (mfrBodyCodeId, isSelected) {
        if (isSelected) {
            this.vehicleToMfrBodyCodesRetrieved = this.getVehicleToMfrBodyCodesByMfrBodyCodeId(mfrBodyCodeId);
            //TODO: number of associations which may be useful in add brake association screen?
            var temp = this.vehicleToMfrBodyCodesForSelectedMfrBodyCode || [];
            for (var _i = 0, _a = this.vehicleToMfrBodyCodesRetrieved; _i < _a.length; _i++) {
                var vehicleToMfrBodyCode = _a[_i];
                temp.push(vehicleToMfrBodyCode);
            }
            this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = temp;
        }
        else {
            var m = this.vehicleToMfrBodyCodesForSelectedMfrBodyCode.filter(function (x) { return x.mfrBodyCode.id != mfrBodyCodeId; });
            this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = m;
        }
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.getVehicleToMfrBodyCodesByMfrBodyCodeId = function (id) {
        return this.vehicleToMfrBodyCodesSearchViewModel.result.vehicleToMfrBodyCodes.filter(function (v) { return v.mfrBodyCode.id == id; });
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.search = function () {
        var _this = this;
        this.vehicleToMfrBodyCodesSearchViewModel.searchType = vehicleToMfrBodyCode_search_model_1.SearchType.GeneralSearch;
        this.showLoadingGif = true;
        var inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.mfrBodyCodes) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.mfrBodyCodes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.mfrBodyCodes.push(m.name); });
            if (this.vehicleToMfrBodyCodesSearchViewModel.facets.makes) {
                this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                    .forEach(function (m) { return inputModel.makes.push(m.name); });
            }
            if (this.vehicleToMfrBodyCodesSearchViewModel.facets.models) {
                this.vehicleToMfrBodyCodesSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                    .forEach(function (m) { return inputModel.models.push(m.name); });
            }
            if (this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels) {
                this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                    .forEach(function (s) { return inputModel.subModels.push(s.name); });
            }
            if (this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes) {
                this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                    .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
            }
            if (this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups) {
                this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                    .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
            }
            if (this.vehicleToMfrBodyCodesSearchViewModel.facets.regions) {
                this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                    .forEach(function (m) { return inputModel.regions.push(m.name); });
            }
            this.vehicleToMfrBodyCodesService.search(inputModel).subscribe(function (m) {
                if (m.result.mfrBodyCodes.length > 0 || m.result.vehicleToMfrBodyCodes.length > 0) {
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
        }
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.clearFacet = function (facet, refresh) {
        if (refresh === void 0) { refresh = true; }
        if (facet) {
            facet.forEach(function (item) { return item.isSelected = false; });
        }
        if (refresh) {
            this.refreshFacets();
        }
    };
    VehicleToMfrBodyCodeSearchPanel.prototype.onMfrBodyCodeKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.searchByMfrBodyCodeId();
        }
    };
    __decorate([
        core_1.Input("thresholdRecordCount"), 
        __metadata('design:type', Number)
    ], VehicleToMfrBodyCodeSearchPanel.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.Input("vehicleToMfrBodyCodesSearchViewModel"), 
        __metadata('design:type', Object)
    ], VehicleToMfrBodyCodeSearchPanel.prototype, "vehicleToMfrBodyCodesSearchViewModel", void 0);
    __decorate([
        core_1.Input("vehicleToMfrBodyCodesForSelectedMfrBodyCode"), 
        __metadata('design:type', Array)
    ], VehicleToMfrBodyCodeSearchPanel.prototype, "vehicleToMfrBodyCodesForSelectedMfrBodyCode", void 0);
    __decorate([
        core_1.Output("onSearchEvent"), 
        __metadata('design:type', Object)
    ], VehicleToMfrBodyCodeSearchPanel.prototype, "onSearchEvent", void 0);
    VehicleToMfrBodyCodeSearchPanel = __decorate([
        core_1.Component({
            selector: "vehicletomfrbodycode-searchpanel",
            templateUrl: "app/templates/vehicleToMfrBodyCode/vehicleToMfrBodyCode-searchPanel.component.html",
            providers: [vehicleToMfrBodyCode_service_1.VehicleToMfrBodyCodeService],
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, ng2_toastr_1.ToastsManager, vehicleToMfrBodyCode_service_1.VehicleToMfrBodyCodeService])
    ], VehicleToMfrBodyCodeSearchPanel);
    return VehicleToMfrBodyCodeSearchPanel;
}());
exports.VehicleToMfrBodyCodeSearchPanel = VehicleToMfrBodyCodeSearchPanel;
