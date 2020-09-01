import {Component, OnInit, ViewChild} from '@angular/core';
import { Router } from '@angular/router';
import { ModalComponent } from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {IBrakeSystem} from './brakeSystem.model';
import {BrakeSystemService} from './brakeSystem.service';
import {BaseVehicleService} from '../baseVehicle/baseVehicle.service';
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'brakeSystem-list-component',
    templateUrl: 'app/templates/brakeSystem/brakeSystem-list.component.html',
})

export class BrakeSystemListComponent {
    brakeSystems: IBrakeSystem[];
    filteredBrakeSystems: IBrakeSystem[] = [];
    brakeSystem: IBrakeSystem = {};
    modifiedBrakeSystem: IBrakeSystem = {};
    brakeSystemNameFilter: string = '';

    showLoadingGif: boolean = false;

    @ViewChild('newPopup')
    newPopup: ModalComponent;

    @ViewChild('modifyPopup')
    modifyPopup: ModalComponent;

    @ViewChild('deleteErrorPopup')
    deleteErrorPopup: ModalComponent;

    @ViewChild('deleteConfirmPopup')
    deleteConfirmPopup: ModalComponent;

    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;

    @ViewChild("modifyPopupAcFileUploader")
    modifyPopupAcFileUploader: AcFileUploader;


    @ViewChild("deletePopupAcFileUploader")
    deletePopupAcFileUploader: AcFileUploader;

    constructor(private brakeSystemService: BrakeSystemService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.brakeSystemService.getAllBrakeSystems().subscribe(sm => {
            this.brakeSystems = sm;
            this.showLoadingGif = false;
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
    }

    validationCheck(item: IBrakeSystem): boolean {
        let isValid = true;
        if (!item.name) {
            this.toastr.warning("Name is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }

        return isValid;
    }

    public applyFilter = (keyword?: string) => {
        this.showLoadingGif = true;
        keyword = keyword || '';
        if (keyword == '') {
            keyword = this.brakeSystemNameFilter;
        }
        else {
            this.brakeSystemNameFilter = keyword;
        }

        if ((this.brakeSystemNameFilter) === "") {
            this.brakeSystemService.getAllBrakeSystems().subscribe(sm => {
                this.brakeSystems = sm;
                this.showLoadingGif = false;
                this.filteredBrakeSystems = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.brakeSystemService.getBrakeSystems(this.brakeSystemNameFilter).subscribe(m => {
                this.brakeSystems = <any>m;
                this.showLoadingGif = false;
                this.filteredBrakeSystems = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.brakeSystemService.getBrakeSystems(keyword);
    }

    onSelect(brakeSystem: IBrakeSystem) {
        this.brakeSystemNameFilter = brakeSystem.name;
        this.applyFilter();
        this.filteredBrakeSystems = [];
    }

    onNew() {
        this.brakeSystem = {};
        this.newPopup.open("md");
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

    onNewSubmit() {
        if (!this.validationCheck(this.brakeSystem)) {
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.brakeSystem.attachments = uploadedFiles;
            }
            if (this.brakeSystem.attachments) {
                this.brakeSystem.attachments = this.brakeSystem.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.brakeSystemService.addBrakeSystem(this.brakeSystem).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Brake System", ConstantsWarehouse.changeRequestType.add, this.brakeSystem.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.brakeSystem.name + "\" BrakeSystem change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake System", ConstantsWarehouse.changeRequestType.add, this.brakeSystem.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake System", ConstantsWarehouse.changeRequestType.add, this.brakeSystem.name);
                this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.acFileUploader.reset(true);
                this.newPopup.close();
                this.showLoadingGif = false;
            });

        }, error => {
            this.acFileUploader.reset(true);
            this.newPopup.close();
            this.showLoadingGif = false;
        });
    }

    onModify(brakeSystem: IBrakeSystem) {
        this.brakeSystem = brakeSystem;
        this.showLoadingGif = true;
        if (!brakeSystem.brakeConfigCount && !brakeSystem.vehicleToBrakeConfigCount) {
            this.brakeSystemService.getBrakeSystemDetail(brakeSystem.id).subscribe(m => {
                brakeSystem.brakeConfigCount = m.brakeConfigCount;
                brakeSystem.vehicleToBrakeConfigCount = m.vehicleToBrakeConfigCount;
                this.modifiedBrakeSystem = <IBrakeSystem>JSON.parse(JSON.stringify(brakeSystem));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedBrakeSystem = <IBrakeSystem>JSON.parse(JSON.stringify(brakeSystem));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedBrakeSystem)) {
            return;
        }
        else if (this.modifiedBrakeSystem.name == this.brakeSystem.name) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedBrakeSystem.attachments = uploadedFiles;
            }
            if (this.modifiedBrakeSystem.attachments) {
                this.modifiedBrakeSystem.attachments = this.modifiedBrakeSystem.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.brakeSystemService.updateBrakeSystem(this.modifiedBrakeSystem.id, this.modifiedBrakeSystem).subscribe(response => {
                if (response) {
                    this.newPopup.close();
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Brake System", ConstantsWarehouse.changeRequestType.modify, this.brakeSystem.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.brakeSystem.name + "\" BrakeSystem change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.brakeSystems.filter(x => x.id == this.modifiedBrakeSystem.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake System", ConstantsWarehouse.changeRequestType.modify, this.brakeSystem.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake System", ConstantsWarehouse.changeRequestType.modify, this.brakeSystem.name);
                this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.modifyPopupAcFileUploader.reset(true);
                this.modifyPopup.close();
                this.showLoadingGif = false;
            });

        }, error => {
            this.modifyPopupAcFileUploader.reset(true);
            this.modifyPopup.close();
            this.showLoadingGif = false;
        });
    }

    onDelete(brakeSystem: IBrakeSystem) {
        this.brakeSystem = brakeSystem;
        this.showLoadingGif = true;
        if (!brakeSystem.brakeConfigCount && !brakeSystem.vehicleToBrakeConfigCount) {
            this.brakeSystemService.getBrakeSystemDetail(brakeSystem.id).subscribe(m => {
                brakeSystem.brakeConfigCount = m.brakeConfigCount;
                brakeSystem.vehicleToBrakeConfigCount = m.vehicleToBrakeConfigCount;

                if (brakeSystem.brakeConfigCount > 0) {
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
            if (brakeSystem.brakeConfigCount > 0) {
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
        if (!this.validationCheck(this.brakeSystem)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.brakeSystem.attachments = uploadedFiles;
            }
            if (this.brakeSystem.attachments) {
                this.brakeSystem.attachments = this.brakeSystem.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.brakeSystemService.deleteBrakeSystem(this.brakeSystem.id, this.brakeSystem).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Brake System", ConstantsWarehouse.changeRequestType.remove, this.brakeSystem.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.brakeSystem.name + "\" BrakeSystem change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.brakeSystems.filter(x => x.id == this.brakeSystem.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake System", ConstantsWarehouse.changeRequestType.remove, this.brakeSystem.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake System", ConstantsWarehouse.changeRequestType.remove, this.brakeSystem.name);
                this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.deletePopupAcFileUploader.reset(true);
                this.deleteConfirmPopup.close();
                this.showLoadingGif = false;
            });

        }, error => {
            this.deletePopupAcFileUploader.reset(true);
            this.deleteConfirmPopup.close();
            this.showLoadingGif = false;
        });
    }

    view(brakeSystemVm: IBrakeSystem) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/brakesystem/" + brakeSystemVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}