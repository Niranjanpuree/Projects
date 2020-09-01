import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {ModalComponent} from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from '../../lib/aclibs/ng2-toastr/ng2-toastr';
import {IFuelType} from './fuelType.model';
import {FuelTypeService} from './fuelType.service';
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'fuelType-list-component',
    templateUrl: 'app/templates/fuelType/fuelType-list.component.html',
})

export class FuelTypeListComponent {
    fuelTypes: IFuelType[];
    filteredFuelTypes: IFuelType[] = [];
    fuelType: IFuelType = {};
    modifiedFuelType: IFuelType = {};
    fuelTypeNameFilter: string = '';

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

    constructor(private fuelTypeService: FuelTypeService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.fuelTypeService.getAllFuelTypes().subscribe(sm => {
            this.fuelTypes = sm;
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
            keyword = this.fuelTypeNameFilter;
        }
        else {
            this.fuelTypeNameFilter = keyword;
        }

        if (String(this.fuelTypeNameFilter) === "") {
            this.fuelTypeService.getAllFuelTypes().subscribe(sm => {
                this.fuelTypes = sm;
                this.showLoadingGif = false;
                this.filteredFuelTypes = [];
            },
                error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.fuelTypeService.getFuelTypes(this.fuelTypeNameFilter).subscribe(m => {
                this.fuelTypes = m;
                this.showLoadingGif = false;
                this.filteredFuelTypes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.fuelTypeService.getFuelTypes(keyword);
    }

    onSelect(fuelType: IFuelType) {
        this.fuelTypeNameFilter = fuelType.name;
        this.applyFilter();
        this.filteredFuelTypes = [];
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
        this.fuelType = {};
        this.newPopup.open("md");
    }

    onNewSubmit() {

        if (!this.validationCheck(this.fuelType)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.fuelType.attachments = uploadedFiles;
            }
            if (this.fuelType.attachments) {
                this.fuelType.attachments = this.fuelType.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.fuelTypeService.addFuelType(this.fuelType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Fuel Type", ConstantsWarehouse.changeRequestType.add, this.fuelType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.fuelType.name + "\" Engine  Designation change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Fuel Type", ConstantsWarehouse.changeRequestType.add, this.fuelType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Fuel Type", ConstantsWarehouse.changeRequestType.add, this.fuelType.name);
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

    validationCheck(item: IFuelType): boolean {
        let isValid = true;
        if (!item.name) {
            this.toastr.warning("FuelTypeName is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onModify(fuelType: IFuelType) {
         this.fuelType = fuelType;
        this.showLoadingGif = true;
        if (!fuelType.engineConfigCount && !fuelType.vehicleToEngineConfigCount) {
            this.fuelTypeService.getFuelTypeDetail(fuelType.id).subscribe(m => {
                fuelType.engineConfigCount = m.engineConfigCount;
                fuelType.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;
                this.modifiedFuelType = <IFuelType>JSON.parse(JSON.stringify(fuelType));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedFuelType = <IFuelType>JSON.parse(JSON.stringify(fuelType));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedFuelType)) {
            return;
        }
        else if (this.modifiedFuelType.name == this.fuelType.name) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }
        debugger;

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedFuelType.attachments = uploadedFiles;
            }
            if (this.modifiedFuelType.attachments) {
                this.modifiedFuelType.attachments = this.modifiedFuelType.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.fuelTypeService.updateFuelType(this.modifiedFuelType.id, this.modifiedFuelType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Fuel Type", ConstantsWarehouse.changeRequestType.modify, this.fuelType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.fuelType.name + "\" Engine Designation change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.fuelTypes.filter(x => x.id == this.modifiedFuelType.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Fuel Type", ConstantsWarehouse.changeRequestType.modify, this.fuelType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Fuel Type", ConstantsWarehouse.changeRequestType.modify, this.fuelType.name);
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

    onDelete(fuelType: IFuelType) {
        this.fuelType = fuelType;
        this.showLoadingGif = true;
        if (!fuelType.engineConfigCount && !fuelType.vehicleToEngineConfigCount) {
            this.fuelTypeService.getFuelTypeDetail(fuelType.id).subscribe(m => {
                fuelType.engineConfigCount = m.engineConfigCount;
                fuelType.vehicleToEngineConfigCount = m.vehicleToEngineConfigCount;

                if (fuelType.engineConfigCount > 0) {
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
            if (fuelType.engineConfigCount > 0) {
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
        if (!this.validationCheck(this.fuelType)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.fuelType.attachments = uploadedFiles;
            }
            if (this.fuelType.attachments) {
                this.fuelType.attachments = this.fuelType.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.fuelTypeService.deleteFuelType(this.fuelType.id, this.fuelType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Fuel Type", ConstantsWarehouse.changeRequestType.remove, this.fuelType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.fuelType.name + "\" FuelType change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.fuelTypes.filter(x => x.id == this.fuelType.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Fuel Type", ConstantsWarehouse.changeRequestType.remove, this.fuelType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Fuel Type", ConstantsWarehouse.changeRequestType.remove, this.fuelType.name);
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

    view(fuelTypeVm: IFuelType) {

        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/fueltype/" + fuelTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}



