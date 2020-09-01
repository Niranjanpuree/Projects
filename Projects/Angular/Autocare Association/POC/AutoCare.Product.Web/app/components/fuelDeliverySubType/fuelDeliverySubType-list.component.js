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
var router_1 = require("@angular/router");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var fuelDeliverySubType_service_1 = require("./fuelDeliverySubType.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var FuelDeliverySubTypeListComponent = (function () {
    function FuelDeliverySubTypeListComponent(fuelDeliverySubTypeService, toastr, sharedService, router, navigationService) {
        var _this = this;
        this.fuelDeliverySubTypeService = fuelDeliverySubTypeService;
        this.toastr = toastr;
        this.sharedService = sharedService;
        this.router = router;
        this.navigationService = navigationService;
        this.fuelDeliverySubType = {};
        this.selectedFuelDeliverySubType = {};
        this.filteredFuelDeliverySubTypes = [];
        this.filterTextFuelDeliverySubTypeName = '';
        this.deleteModel = {};
        this.modifiedModel = {};
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || "";
            if (keyword == "") {
                keyword = _this.filterTextFuelDeliverySubTypeName;
            }
            else {
                _this.filterTextFuelDeliverySubTypeName = keyword;
            }
            if (String(_this.filterTextFuelDeliverySubTypeName) === "") {
                _this.fuelDeliverySubTypeService.get().subscribe(function (result) {
                    _this.fuelDeliverySubTypeList = result;
                    _this.showLoadingGif = false;
                    _this.filteredFuelDeliverySubTypes = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.fuelDeliverySubTypeService.getByFilter(_this.filterTextFuelDeliverySubTypeName).subscribe(function (m) {
                    _this.fuelDeliverySubTypeList = m;
                    _this.showLoadingGif = false;
                    _this.filteredFuelDeliverySubTypes = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.fuelDeliverySubTypeService.getByFilter(keyword);
        };
    }
    FuelDeliverySubTypeListComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.fuelDeliverySubTypeList && this.fuelDeliverySubTypeList.length > 0) {
            return;
        }
        this.showLoadingGif = true;
        this.fuelDeliverySubTypeService.get().subscribe(function (fuelDeliverySubTypeList) {
            _this.fuelDeliverySubTypeList = fuelDeliverySubTypeList;
            _this.originalFuelDeliverySubTypeList = _this.fuelDeliverySubTypeList;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.showLoadingGif = false;
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
        });
    };
    FuelDeliverySubTypeListComponent.prototype.onSelect = function (fuelDeliverySubType) {
        this.filterTextFuelDeliverySubTypeName = fuelDeliverySubType.name;
        this.applyFilter();
        this.filteredFuelDeliverySubTypes = [];
    };
    FuelDeliverySubTypeListComponent.prototype.validationCheck = function (item) {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("FuelDeliverySubType Name is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
            if (this.fuelDeliverySubType.changeType == "Modify") {
                if (item.name === this.selectedFuelDeliverySubType.name) {
                    this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                    return false;
                }
            }
        }
        return true;
    };
    FuelDeliverySubTypeListComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.addFuelDeliverySubTypeModal.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyFuelDeliverySubTypeModal.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteFuelDeliverySubTypeConfirmModal.close();
    };
    FuelDeliverySubTypeListComponent.prototype.add = function () {
        var _this = this;
        this.fuelDeliverySubType.changeType = "Add";
        if (!this.validationCheck(this.fuelDeliverySubType)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.fuelDeliverySubType.attachments = uploadedFiles;
            }
            if (_this.fuelDeliverySubType.attachments) {
                _this.fuelDeliverySubType.attachments = _this.fuelDeliverySubType.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.fuelDeliverySubTypeService.add(_this.fuelDeliverySubType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("FuelDeliverySubType", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.fuelDeliverySubType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.fuelDeliverySubType.name + "\" FuelDeliverySubType change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("FuelDeliverySubType", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.fuelDeliverySubType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("FuelDeliverySubType", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.fuelDeliverySubType.name);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.showLoadingGif = false;
                _this.acFileUploader.reset(true);
                _this.addFuelDeliverySubTypeModal.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.acFileUploader.reset(true);
            _this.addFuelDeliverySubTypeModal.close();
        });
    };
    FuelDeliverySubTypeListComponent.prototype.modify = function (fuelDeliverySubTypeVm) {
        var _this = this;
        this.showLoadingGif = true;
        this.selectedFuelDeliverySubType = fuelDeliverySubTypeVm;
        this.fuelDeliverySubType.id = this.selectedFuelDeliverySubType.id;
        this.fuelDeliverySubType.name = this.selectedFuelDeliverySubType.name;
        this.fuelDeliverySubType.comment = "";
        this.modifyFuelDeliverySubTypeModal.open("md");
        if (this.selectedFuelDeliverySubType.fuelDeliveryConfigCount == 0) {
            this.fuelDeliverySubTypeService.getById(this.selectedFuelDeliverySubType.id).subscribe(function (m) {
                _this.selectedFuelDeliverySubType.fuelDeliveryConfigCount = _this.fuelDeliverySubType.fuelDeliveryConfigCount = m.fuelDeliveryConfigCount;
                //this.selectedMake.vehicleCount = this.make.vehicleCount = m.vehicleCount;
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error.toString(), "Load Failed");
            });
        }
        else {
            this.showLoadingGif = false;
        }
    };
    FuelDeliverySubTypeListComponent.prototype.modifySubmit = function () {
        var _this = this;
        this.fuelDeliverySubType.changeType = "Modify";
        if (!this.validationCheck(this.fuelDeliverySubType)) {
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.fuelDeliverySubType.attachments = uploadedFiles;
            }
            if (_this.fuelDeliverySubType.attachments) {
                _this.fuelDeliverySubType.attachments = _this.fuelDeliverySubType.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.fuelDeliverySubTypeService.update(_this.selectedFuelDeliverySubType.id, _this.fuelDeliverySubType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("FuelDeliverySubType", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.selectedFuelDeliverySubType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.fuelDeliverySubType.name + "\" FuelDeliverySubType change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.fuelDeliverySubTypeList.filter(function (x) { return x.id == _this.selectedFuelDeliverySubType.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("FuelDeliverySubType", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.selectedFuelDeliverySubType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("FuelDeliverySubType", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.selectedFuelDeliverySubType.name);
                _this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
                _this.showLoadingGif = false;
                _this.modifyPopupAcFileUploader.reset(true);
                _this.modifyFuelDeliverySubTypeModal.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.modifyPopupAcFileUploader.reset(false);
            _this.modifyFuelDeliverySubTypeModal.close();
        });
    };
    FuelDeliverySubTypeListComponent.prototype.delete = function (fuelDeliverySubTypeVm) {
        var _this = this;
        this.showLoadingGif = true;
        this.selectedFuelDeliverySubType = fuelDeliverySubTypeVm;
        this.fuelDeliverySubType.id = this.selectedFuelDeliverySubType.id;
        this.fuelDeliverySubType.name = this.selectedFuelDeliverySubType.name;
        this.fuelDeliverySubType.comment = "";
        if (this.selectedFuelDeliverySubType.fuelDeliveryConfigCount == 0 /*&& this.selectedMake.vehicleCount == 0*/) {
            this.fuelDeliverySubTypeService.getById(this.selectedFuelDeliverySubType.id).subscribe(function (m) {
                _this.selectedFuelDeliverySubType.fuelDeliveryConfigCount = m.fuelDeliveryConfigCount;
                //this.selectedMake.vehicleCount = m.vehicleCount;
                if (_this.selectedFuelDeliverySubType.fuelDeliveryConfigCount == 0 /*&& this.selectedMake.vehicleCount == 0*/) {
                    _this.showLoadingGif = false;
                    _this.deleteFuelDeliverySubTypeConfirmModal.open("md");
                }
                else {
                    _this.showLoadingGif = false;
                    _this.deleteFuelDeliverySubTypeErrorModal.open("sm");
                }
            }, function (error) {
                _this.showLoadingGif = false;
                _this.toastr.warning(error.toString(), "Load Failed");
            });
        }
        else {
            if (this.selectedFuelDeliverySubType.fuelDeliveryConfigCount == 0 /*&& this.selectedFuelDeliverySubType.vehicleCount == 0*/) {
                this.showLoadingGif = false;
                this.deleteFuelDeliverySubTypeConfirmModal.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteFuelDeliverySubTypeErrorModal.open("sm");
            }
        }
    };
    FuelDeliverySubTypeListComponent.prototype.deleteSubmit = function () {
        var _this = this;
        this.selectedFuelDeliverySubType.changeType = "Delete";
        if (!this.validationCheck(this.selectedFuelDeliverySubType)) {
            return;
        }
        this.deleteFuelDeliverySubTypeConfirmModal.close();
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.selectedFuelDeliverySubType.attachments = uploadedFiles;
            }
            if (_this.fuelDeliverySubType.attachments) {
                _this.selectedFuelDeliverySubType.attachments = _this.selectedFuelDeliverySubType.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.fuelDeliverySubTypeService.delete(_this.selectedFuelDeliverySubType.id, _this.selectedFuelDeliverySubType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("FuelDeliverySubType", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.selectedFuelDeliverySubType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.fuelDeliverySubType.name + "\" FuelDeliverySubType change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.fuelDeliverySubTypeList.filter(function (x) { return x.id == _this.selectedFuelDeliverySubType.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("FuelDeliverySubType", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.selectedFuelDeliverySubType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                _this.ngOnInit();
            }, (function (errorresponses) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("FuelDeliverySubType", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.selectedFuelDeliverySubType.name);
                _this.toastr.warning(errorresponses, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
                _this.showLoadingGif = false;
                _this.deletePopupAcFileUploader.reset(true);
                _this.deleteFuelDeliverySubTypeConfirmModal.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.deletePopupAcFileUploader.reset(true);
            _this.deleteFuelDeliverySubTypeConfirmModal.close();
        });
    };
    FuelDeliverySubTypeListComponent.prototype.view = function (fuelDeliverySubTypeVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/fuelDeliverySubType/" + fuelDeliverySubTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    FuelDeliverySubTypeListComponent.prototype.openAddFuelDeliverySubTypeModal = function () {
        this.fuelDeliverySubType.id = 0;
        this.fuelDeliverySubType.name = "";
        this.fuelDeliverySubType.comment = "";
        this.addFuelDeliverySubTypeModal.open("md");
    };
    FuelDeliverySubTypeListComponent.prototype.setSelectedFuelDeliverySubType = function (fuelDeliverySubType) {
        var selectedFuelDeliverySubType = fuelDeliverySubType;
    };
    FuelDeliverySubTypeListComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('addFuelDeliverySubTypeModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], FuelDeliverySubTypeListComponent.prototype, "addFuelDeliverySubTypeModal", void 0);
    __decorate([
        core_1.ViewChild('modifyFuelDeliverySubTypeModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], FuelDeliverySubTypeListComponent.prototype, "modifyFuelDeliverySubTypeModal", void 0);
    __decorate([
        core_1.ViewChild('deleteFuelDeliverySubTypeConfirmModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], FuelDeliverySubTypeListComponent.prototype, "deleteFuelDeliverySubTypeConfirmModal", void 0);
    __decorate([
        core_1.ViewChild('deleteFuelDeliverySubTypeErrorModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], FuelDeliverySubTypeListComponent.prototype, "deleteFuelDeliverySubTypeErrorModal", void 0);
    __decorate([
        core_1.ViewChild('viewFuelDeliverySubTypeChangeRequestModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], FuelDeliverySubTypeListComponent.prototype, "viewFuelDeliverySubTypeChangeRequestModal", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], FuelDeliverySubTypeListComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], FuelDeliverySubTypeListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], FuelDeliverySubTypeListComponent.prototype, "deletePopupAcFileUploader", void 0);
    FuelDeliverySubTypeListComponent = __decorate([
        core_1.Component({
            selector: 'fuelDeliverySubTypes-list-comp',
            templateUrl: 'app/templates/fuelDeliverySubType/fuelDeliverySubType-list.component.html',
        }), 
        __metadata('design:paramtypes', [fuelDeliverySubType_service_1.FuelDeliverySubTypeService, ng2_toastr_1.ToastsManager, shared_service_1.SharedService, router_1.Router, navigation_service_1.NavigationService])
    ], FuelDeliverySubTypeListComponent);
    return FuelDeliverySubTypeListComponent;
}());
exports.FuelDeliverySubTypeListComponent = FuelDeliverySubTypeListComponent;
