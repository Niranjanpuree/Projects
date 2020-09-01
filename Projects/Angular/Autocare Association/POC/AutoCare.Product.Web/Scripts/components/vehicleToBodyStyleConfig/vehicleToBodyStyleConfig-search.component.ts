import { Component, OnInit, ViewChild, Input,
    DoCheck }                                               from "@angular/core";
import { Router }                        from "@angular/router";
import { ModalComponent }                 from "ng2-bs3-modal/ng2-bs3-modal";
import { AcGridComponent }                                  from "../../lib/aclibs/ac-grid/ac-grid";
import { AcFileUploader }                                   from "../../lib/aclibs/ac-fileuploader/ac-fileuploader";
import { ToastsManager }                                    from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                                    from "../shared/shared.service";
import { NavigationService }                                from "../shared/navigation.service";
import { ConstantsWarehouse }                               from "../constants-warehouse";
import { VehicleToBodyStyleConfigSearchPanel }              from "./vehicleToBodyStyleConfig-searchPanel.component";
import { IVehicleToBodyStyleConfigSearchInputModel,
    IVehicleToBodyStyleConfigSearchViewModel }              from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-search.model";
import { VehicleToBodyStyleConfigService }                  from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.service";
import { IVehicleToBodyStyleConfig }                        from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.model";
import { IBodyStyleConfig }                                 from "../BodyStyleConfig/BodyStyleConfig.model";

@Component({
    selector: "vehicletobodystyleconfig-search",
    templateUrl: "app/templates/vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-search.component.html",
    providers: [VehicleToBodyStyleConfigService, SharedService],
})

export class VehicleToBodyStyleConfigSearchComponent implements OnInit, DoCheck {
    private isHide: boolean = false;
    private vehicleToBodyStyleConfigsRetrieved: IVehicleToBodyStyleConfig[] = [];
    private deleteVehicleToBodyStyleConfig: IVehicleToBodyStyleConfig;
    private isLeftMenuHidden: boolean = false;
    private activeSubMenu: string = '';
    private activeSubMenuGroup: string = '';
    private isBodyStyleConfigExpanded: boolean = true;
    private isAssociatedVehiclesExpanded: boolean = true;
    private isChildClicked: boolean = false;
    private isMenuExpanded: boolean = true;
    private showLoadingGif: boolean = false;
    private makeQuery: string;
    private modelQuery: string;
    private subModelQuery: string;
    private isSelectAllBodyStyleConfigs: boolean;
    private previousBodyStyleConfigs: any;

    @ViewChild("deleteBodyStyleAssociationPopup") deleteBodyStyleAssociationPopup: ModalComponent;
    @ViewChild("acFileUploader") acFileUploader: AcFileUploader;
    @ViewChild("bodyStyleConfigGrid") bodyStyleConfigGrid: AcGridComponent;
    @ViewChild("vehicleToBodyStyleConfigGrid") vehicleToBodyStyleConfigGrid: AcGridComponent;

    @Input("thresholdRecordCount") thresholdRecordCount: number;
    @Input("vehicleToBodyStyleConfigSearchViewModel") vehicleToBodyStyleConfigSearchViewModel: IVehicleToBodyStyleConfigSearchViewModel;
    @Input("vehicleToBodyStyleConfigsForSelectedBodyStyle") vehicleToBodyStyleConfigsForSelectedBodyStyle: IVehicleToBodyStyleConfig[];

    constructor(private sharedService: SharedService, private vehicleToBodyStyleConfigService: VehicleToBodyStyleConfigService,
        private router: Router, private toastr: ToastsManager, private navigationService: NavigationService) {
    }

    ngOnInit() {
        this.sharedService.vehicles = null;        //clear old selections
        //this.sharedService.bodyStyleConfigs = null;    //clear old selections

        // Drawer right start
        var headerht = $('header').innerHeight();
        var navht = $('nav').innerHeight();
        var winht = $(window).height();
        var winwt = 960;
        $(".drawer-left").css('min-height', winht - headerht - navht);
        $(".drawer-left").css('width', winwt);
        $(document).on('click', '.drawer-show', event => {
            $(".drawer-left").css('width', winwt);
        });
        $(".drawer-left span").on('click', () => {

            var drawerwt = $(".drawer-left").width();

            if (drawerwt == 15) {
                $(".drawer-left").css('width', winwt);
            }
            else {
                $(".drawer-left").css('width', 15);
            }
        });
        $(document).on('click', event => {
            if (!$(event.target).closest(".drawer-left").length) {
                // Hide the menus.
                var drawerwt = $(".drawer-left").width();
                if (drawerwt > 15) {
                    $(".drawer-left").css('width', 15);
                }
            }
        });
    }

    ngDoCheck() {
        if (this.previousBodyStyleConfigs != this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs) {
            this.isSelectAllBodyStyleConfigs = false;
            if (this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.length > 0) {
                if (this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.every(item => item.isSelected))
                    this.isSelectAllBodyStyleConfigs = true;
                this.previousBodyStyleConfigs = this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs;
            }
        }
    }

