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
var bodyStyleConfig_service_1 = require("./bodyStyleConfig.service");
var bodyType_service_1 = require("../bodyType/bodyType.service");
var bodyNumDoors_service_1 = require("../bodyNumDoors/bodyNumDoors.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var BodyStyleConfigModifyComponent = (function () {
    function BodyStyleConfigModifyComponent(bodyStyleConfigService, bodyNumDoorService, bodyTypeService, toastr, route, router) {
        this.bodyStyleConfigService = bodyStyleConfigService;
        this.bodyNumDoorService = bodyNumDoorService;
        this.bodyTypeService = bodyTypeService;
        this.toastr = toastr;
        this.route = route;
        this.router = router;
        this.showLoadingGif = false;
    }
    BodyStyleConfigModifyComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        var id = Number(this.route.snapshot.params["id"]);
        this.bodyTypeService.getAllBodyTypes().subscribe(function (x) { return _this.bodyTypes = x; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        this.bodyNumDoorService.getAllBodyNumDoors().subscribe(function (x) { return _this.bodyNumDoors = x; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        this.bodyStyleConfigService.getBodyStyleConfig(id).subscribe(function (x) {
            _this.existingBodyStyleConfig = x;
            _this.modifiedBodyStyleConfig = JSON.parse(JSON.stringify(_this.existingBodyStyleConfig));
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    // validation
    BodyStyleConfigModifyComponent.prototype.validateModifyBodyStyleConfig = function () {
        var isValid = true;
        // check required fields
        if (Number(this.modifiedBodyStyleConfig.bodyNumDoorsId) === -1) {
            this.toastr.warning("Please select Body Number Doors.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.modifiedBodyStyleConfig.bodyTypeId) === -1) {
            this.toastr.warning("Please select Body Type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    BodyStyleConfigModifyComponent.prototype.onSubmitChangeRequests = function () {
        var _this = this;
        if (this.validateModifyBodyStyleConfig()) {
            var bodyStyleConfigIdentity_1 = this.modifiedBodyStyleConfig.numDoors + ','
                + this.modifiedBodyStyleConfig.bodyTypeName;
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    _this.modifiedBodyStyleConfig.attachments = uploadedFiles;
                }
                if (_this.modifiedBodyStyleConfig.attachments) {
                    _this.modifiedBodyStyleConfig.attachments = _this.modifiedBodyStyleConfig.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.bodyStyleConfigService.updateBodyStyleConfig(_this.modifiedBodyStyleConfig.id, _this.modifiedBodyStyleConfig).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Body System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, bodyStyleConfigIdentity_1);
                        successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " Body System " + bodyStyleConfigIdentity_1 + " change request ID \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this.router.navigateByUrl("/system/search");
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, bodyStyleConfigIdentity_1);
                        errorMessage.title = "Your requested change cannot be submitted.";
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, (function (errorresponse) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, bodyStyleConfigIdentity_1);
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
    BodyStyleConfigModifyComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BodyStyleConfigModifyComponent.prototype, "acFileUploader", void 0);
    BodyStyleConfigModifyComponent = __decorate([
        core_1.Component({
            selector: "bodyStyleConfig-modify-component",
            templateUrl: "app/templates/bodyStyleConfig/bodyStyleConfig-modify.component.html",
            providers: [bodyStyleConfig_service_1.BodyStyleConfigService],
        }), 
        __metadata('design:paramtypes', [bodyStyleConfig_service_1.BodyStyleConfigService, bodyNumDoors_service_1.BodyNumDoorsService, bodyType_service_1.BodyTypeService, ng2_toastr_1.ToastsManager, router_1.ActivatedRoute, router_1.Router])
    ], BodyStyleConfigModifyComponent);
    return BodyStyleConfigModifyComponent;
}());
exports.BodyStyleConfigModifyComponent = BodyStyleConfigModifyComponent;
