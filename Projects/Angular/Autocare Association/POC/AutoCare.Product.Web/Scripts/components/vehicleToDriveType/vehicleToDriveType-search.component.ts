import { Component, OnInit, ViewChild, Input,
    Output, EventEmitter, DoCheck }                              from "@angular/core";
import { Router }                    from "@angular/router";
import { ModalComponent }             from "ng2-bs3-modal/ng2-bs3-modal";
import { IVehicleToDriveType }                            from "./vehicleToDriveType.model";
import { VehicleToDriveTypeService }                      from "./vehicleToDriveType.service";
import { ToastsManager }                                from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                                from "../shared/shared.service";
import { NavigationService }                            from "../shared/navigation.service";
import { ConstantsWarehouse }                           from "../constants-warehouse";
import { IVehicleToDriveTypeSearchInputModel
    , IVehicleToDriveTypeSearchViewModel }                from "./vehicleToDriveType-search.model";
import { AcGridComponent }                                  from "../../lib/aclibs/ac-grid/ac-grid";
import { AcFileUploader }                                   from "../../lib/aclibs/ac-fileuploader/ac-fileuploader";
import { pageChangedEventArg }                              from "../../lib/aclibs/ac-grid/ac-grid.component";
import {IDriveType} from "../driveType/driveType.model";
import { DriveTypeService }                               from "../driveType/driveType.Service";

@Component({
    selector: "vehicletodrivetype-search",
    templateUrl: "app/templates/vehicleToDriveType/vehicleToDriveType-search.component.html",
    providers: [VehicleToDriveTypeService, DriveTypeService]
})

export class VehicleToDriveTypeSearchComponent implements OnInit, DoCheck {
    private isHide: boolean = false;
    private vehicleTodriveTypesRetrieved: IVehicleToDriveType[] = [];
    private deleteVehicleToDriveType: IVehicleToDriveType;
    private isLeftMenuHidden: boolean = false;
    private activeSubMenu: string = '';
    private activeSubMenuGroup: string = '';
    private isDriveTypeExpanded: boolean = true;
    private isAssociatedVehiclesExpanded: boolean = true;
    private isSystemsMenuExpanded: boolean = true;
    private isChildClicked: boolean = false;
    private isMenuExpanded: boolean = true;
    private showLoadingGif: boolean = false;
    private makeQuery: string;
    private modelQuery: string;
    private subModelQuery: string;
    private isSelectAllDriveTypes: boolean;
    private previousdriveTypes: any;

    //drive Type grid control variables

    driveTypes: IDriveType[];
    //filteredDriveTypes: IDriveType[] = [];
    driveType: IDriveType = {};
    modifiedDriveType: IDriveType = {};
    //driveTypeNameFilter: string = '';

    //drive type modals

    @ViewChild('newPopup')newPopup: ModalComponent;
    @ViewChild('modifyPopup')modifyPopup: ModalComponent;
    @ViewChild('deleteErrorPopup')deleteErrorPopup: ModalComponent;
    @ViewChild('deleteConfirmPopup')deleteConfirmPopup: ModalComponent;
    @ViewChild(AcFileUploader)acFileUploader: AcFileUploader;
    @ViewChild("modifyPopupAcFileUploader")modifyPopupAcFileUploader: AcFileUploader;
    @ViewChild("deletePopupAcFileUploader") deletePopupAcFileUploader: AcFileUploader;
    @ViewChild('deleteDriveTypeAssociationPopup') deleteDriveTypeAssociationPopup: ModalComponent;
    //@ViewChild(AcFileUploader) acFileUploader: AcFileUploader;
    @ViewChild("driveTypeGrid") driveTypeGrid: AcGridComponent;
    @ViewChild("vehicleToDriveTypeGrid") vehicleToDriveTypeGrid: AcGridComponent;

    @Input("thresholdRecordCount") thresholdRecordCount: number;
    @Input("vehicleToDriveTypeSearchViewModel") vehicleToDriveTypeSearchViewModel: IVehicleToDriveTypeSearchViewModel;
    @Input("vehicleToDriveTypesForSelectedDriveType") vehicleToDriveTypesForSelectedDriveType: IVehicleToDriveType[];

    constructor(private sharedService: SharedService, private driveTypeService: DriveTypeService, private vehicleToDriveTypeService: VehicleToDriveTypeService,
        private router: Router, private toastr: ToastsManager, private navigationService: NavigationService) {
    }

