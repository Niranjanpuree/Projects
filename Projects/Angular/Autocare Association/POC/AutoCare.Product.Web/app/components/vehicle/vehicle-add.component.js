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
var basevehicle_service_1 = require("../basevehicle/basevehicle.service");
var httpHelper_1 = require("../httpHelper");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var router_1 = require("@angular/router");
var region_service_1 = require("../region/region.service");
var submodel_service_1 = require("../submodel/submodel.service");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var vehicle_service_1 = require("./vehicle.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var shared_service_1 = require('../shared/shared.service');
var VehicleAddComponent = (function () {
    function VehicleAddComponent(baseVehicleService, regionService, subModelService, toastr, vehicleService, route, _router, sharedService) {
        this.baseVehicleService = baseVehicleService;
        this.regionService = regionService;
        this.subModelService = subModelService;
        this.toastr = toastr;
        this.vehicleService = vehicleService;
        this.route = route;
        this._router = _router;
        this.sharedService = sharedService;
        this.baseVehicle = {};
        this.isBaseVehicleLoading = true;
        this.comment = "";
        this.subModels = [];
        this.regions = [];
        this.proposedVehicles = [];
        this.proposedVehicle = { id: 0, subModelId: -1, regionId: -1 };
        this.changeRequestVehicles = [];
        this.existingVehicles = [];
        this.isExistingVehiclesLoading = true;
        this.attachments = [];
        this.vehicle = { id: 0, subModelId: -1, regionId: -1, attachments: [] };
        this.selectedSubModelId = -1;
        this.selectedRegionId = -1;
        this.showLoadingGif = false;
    }
    VehicleAddComponent.prototype.ngOnInit = function () {
        this.baseVehicleId = this.route.snapshot.params["basevid"];
        this.searchBaseVehicle();
    };
    VehicleAddComponent.prototype.addToProposedVehicles = function () {
        var _this = this;
        if (this.selectedSubModelId == -1 || this.selectedRegionId == -1) {
            this.toastr.warning("Please select submodel and region from dropdowns to add a vehicle", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (!this.exists()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                //let proposedVehicle = {
                //    id: 0,
                //    baseVehicleId: this.baseVehicleId,
                //    subModelId: this.selectedSubModelId,
                //    subModelName: this.subModels.filter(s => s.id == this.selectedSubModelId)[0].name,
                //    regionId: this.selectedRegionId,
                //    regionName: this.regions.filter(r => r.id == this.selectedRegionId)[0].name,
                //    comment: this.proposedVehicle.comment,
                //    attachments: uploadedFiles
                //};
                _this.proposedVehicle.id = 0;
                _this.proposedVehicle.baseVehicleId = _this.baseVehicleId;
                _this.proposedVehicle.subModelId = _this.selectedSubModelId;
                _this.proposedVehicle.subModelName = _this.subModels.filter(function (s) { return s.id == _this.selectedSubModelId; })[0].name;
                _this.proposedVehicle.regionId = _this.selectedRegionId;
                _this.proposedVehicle.regionName = _this.regions.filter(function (r) { return r.id == _this.selectedRegionId; })[0].name;
                _this.proposedVehicle.comment = _this.comment;
                _this.proposedVehicle.attachments = uploadedFiles;
                _this.proposedVehicles.push(_this.proposedVehicle);
                _this.proposedVehicle = { id: 0, baseVehicleId: 0, subModelId: -1, regionId: -1, comment: '' };
                _this.resetUIControls();
                _this.acFileUploader.reset();
                _this.showLoadingGif = false;
            }, function (error) {
                _this.acFileUploader.reset();
                _this.showLoadingGif = false;
            });
        }
        else {
            this.toastr.warning("Selected vehicle already exists, please add a different vehicle", constants_warehouse_1.ConstantsWarehouse.validationTitle);
        }
        // clean variables
    };
    VehicleAddComponent.prototype.exists = function () {
        var _this = this;
        var matchingVehicles = this.proposedVehicles.filter(function (item) { return item.subModelId == _this.selectedSubModelId
            && item.regionId == _this.selectedRegionId; });
        return (matchingVehicles && matchingVehicles.length > 0);
    };
    VehicleAddComponent.prototype.createVehicleChangeRequests = function () {
        var _this = this;
        this.proposedVehicles.forEach(function (proposedVehicle) {
            var vehicleIdentity = _this.subModels.filter(function (item) { return item.id == proposedVehicle.subModelId; })[0].name + ','
                + _this.regions.filter(function (item) { return item.id == proposedVehicle.regionId; })[0].name;
            _this.showLoadingGif = true;
            _this.vehicleService.createVehicleChangeRequests(proposedVehicle).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleIdentity);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " Vehicle \"" + vehicleIdentity + "\"  change request Id  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.proposedVehicles = [];
                    _this._router.navigateByUrl('vehicle/search');
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleIdentity);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                _this.showLoadingGif = false;
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, vehicleIdentity);
                _this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            });
        });
    };
    VehicleAddComponent.prototype.getRegionsSubModels = function () {
        var _this = this;
        this.regionService.getRegion().subscribe(function (regions) {
            _this.regions = [];
            _this.regions = regions;
            _this.subModelService.getAllSubModels().subscribe(function (submodels) {
                _this.subModels = [];
                _this.subModels = submodels;
                _this.getPendingChangeRequests();
            });
        });
    };
    VehicleAddComponent.prototype.searchBaseVehicle = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.getRegionsSubModels();
        this.baseVehicleService.getBaseVehicle(this.baseVehicleId).subscribe(function (baseVehicle) {
            _this.baseVehicle = {};
            _this.baseVehicle = baseVehicle;
            _this.existingVehicles = [];
            _this.vehicleService.getVehiclesByBaseVehicleId(_this.baseVehicleId).subscribe(function (existingVehicles) {
                _this.existingVehicles = existingVehicles;
                _this.isBaseVehicleLoading = false;
                _this.isExistingVehiclesLoading = false;
                _this.showLoadingGif = false;
            });
        });
    };
    VehicleAddComponent.prototype.getPendingChangeRequests = function () {
        var _this = this;
        this.vehicleService.getPendingChangeRequest(this.baseVehicleId).subscribe(function (changeRequestVehicles) {
            _this.changeRequestVehicles = [];
            var _loop_1 = function(vehicle) {
                vehicle.subModelName = _this.subModels.filter(function (s) { return s.id == vehicle.subModelId; })[0].name;
                vehicle.regionName = _this.regions.filter(function (r) { return r.id == vehicle.regionId; })[0].name;
            };
            for (var _i = 0, changeRequestVehicles_1 = changeRequestVehicles; _i < changeRequestVehicles_1.length; _i++) {
                var vehicle = changeRequestVehicles_1[_i];
                _loop_1(vehicle);
            }
            _this.changeRequestVehicles = changeRequestVehicles;
        });
    };
    VehicleAddComponent.prototype.resetUIControls = function () {
        this.selectedRegionId = -1;
        this.selectedSubModelId = -1;
        this.comment = "";
    };
    VehicleAddComponent.prototype.openCommentPopupModal = function (proposedVehicle) {
        this.vehicle.subModelId = proposedVehicle.subModelId;
        this.vehicle.regionId = proposedVehicle.regionId;
        this.vehicle.comment = proposedVehicle.comment;
        this.commentPopupModel.open("md");
    };
    VehicleAddComponent.prototype.onCommentConfirm = function () {
        var _this = this;
        this.proposedVehicles.filter(function (item) { return item.subModelId == _this.vehicle.subModelId
            && item.regionId == _this.vehicle.regionId; })[0].comment = this.vehicle.comment;
        this.commentPopupModel.close();
    };
    VehicleAddComponent.prototype.openAttachmentPopupModal = function (proposedVehicle) {
        this.vehicle = {
            id: 0,
            subModelId: -1,
            subModelName: '',
            regionId: -1,
            regionName: '',
            attachments: []
        };
        this.vehicle.subModelId = proposedVehicle.subModelId;
        this.vehicle.regionId = proposedVehicle.regionId;
        this.vehicle.attachments = proposedVehicle.attachments;
        this.attachments = this.sharedService.clone(proposedVehicle.attachments);
        this.attachmentsPopupModel.open("md");
        if (this.attachmentsPopupAcFileUploader) {
            this.attachmentsPopupAcFileUploader.reset(false);
            this.attachmentsPopupAcFileUploader.existingFiles = proposedVehicle.attachments;
            this.attachmentsPopupAcFileUploader.setAcFiles();
        }
    };
    VehicleAddComponent.prototype.onAttachmentConfirm = function () {
        var _this = this;
        this.showLoadingGif = true;
        var objectIdentity = this.proposedVehicles.filter(function (item) { return item.subModelId == _this.vehicle.subModelId
            && item.regionId == _this.vehicle.regionId; })[0];
        this.attachmentsPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles) {
                objectIdentity.attachments = uploadedFiles;
            }
            if (objectIdentity.attachments && _this.attachmentsPopupAcFileUploader.getFilesMarkedToDelete().length > 0) {
                objectIdentity.attachments = objectIdentity.attachments.concat(_this.attachmentsPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.showLoadingGif = false;
            _this.attachmentsPopupModel.close();
        });
    };
    VehicleAddComponent.prototype.onAttachmentCancel = function () {
        var _this = this;
        var objectIdentity = this.proposedVehicles.filter(function (item) { return item.subModelId == _this.vehicle.subModelId
            && item.regionId == _this.vehicle.regionId; })[0];
        objectIdentity.attachments = this.sharedService.clone(this.attachments);
        this.attachmentsPopupAcFileUploader.setAcFiles();
        this.attachmentsPopupModel.dismiss();
    };
    VehicleAddComponent.prototype.onViewExistingVehicleForSelectedBaseVehicleCR = function (existingVehicleVm) {
        var changeRequestLink = "/change/review/vehicle/" + existingVehicleVm.changeRequestId;
        this._router.navigateByUrl(changeRequestLink);
    };
    VehicleAddComponent.prototype.onViewPendingVehicleCR = function (pendingVehicleVM) {
        var changeRequestLink = "/change/review/vehicle/" + pendingVehicleVM.changeRequestId;
        this._router.navigateByUrl(changeRequestLink);
    };
    VehicleAddComponent.prototype.onRemoveVehicle = function (vehicle) {
        if (confirm("Remove from selection?")) {
            var index = this.proposedVehicles.indexOf(vehicle);
            if (index > -1) {
                this.proposedVehicles.splice(index, 1);
            }
        }
    };
    VehicleAddComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.attachmentsPopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleAddComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("attachmentsPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleAddComponent.prototype, "attachmentsPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild('commentPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleAddComponent.prototype, "commentPopupModel", void 0);
    __decorate([
        core_1.ViewChild('attachmentsPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleAddComponent.prototype, "attachmentsPopupModel", void 0);
    VehicleAddComponent = __decorate([
        core_1.Component({
            selector: 'vehicle-add-component',
            templateUrl: 'app/templates/vehicle/vehicle-add.component.html',
            providers: [basevehicle_service_1.BaseVehicleService, httpHelper_1.HttpHelper, region_service_1.RegionService, submodel_service_1.SubModelService, vehicle_service_1.VehicleService, shared_service_1.SharedService]
        }), 
        __metadata('design:paramtypes', [basevehicle_service_1.BaseVehicleService, region_service_1.RegionService, submodel_service_1.SubModelService, ng2_toastr_1.ToastsManager, vehicle_service_1.VehicleService, router_1.ActivatedRoute, router_1.Router, shared_service_1.SharedService])
    ], VehicleAddComponent);
    return VehicleAddComponent;
}());
exports.VehicleAddComponent = VehicleAddComponent;
