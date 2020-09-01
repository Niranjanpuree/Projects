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
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var vehicleType_service_1 = require('./vehicleType.service');
var vehicleTypeGroup_service_1 = require('../vehicleTypeGroup/vehicleTypeGroup.service');
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var VehicleTypeListComponent = (function () {
    function VehicleTypeListComponent(vehicleTypeService, vehicleTypeGroupService, router, toastr, navigationService) {
        var _this = this;
        this.vehicleTypeService = vehicleTypeService;
        this.vehicleTypeGroupService = vehicleTypeGroupService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.filteredVehicleTypes = [];
        this.vehicleType = {};
        this.modifiedVehicleType = {};
        this.vehicleTypeNameFilter = '';
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.vehicleTypeNameFilter;
            }
            else {
                _this.vehicleTypeNameFilter = keyword;
            }
            if (String(_this.vehicleTypeNameFilter) === "") {
                _this.vehicleTypeService.getAllVehicleTypes().subscribe(function (sm) {
                    _this.vehicleTypes = sm;
                    _this.showLoadingGif = false;
                    _this.filteredVehicleTypes = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.vehicleTypeService.getVehicleTypes(_this.vehicleTypeNameFilter).subscribe(function (m) {
                    _this.vehicleTypes = m;
                    _this.showLoadingGif = false;
                    _this.filteredVehicleTypes = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.vehicleTypeService.getVehicleTypes(keyword);
        };
    }
    VehicleTypeListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.vehicleTypeService.getAllVehicleTypes().subscribe(function (sm) {
            _this.vehicleTypes = sm;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
        this.vehicleTypeGroupService.getAllVehicleTypeGroups().subscribe(function (m) {
            _this.vehicleTypeGroups = m;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleTypeListComponent.prototype.onSelect = function (vehicleType) {
        this.vehicleTypeNameFilter = vehicleType.name;
        this.applyFilter();
        this.filteredVehicleTypes = [];
    };
    VehicleTypeListComponent.prototype.validationCheck = function (item) {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("Vehicle Type Name is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
            else if (this.vehicleType.vehicleTypeGroupId === -1) {
                this.toastr.warning('Please select Vehicle Type Group', constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
        }
        return true;
    };
    VehicleTypeListComponent.prototype.onCancel = function (action) {
        this.newPopupAcFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
    };
    VehicleTypeListComponent.prototype.onNew = function () {
        this.vehicleType = {};
        this.vehicleType.vehicleTypeGroupId = -1;
        this.newPopup.open("md");
    };
    VehicleTypeListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.vehicleType)) {
            return;
        }
        this.showLoadingGif = true;
        this.newPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.vehicleType.attachments = uploadedFiles;
            }
            if (_this.vehicleType.attachments) {
                _this.vehicleType.attachments = _this.vehicleType.attachments.concat(_this.newPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleTypeService.addVehicleType(_this.vehicleType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.vehicleType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.vehicleType.name + "\" Vehicle Type change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.vehicleType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.vehicleType.name);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.newPopupAcFileUploader.reset(true);
                _this.newPopup.close();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.newPopupAcFileUploader.reset(true);
            _this.newPopup.close();
            _this.showLoadingGif = false;
        });
    };
    VehicleTypeListComponent.prototype.onModify = function (vehicleType) {
        var _this = this;
        this.vehicleType = vehicleType;
        this.showLoadingGif = true;
        if (!vehicleType.modelCount && !vehicleType.baseVehicleCount && !vehicleType.vehicleCount) {
            this.vehicleTypeService.getVehicleTypeDetail(vehicleType.id).subscribe(function (m) {
                vehicleType.modelCount = m.modelCount;
                vehicleType.baseVehicleCount = m.baseVehicleCount;
                vehicleType.vehicleCount = m.vehicleCount;
                _this.modifiedVehicleType = JSON.parse(JSON.stringify(vehicleType));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedVehicleType = JSON.parse(JSON.stringify(vehicleType));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    VehicleTypeListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.vehicleType)) {
            return;
        }
        if (this.modifiedVehicleType.name == this.vehicleType.name && this.modifiedVehicleType.vehicleTypeGroupId == this.vehicleType.vehicleTypeGroupId) {
            this.toastr.info("Nothing Changed");
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedVehicleType.attachments = uploadedFiles;
            }
            if (_this.modifiedVehicleType.attachments) {
                _this.modifiedVehicleType.attachments = _this.modifiedVehicleType.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleTypeService.updateVehicleType(_this.modifiedVehicleType.id, _this.modifiedVehicleType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.vehicleType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.vehicleType.name + "\" Vehicle Type change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.vehicleTypes.filter(function (x) { return x.id == _this.modifiedVehicleType.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.vehicleType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.vehicleType.name);
                _this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
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
    VehicleTypeListComponent.prototype.onDelete = function (vehicleType) {
        var _this = this;
        this.vehicleType = vehicleType;
        this.vehicleType.comment = "";
        this.showLoadingGif = true;
        if (!vehicleType.modelCount && !vehicleType.baseVehicleCount && !vehicleType.vehicleCount) {
            this.vehicleTypeService.getVehicleTypeDetail(vehicleType.id).subscribe(function (m) {
                vehicleType.modelCount = m.modelCount;
                vehicleType.baseVehicleCount = m.baseVehicleCount;
                vehicleType.vehicleCount = m.vehicleCount;
                if (vehicleType.modelCount > 0) {
                    _this.showLoadingGif = false;
                    _this.deleteErrorPopup.open("sm");
                }
                else {
                    _this.showLoadingGif = false;
                    _this.deleteConfirmPopup.open("md");
                }
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            if (vehicleType.modelCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("sm");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    VehicleTypeListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        this.vehicleType.changeType = "Delete";
        if (!this.validationCheck(this.vehicleType)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.vehicleType.attachments = uploadedFiles;
            }
            if (_this.vehicleType.attachments) {
                _this.vehicleType.attachments = _this.vehicleType.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleTypeService.deleteVehicleTypePost(_this.vehicleType.id, _this.vehicleType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.vehicleType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.vehicleType.name + "\" Vehicle Type change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.vehicleTypes.filter(function (x) { return x.id == _this.vehicleType.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.vehicleType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.vehicleType.name);
                _this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
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
    VehicleTypeListComponent.prototype.view = function (vehicleTypeVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletype/" + vehicleTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    VehicleTypeListComponent.prototype.cleanupComponent = function () {
        return this.newPopupAcFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleTypeListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleTypeListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleTypeListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleTypeListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild('viewVehicleTypeChangeRequestModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleTypeListComponent.prototype, "viewVehicleTypeChangeRequestModal", void 0);
    __decorate([
        core_1.ViewChild("newPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleTypeListComponent.prototype, "newPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleTypeListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleTypeListComponent.prototype, "deletePopupAcFileUploader", void 0);
    VehicleTypeListComponent = __decorate([
        core_1.Component({
            selector: 'vehicleType-list-component',
            templateUrl: 'app/templates/vehicleType/vehicleType-list.component.html',
        }), 
        __metadata('design:paramtypes', [vehicleType_service_1.VehicleTypeService, vehicleTypeGroup_service_1.VehicleTypeGroupService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleTypeListComponent);
    return VehicleTypeListComponent;
}());
exports.VehicleTypeListComponent = VehicleTypeListComponent;