    ngOnInit() {
        this.sharedService.vehicles = null;        //clear old selections
        this.sharedService.driveTypes = null;    //clear old selections

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

    onNew() {
        this.driveType = {};
        this.newPopup.open("md");
    }

    onNewSubmit() {
        if (!this.validationCheck(this.driveType)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.driveType.attachments = uploadedFiles;
            }
            if (this.driveType.attachments) {
                this.driveType.attachments = this.driveType.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.driveTypeService.addDriveType(this.driveType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Drive Type Name", ConstantsWarehouse.changeRequestType.add, this.driveType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the Drive Type  of name \"" + this.driveType.name + "\" Drive Type change request Id  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Drive Type ", ConstantsWarehouse.changeRequestType.add, this.driveType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Drive Type ", ConstantsWarehouse.changeRequestType.add, this.driveType.name);
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

    onModify(driveType: IDriveType) {
        this.driveType = driveType;
        this.showLoadingGif = true;
        if (!driveType.vehicleToDriveTypeCount) {
            this.driveTypeService.getDriveType(driveType.id).subscribe(m => {
                this.driveType.vehicleToDriveTypeCount = m.vehicleToDriveTypeCount;
                this.modifiedDriveType = <IDriveType>JSON.parse(JSON.stringify(driveType));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedDriveType = <IDriveType>JSON.parse(JSON.stringify(driveType));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedDriveType)) {
            return;
        }
        else if (this.modifiedDriveType.name == this.driveType.name) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedDriveType.attachments = uploadedFiles;
            }
            if (this.modifiedDriveType.attachments) {
                this.modifiedDriveType.attachments = this.modifiedDriveType.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.driveTypeService.updateDriveType(this.modifiedDriveType.id, this.modifiedDriveType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Drive Type", ConstantsWarehouse.changeRequestType.modify, this.driveType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the Drive Type with name \"" + this.driveType.name  + "\" Wheel Base change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.vehicleToDriveTypeSearchViewModel.result.driveTypes.find(x => x.id === this.modifiedDriveType.id).changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Drive Type", ConstantsWarehouse.changeRequestType.modify, this.driveType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Drive Type", ConstantsWarehouse.changeRequestType.modify, this.driveType.name);
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

    onDelete(driveType: IDriveType) {
        this.driveType = driveType;
        this.showLoadingGif = true;
        if (!driveType.vehicleToDriveTypeCount) {
            this.driveTypeService.getDriveType(driveType.id).subscribe(m => {
                driveType.vehicleToDriveTypeCount = m.vehicleToDriveTypeCount;
                this.driveType.vehicleToDriveTypeCount = driveType.vehicleToDriveTypeCount;
                if (driveType.vehicleToDriveTypeCount > 0) {
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
            if (driveType.vehicleToDriveTypeCount > 0) {
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
        if (!this.validationCheck(this.driveType)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.driveType.attachments = uploadedFiles;
            }
            if (this.driveType.attachments) {
                this.driveType.attachments = this.driveType.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.driveTypeService.deleteDriveType(this.driveType.id, this.driveType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Drive Type", ConstantsWarehouse.changeRequestType.remove, this.driveType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the Drive Type with name \"" + this.driveType.name + "\" Drive Type change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.vehicleToDriveTypeSearchViewModel.result.driveTypes.find(x => x.id === this.driveType.id).changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Drive Type", ConstantsWarehouse.changeRequestType.remove, this.driveType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Drive Type", ConstantsWarehouse.changeRequestType.remove, this.driveType.name);
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

    onReplace(driveType: IDriveType) {

        var replaceRequestLink = "/DriveType/replace/" + driveType.id;
        this.router.navigateByUrl(replaceRequestLink);
    }
    
    validationCheck(item: IDriveType): boolean {
        let isValid = true;
        if (!item.name) {
            this.toastr.warning("Drive Type  is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onCancel(action: string) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
        this.deleteErrorPopup.close();
    }

    ngDoCheck() {
        if (this.previousdriveTypes != this.vehicleToDriveTypeSearchViewModel.result.driveTypes) {
            this.isSelectAllDriveTypes = false;
            if (this.vehicleToDriveTypeSearchViewModel.result.driveTypes.length > 0) {
                if (this.vehicleToDriveTypeSearchViewModel.result.driveTypes.every(item => item.isSelected))
                    this.isSelectAllDriveTypes = true;
                this.previousdriveTypes = this.vehicleToDriveTypeSearchViewModel.result.driveTypes;
            }
        }
    }

    refreshGrids() {
        if (this.driveTypeGrid)
            this.driveTypeGrid.refresh();
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
    }

    private onSelectAllDriveType(selected: boolean) {
        this.isSelectAllDriveTypes = selected;
        if (this.vehicleToDriveTypeSearchViewModel.result.driveTypes == null) {
            return;
        }
        this.vehicleToDriveTypesForSelectedDriveType = [];
        this.vehicleToDriveTypeSearchViewModel.result.driveTypes.forEach(item => {
            item.isSelected = selected;
            this.refreshAssociationWithDriveTypeId(item.id, item.isSelected);
        });

        // refresh grid
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
    }

    private onDriveTypeSelected(driveType: IDriveType) {
        this.refreshAssociationWithDriveTypeId(driveType.id, !driveType.isSelected);

        if (driveType.isSelected) {
            //unchecked
            this.isSelectAllDriveTypes = false;
        }
        else {
            //checked
            var excludedDriveType = this.vehicleToDriveTypeSearchViewModel.result.driveTypes.filter(item => item.id != driveType.id);
            if (excludedDriveType.every(item => item.isSelected)) {
                this.isSelectAllDriveTypes = true;
            }
        }

         //refresh grid
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
    }

    private refreshAssociationWithDriveTypeId(driveTypeId, isSelected) {
        if (isSelected) {
            this.vehicleTodriveTypesRetrieved = this.getVehicleTodriveTypesByDriveTypeId(driveTypeId);
            //TODO: number of associations which may be useful in add brake association screen?
            let temp = this.vehicleToDriveTypesForSelectedDriveType || [];
            for (var vehicleToDriveType of this.vehicleTodriveTypesRetrieved) {
                temp.push(vehicleToDriveType);
            }
            this.vehicleToDriveTypesForSelectedDriveType = temp;
        }
        else {
            let m = this.vehicleToDriveTypesForSelectedDriveType.filter(x => x.driveType.id != driveTypeId);
            this.vehicleToDriveTypesForSelectedDriveType = m;
        }
    }

    private getVehicleTodriveTypesByDriveTypeId(id) {
        return this.vehicleToDriveTypeSearchViewModel.result.vehicleToDriveTypes.filter(v => v.driveType.id == id);
    }

    private onSelectedNewDriveTypeAssociation() {
        this.sharedService.driveTypes = this.vehicleToDriveTypeSearchViewModel.result.driveTypes.filter(item => item.isSelected);
        this.sharedService.vehicleToDriveTypeSearchViewModel = this.vehicleToDriveTypeSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletodrivetype/add"]);
    }

    private onDeleteVehicleToDriveType(vehicleToDriveType: IVehicleToDriveType) {
        this.deleteVehicleToDriveType = vehicleToDriveType;
        this.deleteVehicleToDriveType.comment = "";
        this.deleteDriveTypeAssociationPopup.open("md");
    }

    private onDeleteVehicleToDriveTypeSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteVehicleToDriveType.attachments = uploadedFiles;
            }
            if (this.deleteVehicleToDriveType.attachments) {
                this.deleteVehicleToDriveType.attachments = this.deleteVehicleToDriveType.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleToDriveTypeService.deleteVehicleToDriveType(this.deleteVehicleToDriveType.id, this.deleteVehicleToDriveType).subscribe(response => {
                if (response) {
                    this.deleteDriveTypeAssociationPopup.close();

                    let successMessage = ConstantsWarehouse.notificationMessage.success("Drive Type Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToDriveType.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + "Vehicle to drive type association body type id \"" + this.deleteVehicleToDriveType.id + "\" Vehicleid  \"" + this.deleteVehicleToDriveType.vehicleId + "\" with change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.vehicleToDriveTypesForSelectedDriveType.find(x => x.id === this.deleteVehicleToDriveType.id).changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Drive Type Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToDriveType.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                    this.showLoadingGif = false;
                }
                this.showLoadingGif = false;
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Drive Type Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToDriveType.id);
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
        this.sharedService.vehicleToDriveTypeSearchViewModel = this.vehicleToDriveTypeSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    }

    private onViewDriveTypeCr(driveTypeVm: IDriveType) {
        this.sharedService.vehicleToDriveTypeSearchViewModel = this.vehicleToDriveTypeSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/drivetype/" + driveTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    private onViewAssociatedVehiclesCr(associatedVehicleVm: IVehicleToDriveType) {
        this.sharedService.vehicleToDriveTypeSearchViewModel = this.vehicleToDriveTypeSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletodrivetype/" + associatedVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }
}

