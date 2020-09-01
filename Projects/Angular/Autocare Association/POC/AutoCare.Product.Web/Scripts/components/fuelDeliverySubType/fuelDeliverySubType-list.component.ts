import { Component, OnInit, ViewChild }         from "@angular/core";
import { Router}            from "@angular/router";
import {ToastsManager}                          from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {ModalComponent}       from "ng2-bs3-modal/ng2-bs3-modal";

import { IFuelDeliverySubType} from  "./fuelDeliverySubType.model";
import { FuelDeliverySubTypeService } from "./fuelDeliverySubType.service";
import { ConstantsWarehouse }                   from "../constants-warehouse";
import { AcFileUploader }                       from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { SharedService  }                       from "../shared/shared.service";
import { NavigationService }                    from "../shared/navigation.service";
import {IIdentityInfo}                          from "../shared/shared.model";
import { Observable }                           from 'rxjs/Observable';

@Component({
    selector: 'fuelDeliverySubTypes-list-comp',
    templateUrl: 'app/templates/fuelDeliverySubType/fuelDeliverySubType-list.component.html',
})

export class FuelDeliverySubTypeListComponent implements OnInit {
    fuelDeliverySubType: IFuelDeliverySubType = {}
    selectedFuelDeliverySubType: IFuelDeliverySubType = {};
    fuelDeliverySubTypeList: IFuelDeliverySubType[];
    filteredFuelDeliverySubTypes: IFuelDeliverySubType[] = [];
    originalFuelDeliverySubTypeList: IFuelDeliverySubType[];
    filterTextFuelDeliverySubTypeName: string = '';
    deleteModel: IFuelDeliverySubType = {};
    modifiedModel: IFuelDeliverySubType = {};
    showLoadingGif: boolean = false;

    @ViewChild('addFuelDeliverySubTypeModal')
    addFuelDeliverySubTypeModal: ModalComponent;

    @ViewChild('modifyFuelDeliverySubTypeModal')
    modifyFuelDeliverySubTypeModal: ModalComponent;

    @ViewChild('deleteFuelDeliverySubTypeConfirmModal')
    deleteFuelDeliverySubTypeConfirmModal: ModalComponent;

    @ViewChild('deleteFuelDeliverySubTypeErrorModal')
    deleteFuelDeliverySubTypeErrorModal: ModalComponent;

    @ViewChild('viewFuelDeliverySubTypeChangeRequestModal')
    viewFuelDeliverySubTypeChangeRequestModal: ModalComponent;

    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;

    @ViewChild("modifyPopupAcFileUploader")
    modifyPopupAcFileUploader: AcFileUploader;

    @ViewChild("deletePopupAcFileUploader")
    deletePopupAcFileUploader: AcFileUploader;

    constructor(private fuelDeliverySubTypeService: FuelDeliverySubTypeService, private toastr: ToastsManager, private sharedService: SharedService,
        private router: Router, private navigationService: NavigationService) { }

    ngOnInit() {
        if (this.fuelDeliverySubTypeList && this.fuelDeliverySubTypeList.length > 0) {
            return;
        }
        this.showLoadingGif = true;
        this.fuelDeliverySubTypeService.get().subscribe(fuelDeliverySubTypeList => {
            this.fuelDeliverySubTypeList = fuelDeliverySubTypeList;
            this.originalFuelDeliverySubTypeList = this.fuelDeliverySubTypeList;
            this.showLoadingGif = false;
        }, error => {
            this.showLoadingGif = false;
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
        });
    }

