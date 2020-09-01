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
var vehicleToBedConfig_service_1 = require("../vehicleToBedConfig/vehicleToBedConfig.service");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var constants_warehouse_1 = require("../constants-warehouse");
var ac_grid_1 = require('../../lib/aclibs/ac-grid/ac-grid');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var VehicleToBedConfigSearchComponent = (function () {
    function VehicleToBedConfigSearchComponent(sharedService, vehicleToBedConfigService, router, toastr, navigationService) {
        this.sharedService = sharedService;
        this.vehicleToBedConfigService = vehicleToBedConfigService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.isHide = false;
        this.vehicleToBedConfigsRetrieved = [];
        this.isLeftMenuHidden = false;
        this.activeSubMenu = '';
        this.activeSubMenuGroup = '';
        this.isBedSystemsExpanded = true;
        this.isAssociatedVehiclesExpanded = true;
        this.isSystemsMenuExpanded = true;
        this.isChildClicked = false;
        this.isMenuExpanded = true;
        this.showLoadingGif = false;
    }
    VehicleToBedConfigSearchComponent.prototype.ngOnInit = function () {
        this.sharedService.vehicles = null; //clear old selections
        this.sharedService.bedConfigs = null; //clear old selections
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
    VehicleToBedConfigSearchComponent.prototype.ngDoCheck = function () {
        if (this.previousBedConfigs != this.vehicleToBedConfigSearchViewModel.result.bedConfigs) {
            this.isSelectAllBedConfigs = false;
            if (this.vehicleToBedConfigSearchViewModel.result.bedConfigs.length > 0) {
                if (this.vehicleToBedConfigSearchViewModel.result.bedConfigs.every(function (item) { return item.isSelected; }))
                    this.isSelectAllBedConfigs = true;
                this.previousBedConfigs = this.vehicleToBedConfigSearchViewModel.result.bedConfigs;
            }
        }
    };
    VehicleToBedConfigSearchComponent.prototype.refreshGrids = function () {
        if (this.bedConfigGrid)
            this.bedConfigGrid.refresh();
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
    };
    VehicleToBedConfigSearchComponent.prototype.onSelectAllBedConfig = function (selected) {
        var _this = this;
        this.isSelectAllBedConfigs = selected;
        if (this.vehicleToBedConfigSearchViewModel.result.bedConfigs == null) {
            return;
        }
        this.vehicleToBedConfigsForSelectedBed = [];
        // get vehicle to bed config of base vehicle
        this.vehicleToBedConfigSearchViewModel.result.bedConfigs.forEach(function (item) {
            item.isSelected = selected;
            _this.refreshAssociationWithBedConfigId(item.id, item.isSelected);
        });
        // refresh vehicleToBedConfig grid.
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
    };
    VehicleToBedConfigSearchComponent.prototype.onBedConfigSelected = function (bedConfig) {
        this.refreshAssociationWithBedConfigId(bedConfig.id, !bedConfig.isSelected);
        if (bedConfig.isSelected) {
            //unchecked
            this.isSelectAllBedConfigs = false;
        }
        else {
            //checked
            var excludedBodyStyles = this.vehicleToBedConfigSearchViewModel.result.bedConfigs.filter(function (item) { return item.id != bedConfig.id; });
            if (excludedBodyStyles.every(function (item) { return item.isSelected; })) {
                this.isSelectAllBedConfigs = true;
            }
        }
        // refresh vehicle to bed config grid.
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
    };
    VehicleToBedConfigSearchComponent.prototype.refreshAssociationWithBedConfigId = function (bedConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToBedConfigsRetrieved = this.getVehicleToBedConfigsByBedConfigId(bedConfigId);
            //TODO: number of associations which may be useful in add bed association screen?
            var temp = this.vehicleToBedConfigsForSelectedBed || [];
            for (var _i = 0, _a = this.vehicleToBedConfigsRetrieved; _i < _a.length; _i++) {
                var vehicleToBedConfig = _a[_i];
                temp.push(vehicleToBedConfig);
            }
            this.vehicleToBedConfigsForSelectedBed = temp;
        }
        else {
            var m = this.vehicleToBedConfigsForSelectedBed.filter(function (x) { return x.bedConfig.id != bedConfigId; });
            this.vehicleToBedConfigsForSelectedBed = m;
        }
    };
    VehicleToBedConfigSearchComponent.prototype.getVehicleToBedConfigsByBedConfigId = function (id) {
        return this.vehicleToBedConfigSearchViewModel.result.vehicleToBedConfigs.filter(function (v) { return v.bedConfig.id == id; });
    };
    VehicleToBedConfigSearchComponent.prototype.onSelectedNewBedAssociation = function () {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletobedconfig/add"]);
    };
    VehicleToBedConfigSearchComponent.prototype.onDeleteVehicleToBedConfig = function (vehicleToBedConfig) {
        this.deleteVehicleToBedConfig = vehicleToBedConfig;
        this.deleteVehicleToBedConfig.comment = "";
        this.deleteBedAssociationPopup.open("md");
    };
    VehicleToBedConfigSearchComponent.prototype.onDeleteVehicleToBedConfigSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteVehicleToBedConfig.attachments = uploadedFiles;
            }
            if (_this.deleteVehicleToBedConfig.attachments) {
                _this.deleteVehicleToBedConfig.attachments = _this.deleteVehicleToBedConfig.attachments.concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleToBedConfigService.deleteVehicleToBedConfig(_this.deleteVehicleToBedConfig.id, _this.deleteVehicleToBedConfig).subscribe(function (response) {
                if (response) {
                    _this.deleteBedAssociationPopup.close();
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Bed Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBedConfig.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + "Vehicle to bed association bed config id \"" + _this.deleteVehicleToBedConfig.id + "\" Vehicleid  \"" + _this.deleteVehicleToBedConfig.vehicleId + "\" with change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBedConfig.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                    _this.showLoadingGif = false;
                }
                _this.showLoadingGif = false;
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Bed Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBedConfig.id);
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
    VehicleToBedConfigSearchComponent.prototype.routerLinkRedirect = function (route, id) {
        this.sharedService.vehicleToBedConfigSearchViewModel = this.vehicleToBedConfigSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    };
    VehicleToBedConfigSearchComponent.prototype.onViewBedConfigCr = function (bedConfigVm) {
        this.sharedService.vehicleToBedConfigSearchViewModel = this.vehicleToBedConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/bedconfig/" + bedConfigVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    VehicleToBedConfigSearchComponent.prototype.onViewAssociatedVehiclesCr = function (associatedVehicleVm) {
        this.sharedService.vehicleToBedConfigSearchViewModel = this.vehicleToBedConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletobedconfig/" + associatedVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    __decorate([
        core_1.ViewChild('deleteBedAssociationPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToBedConfigSearchComponent.prototype, "deleteBedAssociationPopup", void 0);
    __decorate([
        core_1.ViewChild(ac_fileuploader_1.AcFileUploader), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToBedConfigSearchComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("bedConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleToBedConfigSearchComponent.prototype, "bedConfigGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleToBedConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleToBedConfigSearchComponent.prototype, "vehicleToBedConfigGrid", void 0);
    __decorate([
        core_1.Input("thresholdRecordCount"), 
        __metadata('design:type', Number)
    ], VehicleToBedConfigSearchComponent.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.Input("vehicleToBedConfigSearchViewModel"), 
        __metadata('design:type', Object)
    ], VehicleToBedConfigSearchComponent.prototype, "vehicleToBedConfigSearchViewModel", void 0);
    __decorate([
        core_1.Input("vehicleToBedConfigsForSelectedBed"), 
        __metadata('design:type', Array)
    ], VehicleToBedConfigSearchComponent.prototype, "vehicleToBedConfigsForSelectedBed", void 0);
    VehicleToBedConfigSearchComponent = __decorate([
        //pushkar: remove if not needed
        core_1.Component({
            selector: "vehicletobedconfig-search",
            templateUrl: "app/templates/vehicletobedConfig/vehicletobedConfig-search.component.html",
            providers: [vehicleToBedConfig_service_1.VehicleToBedConfigService, shared_service_1.SharedService]
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, vehicleToBedConfig_service_1.VehicleToBedConfigService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleToBedConfigSearchComponent);
    return VehicleToBedConfigSearchComponent;
}());
exports.VehicleToBedConfigSearchComponent = VehicleToBedConfigSearchComponent;
