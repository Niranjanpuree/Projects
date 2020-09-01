import { Component, OnInit, ViewChild, Input,
    DoCheck }                                   from "@angular/core";
import { Router }            from "@angular/router";
import { ModalComponent }     from "ng2-bs3-modal/ng2-bs3-modal";
import { IVehicleToBedConfig }                  from "../vehicleTobedConfig/vehicleTobedConfig.model";
import { VehicleToBedConfigService }            from "../vehicleToBedConfig/vehicleToBedConfig.service";
import { IBedConfig }                           from "../bedConfig/bedConfig.model";
import { ToastsManager }                        from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                        from "../shared/shared.service";
import { NavigationService }                    from "../shared/navigation.service";
import { ConstantsWarehouse }                   from "../constants-warehouse";
import { IVehicleToBedConfigSearchInputModel
    , IVehicleToBedConfigSearchViewModel
    , SearchType }                              from "../vehicleToBedConfig/vehicleToBedConfig-search.model";
import { AcGridComponent }                      from '../../lib/aclibs/ac-grid/ac-grid';
import { AcFileUploader }                       from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { pageChangedEventArg }                  from "../../lib/aclibs/ac-grid/ac-grid.component";  //pushkar: remove if not needed

@Component({
    selector: "vehicletobedconfig-search",
    templateUrl: "app/templates/vehicletobedConfig/vehicletobedConfig-search.component.html",
    providers: [VehicleToBedConfigService, SharedService]
})

export class VehicleToBedConfigSearchComponent implements OnInit, DoCheck {
    private isHide: boolean = false;
    private vehicleToBedConfigSearchInputModel: IVehicleToBedConfigSearchInputModel;
    private vehicleToBedConfigsRetrieved: IVehicleToBedConfig[] = [];
    private deleteVehicleToBedConfig: IVehicleToBedConfig;
    private isLeftMenuHidden: boolean = false;
    private activeSubMenu: string = '';
    private activeSubMenuGroup: string = '';
    private isBedSystemsExpanded: boolean = true;
    private isAssociatedVehiclesExpanded: boolean = true;
    private isSystemsMenuExpanded: boolean = true;
    private isChildClicked: boolean = false;
    private isMenuExpanded: boolean = true;
    private showLoadingGif: boolean = false;
    private makeQuery: string;
    private modelQuery: string;
    private subModelQuery: string;
    private isSelectAllBedConfigs: boolean;
    private previousBedConfigs: any;

    @ViewChild('deleteBedAssociationPopup') deleteBedAssociationPopup: ModalComponent;
    @ViewChild(AcFileUploader) acFileUploader: AcFileUploader;
    @ViewChild("bedConfigGrid") bedConfigGrid: AcGridComponent;
    @ViewChild("vehicleToBedConfigGrid") vehicleToBedConfigGrid: AcGridComponent;

    @Input("thresholdRecordCount") thresholdRecordCount: number;
    @Input("vehicleToBedConfigSearchViewModel") vehicleToBedConfigSearchViewModel: IVehicleToBedConfigSearchViewModel;
    @Input("vehicleToBedConfigsForSelectedBed") vehicleToBedConfigsForSelectedBed: IVehicleToBedConfig[];

    constructor(private sharedService: SharedService, private vehicleToBedConfigService: VehicleToBedConfigService
        , private router: Router, private toastr: ToastsManager, private navigationService: NavigationService) {
    }

    ngOnInit() {
        this.sharedService.vehicles = null;        //clear old selections
        this.sharedService.bedConfigs = null;    //clear old selections

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
        if (this.previousBedConfigs != this.vehicleToBedConfigSearchViewModel.result.bedConfigs) {
            this.isSelectAllBedConfigs = false;
            if (this.vehicleToBedConfigSearchViewModel.result.bedConfigs.length > 0) {
                if (this.vehicleToBedConfigSearchViewModel.result.bedConfigs.every(item => item.isSelected))
                    this.isSelectAllBedConfigs = true;
                this.previousBedConfigs = this.vehicleToBedConfigSearchViewModel.result.bedConfigs;
            }
        }
    }

