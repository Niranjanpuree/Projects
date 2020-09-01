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
var wheelBase_service_1 = require("./wheelBase.service");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var WheelBaseReplaceConfirmComponent = (function () {
    function WheelBaseReplaceConfirmComponent(wheelBaseService, router, route, toastr) {
        this.wheelBaseService = wheelBaseService;
        this.router = router;
        this.route = route;
        this.toastr = toastr;
        this.showLoadingGif = false;
    }
    WheelBaseReplaceConfirmComponent.prototype.ngOnInit = function () {
        var id = Number(this.route.snapshot.params["id"]);
        this.existingWheelBase = this.wheelBaseService.existingWheelBase;
        this.replacementWheelBase = this.wheelBaseService.replacementWheelBase;
    };
    // validation
    WheelBaseReplaceConfirmComponent.prototype.validateReplaceConfirmWheelBase = function () {
        var isValid = true;
        // check required fields
        if (!this.existingWheelBase || !this.existingWheelBase.vehicleToWheelBases || !this.replacementWheelBase) {
            this.toastr.warning("Not implemented.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        if (Number(this.replacementWheelBase.id) < 1) {
            this.toastr.warning("Please select replacement Wheel Base.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingWheelBase.id) === Number(this.replacementWheelBase.id)) {
            this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingWheelBase.vehicleToWheelBases.filter(function (item) { return item.isSelected; }).length <= 0) {
            this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    WheelBaseReplaceConfirmComponent.prototype.onSubmitChangeRequest = function () {
        var _this = this;
        if (this.validateReplaceConfirmWheelBase()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    _this.existingWheelBase.attachments = uploadedFiles;
                }
                if (_this.existingWheelBase.attachments) {
                    _this.existingWheelBase.attachments = _this.existingWheelBase.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.existingWheelBase.vehicleToWheelBases = _this.existingWheelBase.vehicleToWheelBases.filter(function (item) { return item.isSelected; });
                _this.existingWheelBase.vehicleToWheelBases.forEach(function (v) { v.wheelBaseId = _this.replacementWheelBase.id; });
                _this.wheelBaseService.replaceWheelBase(_this.existingWheelBase.id, _this.existingWheelBase).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.existingWheelBase.id);
                        successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " Wheel Base ID " + _this.existingWheelBase.id + " change request ID \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.existingWheelBase.id);
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, (function (errorresponse) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.existingWheelBase.id);
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
    WheelBaseReplaceConfirmComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], WheelBaseReplaceConfirmComponent.prototype, "acFileUploader", void 0);
    WheelBaseReplaceConfirmComponent = __decorate([
        core_1.Component({
            selector: "wheelBase-replace-component",
            templateUrl: "app/templates/wheelBase/wheelBase-replaceConfirm.component.html",
        }), 
        __metadata('design:paramtypes', [wheelBase_service_1.WheelBaseService, router_1.Router, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager])
    ], WheelBaseReplaceConfirmComponent);
    return WheelBaseReplaceConfirmComponent;
}());
exports.WheelBaseReplaceConfirmComponent = WheelBaseReplaceConfirmComponent;
