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
var ac_grid_1 = require("../../lib/aclibs/ac-grid/ac-grid");
var ac_fileuploader_1 = require("../../lib/aclibs/ac-fileuploader/ac-fileuploader");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var constants_warehouse_1 = require("../constants-warehouse");
var vehicleToBodyStyleConfig_service_1 = require("../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.service");
var VehicleToBodyStyleConfigSearchComponent = (function () {
    function VehicleToBodyStyleConfigSearchComponent(sharedService, vehicleToBodyStyleConfigService, router, toastr, navigationService) {
        this.sharedService = sharedService;
        this.vehicleToBodyStyleConfigService = vehicleToBodyStyleConfigService;
        this.router = router;
        this.toastr = toastr;
        this.navigationService = navigationService;
        this.isHide = false;
        this.vehicleToBodyStyleConfigsRetrieved = [];
        this.isLeftMenuHidden = false;
        this.activeSubMenu = '';
        this.activeSubMenuGroup = '';
        this.isBodyStyleConfigExpanded = true;
        this.isAssociatedVehiclesExpanded = true;
        this.isChildClicked = false;
        this.isMenuExpanded = true;
        this.showLoadingGif = false;
    }
    VehicleToBodyStyleConfigSearchComponent.prototype.ngOnInit = function () {
        this.sharedService.vehicles = null; //clear old selections
        //this.sharedService.bodyStyleConfigs = null;    //clear old selections
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
    VehicleToBodyStyleConfigSearchComponent.prototype.ngDoCheck = function () {
        if (this.previousBodyStyleConfigs != this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs) {
            this.isSelectAllBodyStyleConfigs = false;
            if (this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.length > 0) {
                if (this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.every(function (item) { return item.isSelected; }))
                    this.isSelectAllBodyStyleConfigs = true;
                this.previousBodyStyleConfigs = this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs;
            }
        }
    };
    VehicleToBodyStyleConfigSearchComponent.prototype.refreshGrids = function () {
        if (this.bodyStyleConfigGrid)
            this.bodyStyleConfigGrid.refresh();
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
    };
    VehicleToBodyStyleConfigSearchComponent.prototype.onSelectAllBodyStyleConfigs = function (selected) {
        var _this = this;
        this.isSelectAllBodyStyleConfigs = selected;
        if (this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs == null) {
            return;
        }
        this.vehicleToBodyStyleConfigsForSelectedBodyStyle = [];
        this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.forEach(function (item) {
            item.isSelected = selected;
            _this.refreshAssociationWithBodyStyleConfigId(item.id, item.isSelected);
        });
        // refresh grid
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
    };
    VehicleToBodyStyleConfigSearchComponent.prototype.refreshAssociationWithBodyStyleConfigId = function (bodyStyleConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToBodyStyleConfigsRetrieved = this.getVehicleToBodyStyleConfigsByBodyStyleConfigId(bodyStyleConfigId);
            //TODO: number of associations which may be useful in add brake association screen?
            var temp = this.vehicleToBodyStyleConfigsForSelectedBodyStyle || [];
            for (var _i = 0, _a = this.vehicleToBodyStyleConfigsRetrieved; _i < _a.length; _i++) {
                var vehicleToBodyStyleConfig = _a[_i];
                temp.push(vehicleToBodyStyleConfig);
            }
            this.vehicleToBodyStyleConfigsForSelectedBodyStyle = temp;
        }
        else {
            var m = this.vehicleToBodyStyleConfigsForSelectedBodyStyle.filter(function (x) { return x.bodyStyleConfig.id != bodyStyleConfigId; });
            this.vehicleToBodyStyleConfigsForSelectedBodyStyle = m;
        }
    };
    VehicleToBodyStyleConfigSearchComponent.prototype.getVehicleToBodyStyleConfigsByBodyStyleConfigId = function (id) {
        return this.vehicleToBodyStyleConfigSearchViewModel.result.vehicleToBodyStyleConfigs.filter(function (v) { return v.bodyStyleConfig.id == id; });
    };
    VehicleToBodyStyleConfigSearchComponent.prototype.onBodyStyleConfigSelected = function (bodyStyleConfig) {
        this.refreshAssociationWithBodyStyleConfigId(bodyStyleConfig.id, !bodyStyleConfig.isSelected);
        if (bodyStyleConfig.isSelected) {
            //unchecked
            this.isSelectAllBodyStyleConfigs = false;
        }
        else {
            //checked
            var excludedBodyStyles = this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.filter(function (item) { return item.id != bodyStyleConfig.id; });
            if (excludedBodyStyles.every(function (item) { return item.isSelected; })) {
                this.isSelectAllBodyStyleConfigs = true;
            }
        }
        // refresh grid
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
    };
    VehicleToBodyStyleConfigSearchComponent.prototype.onNewVehicleToBodyStyleConfig = function () {
        this.sharedService.bodyStyleConfigs = this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.filter(function (item) { return item.isSelected; });
        this.sharedService.vehicleToBodyStyleConfigSearchViewModel = this.vehicleToBodyStyleConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletobodystyleconfig/add"]);
    };
    VehicleToBodyStyleConfigSearchComponent.prototype.onDeleteVehicleToBodyStyleConfig = function (vehicleToBodyStyleConfig) {
        this.deleteVehicleToBodyStyleConfig = vehicleToBodyStyleConfig;
        this.deleteVehicleToBodyStyleConfig.comment = "";
        this.deleteBodyStyleAssociationPopup.open("md");
    };
    VehicleToBodyStyleConfigSearchComponent.prototype.onDeleteVehicleToBodyStyleConfigSubmit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteVehicleToBodyStyleConfig.attachments = uploadedFiles;
            }
            if (_this.deleteVehicleToBodyStyleConfig.attachments) {
                _this.deleteVehicleToBodyStyleConfig.attachments = _this.deleteVehicleToBodyStyleConfig.attachments
                    .concat(_this.acFileUploader.getFilesMarkedToDelete());
            }
            _this.vehicleToBodyStyleConfigService.deleteVehicleToBodyStyleConfig(_this.deleteVehicleToBodyStyleConfig.id, _this.deleteVehicleToBodyStyleConfig)
                .subscribe(function (response) {
                if (response) {
                    _this.deleteBodyStyleAssociationPopup.close();
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Body Style Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBodyStyleConfig.id);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + "Vehicle to body style association body style config id \"" + _this.deleteVehicleToBodyStyleConfig.id + "\" Vehicleid  \"" + _this.deleteVehicleToBodyStyleConfig.vehicleId + "\" with change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body Style Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBodyStyleConfig.id);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                    _this.showLoadingGif = false;
                }
                _this.showLoadingGif = false;
            }, function (error) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Body Style Association", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.deleteVehicleToBodyStyleConfig.id);
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
    VehicleToBodyStyleConfigSearchComponent.prototype.routerLinkRedirect = function (route, id) {
        this.sharedService.vehicleToBodyStyleConfigSearchViewModel = this.vehicleToBodyStyleConfigSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    };
    VehicleToBodyStyleConfigSearchComponent.prototype.onViewBodyStyleConfigCr = function (bodyStyleConfig) {
        this.sharedService.vehicleToBodyStyleConfigSearchViewModel = this.vehicleToBodyStyleConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/bodystyleconfig/" + bodyStyleConfig.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    VehicleToBodyStyleConfigSearchComponent.prototype.onViewAssociatedVehiclesCr = function (associatedVehicle) {
        this.sharedService.vehicleToBodyStyleConfigSearchViewModel = this.vehicleToBodyStyleConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletobodystyleconfig/" + associatedVehicle.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    __decorate([
        core_1.ViewChild("deleteBodyStyleAssociationPopup"), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], VehicleToBodyStyleConfigSearchComponent.prototype, "deleteBodyStyleAssociationPopup", void 0);
    __decorate([
        core_1.ViewChild("acFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], VehicleToBodyStyleConfigSearchComponent.prototype, "acFileUploader", void 0);
    __decorate([
        core_1.ViewChild("bodyStyleConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleToBodyStyleConfigSearchComponent.prototype, "bodyStyleConfigGrid", void 0);
    __decorate([
        core_1.ViewChild("vehicleToBodyStyleConfigGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], VehicleToBodyStyleConfigSearchComponent.prototype, "vehicleToBodyStyleConfigGrid", void 0);
    __decorate([
        core_1.Input("thresholdRecordCount"), 
        __metadata('design:type', Number)
    ], VehicleToBodyStyleConfigSearchComponent.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.Input("vehicleToBodyStyleConfigSearchViewModel"), 
        __metadata('design:type', Object)
    ], VehicleToBodyStyleConfigSearchComponent.prototype, "vehicleToBodyStyleConfigSearchViewModel", void 0);
    __decorate([
        core_1.Input("vehicleToBodyStyleConfigsForSelectedBodyStyle"), 
        __metadata('design:type', Array)
    ], VehicleToBodyStyleConfigSearchComponent.prototype, "vehicleToBodyStyleConfigsForSelectedBodyStyle", void 0);
    VehicleToBodyStyleConfigSearchComponent = __decorate([
        core_1.Component({
            selector: "vehicletobodystyleconfig-search",
            templateUrl: "app/templates/vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-search.component.html",
            providers: [vehicleToBodyStyleConfig_service_1.VehicleToBodyStyleConfigService, shared_service_1.SharedService],
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, vehicleToBodyStyleConfig_service_1.VehicleToBodyStyleConfigService, router_1.Router, ng2_toastr_1.ToastsManager, navigation_service_1.NavigationService])
    ], VehicleToBodyStyleConfigSearchComponent);
    return VehicleToBodyStyleConfigSearchComponent;
}());
exports.VehicleToBodyStyleConfigSearchComponent = VehicleToBodyStyleConfigSearchComponent;
