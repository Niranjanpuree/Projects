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
var ng2_bs3_modal_1 = require('ng2-bs3-modal/ng2-bs3-modal');
var ng2_toastr_1 = require('../../lib/aclibs/ng2-toastr/ng2-toastr');
var fuelType_service_1 = require('./fuelType.service');
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var FuelTypeListComponent = (function () {
    function FuelTypeListComponent(fuelTypeService, router, toastr, navigationService) {
        var _this = this;
        this.fuelTypeService = fuelTypeService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.filteredFuelTypes = [];
        this.fuelType = {};
        this.modifiedFuelType = {};
        this.fuelTypeNameFilter = '';
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.fuelTypeNameFilter;
            }
            else {
                _this.fuelTypeNameFilter = keyword;
            }
            if (String(_this.fuelTypeNameFilter) === "") {
                _this.fuelTypeService.getAllFuelTypes().subscribe(function (sm) {
                    _this.fuelTypes = sm;
                    _this.showLoadingGif = false;
                    _this.filteredFuelTypes = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.fuelTypeService.getFuelTypes(_this.fuelTypeNameFilter).subscribe(function (m) {
                    _this.fuelTypes = m;
                    _this.showLoadingGif = false;
                    _this.filteredFuelTypes = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.fuelTypeService.getFuelTypes(keyword);
        };
    }
    FuelTypeListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.fuelTypeService.getAllFuelTypes().subscribe(function (sm) {
            _this.fuelTypes = sm;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    FuelTypeListComponent.prototype.onSelect = function (fuelType) {
        this.fuelTypeNameFilter = fuelType.name;
        this.applyFilter();
        this.filteredFuelTypes = [];
    };
    FuelTypeListComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
    };
    FuelTypeListComponent.prototype.onNew = function () {
        this.fuelType = {};
        this.newPopup.open("md");
    };
    FuelTypeListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.fuelType)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.fuelType.attachments = uploadedFiles;
            }
            if (_this.fuelType.attachments) {
                _this.fuelType.attachments = _this.fuelType.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.fuelTypeService.addFuelType(_this.fuelType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Fuel Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.fuelType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.fuelType.name + "\" Engine  Designation change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Fuel Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.fuelType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Fuel Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.fuelType.name);
                _this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.showLoadingGif = false;
                _this.acFileUploader.reset(true);
                _this.newPopup.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.acFileUploader.reset(true);
            _this.newPopup.close();
        });
    };
    FuelTypeListComponent.prototype.validationCheck = function (item) {
        var isValid = true;
        if (!item.name) {
            this.toastr.warning("FuelTypeName is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    FuelTypeListComponent.prototype.onModify = function (fuelType) {
        var _this = this;
        this.fuelType = fuelType;
        this.showLoadingGif = true;
        if (!fuelType.engineConfigCount && !fuelType.vehicleToEngineConfigCount) {
            this.fuelTypeService.getFuelTypeDetail(fuelType.id).subscribe(function (m) {
                fuelType.engineConfigCount = m.engineConfigCount;
                fuelType.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;
                _this.modifiedFuelType = JSON.parse(JSON.stringify(fuelType));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedFuelType = JSON.parse(JSON.stringify(fuelType));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    FuelTypeListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedFuelType)) {
            return;
        }
        else if (this.modifiedFuelType.name == this.fuelType.name) {
            this.toastr.warning("Nothing changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        debugger;
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedFuelType.attachments = uploadedFiles;
            }
            if (_this.modifiedFuelType.attachments) {
                _this.modifiedFuelType.attachments = _this.modifiedFuelType.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.fuelTypeService.updateFuelType(_this.modifiedFuelType.id, _this.modifiedFuelType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Fuel Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.fuelType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.fuelType.name + "\" Engine Designation change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.fuelTypes.filter(function (x) { return x.id == _this.modifiedFuelType.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Fuel Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.fuelType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Fuel Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.fuelType.name);
                _this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.showLoadingGif = false;
                _this.modifyPopupAcFileUploader.reset(true);
                _this.modifyPopup.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.modifyPopupAcFileUploader.reset(true);
            _this.modifyPopup.close();
        });
    };
    FuelTypeListComponent.prototype.onDelete = function (fuelType) {
        var _this = this;
        this.fuelType = fuelType;
        this.showLoadingGif = true;
        if (!fuelType.engineConfigCount && !fuelType.vehicleToEngineConfigCount) {
            this.fuelTypeService.getFuelTypeDetail(fuelType.id).subscribe(function (m) {
                fuelType.engineConfigCount = m.engineConfigCount;
                fuelType.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;
                if (fuelType.engineConfigCount > 0) {
                    _this.showLoadingGif = false;
                    _this.deleteErrorPopup.open("md");
                }
                else {
                    _this.showLoadingGif = false;
                    _this.deleteConfirmPopup.open("md");
                }
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            if (fuelType.engineConfigCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    FuelTypeListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        if (!this.validationCheck(this.fuelType)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.fuelType.attachments = uploadedFiles;
            }
            if (_this.fuelType.attachments) {
                _this.fuelType.attachments = _this.fuelType.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.fuelTypeService.deleteFuelType(_this.fuelType.id, _this.fuelType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Fuel Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.fuelType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.fuelType.name + "\" FuelType change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.fuelTypes.filter(function (x) { return x.id == _this.fuelType.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Fuel Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.fuelType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Fuel Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.fuelType.name);
                _this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.showLoadingGif = false;
                _this.deletePopupAcFileUploader.reset(true);
                _this.deleteConfirmPopup.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.deletePopupAcFileUploader.reset(true);
            _this.deleteConfirmPopup.close();
        });
    };
    FuelTypeListComponent.prototype.view = function (fuelTypeVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/fueltype/" + fuelTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    FuelTypeListComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], FuelTypeListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], FuelTypeListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], FuelTypeListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], FuelTypeListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], FuelTypeListComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], FuelTypeListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], FuelTypeListComponent.prototype, "deletePopupAcFileUploader", void 0);
    FuelTypeListComponent = __decorate([
        core_1.Component({
            selector: 'fuelType-list-component',
            templateUrl: 'app/templates/fuelType/fuelType-list.component.html',
        }), 
        __metadata('design:paramtypes', [fuelType_service_1.FuelTypeService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], FuelTypeListComponent);
    return FuelTypeListComponent;
}());
exports.FuelTypeListComponent = FuelTypeListComponent;
