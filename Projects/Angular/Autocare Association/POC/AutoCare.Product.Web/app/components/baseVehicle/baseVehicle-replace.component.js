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
var baseVehicle_service_1 = require("./baseVehicle.service");
var make_service_1 = require("../make/make.service");
var year_service_1 = require("../year/year.service");
var vehicle_service_1 = require("../vehicle/vehicle.service");
var httpHelper_1 = require("../httpHelper");
var constants_warehouse_1 = require('../constants-warehouse');
var BaseVehicleReplaceComponent = (function () {
    function BaseVehicleReplaceComponent(_makeService, yearService, baseVehicleService, vehicleService, route, toastr, router) {
        this._makeService = _makeService;
        this.yearService = yearService;
        this.baseVehicleService = baseVehicleService;
        this.vehicleService = vehicleService;
        this.route = route;
        this.toastr = toastr;
        this.router = router;
        this.newBaseVehicle = { id: -1, makeId: -1, makeName: "", modelId: -1, modelName: "", yearId: -1, vehicles: null };
        this.showLoadingGif = false;
    }
    BaseVehicleReplaceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.isSelectAllVehiclesToReplace = false;
        this.showLoadingGif = true;
        var id = this.route.snapshot.params['id'];
        this.makes = [];
        this.models = [];
        this.years = [];
        this.baseVehicleService.getBaseVehicle(id).subscribe(function (m) {
            _this.existingBaseVehicle = m;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
        this.yearService.getYears().subscribe(function (m) { return _this.years = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    BaseVehicleReplaceComponent.prototype.onBaseIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.onBaseIdSearch();
        }
    };
    BaseVehicleReplaceComponent.prototype.onBaseIdSearch = function () {
        var _this = this;
        var baseId = Number(this.baseIdSearchText);
        if (isNaN(baseId)) {
            this.toastr.warning("Invalid Base ID", "Message");
            return;
        }
        if (this.newBaseVehicle.id == baseId) {
            return;
        }
        this.newBaseVehicle = { id: -1, yearId: -1, makeId: -1, makeName: "", modelId: -1, modelName: "", vehicles: null };
        this.makes = null;
        this.models = null;
        this.showLoadingGif = true;
        this.baseVehicleService.getBaseVehicle(baseId).subscribe(function (b) {
            _this._makeService.getMakesByYearId(b.yearId).subscribe(function (mks) {
                _this.makes = mks;
                _this.baseVehicleService.getModelsByYearIdAndMakeId(b.yearId, b.makeId).subscribe(function (mdls) {
                    _this.models = mdls;
                    _this.newBaseVehicle = b;
                    _this.showLoadingGif = false;
                }, function (error) {
                    _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                    _this.showLoadingGif = false;
                });
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }, function (error) {
            var errorMessage = JSON.parse(String(error)).message;
            _this.toastr.warning(errorMessage, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    BaseVehicleReplaceComponent.prototype.onSelectYear = function () {
        var _this = this;
        this.newBaseVehicle.id = -1;
        this.models = [];
        this.newBaseVehicle.makeId = -1;
        if (this.newBaseVehicle.yearId == -1) {
            this.makes = [];
            return;
        }
        this.makes = null;
        this._makeService.getMakesByYearId(this.newBaseVehicle.yearId).subscribe(function (m) { return _this.makes = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    BaseVehicleReplaceComponent.prototype.onSelectMake = function () {
        var _this = this;
        this.newBaseVehicle.id = -1;
        this.models = [];
        if (this.newBaseVehicle.makeId == -1) {
            this.models = [];
            return;
        }
        this.models = null;
        this.baseVehicleService.getModelsByYearIdAndMakeId(this.newBaseVehicle.yearId, this.newBaseVehicle.makeId).subscribe(function (m) { return _this.models = m; }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
    };
    BaseVehicleReplaceComponent.prototype.onViewAffectedVehicles = function () {
        var _this = this;
        if (this.existingBaseVehicle && !this.existingBaseVehicle.vehicles) {
            this.showLoadingGif = true;
            this.vehicleService.getVehiclesByBaseVehicleId(this.existingBaseVehicle.id).subscribe(function (m) {
                _this.existingBaseVehicle.vehicles = m;
                _this.existingBaseVehicle.vehicleCount = m.length;
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }
    };
    BaseVehicleReplaceComponent.prototype.onSelectAllVehicles = function (isSelected) {
        this.isSelectAllVehiclesToReplace = isSelected;
        if (this.existingBaseVehicle.vehicles == null) {
            return;
        }
        this.existingBaseVehicle.vehicles.forEach(function (item) { return item.isSelected = isSelected; });
    };
    BaseVehicleReplaceComponent.prototype.onVehiclesToReplaceSelect = function (vehicle) {
        if (vehicle.isSelected) {
            //unchecked
            this.isSelectAllVehiclesToReplace = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingBaseVehicle.vehicles.filter(function (item) { return item.id != vehicle.id; });
            if (excludedVehicle.every(function (item) { return item.isSelected; })) {
                this.isSelectAllVehiclesToReplace = true;
            }
        }
    };
    BaseVehicleReplaceComponent.prototype.onContinue = function () {
        var _this = this;
        if (!this.newBaseVehicle || this.newBaseVehicle.id == -1) {
            this.toastr.warning("Please select a replacement base vehicle", "Message");
            return;
        }
        if (this.existingBaseVehicle.vehicles == null) {
            this.toastr.warning("No vehicles selected", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.newBaseVehicle.id == this.existingBaseVehicle.id) {
            this.toastr.warning("Nothing changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.existingBaseVehicle.vehicles.filter(function (item) { return item.isSelected; }).length <= 0) {
            this.toastr.warning("No vehicles selected", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.baseVehicleService.existingBaseVehicle = this.existingBaseVehicle;
        this.baseVehicleService.existingBaseVehicle.vehicles = this.existingBaseVehicle.vehicles.filter(function (item) { return item.isSelected; });
        this.newBaseVehicle.makeName = this.makes.filter(function (item) { return item.id == _this.newBaseVehicle.makeId; })[0].name;
        var selectedModel = this.models.filter(function (item) { return item.baseVehicleId == _this.newBaseVehicle.id; })[0];
        this.newBaseVehicle.modelId = selectedModel.id;
        this.newBaseVehicle.modelName = selectedModel.name;
        this.baseVehicleService.replacementBaseVehicle = this.newBaseVehicle;
        this.router.navigateByUrl("/basevehicle/replace/" + this.existingBaseVehicle.id + "/confirm");
    };
    BaseVehicleReplaceComponent = __decorate([
        core_1.Component({
            selector: "baseVehicle-replace-component",
            templateUrl: "app/templates/baseVehicle/baseVehicle-replace.component.html",
            providers: [year_service_1.YearService, make_service_1.MakeService, vehicle_service_1.VehicleService, httpHelper_1.HttpHelper]
        }), 
        __metadata('design:paramtypes', [make_service_1.MakeService, year_service_1.YearService, baseVehicle_service_1.BaseVehicleService, vehicle_service_1.VehicleService, router_1.ActivatedRoute, ng2_toastr_1.ToastsManager, router_1.Router])
    ], BaseVehicleReplaceComponent);
    return BaseVehicleReplaceComponent;
}());
exports.BaseVehicleReplaceComponent = BaseVehicleReplaceComponent;
