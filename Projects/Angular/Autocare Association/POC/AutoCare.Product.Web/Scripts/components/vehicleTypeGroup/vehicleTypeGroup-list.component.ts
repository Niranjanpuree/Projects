import { Component, OnInit, ViewChild} from '@angular/core';
import { Router} from '@angular/router';
import { ModalComponent } from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { IVehicleTypeGroup} from './vehicleTypeGroup.model';
import { VehicleTypeGroupService} from './vehicleTypeGroup.service';
import { ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'vehicleTypeGroup-list-component',
    templateUrl: 'app/templates/vehicleTypeGroup/vehicleTypeGroup-list.component.html',
})

export class VehicleTypeGroupListComponent {
    vehicleTypeGroups: IVehicleTypeGroup[];
    filteredVehicleTypeGroups: IVehicleTypeGroup[] = [];
    vehicleTypeGroup: IVehicleTypeGroup = {};
    modifiedVehicleTypeGroup: IVehicleTypeGroup = {};
    vehicleTypeGroupNameFilter: string = '';

    @ViewChild('newPopup')
    newPopup: ModalComponent;

    @ViewChild('modifyPopup')
    modifyPopup: ModalComponent;

    @ViewChild('deleteErrorPopup')
    deleteErrorPopup: ModalComponent;

    @ViewChild('deleteConfirmPopup')
    deleteConfirmPopup: ModalComponent;

    @ViewChild("newPopupAcFileUploader")
    newPopupAcFileUploader: AcFileUploader;

    @ViewChild("modifyPopupAcFileUploader")
    modifyPopupAcFileUploader: AcFileUploader;

    @ViewChild("deletePopupAcFileUploader")
    deletePopupAcFileUploader: AcFileUploader;

    showLoadingGif: boolean = false;

