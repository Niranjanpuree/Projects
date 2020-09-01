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
var region_service_1 = require('./region.service');
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var RegionListComponent = (function () {
    function RegionListComponent(regionService, router, toastr, navigationService) {
        var _this = this;
        this.regionService = regionService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.region = {};
        this.modifiedRegion = {};
        this.filteredRegions = [];
        this.regionNameFilter = '';
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.regionNameFilter;
            }
            else {
                _this.regionNameFilter = keyword;
            }
            if (String(_this.regionNameFilter) === "") {
                _this.regionService.getRegion().subscribe(function (s) {
                    _this.regions = s;
                    _this.showLoadingGif = false;
                    _this.filteredRegions = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.regionService.getRegionByNameFilter(_this.regionNameFilter).subscribe(function (m) {
                    _this.regions = m;
                    _this.showLoadingGif = false;
                    _this.filteredRegions = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.regionService.getRegionByNameFilter(keyword);
        };
    }
    RegionListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.regionService.getRegion().subscribe(function (s) {
            _this.regions = s;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    RegionListComponent.prototype.onSelect = function (region) {
        this.regionNameFilter = region.name;
        this.applyFilter();
        this.filteredRegions = [];
    };
    RegionListComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
    };
    RegionListComponent.prototype.validationCheck = function (item) {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("Region Name is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
            if (!item.regionAbbr) {
                this.toastr.warning("Region Abbreviation is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
            else {
                if (item.regionAbbr.length > 3) {
                    this.toastr.warning("Region Abbreviation cannot be more than 3 characters.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                    return false;
                }
                if (item.changeType == "Modify") {
                    if (this.region.name == item.name) {
                        this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                        return false;
                    }
                }
            }
        }
        return true;
    };
    RegionListComponent.prototype.onNew = function () {
        this.region = {};
        this.newPopup.open("md");
    };
    RegionListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        this.region.changeType = "Add";
        if (!this.validationCheck(this.region)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.region.attachments = uploadedFiles;
            }
            if (_this.region.attachments) {
                _this.region.attachments = _this.region.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.regionService.addRegion(_this.region).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Region", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.region.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.region.name + "\" Region change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Region", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.region.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Region", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.region.name);
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
    RegionListComponent.prototype.onModify = function (region) {
        var _this = this;
        this.region = region;
        this.showLoadingGif = true;
        if (!region.vehicleCount) {
            this.regionService.getRegionDetail(region.id).subscribe(function (m) {
                region.vehicleCount = m.vehicleCount;
                _this.modifiedRegion = JSON.parse(JSON.stringify(region));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedRegion = JSON.parse(JSON.stringify(region));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    RegionListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        this.modifiedRegion.changeType = "Modify";
        if (!this.validationCheck(this.modifiedRegion)) {
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedRegion.attachments = uploadedFiles;
            }
            if (_this.modifiedRegion.attachments) {
                _this.modifiedRegion.attachments = _this.modifiedRegion.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.regionService.updateRegion(_this.modifiedRegion.id, _this.modifiedRegion).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Region", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.region.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.region.name + "\" Region change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.regions.filter(function (x) { return x.id == _this.modifiedRegion.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Region", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.region.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                    _this.showLoadingGif = false;
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Region", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.region.name);
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
    RegionListComponent.prototype.onDelete = function (region) {
        var _this = this;
        this.region = region;
        this.region.comment = "";
        this.showLoadingGif = true;
        if (!region.vehicleCount) {
            this.regionService.getRegionDetail(region.id).subscribe(function (m) {
                region.vehicleCount = m.vehicleCount;
                if (region.vehicleCount > 0) {
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
            if (region.vehicleCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("sm");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    RegionListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        this.region.changeType = "Delete";
        if (!this.validationCheck(this.region)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.region.attachments = uploadedFiles;
            }
            if (_this.region.attachments) {
                _this.region.attachments = _this.region.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.regionService.deleteRegionPost(_this.region.id, _this.region).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Region", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.region.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.region.name + "\" Region change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.regions.filter(function (x) { return x.id == _this.region.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Region", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.region.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Region", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.region.name);
                _this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.showLoadingGif = false;
                _this.deletePopupAcFileUploader.reset();
                _this.deleteConfirmPopup.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.deletePopupAcFileUploader.reset(true);
            _this.deleteConfirmPopup.close();
        });
    };
    RegionListComponent.prototype.view = function (regionVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/region/" + regionVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    RegionListComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], RegionListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], RegionListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], RegionListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], RegionListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], RegionListComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], RegionListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], RegionListComponent.prototype, "deletePopupAcFileUploader", void 0);
    RegionListComponent = __decorate([
        core_1.Component({
            selector: 'region-list-component',
            templateUrl: 'app/templates/region/region-list.component.html',
        }), 
        __metadata('design:paramtypes', [region_service_1.RegionService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], RegionListComponent);
    return RegionListComponent;
}());
exports.RegionListComponent = RegionListComponent;
