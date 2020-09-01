import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {ModalComponent} from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {IVehicleType} from './vehicleType.model';
import {VehicleTypeService} from './vehicleType.service';
import {IVehicleTypeGroup} from '../vehicleTypeGroup/vehicleTypeGroup.model';
import {VehicleTypeGroupService} from '../vehicleTypeGroup/vehicleTypeGroup.service';
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'vehicleType-list-component',
    templateUrl: 'app/templates/vehicleType/vehicleType-list.component.html',
})

export class VehicleTypeListComponent {
    vehicleTypes: IVehicleType[];
    filteredVehicleTypes: IVehicleType[] = [];
    vehicleTypeGroups: IVehicleTypeGroup[];
    vehicleType: IVehicleType = {};
    modifiedVehicleType: IVehicleType = {};

    vehicleTypeNameFilter: string = '';

    @ViewChild('newPopup')
    newPopup: ModalComponent;

    @ViewChild('modifyPopup')
    modifyPopup: ModalComponent;

    @ViewChild('deleteErrorPopup')
    deleteErrorPopup: ModalComponent;

    @ViewChild('deleteConfirmPopup')
    deleteConfirmPopup: ModalComponent;

    @ViewChild('viewVehicleTypeChangeRequestModal')
    viewVehicleTypeChangeRequestModal: ModalComponent;

    @ViewChild("newPopupAcFileUploader")
    newPopupAcFileUploader: AcFileUploader;

    @ViewChild("modifyPopupAcFileUploader")
    modifyPopupAcFileUploader: AcFileUploader;

    @ViewChild("deletePopupAcFileUploader")
    deletePopupAcFileUploader: AcFileUploader;

    showLoadingGif: boolean = false;

    constructor(private vehicleTypeService: VehicleTypeService, private vehicleTypeGroupService: VehicleTypeGroupService,
        private router: Router, private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.vehicleTypeService.getAllVehicleTypes().subscribe(sm => {
            this.vehicleTypes = sm;
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });

        this.vehicleTypeGroupService.getAllVehicleTypeGroups().subscribe(m => {
            this.vehicleTypeGroups = m;
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
            keyword = this.vehicleTypeNameFilter;
        }
        else {
            this.vehicleTypeNameFilter = keyword;
        }

        if (String(this.vehicleTypeNameFilter) === "") {
            this.vehicleTypeService.getAllVehicleTypes().subscribe(sm => {
                this.vehicleTypes = sm;
                this.showLoadingGif = false;
                this.filteredVehicleTypes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.vehicleTypeService.getVehicleTypes(this.vehicleTypeNameFilter).subscribe(m => {
                this.vehicleTypes = m;
                this.showLoadingGif = false;
                this.filteredVehicleTypes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.vehicleTypeService.getVehicleTypes(keyword);
    }

    onSelect(vehicleType: IVehicleType) {
        this.vehicleTypeNameFilter = vehicleType.name;
        this.applyFilter();
        this.filteredVehicleTypes = [];
    }

    validationCheck(item: IVehicleType): boolean {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("Vehicle Type Name is required.", ConstantsWarehouse.validationTitle);
                return false;
            }
            else if (<Number>this.vehicleType.vehicleTypeGroupId === -1) {
                this.toastr.warning('Please select Vehicle Type Group', ConstantsWarehouse.validationTitle);
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
        this.vehicleType = {};
        this.vehicleType.vehicleTypeGroupId = -1;
        this.newPopup.open("md");
    }

    onNewSubmit() {
        if (!this.validationCheck(this.vehicleType)) {
            return;
        }

        this.showLoadingGif = true;
        this.newPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.vehicleType.attachments = uploadedFiles;
            }
            if (this.vehicleType.attachments) {
                this.vehicleType.attachments = this.vehicleType.attachments.concat(this.newPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleTypeService.addVehicleType(this.vehicleType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle Type", ConstantsWarehouse.changeRequestType.add, this.vehicleType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.vehicleType.name + "\" Vehicle Type change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle Type", ConstantsWarehouse.changeRequestType.add, this.vehicleType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle Type", ConstantsWarehouse.changeRequestType.add, this.vehicleType.name);
                this.toastr.warning(errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.newPopupAcFileUploader.reset(true);
                this.newPopup.close();
                this.showLoadingGif = false;
            });
        }, error => {
            this.newPopupAcFileUploader.reset(true);
            this.newPopup.close();
            this.showLoadingGif = false;
        });
    }

    onModify(vehicleType: IVehicleType) {
        this.vehicleType = vehicleType;
        this.showLoadingGif = true;
        if (!vehicleType.modelCount && !vehicleType.baseVehicleCount && !vehicleType.vehicleCount) {
            this.vehicleTypeService.getVehicleTypeDetail(vehicleType.id).subscribe(m => {
                vehicleType.modelCount = m.modelCount;
                vehicleType.baseVehicleCount = m.baseVehicleCount;
                vehicleType.vehicleCount = m.vehicleCount;
                this.modifiedVehicleType = <IVehicleType>JSON.parse(JSON.stringify(vehicleType));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedVehicleType = <IVehicleType>JSON.parse(JSON.stringify(vehicleType));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.vehicleType)) {
            return;
        }
        if (this.modifiedVehicleType.name == this.vehicleType.name && this.modifiedVehicleType.vehicleTypeGroupId == this.vehicleType.vehicleTypeGroupId) {
            this.toastr.info("Nothing Changed");
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedVehicleType.attachments = uploadedFiles;
            }
            if (this.modifiedVehicleType.attachments) {
                this.modifiedVehicleType.attachments = this.modifiedVehicleType.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleTypeService.updateVehicleType(this.modifiedVehicleType.id, this.modifiedVehicleType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle Type", ConstantsWarehouse.changeRequestType.modify, this.vehicleType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.vehicleType.name + "\" Vehicle Type change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.vehicleTypes.filter(x => x.id == this.modifiedVehicleType.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle Type", ConstantsWarehouse.changeRequestType.modify, this.vehicleType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (errorresponse => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle Type", ConstantsWarehouse.changeRequestType.modify, this.vehicleType.name);
                this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }), () => {
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

    onDelete(vehicleType: IVehicleType) {
        this.vehicleType = vehicleType;
        this.vehicleType.comment = "";
        this.showLoadingGif = true;
        if (!vehicleType.modelCount && !vehicleType.baseVehicleCount && !vehicleType.vehicleCount) {
            this.vehicleTypeService.getVehicleTypeDetail(vehicleType.id).subscribe(m => {
                vehicleType.modelCount = m.modelCount;
                vehicleType.baseVehicleCount = m.baseVehicleCount;
                vehicleType.vehicleCount = m.vehicleCount;

                if (vehicleType.modelCount > 0) {
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
            if (vehicleType.modelCount > 0) {
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
        this.vehicleType.changeType = "Delete";
        if (!this.validationCheck(this.vehicleType)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.vehicleType.attachments = uploadedFiles;
            }
            if (this.vehicleType.attachments) {
                this.vehicleType.attachments = this.vehicleType.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleTypeService.deleteVehicleTypePost(this.vehicleType.id, this.vehicleType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle Type", ConstantsWarehouse.changeRequestType.remove, this.vehicleType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.vehicleType.name + "\" Vehicle Type change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.vehicleTypes.filter(x => x.id == this.vehicleType.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle Type", ConstantsWarehouse.changeRequestType.remove, this.vehicleType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (errorresponse => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle Type", ConstantsWarehouse.changeRequestType.remove, this.vehicleType.name);
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

    view(vehicleTypeVm: IVehicleType) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicletype/" + vehicleTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.newPopupAcFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}