    public applyFilter = (keyword?: string) => {
        this.showLoadingGif = true;
        keyword = keyword || "";
        if (keyword == "") {
            keyword = this.filterTextFuelDeliverySubTypeName;
        }
        else {
            this.filterTextFuelDeliverySubTypeName = keyword;
        }
        if (String(this.filterTextFuelDeliverySubTypeName) === "") {
            this.fuelDeliverySubTypeService.get().subscribe(result => {
                this.fuelDeliverySubTypeList = result;
                this.showLoadingGif = false;
                this.filteredFuelDeliverySubTypes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.fuelDeliverySubTypeService.getByFilter(this.filterTextFuelDeliverySubTypeName).subscribe(m => {
                this.fuelDeliverySubTypeList = m;
                this.showLoadingGif = false;
                this.filteredFuelDeliverySubTypes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.fuelDeliverySubTypeService.getByFilter(keyword);
    }

    onSelect(fuelDeliverySubType: IFuelDeliverySubType) {
        this.filterTextFuelDeliverySubTypeName = fuelDeliverySubType.name;
        this.applyFilter();
        this.filteredFuelDeliverySubTypes = [];
    }

    validationCheck(item: IFuelDeliverySubType): boolean {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("FuelDeliverySubType Name is required.", ConstantsWarehouse.validationTitle);
                return false;
            }
            if (this.fuelDeliverySubType.changeType == "Modify") {
                if (item.name === this.selectedFuelDeliverySubType.name) {
                    this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
                    return false;
                }
            }
        }
        return true;
    }

    onCancel(action: string) {
        this.acFileUploader.reset(true);
        this.addFuelDeliverySubTypeModal.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyFuelDeliverySubTypeModal.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteFuelDeliverySubTypeConfirmModal.close();
    }

    add() {
        this.fuelDeliverySubType.changeType = "Add";
        if (!this.validationCheck(this.fuelDeliverySubType)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.fuelDeliverySubType.attachments = uploadedFiles;
            }
            if (this.fuelDeliverySubType.attachments) {
                this.fuelDeliverySubType.attachments = this.fuelDeliverySubType.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.fuelDeliverySubTypeService.add(this.fuelDeliverySubType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("FuelDeliverySubType", ConstantsWarehouse.changeRequestType.add, this.fuelDeliverySubType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.fuelDeliverySubType.name + "\" FuelDeliverySubType change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("FuelDeliverySubType", ConstantsWarehouse.changeRequestType.add, this.fuelDeliverySubType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("FuelDeliverySubType", ConstantsWarehouse.changeRequestType.add, this.fuelDeliverySubType.name);
                this.toastr.warning(errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.showLoadingGif = false;
                this.acFileUploader.reset(true);
                this.addFuelDeliverySubTypeModal.close();
            });

        }, error => {
            this.showLoadingGif = false;
            this.acFileUploader.reset(true);
            this.addFuelDeliverySubTypeModal.close();
        });
    }

    modify(fuelDeliverySubTypeVm: IFuelDeliverySubType) {
        this.showLoadingGif = true;
        this.selectedFuelDeliverySubType = fuelDeliverySubTypeVm;
        this.fuelDeliverySubType.id = this.selectedFuelDeliverySubType.id;
        this.fuelDeliverySubType.name = this.selectedFuelDeliverySubType.name;
        this.fuelDeliverySubType.comment = "";
        this.modifyFuelDeliverySubTypeModal.open("md");
        if (this.selectedFuelDeliverySubType.fuelDeliveryConfigCount == 0) {
            this.fuelDeliverySubTypeService.getById(this.selectedFuelDeliverySubType.id).subscribe(
                m => {
                    this.selectedFuelDeliverySubType.fuelDeliveryConfigCount = this.fuelDeliverySubType.fuelDeliveryConfigCount = m.fuelDeliveryConfigCount;
                    //this.selectedMake.vehicleCount = this.make.vehicleCount = m.vehicleCount;
                    this.showLoadingGif = false;
                },
                error => {
                    this.toastr.warning(<any>error.toString(), "Load Failed");
                }
            );
        } else {
            this.showLoadingGif = false;
        }
    }

    modifySubmit() {
        this.fuelDeliverySubType.changeType = "Modify";
        if (!this.validationCheck(this.fuelDeliverySubType)) {
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.fuelDeliverySubType.attachments = uploadedFiles;
            }
            if (this.fuelDeliverySubType.attachments) {
                this.fuelDeliverySubType.attachments = this.fuelDeliverySubType.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.fuelDeliverySubTypeService.update(this.selectedFuelDeliverySubType.id, this.fuelDeliverySubType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("FuelDeliverySubType", ConstantsWarehouse.changeRequestType.modify, this.selectedFuelDeliverySubType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.fuelDeliverySubType.name + "\" FuelDeliverySubType change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.fuelDeliverySubTypeList.filter(x => x.id == this.selectedFuelDeliverySubType.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("FuelDeliverySubType", ConstantsWarehouse.changeRequestType.modify, this.selectedFuelDeliverySubType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (errorresponse => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("FuelDeliverySubType", ConstantsWarehouse.changeRequestType.modify, this.selectedFuelDeliverySubType.name);
                this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }), () => {
                this.showLoadingGif = false;
                this.modifyPopupAcFileUploader.reset(true);
                this.modifyFuelDeliverySubTypeModal.close();

            });
        }, error => {
            this.showLoadingGif = false;
            this.modifyPopupAcFileUploader.reset(false);
            this.modifyFuelDeliverySubTypeModal.close();

        });
    }

    delete(fuelDeliverySubTypeVm: IFuelDeliverySubType) {
        this.showLoadingGif = true;
        this.selectedFuelDeliverySubType = fuelDeliverySubTypeVm;

        this.fuelDeliverySubType.id = this.selectedFuelDeliverySubType.id;
        this.fuelDeliverySubType.name = this.selectedFuelDeliverySubType.name;
        this.fuelDeliverySubType.comment = "";

        if (this.selectedFuelDeliverySubType.fuelDeliveryConfigCount == 0 /*&& this.selectedMake.vehicleCount == 0*/) {
            this.fuelDeliverySubTypeService.getById(this.selectedFuelDeliverySubType.id).subscribe(
                m => {
                    this.selectedFuelDeliverySubType.fuelDeliveryConfigCount = m.fuelDeliveryConfigCount;
                    //this.selectedMake.vehicleCount = m.vehicleCount;

                    if (this.selectedFuelDeliverySubType.fuelDeliveryConfigCount == 0 /*&& this.selectedMake.vehicleCount == 0*/) {
                        this.showLoadingGif = false;
                        this.deleteFuelDeliverySubTypeConfirmModal.open("md");
                    }
                    else {
                        this.showLoadingGif = false;
                        this.deleteFuelDeliverySubTypeErrorModal.open("sm");
                    }
                },
                error => {
                    this.showLoadingGif = false;
                    this.toastr.warning(<any>error.toString(), "Load Failed");
                }
            );
        }
        else {
            if (this.selectedFuelDeliverySubType.fuelDeliveryConfigCount == 0 /*&& this.selectedFuelDeliverySubType.vehicleCount == 0*/) {
                this.showLoadingGif = false;
                this.deleteFuelDeliverySubTypeConfirmModal.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteFuelDeliverySubTypeErrorModal.open("sm");
            }
        }
    }

    deleteSubmit() {
        this.selectedFuelDeliverySubType.changeType = "Delete";
        if (!this.validationCheck(this.selectedFuelDeliverySubType)) {
            return;
        }
        this.deleteFuelDeliverySubTypeConfirmModal.close();

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.selectedFuelDeliverySubType.attachments = uploadedFiles;
            }
            if (this.fuelDeliverySubType.attachments) {
                this.selectedFuelDeliverySubType.attachments = this.selectedFuelDeliverySubType.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());

            }
            this.fuelDeliverySubTypeService.delete(this.selectedFuelDeliverySubType.id, this.selectedFuelDeliverySubType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("FuelDeliverySubType", ConstantsWarehouse.changeRequestType.remove, this.selectedFuelDeliverySubType.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.fuelDeliverySubType.name + "\" FuelDeliverySubType change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.fuelDeliverySubTypeList.filter(x => x.id == this.selectedFuelDeliverySubType.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("FuelDeliverySubType", ConstantsWarehouse.changeRequestType.remove, this.selectedFuelDeliverySubType.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                this.ngOnInit();
            }, (errorresponses => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("FuelDeliverySubType", ConstantsWarehouse.changeRequestType.remove, this.selectedFuelDeliverySubType.name);
                this.toastr.warning(errorresponses, errorMessage.title);
                this.showLoadingGif = false;
            }), () => {
                this.showLoadingGif = false;
                this.deletePopupAcFileUploader.reset(true);
                this.deleteFuelDeliverySubTypeConfirmModal.close();
            });

        }, error => {
            this.showLoadingGif = false;
            this.deletePopupAcFileUploader.reset(true);
            this.deleteFuelDeliverySubTypeConfirmModal.close();
        });
    }

    view(fuelDeliverySubTypeVm: IFuelDeliverySubType) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/fuelDeliverySubType/" + fuelDeliverySubTypeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    openAddFuelDeliverySubTypeModal() {
        this.fuelDeliverySubType.id = 0;
        this.fuelDeliverySubType.name = "";
        this.fuelDeliverySubType.comment = "";

        this.addFuelDeliverySubTypeModal.open("md");
    }

    setSelectedFuelDeliverySubType(fuelDeliverySubType: IFuelDeliverySubType) {
        let selectedFuelDeliverySubType = fuelDeliverySubType;
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}