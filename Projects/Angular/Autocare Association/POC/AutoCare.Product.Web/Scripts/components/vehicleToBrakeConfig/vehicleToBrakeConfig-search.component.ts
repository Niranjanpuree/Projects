import { Component, OnInit, ViewChild, Input,
    DoCheck }                                               from "@angular/core";
import { Router }                        from "@angular/router";
import { ModalComponent }                 from "ng2-bs3-modal/ng2-bs3-modal";
import { IVehicleToBrakeConfig }                            from "../vehicleTobrakeConfig/vehicleTobrakeConfig.model";
import { VehicleToBrakeConfigService }                      from "../vehicleToBrakeConfig/vehicleToBrakeConfig.service";
import { IBrakeConfig }                                     from "../brakeConfig/brakeConfig.model";
import { ToastsManager }                                    from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                                    from "../shared/shared.service";
import { NavigationService }                                from "../shared/navigation.service";
import { ConstantsWarehouse }                               from "../constants-warehouse";
import { IVehicleToBrakeConfigSearchInputModel
    , IVehicleToBrakeConfigSearchViewModel }                from "../vehicleToBrakeConfig/vehicleToBrakeConfig-search.model";
import { AcGridComponent }                                  from "../../lib/aclibs/ac-grid/ac-grid";
import { AcFileUploader }                                   from "../../lib/aclibs/ac-fileuploader/ac-fileuploader";
import { pageChangedEventArg }                              from "../../lib/aclibs/ac-grid/ac-grid.component";

@Component({
    selector: "vehicletobrakeconfig-search",
    templateUrl: "app/templates/vehicletobrakeConfig/vehicletobrakeConfig-search.component.html",
    providers: [VehicleToBrakeConfigService],
})

export class VehicleToBrakeConfigSearchComponent implements OnInit, DoCheck {
    private isHide: boolean = false;
    private vehicleToBrakeConfigsRetrieved: IVehicleToBrakeConfig[] = [];
    private deleteVehicleToBrakeConfig: IVehicleToBrakeConfig;
    private isLeftMenuHidden: boolean = false;
    private activeSubMenu: string = '';
    private activeSubMenuGroup: string = '';
    private isBrakeSystemsExpanded: boolean = true;
    private isAssociatedVehiclesExpanded: boolean = true;
    private isSystemsMenuExpanded: boolean = true;
    private isChildClicked: boolean = false;
    private isMenuExpanded: boolean = true;
    private showLoadingGif: boolean = false;
    private makeQuery: string;
    private modelQuery: string;
    private subModelQuery: string;
    private isSelectAllBrakeSystems: boolean;
    private previousBrakeConfigs: any;

    @ViewChild('deleteBrakeAssociationPopup') deleteBrakeAssociationPopup: ModalComponent;
    @ViewChild(AcFileUploader) acFileUploader: AcFileUploader;
    @ViewChild("brakeConfigGrid") brakeConfigGrid: AcGridComponent;
    @ViewChild("vehicleToBrakeConfigGrid") vehicleToBrakeConfigGrid: AcGridComponent;

    @Input("thresholdRecordCount") thresholdRecordCount: number;
    @Input("vehicleToBrakeConfigSearchViewModel") vehicleToBrakeConfigSearchViewModel: IVehicleToBrakeConfigSearchViewModel;
    @Input("vehicleToBrakeConfigsForSelectedBrake") vehicleToBrakeConfigsForSelectedBrake: IVehicleToBrakeConfig[];

    constructor(private sharedService: SharedService, private vehicleToBrakeConfigService: VehicleToBrakeConfigService,
        private router: Router, private toastr: ToastsManager, private navigationService: NavigationService) {
    }

    ngOnInit() {
        this.sharedService.vehicles = null;        //clear old selections
        this.sharedService.brakeConfigs = null;    //clear old selections

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
    }

    ngDoCheck() {
        if (this.previousBrakeConfigs != this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs) {
            this.isSelectAllBrakeSystems = false;
            if (this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.length > 0) {
                if (this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.every(item => item.isSelected))
                    this.isSelectAllBrakeSystems = true;
                this.previousBrakeConfigs = this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs;
            }
        }
    }

    refreshGrids() {
        if (this.brakeConfigGrid)
            this.brakeConfigGrid.refresh();
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
    }

