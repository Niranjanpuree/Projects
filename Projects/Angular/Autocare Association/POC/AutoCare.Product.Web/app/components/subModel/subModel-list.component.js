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
var subModel_service_1 = require("./subModel.service");
var router_1 = require("@angular/router");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var SubModelListComponent = (function () {
    function SubModelListComponent(subModelService, router, toastr, navigationService) {
        var _this = this;
        this.subModelService = subModelService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.filteredSubModels = [];
        this.subModel = {};
        this.modifiedSubModel = {};
        this.subModelNameFilter = '';
        this.showLoadingGif = false;
        this.getSuggestions = function (keyword) {
            return _this.subModelService.getFilteredSubModels(keyword);
        };
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.subModelNameFilter;
            }
            else {
                _this.subModelNameFilter = keyword;
            }
            if (String(_this.subModelNameFilter) === "") {
                _this.subModelService.getSubModels().subscribe(function (sm) {
                    _this.subModels = sm;
                    _this.showLoadingGif = false;
                    _this.filteredSubModels = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.subModelService.getFilteredSubModels(_this.subModelNameFilter).subscribe(function (m) {
                    _this.subModels = m;
                    _this.showLoadingGif = false;
                    _this.filteredSubModels = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
    }
    SubModelListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.subModelService.getSubModels().subscribe(function (sm) {
            _this.subModels = sm;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    SubModelListComponent.prototype.onSelect = function (subModel) {
        this.subModelNameFilter = subModel.name;
        this.filteredSubModels = [];
    };
    SubModelListComponent.prototype.onCancel = function (action) {
        this.newPopupAcFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
    };
    SubModelListComponent.prototype.onNew = function () {
        this.subModel = {};
        this.newPopup.open("md");
    };
    SubModelListComponent.prototype.validationCheck = function (item) {
        if (!item.name) {
            this.toastr.warning("Name is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return false;
        }
        return true;
    };
    SubModelListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.subModel)) {
            return;
        }
        this.showLoadingGif = true;
        this.newPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.subModel.attachments = uploadedFiles;
            }
            if (_this.subModel.attachments) {
                _this.subModel.attachments = _this.subModel.attachments.concat(_this.newPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.subModelService.addSubModel(_this.subModel).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Sub-Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.subModel.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.subModel.name + "\" Sub Model change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Sub-Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.subModel.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Sub-Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.subModel.name);
                _this.toastr.warning(error, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
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
    SubModelListComponent.prototype.onModify = function (subModel) {
        var _this = this;
        this.subModel = subModel;
        this.showLoadingGif = true;
        if (!subModel.vehicleCount) {
            this.subModelService.getSubModelDetail(subModel.id).subscribe(function (m) {
                subModel.vehicleCount = m.vehicleCount;
                _this.modifiedSubModel = JSON.parse(JSON.stringify(subModel));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedSubModel = JSON.parse(JSON.stringify(subModel));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    SubModelListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedSubModel)) {
            return;
        }
        if (this.modifiedSubModel.name == this.subModel.name) {
            this.toastr.warning("Nothing has changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedSubModel.attachments = uploadedFiles;
            }
            if (_this.modifiedSubModel.attachments) {
                _this.modifiedSubModel.attachments = _this.modifiedSubModel.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.subModelService.updateSubModel(_this.modifiedSubModel.id, _this.modifiedSubModel).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Sub-Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.subModel.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.subModel.name + "\" Sub Model change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.subModels.filter(function (x) { return x.id == _this.modifiedSubModel.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Sub-Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.subModel.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Sub-Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.subModel.name);
                _this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
                _this.modifyPopupAcFileUploader.reset(true);
                _this.modifyPopup.close();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.modifyPopupAcFileUploader.reset(true);
            _this.modifyPopup.close();
        });
    };
    SubModelListComponent.prototype.onDelete = function (subModel) {
        var _this = this;
        this.subModel = subModel;
        this.showLoadingGif = true;
        if (!subModel.vehicleCount) {
            this.subModelService.getSubModelDetail(subModel.id).subscribe(function (m) {
                subModel.vehicleCount = m.vehicleCount;
                if (subModel.vehicleCount > 0) {
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
            if (subModel.vehicleCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("sm");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    SubModelListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        if (!this.validationCheck(this.subModel)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.subModel.attachments = uploadedFiles;
            }
            if (_this.subModel.attachments) {
                _this.subModel.attachments = _this.subModel.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.subModelService.deleteSubModel(_this.subModel.id, _this.subModel).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Sub-Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.subModel.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.subModel.name + "\" Sub Model change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.subModels.filter(function (x) { return x.id == _this.subModel.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Sub-Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.subModel.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Sub-Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.subModel.name);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
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
    SubModelListComponent.prototype.view = function (SubModelVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/submodel/" + SubModelVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    SubModelListComponent.prototype.cleanupComponent = function () {
        return this.newPopupAcFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], SubModelListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], SubModelListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], SubModelListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], SubModelListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild("newPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], SubModelListComponent.prototype, "newPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], SubModelListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], SubModelListComponent.prototype, "deletePopupAcFileUploader", void 0);
    SubModelListComponent = __decorate([
        core_1.Component({
            selector: 'subModel-list-component',
            templateUrl: 'app/templates/subModel/subModel-list.component.html',
        }), 
        __metadata('design:paramtypes', [subModel_service_1.SubModelService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], SubModelListComponent);
    return SubModelListComponent;
}());
exports.SubModelListComponent = SubModelListComponent;
