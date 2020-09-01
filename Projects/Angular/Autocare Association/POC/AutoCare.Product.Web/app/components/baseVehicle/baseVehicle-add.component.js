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
var core_1 = require('@angular/core');
var router_1 = require('@angular/router');
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var baseVehicle_service_1 = require('./baseVehicle.service');
var make_service_1 = require('../make/make.service');
var model_service_1 = require('../model/model.service');
var year_service_1 = require('../year/year.service');
var httpHelper_1 = require('../httpHelper');
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var shared_service_1 = require('../shared/shared.service');
var BaseVehicleAddComponent = (function () {
    function BaseVehicleAddComponent(makeService, modelService, yearService, baseVehicleService, router, toastr, sharedService) {
        this.makeService = makeService;
        this.modelService = modelService;
        this.yearService = yearService;
        this.baseVehicleService = baseVehicleService;
        this.router = router;
        this.toastr = toastr;
        this.sharedService = sharedService;
        this.baseVehicle = { id: 0, makeId: -1, makeName: '', modelId: -1, modelName: '', yearId: -1, vehicles: null };
        this.proposedBaseVehicles = [];
        this.attachments = [];
        this.comment = '';
        this.proposedBaseVehicle = { id: 0, makeId: -1, makeName: '', modelId: -1, modelName: '', yearId: -1, vehicles: null, attachments: null };
        this.showLoadingGif = false;
    }
    BaseVehicleAddComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.makeService.getAllMakes().subscribe(function (mks) {
            _this.makes = mks;
            _this.modelService.getAllModels().subscribe(function (mdls) {
                _this.models = mdls;
                _this.baseVehicleService.getPendingChangeRequests().subscribe(function (crs) {
                    crs.forEach(function (cr) {
                        cr.makeName = mks.filter(function (item) { return item.id == cr.makeId; })[0].name;
                        cr.modelName = mdls.filter(function (item) { return item.id == cr.modelId; })[0].name;
                    });
                    _this.pendingBaseVehicleChangeRequests = crs;
                    _this.showLoadingGif = false;
                }, function (error) {
                    _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                });
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            });
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
        this.yearService.getYears().subscribe(function (m) { return _this.years = m; }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
        });
    };
    BaseVehicleAddComponent.prototype.onAddToProposedChanges = function () {
        var _this = this;
        if (this.baseVehicle.makeId == -1) {
            this.toastr.warning('Please select Make.', constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.baseVehicle.modelId == -1) {
            this.toastr.warning('Please select Model.', constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.baseVehicle.yearId == -1) {
            this.toastr.warning('Please select Year.', constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        var filteredBaseVehicles = this.proposedBaseVehicles.filter(function (item) { return item.makeId == _this.baseVehicle.makeId
            && item.modelId == _this.baseVehicle.modelId
            && item.yearId == _this.baseVehicle.yearId; });
        if (filteredBaseVehicles && filteredBaseVehicles.length > 0) {
            this.toastr.warning('Selected Base Vehicle already added', constants_warehouse_1.ConstantsWarehouse.validationTitle);
        }
        else {
            this.showLoadingGif = true;
            // validate change information       
            // fill brake config information and push to proposed brake configs
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                _this.baseVehicle.makeName = _this.makes.filter(function (item) { return item.id == _this.baseVehicle.makeId; })[0].name;
                _this.baseVehicle.modelName = _this.models.filter(function (item) { return item.id == _this.baseVehicle.modelId; })[0].name;
                _this.baseVehicle.attachments = uploadedFiles;
                _this.proposedBaseVehicles.push(_this.baseVehicle);
                _this.baseVehicle = { id: 0, makeId: -1, makeName: '', modelId: -1, modelName: '', yearId: -1, vehicles: null };
                _this.acFileUploader.reset(false);
                _this.showLoadingGif = false;
            }, function (error) {
                _this.acFileUploader.reset();
                _this.showLoadingGif = false;
            });
        }
    };
    BaseVehicleAddComponent.prototype.onSubmitChangeRequests = function () {
        var _this = this;
        this.proposedBaseVehicles.forEach(function (proposedBaseVehicle) {
            var baseVehicleIdentity = _this.makes.filter(function (item) { return item.id == proposedBaseVehicle.makeId; })[0].name + ', '
                + _this.models.filter(function (item) { return item.id == proposedBaseVehicle.modelId; })[0].name;
            _this.showLoadingGif = true;
            _this.baseVehicleService.addBaseVehicle(proposedBaseVehicle).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Base Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, baseVehicleIdentity);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + baseVehicleIdentity + "\" Base Vehicle requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.router.navigateByUrl('vehicle/search');
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Base Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, baseVehicleIdentity);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                _this.showLoadingGif = false;
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Base Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, baseVehicleIdentity);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.acFileUploader.reset();
                _this.showLoadingGif = false;
            });
        });
    };
    BaseVehicleAddComponent.prototype.openCommentPopupModal = function (proposedBaseVehicle) {
        this.proposedBaseVehicle.makeId = proposedBaseVehicle.makeId;
        this.proposedBaseVehicle.modelId = proposedBaseVehicle.modelId;
        this.proposedBaseVehicle.yearId = proposedBaseVehicle.yearId;
        this.proposedBaseVehicle.comment = proposedBaseVehicle.comment;
        this.commentPopupModel.open("md");
    };
    BaseVehicleAddComponent.prototype.onCommentConfirm = function () {
        var _this = this;
        this.proposedBaseVehicles.filter(function (item) { return item.makeId == _this.proposedBaseVehicle.makeId
            && item.modelId == _this.proposedBaseVehicle.modelId
            && item.yearId == _this.proposedBaseVehicle.yearId; })[0].comment = this.proposedBaseVehicle.comment;
        this.commentPopupModel.close();
    };
    BaseVehicleAddComponent.prototype.openAttachmentPopupModal = function (proposedBaseVehicle) {
        this.attachmentsPopupModel.open("lg");
        this.proposedBaseVehicle = { id: 0, makeId: -1, makeName: '', modelId: -1, modelName: '', yearId: -1, vehicles: null, attachments: [] };
        this.proposedBaseVehicle.makeId = proposedBaseVehicle.makeId;
        this.proposedBaseVehicle.modelId = proposedBaseVehicle.modelId;
        this.proposedBaseVehicle.yearId = proposedBaseVehicle.yearId;
        this.proposedBaseVehicle.attachments = proposedBaseVehicle.attachments;
        this.attachments = this.sharedService.clone(proposedBaseVehicle.attachments);
        if (this.attachmentsPopupAcFileUploader) {
            this.attachmentsPopupAcFileUploader.reset(false);
            this.attachmentsPopupAcFileUploader.existingFiles = proposedBaseVehicle.attachments;
            this.attachmentsPopupAcFileUploader.setAcFiles();
        }
    };
    BaseVehicleAddComponent.prototype.onAttachmentConfirm = function () {
        var _this = this;
        this.showLoadingGif = true;
        var objectIdentity = this.proposedBaseVehicles.filter(function (item) { return item.makeId == _this.proposedBaseVehicle.makeId
            && item.modelId == _this.proposedBaseVehicle.modelId
            && item.yearId == _this.proposedBaseVehicle.yearId; })[0];
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
    BaseVehicleAddComponent.prototype.onAttachmentCancel = function () {
        var _this = this;
        var objectIdentity = this.proposedBaseVehicles.filter(function (item) { return item.makeId == _this.proposedBaseVehicle.makeId
            && item.modelId == _this.proposedBaseVehicle.modelId
            && item.yearId == _this.proposedBaseVehicle.yearId; })[0];
        objectIdentity.attachments = this.sharedService.clone(this.attachments);
        this.attachmentsPopupAcFileUploader.setAcFiles();
        this.attachmentsPopupModel.dismiss();
    };
    BaseVehicleAddComponent.prototype.onViewPendingNew = function (baseVehicleVm) {
        var changeRequestLink = "/change/review/basevehicle/" + baseVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    BaseVehicleAddComponent.prototype.onRemoveBaseVehicle = function (baseVehicle) {
        if (confirm("Remove from selection?")) {
            var index = this.proposedBaseVehicles.indexOf(baseVehicle);
            if (index > -1) {
                this.proposedBaseVehicles.splice(index, 1);
            }
        }
    };
    BaseVehicleAddComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.attachmentsPopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BaseVehicleAddComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("attachmentsPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BaseVehicleAddComponent.prototype, "attachmentsPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild('commentPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BaseVehicleAddComponent.prototype, "commentPopupModel", void 0);
    __decorate([
        core_1.ViewChild('attachmentsPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BaseVehicleAddComponent.prototype, "attachmentsPopupModel", void 0);
    BaseVehicleAddComponent = __decorate([
        core_1.Component({
            selector: 'baseVehicle-component',
            templateUrl: 'app/templates/baseVehicle/baseVehicle-add.component.html',
            providers: [baseVehicle_service_1.BaseVehicleService, model_service_1.ModelService, make_service_1.MakeService, year_service_1.YearService, httpHelper_1.HttpHelper, shared_service_1.SharedService]
        }), 
        __metadata('design:paramtypes', [make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, router_1.Router, ng2_toastr_1.ToastsManager, shared_service_1.SharedService])
    ], BaseVehicleAddComponent);
    return BaseVehicleAddComponent;
}());
exports.BaseVehicleAddComponent = BaseVehicleAddComponent;
