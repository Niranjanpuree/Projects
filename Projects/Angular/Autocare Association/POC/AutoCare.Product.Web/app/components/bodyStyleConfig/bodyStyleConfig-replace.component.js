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
var bodyType_service_1 = require("../bodyType/bodyType.service");
var bodyNumDoors_service_1 = require("../bodyNumDoors/bodyNumDoors.service");
var bodyStyleConfig_service_1 = require("./bodyStyleConfig.service");
var constants_warehouse_1 = require("../constants-warehouse");
var vehicleToBodyStyleConfig_service_1 = require("../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.service");
var ac_grid_1 = require('../../lib/aclibs/ac-grid/ac-grid');
var BodyStyleConfigReplaceComponent = (function () {
    function BodyStyleConfigReplaceComponent(bodyTypeService, bodyStyleConfigService, bodyNumDoorsService, router, route, toastr, vehicleToBodyStyleConfigService) {
        this.bodyTypeService = bodyTypeService;
        this.bodyStyleConfigService = bodyStyleConfigService;
        this.bodyNumDoorsService = bodyNumDoorsService;
        this.router = router;
        this.route = route;
        this.toastr = toastr;
        this.vehicleToBodyStyleConfigService = vehicleToBodyStyleConfigService;
        this.showLoadingGif = false;
        this.vehicleTypeGroupFacet = [];
        this.thresholdRecordCount = 100000; //NOTE: keep this number large so that "select all" checkbox always appears
        // initialize empty bed config
        this.replaceBodyStyleConfig = {
            id: 0,
            bodyNumDoorsId: -1,
            numDoors: "",
            bodyTypeId: -1,
            bodyTypeName: "",
            isSelected: false
        };
    }
    BodyStyleConfigReplaceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.isSelectAllVehicleToBodyStyleConfig = false;
        // Load existing body style config with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        this.bodyNumDoors = [];
        this.bodyTypes = [];
        this.bodyStyleConfigService.getBodyStyleConfig(id).subscribe(function (result) {
            _this.existingBodyStyleConfig = result;
            _this.bodyNumDoorsService.getAllBodyNumDoors().subscribe(function (bl) {
                _this.bodyNumDoors = bl;
                _this.refreshFacets();
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
        this.vehicleToBodyStyleConfigSearchViewModel = {
            facets: {
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                startYears: [],
                endYears: [],
                makes: [],
                models: [],
                subModels: [],
                bodyNumDoors: [],
                bodyTypes: []
            },
            result: { bodyStyleConfigs: [], vehicleToBodyStyleConfigs: [] }
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
    BodyStyleConfigReplaceComponent.prototype.validateReplaceBodyStyleConfig = function () {
        var isValid = true;
        // check required fields
        if (Number(this.replaceBodyStyleConfig.bodyNumDoorsId) === -1) {
            this.toastr.warning("Please select Body num doors.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBodyStyleConfig.bodyTypeId) === -1) {
            this.toastr.warning("Please select Body type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBodyStyleConfig.id) < 1) {
            this.toastr.warning("Please select replacement Body style config system.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingBodyStyleConfig.id) === Number(this.replaceBodyStyleConfig.id)) {
            this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.filter(function (item) { return item.isSelected; }).length <= 0) {
            this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    // event on model change in selection of Bed Type
    BodyStyleConfigReplaceComponent.prototype.onChangeBodyType = function (bodyTypeId, bodyNumDoorsId) {
        var _this = this;
        this.replaceBodyStyleConfig.id = null;
        if (bodyTypeId > 0 && bodyNumDoorsId > 0) {
            this.bodyStyleConfigService.getByChildIds(bodyNumDoorsId, bodyTypeId).subscribe(function (bc) {
                _this.replaceBodyStyleConfig = bc;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            });
        }
        else {
            this.replaceBodyStyleConfig.id = 0;
        }
    };
    // event on model change in selection of Bed Length
    BodyStyleConfigReplaceComponent.prototype.onChangeBodyNumDoors = function (bodyNumDoorsId) {
        var _this = this;
        this.replaceBodyStyleConfig.bodyTypeId = -1;
        if (this.replaceBodyStyleConfig.bodyNumDoorsId == -1) {
            this.bodyTypes = [];
            this.replaceBodyStyleConfig.id = 0;
            return;
        }
        this.bodyTypes = null;
        this.bodyTypeService.getByBodyNumDoorsId(bodyNumDoorsId).subscribe(function (bt) {
            _this.bodyTypes = bt;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
        }); // bed types
    };
    BodyStyleConfigReplaceComponent.prototype.onBodyStyleConfigIdKeyPress = function (event) {
        if (Number(event.keyCode) === 13) {
            this.onBodyStyleConfigIdSearch();
        }
    };
    // event on bed config id search
    BodyStyleConfigReplaceComponent.prototype.onBodyStyleConfigIdSearch = function () {
        var _this = this;
        var bodyStyleConfigId = Number(this.bodyStyleConfigIdSearchText);
        if (isNaN(bodyStyleConfigId) || bodyStyleConfigId <= 0) {
            this.toastr.warning("Invalid Bed Config Id.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        // empty replace bed config
        this.replaceBodyStyleConfig = {
            id: 0,
            bodyNumDoorsId: -1,
            numDoors: "",
            bodyTypeId: -1,
            bodyTypeName: "",
            isSelected: false
        };
        this.bodyTypes = null;
        this.bodyNumDoors = null;
        this.showLoadingGif = true;
        this.bodyStyleConfigService.getBodyStyleConfig(Number(this.bodyStyleConfigIdSearchText)).subscribe(function (result) {
            _this.bodyTypeService.getByBodyTypeId(result.bodyTypeId).subscribe(function (bt) {
                _this.bodyTypes = bt;
                _this.bodyNumDoorsService.getByBodyNumDoorsId(result.bodyNumDoorsId).subscribe(function (bl) {
                    _this.bodyNumDoors = bl;
                    _this.bodyStyleConfigService.getByChildIds(result.bodyNumDoorsId, result.bodyTypeId).subscribe(function (result) {
                        _this.replaceBodyStyleConfig = result;
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
    };
    BodyStyleConfigReplaceComponent.prototype.onSelectAllVehicleToBodyStyleConfig = function (isSelected) {
        this.isSelectAllVehicleToBodyStyleConfig = isSelected;
        if (this.existingBodyStyleConfig.vehicleToBodyStyleConfigs == null) {
            return;
        }
        this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.forEach(function (item) { return item.isSelected = isSelected; });
    };
    BodyStyleConfigReplaceComponent.prototype.onClearFilters = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.refreshFacets();
    };
    BodyStyleConfigReplaceComponent.prototype.getDefaultInputModel = function () {
        return {
            bodyStyleConfigId: 0,
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
    BodyStyleConfigReplaceComponent.prototype.refreshFacets = function () {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
        inputModel.bodyStyleConfigId = this.existingBodyStyleConfig.id;
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
        this.showLoadingGif = true;
        this.vehicleToBodyStyleConfigService.refreshFacets(inputModel).subscribe(function (data) {
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
    BodyStyleConfigReplaceComponent.prototype.filterMakes = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.makes != null) {
            var inputElement = $event.target;
            this.makeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BodyStyleConfigReplaceComponent.prototype.filterModels = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models != null) {
            var inputElement = $event.target;
            this.modelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BodyStyleConfigReplaceComponent.prototype.filterSubModels = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels != null) {
            var inputElement = $event.target;
            this.subModelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BodyStyleConfigReplaceComponent.prototype.filterRegions = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.regions != null) {
            var inputElement = $event.target;
            this.regionFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BodyStyleConfigReplaceComponent.prototype.filterVehicleTypeGroups = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = $event.target;
            this.vehicleTypeGroupFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BodyStyleConfigReplaceComponent.prototype.filterVehicleTypes = function ($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = $event.target;
            this.vehicleTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1; });
        }
    };
    BodyStyleConfigReplaceComponent.prototype.onYearSelected = function () {
        this.refreshFacets();
    };
    BodyStyleConfigReplaceComponent.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    BodyStyleConfigReplaceComponent.prototype.updateRegionFacet = function (regions) {
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
    BodyStyleConfigReplaceComponent.prototype.updateYearFacet = function (years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    };
    BodyStyleConfigReplaceComponent.prototype.updateMakeFacet = function (makes) {
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
    //TODO: makeName is not used
    BodyStyleConfigReplaceComponent.prototype.updateModelFacet = function (models, makeName) {
        var existingSelectedModels = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.filter(function (model) { return model.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToBodyStyleConfigSearchViewModel.facets.models = [];
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
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models.push(newModel);
        }
        this.modelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.slice();
    };
    BodyStyleConfigReplaceComponent.prototype.updateSubModelFacet = function (subModels, modelName) {
        var existingSelectedSubModels = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.filter(function (submodel) { return submodel.isSelected; })
            .map(function (item) { return item.name; });
        this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels = [];
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
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.push(newSubModel);
        }
        this.subModelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.slice();
    };
    BodyStyleConfigReplaceComponent.prototype.updateVehicleTypeGroupFacet = function (vehicleTypeGroups) {
        var existingSelectedItems = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
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
    BodyStyleConfigReplaceComponent.prototype.updateVehicleTypeFacet = function (vehicleTypes) {
        var existingSelectedItems = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
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
    BodyStyleConfigReplaceComponent.prototype.searchVehicleToBodyStyleConfigs = function () {
        var _this = this;
        this.showLoadingGif = true;
        var inputModel = this.getDefaultInputModel();
        inputModel.bodyStyleConfigId = this.existingBodyStyleConfig.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;
        //NOTE: inputModel.bodyStyleConfigId = this.existingBodyStyleConfig.id; should be sufficient here
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
        this.vehicleToBodyStyleConfigService.getAssociations(inputModel).subscribe(function (result) {
            if (result.length > 0) {
                _this.existingBodyStyleConfig.vehicleToBodyStyleConfigs = result;
                _this.existingBodyStyleConfig.vehicleToBodyStyleConfigCount = result.length;
                _this.isSelectAllVehicleToBodyStyleConfig = false;
                if (_this.vehicleToBodyStyleConfigGrid)
                    _this.vehicleToBodyStyleConfigGrid.refresh();
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
    BodyStyleConfigReplaceComponent.prototype.onViewAffectedAssociations = function () {
        $(".drawer-left").css('width', 800); //show the filter panel to enable search
    };
    BodyStyleConfigReplaceComponent.prototype.clearAllFacets = function () {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
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
        this.refreshFacets();
    };
    BodyStyleConfigReplaceComponent.prototype.clearFacet = function (facet, refresh) {
        if (refresh === void 0) { refresh = true; }
        if (facet) {
            facet.forEach(function (item) { return item.isSelected = false; });
        }
        if (refresh) {
            this.refreshFacets();
        }
    };
    BodyStyleConfigReplaceComponent.prototype.onVehicleToBodyStyleConfigSelected = function (vechicleToBodyStyleConfig) {
        if (vechicleToBodyStyleConfig.isSelected) {
            //unchecked
            this.isSelectAllVehicleToBodyStyleConfig = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.filter(function (item) { return item.id != vechicleToBodyStyleConfig.id; });
            if (excludedVehicle.every(function (item) { return item.isSelected; })) {
                this.isSelectAllVehicleToBodyStyleConfig = true;
            }
        }
    };
    // event on continue
    BodyStyleConfigReplaceComponent.prototype.onContinue = function () {
        var _this = this;
        if (this.validateReplaceBodyStyleConfig()) {
            this.bodyStyleConfigService.existingBodyStyleConfig = this.existingBodyStyleConfig;
            this.bodyStyleConfigService.existingBodyStyleConfig.vehicleToBodyStyleConfigs = this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.filter(function (item) { return item.isSelected; });
            this.replaceBodyStyleConfig.numDoors = this.bodyNumDoors.filter(function (item) { return item.id === Number(_this.replaceBodyStyleConfig.bodyNumDoorsId); })[0].numDoors;
            this.replaceBodyStyleConfig.bodyTypeName = this.bodyTypes.filter(function (item) { return item.id === Number(_this.replaceBodyStyleConfig.bodyTypeId); })[0].name;
            this.bodyStyleConfigService.replacementBodyStyleConfig = this.replaceBodyStyleConfig;
            this.router.navigateByUrl("/bodystyleconfig/replace/confirm/" + this.existingBodyStyleConfig.id);
        }
    };
    __decorate([
        //NOTE: keep this number large so that "select all" checkbox always appears
        core_1.ViewChild("vehicleToBodyStyleConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], BodyStyleConfigReplaceComponent.prototype, "vehicleToBodyStyleConfigGrid", void 0);
    BodyStyleConfigReplaceComponent = __decorate([
        core_1.Component({
            selector: "bodyStyleConfig-replace-component",
            templateUrl: "app/templates/bodyStyleConfig/bodyStyleConfig-replace.component.html",
            providers: [vehicleToBodyStyleConfig_service_1.VehicleToBodyStyleConfigService],
        }), 
        __metadata('design:paramtypes', [bodyType_service_1.BodyTypeService, bodyStyleConfig_service_1.BodyStyleConfigService, bodyNumDoors_service_1.BodyNumDoorsService, router_1.Router, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager, vehicleToBodyStyleConfig_service_1.VehicleToBodyStyleConfigService])
    ], BodyStyleConfigReplaceComponent);
    return BodyStyleConfigReplaceComponent;
}());
exports.BodyStyleConfigReplaceComponent = BodyStyleConfigReplaceComponent;
