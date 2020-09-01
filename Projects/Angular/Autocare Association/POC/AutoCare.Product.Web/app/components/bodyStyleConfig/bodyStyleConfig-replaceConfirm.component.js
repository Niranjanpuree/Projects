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
var bodyStyleConfig_service_1 = require("./bodyStyleConfig.service");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var BodyStyleConfigReplaceConfirmComponent = (function () {
    function BodyStyleConfigReplaceConfirmComponent(bodyStyleConfigService, router, route, toastr) {
        this.bodyStyleConfigService = bodyStyleConfigService;
        this.router = router;
        this.route = route;
        this.toastr = toastr;
        this.showLoadingGif = false;
    }
    BodyStyleConfigReplaceConfirmComponent.prototype.ngOnInit = function () {
        // Load existing bed config with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        // Get existing / replace bed config records from factory/ service.
        this.existingBodyStyleConfig = this.bodyStyleConfigService.existingBodyStyleConfig;
        this.replacementBodyStyleConfig = this.bodyStyleConfigService.replacementBodyStyleConfig;
    };
    // validation
    BodyStyleConfigReplaceConfirmComponent.prototype.validateReplaceConfirmBodyStyleConfig = function () {
        var isValid = true;
        // check required fields
        if (!this.existingBodyStyleConfig || !this.existingBodyStyleConfig.vehicleToBodyStyleConfigs || !this.replacementBodyStyleConfig) {
            this.toastr.warning("Not implemented.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementBodyStyleConfig.bodyNumDoorsId) === -1) {
            this.toastr.warning("Please select Body Num Doors.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementBodyStyleConfig.bodyTypeId) === -1) {
            this.toastr.warning("Please select Body type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementBodyStyleConfig.id) < 1) {
            this.toastr.warning("Please select replacement Body style config system.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingBodyStyleConfig.id) === Number(this.replacementBodyStyleConfig.id)) {
            this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.filter(function (item) { return item.isSelected; }).length <= 0) {
            this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    BodyStyleConfigReplaceConfirmComponent.prototype.onSubmitChangeRequest = function () {
        var _this = this;
        if (this.validateReplaceConfirmBodyStyleConfig()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    _this.existingBodyStyleConfig.attachments = uploadedFiles;
                }
                if (_this.existingBodyStyleConfig.attachments) {
                    _this.existingBodyStyleConfig.attachments = _this.existingBodyStyleConfig.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.existingBodyStyleConfig.vehicleToBodyStyleConfigs = _this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.filter(function (item) { return item.isSelected; });
                _this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.forEach(function (v) { v.bodyStyleConfigId = _this.replacementBodyStyleConfig.id; });
                var bodyStyleConfigIdentity = "Body style config id: " + _this.existingBodyStyleConfig.id;
                _this.bodyStyleConfigService.replaceBodyStyleConfig(_this.existingBodyStyleConfig.id, _this.existingBodyStyleConfig).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Body Style Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, bodyStyleConfigIdentity);
                        successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " Body Style Config " + bodyStyleConfigIdentity + " change request ID \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body Style Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, bodyStyleConfigIdentity);
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, (function (errorresponse) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body Style Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, bodyStyleConfigIdentity);
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
    BodyStyleConfigReplaceConfirmComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BodyStyleConfigReplaceConfirmComponent.prototype, "acFileUploader", void 0);
    BodyStyleConfigReplaceConfirmComponent = __decorate([
        core_1.Component({
            selector: "bodyStyleConfig-replace-component",
            templateUrl: "app/templates/bodyStyleConfig/bodyStyleConfig-replaceConfirm.component.html",
        }), 
        __metadata('design:paramtypes', [bodyStyleConfig_service_1.BodyStyleConfigService, router_1.Router, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager])
    ], BodyStyleConfigReplaceConfirmComponent);
    return BodyStyleConfigReplaceConfirmComponent;
}());
exports.BodyStyleConfigReplaceConfirmComponent = BodyStyleConfigReplaceConfirmComponent;
