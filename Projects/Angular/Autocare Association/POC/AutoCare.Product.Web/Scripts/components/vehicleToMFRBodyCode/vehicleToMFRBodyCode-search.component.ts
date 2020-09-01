import { Component, OnInit, ViewChild, Input,
    Output, EventEmitter, DoCheck }                              from "@angular/core";
import { Router }                    from "@angular/router";
import { ModalComponent }             from "ng2-bs3-modal/ng2-bs3-modal";
import { IVehicleToMfrBodyCode }                            from "./vehicleToMfrBodyCode.model";
import { VehicleToMfrBodyCodeService }                      from "./vehicleToMfrBodyCode.service";
import { ToastsManager }                                from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                                from "../shared/shared.service";
import { NavigationService }                            from "../shared/navigation.service";
import { ConstantsWarehouse }                           from "../constants-warehouse";
import { IVehicleToMfrBodyCodeSearchInputModel
    , IVehicleToMfrBodyCodeSearchViewModel }                from "./vehicleToMfrBodyCode-search.model";
import { AcGridComponent }                                  from "../../lib/aclibs/ac-grid/ac-grid";
import { AcFileUploader }                                   from "../../lib/aclibs/ac-fileuploader/ac-fileuploader";
import { pageChangedEventArg }                              from "../../lib/aclibs/ac-grid/ac-grid.component";
import {IMfrBodyCode} from "../mfrBodyCode/mfrBodyCode.model";
import {MfrBodyCodeService} from "../mfrBodyCode/mfrBodyCode.service";

@Component({
    selector: "vehicletomfrbodycode-search",
    templateUrl: "app/templates/vehicleToMfrBodyCode/vehicleToMfrBodyCode-search.component.html",
    providers: [VehicleToMfrBodyCodeService, MfrBodyCodeService],
})

export class VehicleToMfrBodyCodeSearchComponent implements OnInit, DoCheck {
    private isHide: boolean = false;
    private vehicleTomfrBodyCodesRetrieved: IVehicleToMfrBodyCode[] = [];
    private deleteVehicleToMfrBodyCode: IVehicleToMfrBodyCode;
    private isLeftMenuHidden: boolean = false;
    private activeSubMenu: string = '';
    private activeSubMenuGroup: string = '';
    private isMfrBodyCodeExpanded: boolean = true;
    private isAssociatedVehiclesExpanded: boolean = true;
    private isSystemsMenuExpanded: boolean = true;
    private isChildClicked: boolean = false;
    private isMenuExpanded: boolean = true;
    private showLoadingGif: boolean = false;
    private makeQuery: string;
    private modelQuery: string;
    private subModelQuery: string;
    private isSelectAllMfrBodyCodes: boolean;
    private previousmfrBodyCodes: any;
    private mfrBodyCode: IMfrBodyCode = {};
    private modifyMfrBodyCode: IMfrBodyCode = {};
    private selectedMfrBodyCode: IMfrBodyCode = {};

    @ViewChild('deleteMfrBodyCodeAssociationPopup') deleteMfrBodyCodeAssociationPopup: ModalComponent;
    @ViewChild(AcFileUploader) acFileUploader: AcFileUploader;
    @ViewChild("mfrBodyCodeGrid") mfrBodyCodeGrid: AcGridComponent;
    @ViewChild("vehicleToMfrBodyCodeGrid") vehicleToMfrBodyCodeGrid: AcGridComponent;
    @ViewChild('addMfrBodyCodeModal') addMfrBodyCodeModalPopup: ModalComponent;
    @ViewChild('modifyMfrBodyCodeModal') modifyMfrBodyCodeModalPopup: ModalComponent;
    @ViewChild('modifyPopupAcFileUploader') modifyPopupAcFileUploader: AcFileUploader;
    @ViewChild('deletePopupAcFileUploader') deletePopupAcFileUploader: AcFileUploader;
    @ViewChild('deleteMfrBodyCodeModal') deleteMfrBodyCodeModalPopup: ModalComponent;

    @Input("thresholdRecordCount") thresholdRecordCount: number;
    @Input("vehicleToMfrBodyCodeSearchViewModel") vehicleToMfrBodyCodeSearchViewModel: IVehicleToMfrBodyCodeSearchViewModel;
    @Input("vehicleToMfrBodyCodesForSelectedMfrBodyCode") vehicleToMfrBodyCodesForSelectedMfrBodyCode: IVehicleToMfrBodyCode[];

