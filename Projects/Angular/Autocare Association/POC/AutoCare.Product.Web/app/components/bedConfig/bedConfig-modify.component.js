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
var bedConfig_service_1 = require("./bedConfig.service");
var bedType_service_1 = require("../bedType/bedType.service");
var bedLength_service_1 = require("../bedLength/bedLength.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var BedConfigModifyComponent = (function () {
    function BedConfigModifyComponent(bedConfigService, bedLengthService, bedTypeService, toastr, route, router) {
        this.bedConfigService = bedConfigService;
        this.bedLengthService = bedLengthService;
        this.bedTypeService = bedTypeService;
        this.toastr = toastr;
        this.route = route;
        this.router = router;
        this.showLoadingGif = false;
    }
    BedConfigModifyComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        var id = Number(this.route.snapshot.params["id"]);
        this.bedTypeService.getAllBedTypes().subscribe(function (x) { return _this.bedTypes = x; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        this.bedLengthService.getAllBedLengths().subscribe(function (x) { return _this.bedLengths = x; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        this.bedConfigService.getBedConfig(id).subscribe(function (x) {
            _this.existingBedConfig = x;
            _this.modifiedBedConfig = JSON.parse(JSON.stringify(_this.existingBedConfig));
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    // validation
    BedConfigModifyComponent.prototype.validateModifyBedConfig = function () {
        var isValid = true;
        // check required fields
        if (Number(this.modifiedBedConfig.bedLengthId) === -1) {
            this.toastr.warning("Please select Bed Length.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.modifiedBedConfig.bedTypeId) === -1) {
            this.toastr.warning("Please select Bed Type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    BedConfigModifyComponent.prototype.onSubmitChangeRequests = function () {
        var _this = this;
        if (this.validateModifyBedConfig()) {
            var bedConfigIdentity_1 = this.modifiedBedConfig.length + ','
                + this.modifiedBedConfig.bedTypeName;
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    _this.modifiedBedConfig.attachments = uploadedFiles;
                }
                if (_this.modifiedBedConfig.attachments) {
                    _this.modifiedBedConfig.attachments = _this.modifiedBedConfig.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.bedConfigService.updateBedConfig(_this.modifiedBedConfig.id, _this.modifiedBedConfig).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Bed System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, bedConfigIdentity_1);
                        successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " Bed System " + bedConfigIdentity_1 + " change request ID \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, bedConfigIdentity_1);
                        errorMessage.title = "Your requested change cannot be submitted.";
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, (function (errorresponse) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, bedConfigIdentity_1);
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
    BedConfigModifyComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BedConfigModifyComponent.prototype, "acFileUploader", void 0);
    BedConfigModifyComponent = __decorate([
        core_1.Component({
            selector: "bedConfig-modify-component",
            templateUrl: "app/templates/bedConfig/bedConfig-modify.component.html",
            providers: [bedConfig_service_1.BedConfigService],
        }), 
        __metadata('design:paramtypes', [bedConfig_service_1.BedConfigService, bedLength_service_1.BedLengthService, bedType_service_1.BedTypeService, ng2_toastr_1.ToastsManager, router_1.ActivatedRoute, router_1.Router])
    ], BedConfigModifyComponent);
    return BedConfigModifyComponent;
}());
exports.BedConfigModifyComponent = BedConfigModifyComponent;
