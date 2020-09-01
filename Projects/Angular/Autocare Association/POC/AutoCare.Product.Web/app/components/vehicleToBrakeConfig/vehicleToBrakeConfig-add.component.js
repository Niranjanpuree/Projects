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
var baseVehicle_service_1 = require("../baseVehicle/baseVehicle.service");
var make_service_1 = require("../make/make.service");
var model_service_1 = require("../model/model.service");
var year_service_1 = require("../year/year.service");
var region_service_1 = require("../region/region.service");
var submodel_service_1 = require("../submodel/submodel.service");
var vehicleToBrakeConfig_service_1 = require("./vehicleToBrakeConfig.service");
var vehicle_service_1 = require("../vehicle/vehicle.service");
var brakeConfig_service_1 = require("../brakeConfig/brakeConfig.service");
var brakeType_service_1 = require("../brakeType/brakeType.service");
var brakeSystem_service_1 = require("../brakeSystem/brakeSystem.service");
var brakeABS_service_1 = require("../brakeABS/brakeABS.service");
var httpHelper_1 = require("../httpHelper");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var VehicleToBrakeConfigAddComponent = (function () {
    function VehicleToBrakeConfigAddComponent(makeService, modelService, yearService, baseVehicleService, subModelService, regionService, vehicleToBrakeConfigService, vehicleService, brakeTypeService, brakeSystemService, brakeABSService, brakeConfigService, router, sharedService, toastr, navgationService) {
        this.makeService = makeService;
        this.modelService = modelService;
        this.yearService = yearService;
        this.baseVehicleService = baseVehicleService;
        this.subModelService = subModelService;
        this.regionService = regionService;
        this.vehicleToBrakeConfigService = vehicleToBrakeConfigService;
        this.vehicleService = vehicleService;
        this.brakeTypeService = brakeTypeService;
        this.brakeSystemService = brakeSystemService;
        this.brakeABSService = brakeABSService;
        this.brakeConfigService = brakeConfigService;
        this.router = router;
        this.sharedService = sharedService;
        this.toastr = toastr;
        this.navgationService = navgationService;
        this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        this.vehicles = [];
        this.brakeConfig = { id: -1, frontBrakeTypeId: -1, rearBrakeTypeId: -1, brakeABSId: -1, brakeSystemId: -1 };
        this.brakeConfigs = [];
        this.acFiles = [];
        this.proposedVehicleToBrakeConfigsSelectionCount = 0;
        this.showLoadingGif = false;
        if (this.sharedService.vehicles) {
            this.vehicles = this.sharedService.vehicles;
        }
        if (this.sharedService.brakeConfigs) {
            this.brakeConfigs = this.sharedService.brakeConfigs;
        }
        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('vehicletobrakeconfig') > 0)
                this.backNavigationText = "Return to Brake System Search";
            else
                this.backNavigationText = "Return to Vehicle Search";
        }
    }
    VehicleToBrakeConfigAddComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.years = [];
        this.makes = [];
        this.models = [];
        this.subModels = [];
        this.regions = [];
        this.rearBrakeTypes = [];
        this.brakeSystems = [];
        this.brakeABSes = [];
        this.yearService.getYears().subscribe(function (m) {
            _this.years = m;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        }); //TODO: load all years that are attached to basevehicles
        this.brakeTypeService.getAllBrakeTypes().subscribe(function (m) { return _this.frontBrakeTypes = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); }); //TODO: load all frontbraketypes that are attached to brakeconfigs
        this.selectAllChecked = false;
    };
    VehicleToBrakeConfigAddComponent.prototype.onVehicleIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onVehicleIdSearch();
        }
    };
    VehicleToBrakeConfigAddComponent.prototype.onVehicleIdSearch = function () {
        var _this = this;
        var vehicleId = Number(this.vehicleIdSearchText);
        if (isNaN(vehicleId)) {
            this.toastr.warning("Invalid Vehicle ID", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.vehicle.id == vehicleId) {
            return;
        }
        this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        this.makes = null;
        this.models = null;
        this.subModels = null;
        this.regions = null;
        //TODO : may need to load makes related to the base ID
        this.showLoadingGif = true;
        this.vehicleService.getVehicle(vehicleId).subscribe(function (v) {
            _this.makeService.getMakesByYearId(v.yearId).subscribe(function (mks) {
                _this.makes = mks;
                _this.baseVehicleService.getModelsByYearIdAndMakeId(v.yearId, v.makeId).subscribe(function (mdls) {
                    _this.models = mdls;
                    _this.subModelService.getSubModelsByBaseVehicleId(v.baseVehicleId).subscribe(function (subMdls) {
                        _this.subModels = subMdls;
                        _this.regionService.getRegionsByBaseVehicleIdAndSubModelId(v.baseVehicleId, v.subModelId).subscribe(function (r) {
                            _this.regions = r;
                            _this.vehicle = v;
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
    VehicleToBrakeConfigAddComponent.prototype.onSelectYear = function () {
        var _this = this;
        this.vehicle.id = -1;
        this.vehicle.makeId = -1;
        this.vehicle.baseVehicleId = -1;
        this.models = [];
        this.vehicle.subModelId = -1;
        this.subModels = [];
        this.vehicle.regionId = -1;
        this.regions = [];
        if (this.vehicle.yearId == -1) {
            this.makes = [];
            return;
        }
        this.makes = null;
        this.makeService.getMakesByYearId(this.vehicle.yearId).subscribe(function (m) { return _this.makes = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    VehicleToBrakeConfigAddComponent.prototype.onSelectMake = function () {
        var _this = this;
        this.vehicle.id = -1;
        this.vehicle.baseVehicleId = -1;
        this.vehicle.subModelId = -1;
        this.subModels = [];
        this.vehicle.regionId = -1;
        this.regions = [];
        if (this.vehicle.makeId == -1) {
            this.models = [];
            return;
        }
        this.models = null;
        this.baseVehicleService.getModelsByYearIdAndMakeId(this.vehicle.yearId, this.vehicle.makeId).subscribe(function (m) { return _this.models = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    VehicleToBrakeConfigAddComponent.prototype.onSelectModel = function () {
        var _this = this;
        this.vehicle.id = -1;
        this.vehicle.subModelId = -1;
        this.vehicle.regionId = -1;
        this.regions = [];
        if (this.vehicle.baseVehicleId == -1) {
            this.subModels = [];
            return;
        }
        this.subModels = null;
        this.subModelService.getSubModelsByBaseVehicleId(this.vehicle.baseVehicleId).subscribe(function (m) { return _this.subModels = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    VehicleToBrakeConfigAddComponent.prototype.onSelectSubModel = function () {
        var _this = this;
        this.vehicle.id = -1;
        this.vehicle.regionId = -1;
        if (this.vehicle.subModelId == -1) {
            this.regions = [];
            return;
        }
        this.regions = null;
        this.regionService.getRegionsByBaseVehicleIdAndSubModelId(this.vehicle.baseVehicleId, this.vehicle.subModelId).subscribe(function (m) { return _this.regions = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    VehicleToBrakeConfigAddComponent.prototype.onSelectRegion = function () {
        var _this = this;
        this.vehicle.id = -1;
        if (this.vehicle.regionId == -1) {
            return;
        }
        this.showLoadingGif = true;
        this.vehicleService.getVehicleByBaseVehicleIdSubModelIdAndRegionId(this.vehicle.baseVehicleId, this.vehicle.subModelId, this.vehicle.regionId).subscribe(function (m) {
            _this.vehicle = m;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToBrakeConfigAddComponent.prototype.onRemoveVehicle = function (vehicleId) {
        if (confirm("Remove Vehicle Id " + vehicleId + " from selection?")) {
            this.vehicles = this.vehicles.filter(function (item) { return item.id != vehicleId; });
            this.refreshProposedVehicleToBrakeConfigs();
        }
    };
    VehicleToBrakeConfigAddComponent.prototype.onAddVehicleToSelection = function () {
        var _this = this;
        //TODO: do not allow addition if this item has an open CR
        if (this.vehicle.makeId == -1) {
            this.toastr.warning("Please select Make.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.vehicle.modelId == -1) {
            this.toastr.warning("Please select Model.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.vehicle.baseVehicleId == -1) {
            this.toastr.warning("Please select Years.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.vehicle.subModelId == -1) {
            this.toastr.warning("Please select Sub-Model.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.vehicle.regionId == -1) {
            this.toastr.warning("Please select Region.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.vehicle.id == -1) {
            this.toastr.warning("Vehicle ID not available.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        var filteredVehiclses = this.vehicles.filter(function (item) { return item.id == _this.vehicle.id; });
        if (filteredVehiclses && filteredVehiclses.length) {
            this.toastr.warning("Selected Vehicle already added", constants_warehouse_1.ConstantsWarehouse.validationTitle);
        }
        else {
            //NOTE: names are already available
            //this.vehicle.makeName = this.makes.filter(item => item.id == this.vehicle.makeId)[0].name;
            //this.vehicle.modelName = this.models.filter(item => item.id == this.vehicle.modelId)[0].name;
            //this.vehicle.subModelName = this.subModels.filter(item => item.id == this.vehicle.subModelId)[0].name;
            this.vehicles.push(this.vehicle);
            this.refreshProposedVehicleToBrakeConfigs();
            this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        }
    };
    VehicleToBrakeConfigAddComponent.prototype.onViewBrakeAssociations = function (vehicle) {
        var _this = this;
        this.popupVehicle = vehicle;
        this.brakeAssociationsPopup.open("lg");
        if (!this.popupVehicle.vehicleToBrakeConfigs) {
            this.vehicleToBrakeConfigService.getByVehicleId(this.popupVehicle.id).subscribe(function (m) {
                _this.popupVehicle.vehicleToBrakeConfigs = m;
                _this.popupVehicle.vehicleToBrakeConfigCount = m.length;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    VehicleToBrakeConfigAddComponent.prototype.onBrakeConfigIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onBrakeConfigIdSearch();
        }
    };
    VehicleToBrakeConfigAddComponent.prototype.onBrakeConfigIdSearch = function () {
        var _this = this;
        var brakeConfigId = Number(this.brakeConfigIdSearchText);
        if (isNaN(brakeConfigId)) {
            this.toastr.warning("Invalid Brake ID", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.brakeConfig.id == brakeConfigId) {
            return;
        }
        this.brakeConfig = { id: -1, frontBrakeTypeId: -1, rearBrakeTypeId: -1, brakeABSId: -1, brakeSystemId: -1 };
        var savedRearBrakeTypes = this.rearBrakeTypes;
        var savedBrakeABSes = this.brakeABSes;
        var savedBrakeSystems = this.brakeSystems;
        this.rearBrakeTypes = null;
        this.brakeABSes = null;
        this.brakeSystems = null;
        //TODO : may need to load front brake types related to the brake config ID
        this.showLoadingGif = true;
        this.brakeConfigService.getBrakeConfig(brakeConfigId).subscribe(function (bc) {
            _this.brakeTypeService.getByFrontBrakeTypeId(bc.frontBrakeTypeId).subscribe(function (r) {
                _this.rearBrakeTypes = r;
                _this.brakeABSService.getByFrontBrakeTypeIdRearBrakeTypeId(bc.frontBrakeTypeId, bc.rearBrakeTypeId).subscribe(function (a) {
                    _this.brakeABSes = a;
                    _this.brakeSystemService.getByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId(bc.frontBrakeTypeId, bc.rearBrakeTypeId, bc.brakeABSId).subscribe(function (s) {
                        _this.brakeSystems = s;
                        _this.brakeConfig = bc;
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
            var errorMessage = JSON.parse(String(error)).message;
            _this.toastr.warning(errorMessage, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.rearBrakeTypes = savedRearBrakeTypes;
            _this.brakeABSes = savedBrakeABSes;
            _this.brakeSystems = savedBrakeSystems;
            _this.showLoadingGif = false;
        });
    };
    VehicleToBrakeConfigAddComponent.prototype.onSelectFrontBrakeType = function () {
        var _this = this;
        this.brakeConfig.id = -1;
        this.brakeConfig.rearBrakeTypeId = -1;
        this.brakeConfig.brakeABSId = -1;
        this.brakeABSes = [];
        this.brakeConfig.brakeSystemId = -1;
        this.brakeSystems = [];
        if (this.brakeConfig.frontBrakeTypeId == -1) {
            this.rearBrakeTypes = [];
            return;
        }
        this.rearBrakeTypes = null;
        this.brakeTypeService.getByFrontBrakeTypeId(this.brakeConfig.frontBrakeTypeId).subscribe(function (m) { return _this.rearBrakeTypes = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    VehicleToBrakeConfigAddComponent.prototype.onSelectRearBrakeType = function () {
        var _this = this;
        this.brakeConfig.id = -1;
        this.brakeConfig.brakeABSId = -1;
        this.brakeConfig.brakeSystemId = -1;
        this.brakeSystems = [];
        if (this.brakeConfig.rearBrakeTypeId == -1) {
            this.brakeABSes = [];
            return;
        }
        this.brakeABSes = null;
        this.brakeABSService.getByFrontBrakeTypeIdRearBrakeTypeId(this.brakeConfig.frontBrakeTypeId, this.brakeConfig.rearBrakeTypeId).subscribe(function (m) { return _this.brakeABSes = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    VehicleToBrakeConfigAddComponent.prototype.onSelectBrakeABS = function () {
        var _this = this;
        this.brakeConfig.id = -1;
        this.brakeConfig.brakeSystemId = -1;
        if (this.brakeConfig.brakeABSId == -1) {
            this.brakeSystems = [];
            return;
        }
        this.brakeSystems = null;
        this.brakeSystemService.getByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId(this.brakeConfig.frontBrakeTypeId, this.brakeConfig.rearBrakeTypeId, this.brakeConfig.brakeABSId).subscribe(function (m) { return _this.brakeSystems = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    VehicleToBrakeConfigAddComponent.prototype.onSelectBrakeSystem = function () {
        var _this = this;
        this.brakeConfig.id = -1;
        if (this.brakeConfig.brakeSystemId == -1) {
            return;
        }
        this.brakeConfigService.getByChildIds(this.brakeConfig.frontBrakeTypeId, this.brakeConfig.rearBrakeTypeId, this.brakeConfig.brakeABSId, this.brakeConfig.brakeSystemId).subscribe(function (m) { return _this.brakeConfig = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    VehicleToBrakeConfigAddComponent.prototype.onAddBrakeConfigToSelection = function () {
        var _this = this;
        //TODO: do not allow addition if this item has an open CR
        if (this.brakeConfig.frontBrakeTypeId == -1) {
            this.toastr.warning("Please select Front Brake Type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.brakeConfig.rearBrakeTypeId == -1) {
            this.toastr.warning("Please select Rear Brake Type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.brakeConfig.brakeABSId == -1) {
            this.toastr.warning("Please select Brake ABS.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.brakeConfig.brakeSystemId == -1) {
            this.toastr.warning("Please select Brake System.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.brakeConfig.id == -1) {
            this.toastr.warning("Brake ID not available.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        var filteredBrakeConfigs = this.brakeConfigs.filter(function (item) { return item.id == _this.brakeConfig.id; });
        if (filteredBrakeConfigs && filteredBrakeConfigs.length) {
            this.toastr.warning("Selected Brake ID already added", constants_warehouse_1.ConstantsWarehouse.validationTitle);
        }
        else {
            this.brakeConfig.frontBrakeTypeName = this.frontBrakeTypes.filter(function (item) { return item.id == _this.brakeConfig.frontBrakeTypeId; })[0].name;
            this.brakeConfig.rearBrakeTypeName = this.rearBrakeTypes.filter(function (item) { return item.id == _this.brakeConfig.rearBrakeTypeId; })[0].name;
            this.brakeConfig.brakeABSName = this.brakeABSes.filter(function (item) { return item.id == _this.brakeConfig.brakeABSId; })[0].name;
            this.brakeConfig.brakeSystemName = this.brakeSystems.filter(function (item) { return item.id == _this.brakeConfig.brakeSystemId; })[0].name;
            this.brakeConfigs.push(this.brakeConfig);
            this.refreshProposedVehicleToBrakeConfigs();
            this.brakeConfig = { id: -1, frontBrakeTypeId: -1, rearBrakeTypeId: -1, brakeABSId: -1, brakeSystemId: -1 };
        }
        this.selectAllChecked = true;
    };
    VehicleToBrakeConfigAddComponent.prototype.onViewAssociations = function (brakeConfig) {
        var _this = this;
        this.popupBrakeConfig = brakeConfig;
        this.associationsPopup.open("lg");
        if (!this.popupBrakeConfig.vehicleToBrakeConfigs) {
            var inputModel = this.getDefaultInputModel();
            inputModel.brakeConfigId = this.popupBrakeConfig.id;
            this.vehicleToBrakeConfigService.getAssociations(inputModel).subscribe(function (m) {
                _this.popupBrakeConfig.vehicleToBrakeConfigs = m;
                _this.popupBrakeConfig.vehicleToBrakeConfigCount = m.length;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    VehicleToBrakeConfigAddComponent.prototype.getDefaultInputModel = function () {
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
    VehicleToBrakeConfigAddComponent.prototype.onRemoveBrakeConfig = function (brakeConfigId) {
        if (confirm("Remove Brake Id " + brakeConfigId + " from selection?")) {
            this.brakeConfigs = this.brakeConfigs.filter(function (item) { return item.id != brakeConfigId; });
            this.refreshProposedVehicleToBrakeConfigs();
        }
    };
    VehicleToBrakeConfigAddComponent.prototype.refreshProposedVehicleToBrakeConfigs = function () {
        var _this = this;
        if (this.vehicles.length == 0 || this.brakeConfigs.length == 0) {
            return;
        }
        if (this.showLoadingGif) {
            return;
        }
        this.showLoadingGif = true;
        var allProposedVehicleToBrakeConfigs = [];
        this.vehicles.forEach(function (v) {
            _this.brakeConfigs.forEach(function (b) {
                allProposedVehicleToBrakeConfigs.push({
                    vehicle: {
                        id: v.id,
                        baseVehicleId: v.baseVehicleId,
                        makeId: null,
                        makeName: v.makeName,
                        modelId: null,
                        modelName: v.modelName,
                        yearId: v.yearId,
                        subModelId: null,
                        subModelName: v.subModelName,
                        regionId: null,
                        regionName: v.regionName,
                        sourceId: null,
                        sourceName: '',
                        publicationStageId: null,
                        publicationStageName: '',
                        publicationStageDate: null,
                        publicationStageSource: '',
                        publicationEnvironment: '',
                        vehicleToBrakeConfigCount: null,
                        isSelected: false
                    },
                    brakeConfig: {
                        id: b.id,
                        frontBrakeTypeId: b.frontBrakeTypeId,
                        frontBrakeTypeName: b.frontBrakeTypeName,
                        rearBrakeTypeId: b.rearBrakeTypeId,
                        rearBrakeTypeName: b.rearBrakeTypeName,
                        brakeSystemId: b.brakeSystemId,
                        brakeSystemName: b.brakeSystemName,
                        brakeABSId: b.brakeABSId,
                        brakeABSName: b.brakeABSName,
                        vehicleToBrakeConfigCount: 0
                    },
                    numberOfBrakesAssociation: -1,
                    isSelected: true
                });
            });
        });
        var selectedVehicleIds = this.vehicles.map(function (x) { return x.id; });
        var selectedBrakeConfigIds = this.brakeConfigs.map(function (x) { return x.id; });
        this.vehicleToBrakeConfigService.getVehicleToBrakeConfigsByVehicleIdsAndBrakeConfigIds(selectedVehicleIds, selectedBrakeConfigIds)
            .subscribe(function (m) {
            _this.proposedVehicleToBrakeConfigs = [];
            _this.existingVehicleToBrakeConfigs = m;
            if (_this.existingVehicleToBrakeConfigs == null || _this.existingVehicleToBrakeConfigs.length == 0) {
                _this.proposedVehicleToBrakeConfigs = allProposedVehicleToBrakeConfigs;
            }
            else {
                var existingVehicleIds_1 = _this.existingVehicleToBrakeConfigs.map(function (x) { return x.vehicle.id; });
                var existingBrakeConfigIds_1 = _this.existingVehicleToBrakeConfigs.map(function (x) { return x.brakeConfig.id; });
                _this.proposedVehicleToBrakeConfigs = allProposedVehicleToBrakeConfigs.filter(function (item) { return existingVehicleIds_1.indexOf(item.vehicle.id) < 0 || existingBrakeConfigIds_1.indexOf(item.brakeConfig.id) < 0; });
            }
            if (_this.proposedVehicleToBrakeConfigs != null) {
                _this.proposedVehicleToBrakeConfigsSelectionCount = _this.proposedVehicleToBrakeConfigs.filter(function (item) { return item.isSelected; }).length;
            }
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToBrakeConfigAddComponent.prototype.onSelectAllProposedBrakeAssociations = function (event) {
        this.selectAllChecked = event;
        if (this.proposedVehicleToBrakeConfigs == null) {
            return;
        }
        this.proposedVehicleToBrakeConfigs.forEach(function (item) { return item.isSelected = event.target.checked; });
        this.proposedVehicleToBrakeConfigsSelectionCount = this.proposedVehicleToBrakeConfigs.filter(function (item) { return item.isSelected; }).length;
    };
    VehicleToBrakeConfigAddComponent.prototype.refreshProposedVehicleToBrakeConfigsSelectionCount = function (event, vehicleTobrakeconfig) {
        if (event.target.checked) {
            this.proposedVehicleToBrakeConfigsSelectionCount++;
            var excludedVehicle = this.proposedVehicleToBrakeConfigs.filter(function (item) { return item.isSelected; });
            if (excludedVehicle.length == this.proposedVehicleToBrakeConfigs.length - 1) {
                this.selectAllChecked = true;
            }
        }
        else {
            this.proposedVehicleToBrakeConfigsSelectionCount--;
            this.selectAllChecked = false;
        }
    };
    VehicleToBrakeConfigAddComponent.prototype.onSubmitAssociations = function () {
        if (!this.proposedVehicleToBrakeConfigs) {
            return;
        }
        var length = this.proposedVehicleToBrakeConfigs.length;
        if (this.proposedVehicleToBrakeConfigs.filter(function (item) { return item.isSelected; }).length == 0) {
            this.toastr.warning("No brake associations selected", constants_warehouse_1.ConstantsWarehouse.errorTitle);
            return;
        }
        this.submitAssociations(length);
    };
    VehicleToBrakeConfigAddComponent.prototype.getNextVehicleToBrakeConfig = function (index) {
        if (!this.proposedVehicleToBrakeConfigs || this.proposedVehicleToBrakeConfigs.length == 0) {
            return null;
        }
        var nextConfig = this.proposedVehicleToBrakeConfigs[index];
        return nextConfig;
    };
    VehicleToBrakeConfigAddComponent.prototype.submitAssociations = function (length) {
        var _this = this;
        this.showLoadingGif = true;
        length = length - 1;
        if (length >= 0) {
            var proposedVehicleToBrakeConfig_1 = this.getNextVehicleToBrakeConfig(length);
            proposedVehicleToBrakeConfig_1.comment = this.commenttoadd;
            var vehicleToBrakeConfigIdentity_1 = "(Vehicle ID: " + proposedVehicleToBrakeConfig_1.vehicle.id + ", Brake Config ID: " + proposedVehicleToBrakeConfig_1.brakeConfig.id + ")";
            this.acFiles = this.acFileUploader.getAcFiles();
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    proposedVehicleToBrakeConfig_1.attachments = _this.sharedService.clone(uploadedFiles);
                }
                if (proposedVehicleToBrakeConfig_1.attachments) {
                    proposedVehicleToBrakeConfig_1.attachments = proposedVehicleToBrakeConfig_1.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.vehicleToBrakeConfigService.addVehicleToBrakeConfig(proposedVehicleToBrakeConfig_1).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle to Brake Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToBrakeConfigIdentity_1);
                        successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + vehicleToBrakeConfigIdentity_1 + "\" Vehicle to Brake Config change requestid  \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.acFileUploader.reset();
                        _this.acFileUploader.setAcFiles(_this.acFiles);
                        _this.submitAssociations(length);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle to Brake Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToBrakeConfigIdentity_1);
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                        _this.acFileUploader.reset();
                        _this.acFileUploader.setAcFiles(_this.acFiles);
                        _this.submitAssociations(length);
                    }
                }, function (error) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle to Brake Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToBrakeConfigIdentity_1);
                    _this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                    _this.acFileUploader.reset();
                    _this.acFileUploader.setAcFiles(_this.acFiles);
                    _this.submitAssociations(length);
                });
            }, function (error) {
                _this.showLoadingGif = false;
            });
        }
        else {
            this.acFileUploader.reset();
            this.showLoadingGif = false;
        }
    };
    VehicleToBrakeConfigAddComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToBrakeConfigAddComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild('brakeAssociationsPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToBrakeConfigAddComponent.prototype, "brakeAssociationsPopup", void 0);
    __decorate([
        core_1.ViewChild('associationsPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToBrakeConfigAddComponent.prototype, "associationsPopup", void 0);
    VehicleToBrakeConfigAddComponent = __decorate([
        core_1.Component({
            selector: "vehicleToBrakeConfig-add-component",
            templateUrl: "app/templates/vehicleToBrakeConfig/vehicleToBrakeConfig-add.component.html",
            providers: [make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, submodel_service_1.SubModelService, region_service_1.RegionService, httpHelper_1.HttpHelper]
        }), 
        __metadata('design:paramtypes', [make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, submodel_service_1.SubModelService, region_service_1.RegionService, vehicleToBrakeConfig_service_1.VehicleToBrakeConfigService, vehicle_service_1.VehicleService, brakeType_service_1.BrakeTypeService, brakeSystem_service_1.BrakeSystemService, brakeABS_service_1.BrakeABSService, brakeConfig_service_1.BrakeConfigService, router_1.Router, shared_service_1.SharedService, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleToBrakeConfigAddComponent);
    return VehicleToBrakeConfigAddComponent;
}());
exports.VehicleToBrakeConfigAddComponent = VehicleToBrakeConfigAddComponent;
