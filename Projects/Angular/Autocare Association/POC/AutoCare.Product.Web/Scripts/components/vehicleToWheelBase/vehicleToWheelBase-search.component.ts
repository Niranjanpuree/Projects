import { Component, OnInit, ViewChild, Input,
    DoCheck }                                               from "@angular/core";
import { Router }                        from "@angular/router";
import { ModalComponent }                 from "ng2-bs3-modal/ng2-bs3-modal";
import { IVehicleToWheelBase }                            from "../vehicleToWheelBase/vehicleToWheelBase.model";
import { VehicleToWheelBaseService }                      from "../vehicleToWheelBase/vehicleToWheelBase.service";
import { IWheelBase }                                     from "../wheelbase/wheelbase.model";
import { ToastsManager }                                    from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                                    from "../shared/shared.service";
import { NavigationService }                                from "../shared/navigation.service";
import { ConstantsWarehouse }                               from "../constants-warehouse";
import { IVehicleToWheelBaseSearchInputModel
    , IVehicleToWheelBaseSearchViewModel }                from "../vehicleToWheelBase/vehicleToWheelBase-search.model";
import { AcGridComponent }                                  from "../../lib/aclibs/ac-grid/ac-grid";
import { AcFileUploader }                                   from "../../lib/aclibs/ac-fileuploader/ac-fileuploader";
import { pageChangedEventArg }                              from "../../lib/aclibs/ac-grid/ac-grid.component";
import { WheelBaseService }                               from "../wheelBase/wheelBase.Service";

@Component({
    selector: "vehicletowheelbase-search",
    templateUrl: "app/templates/vehicleToWheelBase/vehicleToWheelBase-search.component.html",
    providers: [VehicleToWheelBaseService, WheelBaseService],
})

export class VehicleToWheelBaseSearchComponent implements OnInit, DoCheck {
    private isHide: boolean = false;
    private vehicleToWheelBaseRetrieved: IVehicleToWheelBase[] = [];
    private deleteVehicleToWheelBase: IVehicleToWheelBase;
    private isLeftMenuHidden: boolean = false;
    private activeSubMenu: string = '';
    private activeSubMenuGroup: string = '';
    private isWheelBaseSystemsExpanded: boolean = true;
    private isAssociatedVehiclesExpanded: boolean = true;
    private isSystemsMenuExpanded: boolean = true;
    private isChildClicked: boolean = false;
    private isMenuExpanded: boolean = true;
    private showLoadingGif: boolean = false;
    private makeQuery: string;
    private modelQuery: string;
    private subModelQuery: string;
    private isSelectAllWheelBaseSystems: boolean;
    private previousWheelBase: any;
    wheelBase: IWheelBase = {};
    modifiedWheelBase: IWheelBase = {};
    wheelBases: IWheelBase[];

    @ViewChild('deleteWheelBaseAssociationPopup')
    deleteWheelBaseAssociationPopup: ModalComponent;

    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;

    @ViewChild("wheelBaseConfigGrid")
    wheelBaseGrid: AcGridComponent;

    @ViewChild("vehicleToWheelBaseGrid")
    vehicleToWheelBaseGrid: AcGridComponent;

    @ViewChild('newPopup')
    newPopup: ModalComponent;

    @ViewChild('modifyPopup')
    modifyPopup: ModalComponent;

    @ViewChild('deleteErrorPopup')
    deleteErrorPopup: ModalComponent;

    @ViewChild('deleteConfirmPopup')
    deleteConfirmPopup: ModalComponent;

    @ViewChild("modifyPopupAcFileUploader")
    modifyPopupAcFileUploader: AcFileUploader;


    @ViewChild("deletePopupAcFileUploader")
    deletePopupAcFileUploader: AcFileUploader;

    @Input("thresholdRecordCount") thresholdRecordCount: number;
    @Input("vehicleToWheelBaseSearchViewModel") vehicleToWheelBaseSearchViewModel: IVehicleToWheelBaseSearchViewModel;
    @Input("vehicleToWheelBaseForSelectedWheelBase") vehicleToWheelBaseForSelectedWheelBase: IVehicleToWheelBase[];

