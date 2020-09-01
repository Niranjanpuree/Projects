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
var bodyNumDoors_service_1 = require('./bodyNumDoors.service');
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var BodyNumDoorsListComponent = (function () {
    function BodyNumDoorsListComponent(bodyNumDoorsService, router, toastr, navigationService) {
        var _this = this;
        this.bodyNumDoorsService = bodyNumDoorsService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.filteredBodyNumDoors = [];
        this.bodyNumDoors = {};
        this.modifiedBodyNumDoors = {};
        this.bodyNumDoorsNameFilter = '';
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.bodyNumDoorsNameFilter;
            }
            else {
                _this.bodyNumDoorsNameFilter = keyword;
            }
            if (String(_this.bodyNumDoorsNameFilter) === "") {
                _this.bodyNumDoorsService.getAllBodyNumDoors().subscribe(function (sm) {
                    _this.bodyNumDoorses = sm;
                    _this.showLoadingGif = false;
                    _this.filteredBodyNumDoors = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.bodyNumDoorsService.getBodyNumDoors(_this.bodyNumDoorsNameFilter).subscribe(function (m) {
                    _this.bodyNumDoorses = m;
                    _this.showLoadingGif = false;
                    _this.filteredBodyNumDoors = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.bodyNumDoorsService.getBodyNumDoors(keyword);
        };
    }
    BodyNumDoorsListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.bodyNumDoorsService.getAllBodyNumDoors().subscribe(function (sm) {
            _this.bodyNumDoorses = sm;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    BodyNumDoorsListComponent.prototype.onSelect = function (bodyNumDoors) {
        this.bodyNumDoorsNameFilter = bodyNumDoors.numDoors;
        this.applyFilter();
        this.filteredBodyNumDoors = [];
    };
    BodyNumDoorsListComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
    };
    BodyNumDoorsListComponent.prototype.onNew = function () {
        this.bodyNumDoors = {};
        this.newPopup.open("md");
    };
    BodyNumDoorsListComponent.prototype.validationCheck = function (item) {
        var isValid = true;
        if (!item.numDoors) {
            this.toastr.warning("NumDoors is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    BodyNumDoorsListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.bodyNumDoors)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.bodyNumDoors.attachments = uploadedFiles;
            }
            if (_this.bodyNumDoors.attachments) {
                _this.bodyNumDoors.attachments = _this.bodyNumDoors.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.bodyNumDoorsService.addBodyNumDoors(_this.bodyNumDoors).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Body NumDoors", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.bodyNumDoors.numDoors);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.bodyNumDoors.numDoors + "\" Body NumDoors change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body NumDoors", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.bodyNumDoors.numDoors);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body NumDoors", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.bodyNumDoors.numDoors);
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
    BodyNumDoorsListComponent.prototype.onModify = function (bodyNumDoors) {
        var _this = this;
        this.bodyNumDoors = bodyNumDoors;
        this.showLoadingGif = true;
        if (!bodyNumDoors.bodyStyleConfigCount && !bodyNumDoors.vehicleToBodyStyleConfigCount) {
            this.bodyNumDoorsService.getBodyNumDoorsDetail(bodyNumDoors.id).subscribe(function (m) {
                bodyNumDoors.bodyStyleConfigCount = m.bodyStyleConfigCount;
                bodyNumDoors.vehicleToBodyStyleConfigCount = m.vehicleToBodyStyleConfigCount;
                _this.modifiedBodyNumDoors = JSON.parse(JSON.stringify(bodyNumDoors));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedBodyNumDoors = JSON.parse(JSON.stringify(bodyNumDoors));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    BodyNumDoorsListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedBodyNumDoors)) {
            return;
        }
        else if (this.modifiedBodyNumDoors.numDoors == this.bodyNumDoors.numDoors) {
            this.toastr.warning("Nothing changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedBodyNumDoors.attachments = uploadedFiles;
            }
            if (_this.modifiedBodyNumDoors.attachments) {
                _this.modifiedBodyNumDoors.attachments = _this.modifiedBodyNumDoors.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.bodyNumDoorsService.updateBodyNumDoors(_this.modifiedBodyNumDoors.id, _this.modifiedBodyNumDoors).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Body NumDoors", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.bodyNumDoors.numDoors);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.bodyNumDoors.numDoors + "\" Body NumDoors change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.bodyNumDoorses.filter(function (x) { return x.id == _this.modifiedBodyNumDoors.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body NumDoors", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.bodyNumDoors.numDoors);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body NumDoors", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.bodyNumDoors.numDoors);
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
    BodyNumDoorsListComponent.prototype.onDelete = function (bodyNumDoors) {
        var _this = this;
        this.bodyNumDoors = bodyNumDoors;
        this.showLoadingGif = true;
        if (!bodyNumDoors.bodyStyleConfigCount && !bodyNumDoors.vehicleToBodyStyleConfigCount) {
            this.bodyNumDoorsService.getBodyNumDoorsDetail(bodyNumDoors.id).subscribe(function (m) {
                bodyNumDoors.bodyStyleConfigCount = m.bodyStyleConfigCount;
                bodyNumDoors.vehicleToBodyStyleConfigCount = m.vehicleToBodyStyleConfigCount;
                if (bodyNumDoors.bodyStyleConfigCount > 0) {
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
            if (bodyNumDoors.bodyStyleConfigCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    BodyNumDoorsListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        if (!this.validationCheck(this.bodyNumDoors)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.bodyNumDoors.attachments = uploadedFiles;
            }
            if (_this.bodyNumDoors.attachments) {
                _this.bodyNumDoors.attachments = _this.bodyNumDoors.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.bodyNumDoorsService.deleteBodyNumDoors(_this.bodyNumDoors.id, _this.bodyNumDoors).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Body NumDoors", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.bodyNumDoors.numDoors);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.bodyNumDoors.numDoors + "\" Body NumDoors change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.bodyNumDoorses.filter(function (x) { return x.id == _this.bodyNumDoors.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body NumDoors", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.bodyNumDoors.numDoors);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body NumDoors", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.bodyNumDoors.numDoors);
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
    BodyNumDoorsListComponent.prototype.view = function (bodyNumDoorsVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/bodynumdoors/" + bodyNumDoorsVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    BodyNumDoorsListComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BodyNumDoorsListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BodyNumDoorsListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BodyNumDoorsListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BodyNumDoorsListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BodyNumDoorsListComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BodyNumDoorsListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BodyNumDoorsListComponent.prototype, "deletePopupAcFileUploader", void 0);
    BodyNumDoorsListComponent = __decorate([
        core_1.Component({
            selector: 'bodyNumDoors-list-component',
            templateUrl: 'app/templates/bodyNumDoors/bodyNumDoors-list.component.html',
        }), 
        __metadata('design:paramtypes', [bodyNumDoors_service_1.BodyNumDoorsService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], BodyNumDoorsListComponent);
    return BodyNumDoorsListComponent;
}());
exports.BodyNumDoorsListComponent = BodyNumDoorsListComponent;
