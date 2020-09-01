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
var mfrBodyCode_service_1 = require("./mfrBodyCode.service");
var vehicletomfrbodycode_service_1 = require("../vehicletomfrbodycode/vehicletomfrbodycode.service");
var ac_grid_1 = require('../../lib/aclibs/ac-grid/ac-grid');
var constants_warehouse_1 = require("../constants-warehouse");
var MfrBodyCodeReplaceComponent = (function () {
    function MfrBodyCodeReplaceComponent(mfrBodyCodeService, router, route, toastr, vehicleToMfrBodyCodeService) {
        this.mfrBodyCodeService = mfrBodyCodeService;
        this.router = router;
        this.route = route;
        this.toastr = toastr;
        this.vehicleToMfrBodyCodeService = vehicleToMfrBodyCodeService;
        this.showLoadingGif = false;
        this.vehicleTypeGroupFacet = [];
        this.mfrBodyCodes = [];
        this.thresholdRecordCount = 100000; //NOTE: keep this number large so that "select all" checkbox always appears
        // initialize empty MfrBOdyCode
        this.replaceMfrBodyCode = {
            id: -1
        };
    }
    MfrBodyCodeReplaceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.isSelectAllVehicleToMfrBodyCode = false;
        // Load existing MfrBOdyCode config with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        this.mfrBodyCodes = [];
        this.mfrBodyCodeService.getMfrBodyCode(id).subscribe(function (result) {
            _this.existingMfrBodyCode = result;
            _this.mfrBodyCodeService.getMfrBodyCodes().subscribe(function (mbc) {
                _this.mfrBodyCodes = mbc;
                _this.refreshFacets();
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
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
                mfrBodyCodes: [],
            },
            result: { mfrBodyCodes: [], vehicleToMfrBodyCodes: [] }
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
    MfrBodyCodeReplaceComponent.prototype.validateReplaceMfrBodyCode = function () {
        var isValid = true;
        // check required fields
        if (Number(this.replaceMfrBodyCode.id) === -1) {
            this.toastr.warning("Please select Mfr Body Code.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceMfrBodyCode.id) < 1) {
            this.toastr.warning("Please select replacement Mfr Body Code.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingMfrBodyCode.id) === Number(this.replaceMfrBodyCode.id)) {
            this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingMfrBodyCode.vehicleToMfrBodyCodes.filter(function (item) { return item.isSelected; }).length <= 0) {
            this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    // event on model change in selection of MfrBOdyCode
    MfrBodyCodeReplaceComponent.prototype.onChangeMfrBodyCode = function (mfrBodyCodeId) {
        if (mfrBodyCodeId > 0)
            this.replaceMfrBodyCode.id = mfrBodyCodeId;
        else
            this.replaceMfrBodyCode.id = 0;
    };
    MfrBodyCodeReplaceComponent.prototype.onMfrBodyCodeIdKeyPress = function (event) {
        if (Number(event.keyCode) === 13) {
            this.onMfrBodyCodeIdSearch();
        }
    };
    // event on MfrBodyCode config id search
    MfrBodyCodeReplaceComponent.prototype.onMfrBodyCodeIdSearch = function () {
        var _this = this;
        var mfrBodyCodeId = Number(this.mfrBodyCodeIdSearchText);
        if (isNaN(mfrBodyCodeId) || mfrBodyCodeId <= 0) {
            this.toastr.warning("Invalid Mfr Body Code Id.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.replaceMfrBodyCode.id = -1;
        this.showLoadingGif = true;
        this.mfrBodyCodeService.getMfrBodyCode(Number(this.mfrBodyCodeIdSearchText)).subscribe(function (result) {
            _this.replaceMfrBodyCode = result;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    MfrBodyCodeReplaceComponent.prototype.onSelectAllVehicleToMfrBodyCode = function (isSelected) {
        this.isSelectAllVehicleToMfrBodyCode = isSelected;
        if (this.existingMfrBodyCode.vehicleToMfrBodyCodes == null) {
            return;
        }
        this.existingMfrBodyCode.vehicleToMfrBodyCodes.forEach(function (item) { return item.isSelected = isSelected; });
    };
    MfrBodyCodeReplaceComponent.prototype.onClearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.refreshFacets();
    };
    MfrBodyCodeReplaceComponent.prototype.getDefaultInputModel = function () {
        return {
            mfrBodyCodeId: 0,
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
            mfrBodyCodes: []
        };
    };
    MfrBodyCodeReplaceComponent.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
        inputModel.mfrBodyCodeId = this.existingMfrBodyCode.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.regions) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.makes) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.models) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.subModels.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        this.showLoadingGif = true;
        this.vehicleToMfrBodyCodeService.refreshFacets(inputModel).subscribe(function (data) {
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
    MfrBodyCodeReplaceComponent.prototype.filterMakes = function ($event) {
        if (this.vehicleToMfrBodyCodeSearchViewModel != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    MfrBodyCodeReplaceComponent.prototype.filterModels = function ($event) {
        if (this.vehicleToMfrBodyCodeSearchViewModel != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    MfrBodyCodeReplaceComponent.prototype.filterSubModels = function ($event) {
        if (this.vehicleToMfrBodyCodeSearchViewModel != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    MfrBodyCodeReplaceComponent.prototype.filterRegions = function ($event) {
        if (this.vehicleToMfrBodyCodeSearchViewModel != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    MfrBodyCodeReplaceComponent.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleToMfrBodyCodeSearchViewModel != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    MfrBodyCodeReplaceComponent.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleToMfrBodyCodeSearchViewModel != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    MfrBodyCodeReplaceComponent.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    MfrBodyCodeReplaceComponent.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    MfrBodyCodeReplaceComponent.prototype.updateRegionFacet = function (regions) {
        var existingSelectedRegions = this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToMfrBodyCodeSearchViewModel.facets.regions = [];
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
            this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.push(newItem);
        }
        this.regionFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.slice();
    };
    MfrBodyCodeReplaceComponent.prototype.updateYearFacet = function (years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    MfrBodyCodeReplaceComponent.prototype.updateMakeFacet = function (makes) {
        var existingSelectedMakes = this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.filter(function (make) { return make.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToMfrBodyCodeSearchViewModel.facets.makes = [];
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
            this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.push(newMake);
        }
        this.makeFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.slice();
    };
    //TODO: makeName is not used
    MfrBodyCodeReplaceComponent.prototype.updateModelFacet = function (models, makeName) {
        var existingSelectedModels = this.vehicleToMfrBodyCodeSearchViewModel.facets.models.filter(function (model) { return model.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToMfrBodyCodeSearchViewModel.facets.models = [];
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
            this.vehicleToMfrBodyCodeSearchViewModel.facets.models.push(newModel);
        }
        this.modelFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.models.slice();
    };
    MfrBodyCodeReplaceComponent.prototype.updateSubModelFacet = function (subModels, modelName) {
        var existingSelectedSubModels = this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.filter(function (submodel) { return submodel.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels = [];
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
            this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.push(newSubModel);
        }
        this.subModelFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.slice();
    };
    MfrBodyCodeReplaceComponent.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
        var existingSelectedItems = this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups = [];
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
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }
        this.vehicleTypeGroupFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.slice();
    };
    MfrBodyCodeReplaceComponent.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
        var existingSelectedItems = this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes = [];
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
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.push(newItem);
        }
        this.vehicleTypeFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.slice();
    };
    MfrBodyCodeReplaceComponent.prototype.searchVehicleToMfrBodyCodes = function () {
        var _this = this;
        this.showLoadingGif = true;
        var inputModel = this.getDefaultInputModel();
        inputModel.mfrBodyCodeId = this.existingMfrBodyCode.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.makes) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.makes.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.models) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.models.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.models.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.filter(function (item) { return item.isSelected; })
                .forEach(function (s) { return inputModel.subModels.push(s.name); });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypes.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.vehicleTypeGroups.push(m.name); });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.regions) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.regions.push(m.name); });
        }
        this.vehicleToMfrBodyCodeService.getAssociations(inputModel).subscribe(function (result) {
            if (result.length > 0) {
                _this.existingMfrBodyCode.vehicleToMfrBodyCodes = result;
                _this.existingMfrBodyCode.vehicleToMfrBodyCodeCount = result.length;
                _this.isSelectAllVehicleToMfrBodyCode = false;
                if (_this.vehicleToMfrBodyCodeGrid)
                    _this.vehicleToMfrBodyCodeGrid.refresh();
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
    MfrBodyCodeReplaceComponent.prototype.onViewAffectedAssociations = function () {
        $(".drawer-left").css('width', 800); //show the filter panel to enable search
    };
    MfrBodyCodeReplaceComponent.prototype.clearAllFacets = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.regions) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.makes) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.models) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.models.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.forEach(function (item) { return item.isSelected = false; });
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.forEach(function (item) { return item.isSelected = false; });
        }
        this.refreshFacets();
    };
    MfrBodyCodeReplaceComponent.prototype.clearFacet = function (facet, refresh) {
        if (refresh === void 0) { refresh = true; }
        if (facet) {
            facet.forEach(function (item) { return item.isSelected = false; });
        }
        if (refresh) {
            this.refreshFacets();
        }
    };
    MfrBodyCodeReplaceComponent.prototype.onVehicleToMfrBodyCodeSelected = function (vechicleToMfrBodyCode) {
        if (vechicleToMfrBodyCode.isSelected) {
            //unchecked
            this.isSelectAllVehicleToMfrBodyCode = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingMfrBodyCode.vehicleToMfrBodyCodes.filter(function (item) { return item.id != vechicleToMfrBodyCode.id; });
            if (excludedVehicle.every(function (item) { return item.isSelected; })) {
                this.isSelectAllVehicleToMfrBodyCode = true;
            }
        }
    };
    // event on continue
    MfrBodyCodeReplaceComponent.prototype.onContinue = function () {
        var _this = this;
        if (this.validateReplaceMfrBodyCode()) {
            this.mfrBodyCodeService.existingMfrBodyCode = this.existingMfrBodyCode;
            this.mfrBodyCodeService.existingMfrBodyCode.vehicleToMfrBodyCodes = this.existingMfrBodyCode.vehicleToMfrBodyCodes.filter(function (item) { return item.isSelected; });
            this.replaceMfrBodyCode.name = this.mfrBodyCodes.filter(function (item) { return item.id === Number(_this.replaceMfrBodyCode.id); })[0].name;
            this.mfrBodyCodeService.replacementMfrBodyCode = this.replaceMfrBodyCode;
            this.router.navigateByUrl("/mfrbodycode/replace/confirm/" + this.existingMfrBodyCode.id);
        }
    };
    __decorate([
        //NOTE: keep this number large so that "select all" checkbox always appears
        core_1.ViewChild("vehicleToMfrBodyCodeGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], MfrBodyCodeReplaceComponent.prototype, "vehicleToMfrBodyCodeGrid", void 0);
    MfrBodyCodeReplaceComponent = __decorate([
        core_1.Component({
            selector: "mfrBodyCode-replace-component",
            templateUrl: "app/templates/mfrBodyCode/mfrBodyCode-replace.component.html",
            providers: [vehicletomfrbodycode_service_1.VehicleToMfrBodyCodeService]
        }), 
        __metadata('design:paramtypes', [mfrBodyCode_service_1.MfrBodyCodeService, router_1.Router, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager, vehicletomfrbodycode_service_1.VehicleToMfrBodyCodeService])
    ], MfrBodyCodeReplaceComponent);
    return MfrBodyCodeReplaceComponent;
}());
exports.MfrBodyCodeReplaceComponent = MfrBodyCodeReplaceComponent;
