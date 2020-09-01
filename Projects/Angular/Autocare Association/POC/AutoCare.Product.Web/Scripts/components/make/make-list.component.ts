import { Component, OnInit, ViewChild }         from "@angular/core";
import { Router}            from "@angular/router";
import {ToastsManager}                          from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {ModalComponent}       from "ng2-bs3-modal/ng2-bs3-modal";

import { IMake }                                from "./make.model";
import { MakeService }                          from "./make.service";
import { ConstantsWarehouse }                   from "../constants-warehouse";
import { AcFileUploader }                       from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { SharedService  }                       from "../shared/shared.service";
import { NavigationService }                    from "../shared/navigation.service";
import {IIdentityInfo}                          from "../shared/shared.model";
import { Observable }                           from 'rxjs/Observable';

@Component({
    selector: 'makes-list-comp',
    templateUrl: 'app/templates/make/make-list.component.html',
})

export class MakeListComponent implements OnInit {
    make: IMake = {}
    selectedMake: IMake = {};
    makeList: IMake[];
    filteredMakes: IMake[] = [];
    originalMakeList: IMake[];
    filterTextMakeName: string = '';
    deleteModel: IMake = {};
    modifiedModel: IMake = {};
    showLoadingGif: boolean = false;

    @ViewChild('addMakeModal')
    addMakeModal: ModalComponent;

    @ViewChild('modifyMakeModal')
    modifyMakeModal: ModalComponent;

    @ViewChild('deleteMakeConfirmModal')
    deleteMakeConfirmModal: ModalComponent;

    @ViewChild('deleteMakeErrorModal')
    deleteMakeErrorModal: ModalComponent;

    @ViewChild('viewMakeChangeRequestModal')
    viewMakeChangeRequestModal: ModalComponent;

    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;

    @ViewChild("modifyPopupAcFileUploader")
    modifyPopupAcFileUploader: AcFileUploader;

    @ViewChild("deletePopupAcFileUploader")
    deletePopupAcFileUploader: AcFileUploader;

    constructor(private makeService: MakeService, private toastr: ToastsManager, private sharedService: SharedService,
        private router: Router, private navigationService: NavigationService) { }

