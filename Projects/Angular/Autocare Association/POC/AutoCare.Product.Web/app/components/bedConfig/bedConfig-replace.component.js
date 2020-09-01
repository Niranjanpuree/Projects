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
var bedType_service_1 = require("../bedType/bedType.service");
var bedLength_service_1 = require("../bedLength/bedLength.service");
var bedConfig_service_1 = require("./bedConfig.service");
var constants_warehouse_1 = require("../constants-warehouse");
var vehicleToBedConfig_service_1 = require("../vehicleToBedConfig/vehicleToBedConfig.service");
var ac_grid_1 = require('../../lib/aclibs/ac-grid/ac-grid');
var BedConfigReplaceComponent = (function () {
    function BedConfigReplaceComponent(bedTypeService, bedConfigService, bedLengthService, router, route, toastr, vehicleToBedConfigService) {
        this.bedTypeService = bedTypeService;
        this.bedConfigService = bedConfigService;
        this.bedLengthService = bedLengthService;
        this.router = router;
        this.route = route;
        this.toastr = toastr;
        this.vehicleToBedConfigService = vehicleToBedConfigService;
        this.showLoadingGif = false;
        this.vehicleTypeGroupFacet = [];
        this.thresholdRecordCount = 100000; //NOTE: keep this number large so that "select all" checkbox always appears
        // initialize empty bed config
        this.replaceBedConfig = {
            id: 0,
            bedLengthId: -1,
            length: "",
            bedLengthMetric: "",
            bedTypeId: -1,
            bedTypeName: "",
            isSelected: false
        };
    }
    BedConfigReplaceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.isSelectAllVehicleToBedConfig = false;
        // Load existing bed config with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        this.bedLengths = [];
        this.bedTypes = [];
        this.bedConfigService.getBedConfig(id).subscribe(function (result) {
            _this.existingBedConfig = result;
            _this.bedLengthService.getAllBedLengths().subscribe(function (bl) {
                _this.bedLengths = bl;
                _this.refreshFacets();
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            }); //bed length
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        }); //bed config
        this.vehicleToBedConfigSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: [],
                bedTypes: [],
                bedLengths: [],
            },
            result: { bedConfigs: [], vehicleToBedConfigs: [] }
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
    BedConfigReplaceComponent.prototype.validateReplaceBedConfig = function () {
        var isValid = true;
        // check required fields
        if (Number(this.replaceBedConfig.bedLengthId) === -1) {
            this.toastr.warning("Please select Bed length.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBedConfig.bedTypeId) === -1) {
            this.toastr.warning("Please select Bed type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBedConfig.id) < 1) {
            this.toastr.warning("Please select replacement Bed config system.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingBedConfig.id) === Number(this.replaceBedConfig.id)) {
            this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingBedConfig.vehicleToBedConfigs.filter(function (item) { return item.isSelected; }).length <= 0) {
            this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    // event on model change in selection of Bed Type
    BedConfigReplaceComponent.prototype.onChangeBedType = function (bedTypeId, bedLengthId) {
        var _this = this;
        this.replaceBedConfig.id = null;
        if (bedTypeId > 0 && bedLengthId > 0) {
            this.bedConfigService.getByChildIds(bedLengthId, bedTypeId).subscribe(function (bc) {
                _this.replaceBedConfig = bc;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            });
        }
        else {
            this.replaceBedConfig.id = 0;
        }
    };
    // event on model change in selection of Bed Length
    BedConfigReplaceComponent.prototype.onChangeBedLength = function (bedLengthId) {
        var _this = this;
        this.replaceBedConfig.bedTypeId = -1;
        if (this.replaceBedConfig.bedLengthId == -1) {
            this.bedTypes = [];
            this.replaceBedConfig.id = 0;
            return;
        }
        this.bedTypes = null;
        this.bedTypeService.getByBedLengthId(bedLengthId).subscribe(function (bt) {
            _this.bedTypes = bt;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
        }); // bed types
    };
    BedConfigReplaceComponent.prototype.onBedConfigIdKeyPress = function (event) {
        if (Number(event.keyCode) === 13) {
            this.onBedConfigIdSearch();
        }
    };
    // event on bed config id search
    BedConfigReplaceComponent.prototype.onBedConfigIdSearch = function () {
        var _this = this;
        var bedConfigId = Number(this.bedConfigIdSearchText);
        if (isNaN(bedConfigId) || bedConfigId <= 0) {
            this.toastr.warning("Invalid Bed Config Id.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        // empty replace bed config
        this.replaceBedConfig = {
            id: 0,
            bedLengthId: -1,
            length: "",
            bedLengthMetric: "",
            bedTypeId: -1,
            bedTypeName: "",
            isSelected: false
        };
        this.bedTypes = null;
        this.showLoadingGif = true;
        this.bedConfigService.getBedConfig(Number(this.bedConfigIdSearchText)).subscribe(function (result) {
            _this.bedTypeService.getByBedLengthId(result.bedLengthId).subscribe(function (a) {
                _this.bedTypes = a;
                _this.bedConfigService.getByChildIds(result.bedLengthId, result.bedTypeId).subscribe(function (result) {
                    _this.replaceBedConfig = result;
                    _this.showLoadingGif = false;
                }, function (error) {
                    _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                    _this.showLoadingGif = false;
                });
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    BedConfigReplaceComponent.prototype.onSelectAllVehicleToBedConfig = function (isSelected) {
        this.isSelectAllVehicleToBedConfig = isSelected;
        if (this.existingBedConfig.vehicleToBedConfigs == null) {
            return;
        }
        this.existingBedConfig.vehicleToBedConfigs.forEach(function (item) { return item.isSelected = isSelected; });
    };
    BedConfigReplaceComponent.prototype.onClearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.refreshFacets();
    };
    BedConfigReplaceComponent.prototype.getDefaultInputModel = function () {
        return {
            bedConfigId: 0,
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
    BedConfigReplaceComponent.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
        inputModel.bedConfigId = this.existingBedConfig.id;
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
        this.showLoadingGif = true;
        this.vehicleToBedConfigService.refreshFacets(inputModel).subscribe(function (data) {
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
    BedConfigReplaceComponent.prototype.filterMakes = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleToBedConfigSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BedConfigReplaceComponent.prototype.filterModels = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleToBedConfigSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BedConfigReplaceComponent.prototype.filterSubModels = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleToBedConfigSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BedConfigReplaceComponent.prototype.filterRegions = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleToBedConfigSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BedConfigReplaceComponent.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BedConfigReplaceComponent.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BedConfigReplaceComponent.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    BedConfigReplaceComponent.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    BedConfigReplaceComponent.prototype.updateRegionFacet = function (regions) {
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
    BedConfigReplaceComponent.prototype.updateYearFacet = function (years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    BedConfigReplaceComponent.prototype.updateMakeFacet = function (makes) {
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
    BedConfigReplaceComponent.prototype.updateModelFacet = function (models, makeName) {
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
    BedConfigReplaceComponent.prototype.updateSubModelFacet = function (subModels, modelName) {
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
    BedConfigReplaceComponent.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
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
    BedConfigReplaceComponent.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
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
    BedConfigReplaceComponent.prototype.searchVehicleToBedConfigs = function () {
        var _this = this;
        this.showLoadingGif = true;
        var inputModel = this.getDefaultInputModel();
        inputModel.bedConfigId = this.existingBedConfig.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        //NOTE: inputModel.bedConfigId = this.existingBedConfig.id; should be sufficient here
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
        this.vehicleToBedConfigService.getAssociations(inputModel).subscribe(function (result) {
            if (result.length > 0) {
                _this.existingBedConfig.vehicleToBedConfigs = result;
                _this.existingBedConfig.vehicleToBedConfigCount = result.length;
                _this.isSelectAllVehicleToBedConfig = false;
                if (_this.vehicleToBedConfigGrid)
                    _this.vehicleToBedConfigGrid.refresh();
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
    BedConfigReplaceComponent.prototype.onViewAffectedAssociations = function () {
        $(".drawer-left").css('width', 800); //show the filter panel to enable search
    };
    BedConfigReplaceComponent.prototype.clearAllFacets = function () {
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
        this.refreshFacets();
    };
    BedConfigReplaceComponent.prototype.clearFacet = function (facet, refresh) {
        if (refresh === void 0) { refresh = true; }
        if (facet) {
            facet.forEach(function (item) { return item.isSelected = false; });
        }
        if (refresh) {
            this.refreshFacets();
        }
    };
    BedConfigReplaceComponent.prototype.onVehicleToBedConfigSelected = function (vechicleToBedConfig) {
        if (vechicleToBedConfig.isSelected) {
            //unchecked
            this.isSelectAllVehicleToBedConfig = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingBedConfig.vehicleToBedConfigs.filter(function (item) { return item.id != vechicleToBedConfig.id; });
            if (excludedVehicle.every(function (item) { return item.isSelected; })) {
                this.isSelectAllVehicleToBedConfig = true;
            }
        }
    };
    // event on continue
    BedConfigReplaceComponent.prototype.onContinue = function () {
        var _this = this;
        if (this.validateReplaceBedConfig()) {
            this.bedConfigService.existingBedConfig = this.existingBedConfig;
            this.bedConfigService.existingBedConfig.vehicleToBedConfigs = this.existingBedConfig.vehicleToBedConfigs.filter(function (item) { return item.isSelected; });
            this.replaceBedConfig.length = this.bedLengths.filter(function (item) { return item.id === Number(_this.replaceBedConfig.bedLengthId); })[0].name;
            this.replaceBedConfig.bedTypeName = this.bedTypes.filter(function (item) { return item.id === Number(_this.replaceBedConfig.bedTypeId); })[0].name;
            this.bedConfigService.replacementBedConfig = this.replaceBedConfig;
            this.router.navigateByUrl("/bedconfig/replace/confirm/" + this.existingBedConfig.id);
        }
    };
    __decorate([
        //NOTE: keep this number large so that "select all" checkbox always appears
        core_1.ViewChild("vehicleToBedConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], BedConfigReplaceComponent.prototype, "vehicleToBedConfigGrid", void 0);
    BedConfigReplaceComponent = __decorate([
        core_1.Component({
            selector: "bedConfig-replace-component",
            templateUrl: "app/templates/bedConfig/bedConfig-replace.component.html",
            providers: [vehicleToBedConfig_service_1.VehicleToBedConfigService],
        }), 
        __metadata('design:paramtypes', [bedType_service_1.BedTypeService, bedConfig_service_1.BedConfigService, bedLength_service_1.BedLengthService, router_1.Router, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager, vehicleToBedConfig_service_1.VehicleToBedConfigService])
    ], BedConfigReplaceComponent);
    return BedConfigReplaceComponent;
}());
exports.BedConfigReplaceComponent = BedConfigReplaceComponent;
