import { Component, OnInit, ViewChild }           from "@angular/core";
import { Router } from "@angular/router";
import { ToastsManager }                    from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ModalComponent } from "ng2-bs3-modal/ng2-bs3-modal";
import { IBodyStyleConfig }         from "./bodyStyleConfig.model";
import { BodyTypeService }     from "../bodyType/bodyType.service";
import { IBodyType }           from "../bodyType/bodyType.model";
import { BodyNumDoorsService }      from "../bodyNumDoors/bodyNumDoors.service";
import { IBodyNumDoors }            from "../bodyNumDoors/bodyNumDoors.model";
import { BodyStyleConfigService }   from "./bodyStyleConfig.service";
import { ConstantsWarehouse }   from "../constants-warehouse";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import BaseVehiclemodel = require("../baseVehicle/baseVehicle.model");
import {SharedService} from '../shared/shared.service';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "bodyStyleConfig-add-component",
    templateUrl: "app/templates/bodyStyleConfig/bodyStyleConfig-add.component.html",
    providers: [SharedService]  //pushkar: remove if new instance not required
})

export class BodyStyleConfigAddComponent implements OnInit {
    public newBodyStyleConfig: IBodyStyleConfig;
    public bodyNumDoors: IBodyNumDoors[];
    public bodyTypes: IBodyType[];
    attachments: any[] = [];
    public proposedBodyStyleConfigs: IBodyStyleConfig[];
    public pendingBodyStyleConfigChangeRequests: IBodyStyleConfig[];
    public bodyStyleConfig: IBodyStyleConfig = { bodyNumDoorsId: 0, bodyTypeId: 0, comment: '' };

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

    constructor(private bodyNumDoorService: BodyNumDoorsService, private bodyStyleConfigService: BodyStyleConfigService, private bodyTypeService: BodyTypeService,
        private toastr: ToastsManager, private router: Router, private sharedService: SharedService) {
        // initialize empty bed config
        this.newBodyStyleConfig = {
            id: 0,
            bodyNumDoorsId: -1,
            numDoors: "",
            bodyTypeId: -1,
            bodyTypeName: "",
            isSelected: false
        };
    }

