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
var constants_warehouse_1 = require("../constants-warehouse");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var BrakeConfigDeleteComponent = (function () {
    function BrakeConfigDeleteComponent(brakeConfigService, toastr, route, router) {
        this.brakeConfigService = brakeConfigService;
        this.toastr = toastr;
        this.route = route;
        this.router = router;
        this.showLoadingGif = false;
    }
    BrakeConfigDeleteComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        // Load Existing brake config with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        this.brakeConfigService.getBrakeConfig(id).subscribe(function (result) {
            _this.brakeConfig = result;
            _this.brakeConfig.vehicleToBrakeConfigs = [];
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    BrakeConfigDeleteComponent.prototype.deleteSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.brakeConfig.attachments = uploadedFiles;
            }
            if (_this.brakeConfig.attachments) {
                _this.brakeConfig.attachments = _this.brakeConfig.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.brakeConfigService.deleteBrakeConfig(_this.brakeConfig.id, _this.brakeConfig).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, "BrakeConfigId: " + _this.brakeConfig.id);
                    successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " BrakeConfigId " + _this.brakeConfig.id + " change request ID \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.router.navigateByUrl("/system/search");
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, "BrakeConfigId: " + _this.brakeConfig.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, "BrakeConfigId: " + _this.brakeConfig.id);
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
    };
    BrakeConfigDeleteComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BrakeConfigDeleteComponent.prototype, "acFileUploader", void 0);
    BrakeConfigDeleteComponent = __decorate([
        core_1.Component({
            selector: "brakeConfig-delete-component",
            templateUrl: "app/templates/brakeConfig/brakeConfig-delete.component.html",
            providers: [brakeConfig_service_1.BrakeConfigService],
        }), 
        __metadata('design:paramtypes', [brakeConfig_service_1.BrakeConfigService, ng2_toastr_1.ToastsManager, router_1.ActivatedRoute, router_1.Router])
    ], BrakeConfigDeleteComponent);
    return BrakeConfigDeleteComponent;
}());
exports.BrakeConfigDeleteComponent = BrakeConfigDeleteComponent;