    constructor(private sharedService: SharedService, private vehicleToMfrBodyCodeService: VehicleToMfrBodyCodeService,
        private router: Router, private toastr: ToastsManager, private navigationService: NavigationService,
        private mfrBodyCodeService: MfrBodyCodeService) {
    }

    ngOnInit() {
        this.sharedService.vehicles = null;        //clear old selections
        this.sharedService.mfrBodyCodes = null;    //clear old selections

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
    }

    ngDoCheck() {
        if (this.previousmfrBodyCodes != this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes) {
            this.isSelectAllMfrBodyCodes = false;
            if (this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.length > 0) {
                if (this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.every(item => item.isSelected))
                    this.isSelectAllMfrBodyCodes = true;
                this.previousmfrBodyCodes = this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes;
            }
        }
    }

    refreshGrids() {
        if (this.mfrBodyCodeGrid)
            this.mfrBodyCodeGrid.refresh();
        if (this.vehicleToMfrBodyCodeGrid)
            this.vehicleToMfrBodyCodeGrid.refresh();
    }

    private onSelectAllMfrBodyCode(selected: boolean) {
        this.isSelectAllMfrBodyCodes = selected;
        if (this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes == null) {
            return;
        }
        this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = [];
        this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.forEach(item => {
            item.isSelected = selected;
            this.refreshAssociationWithMfrBodyCodeId(item.id, item.isSelected);
        });

        // refresh grid
        if (this.vehicleToMfrBodyCodeGrid)
            this.vehicleToMfrBodyCodeGrid.refresh();
    }

    private onMfrBodyCodeSelected(mfrBodyCode: IMfrBodyCode) {
        this.refreshAssociationWithMfrBodyCodeId(mfrBodyCode.id, !mfrBodyCode.isSelected);
        if (mfrBodyCode.isSelected) {
            //unchecked
            this.isSelectAllMfrBodyCodes = false;
        }
        else {
            //checked
            var excludedMfrBodyCode = this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.filter(item => item.id != mfrBodyCode.id);
            if (excludedMfrBodyCode.every(item => item.isSelected)) {
                this.isSelectAllMfrBodyCodes = true;
            }
        }

        // refresh grid
        if (this.vehicleToMfrBodyCodeGrid)
            this.vehicleToMfrBodyCodeGrid.refresh();
    }

    private refreshAssociationWithMfrBodyCodeId(MfrBodyCodeId, isSelected) {
        if (isSelected) {
            this.vehicleTomfrBodyCodesRetrieved = this.getVehicleTomfrBodyCodesByMfrBodyCodeId(MfrBodyCodeId);
            let temp = this.vehicleToMfrBodyCodesForSelectedMfrBodyCode || [];
            for (var vehicleToMfrBodyCode of this.vehicleTomfrBodyCodesRetrieved) {
                temp.push(vehicleToMfrBodyCode);
            }
            this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = temp;
        }
        else {
            let m = this.vehicleToMfrBodyCodesForSelectedMfrBodyCode.filter(x => x.mfrBodyCode.id != MfrBodyCodeId);
            this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = m;
        }
    }

    private getVehicleTomfrBodyCodesByMfrBodyCodeId(id) {
        return this.vehicleToMfrBodyCodeSearchViewModel.result.vehicleToMfrBodyCodes.filter(v => v.mfrBodyCode.id == id);
    }

    private onSelectedNewMfrBodyCodeAssociation() {
        this.sharedService.mfrBodyCodes = this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.filter(item => item.isSelected);
        this.sharedService.vehicleToMfrBodyCodeSearchViewModel = this.vehicleToMfrBodyCodeSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletomfrbodycode/add"]);
    }

    private onDeleteVehicleToMfrBodyCode(vehicleToMfrBodyCode: IVehicleToMfrBodyCode) {
        this.deleteVehicleToMfrBodyCode = vehicleToMfrBodyCode;
        this.deleteVehicleToMfrBodyCode.comment = "";
        this.deleteMfrBodyCodeAssociationPopup.open("md");
    }

