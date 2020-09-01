import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {ModalComponent} from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from '../../lib/aclibs/ng2-toastr/ng2-toastr';
import {IBedType} from './bedType.model';
import {BedTypeService} from './bedType.service';
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'bedType-list-component',
    templateUrl: 'app/templates/bedType/bedType-list.component.html',
})

export class BedTypeListComponent {
    bedTypes: IBedType[];
    filteredBedTypes: IBedType[] = [];
    bedType: IBedType = {};
    modifiedBedType: IBedType = {};
    bedTypeNameFilter: string = '';

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

    constructor(private bedTypeService: BedTypeService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.bedTypeService.getAllBedTypes().subscribe(sm => {
            this.bedTypes = sm;
            this.showLoadingGif = false;
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            }
        );
    }

    public applyFilter = (keyword?: string) => {
        this.showLoadingGif = true;
        keyword = keyword || '';
        if (keyword == '') {
            keyword = this.bedTypeNameFilter;
        }
        else {
            this.bedTypeNameFilter = keyword;
        }

        if (String(this.bedTypeNameFilter) === "") {
            this.bedTypeService.getAllBedTypes().subscribe(sm => {
                this.bedTypes = sm;
                this.showLoadingGif = false;
                this.filteredBedTypes = [];
            },
                error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.bedTypeService.getBedTypes(this.bedTypeNameFilter).subscribe(m => {
                this.bedTypes = m;
                this.showLoadingGif = false;
                this.filteredBedTypes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.bedTypeService.getBedTypes(keyword);
    }

    onSelect(bedType: IBedType) {
        this.bedTypeNameFilter = bedType.name;
        this.applyFilter();
        this.filteredBedTypes = [];
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
        this.bedType = {};
        this.newPopup.open("md");
    }

    onNewSubmit() {

        if (!this.validationCheck(this.bedType)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.bedType.attachments = uploadedFiles;
            }
            if (this.bedType.attachments) {
                this.bedType.attachments = this.bedType.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.bedTypeService.addBedType(this.bedType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Bed Type", ConstantsWarehouse.changeRequestType.add, this.bedType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.bedType.name + "\" Bed Type change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Type", ConstantsWarehouse.changeRequestType.add, this.bedType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Type", ConstantsWarehouse.changeRequestType.add, this.bedType.name);
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

    validationCheck(item: IBedType): boolean {
        let isValid = true;
        if (!item.name) {
            this.toastr.warning("Name is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onModify(bedType: IBedType) {
        this.bedType = bedType;
        this.showLoadingGif = true;
        if (!bedType.bedConfigCount && !bedType.vehicleToBedConfigCount) {
            this.bedTypeService.getBedTypeDetail(bedType.id).subscribe(m => {
                bedType.bedConfigCount = m.bedConfigCount;
                bedType.vehicleToBedConfigCount = m.vehicleToBedConfigCount;
                this.modifiedBedType = <IBedType>JSON.parse(JSON.stringify(bedType));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedBedType = <IBedType>JSON.parse(JSON.stringify(bedType));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedBedType)) {
            return;
        }
        else if (this.modifiedBedType.name == this.bedType.name) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedBedType.attachments = uploadedFiles;
            }
            if (this.modifiedBedType.attachments) {
                this.modifiedBedType.attachments = this.modifiedBedType.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.bedTypeService.updateBedType(this.modifiedBedType.id, this.modifiedBedType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Bed Type", ConstantsWarehouse.changeRequestType.modify, this.bedType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.bedType.name + "\" Bed Type change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.bedTypes.filter(x => x.id == this.modifiedBedType.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Type", ConstantsWarehouse.changeRequestType.modify, this.bedType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Type", ConstantsWarehouse.changeRequestType.modify, this.bedType.name);
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

    onDelete(bedType: IBedType) {
        this.bedType = bedType;
        this.showLoadingGif = true;
        if (!bedType.bedConfigCount && !bedType.vehicleToBedConfigCount) {
            this.bedTypeService.getBedTypeDetail(bedType.id).subscribe(m => {
                bedType.bedConfigCount = m.bedConfigCount;
                bedType.vehicleToBedConfigCount = m.vehicleToBedConfigCount;

                if (bedType.bedConfigCount > 0) {
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
            if (bedType.bedConfigCount > 0) {
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
        if (!this.validationCheck(this.bedType)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.bedType.attachments = uploadedFiles;
            }
            if (this.bedType.attachments) {
                this.bedType.attachments = this.bedType.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.bedTypeService.deleteBedType(this.bedType.id, this.bedType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Bed Type", ConstantsWarehouse.changeRequestType.remove, this.bedType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.bedType.name + "\" BedType change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.bedTypes.filter(x => x.id == this.bedType.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Type", ConstantsWarehouse.changeRequestType.remove, this.bedType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Type", ConstantsWarehouse.changeRequestType.remove, this.bedType.name);
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

    view(bedTypeVm: IBedType) {

        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/bedtype/" + bedTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}



