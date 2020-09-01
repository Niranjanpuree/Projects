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
var vehicleToBedConfig_service_1 = require("./vehicleToBedConfig.service");
var vehicle_service_1 = require("../vehicle/vehicle.service");
var bedConfig_service_1 = require("../bedConfig/bedConfig.service");
var bedType_service_1 = require("../bedType/bedType.service");
var bedlength_service_1 = require("../bedLength/bedlength.service");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var VehicleToBedConfigAddComponent = (function () {
    function VehicleToBedConfigAddComponent(makeService, modelService, yearService, baseVehicleService, subModelService, regionService, vehicleToBedConfigService, vehicleService, bedTypeService, bedLengthService, bedConfigService, router, sharedService, toastr, navgationService) {
        this.makeService = makeService;
        this.modelService = modelService;
        this.yearService = yearService;
        this.baseVehicleService = baseVehicleService;
        this.subModelService = subModelService;
        this.regionService = regionService;
        this.vehicleToBedConfigService = vehicleToBedConfigService;
        this.vehicleService = vehicleService;
        this.bedTypeService = bedTypeService;
        this.bedLengthService = bedLengthService;
        this.bedConfigService = bedConfigService;
        this.router = router;
        this.sharedService = sharedService;
        this.toastr = toastr;
        this.navgationService = navgationService;
        this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        this.vehicles = [];
        this.bedConfig = { id: -1, bedLengthId: -1, bedTypeId: -1 };
        this.bedConfigs = [];
        this.acFiles = [];
        this.proposedVehicleToBedConfigsSelectionCount = 0;
        this.showLoadingGif = false;
        if (this.sharedService.vehicles) {
            this.vehicles = this.sharedService.vehicles;
        }
        if (this.sharedService.bedConfigs) {
            this.bedConfigs = this.sharedService.bedConfigs;
        }
        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('vehicletobedconfig') > 0)
                this.backNavigationText = "Return to Bed System Search";
            else
                this.backNavigationText = "Return to Vehicle Search";
        }
    }
    VehicleToBedConfigAddComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.years = [];
        this.makes = [];
        this.models = [];
        this.subModels = [];
        this.regions = [];
        this.bedTypes = [];
        this.yearService.getYears().subscribe(function (m) {
            _this.years = m;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        }); //TODO: load all years that are attached to basevehicles
        this.bedLengthService.getAllBedLengths().subscribe(function (m) { return _this.bedLengths = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        this.selectAllChecked = false;
    };
    VehicleToBedConfigAddComponent.prototype.onVehicleIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onVehicleIdSearch();
        }
    };
    VehicleToBedConfigAddComponent.prototype.onVehicleIdSearch = function () {
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
    VehicleToBedConfigAddComponent.prototype.onSelectYear = function () {
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
    VehicleToBedConfigAddComponent.prototype.onSelectMake = function () {
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
    VehicleToBedConfigAddComponent.prototype.onSelectModel = function () {
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
    VehicleToBedConfigAddComponent.prototype.onSelectSubModel = function () {
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
    VehicleToBedConfigAddComponent.prototype.onSelectRegion = function () {
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
    VehicleToBedConfigAddComponent.prototype.onRemoveVehicle = function (vehicleId) {
        if (confirm("Remove Vehicle Id " + vehicleId + " from selection?")) {
            this.vehicles = this.vehicles.filter(function (item) { return item.id != vehicleId; });
            this.refreshProposedVehicleToBedConfigs();
        }
    };
    VehicleToBedConfigAddComponent.prototype.onAddVehicleToSelection = function () {
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
            this.refreshProposedVehicleToBedConfigs();
            this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        }
    };
    VehicleToBedConfigAddComponent.prototype.onViewBedAssociations = function (vehicle) {
        var _this = this;
        this.popupVehicle = vehicle;
        this.bedAssociationsPopup.open("lg");
        if (!this.popupVehicle.vehicleToBedConfigs) {
            this.vehicleToBedConfigService.getByVehicleId(this.popupVehicle.id).subscribe(function (m) {
                _this.popupVehicle.vehicleToBedConfigs = m;
                _this.popupVehicle.vehicleToBedConfigCount = m.length;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    VehicleToBedConfigAddComponent.prototype.onBedConfigIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onBedConfigIdSearch();
        }
    };
    VehicleToBedConfigAddComponent.prototype.onBedConfigIdSearch = function () {
        var _this = this;
        var bedConfigId = Number(this.bedConfigIdSearchText);
        if (isNaN(bedConfigId)) {
            this.toastr.warning("Invalid Bed Config ID", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.bedConfig.id == bedConfigId) {
            return;
        }
        this.bedConfig = { id: -1, bedTypeId: -1, bedLengthId: -1 };
        var savedBedTypes = this.bedTypes;
        var savedBedLengths = this.bedLengths;
        this.bedTypes = null;
        //TODO : may need to load front bed types related to the bed config ID
        this.showLoadingGif = true;
        this.bedConfigService.getBedConfig(bedConfigId).subscribe(function (bc) {
            _this.bedTypeService.getByBedLengthId(bc.bedLengthId).subscribe(function (a) {
                _this.bedTypes = a;
                _this.bedConfig = bc;
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }, function (error) {
            var errorMessage = JSON.parse(String(error)).message;
            _this.toastr.warning(errorMessage, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.bedLengths = savedBedLengths;
            _this.bedTypes = savedBedTypes;
            _this.showLoadingGif = false;
        });
    };
    VehicleToBedConfigAddComponent.prototype.onSelectBedLength = function () {
        var _this = this;
        this.bedConfig.id = -1;
        this.bedConfig.bedTypeId = -1;
        if (this.bedConfig.bedLengthId == -1) {
            this.bedTypes = [];
            return;
        }
        this.bedTypes = null;
        this.bedTypeService.getByBedLengthId(this.bedConfig.bedLengthId).subscribe(function (m) { return _this.bedTypes = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    VehicleToBedConfigAddComponent.prototype.onSelectBedType = function () {
        var _this = this;
        this.bedConfig.id = -1;
        if (this.bedConfig.bedTypeId == -1) {
            return;
        }
        this.bedConfigService.getByChildIds(this.bedConfig.bedLengthId, this.bedConfig.bedTypeId).subscribe(function (m) { return _this.bedConfig = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    VehicleToBedConfigAddComponent.prototype.onAddBedConfigToSelection = function () {
        //TODO: do not allow addition if this item has an open CR
        var _this = this;
        if (this.bedConfig.bedLengthId == -1) {
            this.toastr.warning("Please select Bed Length.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.bedConfig.bedTypeId == -1) {
            this.toastr.warning("Please select Bed Type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.bedConfig.id == -1) {
            this.toastr.warning("Bed Config ID not available.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        var filteredBedConfigs = this.bedConfigs.filter(function (item) { return item.id == _this.bedConfig.id; });
        if (filteredBedConfigs && filteredBedConfigs.length) {
            this.toastr.warning("Selected Bed Config ID already added", constants_warehouse_1.ConstantsWarehouse.validationTitle);
        }
        else {
            this.bedConfig.length = this.bedLengths.filter(function (item) { return item.id == _this.bedConfig.bedLengthId; })[0].length;
            this.bedConfig.bedTypeName = this.bedTypes.filter(function (item) { return item.id == _this.bedConfig.bedTypeId; })[0].name;
            this.bedConfigs.push(this.bedConfig);
            this.refreshProposedVehicleToBedConfigs();
            this.bedConfig = { id: -1, bedLengthId: -1, bedTypeId: -1 };
        }
        this.selectAllChecked = true;
    };
    VehicleToBedConfigAddComponent.prototype.onViewAssociations = function (bedConfig) {
        var _this = this;
        this.popupBedConfig = bedConfig;
        this.associationsPopup.open("lg");
        if (!this.popupBedConfig.vehicleToBedConfigs) {
            var inputModel = this.getDefaultInputModel();
            inputModel.bedConfigId = this.popupBedConfig.id;
            this.vehicleToBedConfigService.getAssociations(inputModel).subscribe(function (m) {
                _this.popupBedConfig.vehicleToBedConfigs = m;
                _this.popupBedConfig.vehicleToBedConfigCount = m.length;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    VehicleToBedConfigAddComponent.prototype.getDefaultInputModel = function () {
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
    VehicleToBedConfigAddComponent.prototype.onRemoveBedConfig = function (bedConfigId) {
        if (confirm("Remove Bed Config Id " + bedConfigId + " from selection?")) {
            this.bedConfigs = this.bedConfigs.filter(function (item) { return item.id != bedConfigId; });
            this.refreshProposedVehicleToBedConfigs();
        }
    };
    VehicleToBedConfigAddComponent.prototype.refreshProposedVehicleToBedConfigs = function () {
        var _this = this;
        if (this.vehicles.length == 0 || this.bedConfigs.length == 0) {
            return;
        }
        if (this.showLoadingGif) {
            return;
        }
        this.showLoadingGif = true;
        var allProposedVehicleToBedConfigs = [];
        this.vehicles.forEach(function (v) {
            _this.bedConfigs.forEach(function (b) {
                allProposedVehicleToBedConfigs.push({
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
                        vehicleToBedConfigCount: null,
                        isSelected: false
                    },
                    bedConfig: {
                        id: b.id,
                        bedLengthId: b.bedLengthId,
                        length: b.length,
                        bedLengthMetric: b.bedLengthMetric,
                        bedTypeId: b.bedTypeId,
                        bedTypeName: b.bedTypeName,
                        vehicleToBedConfigCount: 0
                    },
                    numberOfBedAssociation: -1,
                    isSelected: true
                });
            });
        });
        var selectedVehicleIds = this.vehicles.map(function (x) { return x.id; });
        var selectedBedConfigIds = this.bedConfigs.map(function (x) { return x.id; });
        this.vehicleToBedConfigService.getVehicleToBedConfigsByVehicleIdsAndBedConfigIds(selectedVehicleIds, selectedBedConfigIds)
            .subscribe(function (m) {
            _this.proposedVehicleToBedConfigs = [];
            _this.existingVehicleToBedConfigs = m;
            if (_this.existingVehicleToBedConfigs == null || _this.existingVehicleToBedConfigs.length == 0) {
                _this.proposedVehicleToBedConfigs = allProposedVehicleToBedConfigs;
            }
            else {
                var existingVehicleIds_1 = _this.existingVehicleToBedConfigs.map(function (x) { return x.vehicle.id; });
                var existingBedConfigIds_1 = _this.existingVehicleToBedConfigs.map(function (x) { return x.bedConfig.id; });
                _this.proposedVehicleToBedConfigs = allProposedVehicleToBedConfigs.filter(function (item) { return existingVehicleIds_1.indexOf(item.vehicle.id) < 0 || existingBedConfigIds_1.indexOf(item.bedConfig.id) < 0; });
            }
            if (_this.proposedVehicleToBedConfigs != null) {
                _this.proposedVehicleToBedConfigsSelectionCount = _this.proposedVehicleToBedConfigs.filter(function (item) { return item.isSelected; }).length;
            }
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToBedConfigAddComponent.prototype.onSelectAllProposedBedAssociations = function (event) {
        this.selectAllChecked = event;
        if (this.proposedVehicleToBedConfigs == null) {
            return;
        }
        this.proposedVehicleToBedConfigs.forEach(function (item) { return item.isSelected = event.target.checked; });
        this.proposedVehicleToBedConfigsSelectionCount = this.proposedVehicleToBedConfigs.filter(function (item) { return item.isSelected; }).length;
    };
    VehicleToBedConfigAddComponent.prototype.refreshProposedVehicleToBedConfigsSelectionCount = function (event, vehicleTobedconfig) {
        if (event.target.checked) {
            this.proposedVehicleToBedConfigsSelectionCount++;
            var excludedVehicle = this.proposedVehicleToBedConfigs.filter(function (item) { return item.isSelected; });
            if (excludedVehicle.length == this.proposedVehicleToBedConfigs.length - 1) {
                this.selectAllChecked = true;
            }
        }
        else {
            this.proposedVehicleToBedConfigsSelectionCount--;
            this.selectAllChecked = false;
        }
    };
    VehicleToBedConfigAddComponent.prototype.onSubmitAssociations = function () {
        if (!this.proposedVehicleToBedConfigs) {
            return;
        }
        var length = this.proposedVehicleToBedConfigs.length;
        if (this.proposedVehicleToBedConfigs.filter(function (item) { return item.isSelected; }).length == 0) {
            this.toastr.warning("No bed associations selected", constants_warehouse_1.ConstantsWarehouse.errorTitle);
            return;
        }
        this.submitAssociations(length);
    };
    VehicleToBedConfigAddComponent.prototype.getNextVehicleToBedConfig = function (index) {
        if (!this.proposedVehicleToBedConfigs || this.proposedVehicleToBedConfigs.length == 0) {
            return null;
        }
        var nextConfig = this.proposedVehicleToBedConfigs[index];
        return nextConfig;
    };
    VehicleToBedConfigAddComponent.prototype.submitAssociations = function (length) {
        var _this = this;
        this.showLoadingGif = true;
        length = length - 1;
        if (length >= 0) {
            var proposedVehicleToBedConfig_1 = this.getNextVehicleToBedConfig(length);
            proposedVehicleToBedConfig_1.comment = this.commenttoadd;
            var vehicleToBedConfigIdentity_1 = "(Vehicle ID: " + proposedVehicleToBedConfig_1.vehicle.id + ", Bed Config ID: " + proposedVehicleToBedConfig_1.bedConfig.id + ")";
            this.acFiles = this.acFileUploader.getAcFiles();
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    proposedVehicleToBedConfig_1.attachments = _this.sharedService.clone(uploadedFiles);
                }
                if (proposedVehicleToBedConfig_1.attachments) {
                    proposedVehicleToBedConfig_1.attachments = proposedVehicleToBedConfig_1.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.vehicleToBedConfigService.addVehicleToBedConfig(proposedVehicleToBedConfig_1).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle to Bed Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToBedConfigIdentity_1);
                        successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + vehicleToBedConfigIdentity_1 + "\" Vehicle to Bed Config change requestid  \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.acFileUploader.reset();
                        _this.acFileUploader.setAcFiles(_this.acFiles);
                        _this.submitAssociations(length);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle to Bed Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToBedConfigIdentity_1);
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                        _this.acFileUploader.reset();
                        _this.acFileUploader.setAcFiles(_this.acFiles);
                        _this.submitAssociations(length);
                    }
                }, function (error) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle to Bed Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToBedConfigIdentity_1);
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
    VehicleToBedConfigAddComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToBedConfigAddComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild('bedAssociationsPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToBedConfigAddComponent.prototype, "bedAssociationsPopup", void 0);
    __decorate([
        core_1.ViewChild('associationsPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToBedConfigAddComponent.prototype, "associationsPopup", void 0);
    VehicleToBedConfigAddComponent = __decorate([
        core_1.Component({
            selector: "vehicleToBedConfig-add-component",
            templateUrl: "app/templates/vehicleToBedConfig/vehicleToBedConfig-add.component.html",
            providers: [make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, submodel_service_1.SubModelService, region_service_1.RegionService, bedlength_service_1.BedLengthService, bedType_service_1.BedTypeService]
        }), 
        __metadata('design:paramtypes', [make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, submodel_service_1.SubModelService, region_service_1.RegionService, vehicleToBedConfig_service_1.VehicleToBedConfigService, vehicle_service_1.VehicleService, bedType_service_1.BedTypeService, bedlength_service_1.BedLengthService, bedConfig_service_1.BedConfigService, router_1.Router, shared_service_1.SharedService, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleToBedConfigAddComponent);
    return VehicleToBedConfigAddComponent;
}());
exports.VehicleToBedConfigAddComponent = VehicleToBedConfigAddComponent;
