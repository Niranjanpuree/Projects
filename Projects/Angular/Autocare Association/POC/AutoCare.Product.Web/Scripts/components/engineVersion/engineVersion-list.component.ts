import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {ModalComponent} from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from '../../lib/aclibs/ng2-toastr/ng2-toastr';
import {IEngineVersion} from './engineVersion.model';
import {EngineVersionService} from './engineVersion.service';
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'engineVersion-list-component',
    templateUrl: 'app/templates/engineVersion/engineVersion-list.component.html',
})

export class EngineVersionListComponent {
    engineVersions: IEngineVersion[];
    filteredEngineVersions: IEngineVersion[] = [];
    engineVersion: IEngineVersion = {};
    modifiedEngineVersion: IEngineVersion = {};
    engineVersionNameFilter: string = '';

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

    constructor(private engineVersionService: EngineVersionService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.engineVersionService.getAllEngineVersions().subscribe(sm => {
            this.engineVersions = sm;
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
            keyword = this.engineVersionNameFilter;
        }
        else {
            this.engineVersionNameFilter = keyword;
        }

        if (String(this.engineVersionNameFilter) === "") {
            this.engineVersionService.getAllEngineVersions().subscribe(sm => {
                this.engineVersions = sm;
                this.showLoadingGif = false;
                this.filteredEngineVersions = [];
            },
                error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.engineVersionService.getEngineVersions(this.engineVersionNameFilter).subscribe(m => {
                this.engineVersions = m;
                this.showLoadingGif = false;
                this.filteredEngineVersions = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.engineVersionService.getEngineVersions(keyword);
    }

    onSelect(engineVersion: IEngineVersion) {
        this.engineVersionNameFilter = engineVersion.engineVersionName;
        this.applyFilter();
        this.filteredEngineVersions = [];
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
        this.engineVersion = {};
        this.newPopup.open("md");
    }

    onNewSubmit() {

        if (!this.validationCheck(this.engineVersion)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.engineVersion.attachments = uploadedFiles;
            }
            if (this.engineVersion.attachments) {
                this.engineVersion.attachments = this.engineVersion.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.engineVersionService.addEngineVersion(this.engineVersion).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Engine  Version", ConstantsWarehouse.changeRequestType.add, this.engineVersion.engineVersionName);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.engineVersion.engineVersionName + "\" Engine  Version change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Engine Version", ConstantsWarehouse.changeRequestType.add, this.engineVersion.engineVersionName);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Engine Version", ConstantsWarehouse.changeRequestType.add, this.engineVersion.engineVersionName);
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

    validationCheck(item: IEngineVersion): boolean {
        let isValid = true;
        if (!item.engineVersionName) {
            this.toastr.warning("EngineVersionName is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onModify(engineVersion: IEngineVersion) {
        debugger;
        this.engineVersion = engineVersion;
        this.showLoadingGif = true;
        if (!engineVersion.engineConfigCount && !engineVersion.vehicleToEngineConfigCount) {
            this.engineVersionService.getEngineVersionDetail(engineVersion.engineVersionId).subscribe(m => {
                engineVersion.engineConfigCount = m.engineConfigCount;
                engineVersion.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;
                this.modifiedEngineVersion = <IEngineVersion>JSON.parse(JSON.stringify(engineVersion));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedEngineVersion = <IEngineVersion>JSON.parse(JSON.stringify(engineVersion));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedEngineVersion)) {
            return;
        }
        else if (this.modifiedEngineVersion.engineVersionName == this.engineVersion.engineVersionName) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedEngineVersion.attachments = uploadedFiles;
            }
            if (this.modifiedEngineVersion.attachments) {
                this.modifiedEngineVersion.attachments = this.modifiedEngineVersion.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.engineVersionService.updateEngineVersion(this.modifiedEngineVersion.engineVersionId, this.modifiedEngineVersion).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Engine Version", ConstantsWarehouse.changeRequestType.modify, this.engineVersion.engineVersionName);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.engineVersion.engineVersionName + "\" Engine Version change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.engineVersions.filter(x => x.engineVersionId == this.modifiedEngineVersion.engineVersionId)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Engine Version", ConstantsWarehouse.changeRequestType.modify, this.engineVersion.engineVersionName);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Engine Version", ConstantsWarehouse.changeRequestType.modify, this.engineVersion.engineVersionName);
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

    onDelete(engineVersion: IEngineVersion) {
        this.engineVersion = engineVersion;
        this.showLoadingGif = true;
        if (!engineVersion.engineConfigCount && !engineVersion.vehicleToEngineConfigCount) {
            this.engineVersionService.getEngineVersionDetail(engineVersion.engineVersionId).subscribe(m => {
                engineVersion.engineConfigCount = m.engineConfigCount;
                engineVersion.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;

                if (engineVersion.engineConfigCount > 0) {
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
            if (engineVersion.engineConfigCount > 0) {
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
        if (!this.validationCheck(this.engineVersion)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.engineVersion.attachments = uploadedFiles;
            }
            if (this.engineVersion.attachments) {
                this.engineVersion.attachments = this.engineVersion.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.engineVersionService.deleteEngineVersion(this.engineVersion.engineVersionId, this.engineVersion).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Engine Version", ConstantsWarehouse.changeRequestType.remove, this.engineVersion.engineVersionName);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.engineVersion.engineVersionName + "\" EngineVersion change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.engineVersions.filter(x => x.engineVersionId == this.engineVersion.engineVersionId)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Engine Version", ConstantsWarehouse.changeRequestType.remove, this.engineVersion.engineVersionName);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Engine Version", ConstantsWarehouse.changeRequestType.remove, this.engineVersion.engineVersionName);
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

    view(engineVersionVm: IEngineVersion) {

        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/engineversion/" + engineVersionVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}



