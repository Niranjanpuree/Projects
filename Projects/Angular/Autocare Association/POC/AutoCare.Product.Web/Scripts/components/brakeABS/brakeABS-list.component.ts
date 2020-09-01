import {Component, OnInit, ViewChild} from '@angular/core';
import { Router } from '@angular/router';
import { ModalComponent } from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from '../../lib/aclibs/ng2-toastr/ng2-toastr';
import { AutoCompleteDirective } from '../../lib/aclibs/ac-autocomplete/ac-autocomplete';
import { AcGridComponent } from "../../lib/aclibs/ac-grid/ac-grid";
import {IBrakeABS} from './brakeABS.model';
import {BrakeABSService} from './brakeABS.service';
import {BaseVehicleService} from '../baseVehicle/baseVehicle.service';
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'brakeABS-list-component',
    templateUrl: 'app/templates/brakeABS/brakeABS-list.component.html',
})

export class BrakeABSListComponent {
    brakeABSs: IBrakeABS[];
    filteredBrakeABSes: IBrakeABS[] = [];
    brakeABS: IBrakeABS = {};
    modifiedModel: IBrakeABS = {};
    deleteModel: IBrakeABS = {};
    modifiedBrakeABS: IBrakeABS = {};
    brakeABSNameFilter: string = '';

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

    constructor(private brakeABSService: BrakeABSService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.brakeABSService.getAllBrakeABSes().subscribe(
            sm => {
                this.brakeABSs = sm;
                this.showLoadingGif = false;
            },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            }
        );
    }

    validationCheck(item: IBrakeABS): boolean {
        let isValid = true;
        if (!item.name) {
            this.toastr.warning("Name is required.", ConstantsWarehouse.validationTitle);
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

    public applyFilter = (keyword?: string) => {
        this.showLoadingGif = true;
        keyword = keyword || '';
        if (keyword == '') {
            keyword = this.brakeABSNameFilter;
        }
        else {
            this.brakeABSNameFilter = keyword;
        }

        if (String(this.brakeABSNameFilter) === "") {
            this.brakeABSService.getAllBrakeABSes().subscribe(sm => {
                this.brakeABSs = sm;
                this.showLoadingGif = false;
                this.filteredBrakeABSes = [];
            },
                error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.brakeABSService.getBrakeABSs(this.brakeABSNameFilter).subscribe(m => {
                this.brakeABSs = m;
                this.showLoadingGif = false;
                this.filteredBrakeABSes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.brakeABSService.getBrakeABSs(keyword);
    }

    onSelect(brakeABS: IBrakeABS) {
        this.brakeABSNameFilter = brakeABS.name;
        this.applyFilter();
        this.filteredBrakeABSes = [];
    }

    onNew() {
        this.brakeABS = {};
        this.newPopup.open("md");
    }

    onNewSubmit() {
        if (!this.validationCheck(this.brakeABS)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.brakeABS.attachments = uploadedFiles;
            }
            if (this.brakeABS.attachments) {
                this.brakeABS.attachments = this.brakeABS.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.brakeABSService.addBrakeABS(this.brakeABS).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Brake ABS", ConstantsWarehouse.changeRequestType.add, this.brakeABS.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.brakeABS.name + "\" BrakeABS change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake ABS", ConstantsWarehouse.changeRequestType.add, this.brakeABS.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake ABS", ConstantsWarehouse.changeRequestType.add, this.brakeABS.name);
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

    onModify(brakeABS: IBrakeABS) {
        this.brakeABS = brakeABS;
        this.showLoadingGif = true;
        if (!brakeABS.brakeConfigCount && !brakeABS.vehicleToBrakeConfigCount) {
            this.brakeABSService.getBrakeABSDetail(brakeABS.id).subscribe(m => {
                brakeABS.brakeConfigCount = m.brakeConfigCount;
                brakeABS.vehicleToBrakeConfigCount = m.vehicleToBrakeConfigCount;
                this.modifiedBrakeABS = <IBrakeABS>JSON.parse(JSON.stringify(brakeABS));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedBrakeABS = <IBrakeABS>JSON.parse(JSON.stringify(brakeABS));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedBrakeABS)) {
            return;
        }
        else if (this.modifiedBrakeABS.name == this.brakeABS.name) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedBrakeABS.attachments = uploadedFiles;
            }
            if (this.modifiedBrakeABS.attachments) {
                this.modifiedBrakeABS.attachments = this.modifiedBrakeABS.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.brakeABSService.updateBrakeABS(this.modifiedBrakeABS.id, this.modifiedBrakeABS).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Brake ABS", ConstantsWarehouse.changeRequestType.modify, this.brakeABS.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.brakeABS.name + "\" BrakeABS change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.brakeABSs.filter(x => x.id == this.modifiedBrakeABS.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake ABS", ConstantsWarehouse.changeRequestType.modify, this.brakeABS.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake ABS", ConstantsWarehouse.changeRequestType.modify, this.brakeABS.name);
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

    onDelete(brakeABS: IBrakeABS) {
        this.brakeABS = brakeABS;
        this.showLoadingGif = true;
        if (!brakeABS.brakeConfigCount && !brakeABS.vehicleToBrakeConfigCount) {
            this.brakeABSService.getBrakeABSDetail(brakeABS.id).subscribe(m => {
                brakeABS.brakeConfigCount = m.brakeConfigCount;
                brakeABS.vehicleToBrakeConfigCount = m.vehicleToBrakeConfigCount;

                if (brakeABS.brakeConfigCount > 0) {
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
            if (brakeABS.brakeConfigCount > 0) {
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
        if (!this.validationCheck(this.brakeABS)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.brakeABS.attachments = uploadedFiles;
            }
            if (this.brakeABS.attachments) {
                this.brakeABS.attachments = this.brakeABS.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.brakeABSService.deleteBrakeABS(this.brakeABS.id, this.brakeABS).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Brake ABS", ConstantsWarehouse.changeRequestType.remove, this.brakeABS.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.brakeABS.name + "\" BrakeABS change request Id  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.brakeABSs.filter(x => x.id == this.brakeABS.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake ABS", ConstantsWarehouse.changeRequestType.remove, this.brakeABS.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake ABS", ConstantsWarehouse.changeRequestType.remove, this.brakeABS.name);
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

    view(brakeABSVm: IBrakeABS) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/brakeabs/" + brakeABSVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}