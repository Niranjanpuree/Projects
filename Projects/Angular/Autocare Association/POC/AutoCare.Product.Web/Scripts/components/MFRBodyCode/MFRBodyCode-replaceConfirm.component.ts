import { Component, OnInit, ViewChild }             from "@angular/core";
import { Router, ActivatedRoute }from "@angular/router";
import { ToastsManager }                            from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ModalComponent }         from "ng2-bs3-modal/ng2-bs3-modal";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { IMfrBodyCode }                               from "./mfrBodyCode.model";
import { MfrBodyCodeService }                         from "./mfrBodyCode.service";
import { AcFileUploader }                           from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }                               from 'rxjs/Observable';

@Component({
    selector: "mfrBodyCode-replace-component",
    templateUrl: "app/templates/mfrBodyCode/mfrBodyCode-replaceConfirm.component.html",
})

export class MfrBodyCodeReplaceConfirmComponent implements OnInit {
    

    public existingMfrBodyCode: IMfrBodyCode;
    public replacementMfrBodyCode: IMfrBodyCode;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;

    constructor(private mfrBodyCodeService: MfrBodyCodeService, private router: Router,
        private route: ActivatedRoute, private toastr: ToastsManager) {
    }

    ngOnInit() {
        // Load existing MfrBodyCode  with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        // Get existing / replace MfrBodyCode records from factory/ service.
        this.existingMfrBodyCode = this.mfrBodyCodeService.existingMfrBodyCode;
        this.replacementMfrBodyCode = this.mfrBodyCodeService.replacementMfrBodyCode;
    }

    // validation
    private validateReplaceConfirmMfrBodyCode(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (!this.existingMfrBodyCode || !this.existingMfrBodyCode.vehicleToMfrBodyCodes || !this.replacementMfrBodyCode) {
            this.toastr.warning("Not implemented.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementMfrBodyCode.id) === -1) {
            this.toastr.warning("Please select Mfr Body Code.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.replacementMfrBodyCode.id) < 1) {
            this.toastr.warning("Please select replacement Mfr Body Code.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (Number(this.existingMfrBodyCode.id) === Number(this.replacementMfrBodyCode.id)) {
            this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
            isValid = false;
        } else if (this.existingMfrBodyCode.vehicleToMfrBodyCodes.filter(item => item.isSelected).length <= 0) {
            this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }
    onSubmitChangeRequest() {
        if (this.validateReplaceConfirmMfrBodyCode()) {
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    this.existingMfrBodyCode.attachments = uploadedFiles;
                }
                if (this.existingMfrBodyCode.attachments) {
                    this.existingMfrBodyCode.attachments = this.existingMfrBodyCode.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.existingMfrBodyCode.vehicleToMfrBodyCodes = this.existingMfrBodyCode.vehicleToMfrBodyCodes.filter(item => item.isSelected);
                this.existingMfrBodyCode.vehicleToMfrBodyCodes.forEach(v => { v.mfrBodyCodeId = this.replacementMfrBodyCode.id; });

                let mfrBodyCodeIdentity: string = "Mfr Body Code id: " + this.existingMfrBodyCode.id;

                this.mfrBodyCodeService.replaceMfrBodyCode(this.existingMfrBodyCode.id, this.existingMfrBodyCode).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Mfr Body Code", ConstantsWarehouse.changeRequestType.modify, mfrBodyCodeIdentity);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.modify} Mfr Body Code ${mfrBodyCodeIdentity} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Mfr Body Code", ConstantsWarehouse.changeRequestType.modify, mfrBodyCodeIdentity);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                }, (errorresponse => {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Mfr Body Code", ConstantsWarehouse.changeRequestType.modify, mfrBodyCodeIdentity);
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
