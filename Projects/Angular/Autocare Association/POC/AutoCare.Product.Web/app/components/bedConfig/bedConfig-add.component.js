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
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var bedType_service_1 = require("../bedType/bedType.service");
var bedLength_service_1 = require("../bedLength/bedLength.service");
var bedConfig_service_1 = require("./bedConfig.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var shared_service_1 = require('../shared/shared.service');
var BedConfigAddComponent = (function () {
    function BedConfigAddComponent(bedLengthService, bedConfigService, bedTypeService, toastr, router, sharedService) {
        this.bedLengthService = bedLengthService;
        this.bedConfigService = bedConfigService;
        this.bedTypeService = bedTypeService;
        this.toastr = toastr;
        this.router = router;
        this.sharedService = sharedService;
        this.attachments = [];
        this.bedConfig = { bedLengthId: 0, bedTypeId: 0, comment: '' };
        this.showLoadingGif = false;
        // initialize empty bed config
        this.newBedConfig = {
            id: 0,
            bedLengthId: -1,
            length: "",
            bedLengthMetric: "",
            bedTypeId: -1,
            bedTypeName: "",
            isSelected: false
        };
    }
    BedConfigAddComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        // Load select options for add.
        this.bedTypeService.getAllBedTypes().subscribe(function (bt) {
            _this.bedTypes = bt;
            _this.bedLengthService.getAllBedLengths().subscribe(function (bl) {
                _this.bedLengths = bl;
                // Load pending bed config change requests
                _this.bedConfigService.getPendingChangeRequests().subscribe(function (crs) {
                    crs.forEach(function (cr) {
                        cr.length = bl.filter(function (item) { return item.id === Number(cr.bedLengthId); })[0].length;
                        cr.bedLengthMetric = bl.filter(function (item) { return item.id === Number(cr.bedLengthId); })[0].bedLengthMetric;
                        cr.bedTypeName = bt.filter(function (item) { return item.id === Number(cr.bedTypeId); })[0].name;
                    });
                    _this.pendingBedConfigChangeRequests = crs;
                    _this.showLoadingGif = false;
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); }); // pending bed config change requests
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); }); // bed length
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        }); // bed type
        // assign empty array to proposed change requests
        this.proposedBedConfigs = Array();
    };
    // validation
    BedConfigAddComponent.prototype.validateAddBedConfig = function () {
        var _this = this;
        var isValid = true;
        // check required fields
        if (Number(this.newBedConfig.bedLengthId) === -1) {
            this.toastr.warning("Please select Bed Length.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.newBedConfig.bedTypeId) === -1) {
            this.toastr.warning("Please select Bed Type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else {
            var filteredBedConfigs = this.proposedBedConfigs.filter(function (item) {
                return Number(item.bedLengthId) === Number(_this.newBedConfig.bedLengthId) &&
                    Number(item.bedTypeId) === Number(_this.newBedConfig.bedTypeId);
            });
            if (filteredBedConfigs && filteredBedConfigs.length) {
                this.toastr.warning("Selected Bed Cofig System already added.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                isValid = false;
            }
        }
        return isValid;
    };
    // event on add to proposed changes
    BedConfigAddComponent.prototype.onAddToProposedChanges = function () {
        var _this = this;
        // validate change information
        if (this.validateAddBedConfig()) {
            this.showLoadingGif = true;
            // fill bed config information and push to proposed bed configs
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                _this.newBedConfig.length = _this.bedLengths.filter(function (item) { return item.id === Number(_this.newBedConfig.bedLengthId); })[0].length;
                _this.newBedConfig.bedLengthMetric = _this.bedLengths.filter(function (item) { return item.id === Number(_this.newBedConfig.bedLengthId); })[0].bedLengthMetric;
                _this.newBedConfig.bedTypeName = _this.bedTypes.filter(function (item) { return item.id === Number(_this.newBedConfig.bedTypeId); })[0].name;
                _this.newBedConfig.attachments = uploadedFiles;
                _this.proposedBedConfigs.push(_this.newBedConfig);
                // clear bed config information
                _this.newBedConfig = {
                    id: 0,
                    bedLengthId: -1,
                    length: "",
                    bedLengthMetric: "",
                    bedTypeId: -1,
                    bedTypeName: "",
                    isSelected: false
                };
                _this.acFileUploader.reset();
                _this.showLoadingGif = false;
            }, function (error) {
                _this.acFileUploader.reset();
                _this.showLoadingGif = false;
            });
        }
    };
    // event on submit bed config
    BedConfigAddComponent.prototype.onSubmitChangeRequests = function () {
        var _this = this;
        // loop through proposed bed configs
        var _loop_1 = function(bedConfig) {
            // make current bed config identity
            var bedConfigIdentity = this_1.bedLengths.filter(function (item) { return item.id === Number(bedConfig.bedLengthId); })[0].name + ", "
                + this_1.bedTypes.filter(function (item) { return item.id === Number(bedConfig.bedTypeId); })[0].name;
            this_1.showLoadingGif = true;
            this_1.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    bedConfig.attachments = uploadedFiles;
                }
                _this.bedConfigService.addBedConfig(bedConfig).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Bed System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, bedConfigIdentity);
                        successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " Bed System " + bedConfigIdentity + " change request ID \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, bedConfigIdentity);
                        //errorMessage.title = "Your requested change cannot be submitted.";
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, function (error) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, bedConfigIdentity);
                    //errorMessage.title = "Your requested change cannot be submitted.";
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                    _this.showLoadingGif = false;
                }, function () {
                    _this.acFileUploader.reset();
                    _this.showLoadingGif = false;
                });
            }, function (error) {
                _this.acFileUploader.reset();
                _this.showLoadingGif = false;
            });
        };
        var this_1 = this;
        for (var _i = 0, _a = this.proposedBedConfigs; _i < _a.length; _i++) {
            var bedConfig = _a[_i];
            _loop_1(bedConfig);
        }
    };
    // event on view affected vehicles
    BedConfigAddComponent.prototype.onViewBedSystemCr = function (bedSystemVm) {
        //this.viewChangeRequestModal.open(); // medium/default size popup
        var changeRequestLink = "/change/review/bedconfig/" + bedSystemVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    BedConfigAddComponent.prototype.openCommentPopupModal = function (proposedBedConfig) {
        this.bedConfig = { bedLengthId: 0, bedTypeId: 0, comment: '' };
        this.bedConfig.id = proposedBedConfig.id;
        this.bedConfig.bedLengthId = proposedBedConfig.bedLengthId;
        this.bedConfig.bedTypeId = proposedBedConfig.bedTypeId;
        this.bedConfig.comment = proposedBedConfig.comment;
        this.bedConfig.attachments = proposedBedConfig.attachments;
        this.commentPopupModel.open("md");
    };
    BedConfigAddComponent.prototype.onCommentConfirm = function () {
        var _this = this;
        this.proposedBedConfigs.filter(function (item) { return item.bedLengthId == _this.bedConfig.bedLengthId
            && item.bedTypeId == _this.bedConfig.bedTypeId; })[0].comment = this.bedConfig.comment;
        this.commentPopupModel.close();
    };
    BedConfigAddComponent.prototype.openAttachmentPopupModal = function (proposedBedConfig) {
        this.bedConfig = { bedLengthId: 0, bedTypeId: 0, comment: '', attachments: [] };
        this.bedConfig.id = proposedBedConfig.id;
        this.bedConfig.bedLengthId = proposedBedConfig.bedLengthId;
        this.bedConfig.bedTypeId = proposedBedConfig.bedTypeId;
        this.bedConfig.attachments = proposedBedConfig.attachments;
        this.attachments = this.sharedService.clone(proposedBedConfig.attachments);
        this.attachmentsPopupModel.open("md");
        if (this.attachmentsPopupAcFileUploader) {
            this.attachmentsPopupAcFileUploader.reset(false);
            this.attachmentsPopupAcFileUploader.existingFiles = proposedBedConfig.attachments;
            this.attachmentsPopupAcFileUploader.setAcFiles();
        }
    };
    BedConfigAddComponent.prototype.onAttachmentConfirm = function () {
        var _this = this;
        this.showLoadingGif = true;
        var objectIdentity = this.proposedBedConfigs.filter(function (item) { return item.bedLengthId == _this.bedConfig.bedLengthId
            && item.bedTypeId == _this.bedConfig.bedTypeId; })[0];
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
    BedConfigAddComponent.prototype.onAttachmentCancel = function () {
        var _this = this;
        var objectIdentity = this.proposedBedConfigs.filter(function (item) { return item.bedLengthId == _this.bedConfig.bedLengthId
            && item.bedTypeId == _this.bedConfig.bedTypeId; })[0];
        objectIdentity.attachments = this.sharedService.clone(this.attachments);
        this.attachmentsPopupAcFileUploader.setAcFiles();
        this.attachmentsPopupModel.dismiss();
    };
    BedConfigAddComponent.prototype.onRemoveBedConfig = function (bedConfig) {
        if (confirm("Remove from selection?")) {
            var index = this.proposedBedConfigs.indexOf(bedConfig);
            if (index > -1) {
                this.proposedBedConfigs.splice(index, 1);
            }
        }
    };
    BedConfigAddComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.attachmentsPopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild("viewChangeRequestModal"), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BedConfigAddComponent.prototype, "viewChangeRequestModal", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BedConfigAddComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild('commentPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BedConfigAddComponent.prototype, "commentPopupModel", void 0);
    __decorate([
        core_1.ViewChild('attachmentsPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BedConfigAddComponent.prototype, "attachmentsPopupModel", void 0);
    __decorate([
        core_1.ViewChild("attachmentsPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BedConfigAddComponent.prototype, "attachmentsPopupAcFileUploader", void 0);
    BedConfigAddComponent = __decorate([
        core_1.Component({
            selector: "bedConfig-add-component",
            templateUrl: "app/templates/bedConfig/bedConfig-add.component.html",
            providers: [shared_service_1.SharedService]
        }), 
        __metadata('design:paramtypes', [bedLength_service_1.BedLengthService, bedConfig_service_1.BedConfigService, bedType_service_1.BedTypeService, ng2_toastr_1.ToastsManager, router_1.Router, shared_service_1.SharedService])
    ], BedConfigAddComponent);
    return BedConfigAddComponent;
}());
exports.BedConfigAddComponent = BedConfigAddComponent;