    constructor(private vehicleTypeGroupService: VehicleTypeGroupService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.vehicleTypeGroupService.getAllVehicleTypeGroups().subscribe(sm => {
            this.vehicleTypeGroups = sm;
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    public applyFilter = (keyword?: string) => {
        this.showLoadingGif = true;
        keyword = keyword || '';
        if (keyword == '') {
            keyword = this.vehicleTypeGroupNameFilter;
        }
        else {
            this.vehicleTypeGroupNameFilter = keyword;
        }

        if (String(this.vehicleTypeGroupNameFilter) === "") {
            this.vehicleTypeGroupService.getAllVehicleTypeGroups().subscribe(sm => {
                this.vehicleTypeGroups = sm;
                this.showLoadingGif = false;
                this.filteredVehicleTypeGroups = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.vehicleTypeGroupService.getVehicleTypeGroups(this.vehicleTypeGroupNameFilter).subscribe(m => {
                this.vehicleTypeGroups = m;
                this.showLoadingGif = false;
                this.filteredVehicleTypeGroups = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.vehicleTypeGroupService.getVehicleTypeGroups(keyword);
    }

    onSelect(vehicleTypeGroup: IVehicleTypeGroup) {
        this.vehicleTypeGroupNameFilter = vehicleTypeGroup.name;
        this.applyFilter();
        this.filteredVehicleTypeGroups = [];
    }

    validationCheck(item: IVehicleTypeGroup): boolean {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("Vehicle Type Group Name is required.", ConstantsWarehouse.validationTitle);
                return false;
            }
        }
        return true;
    }

    onCancel(action: string) {
        this.newPopupAcFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
    }

    onNew() {
        this.vehicleTypeGroup = {};
        this.newPopup.open("md");
    }

    onNewSubmit() {
        if (!this.validationCheck(this.vehicleTypeGroup)) {
            return;
        }

        this.showLoadingGif = true;
        this.newPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.vehicleTypeGroup.attachments = uploadedFiles;
            }
            if (this.vehicleTypeGroup.attachments) {
                this.vehicleTypeGroup.attachments = this.vehicleTypeGroup.attachments.concat(this.newPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleTypeGroupService.addVehicleTypeGroup(this.vehicleTypeGroup).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle Type Group", ConstantsWarehouse.changeRequestType.add, this.vehicleTypeGroup.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.vehicleTypeGroup.name + "\" Vehicle Type Group change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle Type Group", ConstantsWarehouse.changeRequestType.add, this.vehicleTypeGroup.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle Type Group", ConstantsWarehouse.changeRequestType.add, this.vehicleTypeGroup.name);
                this.toastr.warning(errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.showLoadingGif = false;
                this.newPopupAcFileUploader.reset(true);
                this.newPopup.close();
            });

        }, error => {
            this.showLoadingGif = false;
            this.newPopupAcFileUploader.reset(true);
            this.newPopup.close();
        });
    }

    onModify(vehicleTypeGroup: IVehicleTypeGroup) {
        this.vehicleTypeGroup = vehicleTypeGroup;
        this.showLoadingGif = true;
        if (!vehicleTypeGroup.vehicleTypeCount) {
            this.vehicleTypeGroupService.getVehicleTypeGroupDetail(vehicleTypeGroup.id).subscribe(m => {
                vehicleTypeGroup.vehicleTypeCount = m.vehicleTypeCount;
                this.modifiedVehicleTypeGroup = <IVehicleTypeGroup>JSON.parse(JSON.stringify(vehicleTypeGroup));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedVehicleTypeGroup = <IVehicleTypeGroup>JSON.parse(JSON.stringify(vehicleTypeGroup));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedVehicleTypeGroup)) {
            return;
        }
        if (this.modifiedVehicleTypeGroup.name == this.vehicleTypeGroup.name) {
            this.toastr.warning("Nothing Changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedVehicleTypeGroup.attachments = uploadedFiles;
            }
            if (this.modifiedVehicleTypeGroup.attachments) {
                this.modifiedVehicleTypeGroup.attachments = this.modifiedVehicleTypeGroup.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleTypeGroupService.updateVehicleTypeGroup(this.modifiedVehicleTypeGroup.id, this.modifiedVehicleTypeGroup).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle Type Group", ConstantsWarehouse.changeRequestType.modify, this.vehicleTypeGroup.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.vehicleTypeGroup.name + "\" Vehicle Type Group change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.vehicleTypeGroups.filter(x => x.id == this.modifiedVehicleTypeGroup.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle Type Group", ConstantsWarehouse.changeRequestType.modify, this.vehicleTypeGroup.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (errorresponse => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle Type Group", ConstantsWarehouse.changeRequestType.modify, this.vehicleTypeGroup.name);
                this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }), () => {
                this.showLoadingGif = false;
                this.modifyPopupAcFileUploader.reset(true);
                this.modifyPopup.close();
            });

        }, error => {
            this.modifyPopupAcFileUploader.reset(true);
            this.modifyPopup.close();
            this.showLoadingGif = false;
        });
    }

    onDelete(vehicleTypeGroup: IVehicleTypeGroup) {
        this.vehicleTypeGroup = vehicleTypeGroup;
        this.vehicleTypeGroup.comment = "";
        this.showLoadingGif = true;
        if (!vehicleTypeGroup.vehicleTypeCount) {
            this.vehicleTypeGroupService.getVehicleTypeGroupDetail(vehicleTypeGroup.id).subscribe(m => {
                vehicleTypeGroup.vehicleTypeCount = m.vehicleTypeCount;

                if (vehicleTypeGroup.vehicleTypeCount > 0) {
                    this.showLoadingGif = false;
                    this.deleteErrorPopup.open("sm");
                }
                else {
                    this.showLoadingGif = false;
                    this.deleteConfirmPopup.open("md");
                }
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            if (vehicleTypeGroup.vehicleTypeCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("sm");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    }

    onDeleteConfirm() {
        this.vehicleTypeGroup.changeType = "Delete";
        if (!this.validationCheck(this.vehicleTypeGroup)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.vehicleTypeGroup.attachments = uploadedFiles;
            }
            if (this.vehicleTypeGroup.attachments) {
                this.vehicleTypeGroup.attachments = this.vehicleTypeGroup.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleTypeGroupService.deleteVehicleTypeGroupPost(this.vehicleTypeGroup.id, this.vehicleTypeGroup).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle Type Group", ConstantsWarehouse.changeRequestType.remove, this.vehicleTypeGroup.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.vehicleTypeGroup.name + "\" Vehicle Type Group change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.vehicleTypeGroups.filter(x => x.id == this.vehicleTypeGroup.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle Type Group", ConstantsWarehouse.changeRequestType.remove, this.vehicleTypeGroup.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (errorresponse => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle Type Group", ConstantsWarehouse.changeRequestType.remove, this.vehicleTypeGroup.name);
                this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }), () => {
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

    view(vehicleTypeGroupVm: IVehicleTypeGroup) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletypegroup/" + vehicleTypeGroupVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.newPopupAcFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}