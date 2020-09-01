import {Component, OnInit, ViewChild} from "@angular/core";
import {BaseVehicleService} from "../basevehicle/basevehicle.service";
import {HttpHelper} from "../httpHelper";
import {ModalComponent}    from "ng2-bs3-modal/ng2-bs3-modal";
import {Router, ActivatedRoute} from "@angular/router";
import {RegionService} from "../region/region.service";
import {SubModelService} from "../submodel/submodel.service";
import {ISubModel} from "../submodel/submodel.model";
import {IRegion} from "../region/region.model";
import {ToastsManager} from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {VehicleService} from "./vehicle.service";
import {ConstantsWarehouse} from "../constants-warehouse";
import {IBaseVehicle} from "../baseVehicle/baseVehicle.model";
import {IVehicle} from "../vehicle/vehicle.model";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import {SharedService} from '../shared/shared.service';
import { Observable }    from 'rxjs/Observable';


@Component({
    selector: 'vehicle-add-component',
    templateUrl: 'app/templates/vehicle/vehicle-add.component.html',
    providers: [BaseVehicleService, HttpHelper, RegionService, SubModelService, VehicleService, SharedService]
})

export class VehicleAddComponent implements OnInit {
    baseVehicleId: number;
    baseVehicle: IBaseVehicle = {};
    isBaseVehicleLoading: boolean = true;
    comment: string = "";
    subModels: ISubModel[] = [];
    regions: IRegion[] = [];
    proposedVehicles: IVehicle[] = [];
    proposedVehicle: IVehicle = { id: 0, subModelId: -1, regionId: -1 };
    changeRequestVehicles: IVehicle[] = [];
    existingVehicles: IVehicle[] = [];
    isExistingVehiclesLoading: boolean = true;
    attachments: any[] = [];
    vehicle: IVehicle = { id: 0, subModelId: -1, regionId: -1, attachments: [] };

    selectedSubModelId: number = -1;
    selectedRegionId: number = -1;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;

    @ViewChild("attachmentsPopupAcFileUploader")
    attachmentsPopupAcFileUploader: AcFileUploader;

    @ViewChild('commentPopupModel')
    commentPopupModel: ModalComponent;

    @ViewChild('attachmentsPopupModel')
    attachmentsPopupModel: ModalComponent;

    showLoadingGif: boolean = false;
    constructor(private baseVehicleService: BaseVehicleService, private regionService: RegionService,
        private subModelService: SubModelService, private toastr: ToastsManager,
        private vehicleService: VehicleService,
        private route: ActivatedRoute, private _router: Router, private sharedService: SharedService) {
    }

    ngOnInit() {
        this.baseVehicleId = this.route.snapshot.params["basevid"];
        this.searchBaseVehicle();
    }

