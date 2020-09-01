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
var mfrBodyCode_service_1 = require("./mfrBodyCode.service");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var MfrBodyCodeReplaceConfirmComponent = (function () {
    function MfrBodyCodeReplaceConfirmComponent(mfrBodyCodeService, router, route, toastr) {
        this.mfrBodyCodeService = mfrBodyCodeService;
        this.router = router;
        this.route = route;
        this.toastr = toastr;
        this.showLoadingGif = false;
    }
    MfrBodyCodeReplaceConfirmComponent.prototype.ngOnInit = function () {
        // Load existing MfrBodyCode  with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        // Get existing / replace MfrBodyCode records from factory/ service.
        this.existingMfrBodyCode = this.mfrBodyCodeService.existingMfrBodyCode;
        this.replacementMfrBodyCode = this.mfrBodyCodeService.replacementMfrBodyCode;
    };
    // validation
    MfrBodyCodeReplaceConfirmComponent.prototype.validateReplaceConfirmMfrBodyCode = function () {
        var isValid = true;
        // check required fields
        if (!this.existingMfrBodyCode || !this.existingMfrBodyCode.vehicleToMfrBodyCodes || !this.replacementMfrBodyCode) {
            this.toastr.warning("Not implemented.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementMfrBodyCode.id) === -1) {
            this.toastr.warning("Please select Mfr Body Code.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementMfrBodyCode.id) < 1) {
            this.toastr.warning("Please select replacement Mfr Body Code.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingMfrBodyCode.id) === Number(this.replacementMfrBodyCode.id)) {
            this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingMfrBodyCode.vehicleToMfrBodyCodes.filter(function (item) { return item.isSelected; }).length <= 0) {
            this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    MfrBodyCodeReplaceConfirmComponent.prototype.onSubmitChangeRequest = function () {
        var _this = this;
        if (this.validateReplaceConfirmMfrBodyCode()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    _this.existingMfrBodyCode.attachments = uploadedFiles;
                }
                if (_this.existingMfrBodyCode.attachments) {
                    _this.existingMfrBodyCode.attachments = _this.existingMfrBodyCode.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.existingMfrBodyCode.vehicleToMfrBodyCodes = _this.existingMfrBodyCode.vehicleToMfrBodyCodes.filter(function (item) { return item.isSelected; });
                _this.existingMfrBodyCode.vehicleToMfrBodyCodes.forEach(function (v) { v.mfrBodyCodeId = _this.replacementMfrBodyCode.id; });
                var mfrBodyCodeIdentity = "Mfr Body Code id: " + _this.existingMfrBodyCode.id;
                _this.mfrBodyCodeService.replaceMfrBodyCode(_this.existingMfrBodyCode.id, _this.existingMfrBodyCode).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Mfr Body Code", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, mfrBodyCodeIdentity);
                        successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " Mfr Body Code " + mfrBodyCodeIdentity + " change request ID \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Mfr Body Code", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, mfrBodyCodeIdentity);
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, (function (errorresponse) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Mfr Body Code", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, mfrBodyCodeIdentity);
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
    MfrBodyCodeReplaceConfirmComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], MfrBodyCodeReplaceConfirmComponent.prototype, "acFileUploader", void 0);
    MfrBodyCodeReplaceConfirmComponent = __decorate([
        core_1.Component({
            selector: "mfrBodyCode-replace-component",
            templateUrl: "app/templates/mfrBodyCode/mfrBodyCode-replaceConfirm.component.html",
        }), 
        __metadata('design:paramtypes', [mfrBodyCode_service_1.MfrBodyCodeService, router_1.Router, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager])
    ], MfrBodyCodeReplaceConfirmComponent);
    return MfrBodyCodeReplaceConfirmComponent;
}());
exports.MfrBodyCodeReplaceConfirmComponent = MfrBodyCodeReplaceConfirmComponent;
