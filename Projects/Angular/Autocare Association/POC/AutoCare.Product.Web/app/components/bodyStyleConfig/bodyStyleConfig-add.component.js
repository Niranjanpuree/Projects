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
var bodyType_service_1 = require("../bodyType/bodyType.service");
var bodyNumDoors_service_1 = require("../bodyNumDoors/bodyNumDoors.service");
var bodyStyleConfig_service_1 = require("./bodyStyleConfig.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var shared_service_1 = require('../shared/shared.service');
var BodyStyleConfigAddComponent = (function () {
    function BodyStyleConfigAddComponent(bodyNumDoorService, bodyStyleConfigService, bodyTypeService, toastr, router, sharedService) {
        this.bodyNumDoorService = bodyNumDoorService;
        this.bodyStyleConfigService = bodyStyleConfigService;
        this.bodyTypeService = bodyTypeService;
        this.toastr = toastr;
        this.router = router;
        this.sharedService = sharedService;
        this.attachments = [];
        this.bodyStyleConfig = { bodyNumDoorsId: 0, bodyTypeId: 0, comment: '' };
        this.showLoadingGif = false;
        // initialize empty bed config
        this.newBodyStyleConfig = {
            id: 0,
            bodyNumDoorsId: -1,
            numDoors: "",
            bodyTypeId: -1,
            bodyTypeName: "",
            isSelected: false
        };
    }
    BodyStyleConfigAddComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        // Load select options for add.
        this.bodyTypeService.getAllBodyTypes().subscribe(function (bt) {
            _this.bodyTypes = bt;
            _this.bodyNumDoorService.getAllBodyNumDoors().subscribe(function (bl) {
                _this.bodyNumDoors = bl;
                // Load pending bed config change requests
                _this.bodyStyleConfigService.getPendingChangeRequests().subscribe(function (crs) {
                    crs.forEach(function (cr) {
                        cr.numDoors = bl.filter(function (item) { return item.id === Number(cr.bodyNumDoorsId); })[0].numDoors;
                        cr.bodyTypeName = bt.filter(function (item) { return item.id === Number(cr.bodyTypeId); })[0].name;
                    });
                    _this.pendingBodyStyleConfigChangeRequests = crs;
                    _this.showLoadingGif = false;
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); }); // pending bed config change requests
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); }); // bed length
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        }); // bed type
        // assign empty array to proposed change requests
        this.proposedBodyStyleConfigs = Array();
    };
    // validation
    BodyStyleConfigAddComponent.prototype.validateAddBodyStyleConfig = function () {
        var _this = this;
        var isValid = true;
        // check required fields
        if (Number(this.newBodyStyleConfig.bodyNumDoorsId) === -1) {
            this.toastr.warning("Please select Body Number Doors.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.newBodyStyleConfig.bodyTypeId) === -1) {
            this.toastr.warning("Please select Body Type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else {
            var filteredBodyStyleConfigs = this.proposedBodyStyleConfigs.filter(function (item) {
                return Number(item.bodyNumDoorsId) === Number(_this.newBodyStyleConfig.bodyNumDoorsId) &&
                    Number(item.bodyTypeId) === Number(_this.newBodyStyleConfig.bodyTypeId);
            });
            if (filteredBodyStyleConfigs && filteredBodyStyleConfigs.length) {
                this.toastr.warning("Selected Body Style Cofig System already added.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                isValid = false;
            }
        }
        return isValid;
    };
    // event on add to proposed changes
    BodyStyleConfigAddComponent.prototype.onAddToProposedChanges = function () {
        var _this = this;
        // validate change information
        if (this.validateAddBodyStyleConfig()) {
            this.showLoadingGif = true;
            // fill bed config information and push to proposed bed configs
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                _this.newBodyStyleConfig.numDoors = _this.bodyNumDoors.filter(function (item) { return item.id === Number(_this.newBodyStyleConfig.bodyNumDoorsId); })[0].numDoors;
                _this.newBodyStyleConfig.bodyTypeName = _this.bodyTypes.filter(function (item) { return item.id === Number(_this.newBodyStyleConfig.bodyTypeId); })[0].name;
                _this.newBodyStyleConfig.attachments = uploadedFiles;
                _this.proposedBodyStyleConfigs.push(_this.newBodyStyleConfig);
                // clear bed config information
                _this.newBodyStyleConfig = {
                    id: 0,
                    bodyNumDoorsId: -1,
                    numDoors: "",
                    bodyTypeId: -1,
                    bodyTypeName: "",
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
    BodyStyleConfigAddComponent.prototype.onSubmitChangeRequests = function () {
        var _this = this;
        // loop through proposed bed configs
        var _loop_1 = function(bodyStyleConfig) {
            // make current bed config identity
            var bodyStyleConfigIdentity = this_1.bodyNumDoors.filter(function (item) { return item.id === Number(bodyStyleConfig.bodyNumDoorsId); })[0].numDoors + ", "
                + this_1.bodyTypes.filter(function (item) { return item.id === Number(bodyStyleConfig.bodyTypeId); })[0].name;
            this_1.showLoadingGif = true;
            this_1.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    bodyStyleConfig.attachments = uploadedFiles;
                }
                _this.bodyStyleConfigService.addBodyStyleConfig(bodyStyleConfig).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Body System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, bodyStyleConfigIdentity);
                        successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " Body System " + bodyStyleConfigIdentity + " change request ID \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, bodyStyleConfigIdentity);
                        //errorMessage.title = "Your requested change cannot be submitted.";
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, function (error) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, bodyStyleConfigIdentity);
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
        for (var _i = 0, _a = this.proposedBodyStyleConfigs; _i < _a.length; _i++) {
            var bodyStyleConfig = _a[_i];
            _loop_1(bodyStyleConfig);
        }
    };
    // event on view affected vehicles
    BodyStyleConfigAddComponent.prototype.onViewBodySystemCr = function (bodySystemVm) {
        //this.viewChangeRequestModal.open(); // medium/default size popup
        var changeRequestLink = "/change/review/bodystyleconfig/" + bodySystemVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    BodyStyleConfigAddComponent.prototype.openCommentPopupModal = function (proposedBodyStyleConfig) {
        this.bodyStyleConfig = { bodyNumDoorsId: 0, bodyTypeId: 0, comment: '' };
        this.bodyStyleConfig.id = proposedBodyStyleConfig.id;
        this.bodyStyleConfig.bodyNumDoorsId = proposedBodyStyleConfig.bodyNumDoorsId;
        this.bodyStyleConfig.bodyTypeId = proposedBodyStyleConfig.bodyTypeId;
        this.bodyStyleConfig.comment = proposedBodyStyleConfig.comment;
        this.bodyStyleConfig.attachments = proposedBodyStyleConfig.attachments;
        this.commentPopupModel.open("md");
    };
    BodyStyleConfigAddComponent.prototype.onCommentConfirm = function () {
        var _this = this;
        this.proposedBodyStyleConfigs.filter(function (item) { return item.bodyNumDoorsId == _this.bodyStyleConfig.bodyNumDoorsId
            && item.bodyTypeId == _this.bodyStyleConfig.bodyTypeId; })[0].comment = this.bodyStyleConfig.comment;
        this.commentPopupModel.close();
    };
    BodyStyleConfigAddComponent.prototype.openAttachmentPopupModal = function (proposedBodyStyleConfig) {
        this.bodyStyleConfig = { bodyNumDoorsId: 0, bodyTypeId: 0, comment: '', attachments: [] };
        this.bodyStyleConfig.id = proposedBodyStyleConfig.id;
        this.bodyStyleConfig.bodyNumDoorsId = proposedBodyStyleConfig.bodyNumDoorsId;
        this.bodyStyleConfig.bodyTypeId = proposedBodyStyleConfig.bodyTypeId;
        this.bodyStyleConfig.attachments = proposedBodyStyleConfig.attachments;
        this.attachments = this.sharedService.clone(proposedBodyStyleConfig.attachments);
        this.attachmentsPopupModel.open("md");
        if (this.attachmentsPopupAcFileUploader) {
            this.attachmentsPopupAcFileUploader.reset(false);
            this.attachmentsPopupAcFileUploader.existingFiles = proposedBodyStyleConfig.attachments;
            this.attachmentsPopupAcFileUploader.setAcFiles();
        }
    };
    BodyStyleConfigAddComponent.prototype.onAttachmentConfirm = function () {
        var _this = this;
        this.showLoadingGif = true;
        var objectIdentity = this.proposedBodyStyleConfigs.filter(function (item) { return item.bodyNumDoorsId == _this.bodyStyleConfig.bodyNumDoorsId
            && item.bodyTypeId == _this.bodyStyleConfig.bodyTypeId; })[0];
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
    BodyStyleConfigAddComponent.prototype.onAttachmentCancel = function () {
        var _this = this;
        var objectIdentity = this.proposedBodyStyleConfigs.filter(function (item) { return item.bodyNumDoorsId == _this.bodyStyleConfig.bodyNumDoorsId
            && item.bodyTypeId == _this.bodyStyleConfig.bodyTypeId; })[0];
        objectIdentity.attachments = this.sharedService.clone(this.attachments);
        this.attachmentsPopupAcFileUploader.setAcFiles();
        this.attachmentsPopupModel.dismiss();
    };
    BodyStyleConfigAddComponent.prototype.onRemoveBodyStyleConfig = function (bodyStyleConfig) {
        if (confirm("Remove from selection?")) {
            var index = this.proposedBodyStyleConfigs.indexOf(bodyStyleConfig);
            if (index > -1) {
                this.proposedBodyStyleConfigs.splice(index, 1);
            }
        }
    };
    BodyStyleConfigAddComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.attachmentsPopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild("viewChangeRequestModal"), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BodyStyleConfigAddComponent.prototype, "viewChangeRequestModal", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BodyStyleConfigAddComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild('commentPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BodyStyleConfigAddComponent.prototype, "commentPopupModel", void 0);
    __decorate([
        core_1.ViewChild('attachmentsPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BodyStyleConfigAddComponent.prototype, "attachmentsPopupModel", void 0);
    __decorate([
        core_1.ViewChild("attachmentsPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BodyStyleConfigAddComponent.prototype, "attachmentsPopupAcFileUploader", void 0);
    BodyStyleConfigAddComponent = __decorate([
        core_1.Component({
            selector: "bodyStyleConfig-add-component",
            templateUrl: "app/templates/bodyStyleConfig/bodyStyleConfig-add.component.html",
            providers: [shared_service_1.SharedService] //pushkar: remove if new instance not required
        }), 
        __metadata('design:paramtypes', [bodyNumDoors_service_1.BodyNumDoorsService, bodyStyleConfig_service_1.BodyStyleConfigService, bodyType_service_1.BodyTypeService, ng2_toastr_1.ToastsManager, router_1.Router, shared_service_1.SharedService])
    ], BodyStyleConfigAddComponent);
    return BodyStyleConfigAddComponent;
}());
exports.BodyStyleConfigAddComponent = BodyStyleConfigAddComponent;
