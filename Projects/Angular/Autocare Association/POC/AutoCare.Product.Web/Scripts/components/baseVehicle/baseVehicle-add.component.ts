import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import { Observable }    from 'rxjs/Observable';
import {ModalComponent}    from "ng2-bs3-modal/ng2-bs3-modal";
import {IBaseVehicle} from './baseVehicle.model';
import {BaseVehicleService} from './baseVehicle.service';
import {IMake} from '../make/make.model';
import {MakeService} from '../make/make.service';
import {IModel} from '../model/model.model';
import {ModelService} from '../model/model.service';
import {IYear} from '../year/year.model';
import {YearService} from '../year/year.service';
import {HttpHelper} from '../httpHelper';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import {SharedService} from '../shared/shared.service';

@Component({
    selector: 'baseVehicle-component',
    templateUrl: 'app/templates/baseVehicle/baseVehicle-add.component.html',
    providers: [BaseVehicleService, ModelService, MakeService, YearService, HttpHelper, SharedService]
})

export class BaseVehicleAddComponent implements OnInit {
    baseVehicle: IBaseVehicle = { id: 0, makeId: -1, makeName: '', modelId: -1, modelName: '', yearId: -1, vehicles: null };
    proposedBaseVehicles: IBaseVehicle[] = [];
    pendingBaseVehicleChangeRequests: IBaseVehicle[];
    makes: IMake[];
    models: IModel[];
    attachments: any[] = [];

    years: IYear[];
    comment: string = '';
    proposedBaseVehicle: IBaseVehicle = { id: 0, makeId: -1, makeName: '', modelId: -1, modelName: '', yearId: -1, vehicles: null, attachments: null };

    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;

    @ViewChild("attachmentsPopupAcFileUploader")
    attachmentsPopupAcFileUploader: AcFileUploader;

    @ViewChild('commentPopupModel')
    commentPopupModel: ModalComponent;

    @ViewChild('attachmentsPopupModel')
    attachmentsPopupModel: ModalComponent;

    showLoadingGif: boolean = false;

    constructor(private makeService: MakeService, private modelService: ModelService,
        private yearService: YearService, private baseVehicleService: BaseVehicleService, private router: Router, private toastr: ToastsManager, private sharedService: SharedService) {
    }

    ngOnInit() {
        this.showLoadingGif = true;
        this.makeService.getAllMakes().subscribe(mks => {
            this.makes = mks;
            this.modelService.getAllModels().subscribe(mdls => {
                this.models = mdls;
                this.baseVehicleService.getPendingChangeRequests().subscribe(crs => {
                    crs.forEach(cr => {
                        cr.makeName = mks.filter(item => item.id == cr.makeId)[0].name;
                        cr.modelName = mdls.filter(item => item.id == cr.modelId)[0].name;
                    });
                    this.pendingBaseVehicleChangeRequests = crs;
                    this.showLoadingGif = false;
                },
                    error => {
                        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    });
            },
                error => {
                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                });
        },

            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });

