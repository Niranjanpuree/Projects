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
var vehicleToBodyStyleConfig_service_1 = require("./vehicleToBodyStyleConfig.service");
var vehicle_service_1 = require("../vehicle/vehicle.service");
var bodyStyleConfig_service_1 = require("../bodyStyleConfig/bodyStyleConfig.service");
var bodyNumDoors_service_1 = require("../bodyNumDoors/bodyNumDoors.service");
var bodyType_service_1 = require("../bodyType/bodyType.service");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var VehicleToBodyStyleConfigAddComponent = (function () {
    function VehicleToBodyStyleConfigAddComponent(makeService, modelService, yearService, baseVehicleService, subModelService, regionService, vehicleToBodyStyleConfigService, vehicleService, bodyTypeService, bodyStyleConfigService, bodyNumDoorsService, router, sharedService, toastr, navgationService) {
        this.makeService = makeService;
        this.modelService = modelService;
        this.yearService = yearService;
        this.baseVehicleService = baseVehicleService;
        this.subModelService = subModelService;
        this.regionService = regionService;
        this.vehicleToBodyStyleConfigService = vehicleToBodyStyleConfigService;
        this.vehicleService = vehicleService;
        this.bodyTypeService = bodyTypeService;
        this.bodyStyleConfigService = bodyStyleConfigService;
        this.bodyNumDoorsService = bodyNumDoorsService;
        this.router = router;
        this.sharedService = sharedService;
        this.toastr = toastr;
        this.navgationService = navgationService;
        this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        this.vehicles = [];
        this.bodyStyleConfig = { id: -1, bodyNumDoorsId: -1, bodyTypeId: -1 };
        this.bodyStyleConfigs = [];
        this.acFiles = [];
        this.proposedVehicleToBodyStyleConfigsSelectionCount = 0;
        this.showLoadingGif = false;
        if (this.sharedService.vehicles) {
            this.vehicles = this.sharedService.vehicles;
        }
        if (this.sharedService.bodyStyleConfigs) {
            this.bodyStyleConfigs = this.sharedService.bodyStyleConfigs;
        }
        this.backNavigation = this.navgationService.backRoute;
        this.backNavigationText = "Return To DashBoard";
        if (this.navgationService.backRoute) {
            var navigate = this.sharedService.systemMenubarSelected;
            if (this.backNavigation == '/vehicle/search') {
                this.backNavigationText = "Return To Vehicle Search";
            }
            if (navigate == 'Body') {
                this.backNavigationText = "Return To Body System Search";
            }
        }
    }
    VehicleToBodyStyleConfigAddComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.years = [];
        this.makes = [];
        this.models = [];
        this.subModels = [];
        this.regions = [];
        this.bodyTypes = [];
        this.yearService.getYears().subscribe(function (m) {
            _this.years = m;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        }); //TODO: load all years that are attached to basevehicles
        this.bodyNumDoorsService.getAllBodyNumDoors().subscribe(function (m) { return _this.bodyNumDoors = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        this.selectAllChecked = false;
    };
    VehicleToBodyStyleConfigAddComponent.prototype.onVehicleIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onVehicleIdSearch();
        }
    };
    VehicleToBodyStyleConfigAddComponent.prototype.onVehicleIdSearch = function () {
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
    VehicleToBodyStyleConfigAddComponent.prototype.onSelectYear = function () {
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
    VehicleToBodyStyleConfigAddComponent.prototype.onSelectMake = function () {
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
    VehicleToBodyStyleConfigAddComponent.prototype.onSelectModel = function () {
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
    VehicleToBodyStyleConfigAddComponent.prototype.onSelectSubModel = function () {
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
    VehicleToBodyStyleConfigAddComponent.prototype.onSelectRegion = function () {
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
    VehicleToBodyStyleConfigAddComponent.prototype.onRemoveVehicle = function (vehicleId) {
        if (confirm("Remove Vehicle Id " + vehicleId + " from selection?")) {
            this.vehicles = this.vehicles.filter(function (item) { return item.id != vehicleId; });
            this.refreshProposedVehicleToBodyStyleConfigs();
        }
    };
    VehicleToBodyStyleConfigAddComponent.prototype.onAddVehicleToSelection = function () {
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
            this.vehicles.push(this.vehicle);
            this.refreshProposedVehicleToBodyStyleConfigs();
            this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        }
    };
    VehicleToBodyStyleConfigAddComponent.prototype.onViewBodyAssociations = function (vehicle) {
        var _this = this;
        this.popupVehicle = vehicle;
        this.bodyAssociationsPopup.open("lg");
        if (!this.popupVehicle.vehicleToBodyStyleConfigs) {
            this.vehicleToBodyStyleConfigService.getByVehicleId(this.popupVehicle.id).subscribe(function (m) {
                _this.popupVehicle.vehicleToBodyStyleConfigs = m;
                _this.popupVehicle.vehicleToBodyStyleConfigCount = m.length;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    VehicleToBodyStyleConfigAddComponent.prototype.onbodyStyleConfigIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onBodyStyleConfigIdSearch();
        }
    };
    VehicleToBodyStyleConfigAddComponent.prototype.onBodyStyleConfigIdSearch = function () {
        var _this = this;
        var bodyStyleConfigId = Number(this.bodyStyleConfigIdSearchText);
        if (isNaN(bodyStyleConfigId)) {
            this.toastr.warning("Invalid Body Style ID", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.bodyStyleConfig.id == bodyStyleConfigId) {
            return;
        }
        this.bodyStyleConfig = { id: -1, bodyNumDoorsId: -1, bodyTypeId: -1 };
        var savedBodyTypes = this.bodyTypes;
        var savedBodyNumDoors = this.bodyNumDoors;
        this.bodyTypes = null;
        this.showLoadingGif = true;
        this.bodyStyleConfigService.getBodyStyleConfig(bodyStyleConfigId).subscribe(function (bc) {
            _this.bodyTypeService.getByBodyNumDoorsId(bc.bodyNumDoorsId).subscribe(function (r) {
                _this.bodyTypes = r;
                _this.bodyStyleConfig = bc;
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }, function (error) {
            var errorMessage = JSON.parse(String(error)).message;
            _this.toastr.warning(errorMessage, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.bodyTypes = savedBodyTypes;
            _this.bodyNumDoors = savedBodyNumDoors;
            _this.showLoadingGif = false;
        });
    };
    VehicleToBodyStyleConfigAddComponent.prototype.onSelectBodyNumberDoors = function () {
        var _this = this;
        this.bodyStyleConfig.id = -1;
        this.bodyStyleConfig.bodyTypeId = -1;
        this.bodyTypes = [];
        if (this.bodyStyleConfig.bodyNumDoorsId == -1) {
            this.bodyTypes = [];
            return;
        }
        this.bodyTypes = null;
        this.bodyTypeService.getByBodyNumDoorsId(this.bodyStyleConfig.bodyNumDoorsId).subscribe(function (m) { return _this.bodyTypes = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    VehicleToBodyStyleConfigAddComponent.prototype.onSelectBodyType = function () {
        var _this = this;
        this.bodyStyleConfig.id = -1;
        if (this.bodyStyleConfig.bodyTypeId == -1) {
            return;
        }
        this.bodyStyleConfigService.getByChildIds(this.bodyStyleConfig.bodyNumDoorsId, this.bodyStyleConfig.bodyTypeId).subscribe(function (m) { return _this.bodyStyleConfig = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    VehicleToBodyStyleConfigAddComponent.prototype.onAddBodyStyleConfigToSelection = function () {
        //TODO: do not allow addition if this item has an open CR
        var _this = this;
        if (this.bodyStyleConfig.bodyNumDoorsId == -1) {
            this.toastr.warning("Please select BodyNum Doors.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.bodyStyleConfig.bodyTypeId == -1) {
            this.toastr.warning("Please select Body Type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.bodyStyleConfig.id == -1) {
            this.toastr.warning("Body Style Config ID not available.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        var filteredBodyStyleConfigs = this.bodyStyleConfigs.filter(function (item) { return item.id == _this.bodyStyleConfig.id; });
        if (filteredBodyStyleConfigs && filteredBodyStyleConfigs.length) {
            this.toastr.warning("Selected Body Style Config ID already added", constants_warehouse_1.ConstantsWarehouse.validationTitle);
        }
        else {
            this.bodyStyleConfig.numDoors = this.bodyNumDoors.filter(function (item) { return item.id == _this.bodyStyleConfig.bodyNumDoorsId; })[0].numDoors;
            this.bodyStyleConfig.bodyTypeName = this.bodyTypes.filter(function (item) { return item.id == _this.bodyStyleConfig.bodyTypeId; })[0].name;
            this.bodyStyleConfigs.push(this.bodyStyleConfig);
            this.refreshProposedVehicleToBodyStyleConfigs();
            this.bodyStyleConfig = { id: -1, bodyNumDoorsId: -1, bodyTypeId: -1 };
        }
        this.selectAllChecked = true;
    };
    VehicleToBodyStyleConfigAddComponent.prototype.onViewAssociations = function (bodyStyleConfig) {
        var _this = this;
        this.popupBodyStyleConfig = bodyStyleConfig;
        this.associationsPopup.open("lg");
        if (!this.popupBodyStyleConfig.vehicleToBodyStyleConfigs) {
            var inputModel = this.getDefaultInputModel();
            inputModel.bodyStyleConfigId = this.popupBodyStyleConfig.id;
            this.vehicleToBodyStyleConfigService.getAssociations(inputModel).subscribe(function (m) {
                _this.popupBodyStyleConfig.vehicleToBodyStyleConfigs = m;
                _this.popupBodyStyleConfig.vehicleToBodyStyleConfigCount = m.length;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    VehicleToBodyStyleConfigAddComponent.prototype.getDefaultInputModel = function () {
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
    VehicleToBodyStyleConfigAddComponent.prototype.onRemoveBodyStyleConfig = function (bodyStyleConfigId) {
        if (confirm("Remove Body Style Config Id " + bodyStyleConfigId + " from selection?")) {
            this.bodyStyleConfigs = this.bodyStyleConfigs.filter(function (item) { return item.id != bodyStyleConfigId; });
            this.refreshProposedVehicleToBodyStyleConfigs();
        }
    };
    VehicleToBodyStyleConfigAddComponent.prototype.refreshProposedVehicleToBodyStyleConfigs = function () {
        var _this = this;
        if (this.vehicles.length == 0 || this.bodyStyleConfigs.length == 0) {
            return;
        }
        if (this.showLoadingGif) {
            return;
        }
        this.showLoadingGif = true;
        var allProposedVehicleToBodyStyleConfigs = [];
        this.vehicles.forEach(function (v) {
            _this.bodyStyleConfigs.forEach(function (b) {
                allProposedVehicleToBodyStyleConfigs.push({
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
                        vehicleToBodyStyleConfigCount: null,
                        isSelected: false
                    },
                    bodyStyleConfig: {
                        id: b.id,
                        bodyNumDoorsId: b.bodyNumDoorsId,
                        bodyTypeId: b.bodyTypeId,
                        bodyTypeName: b.bodyTypeName,
                        vehicleToBodyStyleConfigCount: b.vehicleToBodyStyleConfigCount,
                        numDoors: b.numDoors
                    },
                    numberOfBodyAssociation: -1,
                    isSelected: true
                });
            });
        });
        var selectedVehicleIds = this.vehicles.map(function (x) { return x.id; });
        var selectedBodyStyleConfigStyleIds = this.bodyStyleConfigs.map(function (x) { return x.id; });
        this.vehicleToBodyStyleConfigService.getVehicleToBodyStyleConfigsByVehicleIdsAndBodyStyleConfigIds(selectedVehicleIds, selectedBodyStyleConfigStyleIds)
            .subscribe(function (m) {
            _this.proposedVehicleToBodyStyleConfigs = [];
            _this.existingVehicleToBodyStyleConfigs = m;
            if (_this.existingVehicleToBodyStyleConfigs == null || _this.existingVehicleToBodyStyleConfigs.length == 0) {
                _this.proposedVehicleToBodyStyleConfigs = allProposedVehicleToBodyStyleConfigs;
            }
            else {
                var existingVehicleIds_1 = _this.existingVehicleToBodyStyleConfigs.map(function (x) { return x.vehicle.id; });
                var existingBodyStyleConfigIds_1 = _this.existingVehicleToBodyStyleConfigs.map(function (x) { return x.bodyStyleConfig.id; });
                _this.proposedVehicleToBodyStyleConfigs = allProposedVehicleToBodyStyleConfigs.filter(function (item) { return existingVehicleIds_1.indexOf(item.vehicle.id) < 0 || existingBodyStyleConfigIds_1.indexOf(item.bodyStyleConfig.id) < 0; });
            }
            if (_this.proposedVehicleToBodyStyleConfigs != null) {
                _this.proposedVehicleToBodyStyleConfigsSelectionCount = _this.proposedVehicleToBodyStyleConfigs.filter(function (item) { return item.isSelected; }).length;
            }
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToBodyStyleConfigAddComponent.prototype.onSelectAllProposedBodyAssociations = function (event) {
        this.selectAllChecked = event;
        if (this.proposedVehicleToBodyStyleConfigs == null) {
            return;
        }
        this.proposedVehicleToBodyStyleConfigs.forEach(function (item) { return item.isSelected = event.target.checked; });
        this.proposedVehicleToBodyStyleConfigsSelectionCount = this.proposedVehicleToBodyStyleConfigs.filter(function (item) { return item.isSelected; }).length;
    };
    VehicleToBodyStyleConfigAddComponent.prototype.refreshProposedVehicleToBodyStyleConfigsSelectionCount = function (event, vehicleToBodyStyleconfig) {
        if (event.target.checked) {
            this.proposedVehicleToBodyStyleConfigsSelectionCount++;
            var excludedVehicle = this.proposedVehicleToBodyStyleConfigs.filter(function (item) { return item.isSelected; });
            if (excludedVehicle.length == this.proposedVehicleToBodyStyleConfigs.length - 1) {
                this.selectAllChecked = true;
            }
        }
        else {
            this.proposedVehicleToBodyStyleConfigsSelectionCount--;
            this.selectAllChecked = false;
        }
    };
    VehicleToBodyStyleConfigAddComponent.prototype.onSubmitAssociations = function () {
        if (!this.proposedVehicleToBodyStyleConfigs) {
            return;
        }
        var length = this.proposedVehicleToBodyStyleConfigs.length;
        if (this.proposedVehicleToBodyStyleConfigs.filter(function (item) { return item.isSelected; }).length == 0) {
            this.toastr.warning("No Body Associations Selected", constants_warehouse_1.ConstantsWarehouse.errorTitle);
            return;
        }
        this.submitAssociations(length);
    };
    VehicleToBodyStyleConfigAddComponent.prototype.getNextVehicleToBodyStyleConfig = function (index) {
        if (!this.proposedVehicleToBodyStyleConfigs || this.proposedVehicleToBodyStyleConfigs.length == 0) {
            return null;
        }
        var nextConfig = this.proposedVehicleToBodyStyleConfigs[index];
        return nextConfig;
    };
    VehicleToBodyStyleConfigAddComponent.prototype.submitAssociations = function (length) {
        var _this = this;
        this.showLoadingGif = true;
        length = length - 1;
        if (length >= 0) {
            var proposedVehicleToBodyStyleConfig_1 = this.getNextVehicleToBodyStyleConfig(length);
            proposedVehicleToBodyStyleConfig_1.comment = this.commenttoadd;
            var vehicleToBodyStyleConfigIdentity_1 = "(Vehicle ID: " + proposedVehicleToBodyStyleConfig_1.vehicle.id + ", Body Style Config ID: " + proposedVehicleToBodyStyleConfig_1.bodyStyleConfig.id + ")";
            this.acFiles = this.acFileUploader.getAcFiles();
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    proposedVehicleToBodyStyleConfig_1.attachments = _this.sharedService.clone(uploadedFiles);
                }
                if (proposedVehicleToBodyStyleConfig_1.attachments) {
                    proposedVehicleToBodyStyleConfig_1.attachments = proposedVehicleToBodyStyleConfig_1.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.vehicleToBodyStyleConfigService.addVehicleToBodyStyleConfig(proposedVehicleToBodyStyleConfig_1).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle to Body Style Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, proposedVehicleToBodyStyleConfig_1);
                        successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + vehicleToBodyStyleConfigIdentity_1 + "\" Vehicle to Body Config change requestid  \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.acFileUploader.reset();
                        _this.acFileUploader.setAcFiles(_this.acFiles);
                        _this.submitAssociations(length);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle to Body Style Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToBodyStyleConfigIdentity_1);
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                        _this.acFileUploader.reset();
                        _this.acFileUploader.setAcFiles(_this.acFiles);
                        _this.submitAssociations(length);
                    }
                }, function (error) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle to Body Style Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToBodyStyleConfigIdentity_1);
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
    VehicleToBodyStyleConfigAddComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToBodyStyleConfigAddComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild('bodyAssociationsPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToBodyStyleConfigAddComponent.prototype, "bodyAssociationsPopup", void 0);
    __decorate([
        core_1.ViewChild('associationsPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToBodyStyleConfigAddComponent.prototype, "associationsPopup", void 0);
    VehicleToBodyStyleConfigAddComponent = __decorate([
        core_1.Component({
            selector: "vehicleToBodyStyleConfig-add-component",
            templateUrl: "app/templates/vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-add.component.html",
            providers: [make_service_1.MakeService, bodyNumDoors_service_1.BodyNumDoorsService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, submodel_service_1.SubModelService, region_service_1.RegionService, bodyType_service_1.BodyTypeService]
        }), 
        __metadata('design:paramtypes', [make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, submodel_service_1.SubModelService, region_service_1.RegionService, vehicleToBodyStyleConfig_service_1.VehicleToBodyStyleConfigService, vehicle_service_1.VehicleService, bodyType_service_1.BodyTypeService, bodyStyleConfig_service_1.BodyStyleConfigService, bodyNumDoors_service_1.BodyNumDoorsService, router_1.Router, shared_service_1.SharedService, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleToBodyStyleConfigAddComponent);
    return VehicleToBodyStyleConfigAddComponent;
}());
exports.VehicleToBodyStyleConfigAddComponent = VehicleToBodyStyleConfigAddComponent;
