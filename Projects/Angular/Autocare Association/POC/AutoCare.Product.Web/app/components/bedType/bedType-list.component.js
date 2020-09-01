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
var bedType_service_1 = require('./bedType.service');
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var BedTypeListComponent = (function () {
    function BedTypeListComponent(bedTypeService, router, toastr, navigationService) {
        var _this = this;
        this.bedTypeService = bedTypeService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.filteredBedTypes = [];
        this.bedType = {};
        this.modifiedBedType = {};
        this.bedTypeNameFilter = '';
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.bedTypeNameFilter;
            }
            else {
                _this.bedTypeNameFilter = keyword;
            }
            if (String(_this.bedTypeNameFilter) === "") {
                _this.bedTypeService.getAllBedTypes().subscribe(function (sm) {
                    _this.bedTypes = sm;
                    _this.showLoadingGif = false;
                    _this.filteredBedTypes = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.bedTypeService.getBedTypes(_this.bedTypeNameFilter).subscribe(function (m) {
                    _this.bedTypes = m;
                    _this.showLoadingGif = false;
                    _this.filteredBedTypes = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.bedTypeService.getBedTypes(keyword);
        };
    }
    BedTypeListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.bedTypeService.getAllBedTypes().subscribe(function (sm) {
            _this.bedTypes = sm;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    BedTypeListComponent.prototype.onSelect = function (bedType) {
        this.bedTypeNameFilter = bedType.name;
        this.applyFilter();
        this.filteredBedTypes = [];
    };
    BedTypeListComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
    };
    BedTypeListComponent.prototype.onNew = function () {
        this.bedType = {};
        this.newPopup.open("md");
    };
    BedTypeListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.bedType)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.bedType.attachments = uploadedFiles;
            }
            if (_this.bedType.attachments) {
                _this.bedType.attachments = _this.bedType.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.bedTypeService.addBedType(_this.bedType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Bed Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.bedType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.bedType.name + "\" Bed Type change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.bedType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.bedType.name);
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
    BedTypeListComponent.prototype.validationCheck = function (item) {
        var isValid = true;
        if (!item.name) {
            this.toastr.warning("Name is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    BedTypeListComponent.prototype.onModify = function (bedType) {
        var _this = this;
        this.bedType = bedType;
        this.showLoadingGif = true;
        if (!bedType.bedConfigCount && !bedType.vehicleToBedConfigCount) {
            this.bedTypeService.getBedTypeDetail(bedType.id).subscribe(function (m) {
                bedType.bedConfigCount = m.bedConfigCount;
                bedType.vehicleToBedConfigCount = m.vehicleToBedConfigCount;
                _this.modifiedBedType = JSON.parse(JSON.stringify(bedType));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedBedType = JSON.parse(JSON.stringify(bedType));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    BedTypeListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedBedType)) {
            return;
        }
        else if (this.modifiedBedType.name == this.bedType.name) {
            this.toastr.warning("Nothing changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedBedType.attachments = uploadedFiles;
            }
            if (_this.modifiedBedType.attachments) {
                _this.modifiedBedType.attachments = _this.modifiedBedType.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.bedTypeService.updateBedType(_this.modifiedBedType.id, _this.modifiedBedType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Bed Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.bedType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.bedType.name + "\" Bed Type change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.bedTypes.filter(function (x) { return x.id == _this.modifiedBedType.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.bedType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.bedType.name);
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
    BedTypeListComponent.prototype.onDelete = function (bedType) {
        var _this = this;
        this.bedType = bedType;
        this.showLoadingGif = true;
        if (!bedType.bedConfigCount && !bedType.vehicleToBedConfigCount) {
            this.bedTypeService.getBedTypeDetail(bedType.id).subscribe(function (m) {
                bedType.bedConfigCount = m.bedConfigCount;
                bedType.vehicleToBedConfigCount = m.vehicleToBedConfigCount;
                if (bedType.bedConfigCount > 0) {
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
            if (bedType.bedConfigCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    BedTypeListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        if (!this.validationCheck(this.bedType)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.bedType.attachments = uploadedFiles;
            }
            if (_this.bedType.attachments) {
                _this.bedType.attachments = _this.bedType.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.bedTypeService.deleteBedType(_this.bedType.id, _this.bedType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Bed Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.bedType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.bedType.name + "\" BedType change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.bedTypes.filter(function (x) { return x.id == _this.bedType.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.bedType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.bedType.name);
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
    BedTypeListComponent.prototype.view = function (bedTypeVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/bedtype/" + bedTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    BedTypeListComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BedTypeListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BedTypeListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BedTypeListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BedTypeListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BedTypeListComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BedTypeListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BedTypeListComponent.prototype, "deletePopupAcFileUploader", void 0);
    BedTypeListComponent = __decorate([
        core_1.Component({
            selector: 'bedType-list-component',
            templateUrl: 'app/templates/bedType/bedType-list.component.html',
        }), 
        __metadata('design:paramtypes', [bedType_service_1.BedTypeService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], BedTypeListComponent);
    return BedTypeListComponent;
}());
exports.BedTypeListComponent = BedTypeListComponent;
