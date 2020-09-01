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
var router_1 = require("@angular/router");
var year_service_1 = require('./year.service');
var constants_warehouse_1 = require('../constants-warehouse');
var navigation_service_1 = require("../shared/navigation.service");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var YearListComponent = (function () {
    function YearListComponent(yearService, toastr, router, navigationService) {
        this.yearService = yearService;
        this.toastr = toastr;
        this.router = router;
        this.navigationService = navigationService;
        this.isYearValidNumber = true;
        this.isYearLengthValid = true;
        this.year = { baseVehicleCount: 0 };
        this.selectedYear = {};
        this.showLoadingGif = false;
    }
    YearListComponent.prototype.ngOnInit = function () {
        this.getYears();
    };
    YearListComponent.prototype.getYears = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.yearService.getYears().subscribe(function (yearList) {
            _this.years = yearList;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    YearListComponent.prototype.onCancel = function (action) {
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
    };
    YearListComponent.prototype.validationCheck = function (item) {
        if (item.changetype === "Add") {
            if (!item.id) {
                this.toastr.warning("Year is Invalid.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
            if (isNaN(item.id)) {
                this.toastr.warning("Year is Not a Number.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
            if (item.id.toString().length !== 4) {
                this.toastr.warning("Year must be of four digit", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
        }
        return true;
    };
    YearListComponent.prototype.add = function (year) {
        var _this = this;
        this.year.changetype = "Add";
        if (!this.validationCheck(this.year)) {
            return;
        }
        this.showLoadingGif = true;
        this.yearService.addYear(year)
            .subscribe(function (response) {
            if (response) {
                _this.year.id = null;
                var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Year", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.year.id);
                successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.year.id + "\" Year change requestid  \"" + response + "\" will be reviewed.";
                _this.toastr.success(successMessage.body, successMessage.title);
            }
            else {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Year", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.year.id);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
            }
            _this.showLoadingGif = false;
        }, function (error) {
            _this.showLoadingGif = false;
            var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Year", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.year.id);
            _this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
        });
    };
    YearListComponent.prototype.delete = function (year) {
        var _this = this;
        this.selectedYear = year;
        this.selectedYear.comment = "";
        this.showLoadingGif = true;
        if (!year.baseVehicleCount) {
            this.yearService.getDependencies(year.id).subscribe(function (m) {
                year.baseVehicleCount = m.baseVehicleCount;
                if (year.baseVehicleCount > 0) {
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
            if (year.baseVehicleCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("sm");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    YearListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        this.year.changetype = "Delete";
        if (!this.validationCheck(this.year)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.year.attachments = uploadedFiles;
            }
            if (_this.year.attachments) {
                _this.year.attachments = _this.year.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.yearService.deleteYear(_this.selectedYear.id, _this.year).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Year", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.selectedYear.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.year.id + "\" Region change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.years.filter(function (x) { return x.id == _this.selectedYear.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Year", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.selectedYear.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Year", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.selectedYear.id);
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
    YearListComponent.prototype.view = function (YearVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/year/" + YearVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    YearListComponent.prototype.cleanupComponent = function () {
        return this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], YearListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], YearListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], YearListComponent.prototype, "deletePopupAcFileUploader", void 0);
    YearListComponent = __decorate([
        core_1.Component({
            selector: 'year-list',
            templateUrl: 'app/templates/year/year-list.component.html',
        }), 
        __metadata('design:paramtypes', [year_service_1.YearService, ng2_toastr_1.ToastsManager, router_1.Router, navigation_service_1.NavigationService])
    ], YearListComponent);
    return YearListComponent;
}());
exports.YearListComponent = YearListComponent;
