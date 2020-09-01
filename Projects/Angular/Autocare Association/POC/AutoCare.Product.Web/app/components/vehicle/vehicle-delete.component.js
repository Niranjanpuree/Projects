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
var vehicle_service_1 = require('./vehicle.service');
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var VehicleDeleteComponent = (function () {
    function VehicleDeleteComponent(vehicleService, router, toastr, route) {
        this.vehicleService = vehicleService;
        this.router = router;
        this.toastr = toastr;
        this.route = route;
        this.comment = '';
        this.showLoadingGif = false;
    }
    VehicleDeleteComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        var id = Number(this.route.snapshot.params["id"]);
        this.vehicleService.getVehicle(id).subscribe(function (m) {
            _this.vehicle = m;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleDeleteComponent.prototype.onDeleteSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.vehicle.attachments = uploadedFiles;
            }
            if (_this.vehicle.attachments) {
                _this.vehicle.attachments = _this.vehicle.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleService.deleteVehicle(_this.vehicle.id, _this.vehicle).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, 'Vehicle ID: ' + _this.vehicle.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " Vehicle ID \"" + _this.vehicle.id + "\"  change request Id  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.router.navigateByUrl('vehicle/search');
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, 'Vehicle ID: ' + _this.vehicle.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                _this.showLoadingGif = false;
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, 'Vehicle ID: ' + _this.vehicle.id);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.acFileUploader.reset();
            _this.showLoadingGif = false;
        });
    };
    VehicleDeleteComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleDeleteComponent.prototype, "acFileUploader", void 0);
    VehicleDeleteComponent = __decorate([
        core_1.Component({
            selector: 'vehicle-delete-component',
            templateUrl: 'app/templates/vehicle/vehicle-delete.component.html',
            providers: [vehicle_service_1.VehicleService]
        }), 
        __metadata('design:paramtypes', [vehicle_service_1.VehicleService, router_1.Router, ng2_toastr_1.ToastsManager, router_1.ActivatedRoute])
    ], VehicleDeleteComponent);
    return VehicleDeleteComponent;
}());
exports.VehicleDeleteComponent = VehicleDeleteComponent;