        this.yearService.getYears().subscribe(m => this.years = m,
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            });
    }

    onAddToProposedChanges() {
        if (this.baseVehicle.makeId == -1) {
            this.toastr.warning('Please select Make.', ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.baseVehicle.modelId == -1) {
            this.toastr.warning('Please select Model.', ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.baseVehicle.yearId == -1) {
            this.toastr.warning('Please select Year.', ConstantsWarehouse.validationTitle);
            return;
        }
        let filteredBaseVehicles = this.proposedBaseVehicles.filter(item => item.makeId == this.baseVehicle.makeId
            && item.modelId == this.baseVehicle.modelId
            && item.yearId == this.baseVehicle.yearId);

        if (filteredBaseVehicles && filteredBaseVehicles.length > 0) {
            this.toastr.warning('Selected Base Vehicle already added', ConstantsWarehouse.validationTitle);
        }
        else {
            this.showLoadingGif = true;
            // validate change information       
            // fill brake config information and push to proposed brake configs
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                this.baseVehicle.makeName = this.makes.filter(item => item.id == this.baseVehicle.makeId)[0].name;
                this.baseVehicle.modelName = this.models.filter(item => item.id == this.baseVehicle.modelId)[0].name;
                this.baseVehicle.attachments = uploadedFiles;
                this.proposedBaseVehicles.push(this.baseVehicle);
                this.baseVehicle = { id: 0, makeId: -1, makeName: '', modelId: -1, modelName: '', yearId: -1, vehicles: null };
                this.acFileUploader.reset(false);
                this.showLoadingGif = false;
            }, error => {
                this.acFileUploader.reset();
                this.showLoadingGif = false;
            });


        }
    }

    onSubmitChangeRequests() {
        this.proposedBaseVehicles.forEach(proposedBaseVehicle => {
            let baseVehicleIdentity: string = this.makes.filter(item => item.id == proposedBaseVehicle.makeId)[0].name + ', '
                + this.models.filter(item => item.id == proposedBaseVehicle.modelId)[0].name;
            this.showLoadingGif = true;
            this.baseVehicleService.addBaseVehicle(proposedBaseVehicle).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Base Vehicle", ConstantsWarehouse.changeRequestType.add, baseVehicleIdentity);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + baseVehicleIdentity + "\" Base Vehicle requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.router.navigateByUrl('vehicle/search');
                } else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Base Vehicle", ConstantsWarehouse.changeRequestType.add, baseVehicleIdentity);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                this.showLoadingGif = false;
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Base Vehicle", ConstantsWarehouse.changeRequestType.add, baseVehicleIdentity);
                this.toastr.warning(errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.acFileUploader.reset();
                this.showLoadingGif = false;
            });
        });
    }

    openCommentPopupModal(proposedBaseVehicle: IBaseVehicle) {
        this.proposedBaseVehicle.makeId = proposedBaseVehicle.makeId;
        this.proposedBaseVehicle.modelId = proposedBaseVehicle.modelId;
        this.proposedBaseVehicle.yearId = proposedBaseVehicle.yearId;
        this.proposedBaseVehicle.comment = proposedBaseVehicle.comment;
        this.commentPopupModel.open("md");
    }

    onCommentConfirm() {
        this.proposedBaseVehicles.filter(item => item.makeId == this.proposedBaseVehicle.makeId
            && item.modelId == this.proposedBaseVehicle.modelId
            && item.yearId == this.proposedBaseVehicle.yearId)[0].comment = this.proposedBaseVehicle.comment;
        this.commentPopupModel.close();
    }

    openAttachmentPopupModal(proposedBaseVehicle: IBaseVehicle) {
        this.attachmentsPopupModel.open("lg");
        this.proposedBaseVehicle = { id: 0, makeId: -1, makeName: '', modelId: -1, modelName: '', yearId: -1, vehicles: null, attachments: [] };
        this.proposedBaseVehicle.makeId = proposedBaseVehicle.makeId;
        this.proposedBaseVehicle.modelId = proposedBaseVehicle.modelId;
        this.proposedBaseVehicle.yearId = proposedBaseVehicle.yearId;
        this.proposedBaseVehicle.attachments = proposedBaseVehicle.attachments;
        this.attachments = this.sharedService.clone(proposedBaseVehicle.attachments);


        if (this.attachmentsPopupAcFileUploader) {
            this.attachmentsPopupAcFileUploader.reset(false);
            this.attachmentsPopupAcFileUploader.existingFiles = proposedBaseVehicle.attachments;
            this.attachmentsPopupAcFileUploader.setAcFiles();
        }
    }




    onAttachmentConfirm() {
        this.showLoadingGif = true;
        let objectIdentity = this.proposedBaseVehicles.filter(item => item.makeId == this.proposedBaseVehicle.makeId
            && item.modelId == this.proposedBaseVehicle.modelId
            && item.yearId == this.proposedBaseVehicle.yearId)[0];

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
        let objectIdentity = this.proposedBaseVehicles.filter(item => item.makeId == this.proposedBaseVehicle.makeId
            && item.modelId == this.proposedBaseVehicle.modelId
            && item.yearId == this.proposedBaseVehicle.yearId)[0];
        objectIdentity.attachments = this.sharedService.clone(this.attachments);
        this.attachmentsPopupAcFileUploader.setAcFiles();
        this.attachmentsPopupModel.dismiss();
    }

    onViewPendingNew(baseVehicleVm: IBaseVehicle) {
        var changeRequestLink = "/change/review/basevehicle/" + baseVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    onRemoveBaseVehicle(baseVehicle: IBaseVehicle) {
        if (confirm("Remove from selection?")) {
            var index = this.proposedBaseVehicles.indexOf(baseVehicle);
            if (index > -1) {
                this.proposedBaseVehicles.splice(index, 1);
            }
        }
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers() && this.attachmentsPopupAcFileUploader.cleanupAllTempContainers();
    }
}
