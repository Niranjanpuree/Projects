import { Component, OnInit, ViewChild }             from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { ToastsManager }                            from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { IBrakeConfig }                             from "./brakeConfig.model";
import { IVehicleToBrakeConfig }                    from "../vehicleToBrakeConfig/vehicleToBrakeConfig.model";
import { BrakeConfigService }                       from "./brakeConfig.service";
import { AcFileUploader }                           from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "brakeConfig-replace-component",
    templateUrl: "app/templates/brakeConfig/brakeConfig-replaceConfirm.component.html",
})

export class BrakeConfigReplaceConfirmComponent implements OnInit {
    public existingBrakeConfig: IBrakeConfig;
    public replacementBrakeConfig: IBrakeConfig;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;

    constructor(private brakeConfigService: BrakeConfigService, private router: Router,
        private route: ActivatedRoute, private toastr: ToastsManager) {
    }

    ngOnInit() {
        // Load existing brake config with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        // Get existing / replace brake config records from factory/ service.
        this.existingBrakeConfig = this.brakeConfigService.existingBrakeConfig;
        this.replacementBrakeConfig = this.brakeConfigService.replacementBrakeConfig;
    }

    // validation
    private validateReplaceConfirmBrakeConfig(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (!this.existingBrakeConfig || !this.existingBrakeConfig.vehicleToBrakeConfigs || !this.replacementBrakeConfig) {
            this.toastr.warning("Not implemented.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementBrakeConfig.frontBrakeTypeId) === -1) {
            this.toastr.warning("Please select Front brake type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementBrakeConfig.rearBrakeTypeId) === -1) {
            this.toastr.warning("Please select Rear brake type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementBrakeConfig.brakeABSId) === -1) {
            this.toastr.warning("Please select Brake ABS.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementBrakeConfig.brakeSystemId) === -1) {
            this.toastr.warning("Please select Brake system.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementBrakeConfig.id) < 1) {
            this.toastr.warning("Please select replacement Brake config system.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.existingBrakeConfig.id) === Number(this.replacementBrakeConfig.id)) {
            this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (this.existingBrakeConfig.vehicleToBrakeConfigs.filter(item => item.isSelected).length <= 0) {
            this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    // event on submit change request
    //onSubmitChangeRequest() {
    //    debugger;
    //    if (this.validateReplaceConfirmBrakeConfig()) {
    //        for (let vehicleToBrakeConfig of this.existingBrakeConfig.vehicleToBrakeConfigs.filter(item => item.isSelected)) {
    //            let vehicleToBrakeConfigIdentity: string = "Vehicle to brake config id: " + vehicleToBrakeConfig.id;
    //            // change brake config id of vehicletobrakeconfig
    //            //vehicleToBrakeConfig.brakeConfig.id = this.replacementBrakeConfig.id;
    //            vehicleToBrakeConfig.brakeConfig = this.replacementBrakeConfig;
    //            this.vehicleToBrakeService.updateVehicleToBrakeConfig(vehicleToBrakeConfig.id, vehicleToBrakeConfig).subscribe(response => {
    //                if (response) {
    //                    let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle To Brake Config", ConstantsWarehouse.changeRequestType.modify, vehicleToBrakeConfigIdentity);
    //                    //successMessage.title = "You request to replace a vehicle to brake system will be reviewed.";
    //                    this.toastr.success(successMessage.body, successMessage.title);
    //                }
    //                else {
    //                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle To Brake Config", ConstantsWarehouse.changeRequestType.modify, vehicleToBrakeConfigIdentity);
    //                    //errorMessage.title = "Your requested change cannot be submitted.";
    //                    this.toastr.warning(errorMessage.body, errorMessage.title);
    //                }
    //            }, error => {
    //                let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle To Brake Config", ConstantsWarehouse.changeRequestType.modify, vehicleToBrakeConfigIdentity);
    //                    //errorMessage.title = "Your requested change cannot be submitted.";
    //                    this.toastr.warning(errorMessage.body, errorMessage.title);
    //            });
    //        }
    //        // redirect to search result
    //        //this.router.navigate(["BrakeConfigSearch"]);
    //    }
    //}

    onSubmitChangeRequest() {
        if (this.validateReplaceConfirmBrakeConfig()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    this.existingBrakeConfig.attachments = uploadedFiles;
                }
                if (this.existingBrakeConfig.attachments) {
                    this.existingBrakeConfig.attachments = this.existingBrakeConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.existingBrakeConfig.vehicleToBrakeConfigs = this.existingBrakeConfig.vehicleToBrakeConfigs.filter(item => item.isSelected);
                this.existingBrakeConfig.vehicleToBrakeConfigs.forEach(v => { v.brakeConfigId = this.replacementBrakeConfig.id; });

                let brakeConfigIdentity: string = "Brake config id: " + this.existingBrakeConfig.id;

                this.brakeConfigService.replaceBrakeConfig(this.existingBrakeConfig.id, this.existingBrakeConfig).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Brake Config", ConstantsWarehouse.changeRequestType.modify, brakeConfigIdentity);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.modify} Brake Config ${brakeConfigIdentity} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake Config", ConstantsWarehouse.changeRequestType.modify, brakeConfigIdentity);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                }, (errorresponse => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake Config", ConstantsWarehouse.changeRequestType.modify, brakeConfigIdentity);
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
