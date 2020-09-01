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
var vehicleToMfrBodyCode_service_1 = require("./vehicleToMfrBodyCode.service");
var vehicle_service_1 = require("../vehicle/vehicle.service");
var mfrBodyCode_service_1 = require("../mfrBodyCode/mfrBodyCode.service");
var httpHelper_1 = require("../httpHelper");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var VehicleToMfrBodyCodeAddComponent = (function () {
    function VehicleToMfrBodyCodeAddComponent(makeService, modelService, yearService, baseVehicleService, subModelService, regionService, vehicleToMfrBodyCodeService, vehicleService, mfrBodyCodeService, router, sharedService, toastr, navgationService) {
        this.makeService = makeService;
        this.modelService = modelService;
        this.yearService = yearService;
        this.baseVehicleService = baseVehicleService;
        this.subModelService = subModelService;
        this.regionService = regionService;
        this.vehicleToMfrBodyCodeService = vehicleToMfrBodyCodeService;
        this.vehicleService = vehicleService;
        this.mfrBodyCodeService = mfrBodyCodeService;
        this.router = router;
        this.sharedService = sharedService;
        this.toastr = toastr;
        this.navgationService = navgationService;
        this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        this.vehicles = [];
        this.mfrBodyCode = { id: -1 };
        this.mfrBodyCodes = [];
        this.mfrBodyCodesForSelection = [];
        this.acFiles = [];
        this.proposedVehicleToMfrBodyCodesSelectionCount = 0;
        this.showLoadingGif = false;
        if (this.sharedService.vehicles) {
            this.vehicles = this.sharedService.vehicles;
        }
        if (this.sharedService.mfrBodyCodes) {
            this.mfrBodyCodes = this.sharedService.mfrBodyCodes;
        }
        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('vehicle') > 0)
                this.backNavigationText = "Return to Mfr Body Code Search";
            else
                this.backNavigationText = "Return to Vehicle Search";
        }
    }
    VehicleToMfrBodyCodeAddComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.years = [];
        this.makes = [];
        this.models = [];
        this.subModels = [];
        this.regions = [];
        this.mfrBodyCodes = [];
        this.yearService.getYears().subscribe(function (m) {
            _this.years = m;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        }); //TODO: load all years that are attached to basevehicles
        this.mfrBodyCodeService.getMfrBodyCodes().subscribe(function (m) { return _this.mfrBodyCodesForSelection = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        this.selectAllChecked = false;
    };
    VehicleToMfrBodyCodeAddComponent.prototype.onVehicleIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onVehicleIdSearch();
        }
    };
    VehicleToMfrBodyCodeAddComponent.prototype.onVehicleIdSearch = function () {
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
    VehicleToMfrBodyCodeAddComponent.prototype.onSelectYear = function () {
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
    VehicleToMfrBodyCodeAddComponent.prototype.onSelectMake = function () {
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
    VehicleToMfrBodyCodeAddComponent.prototype.onSelectModel = function () {
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
    VehicleToMfrBodyCodeAddComponent.prototype.onSelectSubModel = function () {
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
    VehicleToMfrBodyCodeAddComponent.prototype.onSelectRegion = function () {
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
    VehicleToMfrBodyCodeAddComponent.prototype.onRemoveVehicle = function (vehicleId) {
        if (confirm("Remove Vehicle Id " + vehicleId + " from selection?")) {
            this.vehicles = this.vehicles.filter(function (item) { return item.id != vehicleId; });
            this.refreshProposedVehicleToMfrBodyCodes();
        }
    };
    VehicleToMfrBodyCodeAddComponent.prototype.onAddVehicleToSelection = function () {
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
            this.refreshProposedVehicleToMfrBodyCodes();
            this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        }
    };
    VehicleToMfrBodyCodeAddComponent.prototype.onViewMfrBodyCodeAssociation = function (vehicle) {
        var _this = this;
        this.popupVehicle = vehicle;
        this.mfrBodyCodeAssociationsPopup.open("lg");
        if (!this.popupVehicle.vehicleToMfrBodyCodes) {
            this.vehicleToMfrBodyCodeService.getByVehicleId(this.popupVehicle.id).subscribe(function (m) {
                _this.popupVehicle.vehicleToMfrBodyCodes = m;
                _this.popupVehicle.vehicleToMfrBodyCodeCount = m.length;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    VehicleToMfrBodyCodeAddComponent.prototype.onMfrBodyCodeIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onMfrBodyCodeIdSearch();
        }
    };
    VehicleToMfrBodyCodeAddComponent.prototype.onMfrBodyCodeIdSearch = function () {
        var _this = this;
        var mfrBodyCodeId = Number(this.mfrBodyCodeIdSearchText);
        if (isNaN(mfrBodyCodeId)) {
            this.toastr.warning("Invalid Mfr Body Code ID", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.mfrBodyCode.id == mfrBodyCodeId) {
            return;
        }
        this.mfrBodyCode = { id: -1 };
        var savedMfrBodyCodes = this.mfrBodyCodes;
        this.showLoadingGif = true;
        this.mfrBodyCodeService.getMfrBodyCode(mfrBodyCodeId).subscribe(function (mbc) {
            _this.mfrBodyCode = mbc;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
        (function (error) {
            var errorMessage = JSON.parse(String(error)).message;
            _this.toastr.warning(errorMessage, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToMfrBodyCodeAddComponent.prototype.onAddMfrBodyCodeToSelection = function () {
        var _this = this;
        //TODO: do not allow addition if this item has an open CR
        if (this.mfrBodyCode.id == -1) {
            this.toastr.warning("Please select Mfr Body Code.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.mfrBodyCode.id == -1) {
            this.toastr.warning("Please select Mfr Body Code.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        var filteredMfrBodyCodes = this.mfrBodyCodes.filter(function (item) { return item.id == _this.mfrBodyCode.id; });
        if (filteredMfrBodyCodes && filteredMfrBodyCodes.length) {
            this.toastr.warning("Selected Mfr Body Code ID already added", constants_warehouse_1.ConstantsWarehouse.validationTitle);
        }
        else {
            this.mfrBodyCode.name = this.mfrBodyCodesForSelection.filter(function (item) { return item.id == _this.mfrBodyCode.id; })[0].name;
            this.mfrBodyCodes.push(this.mfrBodyCode);
            this.refreshProposedVehicleToMfrBodyCodes();
            this.mfrBodyCode = { id: -1 };
        }
        this.selectAllChecked = true;
    };
    VehicleToMfrBodyCodeAddComponent.prototype.onViewAssociations = function (mfrBodyCode) {
        var _this = this;
        this.popupMfrBodyCode = mfrBodyCode;
        this.associationsPopup.open("lg");
        if (!this.popupMfrBodyCode.vehicleToMfrBodyCodes) {
            var inputModel = this.getDefaultInputModel();
            inputModel.mfrBodyCodeId = this.popupMfrBodyCode.id;
            this.vehicleToMfrBodyCodeService.getAssociations(inputModel).subscribe(function (m) {
                _this.popupMfrBodyCode.vehicleToMfrBodyCodes = m;
                _this.popupMfrBodyCode.vehicleToMfrBodyCodeCount = m.length;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    VehicleToMfrBodyCodeAddComponent.prototype.getDefaultInputModel = function () {
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
    VehicleToMfrBodyCodeAddComponent.prototype.onRemoveMfrBodyCode = function (mfrBodyCodeId) {
        if (confirm("Remove Mfr Body Code Id " + mfrBodyCodeId + " from selection?")) {
            this.mfrBodyCodes = this.mfrBodyCodes.filter(function (item) { return item.id != mfrBodyCodeId; });
            this.refreshProposedVehicleToMfrBodyCodes();
        }
    };
    VehicleToMfrBodyCodeAddComponent.prototype.refreshProposedVehicleToMfrBodyCodes = function () {
        var _this = this;
        if (this.vehicles.length == 0 || this.mfrBodyCodes.length == 0) {
            return;
        }
        if (this.showLoadingGif) {
            return;
        }
        this.showLoadingGif = true;
        var allProposedVehicleToMfrBodyCodes = [];
        this.vehicles.forEach(function (v) {
            _this.mfrBodyCodes.forEach(function (b) {
                allProposedVehicleToMfrBodyCodes.push({
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
                        vehicleToMfrBodyCodeCount: null,
                        isSelected: false
                    },
                    mfrBodyCode: {
                        id: b.id,
                        name: b.name,
                        vehicleToMfrBodyCodeCount: 0
                    },
                    numberOfMfrBodyCodesAssociation: -1,
                    isSelected: true
                });
            });
        });
        var selectedVehicleIds = this.vehicles.map(function (x) { return x.id; });
        var selectedMfrBodyCodeIds = this.mfrBodyCodes.map(function (x) { return x.id; });
        this.vehicleToMfrBodyCodeService.getVehicleToMfrBodyCodesByVehicleIdsAndMfrBodyCodeIds(selectedVehicleIds, selectedMfrBodyCodeIds)
            .subscribe(function (m) {
            _this.proposedVehicleToMfrBodyCodes = [];
            _this.existingVehicleToMfrBodyCodes = m;
            if (_this.existingVehicleToMfrBodyCodes == null || _this.existingVehicleToMfrBodyCodes.length == 0) {
                _this.proposedVehicleToMfrBodyCodes = allProposedVehicleToMfrBodyCodes;
            }
            else {
                var existingVehicleIds_1 = _this.existingVehicleToMfrBodyCodes.map(function (x) { return x.vehicle.id; });
                var existingMfrBodyCodeIds_1 = _this.existingVehicleToMfrBodyCodes.map(function (x) { return x.mfrBodyCode.id; });
                _this.proposedVehicleToMfrBodyCodes = allProposedVehicleToMfrBodyCodes.filter(function (item) { return existingVehicleIds_1.indexOf(item.vehicle.id) < 0 || existingMfrBodyCodeIds_1.indexOf(item.mfrBodyCode.id) < 0; });
            }
            if (_this.proposedVehicleToMfrBodyCodes != null) {
                _this.proposedVehicleToMfrBodyCodesSelectionCount = _this.proposedVehicleToMfrBodyCodes.filter(function (item) { return item.isSelected; }).length;
            }
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToMfrBodyCodeAddComponent.prototype.onSelectAllProposedMfrBodyCodeAssociation = function (event) {
        this.selectAllChecked = event;
        if (this.proposedVehicleToMfrBodyCodes == null) {
            return;
        }
        this.proposedVehicleToMfrBodyCodes.forEach(function (item) { return item.isSelected = event.target.checked; });
        this.proposedVehicleToMfrBodyCodesSelectionCount = this.proposedVehicleToMfrBodyCodes.filter(function (item) { return item.isSelected; }).length;
    };
    VehicleToMfrBodyCodeAddComponent.prototype.refreshProposedVehicleToMfrBodyCodesSelectionCount = function (event, vehicleTomfrBodyCode) {
        if (event.target.checked) {
            this.proposedVehicleToMfrBodyCodesSelectionCount++;
            var excludedVehicle = this.proposedVehicleToMfrBodyCodes.filter(function (item) { return item.isSelected; });
            if (excludedVehicle.length == this.proposedVehicleToMfrBodyCodes.length - 1) {
                this.selectAllChecked = true;
            }
        }
        else {
            this.proposedVehicleToMfrBodyCodesSelectionCount--;
            this.selectAllChecked = false;
        }
    };
    VehicleToMfrBodyCodeAddComponent.prototype.onSubmitAssociations = function () {
        if (!this.proposedVehicleToMfrBodyCodes) {
            return;
        }
        var length = this.proposedVehicleToMfrBodyCodes.length;
        if (this.proposedVehicleToMfrBodyCodes.filter(function (item) { return item.isSelected; }).length == 0) {
            this.toastr.warning("No MFR Body Code associations selected", constants_warehouse_1.ConstantsWarehouse.errorTitle);
            return;
        }
        this.submitAssociations(length);
    };
    VehicleToMfrBodyCodeAddComponent.prototype.getNextVehicleToMfrBodyCode = function (index) {
        if (!this.proposedVehicleToMfrBodyCodes || this.proposedVehicleToMfrBodyCodes.length == 0) {
            return null;
        }
        var nextConfig = this.proposedVehicleToMfrBodyCodes[index];
        return nextConfig;
    };
    VehicleToMfrBodyCodeAddComponent.prototype.submitAssociations = function (length) {
        var _this = this;
        this.showLoadingGif = true;
        length = length - 1;
        if (length >= 0) {
            var proposedVehicleToMfrBodyCode_1 = this.getNextVehicleToMfrBodyCode(length);
            proposedVehicleToMfrBodyCode_1.comment = this.commenttoadd;
            var vehicleToMfrBodyCodeIdentity_1 = "(Vehicle ID: " + proposedVehicleToMfrBodyCode_1.vehicle.id + ", Mfr Body Code ID: " + proposedVehicleToMfrBodyCode_1.mfrBodyCode.id + ")";
            this.acFiles = this.acFileUploader.getAcFiles();
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    proposedVehicleToMfrBodyCode_1.attachments = _this.sharedService.clone(uploadedFiles);
                }
                if (proposedVehicleToMfrBodyCode_1.attachments) {
                    proposedVehicleToMfrBodyCode_1.attachments = proposedVehicleToMfrBodyCode_1.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.vehicleToMfrBodyCodeService.addVehicleToMfrBodyCode(proposedVehicleToMfrBodyCode_1).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle to MFR Body Code", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToMfrBodyCodeIdentity_1);
                        successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + vehicleToMfrBodyCodeIdentity_1 + "\" Vehicle to MFR Body Code change requestid  \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.acFileUploader.reset();
                        _this.acFileUploader.setAcFiles(_this.acFiles);
                        _this.submitAssociations(length);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle to Mfr Body Code", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToMfrBodyCodeIdentity_1);
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                        _this.acFileUploader.reset();
                        _this.acFileUploader.setAcFiles(_this.acFiles);
                        _this.submitAssociations(length);
                    }
                }, function (error) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle to Mfr Body Code", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToMfrBodyCodeIdentity_1);
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
    VehicleToMfrBodyCodeAddComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToMfrBodyCodeAddComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild('mfrBodyCodeAssociationsPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToMfrBodyCodeAddComponent.prototype, "mfrBodyCodeAssociationsPopup", void 0);
    __decorate([
        core_1.ViewChild('associationsPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToMfrBodyCodeAddComponent.prototype, "associationsPopup", void 0);
    VehicleToMfrBodyCodeAddComponent = __decorate([
        core_1.Component({
            selector: "vehicleToMfrBodyCode-add-component",
            templateUrl: "app/templates/vehicleToMfrBodyCode/vehicleToMfrBodyCode-add.component.html",
            providers: [make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, submodel_service_1.SubModelService, region_service_1.RegionService, httpHelper_1.HttpHelper]
        }), 
        __metadata('design:paramtypes', [make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, submodel_service_1.SubModelService, region_service_1.RegionService, vehicleToMfrBodyCode_service_1.VehicleToMfrBodyCodeService, vehicle_service_1.VehicleService, mfrBodyCode_service_1.MfrBodyCodeService, router_1.Router, shared_service_1.SharedService, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleToMfrBodyCodeAddComponent);
    return VehicleToMfrBodyCodeAddComponent;
}());
exports.VehicleToMfrBodyCodeAddComponent = VehicleToMfrBodyCodeAddComponent;