    ngOnInit() {
        if (this.makeList && this.makeList.length > 0) {
            return;
        }
        this.showLoadingGif = true;
        this.makeService.get().subscribe(makeList => {
            this.makeList = makeList;
            this.originalMakeList = this.makeList;
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
            keyword = this.filterTextMakeName;
        }
        else {
            this.filterTextMakeName = keyword;
        }
        if (String(this.filterTextMakeName) === "") {
            this.makeService.get().subscribe(result => {
                this.makeList = result;
                this.showLoadingGif = false;
                this.filteredMakes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.makeService.getByFilter(this.filterTextMakeName).subscribe(m => {
                this.makeList = m;
                this.showLoadingGif = false;
                this.filteredMakes = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.makeService.getByFilter(keyword);
    }

    onSelect(make: IMake) {
        this.filterTextMakeName = make.name;
        this.applyFilter();
        this.filteredMakes = [];
    }

    validationCheck(item: IMake): boolean {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("Make Name is required.", ConstantsWarehouse.validationTitle);
                return false;
            }
            if (this.make.changeType == "Modify") {
                if (item.name === this.selectedMake.name) {
                    this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
                    return false;
                }
            }
        }
        return true;
    }

    onCancel(action: string) {
        this.acFileUploader.reset(true);
        this.addMakeModal.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyMakeModal.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteMakeConfirmModal.close();
    }

    add() {
        this.make.changeType = "Add";
        if (!this.validationCheck(this.make)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.make.attachments = uploadedFiles;
            }
            if (this.make.attachments) {
                this.make.attachments = this.make.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.makeService.add(this.make).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Make", ConstantsWarehouse.changeRequestType.add, this.make.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.make.name + "\" Make change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Make", ConstantsWarehouse.changeRequestType.add, this.make.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Make", ConstantsWarehouse.changeRequestType.add, this.make.name);
                this.toastr.warning(errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.showLoadingGif = false;
                this.acFileUploader.reset(true);
                this.addMakeModal.close();
            });

        }, error => {
            this.showLoadingGif = false;
            this.acFileUploader.reset(true);
            this.addMakeModal.close();
        });
    }

    modify(makeVm: IMake) {
        this.showLoadingGif = true;
        this.selectedMake = makeVm;
        this.make.id = this.selectedMake.id;
        this.make.name = this.selectedMake.name;
        this.make.comment = "";
        this.modifyMakeModal.open("md");
        if (this.selectedMake.baseVehicleCount == 0 && this.selectedMake.vehicleCount == 0) {
            this.makeService.getById(this.selectedMake.id).subscribe(
                m => {
                    this.selectedMake.baseVehicleCount = this.make.baseVehicleCount = m.baseVehicleCount;
                    this.selectedMake.vehicleCount = this.make.vehicleCount = m.vehicleCount;
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
        this.make.changeType = "Modify";
        if (!this.validationCheck(this.make)) {
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.make.attachments = uploadedFiles;
            }
            if (this.make.attachments) {
                this.make.attachments = this.make.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.makeService.update(this.selectedMake.id, this.make).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Make", ConstantsWarehouse.changeRequestType.modify, this.selectedMake.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.make.name + "\" Make change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.makeList.filter(x => x.id == this.selectedMake.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Make", ConstantsWarehouse.changeRequestType.modify, this.selectedMake.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (errorresponse => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Make", ConstantsWarehouse.changeRequestType.modify, this.selectedMake.name);
                this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }), () => {
                this.showLoadingGif = false;
                this.modifyPopupAcFileUploader.reset(true);
                this.modifyMakeModal.close();

            });
        }, error => {
            this.showLoadingGif = false;
            this.modifyPopupAcFileUploader.reset(false);
            this.modifyMakeModal.close();

        });
    }

    delete(makeVm: IMake) {
        this.showLoadingGif = true;
        this.selectedMake = makeVm;

        this.make.id = this.selectedMake.id;
        this.make.name = this.selectedMake.name;
        this.make.comment = "";

        if (this.selectedMake.baseVehicleCount == 0 && this.selectedMake.vehicleCount == 0) {
            this.makeService.getById(this.selectedMake.id).subscribe(
                m => {
                    this.selectedMake.baseVehicleCount = m.baseVehicleCount;
                    this.selectedMake.vehicleCount = m.vehicleCount;

                    if (this.selectedMake.baseVehicleCount == 0 && this.selectedMake.vehicleCount == 0) {
                        this.showLoadingGif = false;
                        this.deleteMakeConfirmModal.open("md");
                    }
                    else {
                        this.showLoadingGif = false;
                        this.deleteMakeErrorModal.open("sm");
                    }
                },
                error => {
                    this.showLoadingGif = false;
                    this.toastr.warning(<any>error.toString(), "Load Failed");
                }
            );
        }
        else {
            if (this.selectedMake.baseVehicleCount == 0 && this.selectedMake.vehicleCount == 0) {
                this.showLoadingGif = false;
                this.deleteMakeConfirmModal.open("md");
            }
            else {
                this.showLoadingGif = false;
                this.deleteMakeErrorModal.open("sm");
            }
        }
    }

    deleteSubmit() {
        this.selectedMake.changeType = "Delete";
        if (!this.validationCheck(this.selectedMake)) {
            return;
        }
        this.deleteMakeConfirmModal.close();

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.selectedMake.attachments = uploadedFiles;
            }
            if (this.make.attachments) {
                this.selectedMake.attachments = this.selectedMake.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());

            }
            this.makeService.delete(this.selectedMake.id, this.selectedMake).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Make", ConstantsWarehouse.changeRequestType.remove, this.selectedMake.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.make.name + "\" Make change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.makeList.filter(x => x.id == this.selectedMake.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Make", ConstantsWarehouse.changeRequestType.remove, this.selectedMake.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                this.ngOnInit();
            }, (errorresponses => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Make", ConstantsWarehouse.changeRequestType.remove, this.selectedMake.name);
                this.toastr.warning(errorresponses, errorMessage.title);
                this.showLoadingGif = false;
            }), () => {
                this.showLoadingGif = false;
                this.deletePopupAcFileUploader.reset(true);
                this.deleteMakeConfirmModal.close();
            });

        }, error => {
            this.showLoadingGif = false;
            this.deletePopupAcFileUploader.reset(true);
            this.deleteMakeConfirmModal.close();
        });
    }

    view(makeVm: IMake) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/make/" + makeVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    openAddMakeModal() {
        this.make.id = 0;
        this.make.name = "";
        this.make.comment = "";

        this.addMakeModal.open("md");
    }

    setSelectedMake(make: IMake) {
        let selectedMake = make;
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}