    refreshGrids() {
        if (this.bodyStyleConfigGrid)
            this.bodyStyleConfigGrid.refresh();
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
    }

    private onSelectAllBodyStyleConfigs(selected: boolean) {
        this.isSelectAllBodyStyleConfigs = selected;
        if (this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs == null) {
            return;
        }

        this.vehicleToBodyStyleConfigsForSelectedBodyStyle = [];

        this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.forEach(item => {
            item.isSelected = selected;
            this.refreshAssociationWithBodyStyleConfigId(item.id, item.isSelected);
        });

        // refresh grid
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
    }

    private refreshAssociationWithBodyStyleConfigId(bodyStyleConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToBodyStyleConfigsRetrieved = this.getVehicleToBodyStyleConfigsByBodyStyleConfigId(bodyStyleConfigId);
            //TODO: number of associations which may be useful in add brake association screen?
            let temp = this.vehicleToBodyStyleConfigsForSelectedBodyStyle || [];
            for (var vehicleToBodyStyleConfig of this.vehicleToBodyStyleConfigsRetrieved) {
                temp.push(vehicleToBodyStyleConfig);
            }
            this.vehicleToBodyStyleConfigsForSelectedBodyStyle = temp;
        }
        else {
            let m = this.vehicleToBodyStyleConfigsForSelectedBodyStyle.filter(x => x.bodyStyleConfig.id != bodyStyleConfigId);
            this.vehicleToBodyStyleConfigsForSelectedBodyStyle = m;
        }
    }

    private getVehicleToBodyStyleConfigsByBodyStyleConfigId(id) {
        return this.vehicleToBodyStyleConfigSearchViewModel.result.vehicleToBodyStyleConfigs.filter(v => v.bodyStyleConfig.id == id);
    }

    private onBodyStyleConfigSelected(bodyStyleConfig: IBodyStyleConfig) {
        this.refreshAssociationWithBodyStyleConfigId(bodyStyleConfig.id, !bodyStyleConfig.isSelected);
        if (bodyStyleConfig.isSelected) {
            //unchecked
            this.isSelectAllBodyStyleConfigs = false;
        } else {
            //checked
            var excludedBodyStyles = this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.filter(item => item.id != bodyStyleConfig.id);
            if (excludedBodyStyles.every(item => item.isSelected)) {
                this.isSelectAllBodyStyleConfigs = true;
            }
        }

        // refresh grid
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
    }

    private onNewVehicleToBodyStyleConfig() {
        this.sharedService.bodyStyleConfigs = this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.filter(item => item.isSelected);
        this.sharedService.vehicleToBodyStyleConfigSearchViewModel = this.vehicleToBodyStyleConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletobodystyleconfig/add"]);
    }

    private onDeleteVehicleToBodyStyleConfig(vehicleToBodyStyleConfig: IVehicleToBodyStyleConfig) {
        this.deleteVehicleToBodyStyleConfig = vehicleToBodyStyleConfig;
        this.deleteVehicleToBodyStyleConfig.comment = "";
        this.deleteBodyStyleAssociationPopup.open("md");
    }

    private onDeleteVehicleToBodyStyleConfigSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteVehicleToBodyStyleConfig.attachments = uploadedFiles;
            }
            if (this.deleteVehicleToBodyStyleConfig.attachments) {
                this.deleteVehicleToBodyStyleConfig.attachments = this.deleteVehicleToBodyStyleConfig.attachments
                    .concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleToBodyStyleConfigService.deleteVehicleToBodyStyleConfig(this.deleteVehicleToBodyStyleConfig.id, this.deleteVehicleToBodyStyleConfig)
                .subscribe(response => {
                    if (response) {
                        this.deleteBodyStyleAssociationPopup.close();
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Body Style Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBodyStyleConfig.id);
                        successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + "Vehicle to body style association body style config id \"" + this.deleteVehicleToBodyStyleConfig.id + "\" Vehicleid  \"" + this.deleteVehicleToBodyStyleConfig.vehicleId + "\" with change requestid  \"" + response + "\" will be reviewed.";
                        this.toastr.success(successMessage.body, successMessage.title);
                    }
                    else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Body Style Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBodyStyleConfig.id);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                        this.showLoadingGif = false;
                    }
                    this.showLoadingGif = false;
                }, error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Body Style Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBodyStyleConfig.id);
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
        this.sharedService.vehicleToBodyStyleConfigSearchViewModel = this.vehicleToBodyStyleConfigSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    }

    private onViewBodyStyleConfigCr(bodyStyleConfig: IBodyStyleConfig) {
        this.sharedService.vehicleToBodyStyleConfigSearchViewModel = this.vehicleToBodyStyleConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/bodystyleconfig/" + bodyStyleConfig.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    private onViewAssociatedVehiclesCr(associatedVehicle: IVehicleToBodyStyleConfig) {
        this.sharedService.vehicleToBodyStyleConfigSearchViewModel = this.vehicleToBodyStyleConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletobodystyleconfig/" + associatedVehicle.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }
}