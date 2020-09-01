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
var brakeConfig_service_1 = require("./brakeConfig.service");
var brakeType_service_1 = require("../brakeType/brakeType.service");
var brakeABS_service_1 = require("../brakeABS/brakeABS.service");
var brakeSystem_service_1 = require("../brakeSystem/brakeSystem.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var BrakeConfigModifyComponent = (function () {
    function BrakeConfigModifyComponent(_brakeConfigService, _brakeAbsService, _brakeSystemService, _brakeTypeSerivce, toastr, route, router) {
        this._brakeConfigService = _brakeConfigService;
        this._brakeAbsService = _brakeAbsService;
        this._brakeSystemService = _brakeSystemService;
        this._brakeTypeSerivce = _brakeTypeSerivce;
        this.toastr = toastr;
        this.route = route;
        this.router = router;
        this.showLoadingGif = false;
    }
    BrakeConfigModifyComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        var id = Number(this.route.snapshot.params["id"]);
        this._brakeTypeSerivce.getAllBrakeTypes().subscribe(function (x) { return _this.brakeTypes = x; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        this._brakeAbsService.getAllBrakeABSes().subscribe(function (x) { return _this.brakeABSes = x; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        this._brakeSystemService.getAllBrakeSystems().subscribe(function (x) { return _this.brakeSystems = x; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        this._brakeConfigService.getBrakeConfig(id).subscribe(function (x) {
            _this.existingBrakeConfig = x;
            _this.modifiedBrakeConfig = JSON.parse(JSON.stringify(_this.existingBrakeConfig));
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    // validation
    BrakeConfigModifyComponent.prototype.validateModifyBrakeConfig = function () {
        var isValid = true;
        // check required fields
        if (Number(this.modifiedBrakeConfig.frontBrakeTypeId) === -1) {
            this.toastr.warning("Please select Front brake type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.modifiedBrakeConfig.rearBrakeTypeId) === -1) {
            this.toastr.warning("Please select Rear brake type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.modifiedBrakeConfig.brakeABSId) === -1) {
            this.toastr.warning("Please select Brake ABS.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.modifiedBrakeConfig.brakeSystemId) === -1) {
            this.toastr.warning("Please select Brake system.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    BrakeConfigModifyComponent.prototype.onSubmitChangeRequests = function () {
        var _this = this;
        if (this.validateModifyBrakeConfig()) {
            var brakeConfigIdentity_1 = this.modifiedBrakeConfig.frontBrakeTypeName + ','
                + this.modifiedBrakeConfig.rearBrakeTypeName + ',' + this.modifiedBrakeConfig.brakeABSName
                + ',' + this.modifiedBrakeConfig.brakeSystemName;
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    _this.modifiedBrakeConfig.attachments = uploadedFiles;
                }
                if (_this.modifiedBrakeConfig.attachments) {
                    _this.modifiedBrakeConfig.attachments = _this.modifiedBrakeConfig.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this._brakeConfigService.updateBrakeConfig(_this.modifiedBrakeConfig.id, _this.modifiedBrakeConfig).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, brakeConfigIdentity_1);
                        successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " Brake System " + brakeConfigIdentity_1 + " change request ID \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, brakeConfigIdentity_1);
                        errorMessage.title = "Your requested change cannot be submitted.";
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, (function (errorresponse) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, brakeConfigIdentity_1);
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
    BrakeConfigModifyComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BrakeConfigModifyComponent.prototype, "acFileUploader", void 0);
    BrakeConfigModifyComponent = __decorate([
        core_1.Component({
            selector: "brakeConfig-modify-component",
            templateUrl: "app/templates/brakeConfig/brakeConfig-modify.component.html",
            providers: [brakeConfig_service_1.BrakeConfigService],
        }), 
        __metadata('design:paramtypes', [brakeConfig_service_1.BrakeConfigService, brakeABS_service_1.BrakeABSService, brakeSystem_service_1.BrakeSystemService, brakeType_service_1.BrakeTypeService, ng2_toastr_1.ToastsManager, router_1.ActivatedRoute, router_1.Router])
    ], BrakeConfigModifyComponent);
    return BrakeConfigModifyComponent;
}());
exports.BrakeConfigModifyComponent = BrakeConfigModifyComponent;
