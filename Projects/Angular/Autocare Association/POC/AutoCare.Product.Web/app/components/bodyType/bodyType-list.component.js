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
var bodyType_service_1 = require('./bodyType.service');
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var BodyTypeListComponent = (function () {
    function BodyTypeListComponent(bodyTypeService, router, toastr, navigationService) {
        var _this = this;
        this.bodyTypeService = bodyTypeService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.filteredBodyTypes = [];
        this.bodyType = {};
        this.modifiedBodyType = {};
        this.bodyTypeNameFilter = '';
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.bodyTypeNameFilter;
            }
            else {
                _this.bodyTypeNameFilter = keyword;
            }
            if (String(_this.bodyTypeNameFilter) === "") {
                _this.bodyTypeService.getAllBodyTypes().subscribe(function (sm) {
                    _this.bodyTypes = sm;
                    _this.showLoadingGif = false;
                    _this.filteredBodyTypes = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.bodyTypeService.getBodyTypes(_this.bodyTypeNameFilter).subscribe(function (m) {
                    _this.bodyTypes = m;
                    _this.showLoadingGif = false;
                    _this.filteredBodyTypes = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.bodyTypeService.getBodyTypes(keyword);
        };
    }
    BodyTypeListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.bodyTypeService.getAllBodyTypes().subscribe(function (sm) {
            _this.bodyTypes = sm;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    BodyTypeListComponent.prototype.onSelect = function (bodyType) {
        this.bodyTypeNameFilter = bodyType.name;
        this.applyFilter();
        this.filteredBodyTypes = [];
    };
    BodyTypeListComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
    };
    BodyTypeListComponent.prototype.onNew = function () {
        this.bodyType = {};
        this.newPopup.open("md");
    };
    BodyTypeListComponent.prototype.validationCheck = function (item) {
        var isValid = true;
        if (!item.name) {
            this.toastr.warning("Name is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    BodyTypeListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.bodyType)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.bodyType.attachments = uploadedFiles;
            }
            if (_this.bodyType.attachments) {
                _this.bodyType.attachments = _this.bodyType.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.bodyTypeService.addBodyType(_this.bodyType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Body Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.bodyType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.bodyType.name + "\" Body Type change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.bodyType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.bodyType.name);
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
    BodyTypeListComponent.prototype.onModify = function (bodyType) {
        var _this = this;
        this.bodyType = bodyType;
        this.showLoadingGif = true;
        if (!bodyType.bodyStyleConfigCount && !bodyType.vehicleToBodyStyleConfigCount) {
            this.bodyTypeService.getBodyTypeDetail(bodyType.id).subscribe(function (m) {
                bodyType.bodyStyleConfigCount = m.bodyStyleConfigCount;
                bodyType.vehicleToBodyStyleConfigCount = m.vehicleToBodyStyleConfigCount;
                _this.modifiedBodyType = JSON.parse(JSON.stringify(bodyType));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedBodyType = JSON.parse(JSON.stringify(bodyType));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    BodyTypeListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedBodyType)) {
            return;
        }
        else if (this.modifiedBodyType.name == this.bodyType.name) {
            this.toastr.warning("Nothing changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedBodyType.attachments = uploadedFiles;
            }
            if (_this.modifiedBodyType.attachments) {
                _this.modifiedBodyType.attachments = _this.modifiedBodyType.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.bodyTypeService.updateBodyType(_this.modifiedBodyType.id, _this.modifiedBodyType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Body Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.bodyType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.bodyType.name + "\" Body Type change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.bodyTypes.filter(function (x) { return x.id == _this.modifiedBodyType.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.bodyType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.bodyType.name);
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
    BodyTypeListComponent.prototype.onDelete = function (bodyType) {
        var _this = this;
        this.bodyType = bodyType;
        this.showLoadingGif = true;
        if (!bodyType.bodyStyleConfigCount && !bodyType.vehicleToBodyStyleConfigCount) {
            this.bodyTypeService.getBodyTypeDetail(bodyType.id).subscribe(function (m) {
                bodyType.bodyStyleConfigCount = m.bodyStyleConfigCount;
                bodyType.vehicleToBodyStyleConfigCount = m.vehicleToBodyStyleConfigCount;
                if (bodyType.bodyStyleConfigCount > 0) {
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
            if (bodyType.bodyStyleConfigCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    BodyTypeListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        if (!this.validationCheck(this.bodyType)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.bodyType.attachments = uploadedFiles;
            }
            if (_this.bodyType.attachments) {
                _this.bodyType.attachments = _this.bodyType.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.bodyTypeService.deleteBodyType(_this.bodyType.id, _this.bodyType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Body Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.bodyType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.bodyType.name + "\" Body Type change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.bodyTypes.filter(function (x) { return x.id == _this.bodyType.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.bodyType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.bodyType.name);
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
    BodyTypeListComponent.prototype.view = function (bodyTypeVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/bodytype/" + bodyTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    BodyTypeListComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BodyTypeListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BodyTypeListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BodyTypeListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BodyTypeListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BodyTypeListComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BodyTypeListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BodyTypeListComponent.prototype, "deletePopupAcFileUploader", void 0);
    BodyTypeListComponent = __decorate([
        core_1.Component({
            selector: 'bodyType-list-component',
            templateUrl: 'app/templates/bodyType/bodyType-list.component.html',
        }), 
        __metadata('design:paramtypes', [bodyType_service_1.BodyTypeService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], BodyTypeListComponent);
    return BodyTypeListComponent;
}());
exports.BodyTypeListComponent = BodyTypeListComponent;
