import { Component, OnInit, ViewChild }           from "@angular/core";
import { Router } from "@angular/router";
import { ToastsManager }                    from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ModalComponent } from "ng2-bs3-modal/ng2-bs3-modal";
import { IBrakeConfig }         from "./brakeConfig.model";
import { BrakeTypeService }     from "../brakeType/brakeType.service";
import { IBrakeType }           from "../brakeType/brakeType.model";
import { BrakeABSService }      from "../brakeABS/brakeABS.service";
import { IBrakeABS }            from "../brakeABS/brakeABS.model";
import { BrakeSystemService }   from "../brakeSystem/brakeSystem.service";
import { IBrakeSystem }         from "../brakeSystem/brakeSystem.model";
import { BrakeConfigService }   from "./brakeConfig.service";
import { ConstantsWarehouse }   from "../constants-warehouse";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import BaseVehiclemodel = require("../baseVehicle/baseVehicle.model");
import {SharedService} from '../shared/shared.service';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "brakeConfig-add-component",
    templateUrl: "app/templates/brakeConfig/brakeConfig-add.component.html",
    providers: [SharedService]  //pushkar: remove if existing instance will work
})

export class BrakeConfigAddComponent implements OnInit {
    public newBrakeConfig: IBrakeConfig;
    public brakeTypes: IBrakeType[];
    public brakeABSes: IBrakeABS[];
    public brakeSystems: IBrakeSystem[];
    attachments: any[] = [];
    public proposedBrakeConfigs: IBrakeConfig[];
    public pendingBrakeConfigChangeRequests: IBrakeConfig[];
    public brakeConfig: IBrakeConfig = { frontBrakeTypeId: 0, rearBrakeTypeId: 0, brakeABSId: 0, brakeSystemId: 0, comment: '' };

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

    constructor(private brakeAbsService: BrakeABSService, private brakeConfigService: BrakeConfigService, private brakeSystemService: BrakeSystemService,
        private brakeTypeSerivce: BrakeTypeService, private toastr: ToastsManager, private router: Router, private sharedService: SharedService) {
        // initialize empty brake config
        this.newBrakeConfig = {
            id: 0,
            frontBrakeTypeId: -1,
            frontBrakeTypeName: "",
            rearBrakeTypeId: -1,
            rearBrakeTypeName: "",
            brakeABSId: -1,
            brakeABSName: "",
            brakeSystemId: -1,
            brakeSystemName: "",
            isSelected: false
        };
    }

