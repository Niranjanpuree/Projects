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
var httpHelper_1 = require("../httpHelper");
var make_service_1 = require("../make/make.service");
var model_service_1 = require("../model/model.service");
var year_service_1 = require("../year/year.service");
var constants_warehouse_1 = require("../constants-warehouse");
var vehicle_service_1 = require("../vehicle/vehicle.service");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var BaseVehicleModifyComponent = (function () {
    function BaseVehicleModifyComponent(baseVehicleService, _vehicleService, route, toastr, makeService, modelService, yearService, _router) {
        this.baseVehicleService = baseVehicleService;
        this._vehicleService = _vehicleService;
        this.route = route;
        this.toastr = toastr;
        this.makeService = makeService;
        this.modelService = modelService;
        this.yearService = yearService;
        this._router = _router;
        this.showLoadingGif = false;
        // initalize empty changeBaseVehicle
        this.changeBaseVehicle = { id: 0, makeId: -1, makeName: '', modelId: -1, modelName: '', yearId: -1, vehicles: null };
    }
    BaseVehicleModifyComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        // Load existing base vechile with reference from RouteParams
        var id = Number(this.route.snapshot.params['id']);
        this.baseVehicleService.getBaseVehicle(id).subscribe(function (result) {
            _this.existingBaseVehicle = result;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
        // Load select options for change.
        this.makeService.getAllMakes().subscribe(function (mks) {
            _this.makes = mks;
            _this.modelService.getAllModels().subscribe(function (mdls) {
                _this.models = mdls;
                _this.changeBaseVehicle = JSON.parse(JSON.stringify(_this.existingBaseVehicle));
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            }); // models
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
        }); // makes
        this.yearService.getYears().subscribe(function (m) { return _this.years = m; }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
        }); // years
    };
    // Save changes
    BaseVehicleModifyComponent.prototype.onModifyBaseVehicle = function () {
        var _this = this;
        // validate change information
        if (this.validateChangeBaseVehicle()) {
            // make current base vehicle identity
            var baseVehicleIdentity_1 = this.makes.filter(function (item) { return item.id === Number(_this.changeBaseVehicle.makeId); })[0].name + ", "
                + this.models.filter(function (item) { return item.id === Number(_this.changeBaseVehicle.modelId); })[0].name;
            // modify base vehicle
            this.changeBaseVehicle.id = this.existingBaseVehicle.id;
            //NOTE: save numberOfVehicles in payload string, will be used in change review screen
            this.changeBaseVehicle.vehicleCount = this.existingBaseVehicle.vehicleCount;
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    _this.changeBaseVehicle.attachments = uploadedFiles;
                }
                if (_this.changeBaseVehicle.attachments) {
                    _this.changeBaseVehicle.attachments = _this.changeBaseVehicle.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
                }
                _this.baseVehicleService.updateBaseVehicle(_this.existingBaseVehicle.id, _this.changeBaseVehicle).subscribe(function (response) {
                    if (response) {
                        var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Base Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, baseVehicleIdentity_1);
                        successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + baseVehicleIdentity_1 + "\" Base Vehicle requestid  \"" + response + "\" will be reviewed.";
                        _this.toastr.success(successMessage.body, successMessage.title);
                        _this._router.navigateByUrl('vehicle/search');
                    }
                    else {
                        var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Base Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, baseVehicleIdentity_1);
                        errorMessage.title = "Your requested change cannot be submitted.";
                        _this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    _this.showLoadingGif = false;
                }, function (error) {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Base Vehicle", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, baseVehicleIdentity_1);
                    errorMessage.title = "Your requested change cannot be submitted.";
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
        }
    };
    // validate base vehicle
    BaseVehicleModifyComponent.prototype.validateChangeBaseVehicle = function () {
        var isValid = true;
        // check required fields
        if (Number(this.changeBaseVehicle.makeId) === -1) {
            this.toastr.warning("Please select Make.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.changeBaseVehicle.modelId) === -1) {
            this.toastr.warning("Please select Model.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.changeBaseVehicle.yearId) === -1) {
            this.toastr.warning("Please select Year.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingBaseVehicle.makeId === Number(this.changeBaseVehicle.makeId)
            && this.existingBaseVehicle.modelId === Number(this.changeBaseVehicle.modelId)
            && this.existingBaseVehicle.yearId === Number(this.changeBaseVehicle.yearId)) {
            this.toastr.warning("Existing and Change information are exactly same.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    // view affected vehicles
    BaseVehicleModifyComponent.prototype.onViewAffectedVehicles = function () {
        var _this = this;
        if (!this.existingBaseVehicle) {
            return;
        }
        this.vehiclePopup.open("lg");
        if (!this.existingBaseVehicle.vehicles) {
            this._vehicleService.getVehiclesByBaseVehicleId(this.existingBaseVehicle.id).subscribe(function (m) {
                _this.existingBaseVehicle.vehicles = m;
                _this.existingBaseVehicle.vehicleCount = m.length;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            });
        }
    };
    BaseVehicleModifyComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild("viewAffectedVehiclesModal"), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BaseVehicleModifyComponent.prototype, "vehiclePopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BaseVehicleModifyComponent.prototype, "acFileUploader", void 0);
    BaseVehicleModifyComponent = __decorate([
        core_1.Component({
            selector: "baseVehicle-modify-component",
            templateUrl: "app/templates/baseVehicle/baseVehicle-modify.component.html",
            providers: [baseVehicle_service_1.BaseVehicleService, vehicle_service_1.VehicleService, httpHelper_1.HttpHelper],
        }), 
        __metadata('design:paramtypes', [baseVehicle_service_1.BaseVehicleService, vehicle_service_1.VehicleService, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager, make_service_1.MakeService, model_service_1.ModelService, year_service_1.YearService, router_1.Router])
    ], BaseVehicleModifyComponent);
    return BaseVehicleModifyComponent;
}());
exports.BaseVehicleModifyComponent = BaseVehicleModifyComponent;
