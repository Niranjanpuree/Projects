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
var vehicleToBrakeConfig_service_1 = require("../vehicleToBrakeConfig/vehicleToBrakeConfig.service");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_grid_1 = require("../../lib/aclibs/ac-grid/ac-grid");
var ac_fileuploader_1 = require("../../lib/aclibs/ac-fileuploader/ac-fileuploader");
var VehicleToBrakeConfigSearchComponent = (function () {
    function VehicleToBrakeConfigSearchComponent(sharedService, vehicleToBrakeConfigService, router, toastr, navigationService) {
        this.sharedService = sharedService;
        this.vehicleToBrakeConfigService = vehicleToBrakeConfigService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.isHide = false;
        this.vehicleToBrakeConfigsRetrieved = [];
        this.isLeftMenuHidden = false;
        this.activeSubMenu = '';
        this.activeSubMenuGroup = '';
        this.isBrakeSystemsExpanded = true;
        this.isAssociatedVehiclesExpanded = true;
        this.isSystemsMenuExpanded = true;
        this.isChildClicked = false;
        this.isMenuExpanded = true;
        this.showLoadingGif = false;
    }
    VehicleToBrakeConfigSearchComponent.prototype.ngOnInit = function () {
        this.sharedService.vehicles = null; //clear old selections
        this.sharedService.brakeConfigs = null; //clear old selections
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
    VehicleToBrakeConfigSearchComponent.prototype.ngDoCheck = function () {
        if (this.previousBrakeConfigs != this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs) {
            this.isSelectAllBrakeSystems = false;
            if (this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.length > 0) {
                if (this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.every(function (item) { return item.isSelected; }))
                    this.isSelectAllBrakeSystems = true;
                this.previousBrakeConfigs = this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs;
            }
        }
    };
    VehicleToBrakeConfigSearchComponent.prototype.refreshGrids = function () {
        if (this.brakeConfigGrid)
            this.brakeConfigGrid.refresh();
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
    };
    VehicleToBrakeConfigSearchComponent.prototype.onSelectAllBrakeConfig = function (selected) {
        var _this = this;
        this.isSelectAllBrakeSystems = selected;
        if (this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs == null) {
            return;
        }
        this.vehicleToBrakeConfigsForSelectedBrake = [];
        this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.forEach(function (item) {
            item.isSelected = selected;
            _this.refreshAssociationWithBrakeConfigId(item.id, item.isSelected);
        });
        // refresh grid
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
    };
    VehicleToBrakeConfigSearchComponent.prototype.onBrakeConfigSelected = function (brakeConfig) {
        this.refreshAssociationWithBrakeConfigId(brakeConfig.id, !brakeConfig.isSelected);
        if (brakeConfig.isSelected) {
            //unchecked
            this.isSelectAllBrakeSystems = false;
        }
        else {
            //checked
            var excludedBrakeSystem = this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.filter(function (item) { return item.id != brakeConfig.id; });
            if (excludedBrakeSystem.every(function (item) { return item.isSelected; })) {
                this.isSelectAllBrakeSystems = true;
            }
        }
        // refresh grid
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
    };
    VehicleToBrakeConfigSearchComponent.prototype.refreshAssociationWithBrakeConfigId = function (brakeConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToBrakeConfigsRetrieved = this.getVehicleToBrakeConfigsByBrakeConfigId(brakeConfigId);
            //TODO: number of associations which may be useful in add brake association screen?
            var temp = this.vehicleToBrakeConfigsForSelectedBrake || [];
            for (var _i = 0, _a = this.vehicleToBrakeConfigsRetrieved; _i < _a.length; _i++) {
                var vehicleToBrakeConfig = _a[_i];
                temp.push(vehicleToBrakeConfig);
            }
            this.vehicleToBrakeConfigsForSelectedBrake = temp;
        }
        else {
            var m = this.vehicleToBrakeConfigsForSelectedBrake.filter(function (x) { return x.brakeConfig.id != brakeConfigId; });
            this.vehicleToBrakeConfigsForSelectedBrake = m;
        }
    };
    VehicleToBrakeConfigSearchComponent.prototype.getVehicleToBrakeConfigsByBrakeConfigId = function (id) {
        return this.vehicleToBrakeConfigSearchViewModel.result.vehicleToBrakeConfigs.filter(function (v) { return v.brakeConfig.id == id; });
    };
    VehicleToBrakeConfigSearchComponent.prototype.onSelectedNewBrakeAssociation = function () {
        this.sharedService.brakeConfigs = this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.filter(function (item) { return item.isSelected; });
        this.sharedService.vehicleToBrakeConfigSearchViewModel = this.vehicleToBrakeConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletobrakeconfig/add"]);
    };
    VehicleToBrakeConfigSearchComponent.prototype.onDeleteVehicleToBrakeConfig = function (vehicleToBrakeConfig) {
        this.deleteVehicleToBrakeConfig = vehicleToBrakeConfig;
        this.deleteVehicleToBrakeConfig.comment = "";
        this.deleteBrakeAssociationPopup.open("md");
    };
    VehicleToBrakeConfigSearchComponent.prototype.onDeleteVehicleToBrakeConfigSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteVehicleToBrakeConfig.attachments = uploadedFiles;
            }
            if (_this.deleteVehicleToBrakeConfig.attachments) {
                _this.deleteVehicleToBrakeConfig.attachments = _this.deleteVehicleToBrakeConfig.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleToBrakeConfigService.deleteVehicleToBrakeConfig(_this.deleteVehicleToBrakeConfig.id, _this.deleteVehicleToBrakeConfig).subscribe(function (response) {
                if (response) {
                    _this.deleteBrakeAssociationPopup.close();
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Brake Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBrakeConfig.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + "Vehicle to brake association brake config id \"" + _this.deleteVehicleToBrakeConfig.id + "\" Vehicleid  \"" + _this.deleteVehicleToBrakeConfig.vehicleId + "\" with change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBrakeConfig.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                    _this.showLoadingGif = false;
                }
                _this.showLoadingGif = false;
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Brake Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBrakeConfig.id);
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
    VehicleToBrakeConfigSearchComponent.prototype.routerLinkRedirect = function (route, id) {
        this.sharedService.vehicleToBrakeConfigSearchViewModel = this.vehicleToBrakeConfigSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    };
    VehicleToBrakeConfigSearchComponent.prototype.onViewBrakeConfigCr = function (brakeConfigVm) {
        this.sharedService.vehicleToBrakeConfigSearchViewModel = this.vehicleToBrakeConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/brakeconfig/" + brakeConfigVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    VehicleToBrakeConfigSearchComponent.prototype.onViewAssociatedVehiclesCr = function (associatedVehicleVm) {
        this.sharedService.vehicleToBrakeConfigSearchViewModel = this.vehicleToBrakeConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletobrakeconfig/" + associatedVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    __decorate([
        core_1.ViewChild('deleteBrakeAssociationPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToBrakeConfigSearchComponent.prototype, "deleteBrakeAssociationPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToBrakeConfigSearchComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("brakeConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleToBrakeConfigSearchComponent.prototype, "brakeConfigGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleToBrakeConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleToBrakeConfigSearchComponent.prototype, "vehicleToBrakeConfigGrid", void 0);
    __decorate([
        core_1.Input("thresholdRecordCount"), 
        __metadata('design:type', Number)
    ], VehicleToBrakeConfigSearchComponent.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.Input("vehicleToBrakeConfigSearchViewModel"), 
        __metadata('design:type', Object)
    ], VehicleToBrakeConfigSearchComponent.prototype, "vehicleToBrakeConfigSearchViewModel", void 0);
    __decorate([
        core_1.Input("vehicleToBrakeConfigsForSelectedBrake"), 
        __metadata('design:type', Array)
    ], VehicleToBrakeConfigSearchComponent.prototype, "vehicleToBrakeConfigsForSelectedBrake", void 0);
    VehicleToBrakeConfigSearchComponent = __decorate([
        core_1.Component({
            selector: "vehicletobrakeconfig-search",
            templateUrl: "app/templates/vehicletobrakeConfig/vehicletobrakeConfig-search.component.html",
            providers: [vehicleToBrakeConfig_service_1.VehicleToBrakeConfigService],
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, vehicleToBrakeConfig_service_1.VehicleToBrakeConfigService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleToBrakeConfigSearchComponent);
    return VehicleToBrakeConfigSearchComponent;
}());
exports.VehicleToBrakeConfigSearchComponent = VehicleToBrakeConfigSearchComponent;
