import { Component, OnInit, ViewChild } from '@angular/core';
import { Router}            from "@angular/router";
import { IModel } from './model.model';
import { IVehicleType } from '../vehicleType/vehicleType.model';
import { VehicleTypeService } from '../vehicleType/vehicleType.service';
import { ModelService } from './model.service';
import { ModalComponent } from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ConstantsWarehouse } from '../constants-warehouse';
import { AcFileUploader }                       from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable } from 'rxjs/Observable';
import { NavigationService }     from "../shared/navigation.service";

@Component({
    selector: 'model-list-component',
    templateUrl: 'app/templates/model/model-list.component.html',
})

export class ModelListComponent implements OnInit {
    models: IModel[];
    filteredModels: IModel[] = [];
    vehicleTypes: IVehicleType[];
    model: IModel = {};
    modifiedModel: IModel = {};
    deleteModel: IModel = {};
    modelNameFilter: string = '';

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

    modelData: any[];

    showLoadingGif: boolean = false;

    constructor(private modelService: ModelService, private vehicleTypeService: VehicleTypeService,
        private toastr: ToastsManager, private router: Router, private navigationService: NavigationService) {
    }

    ngOnInit() {
        this.showLoadingGif = true;
        this.modelService.getModels().subscribe(m => {
            this.models = m;
            // load vehicletype
            this.vehicleTypeService.getAllVehicleTypes().subscribe(m => {
                this.vehicleTypes = m;
                this.showLoadingGif = false;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    public getSuggestions = (keyword: string) => {
        return this.modelService.getFilteredModels(keyword);
    }

    public applyFilter = (keyword?: string) => {
        this.showLoadingGif = true;
        keyword = keyword || '';
        if (keyword == '') {
            keyword = this.modelNameFilter;
        }
        else {
            this.modelNameFilter = keyword;
        }

        if (String(this.modelNameFilter) === "") {
            this.modelService.getModels().subscribe(m => {
                this.models = m;
                this.showLoadingGif = false;
                this.filteredModels = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.modelService.getFilteredModels(this.modelNameFilter).subscribe(m => {
                this.models = m;
                this.showLoadingGif = false;
                this.filteredModels = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    onCancel(action: string) {
        this.newPopupAcFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
    }

    onSelect(model: IModel) {
        this.modelNameFilter = model.name;
        this.filteredModels = [];
    }

    validationCheck(item: IModel): boolean {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("Model Name is required.", ConstantsWarehouse.validationTitle);
                return false;
            }
            if (<Number>item.vehicleTypeId === -1) {
                this.toastr.warning("Please select vehicle type.", ConstantsWarehouse.validationTitle);
                return false;
            }
        }

        if (item.changeType == "Modify") {
            if (item.name === this.model.name && item.vehicleTypeId === this.model.vehicleTypeId) {
                this.toastr.warning("Nothing has changed", ConstantsWarehouse.validationTitle);
                return false;
            }
        }
        return true;
    }

    onNew() {
        this.model = {}
        this.model.vehicleTypeId = -1;
        this.newPopup.open("md");
    }

    onNewSubmit() {
        this.model.changeType = "Add";
        if (!this.validationCheck(this.model)) {
            return;
        }

        this.showLoadingGif = true;
        this.newPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.model.attachments = uploadedFiles;
            }
            if (this.model.attachments) {
                this.model.attachments = this.model.attachments.concat(this.newPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.modelService.addModel(this.model).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Model", ConstantsWarehouse.changeRequestType.add, this.model.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.model.name + "\" Model change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Model", ConstantsWarehouse.changeRequestType.add, this.model.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (errorresponse => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Model", ConstantsWarehouse.changeRequestType.add, this.model.name);
                this.toastr.warning(errorresponse, errorMessage.title);
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

    onModify(model: IModel) {
        this.showLoadingGif = true;
        this.model = model;

        if (!model.baseVehicleCount && !model.vehicleCount) {
            this.modelService.getModelDetail(model.id).subscribe(m => {
                model.baseVehicleCount = m.baseVehicleCount;
                model.vehicleCount = m.vehicleCount;
                this.modifiedModel = <IModel>JSON.parse(JSON.stringify(model));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedModel = <IModel>JSON.parse(JSON.stringify(model));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        this.modifiedModel.changeType = "Modify";
        if (!this.validationCheck(this.modifiedModel)) {
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedModel.attachments = uploadedFiles;
            }
            if (this.modifiedModel.attachments) {
                this.modifiedModel.attachments = this.modifiedModel.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.modelService.updateModel(this.modifiedModel.id, this.modifiedModel).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Model", ConstantsWarehouse.changeRequestType.modify, this.model.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.model.name + "\" Model change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.models.filter(x => x.id == this.modifiedModel.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Model", ConstantsWarehouse.changeRequestType.modify, this.model.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                this.showLoadingGif = false;
            }, (errorresponse => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Model", ConstantsWarehouse.changeRequestType.modify, this.model.name);
                this.toastr.warning(errorresponse, errorMessage.title);
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

    onDelete(model: IModel) {
        this.model = model;
        this.deleteModel.comment = "";
        this.showLoadingGif = true;

        if (!model.baseVehicleCount && !model.vehicleCount) {
            this.modelService.getModelDetail(model.id).subscribe(m => {
                model.baseVehicleCount = m.baseVehicleCount;
                model.vehicleCount = m.vehicleCount;

                if (model.baseVehicleCount > 0) {
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
            if (model.baseVehicleCount > 0) {
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
        this.deleteModel.changeType = "Delete";
        if (!this.validationCheck(this.deleteModel)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteModel.attachments = uploadedFiles;
            }
            if (this.deleteModel.attachments) {
                this.deleteModel.attachments = this.deleteModel.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.modelService.deleteModel(this.model.id, this.deleteModel).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Model", ConstantsWarehouse.changeRequestType.remove, this.model.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.model.name + "\" Model change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.models.filter(x => x.id == this.model.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Model", ConstantsWarehouse.changeRequestType.remove, this.model.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (errorresponse => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Model", ConstantsWarehouse.changeRequestType.remove, this.model.name);
                this.toastr.warning(errorresponse, errorMessage.title);
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

    view(ModelVm: IModel) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/model/" + ModelVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.newPopupAcFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}