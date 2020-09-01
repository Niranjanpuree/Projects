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
var vehicleToWheelBase_service_1 = require("../vehicleToWheelBase/vehicleToWheelBase.service");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_grid_1 = require("../../lib/aclibs/ac-grid/ac-grid");
var ac_fileuploader_1 = require("../../lib/aclibs/ac-fileuploader/ac-fileuploader");
var wheelBase_Service_1 = require("../wheelBase/wheelBase.Service");
var VehicleToWheelBaseSearchComponent = (function () {
    function VehicleToWheelBaseSearchComponent(sharedService, wheelBaseService, vehicleToWheelBaseService, router, toastr, navigationService) {
        this.sharedService = sharedService;
        this.wheelBaseService = wheelBaseService;
        this.vehicleToWheelBaseService = vehicleToWheelBaseService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.isHide = false;
        this.vehicleToWheelBaseRetrieved = [];
        this.isLeftMenuHidden = false;
        this.activeSubMenu = '';
        this.activeSubMenuGroup = '';
        this.isWheelBaseSystemsExpanded = true;
        this.isAssociatedVehiclesExpanded = true;
        this.isSystemsMenuExpanded = true;
        this.isChildClicked = false;
        this.isMenuExpanded = true;
        this.showLoadingGif = false;
        this.wheelBase = {};
        this.modifiedWheelBase = {};
    }
    VehicleToWheelBaseSearchComponent.prototype.ngOnInit = function () {
        this.sharedService.vehicles = null; //clear old selections
        this.sharedService.wheelBases = null; //clear old selections
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
        // Drawer right end
    };
    VehicleToWheelBaseSearchComponent.prototype.limittoonedecimal = function (userinput, modify) {
        if (!isNaN(userinput.target.value)) {
            var input = userinput.target.value;
            if (!isNaN(input)) {
                if (input > 0 || input != null) {
                    var myvalue = (input * 2.54).toString();
                    if (modify == 'add') {
                        this.wheelBase.wheelBaseMetric = parseFloat(myvalue).toFixed(1).toString();
                    }
                    if (modify == 'modify') {
                        this.modifiedWheelBase.wheelBaseMetric = parseFloat(myvalue).toFixed(1).toString();
                    }
                }
            }
            if (input) {
                if (userinput.target.value.match(/^(\d+)?([.]?\d{0,1})?$/)) {
                }
                else {
                    if (userinput.target.value.match(/^(\d+)?([.]?\d{0,2})?$/)) {
                        this.wheelBase.base = parseFloat(input).toFixed(1).toString();
                        this.modifiedWheelBase.base = parseFloat(input).toFixed(1).toString();
                    }
                    else {
                        this.wheelBase.base = parseFloat(input).toFixed(1).toString();
                        this.modifiedWheelBase.base = parseFloat(input).toFixed(1).toString();
                    }
                }
            }
        }
    };
    VehicleToWheelBaseSearchComponent.prototype.onNew = function () {
        this.wheelBase = {};
        this.newPopup.open("md");
    };
    VehicleToWheelBaseSearchComponent.prototype.validationCheck = function (item) {
        var isValid = true;
        if (!item.base) {
            this.toastr.warning("Wheel Base  is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        if (!item.wheelBaseMetric) {
            this.toastr.warning("Wheel Base Metric is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    VehicleToWheelBaseSearchComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.wheelBase)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.wheelBase.attachments = uploadedFiles;
            }
            if (_this.wheelBase.attachments) {
                _this.wheelBase.attachments = _this.wheelBase.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.wheelBaseService.addWheelBase(_this.wheelBase).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("WheelBase Name", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.wheelBase.base);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the WheelBase  of name \"" + _this.wheelBase.base + "\" and WheelBase Metric \"" + _this.wheelBase.wheelBaseMetric + "\" Wheel Base change request Id  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.wheelBase.base);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.wheelBase.base);
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
    VehicleToWheelBaseSearchComponent.prototype.onModify = function (wheelBase) {
        var _this = this;
        this.wheelBase = wheelBase;
        this.wheelBase.base = this.wheelBase.base.trim();
        this.wheelBase.wheelBaseMetric = this.wheelBase.wheelBaseMetric.trim();
        this.showLoadingGif = true;
        if (!wheelBase.vehicleToWheelBaseCount) {
            this.wheelBaseService.getWheelBaseDetail(wheelBase.id).subscribe(function (m) {
                _this.wheelBase.vehicleToWheelBaseCount = m.vehicleToWheelBaseCount;
                _this.modifiedWheelBase = JSON.parse(JSON.stringify(wheelBase));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedWheelBase = JSON.parse(JSON.stringify(wheelBase));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    VehicleToWheelBaseSearchComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedWheelBase)) {
            return;
        }
        else if (this.modifiedWheelBase.base == this.wheelBase.base && this.modifiedWheelBase.wheelBaseMetric == this.wheelBase.wheelBaseMetric) {
            this.toastr.warning("Nothing changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedWheelBase.attachments = uploadedFiles;
            }
            if (_this.modifiedWheelBase.attachments) {
                _this.modifiedWheelBase.attachments = _this.modifiedWheelBase.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.wheelBaseService.updateWheelBase(_this.modifiedWheelBase.id, _this.modifiedWheelBase).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.wheelBase.base);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the Wheel Base of length \"" + _this.wheelBase.base + "\" and Wheel Base metric \"" + _this.wheelBase.wheelBaseMetric + "\" Wheel Base change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.vehicleToWheelBaseSearchViewModel.result.wheelBases.filter(function (x) { return x.id == _this.modifiedWheelBase.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.wheelBase.base);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.wheelBase.base);
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
    VehicleToWheelBaseSearchComponent.prototype.onDelete = function (wheelBase) {
        var _this = this;
        this.wheelBase = wheelBase;
        this.showLoadingGif = true;
        if (!wheelBase.vehicleToWheelBaseCount) {
            this.wheelBaseService.getWheelBaseDetail(wheelBase.id).subscribe(function (m) {
                wheelBase.vehicleToWheelBaseCount = m.vehicleToWheelBaseCount;
                _this.wheelBase.vehicleToWheelBaseCount = wheelBase.vehicleToWheelBaseCount;
                if (wheelBase.vehicleToWheelBaseCount > 0) {
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
            if (wheelBase.vehicleToWheelBaseCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    VehicleToWheelBaseSearchComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        if (!this.validationCheck(this.wheelBase)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.wheelBase.attachments = uploadedFiles;
            }
            if (_this.wheelBase.attachments) {
                _this.wheelBase.attachments = _this.wheelBase.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.wheelBaseService.deleteWheelBase(_this.wheelBase.id, _this.wheelBase).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.wheelBase.base);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the Wheel Base of Base Length \"" + _this.wheelBase.base + "\" and Wheel Base metric \"" + _this.wheelBase.wheelBaseMetric + "\" Wheel Base change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.vehicleToWheelBaseSearchViewModel.result.wheelBases.filter(function (x) { return x.id == _this.wheelBase.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.wheelBase.base);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Wheel Base", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.wheelBase.base);
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
    VehicleToWheelBaseSearchComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
        this.deleteWheelBaseAssociationPopup.close();
    };
    VehicleToWheelBaseSearchComponent.prototype.ngDoCheck = function () {
        if (this.previousWheelBase != this.vehicleToWheelBaseSearchViewModel.result.wheelBases) {
            this.isSelectAllWheelBaseSystems = false;
            if (this.vehicleToWheelBaseSearchViewModel.result.wheelBases.length > 0) {
                if (this.vehicleToWheelBaseSearchViewModel.result.wheelBases.every(function (item) { return item.isSelected; }))
                    this.isSelectAllWheelBaseSystems = true;
                this.previousWheelBase = this.vehicleToWheelBaseSearchViewModel.result.wheelBases;
            }
        }
    };
    VehicleToWheelBaseSearchComponent.prototype.refreshGrids = function () {
        if (this.wheelBaseGrid)
            this.wheelBaseGrid.refresh();
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
    };
    VehicleToWheelBaseSearchComponent.prototype.view = function (wheelBaseVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/wheelbase/" + wheelBaseVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    VehicleToWheelBaseSearchComponent.prototype.onSelectAllWheelBase = function (selected) {
        var _this = this;
        this.isSelectAllWheelBaseSystems = selected;
        if (this.vehicleToWheelBaseSearchViewModel.result.wheelBases == null) {
            return;
        }
        this.vehicleToWheelBaseForSelectedWheelBase = [];
        this.vehicleToWheelBaseSearchViewModel.result.wheelBases.forEach(function (item) {
            item.isSelected = selected;
            _this.refreshAssociationWithWheelBaseId(item.id, item.isSelected);
        });
        // refresh grid
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
    };
    VehicleToWheelBaseSearchComponent.prototype.onWheelBaseSelected = function (wheelBase) {
        this.refreshAssociationWithWheelBaseId(wheelBase.id, !wheelBase.isSelected);
        if (wheelBase.isSelected) {
            //unchecked
            this.isSelectAllWheelBaseSystems = false;
        }
        else {
            //checked
            var excludedWheelBaseSystem = this.vehicleToWheelBaseSearchViewModel.result.wheelBases.filter(function (item) { return item.id != wheelBase.id; });
            if (excludedWheelBaseSystem.every(function (item) { return item.isSelected; })) {
                this.isSelectAllWheelBaseSystems = true;
            }
        }
        // refresh grid
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
    };
    VehicleToWheelBaseSearchComponent.prototype.refreshAssociationWithWheelBaseId = function (wheelBaseId, isSelected) {
        if (isSelected) {
            this.vehicleToWheelBaseRetrieved = this.getVehicleToWheelBaseByWheelBaseId(wheelBaseId);
            //TODO: number of associations which may be useful in add WheelBase association screen?
            var temp = this.vehicleToWheelBaseForSelectedWheelBase || [];
            for (var _i = 0, _a = this.vehicleToWheelBaseRetrieved; _i < _a.length; _i++) {
                var vehicleToWheelBase = _a[_i];
                temp.push(vehicleToWheelBase);
            }
            this.vehicleToWheelBaseForSelectedWheelBase = temp;
        }
        else {
            var m = this.vehicleToWheelBaseForSelectedWheelBase.filter(function (x) { return x.wheelBaseId != wheelBaseId; });
            this.vehicleToWheelBaseForSelectedWheelBase = m;
        }
    };
    VehicleToWheelBaseSearchComponent.prototype.getVehicleToWheelBaseByWheelBaseId = function (id) {
        return this.vehicleToWheelBaseSearchViewModel.result.vehicleToWheelBases.filter(function (v) { return v.wheelBaseId == id; });
    };
    VehicleToWheelBaseSearchComponent.prototype.onSelectedNewWheelBaseAssociation = function () {
        this.sharedService.wheelBases = this.vehicleToWheelBaseSearchViewModel.result.wheelBases.filter(function (item) { return item.isSelected; });
        this.sharedService.vehicleToWheelBaseSearchViewModel = this.vehicleToWheelBaseSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletowheelbase/add"]);
    };
    VehicleToWheelBaseSearchComponent.prototype.onDeleteVehicleToWheelBase = function (vehicleToWheelBase) {
        this.deleteVehicleToWheelBase = vehicleToWheelBase;
        this.deleteVehicleToWheelBase.comment = "";
        this.deleteWheelBaseAssociationPopup.open("md");
    };
    VehicleToWheelBaseSearchComponent.prototype.onDeleteVehicleToWheelBaseSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteVehicleToWheelBase.attachments = uploadedFiles;
            }
            if (_this.deleteVehicleToWheelBase.attachments) {
                _this.deleteVehicleToWheelBase.attachments = _this.deleteVehicleToWheelBase.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleToWheelBaseService.deleteVehicleToWheelBase(_this.deleteVehicleToWheelBase.id, _this.deleteVehicleToWheelBase).subscribe(function (response) {
                if (response) {
                    _this.deleteWheelBaseAssociationPopup.close();
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("WheelBase Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToWheelBase.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + "Vehicle to WheelBase association WheelBase id \"" + _this.deleteVehicleToWheelBase.id + "\" Vehicleid  \"" + _this.deleteVehicleToWheelBase.vehicleId + "\" with change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.vehicleToWheelBaseSearchViewModel.result.vehicleToWheelBases.filter(function (x) { return x.id == _this.deleteVehicleToWheelBase.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("WheelBase Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToWheelBase.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                    _this.showLoadingGif = false;
                }
                _this.showLoadingGif = false;
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("WheelBase Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToWheelBase.id);
                _this.toastr.warning(error, errorMessage.title);
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
    VehicleToWheelBaseSearchComponent.prototype.routerLinkRedirect = function (route, id) {
        this.sharedService.vehicleToWheelBaseSearchViewModel = this.vehicleToWheelBaseSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    };
    VehicleToWheelBaseSearchComponent.prototype.onViewWheelBaseCr = function (WheelBaseVm) {
        this.sharedService.vehicleToWheelBaseSearchViewModel = this.vehicleToWheelBaseSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/WheelBase/" + WheelBaseVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    VehicleToWheelBaseSearchComponent.prototype.onViewAssociatedVehiclesCr = function (associatedVehicleVm) {
        this.sharedService.vehicleToWheelBaseSearchViewModel = this.vehicleToWheelBaseSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletowheelbase/" + associatedVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    __decorate([
        core_1.ViewChild('deleteWheelBaseAssociationPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToWheelBaseSearchComponent.prototype, "deleteWheelBaseAssociationPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToWheelBaseSearchComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("wheelBaseConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleToWheelBaseSearchComponent.prototype, "wheelBaseGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleToWheelBaseGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleToWheelBaseSearchComponent.prototype, "vehicleToWheelBaseGrid", void 0);
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToWheelBaseSearchComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToWheelBaseSearchComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToWheelBaseSearchComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToWheelBaseSearchComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToWheelBaseSearchComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToWheelBaseSearchComponent.prototype, "deletePopupAcFileUploader", void 0);
    __decorate([
        core_1.Input("thresholdRecordCount"), 
        __metadata('design:type', Number)
    ], VehicleToWheelBaseSearchComponent.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.Input("vehicleToWheelBaseSearchViewModel"), 
        __metadata('design:type', Object)
    ], VehicleToWheelBaseSearchComponent.prototype, "vehicleToWheelBaseSearchViewModel", void 0);
    __decorate([
        core_1.Input("vehicleToWheelBaseForSelectedWheelBase"), 
        __metadata('design:type', Array)
    ], VehicleToWheelBaseSearchComponent.prototype, "vehicleToWheelBaseForSelectedWheelBase", void 0);
    VehicleToWheelBaseSearchComponent = __decorate([
        core_1.Component({
            selector: "vehicletowheelbase-search",
            templateUrl: "app/templates/vehicleToWheelBase/vehicleToWheelBase-search.component.html",
            providers: [vehicleToWheelBase_service_1.VehicleToWheelBaseService, wheelBase_Service_1.WheelBaseService],
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, wheelBase_Service_1.WheelBaseService, vehicleToWheelBase_service_1.VehicleToWheelBaseService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleToWheelBaseSearchComponent);
    return VehicleToWheelBaseSearchComponent;
}());
exports.VehicleToWheelBaseSearchComponent = VehicleToWheelBaseSearchComponent;