    ngOnInit() {
        this.showLoadingGif = true;
        // Load select options for add.
        this.brakeTypeSerivce.getAllBrakeTypes().subscribe(bt => {
            this.brakeTypes = bt;
            this.brakeAbsService.getAllBrakeABSes().subscribe(babs => {
                this.brakeABSes = babs;
                this.brakeSystemService.getAllBrakeSystems().subscribe(bs => {
                    this.brakeSystems = bs;
                    // Load pending brake config change requests
                    this.brakeConfigService.getPendingChangeRequests().subscribe(crs => {
                        crs.forEach(cr => {
                            cr.frontBrakeTypeName = bt.filter(item => item.id === Number(cr.frontBrakeTypeId))[0].name;
                            cr.rearBrakeTypeName = bt.filter(item => item.id === Number(cr.rearBrakeTypeId))[0].name;
                            cr.brakeABSName = babs.filter(item => item.id === Number(cr.brakeABSId))[0].name;
                            cr.brakeSystemName = bs.filter(item => item.id === Number(cr.brakeSystemId))[0].name;
                        });
                        this.pendingBrakeConfigChangeRequests = crs;
                        this.showLoadingGif = false;
                    },
                        error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle)); // pending brake config change requests
                },
                    error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle)); // brake systems
            },
                error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle)); // brake Abss
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });// brake types
        // assign empty array to proposed change requests
        this.proposedBrakeConfigs = Array<IBrakeConfig>();
    }

    // validation
    private validateAddBrakeConfig(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (Number(this.newBrakeConfig.frontBrakeTypeId) === -1) {
            this.toastr.warning("Please select Front brake type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.newBrakeConfig.rearBrakeTypeId) === -1) {
            this.toastr.warning("Please select Rear brake type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.newBrakeConfig.brakeABSId) === -1) {
            this.toastr.warning("Please select Brake ABS.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.newBrakeConfig.brakeSystemId) === -1) {
            this.toastr.warning("Please select Brake system.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        // check already exists in proposed changes
        else {
            let filteredBrakeConfigs = this.proposedBrakeConfigs.filter(item =>
                Number(item.frontBrakeTypeId) === Number(this.newBrakeConfig.frontBrakeTypeId) &&
                Number(item.rearBrakeTypeId) === Number(this.newBrakeConfig.rearBrakeTypeId) &&
                Number(item.brakeABSId) === Number(this.newBrakeConfig.brakeABSId) &&
                Number(item.brakeSystemId) === Number(this.newBrakeConfig.brakeSystemId))
            if (filteredBrakeConfigs && filteredBrakeConfigs.length) {
                this.toastr.warning("Selected Brake Cofig System already added.", ConstantsWarehouse.validationTitle);
                isValid = false;
            }
        }
        return isValid;
    }

    // event on add to proposed changes
    onAddToProposedChanges() {
        // validate change information
        if (this.validateAddBrakeConfig()) {
            this.showLoadingGif = true;
            // fill brake config information and push to proposed brake configs
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {

                this.newBrakeConfig.frontBrakeTypeName = this.brakeTypes.filter(item => item.id === Number(this.newBrakeConfig.frontBrakeTypeId))[0].name;
                this.newBrakeConfig.rearBrakeTypeName = this.brakeTypes.filter(item => item.id === Number(this.newBrakeConfig.rearBrakeTypeId))[0].name;
                this.newBrakeConfig.brakeABSName = this.brakeABSes.filter(item => item.id === Number(this.newBrakeConfig.brakeABSId))[0].name;
                this.newBrakeConfig.brakeSystemName = this.brakeSystems.filter(item => item.id === Number(this.newBrakeConfig.brakeSystemId))[0].name;
                this.newBrakeConfig.attachments = uploadedFiles;
                this.proposedBrakeConfigs.push(this.newBrakeConfig);
                // clear brake config information
                this.newBrakeConfig = {
                    id: 0,
                    frontBrakeTypeId: -1,
                    frontBrakeTypeName: "",
                    rearBrakeTypeId: -1,
                    rearBrakeTypeName: "",
                    brakeABSId: -1,
                    brakeABSName: "",
                    brakeSystemId: -1,
                    brakeSystemName: "",
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

    // event on submit brake config
    onSubmitChangeRequests() {
        // loop through proposed brake configs
        for (let brakeConfig of this.proposedBrakeConfigs) {
            // make current brake config identity
            let brakeConfigIdentity: string = this.brakeTypes.filter(item => item.id === Number(brakeConfig.frontBrakeTypeId))[0].name + ", "
                + this.brakeTypes.filter(item => item.id === Number(brakeConfig.rearBrakeTypeId))[0].name + ", "
                + this.brakeABSes.filter(item => item.id === Number(brakeConfig.brakeABSId))[0].name + ", "
                + this.brakeSystems.filter(item => item.id === Number(brakeConfig.brakeSystemId))[0].name;
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    brakeConfig.attachments = uploadedFiles;
                }
                this.brakeConfigService.addBrakeConfig(brakeConfig).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Brake System", ConstantsWarehouse.changeRequestType.add, brakeConfigIdentity);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.add} Brake System ${brakeConfigIdentity} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake System", ConstantsWarehouse.changeRequestType.add, brakeConfigIdentity);
                        //errorMessage.title = "Your requested change cannot be submitted.";
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                },
                    error => {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake System", ConstantsWarehouse.changeRequestType.add, brakeConfigIdentity);
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
    onViewBrakeSystemCr(brakeSystemVm: IBrakeConfig) {
        //this.viewChangeRequestModal.open(); // medium/default size popup
        var changeRequestLink = "/change/review/brakeconfig/" + brakeSystemVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }
    openCommentPopupModal(proposedBrakeConfig: IBrakeConfig) {
        this.brakeConfig = { frontBrakeTypeId: 0, rearBrakeTypeId: 0, brakeABSId: 0, brakeSystemId: 0, comment: '' };
        this.brakeConfig.id = proposedBrakeConfig.id;
        this.brakeConfig.frontBrakeTypeId = proposedBrakeConfig.frontBrakeTypeId;
        this.brakeConfig.rearBrakeTypeId = proposedBrakeConfig.rearBrakeTypeId;
        this.brakeConfig.brakeABSId = proposedBrakeConfig.brakeABSId;
        this.brakeConfig.brakeSystemId = proposedBrakeConfig.brakeSystemId;
        this.brakeConfig.comment = proposedBrakeConfig.comment;
        this.brakeConfig.attachments = proposedBrakeConfig.attachments;
        this.commentPopupModel.open("md");
    }

    onCommentConfirm() {
        this.proposedBrakeConfigs.filter(item => item.frontBrakeTypeId == this.brakeConfig.frontBrakeTypeId
            && item.rearBrakeTypeId == this.brakeConfig.rearBrakeTypeId
            && item.brakeABSId == this.brakeConfig.brakeABSId
            && item.brakeSystemId == this.brakeConfig.brakeSystemId)[0].comment = this.brakeConfig.comment;
        this.commentPopupModel.close();
    }

    openAttachmentPopupModal(proposedBrakeConfig: IBrakeConfig) {
        this.brakeConfig = { frontBrakeTypeId: 0, rearBrakeTypeId: 0, brakeABSId: 0, brakeSystemId: 0, comment: '', attachments: [] }
        this.brakeConfig.id = proposedBrakeConfig.id;
        this.brakeConfig.frontBrakeTypeId = proposedBrakeConfig.frontBrakeTypeId;
        this.brakeConfig.rearBrakeTypeId = proposedBrakeConfig.rearBrakeTypeId;
        this.brakeConfig.brakeABSId = proposedBrakeConfig.brakeABSId;
        this.brakeConfig.brakeSystemId = proposedBrakeConfig.brakeSystemId;
        this.brakeConfig.attachments = proposedBrakeConfig.attachments;
        this.attachments = this.sharedService.clone(proposedBrakeConfig.attachments);

        this.attachmentsPopupModel.open("md");
        if (this.attachmentsPopupAcFileUploader) {
            this.attachmentsPopupAcFileUploader.reset(false);
            this.attachmentsPopupAcFileUploader.existingFiles = proposedBrakeConfig.attachments;
            this.attachmentsPopupAcFileUploader.setAcFiles();
        }
    }

    onAttachmentConfirm() {
        this.showLoadingGif = true;
        let objectIdentity = this.proposedBrakeConfigs.filter(item => item.frontBrakeTypeId == this.brakeConfig.frontBrakeTypeId
            && item.rearBrakeTypeId == this.brakeConfig.rearBrakeTypeId
            && item.brakeABSId == this.brakeConfig.brakeABSId
            && item.brakeSystemId == this.brakeConfig.brakeSystemId)[0];

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
        let objectIdentity = this.proposedBrakeConfigs.filter(item => item.frontBrakeTypeId == this.brakeConfig.frontBrakeTypeId
            && item.rearBrakeTypeId == this.brakeConfig.rearBrakeTypeId
            && item.brakeABSId == this.brakeConfig.brakeABSId
            && item.brakeSystemId == this.brakeConfig.brakeSystemId)[0];
        objectIdentity.attachments = this.sharedService.clone(this.attachments);
        this.attachmentsPopupAcFileUploader.setAcFiles();
        this.attachmentsPopupModel.dismiss();
    }

    onRemoveBrakeConfig(brakeConfig:IBrakeConfig) {
        if (confirm("Remove from selection?")) {
            var index = this.proposedBrakeConfigs.indexOf(brakeConfig);
            if (index > -1) {
                this.proposedBrakeConfigs.splice(index, 1);
            }
        }
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.attachmentsPopupAcFileUploader.cleanupAllTempContainers();
    }
}