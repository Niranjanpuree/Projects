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
var subModel_service_1 = require("../subModel/subModel.service");
var vehicleToDriveType_service_1 = require("../vehicleToDriveType/vehicleToDriveType.service");
var vehicle_service_1 = require("../vehicle/vehicle.service");
var DriveType_service_1 = require("../DriveType/DriveType.service");
var httpHelper_1 = require("../httpHelper");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var VehicleToDriveTypeAddComponent = (function () {
    function VehicleToDriveTypeAddComponent(makeService, modelService, yearService, baseVehicleService, subModelService, regionService, vehicleToDriveTypeService, vehicleService, driveTypeService, router, sharedService, toastr, navgationService) {
        this.makeService = makeService;
        this.modelService = modelService;
        this.yearService = yearService;
        this.baseVehicleService = baseVehicleService;
        this.subModelService = subModelService;
        this.regionService = regionService;
        this.vehicleToDriveTypeService = vehicleToDriveTypeService;
        this.vehicleService = vehicleService;
        this.driveTypeService = driveTypeService;
        this.router = router;
        this.sharedService = sharedService;
        this.toastr = toastr;
        this.navgationService = navgationService;
        this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        this.vehicles = [];
        this.driveType = { id: -1 };
        this.driveTypes = [];
        this.allDriveTypes = [];
        this.selectedDriveType = { id: -1 };
        this.acFiles = [];
        this.proposedVehicleToDriveTypesSelectionCount = 0;
        this.showLoadingGif = false;
        if (this.sharedService.vehicles) {
            this.vehicles = this.sharedService.vehicles;
        }
        if (this.sharedService.driveTypes) {
            this.driveTypes = this.sharedService.driveTypes;
        }
        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('vehicletodrivetype') > 0)
                this.backNavigationText = "Return to Drive Type System Search";
            else
                this.backNavigationText = "Return to Vehicle Search";
        }
    }
    VehicleToDriveTypeAddComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.models = [];
        this.years = [];
        this.subModels = [];
        this.regions = [];
        this.yearService.getYears().subscribe(function (m) {
            _this.years = m;
            _this.driveTypeService.getDriveTypes().subscribe(function (d) {
                _this.allDriveTypes = d;
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        }); //TODO: load all years that are attached to basevehicles
        this.selectAllChecked = false;
    };
    VehicleToDriveTypeAddComponent.prototype.onVehicleIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onVehicleIdSearch();
        }
    };
    VehicleToDriveTypeAddComponent.prototype.onVehicleIdSearch = function () {
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
    VehicleToDriveTypeAddComponent.prototype.onSelectYear = function () {
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
    VehicleToDriveTypeAddComponent.prototype.onSelectMake = function () {
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
    VehicleToDriveTypeAddComponent.prototype.onSelectModel = function () {
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
    VehicleToDriveTypeAddComponent.prototype.onSelectSubModel = function () {
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
    VehicleToDriveTypeAddComponent.prototype.onSelectRegion = function () {
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
    VehicleToDriveTypeAddComponent.prototype.onRemoveVehicle = function (vehicleId) {
        if (confirm("Remove Vehicle Id " + vehicleId + " from selection?")) {
            this.vehicles = this.vehicles.filter(function (item) { return item.id != vehicleId; });
            this.refreshProposedVehicleToDriveTypes();
        }
    };
    VehicleToDriveTypeAddComponent.prototype.onAddVehicleToSelection = function () {
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
            this.refreshProposedVehicleToDriveTypes();
            this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        }
    };
    VehicleToDriveTypeAddComponent.prototype.onViewDriveTypeAssociations = function (vehicle) {
        var _this = this;
        this.popupVehicle = vehicle;
        this.driveTypeAssociationsPopup.open("lg");
        if (!this.popupVehicle.vehicleToDriveTypes) {
            this.vehicleToDriveTypeService.getByVehicleId(this.popupVehicle.id).subscribe(function (m) {
                _this.popupVehicle.vehicleToDriveTypes = m;
                _this.popupVehicle.vehicleToDriveTypeCount = m.length;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    VehicleToDriveTypeAddComponent.prototype.onDriveTypeIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onDriveTypeIdSearch();
        }
    };
    VehicleToDriveTypeAddComponent.prototype.onDriveTypeIdSearch = function () {
        var _this = this;
        var driveTypeId = Number(this.driveTypeIdSearchText);
        if (isNaN(driveTypeId)) {
            this.toastr.warning("Invalid Drive Type ID", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.driveType.id == driveTypeId) {
            return;
        }
        this.driveType = { id: -1 };
        this.showLoadingGif = true;
        this.driveTypeService.getDriveType(driveTypeId).subscribe(function (bc) {
            _this.driveType = bc;
            _this.showLoadingGif = false;
        }, function (error) {
            var errorMessage = JSON.parse(String(error)).message;
            _this.toastr.warning(errorMessage, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToDriveTypeAddComponent.prototype.onAddDriveTypeToSelection = function () {
        var _this = this;
        //TODO: do not allow addition if this item has an open CR
        if (this.driveType.id == -1) {
            this.toastr.warning("Drive Type ID not available.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        var filteredDriveTypes = this.driveTypes.filter(function (item) { return item.id == _this.driveType.id; });
        if (filteredDriveTypes && filteredDriveTypes.length) {
            this.toastr.warning("Selected Drive Type ID already added", constants_warehouse_1.ConstantsWarehouse.validationTitle);
        }
        else {
            this.driveTypes.push(this.driveType);
            this.refreshProposedVehicleToDriveTypes();
            this.driveType = { id: -1 };
        }
        this.selectAllChecked = true;
    };
    VehicleToDriveTypeAddComponent.prototype.getDefaultInputModel = function () {
        return {
            driveTypeId: 0,
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
            driveTypes: []
        };
    };
    VehicleToDriveTypeAddComponent.prototype.onViewAssociations = function (driveType) {
        var _this = this;
        this.popupDriveType = driveType;
        this.associationsPopup.open("lg");
        if (!this.popupDriveType.vehicleToDriveTypes) {
            var inputModel = this.getDefaultInputModel();
            inputModel.driveTypeId = this.popupDriveType.id;
            this.vehicleToDriveTypeService.getAssociations(inputModel).subscribe(function (m) {
                _this.popupDriveType.vehicleToDriveTypes = m;
                _this.popupDriveType.vehicleToDriveTypeCount = m.length;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    VehicleToDriveTypeAddComponent.prototype.onRemoveDriveType = function (driveTypeId) {
        if (confirm("Remove Drive Type Id " + driveTypeId + " from selection?")) {
            this.driveTypes = this.driveTypes.filter(function (item) { return item.id != driveTypeId; });
            this.refreshProposedVehicleToDriveTypes();
        }
    };
    VehicleToDriveTypeAddComponent.prototype.refreshProposedVehicleToDriveTypes = function () {
        var _this = this;
        if (this.vehicles.length == 0 || this.driveTypes.length == 0) {
            this.proposedVehicleToDriveTypes = [];
            return;
        }
        if (this.showLoadingGif) {
            return;
        }
        this.showLoadingGif = true;
        var allProposedVehicleToDriveTypes = [];
        this.vehicles.forEach(function (v) {
            _this.driveTypes.forEach(function (b) {
                allProposedVehicleToDriveTypes.push({
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
                        vehicleToDriveTypeCount: null,
                        isSelected: false
                    },
                    driveType: {
                        id: b.id,
                        name: b.name,
                        vehicleToDriveTypeCount: 0,
                    },
                    isSelected: true
                });
            });
        });
        var selectedVehicleIds = this.vehicles.map(function (x) { return x.id; });
        var selectedDriveTypeIds = this.driveTypes.map(function (x) { return x.id; });
        this.vehicleToDriveTypeService.getVehicleToDriveTypesByVehicleIdsAndDriveTypeIds(selectedVehicleIds, selectedDriveTypeIds)
            .subscribe(function (m) {
            _this.proposedVehicleToDriveTypes = [];
            _this.existingVehicleToDriveTypes = m;
            if (_this.existingVehicleToDriveTypes == null || _this.existingVehicleToDriveTypes.length == 0) {
                _this.proposedVehicleToDriveTypes = allProposedVehicleToDriveTypes;
            }
            else {
                var existingVehicleIds_1 = _this.existingVehicleToDriveTypes.map(function (x) { return x.vehicle.id; });
                var existingDriveTypeIds_1 = _this.existingVehicleToDriveTypes.map(function (x) { return x.driveType.id; });
                _this.proposedVehicleToDriveTypes = allProposedVehicleToDriveTypes.filter(function (item) { return existingVehicleIds_1.indexOf(item.vehicle.id) < 0 || existingDriveTypeIds_1.indexOf(item.driveType.id) < 0; });
            }
            if (_this.proposedVehicleToDriveTypes != null) {
                _this.proposedVehicleToDriveTypesSelectionCount = _this.proposedVehicleToDriveTypes.filter(function (item) { return item.isSelected; }).length;
            }
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToDriveTypeAddComponent.prototype.onSelectAllProposedDriveTypeAssociations = function (event) {
        this.selectAllChecked = event;
        if (this.proposedVehicleToDriveTypes == null) {
            return;
        }
        this.proposedVehicleToDriveTypes.forEach(function (item) { return item.isSelected = event.target.checked; });
        this.proposedVehicleToDriveTypesSelectionCount = this.proposedVehicleToDriveTypes.filter(function (item) { return item.isSelected; }).length;
    };
    VehicleToDriveTypeAddComponent.prototype.refreshProposedVehicleToDriveTypesSelectionCount = function (event, vehicleToDriveType) {
        if (event.target.checked) {
            this.proposedVehicleToDriveTypesSelectionCount++;
            var excludedVehicle = this.proposedVehicleToDriveTypes.filter(function (item) { return item.isSelected; });
            if (excludedVehicle.length == this.proposedVehicleToDriveTypes.length - 1) {
                this.selectAllChecked = true;
            }
        }
        else {
            this.proposedVehicleToDriveTypesSelectionCount--;
            this.selectAllChecked = false;
        }
    };
    VehicleToDriveTypeAddComponent.prototype.onSubmitAssociations = function () {
        if (!this.proposedVehicleToDriveTypes) {
            return;
        }
        var length = this.proposedVehicleToDriveTypes.length;
        if (this.proposedVehicleToDriveTypes.filter(function (item) { return item.isSelected; }).length == 0) {
            this.toastr.warning("No drive type associations selected", constants_warehouse_1.ConstantsWarehouse.errorTitle);
            return;
        }
        this.submitAssociations(length);
    };
    VehicleToDriveTypeAddComponent.prototype.getNextVehicleToDriveType = function (index) {
        if (!this.proposedVehicleToDriveTypes || this.proposedVehicleToDriveTypes.length == 0) {
            return null;
        }
        var nextConfig = this.proposedVehicleToDriveTypes[index];
        return nextConfig;
    };
    VehicleToDriveTypeAddComponent.prototype.submitAssociations = function (length) {
        var _this = this;
        this.showLoadingGif = true;
        length = length - 1;
        if (length >= 0) {
            var proposedVehicleToDriveType_1 = this.getNextVehicleToDriveType(length);
            proposedVehicleToDriveType_1.comment = this.commenttoadd;
            var vehicleToDriveTypeIdentity_1 = "(Vehicle ID: " + proposedVehicleToDriveType_1.vehicle.id + ", Drive Type ID: " + proposedVehicleToDriveType_1.driveType.id + ")";
            this.acFiles = this.acFileUploader.getAcFiles();
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    proposedVehicleToDriveType_1.attachments = _this.sharedService.clone(uploadedFiles);
                }
                if (proposedVehicleToDriveType_1.attachments) {
                    proposedVehicleToDriveType_1.attachments = proposedVehicleToDriveType_1.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.vehicleToDriveTypeService.addVehicleToDriveType(proposedVehicleToDriveType_1).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle to Drive Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToDriveTypeIdentity_1);
                        successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + vehicleToDriveTypeIdentity_1 + "\" Vehicle to Drive Type change requestid  \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.acFileUploader.reset();
                        _this.acFileUploader.setAcFiles(_this.acFiles);
                        _this.submitAssociations(length);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle to Drive Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToDriveTypeIdentity_1);
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                        _this.acFileUploader.reset();
                        _this.acFileUploader.setAcFiles(_this.acFiles);
                        _this.submitAssociations(length);
                    }
                }, function (error) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle to Drive Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToDriveTypeIdentity_1);
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
    VehicleToDriveTypeAddComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    VehicleToDriveTypeAddComponent.prototype.onSelectDriveType = function () {
        //this.driveType.id = -1;
        //if (this.driveType.id == -1) {
        //    return;
        //}
        var _this = this;
        this.driveTypeService.getDriveType(this.driveType.id).subscribe(function (m) { return _this.driveType = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToDriveTypeAddComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild('driveTypeAssociationsPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToDriveTypeAddComponent.prototype, "driveTypeAssociationsPopup", void 0);
    __decorate([
        core_1.ViewChild('associationsPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToDriveTypeAddComponent.prototype, "associationsPopup", void 0);
    VehicleToDriveTypeAddComponent = __decorate([
        core_1.Component({
            selector: "vehicleToDriveType-add-component",
            templateUrl: "app/templates/vehicleToDriveType/vehicleToDriveType-add.component.html",
            providers: [make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, subModel_service_1.SubModelService, region_service_1.RegionService, vehicleToDriveType_service_1.VehicleToDriveTypeService, DriveType_service_1.DriveTypeService, httpHelper_1.HttpHelper]
        }), 
        __metadata('design:paramtypes', [make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, subModel_service_1.SubModelService, region_service_1.RegionService, vehicleToDriveType_service_1.VehicleToDriveTypeService, vehicle_service_1.VehicleService, DriveType_service_1.DriveTypeService, router_1.Router, shared_service_1.SharedService, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleToDriveTypeAddComponent);
    return VehicleToDriveTypeAddComponent;
}());
exports.VehicleToDriveTypeAddComponent = VehicleToDriveTypeAddComponent;
