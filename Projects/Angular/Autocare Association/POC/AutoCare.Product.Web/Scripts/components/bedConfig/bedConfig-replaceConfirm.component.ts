import { Component, OnInit, ViewChild }             from "@angular/core";
import { Router, ActivatedRoute }from "@angular/router";
import { ToastsManager }                            from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ModalComponent }         from "ng2-bs3-modal/ng2-bs3-modal";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { IBedConfig }                               from "./bedConfig.model";
import { IVehicleToBedConfig }                      from "../vehicleToBedConfig/vehicleToBedConfig.model";
import { BedConfigService }                         from "./bedConfig.service";
import { AcFileUploader }                           from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }                               from 'rxjs/Observable';

@Component({
    selector: "bedConfig-replace-component",
    templateUrl: "app/templates/bedConfig/bedConfig-replaceConfirm.component.html",
})

export class BedConfigReplaceConfirmComponent implements OnInit {
    public existingBedConfig: IBedConfig;
    public replacementBedConfig: IBedConfig;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;

    constructor(private bedConfigService: BedConfigService, private router: Router,
        private route: ActivatedRoute, private toastr: ToastsManager) {
    }

    ngOnInit() {
        // Load existing bed config with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        // Get existing / replace bed config records from factory/ service.
        this.existingBedConfig = this.bedConfigService.existingBedConfig;
        this.replacementBedConfig = this.bedConfigService.replacementBedConfig;
    }

    // validation
    private validateReplaceConfirmBedConfig(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (!this.existingBedConfig || !this.existingBedConfig.vehicleToBedConfigs || !this.replacementBedConfig) {
            this.toastr.warning("Not implemented.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementBedConfig.bedLengthId) === -1) {
            this.toastr.warning("Please select Bed length.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementBedConfig.bedTypeId) === -1) {
            this.toastr.warning("Please select BEd type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementBedConfig.id) < 1) {
            this.toastr.warning("Please select replacement Bed config system.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.existingBedConfig.id) === Number(this.replacementBedConfig.id)) {
            this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (this.existingBedConfig.vehicleToBedConfigs.filter(item => item.isSelected).length <= 0) {
            this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }
    onSubmitChangeRequest() {
        if (this.validateReplaceConfirmBedConfig()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    this.existingBedConfig.attachments = uploadedFiles;
                }
                if (this.existingBedConfig.attachments) {
                    this.existingBedConfig.attachments = this.existingBedConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.existingBedConfig.vehicleToBedConfigs = this.existingBedConfig.vehicleToBedConfigs.filter(item => item.isSelected);
                this.existingBedConfig.vehicleToBedConfigs.forEach(v => { v.bedConfigId = this.replacementBedConfig.id; });

                let bedConfigIdentity: string = "Bed config id: " + this.existingBedConfig.id;

                this.bedConfigService.replaceBedConfig(this.existingBedConfig.id, this.existingBedConfig).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Bed Config", ConstantsWarehouse.changeRequestType.modify, bedConfigIdentity);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.modify} Bed Config ${bedConfigIdentity} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Config", ConstantsWarehouse.changeRequestType.modify, bedConfigIdentity);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                }, (errorresponse => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Config", ConstantsWarehouse.changeRequestType.modify, bedConfigIdentity);
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
