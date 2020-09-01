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
var bedLength_service_1 = require('./bedLength.service');
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var BedLengthListComponent = (function () {
    function BedLengthListComponent(bedLengthService, router, toastr, navigationService) {
        var _this = this;
        this.bedLengthService = bedLengthService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.filteredBedLengths = [];
        this.bedLength = {};
        this.modifiedBedLength = {};
        this.bedLengthNameFilter = '';
        //dika: string = '';
        //maxlength = 10;
        //countDot = 0;
        //i = 0;
        this.showLoadingGif = false;
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.bedLengthNameFilter;
            }
            else {
                _this.bedLengthNameFilter = keyword;
            }
            if (String(_this.bedLengthNameFilter) === "") {
                _this.bedLengthService.getAllBedLengths().subscribe(function (sm) {
                    _this.bedLengths = sm;
                    _this.showLoadingGif = false;
                    _this.filteredBedLengths = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.bedLengthService.getBedLength(_this.bedLengthNameFilter).subscribe(function (m) {
                    _this.bedLengths = m;
                    _this.showLoadingGif = false;
                    _this.filteredBedLengths = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
        this.getSuggestions = function (keyword) {
            return _this.bedLengthService.getBedLength(keyword);
        };
    }
    BedLengthListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.bedLengthService.getAllBedLengths().subscribe(function (sm) {
            _this.bedLengths = sm;
            _this.showLoadingGif = false;
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    BedLengthListComponent.prototype.onSelect = function (bedlength) {
        this.bedLengthNameFilter = bedlength.length;
        this.applyFilter();
        this.filteredBedLengths = [];
    };
    BedLengthListComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
    };
    //test(input) {
    //    this.maxlength = 10;
    //    if(input){
    //    this.dika = input;
    //            for (this.i = 0; this.i < this.dika.length; this.i++) {
    //        if (this.dika[this.i] == '.') {
    //            this.maxlength = (this.i)+ 2;
    //            }
    //        }
    //    }
    //    //this.bedLength.length = this.dika;
    //}
    BedLengthListComponent.prototype.onNew = function () {
        this.bedLength = {};
        this.newPopup.open("md");
    };
    BedLengthListComponent.prototype.validationCheck = function (item) {
        var isValid = true;
        if (!item.length) {
            this.toastr.warning("Bed Length is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        if (!item.bedLengthMetric) {
            this.toastr.warning("Bed Length Metric is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    BedLengthListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.bedLength)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.bedLength.attachments = uploadedFiles;
            }
            if (_this.bedLength.attachments) {
                _this.bedLength.attachments = _this.bedLength.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.bedLengthService.addBedLength(_this.bedLength).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Bed Length", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.bedLength.length);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the Bed Length  of length \"" + _this.bedLength.length + "\" and bed length Metric \"" + _this.bedLength.bedLengthMetric + "\" Bed Length change request Id  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Length", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.bedLength.length);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Length", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.bedLength.length);
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
    BedLengthListComponent.prototype.onModify = function (bedlength) {
        var _this = this;
        this.bedLength = bedlength;
        this.showLoadingGif = true;
        if (!bedlength.vehicleToBedConfigCount) {
            this.bedLengthService.getBedLengthDetail(bedlength.id).subscribe(function (m) {
                _this.bedLength.vehicleToBedConfigCount = m.vehicleToBedConfigCount;
                _this.bedLength.bedConfigCount = m.bedConfigCount;
                _this.modifiedBedLength = JSON.parse(JSON.stringify(bedlength));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedBedLength = JSON.parse(JSON.stringify(bedlength));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    BedLengthListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedBedLength)) {
            return;
        }
        else if (this.modifiedBedLength.length == this.bedLength.length && this.modifiedBedLength.bedLengthMetric == this.bedLength.bedLengthMetric) {
            this.toastr.warning("Nothing changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedBedLength.attachments = uploadedFiles;
            }
            if (_this.modifiedBedLength.attachments) {
                _this.modifiedBedLength.attachments = _this.modifiedBedLength.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.bedLengthService.updateBedLength(_this.modifiedBedLength.id, _this.modifiedBedLength).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Bed Length", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.bedLength.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the bed length of length \"" + _this.bedLength.length + "\" and bed length metric \"" + _this.bedLength.bedLengthMetric + "\" Bed Length change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.bedLengths.filter(function (x) { return x.id == _this.modifiedBedLength.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Length", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.bedLength.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Length", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.bedLength.length);
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
    BedLengthListComponent.prototype.onDelete = function (bedlength) {
        var _this = this;
        this.bedLength = bedlength;
        this.showLoadingGif = true;
        if (!bedlength.vehicleToBedConfigCount) {
            this.bedLengthService.getBedLengthDetail(bedlength.id).subscribe(function (m) {
                bedlength.vehicleToBedConfigCount = m.vehicleToBedConfigCount;
                _this.bedLength.vehicleToBedConfigCount = bedlength.vehicleToBedConfigCount;
                if (bedlength.vehicleToBedConfigCount > 0) {
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
            if (bedlength.vehicleToBedConfigCount > 0 || bedlength.bedConfigCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    BedLengthListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        if (!this.validationCheck(this.bedLength)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.bedLength.attachments = uploadedFiles;
            }
            if (_this.bedLength.attachments) {
                _this.bedLength.attachments = _this.bedLength.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.bedLengthService.deleteBedLength(_this.bedLength.id, _this.bedLength).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Bed Length", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.bedLength.length);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the bed length of length \"" + _this.bedLength.length + "\" and bed length metric \"" + _this.bedLength.bedLengthMetric + "\" Bed Length change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.bedLengths.filter(function (x) { return x.id == _this.bedLength.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Length", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.bedLength.length);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Length", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.bedLength.length);
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
    BedLengthListComponent.prototype.view = function (bedLengthVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/bedlength/" + bedLengthVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    BedLengthListComponent.prototype.cleanupComponent = function () {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BedLengthListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BedLengthListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BedLengthListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BedLengthListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BedLengthListComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BedLengthListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BedLengthListComponent.prototype, "deletePopupAcFileUploader", void 0);
    BedLengthListComponent = __decorate([
        core_1.Component({
            selector: 'bedLength-list-component',
            templateUrl: 'app/templates/bedLength/bedLength-list.component.html',
        }), 
        __metadata('design:paramtypes', [bedLength_service_1.BedLengthService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], BedLengthListComponent);
    return BedLengthListComponent;
}());
exports.BedLengthListComponent = BedLengthListComponent;
