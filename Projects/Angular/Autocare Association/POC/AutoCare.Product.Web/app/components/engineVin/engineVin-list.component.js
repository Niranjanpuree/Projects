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
var engineVin_service_1 = require('./engineVin.service');
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var EngineVinListComponent = (function () {
    function EngineVinListComponent(engineVinService, router, toastr, navigationService) {
        var _this = this;
        this.engineVinService = engineVinService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.filteredEngineVins = [];
        this.engineVin = {};
        this.modifiedEngineVin = {};
        this.engineVinNameFilter = '';
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.engineVinNameFilter;
            }
            else {
                _this.engineVinNameFilter = keyword;
            }
            if (String(_this.engineVinNameFilter) === "") {
                _this.engineVinService.getAllEngineVins().subscribe(function (sm) {
                    _this.engineVins = sm;
                    _this.showLoadingGif = false;
                    _this.filteredEngineVins = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.engineVinService.getEngineVins(_this.engineVinNameFilter).subscribe(function (m) {
                    _this.engineVins = m;
                    _this.showLoadingGif = false;
                    _this.filteredEngineVins = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.engineVinService.getEngineVins(keyword);
        };
    }
    EngineVinListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.engineVinService.getAllEngineVins().subscribe(function (sm) {
            _this.engineVins = sm;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    EngineVinListComponent.prototype.onSelect = function (engineVin) {
        this.engineVinNameFilter = engineVin.engineVinName;
        this.applyFilter();
        this.filteredEngineVins = [];
    };
    EngineVinListComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
    };
    EngineVinListComponent.prototype.onNew = function () {
        this.engineVin = {};
        this.newPopup.open("md");
    };
    EngineVinListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.engineVin)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.engineVin.attachments = uploadedFiles;
            }
            if (_this.engineVin.attachments) {
                _this.engineVin.attachments = _this.engineVin.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.engineVinService.addEngineVin(_this.engineVin).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Engine  Vin", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.engineVin.engineVinName);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.engineVin.engineVinName + "\" Engine  Vin change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Vin", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.engineVin.engineVinName);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Vin", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.engineVin.engineVinName);
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
    EngineVinListComponent.prototype.validationCheck = function (item) {
        var isValid = true;
        if (!item.engineVinName) {
            this.toastr.warning("EngineVinName is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    EngineVinListComponent.prototype.onModify = function (engineVin) {
        var _this = this;
        debugger;
        this.engineVin = engineVin;
        this.showLoadingGif = true;
        if (!engineVin.engineConfigCount && !engineVin.vehicleToEngineConfigCount) {
            this.engineVinService.getEngineVinDetail(engineVin.engineVinId).subscribe(function (m) {
                engineVin.engineConfigCount = m.engineConfigCount;
                engineVin.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;
                _this.modifiedEngineVin = JSON.parse(JSON.stringify(engineVin));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedEngineVin = JSON.parse(JSON.stringify(engineVin));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    EngineVinListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedEngineVin)) {
            return;
        }
        else if (this.modifiedEngineVin.engineVinName == this.engineVin.engineVinName) {
            this.toastr.warning("Nothing changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedEngineVin.attachments = uploadedFiles;
            }
            if (_this.modifiedEngineVin.attachments) {
                _this.modifiedEngineVin.attachments = _this.modifiedEngineVin.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.engineVinService.updateEngineVin(_this.modifiedEngineVin.engineVinId, _this.modifiedEngineVin).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Engine Vin", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.engineVin.engineVinName);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.engineVin.engineVinName + "\" Engine Vin change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.engineVins.filter(function (x) { return x.engineVinId == _this.modifiedEngineVin.engineVinId; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Vin", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.engineVin.engineVinName);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Vin", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.engineVin.engineVinName);
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
    EngineVinListComponent.prototype.onDelete = function (engineVin) {
        var _this = this;
        this.engineVin = engineVin;
        this.showLoadingGif = true;
        if (!engineVin.engineConfigCount && !engineVin.vehicleToEngineConfigCount) {
            this.engineVinService.getEngineVinDetail(engineVin.engineVinId).subscribe(function (m) {
                engineVin.engineConfigCount = m.engineConfigCount;
                engineVin.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;
                if (engineVin.engineConfigCount > 0) {
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
            if (engineVin.engineConfigCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    EngineVinListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        if (!this.validationCheck(this.engineVin)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.engineVin.attachments = uploadedFiles;
            }
            if (_this.engineVin.attachments) {
                _this.engineVin.attachments = _this.engineVin.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.engineVinService.deleteEngineVin(_this.engineVin.engineVinId, _this.engineVin).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Engine Vin", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.engineVin.engineVinName);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.engineVin.engineVinName + "\" EngineVin change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.engineVins.filter(function (x) { return x.engineVinId == _this.engineVin.engineVinId; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Vin", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.engineVin.engineVinName);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Vin", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.engineVin.engineVinName);
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
    EngineVinListComponent.prototype.view = function (engineVinVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/enginevin/" + engineVinVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    EngineVinListComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], EngineVinListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], EngineVinListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], EngineVinListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], EngineVinListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], EngineVinListComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], EngineVinListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], EngineVinListComponent.prototype, "deletePopupAcFileUploader", void 0);
    EngineVinListComponent = __decorate([
        core_1.Component({
            selector: 'engineVin-list-component',
            templateUrl: 'app/templates/engineVin/engineVin-list.component.html',
        }), 
        __metadata('design:paramtypes', [engineVin_service_1.EngineVinService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], EngineVinListComponent);
    return EngineVinListComponent;
}());
exports.EngineVinListComponent = EngineVinListComponent;