    constructor(private sharedService: SharedService, private wheelBaseService: WheelBaseService, private vehicleToWheelBaseService: VehicleToWheelBaseService,
        private router: Router, private toastr: ToastsManager, private navigationService: NavigationService) {
    }

    ngOnInit() {
        this.sharedService.vehicles = null;        //clear old selections
        this.sharedService.wheelBases = null;    //clear old selections

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

   
    limittoonedecimal(userinput: any,modify:string) {
        if (!isNaN(userinput.target.value)){
            var input = userinput.target.value;
            if (!isNaN(input)) {
                if (input > 0 || input != null) {
                    let myvalue = (input * 2.54).toString();
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
       
    }

    onNew() {
        this.wheelBase = {};
        this.newPopup.open("md");
    }
    validationCheck(item: IWheelBase): boolean {
        let isValid = true;
        if (!item.base) {
            this.toastr.warning("Wheel Base  is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        if (!item.wheelBaseMetric) {
            this.toastr.warning("Wheel Base Metric is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }

        return isValid;
    }


    onNewSubmit() {

        if (!this.validationCheck(this.wheelBase)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.wheelBase.attachments = uploadedFiles;
            }
            if (this.wheelBase.attachments) {
                this.wheelBase.attachments = this.wheelBase.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.wheelBaseService.addWheelBase(this.wheelBase).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("WheelBase Name", ConstantsWarehouse.changeRequestType.add, this.wheelBase.base);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the WheelBase  of name \"" + this.wheelBase.base + "\" and WheelBase Metric \"" + this.wheelBase.wheelBaseMetric + "\" Wheel Base change request Id  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Wheel Base", ConstantsWarehouse.changeRequestType.add, this.wheelBase.base);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Wheel Base", ConstantsWarehouse.changeRequestType.add, this.wheelBase.base);
                this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.showLoadingGif = false;
                this.acFileUploader.reset(true);
                this.newPopup.close();
            });

        }, error => {
            this.showLoadingGif = false;
            this.acFileUploader.reset(true);
            this.newPopup.close();

        });
    }

    onModify(wheelBase: IWheelBase) {
        this.wheelBase = wheelBase;
        this.wheelBase.base = this.wheelBase.base.trim();
        this.wheelBase.wheelBaseMetric = this.wheelBase.wheelBaseMetric.trim();
        this.showLoadingGif = true;
        if (!wheelBase.vehicleToWheelBaseCount) {
            this.wheelBaseService.getWheelBaseDetail(wheelBase.id).subscribe(m => {
                this.wheelBase.vehicleToWheelBaseCount = m.vehicleToWheelBaseCount;
                this.modifiedWheelBase = <IWheelBase>JSON.parse(JSON.stringify(wheelBase));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedWheelBase = <IWheelBase>JSON.parse(JSON.stringify(wheelBase));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedWheelBase)) {
            return;
        }
        else if (this.modifiedWheelBase.base == this.wheelBase.base && this.modifiedWheelBase.wheelBaseMetric == this.wheelBase.wheelBaseMetric) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedWheelBase.attachments = uploadedFiles;
            }
            if (this.modifiedWheelBase.attachments) {
                this.modifiedWheelBase.attachments = this.modifiedWheelBase.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.wheelBaseService.updateWheelBase(this.modifiedWheelBase.id, this.modifiedWheelBase).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Wheel Base", ConstantsWarehouse.changeRequestType.modify, this.wheelBase.base);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the Wheel Base of length \"" + this.wheelBase.base + "\" and Wheel Base metric \"" + this.wheelBase.wheelBaseMetric + "\" Wheel Base change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.vehicleToWheelBaseSearchViewModel.result.wheelBases.filter(x => x.id == this.modifiedWheelBase.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Wheel Base", ConstantsWarehouse.changeRequestType.modify, this.wheelBase.base);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Wheel Base", ConstantsWarehouse.changeRequestType.modify, this.wheelBase.base);
                this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.showLoadingGif = false;
                this.modifyPopupAcFileUploader.reset(true);
                this.modifyPopup.close();
            });

        }, error => {
            this.showLoadingGif = false;
            this.modifyPopupAcFileUploader.reset(true);
            this.modifyPopup.close();
        });
    }

    onDelete(wheelBase: IWheelBase) {
        this.wheelBase = wheelBase;
        this.showLoadingGif = true;
        if (!wheelBase.vehicleToWheelBaseCount) {
            this.wheelBaseService.getWheelBaseDetail(wheelBase.id).subscribe(m => {
                wheelBase.vehicleToWheelBaseCount = m.vehicleToWheelBaseCount;
                this.wheelBase.vehicleToWheelBaseCount = wheelBase.vehicleToWheelBaseCount;
                if (wheelBase.vehicleToWheelBaseCount > 0) {
                    this.showLoadingGif = false;
                    this.deleteErrorPopup.open("md");
                }
                else {
                    this.showLoadingGif = false;
                    this.deleteConfirmPopup.open("md");
                }
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
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
    }

    onDeleteConfirm() {
        if (!this.validationCheck(this.wheelBase)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.wheelBase.attachments = uploadedFiles;
            }
            if (this.wheelBase.attachments) {
                this.wheelBase.attachments = this.wheelBase.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.wheelBaseService.deleteWheelBase(this.wheelBase.id, this.wheelBase).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Wheel Base", ConstantsWarehouse.changeRequestType.remove, this.wheelBase.base);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the Wheel Base of Base Length \"" + this.wheelBase.base + "\" and Wheel Base metric \"" + this.wheelBase.wheelBaseMetric + "\" Wheel Base change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.vehicleToWheelBaseSearchViewModel.result.wheelBases.filter(x => x.id == this.wheelBase.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Wheel Base", ConstantsWarehouse.changeRequestType.remove, this.wheelBase.base);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Wheel Base", ConstantsWarehouse.changeRequestType.remove, this.wheelBase.base);
                this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.showLoadingGif = false;
                this.deletePopupAcFileUploader.reset(true);
                this.deleteConfirmPopup.close();

            });

        }, error => {
            this.showLoadingGif = false;
            this.deletePopupAcFileUploader.reset(true);
            this.deleteConfirmPopup.close();

        });
    }
    onCancel(action: string) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
        this.deleteWheelBaseAssociationPopup.close();
    }
    ngDoCheck() {
        if (this.previousWheelBase != this.vehicleToWheelBaseSearchViewModel.result.wheelBases) {
            this.isSelectAllWheelBaseSystems = false;
            if (this.vehicleToWheelBaseSearchViewModel.result.wheelBases.length > 0) {
                if (this.vehicleToWheelBaseSearchViewModel.result.wheelBases.every(item => item.isSelected))
                    this.isSelectAllWheelBaseSystems = true;
                this.previousWheelBase = this.vehicleToWheelBaseSearchViewModel.result.wheelBases;
            }
        }
    }

    refreshGrids() {
        if (this.wheelBaseGrid)
            this.wheelBaseGrid.refresh();
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
    }

    view(wheelBaseVm: IWheelBase) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/wheelbase/" + wheelBaseVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    private onSelectAllWheelBase(selected: boolean) {
        this.isSelectAllWheelBaseSystems = selected;
        if (this.vehicleToWheelBaseSearchViewModel.result.wheelBases == null) {
            return;
        }
        this.vehicleToWheelBaseForSelectedWheelBase = [];
        this.vehicleToWheelBaseSearchViewModel.result.wheelBases.forEach(item => {
            item.isSelected = selected;
            this.refreshAssociationWithWheelBaseId(item.id, item.isSelected);
        });

        // refresh grid
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
    }

    private onWheelBaseSelected(wheelBase: IWheelBase) {
          this.refreshAssociationWithWheelBaseId(wheelBase.id, !wheelBase.isSelected);

        if (wheelBase.isSelected) {
            //unchecked
            this.isSelectAllWheelBaseSystems = false;
        }
        else {
            //checked
            var excludedWheelBaseSystem = this.vehicleToWheelBaseSearchViewModel.result.wheelBases.filter(item => item.id != wheelBase.id);
            if (excludedWheelBaseSystem.every(item => item.isSelected)) {
                this.isSelectAllWheelBaseSystems = true;
            }
        }

        // refresh grid
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
    }

    private refreshAssociationWithWheelBaseId(wheelBaseId, isSelected) {
        if (isSelected) {
            this.vehicleToWheelBaseRetrieved = this.getVehicleToWheelBaseByWheelBaseId(wheelBaseId);
            //TODO: number of associations which may be useful in add WheelBase association screen?
            let temp = this.vehicleToWheelBaseForSelectedWheelBase || [];
            for (var vehicleToWheelBase of this.vehicleToWheelBaseRetrieved) {
                temp.push(vehicleToWheelBase);
            }
            this.vehicleToWheelBaseForSelectedWheelBase = temp;
        }
        else {
            let m = this.vehicleToWheelBaseForSelectedWheelBase.filter(x => x.wheelBaseId != wheelBaseId);
            this.vehicleToWheelBaseForSelectedWheelBase = m;
        }
    }

    private getVehicleToWheelBaseByWheelBaseId(id) {
        return this.vehicleToWheelBaseSearchViewModel.result.vehicleToWheelBases.filter(v => v.wheelBaseId == id);
    }

    private onSelectedNewWheelBaseAssociation() {
        this.sharedService.wheelBases = this.vehicleToWheelBaseSearchViewModel.result.wheelBases.filter(item => item.isSelected);
        this.sharedService.vehicleToWheelBaseSearchViewModel = this.vehicleToWheelBaseSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletowheelbase/add"]);
    }

    private onDeleteVehicleToWheelBase(vehicleToWheelBase: IVehicleToWheelBase) {
        this.deleteVehicleToWheelBase = vehicleToWheelBase;
        this.deleteVehicleToWheelBase.comment = "";
        this.deleteWheelBaseAssociationPopup.open("md");
    }

    private onDeleteVehicleToWheelBaseSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteVehicleToWheelBase.attachments = uploadedFiles;
            }
            if (this.deleteVehicleToWheelBase.attachments) {
                this.deleteVehicleToWheelBase.attachments = this.deleteVehicleToWheelBase.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleToWheelBaseService.deleteVehicleToWheelBase(this.deleteVehicleToWheelBase.id, this.deleteVehicleToWheelBase).subscribe(response => {
                if (response) {
                    this.deleteWheelBaseAssociationPopup.close();

                    let successMessage = ConstantsWarehouse.notificationMessage.success("WheelBase Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToWheelBase.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + "Vehicle to WheelBase association WheelBase id \"" + this.deleteVehicleToWheelBase.id + "\" Vehicleid  \"" + this.deleteVehicleToWheelBase.vehicleId + "\" with change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.vehicleToWheelBaseSearchViewModel.result.vehicleToWheelBases.filter(x => x.id == this.deleteVehicleToWheelBase.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("WheelBase Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToWheelBase.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                    this.showLoadingGif = false;
                }
                this.showLoadingGif = false;
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("WheelBase Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToWheelBase.id);
                this.toastr.warning(error, errorMessage.title);
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
        this.sharedService.vehicleToWheelBaseSearchViewModel = this.vehicleToWheelBaseSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    }

    private onViewWheelBaseCr(WheelBaseVm: IWheelBase) {
        this.sharedService.vehicleToWheelBaseSearchViewModel = this.vehicleToWheelBaseSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/WheelBase/" + WheelBaseVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    private onViewAssociatedVehiclesCr(associatedVehicleVm: IVehicleToWheelBase) {
        this.sharedService.vehicleToWheelBaseSearchViewModel = this.vehicleToWheelBaseSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletowheelbase/" + associatedVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }
}