    addToProposedVehicles() {
        if (this.selectedSubModelId == -1 || this.selectedRegionId == -1) {
            this.toastr.warning("Please select submodel and region from dropdowns to add a vehicle", ConstantsWarehouse.validationTitle);
            return;
        }
        if (!this.exists()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                //let proposedVehicle = {
                //    id: 0,
                //    baseVehicleId: this.baseVehicleId,
                //    subModelId: this.selectedSubModelId,
                //    subModelName: this.subModels.filter(s => s.id == this.selectedSubModelId)[0].name,
                //    regionId: this.selectedRegionId,
                //    regionName: this.regions.filter(r => r.id == this.selectedRegionId)[0].name,
                //    comment: this.proposedVehicle.comment,
                //    attachments: uploadedFiles
                //};
                this.proposedVehicle.id = 0;
                this.proposedVehicle.baseVehicleId = this.baseVehicleId;
                this.proposedVehicle.subModelId = this.selectedSubModelId;
                this.proposedVehicle.subModelName = this.subModels.filter(s => s.id == this.selectedSubModelId)[0].name;
                this.proposedVehicle.regionId = this.selectedRegionId;
                this.proposedVehicle.regionName = this.regions.filter(r => r.id == this.selectedRegionId)[0].name;
                this.proposedVehicle.comment = this.comment;
                this.proposedVehicle.attachments = uploadedFiles;
                this.proposedVehicles.push(this.proposedVehicle);
                this.proposedVehicle = { id: 0, baseVehicleId: 0, subModelId: -1, regionId: -1, comment: '' };
                this.resetUIControls();
                this.acFileUploader.reset();
                this.showLoadingGif = false;
            }, error => {
                this.acFileUploader.reset();
                this.showLoadingGif = false;
            });
        }
        else {
            this.toastr.warning("Selected vehicle already exists, please add a different vehicle", ConstantsWarehouse.validationTitle);
        }

        // clean variables

    }

    exists() {
        let matchingVehicles = this.proposedVehicles.filter(item => item.subModelId == this.selectedSubModelId
            && item.regionId == this.selectedRegionId);
        return (matchingVehicles && matchingVehicles.length > 0);
    }

    createVehicleChangeRequests() {
        this.proposedVehicles.forEach(proposedVehicle => {
            let vehicleIdentity: string = this.subModels.filter(item => item.id == proposedVehicle.subModelId)[0].name + ','
                + this.regions.filter(item => item.id == proposedVehicle.regionId)[0].name;
            this.showLoadingGif = true;
            this.vehicleService.createVehicleChangeRequests(proposedVehicle).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle", ConstantsWarehouse.changeRequestType.add, vehicleIdentity);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " Vehicle \"" + vehicleIdentity + "\"  change request Id  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.proposedVehicles = [];
                    this._router.navigateByUrl('vehicle/search');
                } else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle", ConstantsWarehouse.changeRequestType.add, vehicleIdentity);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                this.showLoadingGif = false;

            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle", ConstantsWarehouse.changeRequestType.add, vehicleIdentity);
                this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            });
        });
    }

    getRegionsSubModels() {
        this.regionService.getRegion().subscribe(regions => {
            this.regions = [];
            this.regions = regions;

            this.subModelService.getAllSubModels().subscribe(submodels => {
                this.subModels = [];
                this.subModels = submodels;
                this.getPendingChangeRequests();
            });
        });
    }

    searchBaseVehicle() {
        this.showLoadingGif = true;
        this.getRegionsSubModels();
        this.baseVehicleService.getBaseVehicle(this.baseVehicleId).subscribe(baseVehicle => {
            this.baseVehicle = {};
            this.baseVehicle = baseVehicle;
            this.existingVehicles = [];
            this.vehicleService.getVehiclesByBaseVehicleId(this.baseVehicleId).subscribe(existingVehicles => {
                this.existingVehicles = existingVehicles;
                this.isBaseVehicleLoading = false;
                this.isExistingVehiclesLoading = false;
                this.showLoadingGif = false;
            });
        });
    }

    getPendingChangeRequests() {

        this.vehicleService.getPendingChangeRequest(this.baseVehicleId).subscribe(changeRequestVehicles => {
            this.changeRequestVehicles = [];

            for (let vehicle of changeRequestVehicles) {
                vehicle.subModelName = this.subModels.filter(s => s.id == vehicle.subModelId)[0].name;
                vehicle.regionName = this.regions.filter(r => r.id == vehicle.regionId)[0].name;
            }
            this.changeRequestVehicles = changeRequestVehicles;
        });
    }

    resetUIControls() {
        this.selectedRegionId = -1;
        this.selectedSubModelId = -1;
        this.comment = "";
    }
    openCommentPopupModal(proposedVehicle: IVehicle) {

        this.vehicle.subModelId = proposedVehicle.subModelId;
        this.vehicle.regionId = proposedVehicle.regionId;
        this.vehicle.comment = proposedVehicle.comment;
        this.commentPopupModel.open("md");

    }

    onCommentConfirm() {

        this.proposedVehicles.filter(item => item.subModelId == this.vehicle.subModelId
            && item.regionId == this.vehicle.regionId)[0].comment = this.vehicle.comment;
        this.commentPopupModel.close();
    }

    openAttachmentPopupModal(proposedVehicle: IVehicle) {
        this.vehicle = {
            id: 0,
            subModelId: -1,
            subModelName: '',
            regionId: -1,
            regionName: '',
            attachments: []
        };
        this.vehicle.subModelId = proposedVehicle.subModelId;
        this.vehicle.regionId = proposedVehicle.regionId;
        this.vehicle.attachments = proposedVehicle.attachments;
        this.attachments = this.sharedService.clone(proposedVehicle.attachments);

        this.attachmentsPopupModel.open("md");
        if (this.attachmentsPopupAcFileUploader) {
            this.attachmentsPopupAcFileUploader.reset(false);
            this.attachmentsPopupAcFileUploader.existingFiles = proposedVehicle.attachments;
            this.attachmentsPopupAcFileUploader.setAcFiles();
        }
    }

    onAttachmentConfirm() {
        this.showLoadingGif = true;
        let objectIdentity = this.proposedVehicles.filter(item => item.subModelId == this.vehicle.subModelId
            && item.regionId == this.vehicle.regionId)[0];

        this.attachmentsPopupAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles) {
                objectIdentity.attachments = uploadedFiles;
            }
            if (objectIdentity.attachments && this.attachmentsPopupAcFileUploader.getFilesMarkedToDelete().length > 0) {
                objectIdentity.attachments = objectIdentity.attachments.concat(this.attachmentsPopupAcFileUploader.getFilesMarkedToDelete());
            }
            this.showLoadingGif = false;
            this.attachmentsPopupModel.close();
        });

    }
    onAttachmentCancel() {
        let objectIdentity = this.proposedVehicles.filter(item => item.subModelId == this.vehicle.subModelId
            && item.regionId == this.vehicle.regionId)[0]

        objectIdentity.attachments = this.sharedService.clone(this.attachments);

        this.attachmentsPopupAcFileUploader.setAcFiles();
        this.attachmentsPopupModel.dismiss();
    }


    onViewExistingVehicleForSelectedBaseVehicleCR(existingVehicleVm: IVehicle) {
        var changeRequestLink = "/change/review/vehicle/" + existingVehicleVm.changeRequestId;
        this._router.navigateByUrl(changeRequestLink);

    }

    onViewPendingVehicleCR(pendingVehicleVM: IVehicle) {
        var changeRequestLink = "/change/review/vehicle/" + pendingVehicleVM.changeRequestId;
        this._router.navigateByUrl(changeRequestLink);

    }

    onRemoveVehicle(vehicle: IVehicle) {
        if (confirm("Remove from selection?")) {
            var index = this.proposedVehicles.indexOf(vehicle);
            if (index > -1) {
                this.proposedVehicles.splice(index, 1);
            }
        }
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.attachmentsPopupAcFileUploader.cleanupAllTempContainers();
    }

}