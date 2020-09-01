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
var engineDesignation_service_1 = require('./engineDesignation.service');
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var EngineDesignationListComponent = (function () {
    function EngineDesignationListComponent(engineDesignationService, router, toastr, navigationService) {
        var _this = this;
        this.engineDesignationService = engineDesignationService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.filteredEngineDesignations = [];
        this.engineDesignation = {};
        this.modifiedEngineDesignation = {};
        this.engineDesignationNameFilter = '';
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.engineDesignationNameFilter;
            }
            else {
                _this.engineDesignationNameFilter = keyword;
            }
            if (String(_this.engineDesignationNameFilter) === "") {
                _this.engineDesignationService.getAllEngineDesignations().subscribe(function (sm) {
                    _this.engineDesignations = sm;
                    _this.showLoadingGif = false;
                    _this.filteredEngineDesignations = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.engineDesignationService.getEngineDesignations(_this.engineDesignationNameFilter).subscribe(function (m) {
                    _this.engineDesignations = m;
                    _this.showLoadingGif = false;
                    _this.filteredEngineDesignations = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.engineDesignationService.getEngineDesignations(keyword);
        };
    }
    EngineDesignationListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.engineDesignationService.getAllEngineDesignations().subscribe(function (sm) {
            _this.engineDesignations = sm;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    EngineDesignationListComponent.prototype.onSelect = function (engineDesignation) {
        this.engineDesignationNameFilter = engineDesignation.engineDesignationName;
        this.applyFilter();
        this.filteredEngineDesignations = [];
    };
    EngineDesignationListComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
    };
    EngineDesignationListComponent.prototype.onNew = function () {
        this.engineDesignation = {};
        this.newPopup.open("md");
    };
    EngineDesignationListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.engineDesignation)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.engineDesignation.attachments = uploadedFiles;
            }
            if (_this.engineDesignation.attachments) {
                _this.engineDesignation.attachments = _this.engineDesignation.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.engineDesignationService.addEngineDesignation(_this.engineDesignation).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Engine  Designation", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.engineDesignation.engineDesignationName);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.engineDesignation.engineDesignationName + "\" Engine  Designation change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Designation", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.engineDesignation.engineDesignationName);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Designation", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.engineDesignation.engineDesignationName);
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
    EngineDesignationListComponent.prototype.validationCheck = function (item) {
        var isValid = true;
        if (!item.engineDesignationName) {
            this.toastr.warning("EngineDesignationName is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    EngineDesignationListComponent.prototype.onModify = function (engineDesignation) {
        var _this = this;
        debugger;
        this.engineDesignation = engineDesignation;
        this.showLoadingGif = true;
        if (!engineDesignation.engineConfigCount && !engineDesignation.vehicleToEngineConfigCount) {
            this.engineDesignationService.getEngineDesignationDetail(engineDesignation.engineDesignationId).subscribe(function (m) {
                engineDesignation.engineConfigCount = m.engineConfigCount;
                engineDesignation.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;
                _this.modifiedEngineDesignation = JSON.parse(JSON.stringify(engineDesignation));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedEngineDesignation = JSON.parse(JSON.stringify(engineDesignation));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    EngineDesignationListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedEngineDesignation)) {
            return;
        }
        else if (this.modifiedEngineDesignation.engineDesignationName == this.engineDesignation.engineDesignationName) {
            this.toastr.warning("Nothing changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedEngineDesignation.attachments = uploadedFiles;
            }
            if (_this.modifiedEngineDesignation.attachments) {
                _this.modifiedEngineDesignation.attachments = _this.modifiedEngineDesignation.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.engineDesignationService.updateEngineDesignation(_this.modifiedEngineDesignation.engineDesignationId, _this.modifiedEngineDesignation).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Engine Designation", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.engineDesignation.engineDesignationName);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.engineDesignation.engineDesignationName + "\" Engine Designation change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.engineDesignations.filter(function (x) { return x.engineDesignationId == _this.modifiedEngineDesignation.engineDesignationId; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Designation", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.engineDesignation.engineDesignationName);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Designation", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.engineDesignation.engineDesignationName);
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
    EngineDesignationListComponent.prototype.onDelete = function (engineDesignation) {
        var _this = this;
        this.engineDesignation = engineDesignation;
        this.showLoadingGif = true;
        if (!engineDesignation.engineConfigCount && !engineDesignation.vehicleToEngineConfigCount) {
            this.engineDesignationService.getEngineDesignationDetail(engineDesignation.engineDesignationId).subscribe(function (m) {
                engineDesignation.engineConfigCount = m.engineConfigCount;
                engineDesignation.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;
                if (engineDesignation.engineConfigCount > 0) {
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
            if (engineDesignation.engineConfigCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    EngineDesignationListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        if (!this.validationCheck(this.engineDesignation)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.engineDesignation.attachments = uploadedFiles;
            }
            if (_this.engineDesignation.attachments) {
                _this.engineDesignation.attachments = _this.engineDesignation.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.engineDesignationService.deleteEngineDesignation(_this.engineDesignation.engineDesignationId, _this.engineDesignation).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Engine Designation", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.engineDesignation.engineDesignationName);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.engineDesignation.engineDesignationName + "\" EngineDesignation change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.engineDesignations.filter(function (x) { return x.engineDesignationId == _this.engineDesignation.engineDesignationId; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Designation", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.engineDesignation.engineDesignationName);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Engine Designation", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.engineDesignation.engineDesignationName);
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
    EngineDesignationListComponent.prototype.view = function (engineDesignationVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/enginedesignation/" + engineDesignationVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    EngineDesignationListComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], EngineDesignationListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], EngineDesignationListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], EngineDesignationListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], EngineDesignationListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], EngineDesignationListComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], EngineDesignationListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], EngineDesignationListComponent.prototype, "deletePopupAcFileUploader", void 0);
    EngineDesignationListComponent = __decorate([
        core_1.Component({
            selector: 'engineDesignation-list-component',
            templateUrl: 'app/templates/engineDesignation/engineDesignation-list.component.html',
        }), 
        __metadata('design:paramtypes', [engineDesignation_service_1.EngineDesignationService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], EngineDesignationListComponent);
    return EngineDesignationListComponent;
}());
exports.EngineDesignationListComponent = EngineDesignationListComponent;
