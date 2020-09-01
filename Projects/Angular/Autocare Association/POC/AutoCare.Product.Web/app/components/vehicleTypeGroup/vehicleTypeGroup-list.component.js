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
var vehicleTypeGroup_service_1 = require('./vehicleTypeGroup.service');
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var VehicleTypeGroupListComponent = (function () {
    function VehicleTypeGroupListComponent(vehicleTypeGroupService, router, toastr, navigationService) {
        var _this = this;
        this.vehicleTypeGroupService = vehicleTypeGroupService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.filteredVehicleTypeGroups = [];
        this.vehicleTypeGroup = {};
        this.modifiedVehicleTypeGroup = {};
        this.vehicleTypeGroupNameFilter = '';
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.vehicleTypeGroupNameFilter;
            }
            else {
                _this.vehicleTypeGroupNameFilter = keyword;
            }
            if (String(_this.vehicleTypeGroupNameFilter) === "") {
                _this.vehicleTypeGroupService.getAllVehicleTypeGroups().subscribe(function (sm) {
                    _this.vehicleTypeGroups = sm;
                    _this.showLoadingGif = false;
                    _this.filteredVehicleTypeGroups = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.vehicleTypeGroupService.getVehicleTypeGroups(_this.vehicleTypeGroupNameFilter).subscribe(function (m) {
                    _this.vehicleTypeGroups = m;
                    _this.showLoadingGif = false;
                    _this.filteredVehicleTypeGroups = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.vehicleTypeGroupService.getVehicleTypeGroups(keyword);
        };
    }
    VehicleTypeGroupListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.vehicleTypeGroupService.getAllVehicleTypeGroups().subscribe(function (sm) {
            _this.vehicleTypeGroups = sm;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    VehicleTypeGroupListComponent.prototype.onSelect = function (vehicleTypeGroup) {
        this.vehicleTypeGroupNameFilter = vehicleTypeGroup.name;
        this.applyFilter();
        this.filteredVehicleTypeGroups = [];
    };
    VehicleTypeGroupListComponent.prototype.validationCheck = function (item) {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("Vehicle Type Group Name is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
        }
        return true;
    };
    VehicleTypeGroupListComponent.prototype.onCancel = function (action) {
        this.newPopupAcFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
    };
    VehicleTypeGroupListComponent.prototype.onNew = function () {
        this.vehicleTypeGroup = {};
        this.newPopup.open("md");
    };
    VehicleTypeGroupListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.vehicleTypeGroup)) {
            return;
        }
        this.showLoadingGif = true;
        this.newPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.vehicleTypeGroup.attachments = uploadedFiles;
            }
            if (_this.vehicleTypeGroup.attachments) {
                _this.vehicleTypeGroup.attachments = _this.vehicleTypeGroup.attachments.concat(_this.newPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleTypeGroupService.addVehicleTypeGroup(_this.vehicleTypeGroup).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle Type Group", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.vehicleTypeGroup.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.vehicleTypeGroup.name + "\" Vehicle Type Group change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle Type Group", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.vehicleTypeGroup.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle Type Group", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.vehicleTypeGroup.name);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.showLoadingGif = false;
                _this.newPopupAcFileUploader.reset(true);
                _this.newPopup.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.newPopupAcFileUploader.reset(true);
            _this.newPopup.close();
        });
    };
    VehicleTypeGroupListComponent.prototype.onModify = function (vehicleTypeGroup) {
        var _this = this;
        this.vehicleTypeGroup = vehicleTypeGroup;
        this.showLoadingGif = true;
        if (!vehicleTypeGroup.vehicleTypeCount) {
            this.vehicleTypeGroupService.getVehicleTypeGroupDetail(vehicleTypeGroup.id).subscribe(function (m) {
                vehicleTypeGroup.vehicleTypeCount = m.vehicleTypeCount;
                _this.modifiedVehicleTypeGroup = JSON.parse(JSON.stringify(vehicleTypeGroup));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedVehicleTypeGroup = JSON.parse(JSON.stringify(vehicleTypeGroup));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    VehicleTypeGroupListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedVehicleTypeGroup)) {
            return;
        }
        if (this.modifiedVehicleTypeGroup.name == this.vehicleTypeGroup.name) {
            this.toastr.warning("Nothing Changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedVehicleTypeGroup.attachments = uploadedFiles;
            }
            if (_this.modifiedVehicleTypeGroup.attachments) {
                _this.modifiedVehicleTypeGroup.attachments = _this.modifiedVehicleTypeGroup.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleTypeGroupService.updateVehicleTypeGroup(_this.modifiedVehicleTypeGroup.id, _this.modifiedVehicleTypeGroup).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle Type Group", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.vehicleTypeGroup.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.vehicleTypeGroup.name + "\" Vehicle Type Group change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.vehicleTypeGroups.filter(function (x) { return x.id == _this.modifiedVehicleTypeGroup.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle Type Group", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.vehicleTypeGroup.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle Type Group", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.vehicleTypeGroup.name);
                _this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
                _this.showLoadingGif = false;
                _this.modifyPopupAcFileUploader.reset(true);
                _this.modifyPopup.close();
            });
        }, function (error) {
            _this.modifyPopupAcFileUploader.reset(true);
            _this.modifyPopup.close();
            _this.showLoadingGif = false;
        });
    };
    VehicleTypeGroupListComponent.prototype.onDelete = function (vehicleTypeGroup) {
        var _this = this;
        this.vehicleTypeGroup = vehicleTypeGroup;
        this.vehicleTypeGroup.comment = "";
        this.showLoadingGif = true;
        if (!vehicleTypeGroup.vehicleTypeCount) {
            this.vehicleTypeGroupService.getVehicleTypeGroupDetail(vehicleTypeGroup.id).subscribe(function (m) {
                vehicleTypeGroup.vehicleTypeCount = m.vehicleTypeCount;
                if (vehicleTypeGroup.vehicleTypeCount > 0) {
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
            if (vehicleTypeGroup.vehicleTypeCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("sm");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    VehicleTypeGroupListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        this.vehicleTypeGroup.changeType = "Delete";
        if (!this.validationCheck(this.vehicleTypeGroup)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.vehicleTypeGroup.attachments = uploadedFiles;
            }
            if (_this.vehicleTypeGroup.attachments) {
                _this.vehicleTypeGroup.attachments = _this.vehicleTypeGroup.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleTypeGroupService.deleteVehicleTypeGroupPost(_this.vehicleTypeGroup.id, _this.vehicleTypeGroup).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Vehicle Type Group", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.vehicleTypeGroup.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.vehicleTypeGroup.name + "\" Vehicle Type Group change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.vehicleTypeGroups.filter(function (x) { return x.id == _this.vehicleTypeGroup.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle Type Group", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.vehicleTypeGroup.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Vehicle Type Group", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.vehicleTypeGroup.name);
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
    VehicleTypeGroupListComponent.prototype.view = function (vehicleTypeGroupVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletypegroup/" + vehicleTypeGroupVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    VehicleTypeGroupListComponent.prototype.cleanupComponent = function () {
        return this.newPopupAcFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleTypeGroupListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleTypeGroupListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleTypeGroupListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleTypeGroupListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild("newPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleTypeGroupListComponent.prototype, "newPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleTypeGroupListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleTypeGroupListComponent.prototype, "deletePopupAcFileUploader", void 0);
    VehicleTypeGroupListComponent = __decorate([
        core_1.Component({
            selector: 'vehicleTypeGroup-list-component',
            templateUrl: 'app/templates/vehicleTypeGroup/vehicleTypeGroup-list.component.html',
        }), 
        __metadata('design:paramtypes', [vehicleTypeGroup_service_1.VehicleTypeGroupService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleTypeGroupListComponent);
    return VehicleTypeGroupListComponent;
}());
exports.VehicleTypeGroupListComponent = VehicleTypeGroupListComponent;
