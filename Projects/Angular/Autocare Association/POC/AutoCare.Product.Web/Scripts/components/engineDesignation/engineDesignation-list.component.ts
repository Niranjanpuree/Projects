import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {ModalComponent} from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from '../../lib/aclibs/ng2-toastr/ng2-toastr';
import {IEngineDesignation} from './engineDesignation.model';
import {EngineDesignationService} from './engineDesignation.service';
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'engineDesignation-list-component',
    templateUrl: 'app/templates/engineDesignation/engineDesignation-list.component.html',
})

export class EngineDesignationListComponent {
    engineDesignations: IEngineDesignation[];
    filteredEngineDesignations: IEngineDesignation[] = [];
    engineDesignation: IEngineDesignation = {};
    modifiedEngineDesignation: IEngineDesignation = {};
    engineDesignationNameFilter: string = '';

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

    constructor(private engineDesignationService: EngineDesignationService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.engineDesignationService.getAllEngineDesignations().subscribe(sm => {
            this.engineDesignations = sm;
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
            keyword = this.engineDesignationNameFilter;
        }
        else {
            this.engineDesignationNameFilter = keyword;
        }

        if (String(this.engineDesignationNameFilter) === "") {
            this.engineDesignationService.getAllEngineDesignations().subscribe(sm => {
                this.engineDesignations = sm;
                this.showLoadingGif = false;
                this.filteredEngineDesignations = [];
            },
                error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.engineDesignationService.getEngineDesignations(this.engineDesignationNameFilter).subscribe(m => {
                this.engineDesignations = m;
                this.showLoadingGif = false;
                this.filteredEngineDesignations = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.engineDesignationService.getEngineDesignations(keyword);
    }

    onSelect(engineDesignation: IEngineDesignation) {
        this.engineDesignationNameFilter = engineDesignation.engineDesignationName;
        this.applyFilter();
        this.filteredEngineDesignations = [];
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
        this.engineDesignation = {};
        this.newPopup.open("md");
    }

    onNewSubmit() {

        if (!this.validationCheck(this.engineDesignation)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.engineDesignation.attachments = uploadedFiles;
            }
            if (this.engineDesignation.attachments) {
                this.engineDesignation.attachments = this.engineDesignation.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.engineDesignationService.addEngineDesignation(this.engineDesignation).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Engine  Designation", ConstantsWarehouse.changeRequestType.add, this.engineDesignation.engineDesignationName);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.engineDesignation.engineDesignationName + "\" Engine  Designation change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Engine Designation", ConstantsWarehouse.changeRequestType.add, this.engineDesignation.engineDesignationName);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Engine Designation", ConstantsWarehouse.changeRequestType.add, this.engineDesignation.engineDesignationName);
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

    validationCheck(item: IEngineDesignation): boolean {
        let isValid = true;
        if (!item.engineDesignationName) {
            this.toastr.warning("EngineDesignationName is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onModify(engineDesignation: IEngineDesignation) {
        debugger;
        this.engineDesignation = engineDesignation;
        this.showLoadingGif = true;
        if (!engineDesignation.engineConfigCount && !engineDesignation.vehicleToEngineConfigCount) {
            this.engineDesignationService.getEngineDesignationDetail(engineDesignation.engineDesignationId).subscribe(m => {
                engineDesignation.engineConfigCount = m.engineConfigCount;
                engineDesignation.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;
                this.modifiedEngineDesignation = <IEngineDesignation>JSON.parse(JSON.stringify(engineDesignation));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedEngineDesignation = <IEngineDesignation>JSON.parse(JSON.stringify(engineDesignation));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedEngineDesignation)) {
            return;
        }
        else if (this.modifiedEngineDesignation.engineDesignationName == this.engineDesignation.engineDesignationName) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedEngineDesignation.attachments = uploadedFiles;
            }
            if (this.modifiedEngineDesignation.attachments) {
                this.modifiedEngineDesignation.attachments = this.modifiedEngineDesignation.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.engineDesignationService.updateEngineDesignation(this.modifiedEngineDesignation.engineDesignationId, this.modifiedEngineDesignation).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Engine Designation", ConstantsWarehouse.changeRequestType.modify, this.engineDesignation.engineDesignationName);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.engineDesignation.engineDesignationName + "\" Engine Designation change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.engineDesignations.filter(x => x.engineDesignationId == this.modifiedEngineDesignation.engineDesignationId)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Engine Designation", ConstantsWarehouse.changeRequestType.modify, this.engineDesignation.engineDesignationName);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Engine Designation", ConstantsWarehouse.changeRequestType.modify, this.engineDesignation.engineDesignationName);
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

    onDelete(engineDesignation: IEngineDesignation) {
        this.engineDesignation = engineDesignation;
        this.showLoadingGif = true;
        if (!engineDesignation.engineConfigCount && !engineDesignation.vehicleToEngineConfigCount) {
            this.engineDesignationService.getEngineDesignationDetail(engineDesignation.engineDesignationId).subscribe(m => {
                engineDesignation.engineConfigCount = m.engineConfigCount;
                engineDesignation.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;

                if (engineDesignation.engineConfigCount > 0) {
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
            if (engineDesignation.engineConfigCount > 0) {
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
        if (!this.validationCheck(this.engineDesignation)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.engineDesignation.attachments = uploadedFiles;
            }
            if (this.engineDesignation.attachments) {
                this.engineDesignation.attachments = this.engineDesignation.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.engineDesignationService.deleteEngineDesignation(this.engineDesignation.engineDesignationId, this.engineDesignation).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Engine Designation", ConstantsWarehouse.changeRequestType.remove, this.engineDesignation.engineDesignationName);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.engineDesignation.engineDesignationName + "\" EngineDesignation change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.engineDesignations.filter(x => x.engineDesignationId == this.engineDesignation.engineDesignationId)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Engine Designation", ConstantsWarehouse.changeRequestType.remove, this.engineDesignation.engineDesignationName);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Engine Designation", ConstantsWarehouse.changeRequestType.remove, this.engineDesignation.engineDesignationName);
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

    view(engineDesignationVm: IEngineDesignation) {

        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/enginedesignation/" + engineDesignationVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}



