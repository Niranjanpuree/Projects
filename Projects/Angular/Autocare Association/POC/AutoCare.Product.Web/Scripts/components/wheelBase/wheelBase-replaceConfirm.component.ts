import { Component, OnInit, ViewChild }             from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { ToastsManager }                            from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { IWheelBase }                             from "./wheelBase.model";
import { IVehicleToWheelBase }                    from "../vehicleToWheelBase/vehicleToWheelBase.model";
import { WheelBaseService }                       from "./wheelBase.service";
import { AcFileUploader }                           from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "wheelBase-replace-component",
    templateUrl: "app/templates/wheelBase/wheelBase-replaceConfirm.component.html",
})

export class WheelBaseReplaceConfirmComponent implements OnInit {
    public existingWheelBase: IWheelBase;
    public replacementWheelBase: IWheelBase;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;

    constructor(private wheelBaseService: WheelBaseService, private router: Router,
        private route: ActivatedRoute, private toastr: ToastsManager) {
    }

    ngOnInit() {
        let id = Number(this.route.snapshot.params["id"]);

        this.existingWheelBase = this.wheelBaseService.existingWheelBase;
        this.replacementWheelBase = this.wheelBaseService.replacementWheelBase;
    }

    // validation
    private validateReplaceConfirmWheelBase(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (!this.existingWheelBase || !this.existingWheelBase.vehicleToWheelBases || !this.replacementWheelBase) {
            this.toastr.warning("Not implemented.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } if (Number(this.replacementWheelBase.id) < 1) {
            this.toastr.warning("Please select replacement Wheel Base.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.existingWheelBase.id) === Number(this.replacementWheelBase.id)) {
            this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (this.existingWheelBase.vehicleToWheelBases.filter(item => item.isSelected).length <= 0) {
            this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onSubmitChangeRequest() {
        if (this.validateReplaceConfirmWheelBase()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    this.existingWheelBase.attachments = uploadedFiles;
                }
                if (this.existingWheelBase.attachments) {
                    this.existingWheelBase.attachments = this.existingWheelBase.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.existingWheelBase.vehicleToWheelBases = this.existingWheelBase.vehicleToWheelBases.filter(item => item.isSelected);
                this.existingWheelBase.vehicleToWheelBases.forEach(v => { v.wheelBaseId = this.replacementWheelBase.id; });

                this.wheelBaseService.replaceWheelBase(this.existingWheelBase.id, this.existingWheelBase).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Wheel Base", ConstantsWarehouse.changeRequestType.modify, this.existingWheelBase.id);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.modify} Wheel Base ID ${this.existingWheelBase.id} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Wheel Base", ConstantsWarehouse.changeRequestType.modify, this.existingWheelBase.id);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                }, (errorresponse => {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Wheel Base", ConstantsWarehouse.changeRequestType.modify, this.existingWheelBase.id);
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
