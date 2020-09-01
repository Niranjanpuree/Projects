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
var brakeType_service_1 = require("../brakeType/brakeType.service");
var brakeABS_service_1 = require("../brakeABS/brakeABS.service");
var brakeSystem_service_1 = require("../brakeSystem/brakeSystem.service");
var brakeConfig_service_1 = require("./brakeConfig.service");
var constants_warehouse_1 = require("../constants-warehouse");
var vehicleToBrakeConfig_service_1 = require("../vehicleToBrakeConfig/vehicleToBrakeConfig.service");
var ac_grid_1 = require('../../lib/aclibs/ac-grid/ac-grid');
var BrakeConfigReplaceComponent = (function () {
    function BrakeConfigReplaceComponent(brakeABSService, brakeConfigService, brakeSystemService, brakeTypeSerivce, router, route, toastr, vehicleToBrakeConfigService) {
        this.brakeABSService = brakeABSService;
        this.brakeConfigService = brakeConfigService;
        this.brakeSystemService = brakeSystemService;
        this.brakeTypeSerivce = brakeTypeSerivce;
        this.router = router;
        this.route = route;
        this.toastr = toastr;
        this.vehicleToBrakeConfigService = vehicleToBrakeConfigService;
        this.showLoadingGif = false;
        this.vehicleTypeGroupFacet = [];
        this.thresholdRecordCount = 100000; //NOTE: keep this number large so that "select all" checkbox always appears
        // initialize empty brake config
        this.replaceBrakeConfig = {
            id: null,
            frontBrakeTypeId: -1,
            frontBrakeTypeName: "",
            rearBrakeTypeId: -1,
            rearBrakeTypeName: "",
            brakeABSId: -1,
            brakeABSName: "",
            brakeSystemId: -1,
            brakeSystemName: "",
            isSelected: false
        };
    }
    BrakeConfigReplaceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.isSelectAllVehicleToBrakeConfig = false;
        // Load existing brake config with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        this.brakeConfigService.getBrakeConfig(id).subscribe(function (result) {
            _this.existingBrakeConfig = result;
            // Load select options for replace.
            _this.brakeTypeSerivce.getAllBrakeTypes().subscribe(function (bt) {
                _this.frontBrakeTypes = bt;
                _this.refreshFacets();
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            }); // front brake types
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
        this.rearBrakeTypes = [];
        this.brakeABSes = [];
        this.brakeSystems = [];
        this.vehicleToBrakeConfigSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
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
            },
            result: { brakeConfigs: [], vehicleToBrakeConfigs: [] }
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
    BrakeConfigReplaceComponent.prototype.validateReplaceBrakeConfig = function () {
        var isValid = true;
        // check required fields
        if (Number(this.replaceBrakeConfig.frontBrakeTypeId) === -1) {
            this.toastr.warning("Please select Front brake type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBrakeConfig.rearBrakeTypeId) === -1) {
            this.toastr.warning("Please select Rear brake type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBrakeConfig.brakeABSId) === -1) {
            this.toastr.warning("Please select Brake ABS.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBrakeConfig.brakeSystemId) === -1) {
            this.toastr.warning("Please select Brake system.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBrakeConfig.id) < 1) {
            this.toastr.warning("Please select replacement Brake config system.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingBrakeConfig.id) === Number(this.replaceBrakeConfig.id)) {
            this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingBrakeConfig.vehicleToBrakeConfigs != undefined) {
            if (this.existingBrakeConfig.vehicleToBrakeConfigs.filter(function (item) { return item.isSelected; }).length <= 0) {
                this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                isValid = false;
            }
        }
        else if (this.existingBrakeConfig.vehicleToBrakeConfigs == undefined) {
            this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    // event on model change in selection of Front brake type
    BrakeConfigReplaceComponent.prototype.onChangeFrontBrakeType = function (frontBrakeTypeId) {
        var _this = this;
        this.replaceBrakeConfig.brakeABSId = -1;
        this.brakeABSes = [];
        this.replaceBrakeConfig.brakeSystemId = -1;
        this.brakeSystems = [];
        this.replaceBrakeConfig.rearBrakeTypeId = -1;
        if (this.replaceBrakeConfig.frontBrakeTypeId == -1) {
            this.rearBrakeTypes = [];
            return;
        }
        this.rearBrakeTypes = null;
        this.brakeTypeSerivce.getByFrontBrakeTypeId(frontBrakeTypeId).subscribe(function (bt) {
            _this.rearBrakeTypes = bt;
        }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); }); // rear brake types
    };
    // event on model change in selection of Rear brake type
    BrakeConfigReplaceComponent.prototype.onChangeRearBrakeType = function (frontBrakeTypeId, rearBrakeTypeId) {
        var _this = this;
        this.replaceBrakeConfig.brakeSystemId = -1;
        this.brakeSystems = [];
        this.replaceBrakeConfig.brakeABSId = -1;
        if (this.replaceBrakeConfig.rearBrakeTypeId == -1) {
            this.brakeABSes = [];
            return;
        }
        this.brakeABSes = null;
        this.brakeABSService.getByFrontBrakeTypeIdRearBrakeTypeId(frontBrakeTypeId, rearBrakeTypeId).subscribe(function (babs) {
            _this.brakeABSes = babs;
        }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); }); // brake ABSes
    };
    // event on model change in selection of Brake Abs
    BrakeConfigReplaceComponent.prototype.onChangeBrakeABS = function (frontBrakeTypeId, rearBrakeTypeId, brakeABSId) {
        var _this = this;
        this.replaceBrakeConfig.brakeSystemId = -1;
        if (this.replaceBrakeConfig.brakeABSId == -1) {
            this.brakeSystems = [];
            return;
        }
        this.brakeSystems = null;
        this.brakeSystemService.getByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId(frontBrakeTypeId, rearBrakeTypeId, brakeABSId).subscribe(function (bs) {
            _this.brakeSystems = bs;
        }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); }); // brake ABSes
    };
    // event on model change in selection of Brake system
    BrakeConfigReplaceComponent.prototype.onChangeBrakeSystem = function (frontBrakeTypeId, rearBrakeTypeId, brakeABSId, brakeSystemId) {
        var _this = this;
        this.replaceBrakeConfig.id = null;
        // get brake config system info
        if (frontBrakeTypeId > 0 && rearBrakeTypeId > 0 && brakeABSId > 0 && brakeSystemId > 0) {
            this.brakeConfigService.getByChildIds(frontBrakeTypeId, rearBrakeTypeId, brakeABSId, brakeSystemId).subscribe(function (result) {
                _this.replaceBrakeConfig = result;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    // event on brake config id quick search
    BrakeConfigReplaceComponent.prototype.onBrakeConfigIdKeyPress = function (event) {
        if (Number(event.keyCode) === 13) {
            this.onBrakeConfigIdSearch();
        }
    };
    // event on brake config id search
    BrakeConfigReplaceComponent.prototype.onBrakeConfigIdSearch = function () {
        var _this = this;
        var brakeConfigId = Number(this.brakeConfigIdSearchText);
        if (isNaN(brakeConfigId) || brakeConfigId <= 0) {
            this.toastr.warning("Invalid Brake Config Id.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        // empty replace brake config
        this.replaceBrakeConfig = {
            id: 0,
            frontBrakeTypeId: -1,
            frontBrakeTypeName: "",
            rearBrakeTypeId: -1,
            rearBrakeTypeName: "",
            brakeABSId: -1,
            brakeABSName: "",
            brakeSystemId: -1,
            brakeSystemName: "",
            isSelected: false
        };
        this.rearBrakeTypes = null;
        this.brakeABSes = null;
        this.brakeSystems = null;
        this.showLoadingGif = true;
        this.brakeConfigService.getBrakeConfig(Number(this.brakeConfigIdSearchText)).subscribe(function (result) {
            _this.brakeTypeSerivce.getByFrontBrakeTypeId(result.frontBrakeTypeId).subscribe(function (bt) {
                _this.rearBrakeTypes = bt;
                _this.brakeABSService.getByFrontBrakeTypeIdRearBrakeTypeId(result.frontBrakeTypeId, result.rearBrakeTypeId).subscribe(function (babs) {
                    _this.brakeABSes = babs;
                    _this.brakeSystemService.getByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId(result.frontBrakeTypeId, result.rearBrakeTypeId, result.brakeABSId).subscribe(function (bs) {
                        _this.brakeSystems = bs;
                        _this.brakeConfigService.getByChildIds(result.frontBrakeTypeId, result.rearBrakeTypeId, result.brakeABSId, result.brakeSystemId).subscribe(function (result) {
                            _this.replaceBrakeConfig = result;
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
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }, function (error) {
            var errorMessage = JSON.parse(String(error)).message;
            _this.toastr.warning(errorMessage, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    BrakeConfigReplaceComponent.prototype.onSelectAllVehicleToBrakeConfig = function (isSelected) {
        this.isSelectAllVehicleToBrakeConfig = isSelected;
        if (this.existingBrakeConfig.vehicleToBrakeConfigs == null) {
            return;
        }
        this.existingBrakeConfig.vehicleToBrakeConfigs.forEach(function (item) { return item.isSelected = isSelected; });
    };
    BrakeConfigReplaceComponent.prototype.onClearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.refreshFacets();
    };
    BrakeConfigReplaceComponent.prototype.getDefaultInputModel = function () {
        return {
            brakeConfigId: 0,
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
    BrakeConfigReplaceComponent.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
        inputModel.brakeConfigId = this.existingBrakeConfig.id;
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
        this.showLoadingGif = true;
        this.vehicleToBrakeConfigService.refreshFacets(inputModel).subscribe(function (data) {
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
    BrakeConfigReplaceComponent.prototype.filterMakes = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BrakeConfigReplaceComponent.prototype.filterModels = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BrakeConfigReplaceComponent.prototype.filterSubModels = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BrakeConfigReplaceComponent.prototype.filterRegions = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BrakeConfigReplaceComponent.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BrakeConfigReplaceComponent.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BrakeConfigReplaceComponent.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    BrakeConfigReplaceComponent.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    BrakeConfigReplaceComponent.prototype.updateRegionFacet = function (regions) {
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
    BrakeConfigReplaceComponent.prototype.updateYearFacet = function (years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    BrakeConfigReplaceComponent.prototype.updateMakeFacet = function (makes) {
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
    BrakeConfigReplaceComponent.prototype.updateModelFacet = function (models, makeName) {
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
    BrakeConfigReplaceComponent.prototype.updateSubModelFacet = function (subModels, modelName) {
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
    BrakeConfigReplaceComponent.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
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
    BrakeConfigReplaceComponent.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
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
    BrakeConfigReplaceComponent.prototype.searchVehicleToBrakeConfigs = function () {
        var _this = this;
        this.showLoadingGif = true;
        var inputModel = this.getDefaultInputModel();
        inputModel.brakeConfigId = this.existingBrakeConfig.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        //NOTE: inputModel.brakeConfigId = this.existingBrakeConfig.id; should be sufficient here
        //inputModel.frontBrakeTypes.push(this.existingBrakeConfig.frontBrakeTypeName);
        //inputModel.rearBrakeTypes.push(this.existingBrakeConfig.rearBrakeTypeName);
        //inputModel.brakeAbs.push(this.existingBrakeConfig.brakeABSName);
        //inputModel.brakeSystems.push(this.existingBrakeConfig.brakeSystemName);
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
        this.vehicleToBrakeConfigService.getAssociations(inputModel).subscribe(function (result) {
            if (result.length > 0) {
                _this.existingBrakeConfig.vehicleToBrakeConfigs = result;
                _this.existingBrakeConfig.vehicleToBrakeConfigCount = result.length;
                _this.isSelectAllVehicleToBrakeConfig = false;
                if (_this.vehicleToBrakeConfigGrid)
                    _this.vehicleToBrakeConfigGrid.refresh();
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
    BrakeConfigReplaceComponent.prototype.onViewAffectedAssociations = function () {
        $(".drawer-left").css('width', 800); //show the filter panel to enable search
    };
    BrakeConfigReplaceComponent.prototype.clearAllFacets = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
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
        this.refreshFacets();
    };
    BrakeConfigReplaceComponent.prototype.clearFacet = function (facet, refresh) {
        if (refresh === void 0) { refresh = true; }
        if (facet) {
            facet.forEach(function (item) { return item.isSelected = false; });
        }
        if (refresh) {
            this.refreshFacets();
        }
    };
    BrakeConfigReplaceComponent.prototype.onVehicleToBrakeConfigSelected = function (vechicleToBrakeConfig) {
        if (vechicleToBrakeConfig.isSelected) {
            //unchecked
            this.isSelectAllVehicleToBrakeConfig = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingBrakeConfig.vehicleToBrakeConfigs.filter(function (item) { return item.id != vechicleToBrakeConfig.id; });
            if (excludedVehicle.every(function (item) { return item.isSelected; })) {
                this.isSelectAllVehicleToBrakeConfig = true;
            }
        }
    };
    // event on continue
    BrakeConfigReplaceComponent.prototype.onContinue = function () {
        var _this = this;
        // validate
        if (this.validateReplaceBrakeConfig()) {
            // set data in factory/ service
            this.brakeConfigService.existingBrakeConfig = this.existingBrakeConfig;
            this.brakeConfigService.existingBrakeConfig.vehicleToBrakeConfigs = this.existingBrakeConfig.vehicleToBrakeConfigs.filter(function (item) { return item.isSelected; });
            this.replaceBrakeConfig.frontBrakeTypeName = this.frontBrakeTypes.filter(function (item) { return item.id === Number(_this.replaceBrakeConfig.frontBrakeTypeId); })[0].name;
            this.replaceBrakeConfig.rearBrakeTypeName = this.rearBrakeTypes.filter(function (item) { return item.id === Number(_this.replaceBrakeConfig.rearBrakeTypeId); })[0].name;
            this.replaceBrakeConfig.brakeABSName = this.brakeABSes.filter(function (item) { return item.id === Number(_this.replaceBrakeConfig.brakeABSId); })[0].name;
            this.replaceBrakeConfig.brakeSystemName = this.brakeSystems.filter(function (item) { return item.id === Number(_this.replaceBrakeConfig.brakeSystemId); })[0].name;
            this.brakeConfigService.replacementBrakeConfig = this.replaceBrakeConfig;
            // redirect to Confirm page.
            this.router.navigateByUrl("/brakeconfig/replace/confirm/" + this.existingBrakeConfig.id);
        }
        else {
        }
    };
    __decorate([
        //NOTE: keep this number large so that "select all" checkbox always appears
        core_1.ViewChild("vehicleToBrakeConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], BrakeConfigReplaceComponent.prototype, "vehicleToBrakeConfigGrid", void 0);
    BrakeConfigReplaceComponent = __decorate([
        core_1.Component({
            selector: "brakeConfig-replace-component",
            templateUrl: "app/templates/brakeConfig/brakeConfig-replace.component.html",
            providers: [vehicleToBrakeConfig_service_1.VehicleToBrakeConfigService],
        }), 
        __metadata('design:paramtypes', [brakeABS_service_1.BrakeABSService, brakeConfig_service_1.BrakeConfigService, brakeSystem_service_1.BrakeSystemService, brakeType_service_1.BrakeTypeService, router_1.Router, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager, vehicleToBrakeConfig_service_1.VehicleToBrakeConfigService])
    ], BrakeConfigReplaceComponent);
    return BrakeConfigReplaceComponent;
}());
exports.BrakeConfigReplaceComponent = BrakeConfigReplaceComponent;
