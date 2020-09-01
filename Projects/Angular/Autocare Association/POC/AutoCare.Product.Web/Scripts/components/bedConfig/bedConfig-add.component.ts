import { Component, OnInit, ViewChild }           from "@angular/core";
import { Router} from "@angular/router";
import { ToastsManager }                    from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ModalComponent } from "ng2-bs3-modal/ng2-bs3-modal";
import { IBedConfig }         from "./bedConfig.model";
import { BedTypeService }     from "../bedType/bedType.service";
import { IBedType }           from "../bedType/bedType.model";
import { BedLengthService }      from "../bedLength/bedLength.service";
import { IBedLength }            from "../bedLength/bedLength.model";
import { BedConfigService }   from "./bedConfig.service";
import { ConstantsWarehouse }   from "../constants-warehouse";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import BaseVehiclemodel = require("../baseVehicle/baseVehicle.model");
import {SharedService} from '../shared/shared.service';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "bedConfig-add-component",
    templateUrl: "app/templates/bedConfig/bedConfig-add.component.html",
    providers: [SharedService]
})

export class BedConfigAddComponent implements OnInit {
    public newBedConfig: IBedConfig;
    public bedLengths: IBedLength[];
    public bedTypes: IBedType[];
    attachments: any[] = [];
    public proposedBedConfigs: IBedConfig[];
    public pendingBedConfigChangeRequests: IBedConfig[];
    public bedConfig: IBedConfig = { bedLengthId: 0, bedTypeId: 0, comment: '' };

    // popup
    @ViewChild("viewChangeRequestModal")
    public viewChangeRequestModal: ModalComponent;

    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;

    @ViewChild('commentPopupModel')
    commentPopupModel: ModalComponent;

    @ViewChild('attachmentsPopupModel')
    attachmentsPopupModel: ModalComponent;

    @ViewChild("attachmentsPopupAcFileUploader")
    attachmentsPopupAcFileUploader: AcFileUploader;

    showLoadingGif: boolean = false;

    constructor(private bedLengthService: BedLengthService, private bedConfigService: BedConfigService, private bedTypeService: BedTypeService,
         private toastr: ToastsManager, private router: Router, private sharedService: SharedService) {
        // initialize empty bed config
        this.newBedConfig = {
            id: 0,
            bedLengthId: -1,
            length: "",
            bedLengthMetric: "",
            bedTypeId: -1,
            bedTypeName: "",
            isSelected: false
        };
    }