    private onSelectAllBrakeConfig(selected: boolean) {
        this.isSelectAllBrakeSystems = selected;
        if (this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs == null) {
            return;
        }
        this.vehicleToBrakeConfigsForSelectedBrake = [];
        this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.forEach(item => {
            item.isSelected = selected;
            this.refreshAssociationWithBrakeConfigId(item.id, item.isSelected);
        });

        // refresh grid
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
    }

    private onBrakeConfigSelected(brakeConfig: IBrakeConfig) {
        this.refreshAssociationWithBrakeConfigId(brakeConfig.id, !brakeConfig.isSelected);

        if (brakeConfig.isSelected) {
            //unchecked
            this.isSelectAllBrakeSystems = false;
        }
        else {
            //checked
            var excludedBrakeSystem = this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.filter(item => item.id != brakeConfig.id);
            if (excludedBrakeSystem.every(item => item.isSelected)) {
                this.isSelectAllBrakeSystems = true;
            }
        }

        // refresh grid
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
    }

    private refreshAssociationWithBrakeConfigId(brakeConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToBrakeConfigsRetrieved = this.getVehicleToBrakeConfigsByBrakeConfigId(brakeConfigId);
            //TODO: number of associations which may be useful in add brake association screen?
            let temp = this.vehicleToBrakeConfigsForSelectedBrake || [];
            for (var vehicleToBrakeConfig of this.vehicleToBrakeConfigsRetrieved) {
                temp.push(vehicleToBrakeConfig);
            }
            this.vehicleToBrakeConfigsForSelectedBrake = temp;
        }
        else {
            let m = this.vehicleToBrakeConfigsForSelectedBrake.filter(x => x.brakeConfig.id != brakeConfigId);
            this.vehicleToBrakeConfigsForSelectedBrake = m;
        }
    }

    private getVehicleToBrakeConfigsByBrakeConfigId(id) {
        return this.vehicleToBrakeConfigSearchViewModel.result.vehicleToBrakeConfigs.filter(v => v.brakeConfig.id == id);
    }

    private onSelectedNewBrakeAssociation() {
        this.sharedService.brakeConfigs = this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.filter(item => item.isSelected);
        this.sharedService.vehicleToBrakeConfigSearchViewModel = this.vehicleToBrakeConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletobrakeconfig/add"]);
    }

    private onDeleteVehicleToBrakeConfig(vehicleToBrakeConfig: IVehicleToBrakeConfig) {
        this.deleteVehicleToBrakeConfig = vehicleToBrakeConfig;
        this.deleteVehicleToBrakeConfig.comment = "";
        this.deleteBrakeAssociationPopup.open("md");
    }

    private onDeleteVehicleToBrakeConfigSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteVehicleToBrakeConfig.attachments = uploadedFiles;
            }
            if (this.deleteVehicleToBrakeConfig.attachments) {
                this.deleteVehicleToBrakeConfig.attachments = this.deleteVehicleToBrakeConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleToBrakeConfigService.deleteVehicleToBrakeConfig(this.deleteVehicleToBrakeConfig.id, this.deleteVehicleToBrakeConfig).subscribe(response => {
                if (response) {
                    this.deleteBrakeAssociationPopup.close();

                    let successMessage = ConstantsWarehouse.notificationMessage.success("Brake Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBrakeConfig.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + "Vehicle to brake association brake config id \"" + this.deleteVehicleToBrakeConfig.id + "\" Vehicleid  \"" + this.deleteVehicleToBrakeConfig.vehicleId + "\" with change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBrakeConfig.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                    this.showLoadingGif = false;
                }
                this.showLoadingGif = false;
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBrakeConfig.id);
                this.toastr.warning(errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.acFileUploader.reset();
                this.showLoadingGif = false;
            });
        }, error => {
            this.acFileUploader.reset();
            this.showLoadingGif = false;
        });
    }

    private routerLinkRedirect(route: string, id: number) {
        this.sharedService.vehicleToBrakeConfigSearchViewModel = this.vehicleToBrakeConfigSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    }

    private onViewBrakeConfigCr(brakeConfigVm: IBrakeConfig) {
        this.sharedService.vehicleToBrakeConfigSearchViewModel = this.vehicleToBrakeConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/brakeconfig/" + brakeConfigVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    private onViewAssociatedVehiclesCr(associatedVehicleVm: IVehicleToBrakeConfig) {
        this.sharedService.vehicleToBrakeConfigSearchViewModel = this.vehicleToBrakeConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletobrakeconfig/" + associatedVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }
}

