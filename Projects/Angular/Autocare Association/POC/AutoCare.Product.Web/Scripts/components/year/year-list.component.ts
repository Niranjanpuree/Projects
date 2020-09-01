import { Component, OnInit, ViewChild }         from '@angular/core';
import { Router}            from "@angular/router";
import { IYear }                                from './year.model';
import { YearService }                          from './year.service';
import { ConstantsWarehouse }                   from '../constants-warehouse';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';
import { ModalComponent }       from "ng2-bs3-modal/ng2-bs3-modal";
import { ToastsManager }                          from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { AcFileUploader }                       from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';

@Component({
    selector: 'year-list',
    templateUrl: 'app/templates/year/year-list.component.html',
})

export class YearListComponent implements OnInit {
    isYearValidNumber: boolean = true;
    isYearLengthValid: boolean = true;
    year: IYear = { baseVehicleCount: 0 };
    selectedYear: IYear = {}
    years: IYear[];
    showLoadingGif: boolean = false;

    @ViewChild('deleteErrorPopup')
    deleteErrorPopup: ModalComponent;

    @ViewChild('deleteConfirmPopup')
    deleteConfirmPopup: ModalComponent;

    @ViewChild("deletePopupAcFileUploader")
    deletePopupAcFileUploader: AcFileUploader;

    constructor(private yearService: YearService, private toastr: ToastsManager,
        private router: Router, private navigationService: NavigationService) { }

    ngOnInit() {
        this.getYears();
    }

    getYears() {
        this.showLoadingGif = true;
        this.yearService.getYears().subscribe(
            yearList => {
                this.years = yearList;
                this.showLoadingGif = false;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            }
        );
    }

    onCancel(action: string) {
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
    }

    validationCheck(item: IYear): boolean {
        if (item.changetype === "Add") {
            if (!item.id) {
                this.toastr.warning("Year is Invalid.", ConstantsWarehouse.validationTitle);
                return false;
            }

            if (isNaN(item.id)) {
                this.toastr.warning("Year is Not a Number.", ConstantsWarehouse.validationTitle);
                return false;
            }
            if (item.id.toString().length !== 4) {
                this.toastr.warning("Year must be of four digit", ConstantsWarehouse.validationTitle);
                return false;
            }
        }
        return true;
    }

    add(year) {
        this.year.changetype = "Add";
        if (!this.validationCheck(this.year)) {
            return;
        }
        this.showLoadingGif = true;
        this.yearService.addYear(year)
            .subscribe(response => {
                if (response) {
                    this.year.id = null;
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Year", ConstantsWarehouse.changeRequestType.add, this.year.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.year.id + "\" Year change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Year", ConstantsWarehouse.changeRequestType.add, this.year.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                this.showLoadingGif = false;
            }, error => {
                this.showLoadingGif = false;
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Year", ConstantsWarehouse.changeRequestType.add, this.year.id);
                this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
            });
    }

    delete(year) {
        this.selectedYear = year;
        this.selectedYear.comment = "";
        this.showLoadingGif = true;
        if (!year.baseVehicleCount) {
            this.yearService.getDependencies(year.id).subscribe(m => {
                year.baseVehicleCount = m.baseVehicleCount;
                if (year.baseVehicleCount > 0) {
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
            if (year.baseVehicleCount > 0) {
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
        this.year.changetype = "Delete";
        if (!this.validationCheck(this.year)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.year.attachments = uploadedFiles;
            }
            if (this.year.attachments) {
                this.year.attachments = this.year.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.yearService.deleteYear(this.selectedYear.id, this.year).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Year", ConstantsWarehouse.changeRequestType.remove, this.selectedYear.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.year.id + "\" Region change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.years.filter(x => x.id == this.selectedYear.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Year", ConstantsWarehouse.changeRequestType.remove, this.selectedYear.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Year", ConstantsWarehouse.changeRequestType.remove, this.selectedYear.id);
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

    view(YearVm: IYear) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/year/" + YearVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}