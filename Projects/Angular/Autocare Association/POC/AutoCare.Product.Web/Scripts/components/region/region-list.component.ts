import {Component, OnInit, ViewChild  } from '@angular/core';
import {Router} from '@angular/router';
import {ModalComponent} from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { AcGridComponent }                   from "../../lib/aclibs/ac-grid/ac-grid";
import {IRegion, IParentRegion} from './region.model';
import {RegionService} from './region.service';
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { NavigationService }     from "../shared/navigation.service";
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'region-list-component',
    templateUrl: 'app/templates/region/region-list.component.html',
})

export class RegionListComponent implements OnInit {
    regions: IRegion[];
    region: IRegion = {};
    modifiedRegion: IRegion = {};
    parentRegion: IRegion[];
    filteredRegions: IRegion[] = [];
    regionNameFilter: string = '';
    filteredRegionWithAbbrForValidation: IRegion[];
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

    constructor(private regionService: RegionService, private router: Router,
        private toastr: ToastsManager, private navigationService: NavigationService) { }

    ngOnInit() {
        this.showLoadingGif = true;
        this.regionService.getRegion().subscribe(s => {
            this.regions = s;
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    public applyFilter = (keyword?: string) => {
        this.showLoadingGif = true;
        keyword = keyword || '';
        if (keyword == '') {
            keyword = this.regionNameFilter;
        }
        else {
            this.regionNameFilter = keyword;
        }

        if (String(this.regionNameFilter) === "") {
            this.regionService.getRegion().subscribe(s => {
                this.regions = s;
                this.showLoadingGif = false;
                this.filteredRegions = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        } else {
            this.regionService.getRegionByNameFilter(this.regionNameFilter).subscribe(m => {
                this.regions = m;
                this.showLoadingGif = false;
                this.filteredRegions = [];
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    public getSuggestions = (keyword: string) => {
        return this.regionService.getRegionByNameFilter(keyword);
    }

    onSelect(region: IRegion) {
        this.regionNameFilter = region.name;
        this.applyFilter();
        this.filteredRegions = [];
    }

    onCancel(action: string) {
        this.acFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
    }

    validationCheck(item: IRegion): boolean {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("Region Name is required.", ConstantsWarehouse.validationTitle);
                return false;
            }
            if (!item.regionAbbr) {
                this.toastr.warning("Region Abbreviation is required.", ConstantsWarehouse.validationTitle);
                return false;
            } else {
                if (item.regionAbbr.length > 3) {
                    this.toastr.warning("Region Abbreviation cannot be more than 3 characters.", ConstantsWarehouse.validationTitle);
                    return false;
                }
                if (item.changeType == "Modify") {
                    if (this.region.name == item.name) {
                        this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
                        return false;
                    }
                }
            }
        }
        return true;
    }

    onNew() {
        this.region = {}
        this.newPopup.open("md");
    }

    onNewSubmit() {
        this.region.changeType = "Add";
        if (!this.validationCheck(this.region)) {
            return;
        }

        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.region.attachments = uploadedFiles;
            }
            if (this.region.attachments) {
                this.region.attachments = this.region.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.regionService.addRegion(this.region).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Region", ConstantsWarehouse.changeRequestType.add, this.region.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + this.region.name + "\" Region change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Region", ConstantsWarehouse.changeRequestType.add, this.region.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Region", ConstantsWarehouse.changeRequestType.add, this.region.name);
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

    onModify(region: IRegion) {
        this.region = region;
        this.showLoadingGif = true;

        if (!region.vehicleCount) {
            this.regionService.getRegionDetail(region.id).subscribe(m => {
                region.vehicleCount = m.vehicleCount;
                this.modifiedRegion = <IRegion>JSON.parse(JSON.stringify(region));
                this.showLoadingGif = false;
                this.modifyPopup.open("md");
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
        else {
            this.modifiedRegion = <IRegion>JSON.parse(JSON.stringify(region));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    }

    onModifySubmit() {
        this.modifiedRegion.changeType = "Modify";
        if (!this.validationCheck(this.modifiedRegion)) {
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedRegion.attachments = uploadedFiles;
            }
            if (this.modifiedRegion.attachments) {
                this.modifiedRegion.attachments = this.modifiedRegion.attachments.concat(this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.regionService.updateRegion(this.modifiedRegion.id, this.modifiedRegion).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Region", ConstantsWarehouse.changeRequestType.modify, this.region.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.region.name + "\" Region change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.regions.filter(x => x.id == this.modifiedRegion.id)[0].changeRequestId = response;
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Region", ConstantsWarehouse.changeRequestType.modify, this.region.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                    this.showLoadingGif = false;
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Region", ConstantsWarehouse.changeRequestType.modify, this.region.name);
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

    onDelete(region: IRegion) {
        this.region = region;
        this.region.comment = "";
        this.showLoadingGif = true;
        if (!region.vehicleCount) {
            this.regionService.getRegionDetail(region.id).subscribe(m => {
                region.vehicleCount = m.vehicleCount;

                if (region.vehicleCount > 0) {
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
            if (region.vehicleCount > 0) {
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
        this.region.changeType = "Delete";
        if (!this.validationCheck(this.region)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.region.attachments = uploadedFiles;
            }
            if (this.region.attachments) {
                this.region.attachments = this.region.attachments.concat(this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.regionService.deleteRegionPost(this.region.id, this.region).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Region", ConstantsWarehouse.changeRequestType.remove, this.region.name);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.region.name + "\" Region change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.regions.filter(x => x.id == this.region.id)[0].changeRequestId = response;
                }
                else {

                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Region", ConstantsWarehouse.changeRequestType.remove, this.region.name);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Region", ConstantsWarehouse.changeRequestType.remove, this.region.name);
                this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.showLoadingGif = false;
                this.deletePopupAcFileUploader.reset();
                this.deleteConfirmPopup.close();
            });

        }, error => {
            this.showLoadingGif = false;
            this.deletePopupAcFileUploader.reset(true);
            this.deleteConfirmPopup.close();
        });
    }

    view(regionVm: IRegion) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/region/" + regionVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    }
}