    ngOnInit() {
        this.showLoadingGif = true;
        // Load select options for add.
        this.bodyTypeService.getAllBodyTypes().subscribe(bt => {
            this.bodyTypes = bt;
            this.bodyNumDoorService.getAllBodyNumDoors().subscribe(bl => {
                this.bodyNumDoors = bl;
                // Load pending bed config change requests
                    this.bodyStyleConfigService.getPendingChangeRequests().subscribe(crs => {
                    crs.forEach(cr => {
                        cr.numDoors = bl.filter(item => item.id === Number(cr.bodyNumDoorsId))[0].numDoors;
                        cr.bodyTypeName = bt.filter(item => item.id === Number(cr.bodyTypeId))[0].name;
                    });
                    this.pendingBodyStyleConfigChangeRequests = crs;
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
        this.proposedBodyStyleConfigs = Array<IBodyStyleConfig>();
    }

    // validation
    private validateAddBodyStyleConfig(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (Number(this.newBodyStyleConfig.bodyNumDoorsId) === -1) {
            this.toastr.warning("Please select Body Number Doors.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.newBodyStyleConfig.bodyTypeId) === -1) {
            this.toastr.warning("Please select Body Type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        // check already exists in proposed changes
        else {
            let filteredBodyStyleConfigs = this.proposedBodyStyleConfigs.filter(item =>
                Number(item.bodyNumDoorsId) === Number(this.newBodyStyleConfig.bodyNumDoorsId) &&
                Number(item.bodyTypeId) === Number(this.newBodyStyleConfig.bodyTypeId));
            if (filteredBodyStyleConfigs && filteredBodyStyleConfigs.length) {
                this.toastr.warning("Selected Body Style Cofig System already added.", ConstantsWarehouse.validationTitle);
                isValid = false;
            }
        }
        return isValid;
    }

    // event on add to proposed changes
    onAddToProposedChanges() {
        // validate change information
        if (this.validateAddBodyStyleConfig()) {
            this.showLoadingGif = true;
            // fill bed config information and push to proposed bed configs
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {

                this.newBodyStyleConfig.numDoors = this.bodyNumDoors.filter(item => item.id === Number(this.newBodyStyleConfig.bodyNumDoorsId))[0].numDoors;
                this.newBodyStyleConfig.bodyTypeName = this.bodyTypes.filter(item => item.id === Number(this.newBodyStyleConfig.bodyTypeId))[0].name;
                this.newBodyStyleConfig.attachments = uploadedFiles;
                this.proposedBodyStyleConfigs.push(this.newBodyStyleConfig);
                // clear bed config information
                this.newBodyStyleConfig = {
                    id: 0,
                    bodyNumDoorsId: -1,
                    numDoors: "",
                    bodyTypeId: -1,
                    bodyTypeName: "",
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
        for (let bodyStyleConfig of this.proposedBodyStyleConfigs) {
            // make current bed config identity
            let bodyStyleConfigIdentity: string = this.bodyNumDoors.filter(item => item.id === Number(bodyStyleConfig.bodyNumDoorsId))[0].numDoors + ", "
                + this.bodyTypes.filter(item => item.id === Number(bodyStyleConfig.bodyTypeId))[0].name;
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    bodyStyleConfig.attachments = uploadedFiles;
                }
                this.bodyStyleConfigService.addBodyStyleConfig(bodyStyleConfig).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Body System", ConstantsWarehouse.changeRequestType.add, bodyStyleConfigIdentity);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.add} Body System ${bodyStyleConfigIdentity} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Body System", ConstantsWarehouse.changeRequestType.add, bodyStyleConfigIdentity);
                        //errorMessage.title = "Your requested change cannot be submitted.";
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                },
                    error => {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Body System", ConstantsWarehouse.changeRequestType.add, bodyStyleConfigIdentity);
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
    onViewBodySystemCr(bodySystemVm: IBodyStyleConfig) {
        //this.viewChangeRequestModal.open(); // medium/default size popup
        var changeRequestLink = "/change/review/bodystyleconfig/" + bodySystemVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }
    openCommentPopupModal(proposedBodyStyleConfig: IBodyStyleConfig) {
        this.bodyStyleConfig = { bodyNumDoorsId: 0, bodyTypeId: 0, comment: '' };
        this.bodyStyleConfig.id = proposedBodyStyleConfig.id;
        this.bodyStyleConfig.bodyNumDoorsId = proposedBodyStyleConfig.bodyNumDoorsId;
        this.bodyStyleConfig.bodyTypeId = proposedBodyStyleConfig.bodyTypeId;
        this.bodyStyleConfig.comment = proposedBodyStyleConfig.comment;
        this.bodyStyleConfig.attachments = proposedBodyStyleConfig.attachments;
        this.commentPopupModel.open("md");
    }

    onCommentConfirm() {
        this.proposedBodyStyleConfigs.filter(item => item.bodyNumDoorsId == this.bodyStyleConfig.bodyNumDoorsId
            && item.bodyTypeId == this.bodyStyleConfig.bodyTypeId)[0].comment = this.bodyStyleConfig.comment;
        this.commentPopupModel.close();
    }

    openAttachmentPopupModal(proposedBodyStyleConfig: IBodyStyleConfig) {
        this.bodyStyleConfig = { bodyNumDoorsId: 0, bodyTypeId: 0, comment: '', attachments: [] }
        this.bodyStyleConfig.id = proposedBodyStyleConfig.id;
        this.bodyStyleConfig.bodyNumDoorsId = proposedBodyStyleConfig.bodyNumDoorsId;
        this.bodyStyleConfig.bodyTypeId = proposedBodyStyleConfig.bodyTypeId;
        this.bodyStyleConfig.attachments = proposedBodyStyleConfig.attachments;
        this.attachments = this.sharedService.clone(proposedBodyStyleConfig.attachments);

        this.attachmentsPopupModel.open("md");
        if (this.attachmentsPopupAcFileUploader) {
            this.attachmentsPopupAcFileUploader.reset(false);
            this.attachmentsPopupAcFileUploader.existingFiles = proposedBodyStyleConfig.attachments;
            this.attachmentsPopupAcFileUploader.setAcFiles();
        }
    }

    onAttachmentConfirm() {
        this.showLoadingGif = true;
        let objectIdentity = this.proposedBodyStyleConfigs.filter(item => item.bodyNumDoorsId == this.bodyStyleConfig.bodyNumDoorsId
            && item.bodyTypeId == this.bodyStyleConfig.bodyTypeId)[0];

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
        let objectIdentity = this.proposedBodyStyleConfigs.filter(item => item.bodyNumDoorsId == this.bodyStyleConfig.bodyNumDoorsId
            && item.bodyTypeId == this.bodyStyleConfig.bodyTypeId)[0];
        objectIdentity.attachments = this.sharedService.clone(this.attachments);
        this.attachmentsPopupAcFileUploader.setAcFiles();
        this.attachmentsPopupModel.dismiss();
    }

    onRemoveBodyStyleConfig(bodyStyleConfig: IBodyStyleConfig) {
        if (confirm("Remove from selection?")) {
            var index = this.proposedBodyStyleConfigs.indexOf(bodyStyleConfig);
            if (index > -1) {
                this.proposedBodyStyleConfigs.splice(index, 1);
            }
        }
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.attachmentsPopupAcFileUploader.cleanupAllTempContainers();
    }
}