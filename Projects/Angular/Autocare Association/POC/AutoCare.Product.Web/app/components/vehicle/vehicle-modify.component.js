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
var subModel_service_1 = require('../subModel/subModel.service');
var region_service_1 = require('../region/region.service');
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var VehicleModifyComponent = (function () {
    function VehicleModifyComponent(subModelService, regionService, vehicleService, _router, toastr, route) {
        this.subModelService = subModelService;
        this.regionService = regionService;
        this.vehicleService = vehicleService;
        this._router = _router;
        this.toastr = toastr;
        this.route = route;
        this.showLoadingGif = false;
    }
    VehicleModifyComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        var id = Number(this.route.snapshot.params["id"]);
        this.subModelService.getAllSubModels().subscribe(function (m) { return _this.subModels = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        this.regionService.getRegion().subscribe(function (m) { return _this.regions = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        this.vehicleService.getVehicle(id).subscribe(function (m) {
            _this.vehicle = m;
            _this.modifiedVehicle = JSON.parse(JSON.stringify(_this.vehicle));
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleModifyComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (this.modifiedVehicle.subModelId == -1) {
            this.toastr.warning('Please select submodel.', constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        else if (this.modifiedVehicle.regionId == -1) {
            this.toastr.warning('Please select region.', constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        else if (this.vehicle.subModelId == this.modifiedVehicle.subModelId && this.vehicle.regionId == this.modifiedVehicle.regionId) {
            this.toastr.warning('Nothing Changed', constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedVehicle.attachments = uploadedFiles;
            }
            if (_this.modifiedVehicle.attachments) {
                _this.modifiedVehicle.attachments = _this.modifiedVehicle.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleService.updateVehicle(_this.vehicle.id, _this.modifiedVehicle).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, 'Vehicle ID: ' + _this.vehicle.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " Vehicle ID \"" + _this.vehicle.id + "\"  change request Id  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this._router.navigateByUrl('vehicle/search');
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, 'Vehicle ID: ' + _this.vehicle.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                _this.acFileUploader.reset();
                _this.showLoadingGif = false;
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, 'Vehicle ID: ' + _this.vehicle.id);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.showLoadingGif = false;
                _this.acFileUploader.reset(true);
            });
        }, function (error) {
            _this.acFileUploader.reset();
            _this.showLoadingGif = false;
        });
    };
    VehicleModifyComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleModifyComponent.prototype, "acFileUploader", void 0);
    VehicleModifyComponent = __decorate([
        core_1.Component({
            selector: 'vehicle-modify-component',
            templateUrl: 'app/templates/vehicle/vehicle-modify.component.html',
            providers: [vehicle_service_1.VehicleService, subModel_service_1.SubModelService, region_service_1.RegionService]
        }), 
        __metadata('design:paramtypes', [subModel_service_1.SubModelService, region_service_1.RegionService, vehicle_service_1.VehicleService, router_1.Router, ng2_toastr_1.ToastsManager, router_1.ActivatedRoute])
    ], VehicleModifyComponent);
    return VehicleModifyComponent;
}());
exports.VehicleModifyComponent = VehicleModifyComponent;
