import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {ModalComponent} from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {IBodyType} from './bodyType.model';
import {BodyTypeService} from './bodyType.service';
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'bodyType-list-component',
    templateUrl: 'app/templates/bodyType/bodyType-list.component.html',
})

export class BodyTypeListComponent {
    bodyTypes: IBodyType[];
    filteredBodyTypes: IBodyType[] = [];
    bodyType: IBodyType = {};
    modifiedBodyType: IBodyType = {};
    bodyTypeNameFilter: string = '';

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

    constructor(private bodyTypeService: BodyTypeService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.bodyTypeService.getAllBodyTypes().subscribe(sm => {
            this.bodyTypes = sm;
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
            keyword = this.bodyTypeNameFilter;
        }
        else {
            this.bodyTypeNameFilter = keyword;
        }

        if (String(this.bodyTypeNameFilter) === "") {
            this.bodyTypeService.getAllBodyTypes().subscribe(sm => {
                this.bodyTypes = sm;
                this.showLoadingGif = false;
                this.filteredBodyTypes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.bodyTypeService.getBodyTypes(this.bodyTypeNameFilter).subscribe(m => {
                this.bodyTypes = m;
                this.showLoadingGif = false;
                this.filteredBodyTypes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.bodyTypeService.getBodyTypes(keyword);
    }

    onSelect(bodyType: IBodyType) {
        this.bodyTypeNameFilter = bodyType.name;
        this.applyFilter();
        this.filteredBodyTypes = [];
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
        this.bodyType = {};
        this.newPopup.open("md");
    }

    validationCheck(item: IBodyType): boolean {
        let isValid = true;
        if (!item.name) {
            this.toastr.warning("Name is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onNewSubmit() {
        if (!this.validationCheck(this.bodyType)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.bodyType.attachments = uploadedFiles;
            }
            if (this.bodyType.attachments) {
                this.bodyType.attachments = this.bodyType.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.bodyTypeService.addBodyType(this.bodyType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Body Type", ConstantsWarehouse.changeRequestType.add, this.bodyType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.bodyType.name + "\" Body Type change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Body Type", ConstantsWarehouse.changeRequestType.add, this.bodyType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Body Type", ConstantsWarehouse.changeRequestType.add, this.bodyType.name);
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

    onModify(bodyType: IBodyType) {
        this.bodyType = bodyType;
        this.showLoadingGif = true;
        if (!bodyType.bodyStyleConfigCount && !bodyType.vehicleToBodyStyleConfigCount) {
            this.bodyTypeService.getBodyTypeDetail(bodyType.id).subscribe(m => {
                bodyType.bodyStyleConfigCount = m.bodyStyleConfigCount;
                bodyType.vehicleToBodyStyleConfigCount = m.vehicleToBodyStyleConfigCount;
                this.modifiedBodyType = <IBodyType>JSON.parse(JSON.stringify(bodyType));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedBodyType = <IBodyType>JSON.parse(JSON.stringify(bodyType));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedBodyType)) {
            return;
        }
        else if (this.modifiedBodyType.name == this.bodyType.name) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedBodyType.attachments = uploadedFiles;
            }
            if (this.modifiedBodyType.attachments) {
                this.modifiedBodyType.attachments = this.modifiedBodyType.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.bodyTypeService.updateBodyType(this.modifiedBodyType.id, this.modifiedBodyType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Body Type", ConstantsWarehouse.changeRequestType.modify, this.bodyType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.bodyType.name + "\" Body Type change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.bodyTypes.filter(x => x.id == this.modifiedBodyType.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Body Type", ConstantsWarehouse.changeRequestType.modify, this.bodyType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Body Type", ConstantsWarehouse.changeRequestType.modify, this.bodyType.name);
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

    onDelete(bodyType: IBodyType) {
        this.bodyType = bodyType;
        this.showLoadingGif = true;
        if (!bodyType.bodyStyleConfigCount && !bodyType.vehicleToBodyStyleConfigCount) {
            this.bodyTypeService.getBodyTypeDetail(bodyType.id).subscribe(m => {
                bodyType.bodyStyleConfigCount = m.bodyStyleConfigCount;
                bodyType.vehicleToBodyStyleConfigCount = m.vehicleToBodyStyleConfigCount;

                if (bodyType.bodyStyleConfigCount > 0) {
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
            if (bodyType.bodyStyleConfigCount > 0) {
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
        if (!this.validationCheck(this.bodyType)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.bodyType.attachments = uploadedFiles;
            }
            if (this.bodyType.attachments) {
                this.bodyType.attachments = this.bodyType.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.bodyTypeService.deleteBodyType(this.bodyType.id, this.bodyType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Body Type", ConstantsWarehouse.changeRequestType.remove, this.bodyType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.bodyType.name + "\" Body Type change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.bodyTypes.filter(x => x.id == this.bodyType.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Body Type", ConstantsWarehouse.changeRequestType.remove, this.bodyType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Body Type", ConstantsWarehouse.changeRequestType.remove, this.bodyType.name);
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

    view(bodyTypeVm: IBodyType) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/bodytype/" + bodyTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}