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
var constants_warehouse_1 = require("../constants-warehouse");
var driveType_service_1 = require("./driveType.service");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var DriveTypeReplaceConfirmComponent = (function () {
    function DriveTypeReplaceConfirmComponent(driveTypeService, router, route, toastr) {
        this.driveTypeService = driveTypeService;
        this.router = router;
        this.route = route;
        this.toastr = toastr;
        this.showLoadingGif = false;
    }
    DriveTypeReplaceConfirmComponent.prototype.ngOnInit = function () {
        // Load existing DriveType  with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        // Get existing / replace DriveType records from factory/ service.
        this.existingDriveType = this.driveTypeService.existingDriveType;
        this.replacementDriveType = this.driveTypeService.replacementDriveType;
    };
    // validation
    DriveTypeReplaceConfirmComponent.prototype.validateReplaceConfirmDriveType = function () {
        var isValid = true;
        // check required fields
        if (!this.existingDriveType || !this.existingDriveType.vehicleToDriveTypes || !this.replacementDriveType) {
            this.toastr.warning("Not implemented.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementDriveType.id) === -1) {
            this.toastr.warning("Please select Drive Type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementDriveType.id) < 1) {
            this.toastr.warning("Please select replacement Drive Type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingDriveType.id) === Number(this.replacementDriveType.id)) {
            this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingDriveType.vehicleToDriveTypes.filter(function (item) { return item.isSelected; }).length <= 0) {
            this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    DriveTypeReplaceConfirmComponent.prototype.onSubmitChangeRequest = function () {
        var _this = this;
        if (this.validateReplaceConfirmDriveType()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    _this.existingDriveType.attachments = uploadedFiles;
                }
                if (_this.existingDriveType.attachments) {
                    _this.existingDriveType.attachments = _this.existingDriveType.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.existingDriveType.vehicleToDriveTypes = _this.existingDriveType.vehicleToDriveTypes.filter(function (item) { return item.isSelected; });
                _this.existingDriveType.vehicleToDriveTypes.forEach(function (v) { v.driveTypeId = _this.replacementDriveType.id; });
                var driveTypeIdentity = "Drive Type id: " + _this.existingDriveType.id;
                _this.driveTypeService.replaceDriveTypeConfig(_this.existingDriveType.id, _this.existingDriveType).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Drive Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, driveTypeIdentity);
                        successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " Drive Type " + driveTypeIdentity + " change request ID \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Drive Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, driveTypeIdentity);
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, (function (errorresponse) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Drive Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, driveTypeIdentity);
                    _this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                    _this.showLoadingGif = false;
                }), function () {
                    _this.acFileUploader.reset();
                    _this.showLoadingGif = false;
                });
            }, function (error) {
                _this.acFileUploader.reset();
                _this.showLoadingGif = false;
            });
        }
    };
    DriveTypeReplaceConfirmComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], DriveTypeReplaceConfirmComponent.prototype, "acFileUploader", void 0);
    DriveTypeReplaceConfirmComponent = __decorate([
        core_1.Component({
            selector: "driveType-replace-component",
            templateUrl: "app/templates/driveType/driveType-replaceConfirm.component.html",
        }), 
        __metadata('design:paramtypes', [driveType_service_1.DriveTypeService, router_1.Router, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager])
    ], DriveTypeReplaceConfirmComponent);
    return DriveTypeReplaceConfirmComponent;
}());
exports.DriveTypeReplaceConfirmComponent = DriveTypeReplaceConfirmComponent;
