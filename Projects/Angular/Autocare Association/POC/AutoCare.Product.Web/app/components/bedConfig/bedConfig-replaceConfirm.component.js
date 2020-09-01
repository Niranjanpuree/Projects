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
var bedConfig_service_1 = require("./bedConfig.service");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var BedConfigReplaceConfirmComponent = (function () {
    function BedConfigReplaceConfirmComponent(bedConfigService, router, route, toastr) {
        this.bedConfigService = bedConfigService;
        this.router = router;
        this.route = route;
        this.toastr = toastr;
        this.showLoadingGif = false;
    }
    BedConfigReplaceConfirmComponent.prototype.ngOnInit = function () {
        // Load existing bed config with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        // Get existing / replace bed config records from factory/ service.
        this.existingBedConfig = this.bedConfigService.existingBedConfig;
        this.replacementBedConfig = this.bedConfigService.replacementBedConfig;
    };
    // validation
    BedConfigReplaceConfirmComponent.prototype.validateReplaceConfirmBedConfig = function () {
        var isValid = true;
        // check required fields
        if (!this.existingBedConfig || !this.existingBedConfig.vehicleToBedConfigs || !this.replacementBedConfig) {
            this.toastr.warning("Not implemented.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementBedConfig.bedLengthId) === -1) {
            this.toastr.warning("Please select Bed length.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementBedConfig.bedTypeId) === -1) {
            this.toastr.warning("Please select BEd type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementBedConfig.id) < 1) {
            this.toastr.warning("Please select replacement Bed config system.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingBedConfig.id) === Number(this.replacementBedConfig.id)) {
            this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingBedConfig.vehicleToBedConfigs.filter(function (item) { return item.isSelected; }).length <= 0) {
            this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    BedConfigReplaceConfirmComponent.prototype.onSubmitChangeRequest = function () {
        var _this = this;
        if (this.validateReplaceConfirmBedConfig()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    _this.existingBedConfig.attachments = uploadedFiles;
                }
                if (_this.existingBedConfig.attachments) {
                    _this.existingBedConfig.attachments = _this.existingBedConfig.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.existingBedConfig.vehicleToBedConfigs = _this.existingBedConfig.vehicleToBedConfigs.filter(function (item) { return item.isSelected; });
                _this.existingBedConfig.vehicleToBedConfigs.forEach(function (v) { v.bedConfigId = _this.replacementBedConfig.id; });
                var bedConfigIdentity = "Bed config id: " + _this.existingBedConfig.id;
                _this.bedConfigService.replaceBedConfig(_this.existingBedConfig.id, _this.existingBedConfig).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Bed Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, bedConfigIdentity);
                        successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " Bed Config " + bedConfigIdentity + " change request ID \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, bedConfigIdentity);
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, (function (errorresponse) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, bedConfigIdentity);
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
    BedConfigReplaceConfirmComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BedConfigReplaceConfirmComponent.prototype, "acFileUploader", void 0);
    BedConfigReplaceConfirmComponent = __decorate([
        core_1.Component({
            selector: "bedConfig-replace-component",
            templateUrl: "app/templates/bedConfig/bedConfig-replaceConfirm.component.html",
        }), 
        __metadata('design:paramtypes', [bedConfig_service_1.BedConfigService, router_1.Router, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager])
    ], BedConfigReplaceConfirmComponent);
    return BedConfigReplaceConfirmComponent;
}());
exports.BedConfigReplaceConfirmComponent = BedConfigReplaceConfirmComponent;