    ngOnInit() {
        this.showLoadingGif = true;
        // Load select options for add.
        this.bedTypeService.getAllBedTypes().subscribe(bt => {
            this.bedTypes = bt;
            this.bedLengthService.getAllBedLengths().subscribe(bl => {
                this.bedLengths = bl;
                    // Load pending bed config change requests
                    this.bedConfigService.getPendingChangeRequests().subscribe(crs => {
                        crs.forEach(cr => {
                            cr.length = bl.filter(item => item.id === Number(cr.bedLengthId))[0].length;
                            cr.bedLengthMetric = bl.filter(item => item.id === Number(cr.bedLengthId))[0].bedLengthMetric;
                            cr.bedTypeName = bt.filter(item => item.id === Number(cr.bedTypeId))[0].name;
                        });
                        this.pendingBedConfigChangeRequests = crs;
                        this.showLoadingGif = false;
                    },
                        error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle)); // pending bed config change requests
               
            },
                error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle)); // bed length
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });// bed type
        // assign empty array to proposed change requests
        this.proposedBedConfigs = Array<IBedConfig>();
    }

    // validation
    private validateAddBedConfig(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (Number(this.newBedConfig.bedLengthId) === -1) {
            this.toastr.warning("Please select Bed Length.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.newBedConfig.bedTypeId) === -1) {
            this.toastr.warning("Please select Bed Type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        // check already exists in proposed changes
        else {
            let filteredBedConfigs = this.proposedBedConfigs.filter(item =>
                Number(item.bedLengthId) === Number(this.newBedConfig.bedLengthId) &&
                Number(item.bedTypeId) === Number(this.newBedConfig.bedTypeId));
            if (filteredBedConfigs && filteredBedConfigs.length) {
                this.toastr.warning("Selected Bed Cofig System already added.", ConstantsWarehouse.validationTitle);
                isValid = false;
            }
        }
        return isValid;
    }

    // event on add to proposed changes
    onAddToProposedChanges() {
        // validate change information
        if (this.validateAddBedConfig()) {
            this.showLoadingGif = true;
            // fill bed config information and push to proposed bed configs
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {

                this.newBedConfig.length = this.bedLengths.filter(item => item.id === Number(this.newBedConfig.bedLengthId))[0].length;
                this.newBedConfig.bedLengthMetric = this.bedLengths.filter(item => item.id === Number(this.newBedConfig.bedLengthId))[0].bedLengthMetric;
                this.newBedConfig.bedTypeName = this.bedTypes.filter(item => item.id === Number(this.newBedConfig.bedTypeId))[0].name;
                this.newBedConfig.attachments = uploadedFiles;
                this.proposedBedConfigs.push(this.newBedConfig);
                // clear bed config information
                this.newBedConfig = {
                    id: 0,
                    bedLengthId: -1,
                    length: "",
                    bedLengthMetric: "",
                    bedTypeId: -1,
                    bedTypeName: "",
                    isSelected: false
                };
                this.acFileUploader.reset();
                this.showLoadingGif = false;

            }, error => {
                this.acFileUploader.reset();
                this.showLoadingGif = false;
            });
        }
    }

    // event on submit bed config
    onSubmitChangeRequests() {
        // loop through proposed bed configs
        for (let bedConfig of this.proposedBedConfigs) {
            // make current bed config identity
            let bedConfigIdentity: string = this.bedLengths.filter(item => item.id === Number(bedConfig.bedLengthId))[0].name + ", "
                + this.bedTypes.filter(item => item.id === Number(bedConfig.bedTypeId))[0].name;
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    bedConfig.attachments = uploadedFiles;
                }
                this.bedConfigService.addBedConfig(bedConfig).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Bed System", ConstantsWarehouse.changeRequestType.add, bedConfigIdentity);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.add} Bed System ${bedConfigIdentity} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed System", ConstantsWarehouse.changeRequestType.add, bedConfigIdentity);
                        //errorMessage.title = "Your requested change cannot be submitted.";
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                },
                    error => {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed System", ConstantsWarehouse.changeRequestType.add, bedConfigIdentity);
                        //errorMessage.title = "Your requested change cannot be submitted.";
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                        this.showLoadingGif = false;
                    }, () => {
                        this.acFileUploader.reset();
                        this.showLoadingGif = false;
                    });
            }, error => {
                this.acFileUploader.reset();
                this.showLoadingGif = false;

            });
        }
    }

    // event on view affected vehicles
    onViewBedSystemCr(bedSystemVm: IBedConfig) {
        //this.viewChangeRequestModal.open(); // medium/default size popup
        var changeRequestLink = "/change/review/bedconfig/" + bedSystemVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }
    openCommentPopupModal(proposedBedConfig: IBedConfig) {
        this.bedConfig = { bedLengthId: 0, bedTypeId: 0, comment: '' };
        this.bedConfig.id = proposedBedConfig.id;
        this.bedConfig.bedLengthId = proposedBedConfig.bedLengthId;
        this.bedConfig.bedTypeId = proposedBedConfig.bedTypeId;
        this.bedConfig.comment = proposedBedConfig.comment;
        this.bedConfig.attachments = proposedBedConfig.attachments;
        this.commentPopupModel.open("md");
    }

    onCommentConfirm() {
        this.proposedBedConfigs.filter(item => item.bedLengthId == this.bedConfig.bedLengthId
            && item.bedTypeId == this.bedConfig.bedTypeId)[0].comment = this.bedConfig.comment;
        this.commentPopupModel.close();
    }

    openAttachmentPopupModal(proposedBedConfig: IBedConfig) {
        this.bedConfig = { bedLengthId: 0, bedTypeId: 0, comment: '', attachments: [] }
        this.bedConfig.id = proposedBedConfig.id;
        this.bedConfig.bedLengthId = proposedBedConfig.bedLengthId;
        this.bedConfig.bedTypeId = proposedBedConfig.bedTypeId;
        this.bedConfig.attachments = proposedBedConfig.attachments;
        this.attachments = this.sharedService.clone(proposedBedConfig.attachments);

        this.attachmentsPopupModel.open("md");
        if (this.attachmentsPopupAcFileUploader) {
            this.attachmentsPopupAcFileUploader.reset(false);
            this.attachmentsPopupAcFileUploader.existingFiles = proposedBedConfig.attachments;
            this.attachmentsPopupAcFileUploader.setAcFiles();
        }
    }

    onAttachmentConfirm() {
        this.showLoadingGif = true;
        let objectIdentity = this.proposedBedConfigs.filter(item => item.bedLengthId == this.bedConfig.bedLengthId
            && item.bedTypeId == this.bedConfig.bedTypeId)[0];

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
        let objectIdentity = this.proposedBedConfigs.filter(item => item.bedLengthId == this.bedConfig.bedLengthId
            && item.bedTypeId == this.bedConfig.bedTypeId)[0];
        objectIdentity.attachments = this.sharedService.clone(this.attachments);
        this.attachmentsPopupAcFileUploader.setAcFiles();
        this.attachmentsPopupModel.dismiss();
    }

    onRemoveBedConfig(bedConfig:IBedConfig) {
        if (confirm("Remove from selection?")) {
            var index = this.proposedBedConfigs.indexOf(bedConfig);
            if (index > -1) {
                this.proposedBedConfigs.splice(index, 1);
            }
        }
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.attachmentsPopupAcFileUploader.cleanupAllTempContainers();
    }
}