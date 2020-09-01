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
var core_1 = require('@angular/core');
var router_1 = require('@angular/router');
var baseVehicle_service_1 = require('./baseVehicle.service');
var httpHelper_1 = require('../httpHelper');
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var BaseVehicleReplaceConfirmComponent = (function () {
    function BaseVehicleReplaceConfirmComponent(baseVehicleService, route, toastr, _router) {
        this.baseVehicleService = baseVehicleService;
        this.route = route;
        this.toastr = toastr;
        this._router = _router;
        this.comment = '';
        this.showLoadingGif = false;
        this.existingBaseVehicle = baseVehicleService.existingBaseVehicle;
        this.newBaseVehicle = baseVehicleService.replacementBaseVehicle;
    }
    BaseVehicleReplaceConfirmComponent.prototype.onReplaceChecked = function () {
        var _this = this;
        //validation for comment
        if (!this.existingBaseVehicle || !this.existingBaseVehicle.vehicles) {
            return;
        }
        if (this.existingBaseVehicle.vehicles.filter(function (item) { return item.isSelected; }).length <= 0) {
            this.toastr.warning("No vehicles selected", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.existingBaseVehicle.vehicles = this.existingBaseVehicle.vehicles.filter(function (item) { return item.isSelected; });
        this.existingBaseVehicle.vehicles.forEach(function (v) { v.baseVehicleId = _this.newBaseVehicle.id; });
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.existingBaseVehicle.attachments = uploadedFiles;
            }
            if (_this.existingBaseVehicle.attachments) {
                _this.existingBaseVehicle.attachments = _this.existingBaseVehicle.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.baseVehicleService.replaceBaseVehicle(_this.existingBaseVehicle.id, _this.existingBaseVehicle).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Base Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, 'Base Vehicle id: ' + _this.existingBaseVehicle.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.existingBaseVehicle.id + "\" Base Vehicle requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this._router.navigateByUrl('vehicle/search');
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Base Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, 'Base Vehicle id: ' + _this.existingBaseVehicle.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                _this.showLoadingGif = false;
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Base Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, 'Base Vehicle id: ' + _this.existingBaseVehicle.id);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.acFileUploader.reset();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.acFileUploader.reset();
            _this.showLoadingGif = false;
        });
    };
    BaseVehicleReplaceConfirmComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BaseVehicleReplaceConfirmComponent.prototype, "acFileUploader", void 0);
    BaseVehicleReplaceConfirmComponent = __decorate([
        core_1.Component({
            selector: 'baseVehicle-replaceConfirm-component',
            templateUrl: 'app/templates/baseVehicle/baseVehicle-replaceConfirm.component.html',
            providers: [httpHelper_1.HttpHelper]
        }), 
        __metadata('design:paramtypes', [baseVehicle_service_1.BaseVehicleService, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager, router_1.Router])
    ], BaseVehicleReplaceConfirmComponent);
    return BaseVehicleReplaceConfirmComponent;
}());
exports.BaseVehicleReplaceConfirmComponent = BaseVehicleReplaceConfirmComponent;