    private onDeleteVehicleToMfrBodyCodeSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteVehicleToMfrBodyCode.attachments = uploadedFiles;
            }
            if (this.deleteVehicleToMfrBodyCode.attachments) {
                this.deleteVehicleToMfrBodyCode.attachments = this.deleteVehicleToMfrBodyCode.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleToMfrBodyCodeService.deleteVehicleToMfrBodyCode(this.deleteVehicleToMfrBodyCode.id, this.deleteVehicleToMfrBodyCode).subscribe(response => {
                if (response) {
                    this.deleteMfrBodyCodeModalPopup.close();

                    let successMessage = ConstantsWarehouse.notificationMessage.success("Mfr Body Code Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToMfrBodyCode.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + "Vehicle to Mfr Body Code association Mfr Body Code id \"" + this.deleteVehicleToMfrBodyCode.id + "\" Vehicleid  \"" + this.deleteVehicleToMfrBodyCode.vehicleId + "\" with change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.vehicleToMfrBodyCodeSearchViewModel.result.vehicleToMfrBodyCodes.filter(x => x.id == this.deleteVehicleToMfrBodyCode.id)[0].changeRequestId = response;
                    this.deleteMfrBodyCodeAssociationPopup.close();
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Mfr Body Code Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToMfrBodyCode.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                    this.showLoadingGif = false;
                }
                this.showLoadingGif = false;
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Mfr Body Code Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToMfrBodyCode.id);
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
        this.sharedService.vehicleToMfrBodyCodeSearchViewModel = this.vehicleToMfrBodyCodeSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    }


    private onViewMfrBodyCodeCr(mfrBodyCodeVm: IMfrBodyCode) {
        this.sharedService.vehicleToMfrBodyCodeSearchViewModel = this.vehicleToMfrBodyCodeSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/mfrbodycode/" + mfrBodyCodeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    private onViewAssociatedVehiclesCr(associatedVehicleVm: IVehicleToMfrBodyCode) {
        this.sharedService.vehicleToMfrBodyCodeSearchViewModel = this.vehicleToMfrBodyCodeSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletomfrbodycode/" + associatedVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    //Code For Add MfrBody Code Starts

    private openAddMfrBodyCodeModal() {
        this.addMfrBodyCodeModalPopup.open("md");
    }
    private openModifyMfrBodyCodeModal(mfrBodyCode: IMfrBodyCode) {
        this.showLoadingGif = true;
        this.selectedMfrBodyCode = mfrBodyCode;
        this.modifyMfrBodyCode.id = this.selectedMfrBodyCode.id;
        this.modifyMfrBodyCode.name = this.selectedMfrBodyCode.name;
        this.modifyMfrBodyCode.comment = "";
        this.modifyMfrBodyCodeModalPopup.open("md");
        if (this.selectedMfrBodyCode.vehicleToMfrBodyCodeCount == 0) {
            this.mfrBodyCodeService.getMfrBodyCode(this.selectedMfrBodyCode.id).subscribe(result => {
                this.selectedMfrBodyCode.vehicleToMfrBodyCodeCount = this.modifyMfrBodyCode.vehicleToMfrBodyCodeCount = result.vehicleToMfrBodyCodeCount;
                this.showLoadingGif = false;
            },
                error => {
                    this.toastr.warning(<any>error.toString(), "Load Failed");
                    this.showLoadingGif = false;
                });

        }
    }
    private openDeleteMfrBodyCodeModal(mfrBodyCode: IMfrBodyCode) {
        this.showLoadingGif = true;
        this.selectedMfrBodyCode = mfrBodyCode;
        this.mfrBodyCode.id = this.selectedMfrBodyCode.id;
        this.mfrBodyCode.name = this.selectedMfrBodyCode.name;
        this.mfrBodyCode.comment = "";
        this.deleteMfrBodyCodeModalPopup.open("md");
        if (this.selectedMfrBodyCode.vehicleToMfrBodyCodeCount == 0) {
            this.mfrBodyCodeService.getMfrBodyCode(this.selectedMfrBodyCode.id).subscribe(result => {
                this.selectedMfrBodyCode.vehicleToMfrBodyCodeCount = this.mfrBodyCode.vehicleToMfrBodyCodeCount = result.vehicleToMfrBodyCodeCount;
                this.showLoadingGif = false;
            },
                error => {
                    this.toastr.warning(<any>error.toString(), "Load Failed");
                    this.showLoadingGif = false;
                });

        }
    }
    private onAddMfrBodyCodeCancel() {
        this.mfrBodyCode = [];
        this.modifyMfrBodyCode = [];
        this.addMfrBodyCodeModalPopup.close();
        this.acFileUploader.reset(true);
    }
    private onModifyMfrBodyCodeCancel() {
        this.modifyMfrBodyCode = [];
        this.selectedMfrBodyCode = [];
        this.modifyMfrBodyCodeModalPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
    }
    private onDeleteMfrBodyCodeCancel() {
        this.mfrBodyCode = [];
        this.modifyMfrBodyCode = [];
        this.selectedMfrBodyCode = [];
        this.deleteMfrBodyCodeModalPopup.close();
        this.deletePopupAcFileUploader.reset(true);
    }

    private addMfrBodyCode() {
        this.mfrBodyCode.changeType = "Add";
        if (!this.validationCheck(this.mfrBodyCode)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.mfrBodyCode.attachments = uploadedFiles;
            }
            if (this.mfrBodyCode.attachments) {
                this.mfrBodyCode.attachments = this.mfrBodyCode.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.mfrBodyCodeService.addMfrBodyCode(this.mfrBodyCode).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Mfr Body Code", ConstantsWarehouse.changeRequestType.add, this.mfrBodyCode.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.mfrBodyCode.name + "\" Mfr Body Code change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Mfr Body Code", ConstantsWarehouse.changeRequestType.add, this.mfrBodyCode.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Mfr Body Code", ConstantsWarehouse.changeRequestType.add, this.mfrBodyCode.name);
                this.toastr.warning(errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.showLoadingGif = false;
                this.acFileUploader.reset(true);
                this.addMfrBodyCodeModalPopup.close();
            });

        }, error => {
            this.showLoadingGif = false;
            this.acFileUploader.reset(true);
            this.addMfrBodyCodeModalPopup.close();
        });
    }

    private modifySubmit() {
        this.modifyMfrBodyCode.changeType = "Modify";
        if (!this.validationCheck(this.modifyMfrBodyCode)) {
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifyMfrBodyCode.attachments = uploadedFiles;
            }
            if (this.modifyMfrBodyCode.attachments) {
                this.modifyMfrBodyCode.attachments = this.modifyMfrBodyCode.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.mfrBodyCodeService.updateMfrBodyCode(this.selectedMfrBodyCode.id, this.modifyMfrBodyCode).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Mfr Body Code", ConstantsWarehouse.changeRequestType.modify, this.modifyMfrBodyCode.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.modifyMfrBodyCode.name + "\" Mfr Body Code change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.filter(x => x.id == this.modifyMfrBodyCode.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Mfr Body Code", ConstantsWarehouse.changeRequestType.modify, this.selectedMfrBodyCode.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                this.showLoadingGif = true;
            }, (errorresponse => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Mfr Body Code", ConstantsWarehouse.changeRequestType.modify, this.selectedMfrBodyCode.name);
                this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }), () => {
                this.showLoadingGif = false;
                this.modifyPopupAcFileUploader.reset(true);
                this.modifyMfrBodyCodeModalPopup.close();

            });
        }, error => {
            this.showLoadingGif = false;
            this.modifyPopupAcFileUploader.reset(false);
            this.modifyMfrBodyCodeModalPopup.close();

        });
    }

    private deleteSubmit() {
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.selectedMfrBodyCode.attachments = uploadedFiles;
            }
            if (this.selectedMfrBodyCode.attachments) {
                this.selectedMfrBodyCode.attachments = this.selectedMfrBodyCode.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.mfrBodyCodeService.deleteMfrBodyCode(this.selectedMfrBodyCode.id, this.selectedMfrBodyCode).subscribe(response => {
                if (response) {

                    let successMessage = ConstantsWarehouse.notificationMessage.success("", ConstantsWarehouse.changeRequestType.remove, "MfrBodyCodeId: " + this.selectedMfrBodyCode.id);
                    successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.remove} MfrBodyCodeId ${this.selectedMfrBodyCode.id} change request ID "${response}" will be reviewed.`;
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.deleteMfrBodyCodeModalPopup.close();
                    this.vehicleToMfrBodyCodeSearchViewModel.result.mfrBodyCodes.filter(x => x.id == this.selectedMfrBodyCode.id)[0].changeRequestId = response;
                } else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("", ConstantsWarehouse.changeRequestType.remove, "MfrBodyCodeId: " + this.selectedMfrBodyCode.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                this.showLoadingGif = true;
            }, (errorresponse => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("", ConstantsWarehouse.changeRequestType.remove, "MfrBodyCodeId: " + this.selectedMfrBodyCode.id);
                this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }), () => {
                this.deletePopupAcFileUploader.reset();
                this.showLoadingGif = false;
            });
        }, error => {
            this.deletePopupAcFileUploader.reset();
            this.showLoadingGif = false;
        });
    }

    validationCheck(item: IMfrBodyCode): boolean {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("Mfr Body Code is required.", ConstantsWarehouse.validationTitle);
                return false;
            }
            if (this.mfrBodyCode.changeType == "Modify") {
                if (item.name === this.selectedMfrBodyCode.name) {
                    this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
                    return false;
                }
            }
        }
        return true;
    }
}

