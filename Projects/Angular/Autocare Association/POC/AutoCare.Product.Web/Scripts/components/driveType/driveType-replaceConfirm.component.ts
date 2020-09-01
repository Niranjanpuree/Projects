import { Component, OnInit, ViewChild }             from "@angular/core";
import { Router, ActivatedRoute }from "@angular/router";
import { ToastsManager }                            from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ModalComponent }         from "ng2-bs3-modal/ng2-bs3-modal";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { IDriveType }                               from "./driveType.model";
import { DriveTypeService }                         from "./driveType.service";
import { AcFileUploader }                           from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }                               from 'rxjs/Observable';

@Component({
    selector: "driveType-replace-component",
    templateUrl: "app/templates/driveType/driveType-replaceConfirm.component.html",
})

export class DriveTypeReplaceConfirmComponent implements OnInit {
    public existingDriveType: IDriveType;
    public replacementDriveType: IDriveType;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;

    constructor(private driveTypeService: DriveTypeService, private router: Router,
        private route: ActivatedRoute, private toastr: ToastsManager) {
    }

    ngOnInit() {
        // Load existing DriveType  with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        // Get existing / replace DriveType records from factory/ service.
        this.existingDriveType = this.driveTypeService.existingDriveType;
        this.replacementDriveType = this.driveTypeService.replacementDriveType;
    }

    // validation
    private validateReplaceConfirmDriveType(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (!this.existingDriveType || !this.existingDriveType.vehicleToDriveTypes || !this.replacementDriveType) {
            this.toastr.warning("Not implemented.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementDriveType.id) === -1) {
            this.toastr.warning("Please select Drive Type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementDriveType.id) < 1) {
            this.toastr.warning("Please select replacement Drive Type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.existingDriveType.id) === Number(this.replacementDriveType.id)) {
            this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (this.existingDriveType.vehicleToDriveTypes.filter(item => item.isSelected).length <= 0) {
            this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }
    onSubmitChangeRequest() {
        if (this.validateReplaceConfirmDriveType()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    this.existingDriveType.attachments = uploadedFiles;
                }
                if (this.existingDriveType.attachments) {
                    this.existingDriveType.attachments = this.existingDriveType.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.existingDriveType.vehicleToDriveTypes = this.existingDriveType.vehicleToDriveTypes.filter(item => item.isSelected);
                this.existingDriveType.vehicleToDriveTypes.forEach(v => { v.driveTypeId = this.replacementDriveType.id; });

                let driveTypeIdentity: string = "Drive Type id: " + this.existingDriveType.id;

                this.driveTypeService.replaceDriveTypeConfig(this.existingDriveType.id, this.existingDriveType).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Drive Type", ConstantsWarehouse.changeRequestType.modify, driveTypeIdentity);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.modify} Drive Type ${driveTypeIdentity} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Drive Type", ConstantsWarehouse.changeRequestType.modify, driveTypeIdentity);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                }, (errorresponse => {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Drive Type", ConstantsWarehouse.changeRequestType.modify, driveTypeIdentity);
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