    refreshGrids() {
        if (this.bedConfigGrid)
            this.bedConfigGrid.refresh();
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
    }

    private onSelectAllBedConfig(selected: boolean) {
        this.isSelectAllBedConfigs = selected;
        if (this.vehicleToBedConfigSearchViewModel.result.bedConfigs == null) {
            return;
        }

        this.vehicleToBedConfigsForSelectedBed = [];

        // get vehicle to bed config of base vehicle
        this.vehicleToBedConfigSearchViewModel.result.bedConfigs.forEach(item => {
            item.isSelected = selected;
            this.refreshAssociationWithBedConfigId(item.id, item.isSelected);
        });

        // refresh vehicleToBedConfig grid.
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
    }

    private onBedConfigSelected(bedConfig: IBedConfig) {
        this.refreshAssociationWithBedConfigId(bedConfig.id, !bedConfig.isSelected);
        if (bedConfig.isSelected) {
            //unchecked
            this.isSelectAllBedConfigs = false;
        }
        else {
            //checked
            var excludedBodyStyles = this.vehicleToBedConfigSearchViewModel.result.bedConfigs.filter(item => item.id != bedConfig.id);
            if (excludedBodyStyles.every(item => item.isSelected)) {
                this.isSelectAllBedConfigs = true;
            }
        }

        // refresh vehicle to bed config grid.
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
    }

    private refreshAssociationWithBedConfigId(bedConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToBedConfigsRetrieved = this.getVehicleToBedConfigsByBedConfigId(bedConfigId);
            //TODO: number of associations which may be useful in add bed association screen?
            let temp = this.vehicleToBedConfigsForSelectedBed || [];
            for (var vehicleToBedConfig of this.vehicleToBedConfigsRetrieved) {
                temp.push(vehicleToBedConfig);
            }
            this.vehicleToBedConfigsForSelectedBed = temp;
          }
        else {
            let m = this.vehicleToBedConfigsForSelectedBed.filter(x => x.bedConfig.id != bedConfigId);
            this.vehicleToBedConfigsForSelectedBed = m;
            
        }
    }

    private getVehicleToBedConfigsByBedConfigId(id) {
        return this.vehicleToBedConfigSearchViewModel.result.vehicleToBedConfigs.filter(v => v.bedConfig.id == id);
    }

    private onSelectedNewBedAssociation() {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletobedconfig/add"]);
    }

    private onDeleteVehicleToBedConfig(vehicleToBedConfig: IVehicleToBedConfig) {
        this.deleteVehicleToBedConfig = vehicleToBedConfig;
        this.deleteVehicleToBedConfig.comment = "";
        this.deleteBedAssociationPopup.open("md");
    }

    private onDeleteVehicleToBedConfigSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteVehicleToBedConfig.attachments = uploadedFiles;
            }
            if (this.deleteVehicleToBedConfig.attachments) {
                this.deleteVehicleToBedConfig.attachments = this.deleteVehicleToBedConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleToBedConfigService.deleteVehicleToBedConfig(this.deleteVehicleToBedConfig.id, this.deleteVehicleToBedConfig).subscribe(response => {
                if (response) {
                    this.deleteBedAssociationPopup.close();

                    let successMessage = ConstantsWarehouse.notificationMessage.success("Bed Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBedConfig.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + "Vehicle to bed association bed config id \"" + this.deleteVehicleToBedConfig.id + "\" Vehicleid  \"" + this.deleteVehicleToBedConfig.vehicleId + "\" with change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBedConfig.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                    this.showLoadingGif = false;
                }
                this.showLoadingGif = false;
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBedConfig.id);
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
        this.sharedService.vehicleToBedConfigSearchViewModel = this.vehicleToBedConfigSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    }

    private onViewBedConfigCr(bedConfigVm: IBedConfig) {
        this.sharedService.vehicleToBedConfigSearchViewModel = this.vehicleToBedConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/bedconfig/" + bedConfigVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    private onViewAssociatedVehiclesCr(associatedVehicleVm: IVehicleToBedConfig) {
        this.sharedService.vehicleToBedConfigSearchViewModel = this.vehicleToBedConfigSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletobedconfig/" + associatedVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }
}
