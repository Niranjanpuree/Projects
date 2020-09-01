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
var vehicleToDriveType_service_1 = require("./vehicleToDriveType.service");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_grid_1 = require("../../lib/aclibs/ac-grid/ac-grid");
var ac_fileuploader_1 = require("../../lib/aclibs/ac-fileuploader/ac-fileuploader");
var driveType_Service_1 = require("../driveType/driveType.Service");
var VehicleToDriveTypeSearchComponent = (function () {
    function VehicleToDriveTypeSearchComponent(sharedService, driveTypeService, vehicleToDriveTypeService, router, toastr, navigationService) {
        this.sharedService = sharedService;
        this.driveTypeService = driveTypeService;
        this.vehicleToDriveTypeService = vehicleToDriveTypeService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.isHide = false;
        this.vehicleTodriveTypesRetrieved = [];
        this.isLeftMenuHidden = false;
        this.activeSubMenu = '';
        this.activeSubMenuGroup = '';
        this.isDriveTypeExpanded = true;
        this.isAssociatedVehiclesExpanded = true;
        this.isSystemsMenuExpanded = true;
        this.isChildClicked = false;
        this.isMenuExpanded = true;
        this.showLoadingGif = false;
        //filteredDriveTypes: IDriveType[] = [];
        this.driveType = {};
        this.modifiedDriveType = {};
    }
    VehicleToDriveTypeSearchComponent.prototype.ngOnInit = function () {
        this.sharedService.vehicles = null; //clear old selections
        this.sharedService.driveTypes = null; //clear old selections
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
    VehicleToDriveTypeSearchComponent.prototype.onNew = function () {
        this.driveType = {};
        this.newPopup.open("md");
    };
    VehicleToDriveTypeSearchComponent.prototype.onNewSubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.driveType)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.driveType.attachments = uploadedFiles;
            }
            if (_this.driveType.attachments) {
                _this.driveType.attachments = _this.driveType.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.driveTypeService.addDriveType(_this.driveType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Drive Type Name", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.driveType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the Drive Type  of name \"" + _this.driveType.name + "\" Drive Type change request Id  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Drive Type ", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.driveType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Drive Type ", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.driveType.name);
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
    VehicleToDriveTypeSearchComponent.prototype.onModify = function (driveType) {
        var _this = this;
        this.driveType = driveType;
        this.showLoadingGif = true;
        if (!driveType.vehicleToDriveTypeCount) {
            this.driveTypeService.getDriveType(driveType.id).subscribe(function (m) {
                _this.driveType.vehicleToDriveTypeCount = m.vehicleToDriveTypeCount;
                _this.modifiedDriveType = JSON.parse(JSON.stringify(driveType));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedDriveType = JSON.parse(JSON.stringify(driveType));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    VehicleToDriveTypeSearchComponent.prototype.onModifySubmit = function () {
        var _this = this;
        if (!this.validationCheck(this.modifiedDriveType)) {
            return;
        }
        else if (this.modifiedDriveType.name == this.driveType.name) {
            this.toastr.warning("Nothing changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedDriveType.attachments = uploadedFiles;
            }
            if (_this.modifiedDriveType.attachments) {
                _this.modifiedDriveType.attachments = _this.modifiedDriveType.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.driveTypeService.updateDriveType(_this.modifiedDriveType.id, _this.modifiedDriveType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Drive Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.driveType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the Drive Type with name \"" + _this.driveType.name + "\" Wheel Base change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.vehicleToDriveTypeSearchViewModel.result.driveTypes.find(function (x) { return x.id === _this.modifiedDriveType.id; }).changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Drive Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.driveType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Drive Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.driveType.name);
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
    VehicleToDriveTypeSearchComponent.prototype.onDelete = function (driveType) {
        var _this = this;
        this.driveType = driveType;
        this.showLoadingGif = true;
        if (!driveType.vehicleToDriveTypeCount) {
            this.driveTypeService.getDriveType(driveType.id).subscribe(function (m) {
                driveType.vehicleToDriveTypeCount = m.vehicleToDriveTypeCount;
                _this.driveType.vehicleToDriveTypeCount = driveType.vehicleToDriveTypeCount;
                if (driveType.vehicleToDriveTypeCount > 0) {
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
            if (driveType.vehicleToDriveTypeCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    VehicleToDriveTypeSearchComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        if (!this.validationCheck(this.driveType)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.driveType.attachments = uploadedFiles;
            }
            if (_this.driveType.attachments) {
                _this.driveType.attachments = _this.driveType.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.driveTypeService.deleteDriveType(_this.driveType.id, _this.driveType).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Drive Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.driveType.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the Drive Type with name \"" + _this.driveType.name + "\" Drive Type change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.vehicleToDriveTypeSearchViewModel.result.driveTypes.find(function (x) { return x.id === _this.driveType.id; }).changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Drive Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.driveType.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Drive Type", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.driveType.name);
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
    VehicleToDriveTypeSearchComponent.prototype.onReplace = function (driveType) {
        var replaceRequestLink = "/DriveType/replace/" + driveType.id;
        this.router.navigateByUrl(replaceRequestLink);
    };
    VehicleToDriveTypeSearchComponent.prototype.validationCheck = function (item) {
        var isValid = true;
        if (!item.name) {
            this.toastr.warning("Drive Type  is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    };
    VehicleToDriveTypeSearchComponent.prototype.onCancel = function (action) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
    };
    VehicleToDriveTypeSearchComponent.prototype.ngDoCheck = function () {
        if (this.previousdriveTypes != this.vehicleToDriveTypeSearchViewModel.result.driveTypes) {
            this.isSelectAllDriveTypes = false;
            if (this.vehicleToDriveTypeSearchViewModel.result.driveTypes.length > 0) {
                if (this.vehicleToDriveTypeSearchViewModel.result.driveTypes.every(function (item) { return item.isSelected; }))
                    this.isSelectAllDriveTypes = true;
                this.previousdriveTypes = this.vehicleToDriveTypeSearchViewModel.result.driveTypes;
            }
        }
    };
    VehicleToDriveTypeSearchComponent.prototype.refreshGrids = function () {
        if (this.driveTypeGrid)
            this.driveTypeGrid.refresh();
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
    };
    VehicleToDriveTypeSearchComponent.prototype.onSelectAllDriveType = function (selected) {
        var _this = this;
        this.isSelectAllDriveTypes = selected;
        if (this.vehicleToDriveTypeSearchViewModel.result.driveTypes == null) {
            return;
        }
        this.vehicleToDriveTypesForSelectedDriveType = [];
        this.vehicleToDriveTypeSearchViewModel.result.driveTypes.forEach(function (item) {
            item.isSelected = selected;
            _this.refreshAssociationWithDriveTypeId(item.id, item.isSelected);
        });
        // refresh grid
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
    };
    VehicleToDriveTypeSearchComponent.prototype.onDriveTypeSelected = function (driveType) {
        this.refreshAssociationWithDriveTypeId(driveType.id, !driveType.isSelected);
        if (driveType.isSelected) {
            //unchecked
            this.isSelectAllDriveTypes = false;
        }
        else {
            //checked
            var excludedDriveType = this.vehicleToDriveTypeSearchViewModel.result.driveTypes.filter(function (item) { return item.id != driveType.id; });
            if (excludedDriveType.every(function (item) { return item.isSelected; })) {
                this.isSelectAllDriveTypes = true;
            }
        }
        //refresh grid
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
    };
    VehicleToDriveTypeSearchComponent.prototype.refreshAssociationWithDriveTypeId = function (driveTypeId, isSelected) {
        if (isSelected) {
            this.vehicleTodriveTypesRetrieved = this.getVehicleTodriveTypesByDriveTypeId(driveTypeId);
            //TODO: number of associations which may be useful in add brake association screen?
            var temp = this.vehicleToDriveTypesForSelectedDriveType || [];
            for (var _i = 0, _a = this.vehicleTodriveTypesRetrieved; _i < _a.length; _i++) {
                var vehicleToDriveType = _a[_i];
                temp.push(vehicleToDriveType);
            }
            this.vehicleToDriveTypesForSelectedDriveType = temp;
        }
        else {
            var m = this.vehicleToDriveTypesForSelectedDriveType.filter(function (x) { return x.driveType.id != driveTypeId; });
            this.vehicleToDriveTypesForSelectedDriveType = m;
        }
    };
    VehicleToDriveTypeSearchComponent.prototype.getVehicleTodriveTypesByDriveTypeId = function (id) {
        return this.vehicleToDriveTypeSearchViewModel.result.vehicleToDriveTypes.filter(function (v) { return v.driveType.id == id; });
    };
    VehicleToDriveTypeSearchComponent.prototype.onSelectedNewDriveTypeAssociation = function () {
        this.sharedService.driveTypes = this.vehicleToDriveTypeSearchViewModel.result.driveTypes.filter(function (item) { return item.isSelected; });
        this.sharedService.vehicleToDriveTypeSearchViewModel = this.vehicleToDriveTypeSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletodrivetype/add"]);
    };
    VehicleToDriveTypeSearchComponent.prototype.onDeleteVehicleToDriveType = function (vehicleToDriveType) {
        this.deleteVehicleToDriveType = vehicleToDriveType;
        this.deleteVehicleToDriveType.comment = "";
        this.deleteDriveTypeAssociationPopup.open("md");
    };
    VehicleToDriveTypeSearchComponent.prototype.onDeleteVehicleToDriveTypeSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteVehicleToDriveType.attachments = uploadedFiles;
            }
            if (_this.deleteVehicleToDriveType.attachments) {
                _this.deleteVehicleToDriveType.attachments = _this.deleteVehicleToDriveType.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleToDriveTypeService.deleteVehicleToDriveType(_this.deleteVehicleToDriveType.id, _this.deleteVehicleToDriveType).subscribe(function (response) {
                if (response) {
                    _this.deleteDriveTypeAssociationPopup.close();
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Drive Type Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToDriveType.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + "Vehicle to drive type association body type id \"" + _this.deleteVehicleToDriveType.id + "\" Vehicleid  \"" + _this.deleteVehicleToDriveType.vehicleId + "\" with change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.vehicleToDriveTypesForSelectedDriveType.find(function (x) { return x.id === _this.deleteVehicleToDriveType.id; }).changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Drive Type Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToDriveType.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                    _this.showLoadingGif = false;
                }
                _this.showLoadingGif = false;
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Drive Type Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToDriveType.id);
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
    VehicleToDriveTypeSearchComponent.prototype.routerLinkRedirect = function (route, id) {
        this.sharedService.vehicleToDriveTypeSearchViewModel = this.vehicleToDriveTypeSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    };
    VehicleToDriveTypeSearchComponent.prototype.onViewDriveTypeCr = function (driveTypeVm) {
        this.sharedService.vehicleToDriveTypeSearchViewModel = this.vehicleToDriveTypeSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/drivetype/" + driveTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    VehicleToDriveTypeSearchComponent.prototype.onViewAssociatedVehiclesCr = function (associatedVehicleVm) {
        this.sharedService.vehicleToDriveTypeSearchViewModel = this.vehicleToDriveTypeSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletodrivetype/" + associatedVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToDriveTypeSearchComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToDriveTypeSearchComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToDriveTypeSearchComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToDriveTypeSearchComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToDriveTypeSearchComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToDriveTypeSearchComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToDriveTypeSearchComponent.prototype, "deletePopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild('deleteDriveTypeAssociationPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToDriveTypeSearchComponent.prototype, "deleteDriveTypeAssociationPopup", void 0);
    __decorate([
        core_1.ViewChild("driveTypeGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleToDriveTypeSearchComponent.prototype, "driveTypeGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleToDriveTypeGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleToDriveTypeSearchComponent.prototype, "vehicleToDriveTypeGrid", void 0);
    __decorate([
        core_1.Input("thresholdRecordCount"), 
        __metadata('design:type', Number)
    ], VehicleToDriveTypeSearchComponent.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.Input("vehicleToDriveTypeSearchViewModel"), 
        __metadata('design:type', Object)
    ], VehicleToDriveTypeSearchComponent.prototype, "vehicleToDriveTypeSearchViewModel", void 0);
    __decorate([
        core_1.Input("vehicleToDriveTypesForSelectedDriveType"), 
        __metadata('design:type', Array)
    ], VehicleToDriveTypeSearchComponent.prototype, "vehicleToDriveTypesForSelectedDriveType", void 0);
    VehicleToDriveTypeSearchComponent = __decorate([
        core_1.Component({
            selector: "vehicletodrivetype-search",
            templateUrl: "app/templates/vehicleToDriveType/vehicleToDriveType-search.component.html",
            providers: [vehicleToDriveType_service_1.VehicleToDriveTypeService, driveType_Service_1.DriveTypeService]
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, driveType_Service_1.DriveTypeService, vehicleToDriveType_service_1.VehicleToDriveTypeService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleToDriveTypeSearchComponent);
    return VehicleToDriveTypeSearchComponent;
}());
exports.VehicleToDriveTypeSearchComponent = VehicleToDriveTypeSearchComponent;
