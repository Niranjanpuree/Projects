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
var engineVersion_service_1 = require('./engineVersion.service');
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var EngineVersionListComponent = (function () {
    function EngineVersionListComponent(engineVersionService, router, toastr, navigationService) {
        var _this = this;
        this.engineVersionService = engineVersionService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.filteredEngineVersions = [];
        this.engineVersion = {};
        this.modifiedEngineVersion = {};
        this.engineVersionNameFilter = '';
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.engineVersionNameFilter;
            }
            else {
                _this.engineVersionNameFilter = keyword;
            }
            if (String(_this.engineVersionNameFilter) === "") {
                _this.engineVersionService.getAllEngineVersions().subscribe(function (sm) {
                    _this.engineVersions = sm;
                    _this.showLoadingGif = false;
                    _this.filteredEngineVersions = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.engineVersionService.getEngineVersions(_this.engineVersionNameFilter).subscribe(function (m) {
                    _this.engineVersions = m;
                    _this.showLoadingGif = false;
                    _this.filteredEngineVersions = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.engineVersionService.getEngineVersions(keyword);
        };
    }
    EngineVersionListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.engineVersionService.getAllEngineVersions().subscribe(function (sm) {
            _this.engineVersions = sm;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    EngineVersionListComponent.prototype.onSelect = function (engineVersion) {
        this.engineVersionNameFilter = engineVersion.engineVersionName;
        this.applyFilter();
        this.filteredEngineVersions = [];
    };
    EngineVersionListComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
    };
    EngineVersionListComponent.prototype.onNew = function () {
        this.engineVersion = {};
        this.newPopup.open("md");
    };
    EngineVersionListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.engineVersion)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.engineVersion.attachments = uploadedFiles;
            }
            if (_this.engineVersion.attachments) {
                _this.engineVersion.attachments = _this.engineVersion.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.engineVersionService.addEngineVersion(_this.engineVersion).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Engine  Version", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.engineVersion.engineVersionName);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.engineVersion.engineVersionName + "\" Engine  Version change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Version", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.engineVersion.engineVersionName);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Version", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.engineVersion.engineVersionName);
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
    EngineVersionListComponent.prototype.validationCheck = function (item) {
        var isValid = true;
        if (!item.engineVersionName) {
            this.toastr.warning("EngineVersionName is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    EngineVersionListComponent.prototype.onModify = function (engineVersion) {
        var _this = this;
        debugger;
        this.engineVersion = engineVersion;
        this.showLoadingGif = true;
        if (!engineVersion.engineConfigCount && !engineVersion.vehicleToEngineConfigCount) {
            this.engineVersionService.getEngineVersionDetail(engineVersion.engineVersionId).subscribe(function (m) {
                engineVersion.engineConfigCount = m.engineConfigCount;
                engineVersion.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;
                _this.modifiedEngineVersion = JSON.parse(JSON.stringify(engineVersion));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedEngineVersion = JSON.parse(JSON.stringify(engineVersion));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    EngineVersionListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedEngineVersion)) {
            return;
        }
        else if (this.modifiedEngineVersion.engineVersionName == this.engineVersion.engineVersionName) {
            this.toastr.warning("Nothing changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedEngineVersion.attachments = uploadedFiles;
            }
            if (_this.modifiedEngineVersion.attachments) {
                _this.modifiedEngineVersion.attachments = _this.modifiedEngineVersion.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.engineVersionService.updateEngineVersion(_this.modifiedEngineVersion.engineVersionId, _this.modifiedEngineVersion).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Engine Version", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.engineVersion.engineVersionName);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.engineVersion.engineVersionName + "\" Engine Version change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.engineVersions.filter(function (x) { return x.engineVersionId == _this.modifiedEngineVersion.engineVersionId; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Version", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.engineVersion.engineVersionName);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Version", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.engineVersion.engineVersionName);
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
    EngineVersionListComponent.prototype.onDelete = function (engineVersion) {
        var _this = this;
        this.engineVersion = engineVersion;
        this.showLoadingGif = true;
        if (!engineVersion.engineConfigCount && !engineVersion.vehicleToEngineConfigCount) {
            this.engineVersionService.getEngineVersionDetail(engineVersion.engineVersionId).subscribe(function (m) {
                engineVersion.engineConfigCount = m.engineConfigCount;
                engineVersion.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;
                if (engineVersion.engineConfigCount > 0) {
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
            if (engineVersion.engineConfigCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    EngineVersionListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        if (!this.validationCheck(this.engineVersion)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.engineVersion.attachments = uploadedFiles;
            }
            if (_this.engineVersion.attachments) {
                _this.engineVersion.attachments = _this.engineVersion.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.engineVersionService.deleteEngineVersion(_this.engineVersion.engineVersionId, _this.engineVersion).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Engine Version", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.engineVersion.engineVersionName);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.engineVersion.engineVersionName + "\" EngineVersion change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.engineVersions.filter(function (x) { return x.engineVersionId == _this.engineVersion.engineVersionId; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Version", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.engineVersion.engineVersionName);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Version", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.engineVersion.engineVersionName);
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
    EngineVersionListComponent.prototype.view = function (engineVersionVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/engineversion/" + engineVersionVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    EngineVersionListComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], EngineVersionListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], EngineVersionListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], EngineVersionListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], EngineVersionListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], EngineVersionListComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], EngineVersionListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], EngineVersionListComponent.prototype, "deletePopupAcFileUploader", void 0);
    EngineVersionListComponent = __decorate([
        core_1.Component({
            selector: 'engineVersion-list-component',
            templateUrl: 'app/templates/engineVersion/engineVersion-list.component.html',
        }), 
        __metadata('design:paramtypes', [engineVersion_service_1.EngineVersionService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], EngineVersionListComponent);
    return EngineVersionListComponent;
}());
exports.EngineVersionListComponent = EngineVersionListComponent;
