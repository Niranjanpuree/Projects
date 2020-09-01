import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {ModalComponent} from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {IBedLength} from './bedLength.model';
import {BedLengthService} from './bedLength.service';
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'bedLength-list-component',
    templateUrl: 'app/templates/bedLength/bedLength-list.component.html',
})

export class BedLengthListComponent {
    bedLengths: IBedLength[];
    filteredBedLengths: IBedLength[] = [];
    bedLength: IBedLength = {};
    modifiedBedLength: IBedLength = {};
    bedLengthNameFilter: string = '';
    //dika: string = '';
    //maxlength = 10;
    //countDot = 0;
    //i = 0;
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

    constructor(private bedLengthService: BedLengthService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.bedLengthService.getAllBedLengths().subscribe(sm => {
            this.bedLengths = sm;
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
            keyword = this.bedLengthNameFilter;
        }
        else {
            this.bedLengthNameFilter = keyword;
        }

        if (String(this.bedLengthNameFilter) === "") {
            this.bedLengthService.getAllBedLengths().subscribe(sm => {
                this.bedLengths = sm;
                this.showLoadingGif = false;
                this.filteredBedLengths = [];
            },
                error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.bedLengthService.getBedLength(this.bedLengthNameFilter).subscribe(m => {
                this.bedLengths = m;
                this.showLoadingGif = false;
                this.filteredBedLengths = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.bedLengthService.getBedLength(keyword);
    }

    onSelect(bedlength: IBedLength) {
        this.bedLengthNameFilter = bedlength.length;
        this.applyFilter();
        this.filteredBedLengths = [];
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
    //test(input) {
    //    this.maxlength = 10;
    //    if(input){
    //    this.dika = input;
    //            for (this.i = 0; this.i < this.dika.length; this.i++) {
    //        if (this.dika[this.i] == '.') {
    //            this.maxlength = (this.i)+ 2;
    //            }
    //        }
    //    }
    //    //this.bedLength.length = this.dika;
    //}
    onNew() {
        this.bedLength = {};
        this.newPopup.open("md");
    }

    validationCheck(item: IBedLength): boolean {
        let isValid = true;
        if (!item.length) {
            this.toastr.warning("Bed Length is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        if (!item.bedLengthMetric) {
            this.toastr.warning("Bed Length Metric is required.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }

        return isValid;
    }

    onNewSubmit() {

        if (!this.validationCheck(this.bedLength)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.bedLength.attachments = uploadedFiles;
            }
            if (this.bedLength.attachments) {
                this.bedLength.attachments = this.bedLength.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.bedLengthService.addBedLength(this.bedLength).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Bed Length", ConstantsWarehouse.changeRequestType.add, this.bedLength.length);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the Bed Length  of length \"" + this.bedLength.length + "\" and bed length Metric \"" + this.bedLength.bedLengthMetric + "\" Bed Length change request Id  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Length", ConstantsWarehouse.changeRequestType.add, this.bedLength.length);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Length", ConstantsWarehouse.changeRequestType.add, this.bedLength.length);
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

    onModify(bedlength: IBedLength) {
        this.bedLength = bedlength;
        this.showLoadingGif = true;
        if (!bedlength.vehicleToBedConfigCount) {
            this.bedLengthService.getBedLengthDetail(bedlength.id).subscribe(m => {
                this.bedLength.vehicleToBedConfigCount = m.vehicleToBedConfigCount;
                this.bedLength.bedConfigCount = m.bedConfigCount;
                this.modifiedBedLength = <IBedLength>JSON.parse(JSON.stringify(bedlength));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedBedLength = <IBedLength>JSON.parse(JSON.stringify(bedlength));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        if (!this.validationCheck(this.modifiedBedLength)) {
            return;
        }
        else if (this.modifiedBedLength.length == this.bedLength.length && this.modifiedBedLength.bedLengthMetric == this.bedLength.bedLengthMetric) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }

        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedBedLength.attachments = uploadedFiles;
            }
            if (this.modifiedBedLength.attachments) {
                this.modifiedBedLength.attachments = this.modifiedBedLength.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.bedLengthService.updateBedLength(this.modifiedBedLength.id, this.modifiedBedLength).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Bed Length", ConstantsWarehouse.changeRequestType.modify, this.bedLength.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the bed length of length \"" + this.bedLength.length + "\" and bed length metric \"" + this.bedLength.bedLengthMetric + "\" Bed Length change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.bedLengths.filter(x => x.id == this.modifiedBedLength.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Length", ConstantsWarehouse.changeRequestType.modify, this.bedLength.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Length", ConstantsWarehouse.changeRequestType.modify, this.bedLength.length);
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

    onDelete(bedlength: IBedLength) {
        this.bedLength = bedlength;
        this.showLoadingGif = true;
        if (!bedlength.vehicleToBedConfigCount) {
            this.bedLengthService.getBedLengthDetail(bedlength.id).subscribe(m => {
                bedlength.vehicleToBedConfigCount = m.vehicleToBedConfigCount;
                this.bedLength.vehicleToBedConfigCount = bedlength.vehicleToBedConfigCount;
                if (bedlength.vehicleToBedConfigCount > 0) {
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
            if (bedlength.vehicleToBedConfigCount > 0 || bedlength.bedConfigCount > 0) {
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
        if (!this.validationCheck(this.bedLength)) {
            return;
        }

        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.bedLength.attachments = uploadedFiles;
            }
            if (this.bedLength.attachments) {
                this.bedLength.attachments = this.bedLength.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.bedLengthService.deleteBedLength(this.bedLength.id, this.bedLength).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Bed Length", ConstantsWarehouse.changeRequestType.remove, this.bedLength.length);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the bed length of length \"" + this.bedLength.length + "\" and bed length metric \"" + this.bedLength.bedLengthMetric + "\" Bed Length change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.bedLengths.filter(x => x.id == this.bedLength.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Length", ConstantsWarehouse.changeRequestType.remove, this.bedLength.length);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Length", ConstantsWarehouse.changeRequestType.remove, this.bedLength.length);
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

    view(bedLengthVm: IBedLength) {

        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/bedlength/" + bedLengthVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}