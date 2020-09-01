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
var vehicleToWheelBase_service_1 = require("../vehicleToWheelBase/vehicleToWheelBase.service");
var vehicle_service_1 = require("../vehicle/vehicle.service");
var WheelBase_service_1 = require("../WheelBase/WheelBase.service");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var VehicleToWheelBaseAddComponent = (function () {
    function VehicleToWheelBaseAddComponent(makeService, modelService, yearService, baseVehicleService, subModelService, regionService, vehicleToWheelBaseService, vehicleService, wheelBaseService, router, sharedService, toastr, navgationService) {
        this.makeService = makeService;
        this.modelService = modelService;
        this.yearService = yearService;
        this.baseVehicleService = baseVehicleService;
        this.subModelService = subModelService;
        this.regionService = regionService;
        this.vehicleToWheelBaseService = vehicleToWheelBaseService;
        this.vehicleService = vehicleService;
        this.wheelBaseService = wheelBaseService;
        this.router = router;
        this.sharedService = sharedService;
        this.toastr = toastr;
        this.navgationService = navgationService;
        this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        this.vehicles = [];
        this.wheelBase = { id: -1, base: "-1", wheelBaseMetric: "-1" };
        this.wheelBases = [];
        this.allWheelBases = [];
        this.wheelBaseMetrics = [];
        this.acFiles = [];
        this.proposedVehicleToWheelBasesSelectionCount = 0;
        this.showLoadingGif = false;
        if (this.sharedService.vehicles) {
            this.vehicles = this.sharedService.vehicles;
        }
        if (this.sharedService.wheelBases) {
            this.wheelBases = this.sharedService.wheelBases;
        }
        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('vehicletowheelbase') > 0)
                this.backNavigationText = "Return to Wheel Base Search";
            else
                this.backNavigationText = "Return to Vehicle Search";
        }
    }
    VehicleToWheelBaseAddComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.years = [];
        this.makes = [];
        this.models = [];
        this.subModels = [];
        this.regions = [];
        this.wheelBase = { id: -1 };
        this.yearService.getYears().subscribe(function (m) {
            _this.years = m;
            _this.wheelBaseService.getAllWheelBase().subscribe(function (m) {
                _this.allWheelBases = m;
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            });
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
        this.selectAllChecked = false;
    };
    VehicleToWheelBaseAddComponent.prototype.onVehicleIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onVehicleIdSearch();
        }
    };
    VehicleToWheelBaseAddComponent.prototype.onVehicleIdSearch = function () {
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
    VehicleToWheelBaseAddComponent.prototype.onSelectYear = function () {
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
    VehicleToWheelBaseAddComponent.prototype.onSelectMake = function () {
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
    VehicleToWheelBaseAddComponent.prototype.onSelectModel = function () {
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
        this.subModelService.getSubModelsByBaseVehicleId(this.vehicle.baseVehicleId).subscribe(function (m) {
            _this.subModels = m;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
        });
    };
    VehicleToWheelBaseAddComponent.prototype.onSelectSubModel = function () {
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
    VehicleToWheelBaseAddComponent.prototype.onSelectRegion = function () {
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
    VehicleToWheelBaseAddComponent.prototype.onRemoveVehicle = function (vehicleId) {
        if (confirm("Remove Vehicle Id " + vehicleId + " from selection?")) {
            this.vehicles = this.vehicles.filter(function (item) { return item.id != vehicleId; });
            this.refreshProposedVehicleToWheelBases();
        }
    };
    VehicleToWheelBaseAddComponent.prototype.onAddVehicleToSelection = function () {
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
            this.refreshProposedVehicleToWheelBases();
            this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        }
    };
    VehicleToWheelBaseAddComponent.prototype.onViewWheelBaseAssociations = function (vehicle) {
        var _this = this;
        this.popupVehicle = vehicle;
        this.wheelBaseAssociationsPopup.open("lg");
        if (!this.popupVehicle.vehicleToWheelBases) {
            this.vehicleToWheelBaseService.getByVehicleId(this.popupVehicle.id).subscribe(function (m) {
                _this.popupVehicle.vehicleToWheelBases = m;
                _this.popupVehicle.vehicleToWheelBaseCount = m.length;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    VehicleToWheelBaseAddComponent.prototype.onWheelBaseIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onWheelBaseIdSearch();
        }
    };
    VehicleToWheelBaseAddComponent.prototype.onWheelBaseIdSearch = function () {
        var _this = this;
        var wheelBaseId = Number(this.wheelBaseIdSearchText);
        if (isNaN(wheelBaseId)) {
            this.toastr.warning("Invalid Wheel Base ID", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.wheelBase.id == wheelBaseId) {
            return;
        }
        this.wheelBase = { id: -1 };
        //this.wheelBases = null;
        //this.wheelBaseMetrics = null;
        this.showLoadingGif = true;
        this.wheelBaseService.getWheelBaseDetail(wheelBaseId).subscribe(function (wb) {
            _this.wheelBase = wb;
            _this.showLoadingGif = false;
        }, function (error) {
            var errorMessage = JSON.parse(String(error)).message;
            _this.toastr.warning(errorMessage, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToWheelBaseAddComponent.prototype.onAddWheelBaseToSelection = function () {
        var _this = this;
        //TODO: do not allow addition if this item has an open CR
        if (this.wheelBase.id == -1) {
            this.toastr.warning("Wheel Base ID not available.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        var filteredWheelBases = this.wheelBases.filter(function (item) { return item.id == _this.wheelBase.id; });
        if (filteredWheelBases && filteredWheelBases.length) {
            this.toastr.warning("Selected Wheel Base ID already added", constants_warehouse_1.ConstantsWarehouse.validationTitle);
        }
        else {
            this.wheelBase.base = this.allWheelBases.filter(function (item) { return item.id == _this.wheelBase.id; })[0].base;
            this.wheelBase.wheelBaseMetric = this.allWheelBases.filter(function (item) { return item.id == _this.wheelBase.id; })[0].wheelBaseMetric;
            this.wheelBase.vehicleToWheelBaseCount = this.allWheelBases.filter(function (item) { return item.id == _this.wheelBase.id; })[0].vehicleToWheelBaseCount;
            this.wheelBases.push(this.wheelBase);
            this.refreshProposedVehicleToWheelBases();
            this.wheelBase = { id: -1 };
        }
        this.selectAllChecked = true;
    };
    VehicleToWheelBaseAddComponent.prototype.getDefaultInputModel = function () {
        return {
            wheelBaseId: 0,
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: []
        };
    };
    VehicleToWheelBaseAddComponent.prototype.onViewAssociations = function (wheelBase) {
        var _this = this;
        this.popupWheelBase = wheelBase;
        this.associationsPopup.open("lg");
        if (!this.popupWheelBase.vehicleToWheelBases) {
            var inputModel = this.getDefaultInputModel();
            inputModel.wheelBaseId = this.popupWheelBase.id;
            this.vehicleToWheelBaseService.getAssociations(inputModel).subscribe(function (m) {
                _this.popupWheelBase.vehicleToWheelBases = m;
                _this.popupWheelBase.vehicleToWheelBaseCount = m.length;
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
    };
    VehicleToWheelBaseAddComponent.prototype.onRemoveWheelBase = function (wheelBaseId) {
        if (confirm("Remove Wheel Base Id " + wheelBaseId + " from selection?")) {
            this.wheelBases = this.wheelBases.filter(function (item) { return item.id != wheelBaseId; });
            this.refreshProposedVehicleToWheelBases();
        }
    };
    VehicleToWheelBaseAddComponent.prototype.refreshProposedVehicleToWheelBases = function () {
        var _this = this;
        if (this.vehicles.length == 0 || this.wheelBases.length == 0) {
            return;
        }
        if (this.showLoadingGif) {
            return;
        }
        this.showLoadingGif = true;
        var allProposedVehicleToWheelBases = [];
        this.vehicles.forEach(function (v) {
            _this.wheelBases.forEach(function (b) {
                allProposedVehicleToWheelBases.push({
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
                        vehicleToWheelBaseCount: null,
                        isSelected: false
                    },
                    wheelBase: {
                        id: b.id,
                        base: b.base,
                        wheelBaseMetric: b.wheelBaseMetric,
                        vehicleToWheelBaseCount: 0,
                    },
                    isSelected: true
                });
            });
        });
        var selectedVehicleIds = this.vehicles.map(function (x) { return x.id; });
        var selectedWheelBaseIds = this.wheelBases.map(function (x) { return x.id; });
        this.vehicleToWheelBaseService.getVehicleToWheelBasesByVehicleIdsAndWheelBaseIds(selectedVehicleIds, selectedWheelBaseIds)
            .subscribe(function (m) {
            _this.proposedVehicleToWheelBases = [];
            _this.existingVehicleToWheelBases = m;
            if (_this.existingVehicleToWheelBases == null || _this.existingVehicleToWheelBases.length == 0) {
                _this.proposedVehicleToWheelBases = allProposedVehicleToWheelBases;
            }
            else {
                var existingVehicleIds_1 = _this.existingVehicleToWheelBases.map(function (x) { return x.vehicle.id; });
                var existingWheelBaseIds_1 = _this.existingVehicleToWheelBases.map(function (x) { return x.wheelBase.id; });
                _this.proposedVehicleToWheelBases = allProposedVehicleToWheelBases.filter(function (item) { return existingVehicleIds_1.indexOf(item.vehicle.id) < 0 || existingWheelBaseIds_1.indexOf(item.wheelBase.id) < 0; });
            }
            if (_this.proposedVehicleToWheelBases != null) {
                _this.proposedVehicleToWheelBasesSelectionCount = _this.proposedVehicleToWheelBases.filter(function (item) { return item.isSelected; }).length;
            }
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleToWheelBaseAddComponent.prototype.onSelectAllProposedWheelBaseAssociations = function (event) {
        this.selectAllChecked = event;
        if (this.proposedVehicleToWheelBases == null) {
            return;
        }
        this.proposedVehicleToWheelBases.forEach(function (item) { return item.isSelected = event.target.checked; });
        this.proposedVehicleToWheelBasesSelectionCount = this.proposedVehicleToWheelBases.filter(function (item) { return item.isSelected; }).length;
    };
    VehicleToWheelBaseAddComponent.prototype.refreshProposedVehicleToWheelBasesSelectionCount = function (event, vehicleToWheelBase) {
        if (event.target.checked) {
            this.proposedVehicleToWheelBasesSelectionCount++;
            var excludedVehicle = this.proposedVehicleToWheelBases.filter(function (item) { return item.isSelected; });
            if (excludedVehicle.length == this.proposedVehicleToWheelBases.length - 1) {
                this.selectAllChecked = true;
            }
        }
        else {
            this.proposedVehicleToWheelBasesSelectionCount--;
            this.selectAllChecked = false;
        }
    };
    VehicleToWheelBaseAddComponent.prototype.onSubmitAssociations = function () {
        if (!this.proposedVehicleToWheelBases) {
            return;
        }
        var length = this.proposedVehicleToWheelBases.length;
        if (this.proposedVehicleToWheelBases.filter(function (item) { return item.isSelected; }).length == 0) {
            this.toastr.warning("No wheel base associations selected", constants_warehouse_1.ConstantsWarehouse.errorTitle);
            return;
        }
        this.submitAssociations(length);
    };
    VehicleToWheelBaseAddComponent.prototype.getNextVehicleToWheelBase = function (index) {
        if (!this.proposedVehicleToWheelBases || this.proposedVehicleToWheelBases.length == 0) {
            return null;
        }
        var nextConfig = this.proposedVehicleToWheelBases[index];
        return nextConfig;
    };
    VehicleToWheelBaseAddComponent.prototype.submitAssociations = function (length) {
        var _this = this;
        this.showLoadingGif = true;
        length = length - 1;
        if (length >= 0) {
            var proposedVehicleToWheelBase_1 = this.getNextVehicleToWheelBase(length);
            proposedVehicleToWheelBase_1.comment = this.commenttoadd;
            var vehicleToWheelBaseIdentity_1 = "(Vehicle ID: " + proposedVehicleToWheelBase_1.vehicle.id + ", Wheel Base ID: " + proposedVehicleToWheelBase_1.wheelBase.id + ")";
            this.acFiles = this.acFileUploader.getAcFiles();
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    proposedVehicleToWheelBase_1.attachments = _this.sharedService.clone(uploadedFiles);
                }
                if (proposedVehicleToWheelBase_1.attachments) {
                    proposedVehicleToWheelBase_1.attachments = proposedVehicleToWheelBase_1.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.vehicleToWheelBaseService.addVehicleToWheelBase(proposedVehicleToWheelBase_1).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle to Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToWheelBaseIdentity_1);
                        successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + vehicleToWheelBaseIdentity_1 + "\" Vehicle to Wheel Base change requestid  \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.acFileUploader.reset();
                        _this.acFileUploader.setAcFiles(_this.acFiles);
                        _this.submitAssociations(length);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle to Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToWheelBaseIdentity_1);
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                        _this.acFileUploader.reset();
                        _this.acFileUploader.setAcFiles(_this.acFiles);
                        _this.submitAssociations(length);
                    }
                }, function (error) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle to Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleToWheelBaseIdentity_1);
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
    VehicleToWheelBaseAddComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToWheelBaseAddComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild('wheelBaseAssociationsPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToWheelBaseAddComponent.prototype, "wheelBaseAssociationsPopup", void 0);
    __decorate([
        core_1.ViewChild('associationsPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToWheelBaseAddComponent.prototype, "associationsPopup", void 0);
    VehicleToWheelBaseAddComponent = __decorate([
        core_1.Component({
            selector: "vehicleToWheelBase-add-component",
            templateUrl: "app/templates/vehicleToWheelBase/vehicleToWheelBase-add.component.html",
            providers: [make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, submodel_service_1.SubModelService, region_service_1.RegionService, vehicleToWheelBase_service_1.VehicleToWheelBaseService, WheelBase_service_1.WheelBaseService]
        }), 
        __metadata('design:paramtypes', [make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, submodel_service_1.SubModelService, region_service_1.RegionService, vehicleToWheelBase_service_1.VehicleToWheelBaseService, vehicle_service_1.VehicleService, WheelBase_service_1.WheelBaseService, router_1.Router, shared_service_1.SharedService, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleToWheelBaseAddComponent);
    return VehicleToWheelBaseAddComponent;
}());
exports.VehicleToWheelBaseAddComponent = VehicleToWheelBaseAddComponent;
