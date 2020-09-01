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
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var baseVehicle_service_1 = require("./baseVehicle.service");
var constants_warehouse_1 = require("../constants-warehouse");
var httpHelper_1 = require("../httpHelper");
var vehicle_service_1 = require("../vehicle/vehicle.service");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var BaseVehicleDeleteComponent = (function () {
    function BaseVehicleDeleteComponent(baseVehicleService, _vehicleService, route, toastr, _router) {
        this.baseVehicleService = baseVehicleService;
        this._vehicleService = _vehicleService;
        this.route = route;
        this.toastr = toastr;
        this._router = _router;
        this.showLoadingGif = false;
    }
    BaseVehicleDeleteComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        // Load existing base vechile with reference from RouteParams
        var id = Number(this.route.snapshot.params['id']);
        this.baseVehicleService.getBaseVehicle(id).subscribe(function (result) {
            _this.deleteBaseVehicle = result;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    ;
    // delete
    BaseVehicleDeleteComponent.prototype.onDeleteBaseVehicle = function () {
        var _this = this;
        // make current base vehicle identity
        var baseVehicleIdentity = this.deleteBaseVehicle.makeName + ", "
            + this.deleteBaseVehicle.modelName;
        // delete base vehicle
        //Comment parameter is added on deleteBaseVehicle
        //post called for delete to pass whole object
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteBaseVehicle.attachments = uploadedFiles;
            }
            if (_this.deleteBaseVehicle.attachments) {
                _this.deleteBaseVehicle.attachments = _this.deleteBaseVehicle.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.baseVehicleService.deleteBaseVehicle(_this.deleteBaseVehicle.id, _this.deleteBaseVehicle).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Base Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, baseVehicleIdentity);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + baseVehicleIdentity + "\" Base Vehicle requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this._router.navigateByUrl('vehicle/search');
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Base Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, baseVehicleIdentity);
                    errorMessage.title = "Your requested change cannot be submitted";
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                _this.showLoadingGif = false;
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Base Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, baseVehicleIdentity);
                errorMessage.title = "Your requested change cannot be submitted";
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.acFileUploader.reset();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.acFileUploader.reset();
        });
    };
    // view affected vehicles
    BaseVehicleDeleteComponent.prototype.onViewAffectedVehicles = function () {
        var _this = this;
        if (!this.deleteBaseVehicle) {
            return;
        }
        this.vehiclePopup.open("lg");
        if (!this.deleteBaseVehicle.vehicles) {
            this._vehicleService.getVehiclesByBaseVehicleId(this.deleteBaseVehicle.id).subscribe(function (m) {
                _this.deleteBaseVehicle.vehicles = m;
                _this.deleteBaseVehicle.vehicleCount = m.length;
            }, function (error) { _this.toastr.warning(error); });
        }
    };
    BaseVehicleDeleteComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild("viewAffectedVehiclesModal"), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BaseVehicleDeleteComponent.prototype, "vehiclePopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BaseVehicleDeleteComponent.prototype, "acFileUploader", void 0);
    BaseVehicleDeleteComponent = __decorate([
        core_1.Component({
            selector: "baseVehicle-modify-component",
            templateUrl: "app/templates/baseVehicle/baseVehicle-delete.component.html",
            providers: [baseVehicle_service_1.BaseVehicleService, vehicle_service_1.VehicleService, httpHelper_1.HttpHelper]
        }), 
        __metadata('design:paramtypes', [baseVehicle_service_1.BaseVehicleService, vehicle_service_1.VehicleService, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager, router_1.Router])
    ], BaseVehicleDeleteComponent);
    return BaseVehicleDeleteComponent;
}());
exports.BaseVehicleDeleteComponent = BaseVehicleDeleteComponent;
