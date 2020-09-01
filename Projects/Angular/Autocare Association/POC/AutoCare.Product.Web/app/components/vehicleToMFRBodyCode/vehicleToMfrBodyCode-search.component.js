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
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var vehicleToMfrBodyCode_service_1 = require("./vehicleToMfrBodyCode.service");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_grid_1 = require("../../lib/aclibs/ac-grid/ac-grid");
var ac_fileuploader_1 = require("../../lib/aclibs/ac-fileuploader/ac-fileuploader");
var mfrBodyCode_service_1 = require("../mfrBodyCode/mfrBodyCode.service");
var VehicleToMfrBodyCodeSearchComponent = (function () {
    function VehicleToMfrBodyCodeSearchComponent(sharedService, vehicleToMfrBodyCodeService, router, toastr, navigationService, mfrBodyCodeService) {
        this.sharedService = sharedService;
        this.vehicleToMfrBodyCodeService = vehicleToMfrBodyCodeService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.mfrBodyCodeService = mfrBodyCodeService;
        this.isHide = false;
        this.vehicleTomfrBodyCodesRetrieved = [];
        this.isLeftMenuHidden = false;
        this.activeSubMenu = '';
        this.activeSubMenuGroup = '';
        this.isMfrBodyCodeExpanded = true;
        this.isAssociatedVehiclesExpanded = true;
        this.isSystemsMenuExpanded = true;
        this.isChildClicked = false;
        this.isMenuExpanded = true;
        this.showLoadingGif = false;
        this.mfrBodyCode = {};
        this.modifyMfrBodyCode = {};
        this.selectedMfrBodyCode = {};
    }
    VehicleToMfrBodyCodeSearchComponent.prototype.ngOnInit = function () {
        this.sharedService.vehicles = null; //clear old selections
        this.sharedService.mfrBodyCodes = null; //clear old selections
        // Drawer right start
        var headerht = $('header').innerHeight();
        var navht = $('nav').innerHeight();
        var winht = $(window).height();
        var winwt = 960;
        $(".drawer-left").css('min-height', winht - headerht - navht);
        $(".drawer-left").css('width', winwt);
        $(document).on('click', '.drawer-show', function (event) {
            $(".drawer-left").css('width', winwt);
        });
        $(".drawer-left span").on('click', function () {
            var drawerwt = $(".drawer-left").width();
            if (drawerwt == 15) {
                $(".drawer-left").css('width', winwt);
            }
            else {
                $(".drawer-left").css('width', 15);
            }
        });
        $(document).on('click', function (event) {
            if (!$(event.target).closest(".drawer-left").length) {
                // Hide the menus.
                var drawerwt = $(".drawer-left").width();
                if (drawerwt > 15) {
                    $(".drawer-left").css('width', 15);
                }
            }
        });
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.ngDoCheck = function () {
        if (this.previousmfrBodyCodes != this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes) {
            this.isSelectAllMfrBodyCodes = false;
            if (this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.length > 0) {
                if (this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.every(function (item) { return item.isSelected; }))
                    this.isSelectAllMfrBodyCodes = true;
                this.previousmfrBodyCodes = this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes;
            }
        }
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.refreshGrids = function () {
        if (this.mfrBodyCodeGrid)
            this.mfrBodyCodeGrid.refresh();
        if (this.vehicleToMfrBodyCodeGrid)
            this.vehicleToMfrBodyCodeGrid.refresh();
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.onSelectAllMfrBodyCode = function (selected) {
        var _this = this;
        this.isSelectAllMfrBodyCodes = selected;
        if (this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes == null) {
            return;
        }
        this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = [];
        this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.forEach(function (item) {
            item.isSelected = selected;
            _this.refreshAssociationWithMfrBodyCodeId(item.id, item.isSelected);
        });
        // refresh grid
        if (this.vehicleToMfrBodyCodeGrid)
            this.vehicleToMfrBodyCodeGrid.refresh();
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.onMfrBodyCodeSelected = function (mfrBodyCode) {
        this.refreshAssociationWithMfrBodyCodeId(mfrBodyCode.id, !mfrBodyCode.isSelected);
        if (mfrBodyCode.isSelected) {
            //unchecked
            this.isSelectAllMfrBodyCodes = false;
        }
        else {
            //checked
            var excludedMfrBodyCode = this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.filter(function (item) { return item.id != mfrBodyCode.id; });
            if (excludedMfrBodyCode.every(function (item) { return item.isSelected; })) {
                this.isSelectAllMfrBodyCodes = true;
            }
        }
        // refresh grid
        if (this.vehicleToMfrBodyCodeGrid)
            this.vehicleToMfrBodyCodeGrid.refresh();
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.refreshAssociationWithMfrBodyCodeId = function (MfrBodyCodeId, isSelected) {
        if (isSelected) {
            this.vehicleTomfrBodyCodesRetrieved = this.getVehicleTomfrBodyCodesByMfrBodyCodeId(MfrBodyCodeId);
            var temp = this.vehicleToMfrBodyCodesForSelectedMfrBodyCode || [];
            for (var _i = 0, _a = this.vehicleTomfrBodyCodesRetrieved; _i < _a.length; _i++) {
                var vehicleToMfrBodyCode = _a[_i];
                temp.push(vehicleToMfrBodyCode);
            }
            this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = temp;
        }
        else {
            var m = this.vehicleToMfrBodyCodesForSelectedMfrBodyCode.filter(function (x) { return x.mfrBodyCode.id != MfrBodyCodeId; });
            this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = m;
        }
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.getVehicleTomfrBodyCodesByMfrBodyCodeId = function (id) {
        return this.vehicleToMfrBodyCodeSearchViewModel.result.vehicleToMfrBodyCodes.filter(function (v) { return v.mfrBodyCode.id == id; });
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.onSelectedNewMfrBodyCodeAssociation = function () {
        this.sharedService.mfrBodyCodes = this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.filter(function (item) { return item.isSelected; });
        this.sharedService.vehicleToMfrBodyCodeSearchViewModel = this.vehicleToMfrBodyCodeSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletomfrbodycode/add"]);
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.onDeleteVehicleToMfrBodyCode = function (vehicleToMfrBodyCode) {
        this.deleteVehicleToMfrBodyCode = vehicleToMfrBodyCode;
        this.deleteVehicleToMfrBodyCode.comment = "";
        this.deleteMfrBodyCodeAssociationPopup.open("md");
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.onDeleteVehicleToMfrBodyCodeSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteVehicleToMfrBodyCode.attachments = uploadedFiles;
            }
            if (_this.deleteVehicleToMfrBodyCode.attachments) {
                _this.deleteVehicleToMfrBodyCode.attachments = _this.deleteVehicleToMfrBodyCode.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleToMfrBodyCodeService.deleteVehicleToMfrBodyCode(_this.deleteVehicleToMfrBodyCode.id, _this.deleteVehicleToMfrBodyCode).subscribe(function (response) {
                if (response) {
                    _this.deleteMfrBodyCodeModalPopup.close();
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Mfr Body Code Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToMfrBodyCode.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + "Vehicle to Mfr Body Code association Mfr Body Code id \"" + _this.deleteVehicleToMfrBodyCode.id + "\" Vehicleid  \"" + _this.deleteVehicleToMfrBodyCode.vehicleId + "\" with change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.vehicleToMfrBodyCodeSearchViewModel.result.vehicleToMfrBodyCodes.filter(function (x) { return x.id == _this.deleteVehicleToMfrBodyCode.id; })[0].changeRequestId = response;
                    _this.deleteMfrBodyCodeAssociationPopup.close();
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Mfr Body Code Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToMfrBodyCode.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                    _this.showLoadingGif = false;
                }
                _this.showLoadingGif = false;
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Mfr Body Code Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToMfrBodyCode.id);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.acFileUploader.reset();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.acFileUploader.reset();
            _this.showLoadingGif = false;
        });
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.routerLinkRedirect = function (route, id) {
        this.sharedService.vehicleToMfrBodyCodeSearchViewModel = this.vehicleToMfrBodyCodeSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.onViewMfrBodyCodeCr = function (mfrBodyCodeVm) {
        this.sharedService.vehicleToMfrBodyCodeSearchViewModel = this.vehicleToMfrBodyCodeSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/mfrbodycode/" + mfrBodyCodeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.onViewAssociatedVehiclesCr = function (associatedVehicleVm) {
        this.sharedService.vehicleToMfrBodyCodeSearchViewModel = this.vehicleToMfrBodyCodeSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletomfrbodycode/" + associatedVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    //Code For Add MfrBody Code Starts
    VehicleToMfrBodyCodeSearchComponent.prototype.openAddMfrBodyCodeModal = function () {
        this.addMfrBodyCodeModalPopup.open("md");
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.openModifyMfrBodyCodeModal = function (mfrBodyCode) {
        var _this = this;
        this.showLoadingGif = true;
        this.selectedMfrBodyCode = mfrBodyCode;
        this.modifyMfrBodyCode.id = this.selectedMfrBodyCode.id;
        this.modifyMfrBodyCode.name = this.selectedMfrBodyCode.name;
        this.modifyMfrBodyCode.comment = "";
        this.modifyMfrBodyCodeModalPopup.open("md");
        if (this.selectedMfrBodyCode.vehicleToMfrBodyCodeCount == 0) {
            this.mfrBodyCodeService.getMfrBodyCode(this.selectedMfrBodyCode.id).subscribe(function (result) {
                _this.selectedMfrBodyCode.vehicleToMfrBodyCodeCount = _this.modifyMfrBodyCode.vehicleToMfrBodyCodeCount = result.vehicleToMfrBodyCodeCount;
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error.toString(), "Load Failed");
                _this.showLoadingGif = false;
            });
        }
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.openDeleteMfrBodyCodeModal = function (mfrBodyCode) {
        var _this = this;
        this.showLoadingGif = true;
        this.selectedMfrBodyCode = mfrBodyCode;
        this.mfrBodyCode.id = this.selectedMfrBodyCode.id;
        this.mfrBodyCode.name = this.selectedMfrBodyCode.name;
        this.mfrBodyCode.comment = "";
        this.deleteMfrBodyCodeModalPopup.open("md");
        if (this.selectedMfrBodyCode.vehicleToMfrBodyCodeCount == 0) {
            this.mfrBodyCodeService.getMfrBodyCode(this.selectedMfrBodyCode.id).subscribe(function (result) {
                _this.selectedMfrBodyCode.vehicleToMfrBodyCodeCount = _this.mfrBodyCode.vehicleToMfrBodyCodeCount = result.vehicleToMfrBodyCodeCount;
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error.toString(), "Load Failed");
                _this.showLoadingGif = false;
            });
        }
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.onAddMfrBodyCodeCancel = function () {
        this.mfrBodyCode = [];
        this.modifyMfrBodyCode = [];
        this.addMfrBodyCodeModalPopup.close();
        this.acFileUploader.reset(true);
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.onModifyMfrBodyCodeCancel = function () {
        this.modifyMfrBodyCode = [];
        this.selectedMfrBodyCode = [];
        this.modifyMfrBodyCodeModalPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.onDeleteMfrBodyCodeCancel = function () {
        this.mfrBodyCode = [];
        this.modifyMfrBodyCode = [];
        this.selectedMfrBodyCode = [];
        this.deleteMfrBodyCodeModalPopup.close();
        this.deletePopupAcFileUploader.reset(true);
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.addMfrBodyCode = function () {
        var _this = this;
        this.mfrBodyCode.changeType = "Add";
        if (!this.validationCheck(this.mfrBodyCode)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.mfrBodyCode.attachments = uploadedFiles;
            }
            if (_this.mfrBodyCode.attachments) {
                _this.mfrBodyCode.attachments = _this.mfrBodyCode.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.mfrBodyCodeService.addMfrBodyCode(_this.mfrBodyCode).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Mfr Body Code", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.mfrBodyCode.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.mfrBodyCode.name + "\" Mfr Body Code change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Mfr Body Code", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.mfrBodyCode.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Mfr Body Code", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.mfrBodyCode.name);
                _this.toastr.warning(errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.showLoadingGif = false;
                _this.acFileUploader.reset(true);
                _this.addMfrBodyCodeModalPopup.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.acFileUploader.reset(true);
            _this.addMfrBodyCodeModalPopup.close();
        });
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.modifySubmit = function () {
        var _this = this;
        this.modifyMfrBodyCode.changeType = "Modify";
        if (!this.validationCheck(this.modifyMfrBodyCode)) {
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifyMfrBodyCode.attachments = uploadedFiles;
            }
            if (_this.modifyMfrBodyCode.attachments) {
                _this.modifyMfrBodyCode.attachments = _this.modifyMfrBodyCode.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.mfrBodyCodeService.updateMfrBodyCode(_this.selectedMfrBodyCode.id, _this.modifyMfrBodyCode).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Mfr Body Code", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.modifyMfrBodyCode.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.modifyMfrBodyCode.name + "\" Mfr Body Code change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.filter(function (x) { return x.id == _this.modifyMfrBodyCode.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Mfr Body Code", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.selectedMfrBodyCode.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                _this.showLoadingGif = true;
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Mfr Body Code", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.selectedMfrBodyCode.name);
                _this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
                _this.showLoadingGif = false;
                _this.modifyPopupAcFileUploader.reset(true);
                _this.modifyMfrBodyCodeModalPopup.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.modifyPopupAcFileUploader.reset(false);
            _this.modifyMfrBodyCodeModalPopup.close();
        });
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.deleteSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.selectedMfrBodyCode.attachments = uploadedFiles;
            }
            if (_this.selectedMfrBodyCode.attachments) {
                _this.selectedMfrBodyCode.attachments = _this.selectedMfrBodyCode.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.mfrBodyCodeService.deleteMfrBodyCode(_this.selectedMfrBodyCode.id, _this.selectedMfrBodyCode).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, "MfrBodyCodeId: " + _this.selectedMfrBodyCode.id);
                    successMessage.title = "You request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " MfrBodyCodeId " + _this.selectedMfrBodyCode.id + " change request ID \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.deleteMfrBodyCodeModalPopup.close();
                    _this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.filter(function (x) { return x.id == _this.selectedMfrBodyCode.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, "MfrBodyCodeId: " + _this.selectedMfrBodyCode.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                _this.showLoadingGif = true;
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, "MfrBodyCodeId: " + _this.selectedMfrBodyCode.id);
                _this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
                _this.deletePopupAcFileUploader.reset();
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.deletePopupAcFileUploader.reset();
            _this.showLoadingGif = false;
        });
    };
    VehicleToMfrBodyCodeSearchComponent.prototype.validationCheck = function (item) {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("Mfr Body Code is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
            if (this.mfrBodyCode.changeType == "Modify") {
                if (item.name === this.selectedMfrBodyCode.name) {
                    this.toastr.warning("Nothing has changed.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                    return false;
                }
            }
        }
        return true;
    };
    __decorate([
        core_1.ViewChild('deleteMfrBodyCodeAssociationPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToMfrBodyCodeSearchComponent.prototype, "deleteMfrBodyCodeAssociationPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToMfrBodyCodeSearchComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("mfrBodyCodeGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleToMfrBodyCodeSearchComponent.prototype, "mfrBodyCodeGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleToMfrBodyCodeGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleToMfrBodyCodeSearchComponent.prototype, "vehicleToMfrBodyCodeGrid", void 0);
    __decorate([
        core_1.ViewChild('addMfrBodyCodeModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToMfrBodyCodeSearchComponent.prototype, "addMfrBodyCodeModalPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyMfrBodyCodeModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToMfrBodyCodeSearchComponent.prototype, "modifyMfrBodyCodeModalPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopupAcFileUploader'), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToMfrBodyCodeSearchComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild('deletePopupAcFileUploader'), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToMfrBodyCodeSearchComponent.prototype, "deletePopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild('deleteMfrBodyCodeModal'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToMfrBodyCodeSearchComponent.prototype, "deleteMfrBodyCodeModalPopup", void 0);
    __decorate([
        core_1.Input("thresholdRecordCount"), 
        __metadata('design:type', Number)
    ], VehicleToMfrBodyCodeSearchComponent.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.Input("vehicleToMfrBodyCodeSearchViewModel"), 
        __metadata('design:type', Object)
    ], VehicleToMfrBodyCodeSearchComponent.prototype, "vehicleToMfrBodyCodeSearchViewModel", void 0);
    __decorate([
        core_1.Input("vehicleToMfrBodyCodesForSelectedMfrBodyCode"), 
        __metadata('design:type', Array)
    ], VehicleToMfrBodyCodeSearchComponent.prototype, "vehicleToMfrBodyCodesForSelectedMfrBodyCode", void 0);
    VehicleToMfrBodyCodeSearchComponent = __decorate([
        core_1.Component({
            selector: "vehicletomfrbodycode-search",
            templateUrl: "app/templates/vehicleToMfrBodyCode/vehicleToMfrBodyCode-search.component.html",
            providers: [vehicleToMfrBodyCode_service_1.VehicleToMfrBodyCodeService, mfrBodyCode_service_1.MfrBodyCodeService],
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, vehicleToMfrBodyCode_service_1.VehicleToMfrBodyCodeService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService, mfrBodyCode_service_1.MfrBodyCodeService])
    ], VehicleToMfrBodyCodeSearchComponent);
    return VehicleToMfrBodyCodeSearchComponent;
}());
exports.VehicleToMfrBodyCodeSearchComponent = VehicleToMfrBodyCodeSearchComponent;
