import { Component, OnInit, ViewChild }                        from "@angular/core";
import { Router, ActivatedRoute }   from "@angular/router";
import { ToastsManager }                            from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { IBodyStyleConfig }                             from "./bodyStyleConfig.model";
import { BodyStyleConfigService }                       from "./bodyStyleConfig.service";
import { IBodyType }                               from "../bodyType/bodyType.model";
import { BodyTypeService }                         from "../bodyType/bodyType.service";
import { BodyNumDoorsService }      from "../bodyNumDoors/bodyNumDoors.service";
import { IBodyNumDoors }            from "../bodyNumDoors/bodyNumDoors.model";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "bodyStyleConfig-modify-component",
    templateUrl: "app/templates/bodyStyleConfig/bodyStyleConfig-modify.component.html",
    providers: [BodyStyleConfigService],
})

export class BodyStyleConfigModifyComponent implements OnInit {
    existingBodyStyleConfig: IBodyStyleConfig;
    modifiedBodyStyleConfig: IBodyStyleConfig;
    bodyTypes: IBodyType[];
    bodyNumDoors: IBodyNumDoors[];
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;
    constructor(private bodyStyleConfigService: BodyStyleConfigService,
        private bodyNumDoorService: BodyNumDoorsService,
        private bodyTypeService: BodyTypeService,
        private toastr: ToastsManager,
        private route: ActivatedRoute,
        private router: Router
    ) {
    }

    ngOnInit() {
        this.showLoadingGif = true;
        let id = Number(this.route.snapshot.params["id"]);
        this.bodyTypeService.getAllBodyTypes().subscribe(x => this.bodyTypes = x,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        this.bodyNumDoorService.getAllBodyNumDoors().subscribe(x => this.bodyNumDoors = x,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        this.bodyStyleConfigService.getBodyStyleConfig(id).subscribe(x => {
            this.existingBodyStyleConfig = x;
            this.modifiedBodyStyleConfig = <IBodyStyleConfig>JSON.parse(JSON.stringify(this.existingBodyStyleConfig));
            this.showLoadingGif = false;
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
    }

    // validation
    private validateModifyBodyStyleConfig(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (Number(this.modifiedBodyStyleConfig.bodyNumDoorsId) === -1) {
            this.toastr.warning("Please select Body Number Doors.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.modifiedBodyStyleConfig.bodyTypeId) === -1) {
            this.toastr.warning("Please select Body Type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onSubmitChangeRequests() {
        if (this.validateModifyBodyStyleConfig()) {
            let bodyStyleConfigIdentity: string = this.modifiedBodyStyleConfig.numDoors + ','
                + this.modifiedBodyStyleConfig.bodyTypeName;
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    this.modifiedBodyStyleConfig.attachments = uploadedFiles;
                }
                if (this.modifiedBodyStyleConfig.attachments) {
                    this.modifiedBodyStyleConfig.attachments = this.modifiedBodyStyleConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.bodyStyleConfigService.updateBodyStyleConfig(this.modifiedBodyStyleConfig.id, this.modifiedBodyStyleConfig).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Body System", ConstantsWarehouse.changeRequestType.modify, bodyStyleConfigIdentity);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.modify} Body System ${bodyStyleConfigIdentity} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Body System", ConstantsWarehouse.changeRequestType.modify, bodyStyleConfigIdentity);
                        errorMessage.title = "Your requested change cannot be submitted.";
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                }, (errorresponse => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Body System", ConstantsWarehouse.changeRequestType.modify, bodyStyleConfigIdentity);
                    this.toastr.warning(errorresponse ? errorresponse : errorMessage.body, errorMessage.title);
                    this.showLoadingGif = false;
                }), () => {
                    this.acFileUploader.reset();
                    this.showLoadingGif = false;
                });
            }, error => {
                this.acFileUploader.reset();
                this.showLoadingGif = false;
            });
        }
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers();
    }
}
