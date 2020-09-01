import { Component, OnInit, ViewChild }             from "@angular/core";
import { Router, ActivatedRoute }from "@angular/router";
import { ToastsManager }                            from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { IBodyStyleConfig }                         from "./bodyStyleConfig.model";
import { IVehicleToBodyStyleConfig }                from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.model";
import { BodyStyleConfigService }                   from "./bodyStyleConfig.service";
import { AcFileUploader }                           from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }                               from 'rxjs/Observable';

@Component({
    selector: "bodyStyleConfig-replace-component",
    templateUrl: "app/templates/bodyStyleConfig/bodyStyleConfig-replaceConfirm.component.html",
})

export class BodyStyleConfigReplaceConfirmComponent implements OnInit {
    public existingBodyStyleConfig: IBodyStyleConfig;
    public replacementBodyStyleConfig: IBodyStyleConfig;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;

    constructor(private bodyStyleConfigService: BodyStyleConfigService, private router: Router,
        private route: ActivatedRoute, private toastr: ToastsManager) {
    }

    ngOnInit() {
        // Load existing bed config with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        // Get existing / replace bed config records from factory/ service.
        this.existingBodyStyleConfig = this.bodyStyleConfigService.existingBodyStyleConfig;
        this.replacementBodyStyleConfig = this.bodyStyleConfigService.replacementBodyStyleConfig;
    }

    // validation
    private validateReplaceConfirmBodyStyleConfig(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (!this.existingBodyStyleConfig || !this.existingBodyStyleConfig.vehicleToBodyStyleConfigs || !this.replacementBodyStyleConfig) {
            this.toastr.warning("Not implemented.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementBodyStyleConfig.bodyNumDoorsId) === -1) {
            this.toastr.warning("Please select Body Num Doors.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementBodyStyleConfig.bodyTypeId) === -1) {
            this.toastr.warning("Please select Body type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementBodyStyleConfig.id) < 1) {
            this.toastr.warning("Please select replacement Body style config system.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.existingBodyStyleConfig.id) === Number(this.replacementBodyStyleConfig.id)) {
            this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.filter(item => item.isSelected).length <= 0) {
            this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }
    onSubmitChangeRequest() {
        if (this.validateReplaceConfirmBodyStyleConfig()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    this.existingBodyStyleConfig.attachments = uploadedFiles;
                }
                if (this.existingBodyStyleConfig.attachments) {
                    this.existingBodyStyleConfig.attachments = this.existingBodyStyleConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.existingBodyStyleConfig.vehicleToBodyStyleConfigs = this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.filter(item => item.isSelected);
                this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.forEach(v => { v.bodyStyleConfigId = this.replacementBodyStyleConfig.id; });

                let bodyStyleConfigIdentity: string = "Body style config id: " + this.existingBodyStyleConfig.id;

                this.bodyStyleConfigService.replaceBodyStyleConfig(this.existingBodyStyleConfig.id, this.existingBodyStyleConfig).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Body Style Config", ConstantsWarehouse.changeRequestType.modify, bodyStyleConfigIdentity);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.modify} Body Style Config ${bodyStyleConfigIdentity} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Body Style Config", ConstantsWarehouse.changeRequestType.modify, bodyStyleConfigIdentity);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                }, (errorresponse => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Body Style Config", ConstantsWarehouse.changeRequestType.modify, bodyStyleConfigIdentity);
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
