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
var make_service_1 = require("./make.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var MakeListComponent = (function () {
    function MakeListComponent(makeService, toastr, sharedService, router, navigationService) {
        var _this = this;
        this.makeService = makeService;
        this.toastr = toastr;
        this.sharedService = sharedService;
        this.router = router;
        this.navigationService = navigationService;
        this.make = {};
        this.selectedMake = {};
        this.filteredMakes = [];
        this.filterTextMakeName = '';
        this.deleteModel = {};
        this.modifiedModel = {};
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || "";
            if (keyword == "") {
                keyword = _this.filterTextMakeName;
            }
            else {
                _this.filterTextMakeName = keyword;
            }
            if (String(_this.filterTextMakeName) === "") {
                _this.makeService.get().subscribe(function (result) {
                    _this.makeList = result;
                    _this.showLoadingGif = false;
                    _this.filteredMakes = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.makeService.getByFilter(_this.filterTextMakeName).subscribe(function (m) {
                    _this.makeList = m;
                    _this.showLoadingGif = false;
                    _this.filteredMakes = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.makeService.getByFilter(keyword);
        };
    }
    MakeListComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.makeList && this.makeList.length > 0) {
            return;
        }
        this.showLoadingGif = true;
        this.makeService.get().subscribe(function (makeList) {
            _this.makeList = makeList;
            _this.originalMakeList = _this.makeList;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.showLoadingGif = false;
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
        });
    };
    MakeListComponent.prototype.onSelect = function (make) {
        this.filterTextMakeName = make.name;
        this.applyFilter();
        this.filteredMakes = [];
    };
    MakeListComponent.prototype.validationCheck = function (item) {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("Make Name is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
            if (this.make.changeType == "Modify") {
                if (item.name === this.selectedMake.name) {
                    this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                    return false;
                }
            }
        }
        return true;
    };
    MakeListComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.addMakeModal.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyMakeModal.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteMakeConfirmModal.close();
    };
    MakeListComponent.prototype.add = function () {
        var _this = this;
        this.make.changeType = "Add";
        if (!this.validationCheck(this.make)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.make.attachments = uploadedFiles;
            }
            if (_this.make.attachments) {
                _this.make.attachments = _this.make.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.makeService.add(_this.make).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Make", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.make.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.make.name + "\" Make change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Make", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.make.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Make", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.make.name);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.showLoadingGif = false;
                _this.acFileUploader.reset(true);
                _this.addMakeModal.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.acFileUploader.reset(true);
            _this.addMakeModal.close();
        });
    };
    MakeListComponent.prototype.modify = function (makeVm) {
        var _this = this;
        this.showLoadingGif = true;
        this.selectedMake = makeVm;
        this.make.id = this.selectedMake.id;
        this.make.name = this.selectedMake.name;
        this.make.comment = "";
        this.modifyMakeModal.open("md");
        if (this.selectedMake.baseVehicleCount == 0 && this.selectedMake.vehicleCount == 0) {
            this.makeService.getById(this.selectedMake.id).subscribe(function (m) {
                _this.selectedMake.baseVehicleCount = _this.make.baseVehicleCount = m.baseVehicleCount;
                _this.selectedMake.vehicleCount = _this.make.vehicleCount = m.vehicleCount;
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error.toString(), "Load Failed");
            });
        }
        else {
            this.showLoadingGif = false;
        }
    };
    MakeListComponent.prototype.modifySubmit = function () {
        var _this = this;
        this.make.changeType = "Modify";
        if (!this.validationCheck(this.make)) {
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.make.attachments = uploadedFiles;
            }
            if (_this.make.attachments) {
                _this.make.attachments = _this.make.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.makeService.update(_this.selectedMake.id, _this.make).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Make", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.selectedMake.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.make.name + "\" Make change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.makeList.filter(function (x) { return x.id == _this.selectedMake.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Make", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.selectedMake.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Make", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.selectedMake.name);
                _this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
                _this.showLoadingGif = false;
                _this.modifyPopupAcFileUploader.reset(true);
                _this.modifyMakeModal.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.modifyPopupAcFileUploader.reset(false);
            _this.modifyMakeModal.close();
        });
    };
    MakeListComponent.prototype.delete = function (makeVm) {
        var _this = this;
        this.showLoadingGif = true;
        this.selectedMake = makeVm;
        this.make.id = this.selectedMake.id;
        this.make.name = this.selectedMake.name;
        this.make.comment = "";
        if (this.selectedMake.baseVehicleCount == 0 && this.selectedMake.vehicleCount == 0) {
            this.makeService.getById(this.selectedMake.id).subscribe(function (m) {
                _this.selectedMake.baseVehicleCount = m.baseVehicleCount;
                _this.selectedMake.vehicleCount = m.vehicleCount;
                if (_this.selectedMake.baseVehicleCount == 0 && _this.selectedMake.vehicleCount == 0) {
                    _this.showLoadingGif = false;
                    _this.deleteMakeConfirmModal.open("md");
                }
                else {
                    _this.showLoadingGif = false;
                    _this.deleteMakeErrorModal.open("sm");
                }
            }, function (error) {
                _this.showLoadingGif = false;
                _this.toastr.warning(error.toString(), "Load Failed");
            });
        }
        else {
            if (this.selectedMake.baseVehicleCount == 0 && this.selectedMake.vehicleCount == 0) {
                this.showLoadingGif = false;
                this.deleteMakeConfirmModal.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteMakeErrorModal.open("sm");
            }
        }
    };
    MakeListComponent.prototype.deleteSubmit = function () {
        var _this = this;
        this.selectedMake.changeType = "Delete";
        if (!this.validationCheck(this.selectedMake)) {
            return;
        }
        this.deleteMakeConfirmModal.close();
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.selectedMake.attachments = uploadedFiles;
            }
            if (_this.make.attachments) {
                _this.selectedMake.attachments = _this.selectedMake.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.makeService.delete(_this.selectedMake.id, _this.selectedMake).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Make", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.selectedMake.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.make.name + "\" Make change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.makeList.filter(function (x) { return x.id == _this.selectedMake.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Make", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.selectedMake.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                _this.ngOnInit();
            }, (function (errorresponses) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Make", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.selectedMake.name);
                _this.toastr.warning(errorresponses, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
                _this.showLoadingGif = false;
                _this.deletePopupAcFileUploader.reset(true);
                _this.deleteMakeConfirmModal.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.deletePopupAcFileUploader.reset(true);
            _this.deleteMakeConfirmModal.close();
        });
    };
    MakeListComponent.prototype.view = function (makeVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/make/" + makeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    MakeListComponent.prototype.openAddMakeModal = function () {
        this.make.id = 0;
        this.make.name = "";
        this.make.comment = "";
        this.addMakeModal.open("md");
    };
    MakeListComponent.prototype.setSelectedMake = function (make) {
        var selectedMake = make;
    };
    MakeListComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('addMakeModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], MakeListComponent.prototype, "addMakeModal", void 0);
    __decorate([
        core_1.ViewChild('modifyMakeModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], MakeListComponent.prototype, "modifyMakeModal", void 0);
    __decorate([
        core_1.ViewChild('deleteMakeConfirmModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], MakeListComponent.prototype, "deleteMakeConfirmModal", void 0);
    __decorate([
        core_1.ViewChild('deleteMakeErrorModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], MakeListComponent.prototype, "deleteMakeErrorModal", void 0);
    __decorate([
        core_1.ViewChild('viewMakeChangeRequestModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], MakeListComponent.prototype, "viewMakeChangeRequestModal", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], MakeListComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], MakeListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], MakeListComponent.prototype, "deletePopupAcFileUploader", void 0);
    MakeListComponent = __decorate([
        core_1.Component({
            selector: 'makes-list-comp',
            templateUrl: 'app/templates/make/make-list.component.html',
        }), 
        __metadata('design:paramtypes', [make_service_1.MakeService, ng2_toastr_1.ToastsManager, shared_service_1.SharedService, router_1.Router, navigation_service_1.NavigationService])
    ], MakeListComponent);
    return MakeListComponent;
}());
exports.MakeListComponent = MakeListComponent;
