import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {ModalComponent} from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {IBodyNumDoors} from './bodyNumDoors.model';
import {BodyNumDoorsService} from './bodyNumDoors.service';
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'bodyNumDoors-list-component',
    templateUrl: 'app/templates/bodyNumDoors/bodyNumDoors-list.component.html',
})

export class BodyNumDoorsListComponent {
    bodyNumDoorses: IBodyNumDoors[];
    filteredBodyNumDoors: IBodyNumDoors[] = [];
    bodyNumDoors: IBodyNumDoors = {};
    modifiedBodyNumDoors: IBodyNumDoors = {};
    bodyNumDoorsNameFilter: string = '';

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

    constructor(private bodyNumDoorsService: BodyNumDoorsService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.bodyNumDoorsService.getAllBodyNumDoors().subscribe(sm => {
            this.bodyNumDoorses = sm;
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
            keyword = this.bodyNumDoorsNameFilter;
        }
        else {
            this.bodyNumDoorsNameFilter = keyword;
        }

        if (String(this.bodyNumDoorsNameFilter) === "") {
            this.bodyNumDoorsService.getAllBodyNumDoors().subscribe(sm => {
                this.bodyNumDoorses = sm;
                this.showLoadingGif = false;
                this.filteredBodyNumDoors = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.bodyNumDoorsService.getBodyNumDoors(this.bodyNumDoorsNameFilter).subscribe(m => {
                this.bodyNumDoorses = m;
                this.showLoadingGif = false;
                this.filteredBodyNumDoors = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.bodyNumDoorsService.getBodyNumDoors(keyword);
    }

    onSelect(bodyNumDoors: IBodyNumDoors) {
        this.bodyNumDoorsNameFilter = bodyNumDoors.numDoors;
        this.applyFilter();
        this.filteredBodyNumDoors = [];
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
        this.bodyNumDoors = {};
        this.newPopup.open("md");
    }

    validationCheck(item: IBodyNumDoors): boolean {
        let isValid = true;
        if (!item.numDoors) {
            this.toastr.warning("NumDoors is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onNewSubmit() {
        if (!this.validationCheck(this.bodyNumDoors)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.bodyNumDoors.attachments = uploadedFiles;
            }
            if (this.bodyNumDoors.attachments) {
                this.bodyNumDoors.attachments = this.bodyNumDoors.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.bodyNumDoorsService.addBodyNumDoors(this.bodyNumDoors).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Body NumDoors", ConstantsWarehouse.changeRequestType.add, this.bodyNumDoors.numDoors);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.bodyNumDoors.numDoors + "\" Body NumDoors change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Body NumDoors", ConstantsWarehouse.changeRequestType.add, this.bodyNumDoors.numDoors);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Body NumDoors", ConstantsWarehouse.changeRequestType.add, this.bodyNumDoors.numDoors);
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

    onModify(bodyNumDoors: IBodyNumDoors) {
        this.bodyNumDoors = bodyNumDoors;
        this.showLoadingGif = true;
        if (!bodyNumDoors.bodyStyleConfigCount && !bodyNumDoors.vehicleToBodyStyleConfigCount) {
            this.bodyNumDoorsService.getBodyNumDoorsDetail(bodyNumDoors.id).subscribe(m => {
                bodyNumDoors.bodyStyleConfigCount = m.bodyStyleConfigCount;
                bodyNumDoors.vehicleToBodyStyleConfigCount = m.vehicleToBodyStyleConfigCount;
                this.modifiedBodyNumDoors = <IBodyNumDoors>JSON.parse(JSON.stringify(bodyNumDoors));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedBodyNumDoors = <IBodyNumDoors>JSON.parse(JSON.stringify(bodyNumDoors));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedBodyNumDoors)) {
            return;
        }
        else if (this.modifiedBodyNumDoors.numDoors == this.bodyNumDoors.numDoors) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedBodyNumDoors.attachments = uploadedFiles;
            }
            if (this.modifiedBodyNumDoors.attachments) {
                this.modifiedBodyNumDoors.attachments = this.modifiedBodyNumDoors.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.bodyNumDoorsService.updateBodyNumDoors(this.modifiedBodyNumDoors.id, this.modifiedBodyNumDoors).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Body NumDoors", ConstantsWarehouse.changeRequestType.modify, this.bodyNumDoors.numDoors);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.bodyNumDoors.numDoors + "\" Body NumDoors change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.bodyNumDoorses.filter(x => x.id == this.modifiedBodyNumDoors.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Body NumDoors", ConstantsWarehouse.changeRequestType.modify, this.bodyNumDoors.numDoors);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Body NumDoors", ConstantsWarehouse.changeRequestType.modify, this.bodyNumDoors.numDoors);
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

    onDelete(bodyNumDoors: IBodyNumDoors) {
        this.bodyNumDoors = bodyNumDoors;
        this.showLoadingGif = true;
        if (!bodyNumDoors.bodyStyleConfigCount && !bodyNumDoors.vehicleToBodyStyleConfigCount) {
            this.bodyNumDoorsService.getBodyNumDoorsDetail(bodyNumDoors.id).subscribe(m => {
                bodyNumDoors.bodyStyleConfigCount = m.bodyStyleConfigCount;
                bodyNumDoors.vehicleToBodyStyleConfigCount = m.vehicleToBodyStyleConfigCount;

                if (bodyNumDoors.bodyStyleConfigCount > 0) {
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
            if (bodyNumDoors.bodyStyleConfigCount > 0) {
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
        if (!this.validationCheck(this.bodyNumDoors)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.bodyNumDoors.attachments = uploadedFiles;
            }
            if (this.bodyNumDoors.attachments) {
                this.bodyNumDoors.attachments = this.bodyNumDoors.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.bodyNumDoorsService.deleteBodyNumDoors(this.bodyNumDoors.id, this.bodyNumDoors).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Body NumDoors", ConstantsWarehouse.changeRequestType.remove, this.bodyNumDoors.numDoors);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.bodyNumDoors.numDoors + "\" Body NumDoors change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.bodyNumDoorses.filter(x => x.id == this.bodyNumDoors.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Body NumDoors", ConstantsWarehouse.changeRequestType.remove, this.bodyNumDoors.numDoors);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Body NumDoors", ConstantsWarehouse.changeRequestType.remove, this.bodyNumDoors.numDoors);
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

    view(bodyNumDoorsVm: IBodyNumDoors) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/bodynumdoors/" + bodyNumDoorsVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}