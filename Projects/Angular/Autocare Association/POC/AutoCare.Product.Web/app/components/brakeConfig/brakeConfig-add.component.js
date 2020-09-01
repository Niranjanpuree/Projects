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
var brakeType_service_1 = require("../brakeType/brakeType.service");
var brakeABS_service_1 = require("../brakeABS/brakeABS.service");
var brakeSystem_service_1 = require("../brakeSystem/brakeSystem.service");
var brakeConfig_service_1 = require("./brakeConfig.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var shared_service_1 = require('../shared/shared.service');
var BrakeConfigAddComponent = (function () {
    function BrakeConfigAddComponent(brakeAbsService, brakeConfigService, brakeSystemService, brakeTypeSerivce, toastr, router, sharedService) {
        this.brakeAbsService = brakeAbsService;
        this.brakeConfigService = brakeConfigService;
        this.brakeSystemService = brakeSystemService;
        this.brakeTypeSerivce = brakeTypeSerivce;
        this.toastr = toastr;
        this.router = router;
        this.sharedService = sharedService;
        this.attachments = [];
        this.brakeConfig = { frontBrakeTypeId: 0, rearBrakeTypeId: 0, brakeABSId: 0, brakeSystemId: 0, comment: '' };
        this.showLoadingGif = false;
        // initialize empty brake config
        this.newBrakeConfig = {
            id: 0,
            frontBrakeTypeId: -1,
            frontBrakeTypeName: "",
            rearBrakeTypeId: -1,
            rearBrakeTypeName: "",
            brakeABSId: -1,
            brakeABSName: "",
            brakeSystemId: -1,
            brakeSystemName: "",
            isSelected: false
        };
    }
    BrakeConfigAddComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        // Load select options for add.
        this.brakeTypeSerivce.getAllBrakeTypes().subscribe(function (bt) {
            _this.brakeTypes = bt;
            _this.brakeAbsService.getAllBrakeABSes().subscribe(function (babs) {
                _this.brakeABSes = babs;
                _this.brakeSystemService.getAllBrakeSystems().subscribe(function (bs) {
                    _this.brakeSystems = bs;
                    // Load pending brake config change requests
                    _this.brakeConfigService.getPendingChangeRequests().subscribe(function (crs) {
                        crs.forEach(function (cr) {
                            cr.frontBrakeTypeName = bt.filter(function (item) { return item.id === Number(cr.frontBrakeTypeId); })[0].name;
                            cr.rearBrakeTypeName = bt.filter(function (item) { return item.id === Number(cr.rearBrakeTypeId); })[0].name;
                            cr.brakeABSName = babs.filter(function (item) { return item.id === Number(cr.brakeABSId); })[0].name;
                            cr.brakeSystemName = bs.filter(function (item) { return item.id === Number(cr.brakeSystemId); })[0].name;
                        });
                        _this.pendingBrakeConfigChangeRequests = crs;
                        _this.showLoadingGif = false;
                    }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); }); // pending brake config change requests
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); }); // brake systems
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); }); // brake Abss
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        }); // brake types
        // assign empty array to proposed change requests
        this.proposedBrakeConfigs = Array();
    };
    // validation
    BrakeConfigAddComponent.prototype.validateAddBrakeConfig = function () {
        var _this = this;
        var isValid = true;
        // check required fields
        if (Number(this.newBrakeConfig.frontBrakeTypeId) === -1) {
            this.toastr.warning("Please select Front brake type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.newBrakeConfig.rearBrakeTypeId) === -1) {
            this.toastr.warning("Please select Rear brake type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.newBrakeConfig.brakeABSId) === -1) {
            this.toastr.warning("Please select Brake ABS.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.newBrakeConfig.brakeSystemId) === -1) {
            this.toastr.warning("Please select Brake system.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else {
            var filteredBrakeConfigs = this.proposedBrakeConfigs.filter(function (item) {
                return Number(item.frontBrakeTypeId) === Number(_this.newBrakeConfig.frontBrakeTypeId) &&
                    Number(item.rearBrakeTypeId) === Number(_this.newBrakeConfig.rearBrakeTypeId) &&
                    Number(item.brakeABSId) === Number(_this.newBrakeConfig.brakeABSId) &&
                    Number(item.brakeSystemId) === Number(_this.newBrakeConfig.brakeSystemId);
            });
            if (filteredBrakeConfigs && filteredBrakeConfigs.length) {
                this.toastr.warning("Selected Brake Cofig System already added.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                isValid = false;
            }
        }
        return isValid;
    };
    // event on add to proposed changes
    BrakeConfigAddComponent.prototype.onAddToProposedChanges = function () {
        var _this = this;
        // validate change information
        if (this.validateAddBrakeConfig()) {
            this.showLoadingGif = true;
            // fill brake config information and push to proposed brake configs
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                _this.newBrakeConfig.frontBrakeTypeName = _this.brakeTypes.filter(function (item) { return item.id === Number(_this.newBrakeConfig.frontBrakeTypeId); })[0].name;
                _this.newBrakeConfig.rearBrakeTypeName = _this.brakeTypes.filter(function (item) { return item.id === Number(_this.newBrakeConfig.rearBrakeTypeId); })[0].name;
                _this.newBrakeConfig.brakeABSName = _this.brakeABSes.filter(function (item) { return item.id === Number(_this.newBrakeConfig.brakeABSId); })[0].name;
                _this.newBrakeConfig.brakeSystemName = _this.brakeSystems.filter(function (item) { return item.id === Number(_this.newBrakeConfig.brakeSystemId); })[0].name;
                _this.newBrakeConfig.attachments = uploadedFiles;
                _this.proposedBrakeConfigs.push(_this.newBrakeConfig);
                // clear brake config information
                _this.newBrakeConfig = {
                    id: 0,
                    frontBrakeTypeId: -1,
                    frontBrakeTypeName: "",
                    rearBrakeTypeId: -1,
                    rearBrakeTypeName: "",
                    brakeABSId: -1,
                    brakeABSName: "",
                    brakeSystemId: -1,
                    brakeSystemName: "",
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
    // event on submit brake config
    BrakeConfigAddComponent.prototype.onSubmitChangeRequests = function () {
        var _this = this;
        // loop through proposed brake configs
        var _loop_1 = function(brakeConfig) {
            // make current brake config identity
            var brakeConfigIdentity = this_1.brakeTypes.filter(function (item) { return item.id === Number(brakeConfig.frontBrakeTypeId); })[0].name + ", "
                + this_1.brakeTypes.filter(function (item) { return item.id === Number(brakeConfig.rearBrakeTypeId); })[0].name + ", "
                + this_1.brakeABSes.filter(function (item) { return item.id === Number(brakeConfig.brakeABSId); })[0].name + ", "
                + this_1.brakeSystems.filter(function (item) { return item.id === Number(brakeConfig.brakeSystemId); })[0].name;
            this_1.showLoadingGif = true;
            this_1.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    brakeConfig.attachments = uploadedFiles;
                }
                _this.brakeConfigService.addBrakeConfig(brakeConfig).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, brakeConfigIdentity);
                        successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " Brake System " + brakeConfigIdentity + " change request ID \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, brakeConfigIdentity);
                        //errorMessage.title = "Your requested change cannot be submitted.";
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, function (error) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, brakeConfigIdentity);
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
        for (var _i = 0, _a = this.proposedBrakeConfigs; _i < _a.length; _i++) {
            var brakeConfig = _a[_i];
            _loop_1(brakeConfig);
        }
    };
    // event on view affected vehicles
    BrakeConfigAddComponent.prototype.onViewBrakeSystemCr = function (brakeSystemVm) {
        //this.viewChangeRequestModal.open(); // medium/default size popup
        var changeRequestLink = "/change/review/brakeconfig/" + brakeSystemVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    BrakeConfigAddComponent.prototype.openCommentPopupModal = function (proposedBrakeConfig) {
        this.brakeConfig = { frontBrakeTypeId: 0, rearBrakeTypeId: 0, brakeABSId: 0, brakeSystemId: 0, comment: '' };
        this.brakeConfig.id = proposedBrakeConfig.id;
        this.brakeConfig.frontBrakeTypeId = proposedBrakeConfig.frontBrakeTypeId;
        this.brakeConfig.rearBrakeTypeId = proposedBrakeConfig.rearBrakeTypeId;
        this.brakeConfig.brakeABSId = proposedBrakeConfig.brakeABSId;
        this.brakeConfig.brakeSystemId = proposedBrakeConfig.brakeSystemId;
        this.brakeConfig.comment = proposedBrakeConfig.comment;
        this.brakeConfig.attachments = proposedBrakeConfig.attachments;
        this.commentPopupModel.open("md");
    };
    BrakeConfigAddComponent.prototype.onCommentConfirm = function () {
        var _this = this;
        this.proposedBrakeConfigs.filter(function (item) { return item.frontBrakeTypeId == _this.brakeConfig.frontBrakeTypeId
            && item.rearBrakeTypeId == _this.brakeConfig.rearBrakeTypeId
            && item.brakeABSId == _this.brakeConfig.brakeABSId
            && item.brakeSystemId == _this.brakeConfig.brakeSystemId; })[0].comment = this.brakeConfig.comment;
        this.commentPopupModel.close();
    };
    BrakeConfigAddComponent.prototype.openAttachmentPopupModal = function (proposedBrakeConfig) {
        this.brakeConfig = { frontBrakeTypeId: 0, rearBrakeTypeId: 0, brakeABSId: 0, brakeSystemId: 0, comment: '', attachments: [] };
        this.brakeConfig.id = proposedBrakeConfig.id;
        this.brakeConfig.frontBrakeTypeId = proposedBrakeConfig.frontBrakeTypeId;
        this.brakeConfig.rearBrakeTypeId = proposedBrakeConfig.rearBrakeTypeId;
        this.brakeConfig.brakeABSId = proposedBrakeConfig.brakeABSId;
        this.brakeConfig.brakeSystemId = proposedBrakeConfig.brakeSystemId;
        this.brakeConfig.attachments = proposedBrakeConfig.attachments;
        this.attachments = this.sharedService.clone(proposedBrakeConfig.attachments);
        this.attachmentsPopupModel.open("md");
        if (this.attachmentsPopupAcFileUploader) {
            this.attachmentsPopupAcFileUploader.reset(false);
            this.attachmentsPopupAcFileUploader.existingFiles = proposedBrakeConfig.attachments;
            this.attachmentsPopupAcFileUploader.setAcFiles();
        }
    };
    BrakeConfigAddComponent.prototype.onAttachmentConfirm = function () {
        var _this = this;
        this.showLoadingGif = true;
        var objectIdentity = this.proposedBrakeConfigs.filter(function (item) { return item.frontBrakeTypeId == _this.brakeConfig.frontBrakeTypeId
            && item.rearBrakeTypeId == _this.brakeConfig.rearBrakeTypeId
            && item.brakeABSId == _this.brakeConfig.brakeABSId
            && item.brakeSystemId == _this.brakeConfig.brakeSystemId; })[0];
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
    BrakeConfigAddComponent.prototype.onAttachmentCancel = function () {
        var _this = this;
        var objectIdentity = this.proposedBrakeConfigs.filter(function (item) { return item.frontBrakeTypeId == _this.brakeConfig.frontBrakeTypeId
            && item.rearBrakeTypeId == _this.brakeConfig.rearBrakeTypeId
            && item.brakeABSId == _this.brakeConfig.brakeABSId
            && item.brakeSystemId == _this.brakeConfig.brakeSystemId; })[0];
        objectIdentity.attachments = this.sharedService.clone(this.attachments);
        this.attachmentsPopupAcFileUploader.setAcFiles();
        this.attachmentsPopupModel.dismiss();
    };
    BrakeConfigAddComponent.prototype.onRemoveBrakeConfig = function (brakeConfig) {
        if (confirm("Remove from selection?")) {
            var index = this.proposedBrakeConfigs.indexOf(brakeConfig);
            if (index > -1) {
                this.proposedBrakeConfigs.splice(index, 1);
            }
        }
    };
    BrakeConfigAddComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.attachmentsPopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild("viewChangeRequestModal"), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BrakeConfigAddComponent.prototype, "viewChangeRequestModal", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BrakeConfigAddComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild('commentPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BrakeConfigAddComponent.prototype, "commentPopupModel", void 0);
    __decorate([
        core_1.ViewChild('attachmentsPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BrakeConfigAddComponent.prototype, "attachmentsPopupModel", void 0);
    __decorate([
        core_1.ViewChild("attachmentsPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BrakeConfigAddComponent.prototype, "attachmentsPopupAcFileUploader", void 0);
    BrakeConfigAddComponent = __decorate([
        core_1.Component({
            selector: "brakeConfig-add-component",
            templateUrl: "app/templates/brakeConfig/brakeConfig-add.component.html",
            providers: [shared_service_1.SharedService] //pushkar: remove if existing instance will work
        }), 
        __metadata('design:paramtypes', [brakeABS_service_1.BrakeABSService, brakeConfig_service_1.BrakeConfigService, brakeSystem_service_1.BrakeSystemService, brakeType_service_1.BrakeTypeService, ng2_toastr_1.ToastsManager, router_1.Router, shared_service_1.SharedService])
    ], BrakeConfigAddComponent);
    return BrakeConfigAddComponent;
}());
exports.BrakeConfigAddComponent = BrakeConfigAddComponent;
