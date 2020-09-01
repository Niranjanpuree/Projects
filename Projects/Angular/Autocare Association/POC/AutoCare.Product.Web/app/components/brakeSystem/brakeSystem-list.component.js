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
var brakeSystem_service_1 = require('./brakeSystem.service');
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var BrakeSystemListComponent = (function () {
    function BrakeSystemListComponent(brakeSystemService, router, toastr, navigationService) {
        var _this = this;
        this.brakeSystemService = brakeSystemService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.filteredBrakeSystems = [];
        this.brakeSystem = {};
        this.modifiedBrakeSystem = {};
        this.brakeSystemNameFilter = '';
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.brakeSystemNameFilter;
            }
            else {
                _this.brakeSystemNameFilter = keyword;
            }
            if ((_this.brakeSystemNameFilter) === "") {
                _this.brakeSystemService.getAllBrakeSystems().subscribe(function (sm) {
                    _this.brakeSystems = sm;
                    _this.showLoadingGif = false;
                    _this.filteredBrakeSystems = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.brakeSystemService.getBrakeSystems(_this.brakeSystemNameFilter).subscribe(function (m) {
                    _this.brakeSystems = m;
                    _this.showLoadingGif = false;
                    _this.filteredBrakeSystems = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.brakeSystemService.getBrakeSystems(keyword);
        };
    }
    BrakeSystemListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.brakeSystemService.getAllBrakeSystems().subscribe(function (sm) {
            _this.brakeSystems = sm;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    BrakeSystemListComponent.prototype.validationCheck = function (item) {
        var isValid = true;
        if (!item.name) {
            this.toastr.warning("Name is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    BrakeSystemListComponent.prototype.onSelect = function (brakeSystem) {
        this.brakeSystemNameFilter = brakeSystem.name;
        this.applyFilter();
        this.filteredBrakeSystems = [];
    };
    BrakeSystemListComponent.prototype.onNew = function () {
        this.brakeSystem = {};
        this.newPopup.open("md");
    };
    BrakeSystemListComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
    };
    BrakeSystemListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.brakeSystem)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.brakeSystem.attachments = uploadedFiles;
            }
            if (_this.brakeSystem.attachments) {
                _this.brakeSystem.attachments = _this.brakeSystem.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.brakeSystemService.addBrakeSystem(_this.brakeSystem).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.brakeSystem.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.brakeSystem.name + "\" BrakeSystem change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.brakeSystem.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.brakeSystem.name);
                _this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.acFileUploader.reset(true);
                _this.newPopup.close();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.acFileUploader.reset(true);
            _this.newPopup.close();
            _this.showLoadingGif = false;
        });
    };
    BrakeSystemListComponent.prototype.onModify = function (brakeSystem) {
        var _this = this;
        this.brakeSystem = brakeSystem;
        this.showLoadingGif = true;
        if (!brakeSystem.brakeConfigCount && !brakeSystem.vehicleToBrakeConfigCount) {
            this.brakeSystemService.getBrakeSystemDetail(brakeSystem.id).subscribe(function (m) {
                brakeSystem.brakeConfigCount = m.brakeConfigCount;
                brakeSystem.vehicleToBrakeConfigCount = m.vehicleToBrakeConfigCount;
                _this.modifiedBrakeSystem = JSON.parse(JSON.stringify(brakeSystem));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedBrakeSystem = JSON.parse(JSON.stringify(brakeSystem));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    BrakeSystemListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedBrakeSystem)) {
            return;
        }
        else if (this.modifiedBrakeSystem.name == this.brakeSystem.name) {
            this.toastr.warning("Nothing changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedBrakeSystem.attachments = uploadedFiles;
            }
            if (_this.modifiedBrakeSystem.attachments) {
                _this.modifiedBrakeSystem.attachments = _this.modifiedBrakeSystem.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.brakeSystemService.updateBrakeSystem(_this.modifiedBrakeSystem.id, _this.modifiedBrakeSystem).subscribe(function (response) {
                if (response) {
                    _this.newPopup.close();
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.brakeSystem.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.brakeSystem.name + "\" BrakeSystem change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.brakeSystems.filter(function (x) { return x.id == _this.modifiedBrakeSystem.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.brakeSystem.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.brakeSystem.name);
                _this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.modifyPopupAcFileUploader.reset(true);
                _this.modifyPopup.close();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.modifyPopupAcFileUploader.reset(true);
            _this.modifyPopup.close();
            _this.showLoadingGif = false;
        });
    };
    BrakeSystemListComponent.prototype.onDelete = function (brakeSystem) {
        var _this = this;
        this.brakeSystem = brakeSystem;
        this.showLoadingGif = true;
        if (!brakeSystem.brakeConfigCount && !brakeSystem.vehicleToBrakeConfigCount) {
            this.brakeSystemService.getBrakeSystemDetail(brakeSystem.id).subscribe(function (m) {
                brakeSystem.brakeConfigCount = m.brakeConfigCount;
                brakeSystem.vehicleToBrakeConfigCount = m.vehicleToBrakeConfigCount;
                if (brakeSystem.brakeConfigCount > 0) {
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
            if (brakeSystem.brakeConfigCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    BrakeSystemListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        if (!this.validationCheck(this.brakeSystem)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.brakeSystem.attachments = uploadedFiles;
            }
            if (_this.brakeSystem.attachments) {
                _this.brakeSystem.attachments = _this.brakeSystem.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.brakeSystemService.deleteBrakeSystem(_this.brakeSystem.id, _this.brakeSystem).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.brakeSystem.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.brakeSystem.name + "\" BrakeSystem change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.brakeSystems.filter(function (x) { return x.id == _this.brakeSystem.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.brakeSystem.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake System", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.brakeSystem.name);
                _this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.deletePopupAcFileUploader.reset(true);
                _this.deleteConfirmPopup.close();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.deletePopupAcFileUploader.reset(true);
            _this.deleteConfirmPopup.close();
            _this.showLoadingGif = false;
        });
    };
    BrakeSystemListComponent.prototype.view = function (brakeSystemVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/brakesystem/" + brakeSystemVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    BrakeSystemListComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BrakeSystemListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BrakeSystemListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BrakeSystemListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BrakeSystemListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BrakeSystemListComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BrakeSystemListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BrakeSystemListComponent.prototype, "deletePopupAcFileUploader", void 0);
    BrakeSystemListComponent = __decorate([
        core_1.Component({
            selector: 'brakeSystem-list-component',
            templateUrl: 'app/templates/brakeSystem/brakeSystem-list.component.html',
        }), 
        __metadata('design:paramtypes', [brakeSystem_service_1.BrakeSystemService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], BrakeSystemListComponent);
    return BrakeSystemListComponent;
}());
exports.BrakeSystemListComponent = BrakeSystemListComponent;
