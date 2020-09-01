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
var brakeConfig_service_1 = require("./brakeConfig.service");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var BrakeConfigReplaceConfirmComponent = (function () {
    function BrakeConfigReplaceConfirmComponent(brakeConfigService, router, route, toastr) {
        this.brakeConfigService = brakeConfigService;
        this.router = router;
        this.route = route;
        this.toastr = toastr;
        this.showLoadingGif = false;
    }
    BrakeConfigReplaceConfirmComponent.prototype.ngOnInit = function () {
        // Load existing brake config with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        // Get existing / replace brake config records from factory/ service.
        this.existingBrakeConfig = this.brakeConfigService.existingBrakeConfig;
        this.replacementBrakeConfig = this.brakeConfigService.replacementBrakeConfig;
    };
    // validation
    BrakeConfigReplaceConfirmComponent.prototype.validateReplaceConfirmBrakeConfig = function () {
        var isValid = true;
        // check required fields
        if (!this.existingBrakeConfig || !this.existingBrakeConfig.vehicleToBrakeConfigs || !this.replacementBrakeConfig) {
            this.toastr.warning("Not implemented.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementBrakeConfig.frontBrakeTypeId) === -1) {
            this.toastr.warning("Please select Front brake type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementBrakeConfig.rearBrakeTypeId) === -1) {
            this.toastr.warning("Please select Rear brake type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementBrakeConfig.brakeABSId) === -1) {
            this.toastr.warning("Please select Brake ABS.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementBrakeConfig.brakeSystemId) === -1) {
            this.toastr.warning("Please select Brake system.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replacementBrakeConfig.id) < 1) {
            this.toastr.warning("Please select replacement Brake config system.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingBrakeConfig.id) === Number(this.replacementBrakeConfig.id)) {
            this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingBrakeConfig.vehicleToBrakeConfigs.filter(function (item) { return item.isSelected; }).length <= 0) {
            this.toastr.warning("No Associations selected.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    // event on submit change request
    //onSubmitChangeRequest() {
    //    debugger;
    //    if (this.validateReplaceConfirmBrakeConfig()) {
    //        for (let vehicleToBrakeConfig of this.existingBrakeConfig.vehicleToBrakeConfigs.filter(item => item.isSelected)) {
    //            let vehicleToBrakeConfigIdentity: string = "Vehicle to brake config id: " + vehicleToBrakeConfig.id;
    //            // change brake config id of vehicletobrakeconfig
    //            //vehicleToBrakeConfig.brakeConfig.id = this.replacementBrakeConfig.id;
    //            vehicleToBrakeConfig.brakeConfig = this.replacementBrakeConfig;
    //            this.vehicleToBrakeService.updateVehicleToBrakeConfig(vehicleToBrakeConfig.id, vehicleToBrakeConfig).subscribe(response => {
    //                if (response) {
    //                    let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle To Brake Config", ConstantsWarehouse.changeRequestType.modify, vehicleToBrakeConfigIdentity);
    //                    //successMessage.title = "You request to replace a vehicle to brake system will be reviewed.";
    //                    this.toastr.success(successMessage.body, successMessage.title);
    //                }
    //                else {
    //                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle To Brake Config", ConstantsWarehouse.changeRequestType.modify, vehicleToBrakeConfigIdentity);
    //                    //errorMessage.title = "Your requested change cannot be submitted.";
    //                    this.toastr.warning(errorMessage.body, errorMessage.title);
    //                }
    //            }, error => {
    //                let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle To Brake Config", ConstantsWarehouse.changeRequestType.modify, vehicleToBrakeConfigIdentity);
    //                    //errorMessage.title = "Your requested change cannot be submitted.";
    //                    this.toastr.warning(errorMessage.body, errorMessage.title);
    //            });
    //        }
    //        // redirect to search result
    //        //this.router.navigate(["BrakeConfigSearch"]);
    //    }
    //}
    BrakeConfigReplaceConfirmComponent.prototype.onSubmitChangeRequest = function () {
        var _this = this;
        if (this.validateReplaceConfirmBrakeConfig()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    _this.existingBrakeConfig.attachments = uploadedFiles;
                }
                if (_this.existingBrakeConfig.attachments) {
                    _this.existingBrakeConfig.attachments = _this.existingBrakeConfig.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.existingBrakeConfig.vehicleToBrakeConfigs = _this.existingBrakeConfig.vehicleToBrakeConfigs.filter(function (item) { return item.isSelected; });
                _this.existingBrakeConfig.vehicleToBrakeConfigs.forEach(function (v) { v.brakeConfigId = _this.replacementBrakeConfig.id; });
                var brakeConfigIdentity = "Brake config id: " + _this.existingBrakeConfig.id;
                _this.brakeConfigService.replaceBrakeConfig(_this.existingBrakeConfig.id, _this.existingBrakeConfig).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Brake Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, brakeConfigIdentity);
                        successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " Brake Config " + brakeConfigIdentity + " change request ID \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, brakeConfigIdentity);
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, (function (errorresponse) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake Config", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, brakeConfigIdentity);
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
    BrakeConfigReplaceConfirmComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BrakeConfigReplaceConfirmComponent.prototype, "acFileUploader", void 0);
    BrakeConfigReplaceConfirmComponent = __decorate([
        core_1.Component({
            selector: "brakeConfig-replace-component",
            templateUrl: "app/templates/brakeConfig/brakeConfig-replaceConfirm.component.html",
        }), 
        __metadata('design:paramtypes', [brakeConfig_service_1.BrakeConfigService, router_1.Router, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager])
    ], BrakeConfigReplaceConfirmComponent);
    return BrakeConfigReplaceConfirmComponent;
}());
exports.BrakeConfigReplaceConfirmComponent = BrakeConfigReplaceConfirmComponent;
