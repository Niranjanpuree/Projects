import {Component, OnInit, ViewChild} from '@angular/core';
import { Router } from '@angular/router';
import { ModalComponent } from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {IBrakeType} from './brakeType.model';
import {BrakeTypeService} from './brakeType.service';
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'brakeType-list-component',
    templateUrl: 'app/templates/brakeType/brakeType-list.component.html',
})

export class BrakeTypeListComponent {
    brakeTypes: IBrakeType[];
    filteredBrakeTypes: IBrakeType[] = [];
    brakeType: IBrakeType = {};
    modifiedBrakeType: IBrakeType = {};
    brakeTypeNameFilter: string = '';

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

    constructor(private brakeTypeService: BrakeTypeService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.brakeTypeService.getAllBrakeTypes().subscribe(sm => {
            this.brakeTypes = sm;
            this.showLoadingGif = false;
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
    }

    public applyFilter = (keyword?: string) => {
        this.showLoadingGif = true;
        keyword = keyword || '';
        if (keyword == '') {
            keyword = this.brakeTypeNameFilter;
        }
        else {
            this.brakeTypeNameFilter = keyword;
        }

        if (String(this.brakeTypeNameFilter) === "") {
            this.brakeTypeService.getAllBrakeTypes().subscribe(sm => {
                this.brakeTypes = sm;
                this.showLoadingGif = false;
                this.filteredBrakeTypes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.brakeTypeService.getBrakeTypes(this.brakeTypeNameFilter).subscribe(m => {
                this.brakeTypes = m;
                this.showLoadingGif = false;
                this.filteredBrakeTypes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.brakeTypeService.getBrakeTypes(keyword);
    }

    onSelect(brakeType: IBrakeType) {
        this.brakeTypeNameFilter = brakeType.name;
        this.applyFilter();
        this.filteredBrakeTypes = [];
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

    onNew() {
        this.brakeType = {};
        this.newPopup.open("md");
    }

    validationCheck(item: IBrakeType): boolean {
        let isValid = true;
        if (!item.name) {
            this.toastr.warning("Name is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onNewSubmit() {
        if (!this.validationCheck(this.brakeType)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.brakeType.attachments = uploadedFiles;
            }
            if (this.brakeType.attachments) {
                this.brakeType.attachments = this.brakeType.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.brakeTypeService.addBrakeType(this.brakeType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Brake Type", ConstantsWarehouse.changeRequestType.add, this.brakeType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.brakeType.name + "\" Brake Type change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake Type", ConstantsWarehouse.changeRequestType.add, this.brakeType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake Type", ConstantsWarehouse.changeRequestType.add, this.brakeType.name);
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

    onModify(brakeType: IBrakeType) {
        this.brakeType = brakeType;
        this.showLoadingGif = true;
        if (!brakeType.frontBrakeConfigCount && !brakeType.rearBrakeConfigCount && !brakeType.vehicleToBrakeConfigCount) {
            this.brakeTypeService.getBrakeTypeDetail(brakeType.id).subscribe(m => {
                brakeType.frontBrakeConfigCount = m.frontBrakeConfigCount;
                brakeType.rearBrakeConfigCount = m.rearBrakeConfigCount;
                brakeType.vehicleToBrakeConfigCount = m.vehicleToBrakeConfigCount;
                this.modifiedBrakeType = <IBrakeType>JSON.parse(JSON.stringify(brakeType));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedBrakeType = <IBrakeType>JSON.parse(JSON.stringify(brakeType));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedBrakeType)) {
            return;
        }
        else if (this.modifiedBrakeType.name == this.brakeType.name) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedBrakeType.attachments = uploadedFiles;
            }
            if (this.modifiedBrakeType.attachments) {
                this.modifiedBrakeType.attachments = this.modifiedBrakeType.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.brakeTypeService.updateBrakeType(this.modifiedBrakeType.id, this.modifiedBrakeType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Brake Type", ConstantsWarehouse.changeRequestType.modify, this.brakeType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.brakeType.name + "\" Brake Type change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.brakeTypes.filter(x => x.id == this.modifiedBrakeType.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake Type", ConstantsWarehouse.changeRequestType.modify, this.brakeType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake Type", ConstantsWarehouse.changeRequestType.modify, this.brakeType.name);
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

    onDelete(brakeType: IBrakeType) {
        this.brakeType = brakeType;
        this.showLoadingGif = true;
        if (!brakeType.frontBrakeConfigCount && !brakeType.rearBrakeConfigCount && !brakeType.vehicleToBrakeConfigCount) {
            this.brakeTypeService.getBrakeTypeDetail(brakeType.id).subscribe(m => {
                brakeType.frontBrakeConfigCount = m.frontBrakeConfigCount;
                brakeType.rearBrakeConfigCount = m.rearBrakeConfigCount;
                brakeType.vehicleToBrakeConfigCount = m.vehicleToBrakeConfigCount;

                if (brakeType.frontBrakeConfigCount + brakeType.rearBrakeConfigCount > 0) {
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
            if (brakeType.frontBrakeConfigCount + brakeType.rearBrakeConfigCount > 0) {
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
        if (!this.validationCheck(this.brakeType)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.brakeType.attachments = uploadedFiles;
            }
            if (this.brakeType.attachments) {
                this.brakeType.attachments = this.brakeType.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.brakeTypeService.deleteBrakeType(this.brakeType.id, this.brakeType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Brake Type", ConstantsWarehouse.changeRequestType.remove, this.brakeType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.brakeType.name + "\" BrakeType change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.brakeTypes.filter(x => x.id == this.brakeType.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake Type", ConstantsWarehouse.changeRequestType.remove, this.brakeType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake Type", ConstantsWarehouse.changeRequestType.remove, this.brakeType.name);
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

    view(brakeTypeVm: IBrakeType) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/braketype/" + brakeTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}