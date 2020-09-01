import { Component, OnInit, ViewChild }      from "@angular/core";
import { ISubModel }                         from "./subModel.model";
import { SubModelService }                   from "./subModel.service";
import { Router }         from "@angular/router";
import { ModalComponent }  from "ng2-bs3-modal/ng2-bs3-modal";
import { ToastsManager }                     from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ConstantsWarehouse }                from "../constants-warehouse";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }                        from "rxjs/Observable";
import { NavigationService }     from "../shared/navigation.service";

@Component({
    selector: 'subModel-list-component',
    templateUrl: 'app/templates/subModel/subModel-list.component.html',
})

export class SubModelListComponent {
    subModels: ISubModel[];
    filteredSubModels: ISubModel[] = [];
    subModel: ISubModel = {};
    modifiedSubModel: ISubModel = {};
    subModelNameFilter: string = '';

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

    constructor(private subModelService: SubModelService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.subModelService.getSubModels().subscribe(sm => {
            this.subModels = sm;
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    public getSuggestions = (keyword: string) => {
        return this.subModelService.getFilteredSubModels(keyword);
    }

    public applyFilter = (keyword?: string) => {
        this.showLoadingGif = true;
        keyword = keyword || '';
        if (keyword == '') {
            keyword = this.subModelNameFilter;
        }
        else {
            this.subModelNameFilter = keyword;
        }

        if (String(this.subModelNameFilter) === "") {
            this.subModelService.getSubModels().subscribe(sm => {
                this.subModels = sm;
                this.showLoadingGif = false;
                this.filteredSubModels = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.subModelService.getFilteredSubModels(this.subModelNameFilter).subscribe(m => {
                this.subModels = m;
                this.showLoadingGif = false;
                this.filteredSubModels = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    onSelect(subModel: ISubModel) {
        this.subModelNameFilter = subModel.name;
        this.filteredSubModels = [];
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
        this.subModel = {};
        this.newPopup.open("md");
    }

    validationCheck(item: ISubModel): boolean {
        if (!item.name) {
            this.toastr.warning("Name is required.", ConstantsWarehouse.validationTitle);
            return false;
        }
        return true;
    }

    onNewSubmit() {
        if (!this.validationCheck(this.subModel)) {
            return;
        }

        this.showLoadingGif = true;
        this.newPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.subModel.attachments = uploadedFiles;
            }
            if (this.subModel.attachments) {
                this.subModel.attachments = this.subModel.attachments.concat(this.newPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.subModelService.addSubModel(this.subModel).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Sub-Model", ConstantsWarehouse.changeRequestType.add, this.subModel.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.subModel.name + "\" Sub Model change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Sub-Model", ConstantsWarehouse.changeRequestType.add, this.subModel.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Sub-Model", ConstantsWarehouse.changeRequestType.add, this.subModel.name);
                this.toastr.warning(error, errorMessage.title);
                this.showLoadingGif = false;
            }), () => {
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

    onModify(subModel: ISubModel) {
        this.subModel = subModel;
        this.showLoadingGif = true;

        if (!subModel.vehicleCount) {
            this.subModelService.getSubModelDetail(subModel.id).subscribe(m => {
                subModel.vehicleCount = m.vehicleCount;
                this.modifiedSubModel = <ISubModel>JSON.parse(JSON.stringify(subModel));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedSubModel = <ISubModel>JSON.parse(JSON.stringify(subModel));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedSubModel)) {
            return;
        }
        if (this.modifiedSubModel.name == this.subModel.name) {
            this.toastr.warning("Nothing has changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedSubModel.attachments = uploadedFiles;
            }
            if (this.modifiedSubModel.attachments) {
                this.modifiedSubModel.attachments = this.modifiedSubModel.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.subModelService.updateSubModel(this.modifiedSubModel.id, this.modifiedSubModel).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Sub-Model", ConstantsWarehouse.changeRequestType.modify, this.subModel.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.subModel.name + "\" Sub Model change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.subModels.filter(x => x.id == this.modifiedSubModel.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Sub-Model", ConstantsWarehouse.changeRequestType.modify, this.subModel.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (errorresponse => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Sub-Model", ConstantsWarehouse.changeRequestType.modify, this.subModel.name);
                this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }), () => {
                this.modifyPopupAcFileUploader.reset(true);
                this.modifyPopup.close();
                this.showLoadingGif = false;
            });
        }, error => {
            this.showLoadingGif = false;
            this.modifyPopupAcFileUploader.reset(true);
            this.modifyPopup.close();
        });
    }

    onDelete(subModel: ISubModel) {
        this.subModel = subModel;
        this.showLoadingGif = true;
        if (!subModel.vehicleCount) {
            this.subModelService.getSubModelDetail(subModel.id).subscribe(m => {
                subModel.vehicleCount = m.vehicleCount;

                if (subModel.vehicleCount > 0) {
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
            if (subModel.vehicleCount > 0) {
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
        if (!this.validationCheck(this.subModel)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.subModel.attachments = uploadedFiles;
            }
            if (this.subModel.attachments) {
                this.subModel.attachments = this.subModel.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.subModelService.deleteSubModel(this.subModel.id, this.subModel).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Sub-Model", ConstantsWarehouse.changeRequestType.remove, this.subModel.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.subModel.name + "\" Sub Model change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.subModels.filter(x => x.id == this.subModel.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Sub-Model", ConstantsWarehouse.changeRequestType.remove, this.subModel.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Sub-Model", ConstantsWarehouse.changeRequestType.remove, this.subModel.name);
                this.toastr.warning(errorMessage.body, errorMessage.title);
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

    view(SubModelVm: ISubModel) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/submodel/" + SubModelVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.newPopupAcFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}