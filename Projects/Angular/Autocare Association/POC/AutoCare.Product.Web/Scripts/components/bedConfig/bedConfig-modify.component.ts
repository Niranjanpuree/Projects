import { Component, OnInit, ViewChild }                        from "@angular/core";
import { Router, ActivatedRoute }   from "@angular/router";
import { ToastsManager }                            from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { IBedConfig }                             from "./bedConfig.model";
import { BedConfigService }                       from "./bedConfig.service";
import { IBedType }                               from "../bedType/bedType.model";
import { BedTypeService }                         from "../bedType/bedType.service";
import { BedLengthService }                          from "../bedLength/bedLength.service";
import { IBedLength }                                from "../bedLength/bedLength.model";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "bedConfig-modify-component",
    templateUrl: "app/templates/bedConfig/bedConfig-modify.component.html",
    providers: [BedConfigService],
})

export class BedConfigModifyComponent implements OnInit {
    existingBedConfig: IBedConfig;
    modifiedBedConfig: IBedConfig;
    bedTypes: IBedType[];
    bedLengths: IBedLength[];
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;
    constructor(private bedConfigService: BedConfigService,
        private bedLengthService: BedLengthService,
        private bedTypeService: BedTypeService,
        private toastr: ToastsManager,
        private route: ActivatedRoute,
        private router: Router
    ) {
    }

    ngOnInit() {
        this.showLoadingGif = true;
        let id = Number(this.route.snapshot.params["id"]);
        this.bedTypeService.getAllBedTypes().subscribe(x => this.bedTypes = x,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        this.bedLengthService.getAllBedLengths().subscribe(x => this.bedLengths = x,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        this.bedConfigService.getBedConfig(id).subscribe(x => {
            this.existingBedConfig = x;
            this.modifiedBedConfig = <IBedConfig>JSON.parse(JSON.stringify(this.existingBedConfig));
            this.showLoadingGif = false;
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
    }

    // validation
    private validateModifyBedConfig(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (Number(this.modifiedBedConfig.bedLengthId) === -1) {
            this.toastr.warning("Please select Bed Length.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.modifiedBedConfig.bedTypeId) === -1) {
            this.toastr.warning("Please select Bed Type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onSubmitChangeRequests() {
        if (this.validateModifyBedConfig()) {
            let bedConfigIdentity: string = this.modifiedBedConfig.length + ','
                + this.modifiedBedConfig.bedTypeName;
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    this.modifiedBedConfig.attachments = uploadedFiles;
                }
                if (this.modifiedBedConfig.attachments) {
                    this.modifiedBedConfig.attachments = this.modifiedBedConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.bedConfigService.updateBedConfig(this.modifiedBedConfig.id, this.modifiedBedConfig).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Bed System", ConstantsWarehouse.changeRequestType.modify, bedConfigIdentity);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.modify} Bed System ${bedConfigIdentity} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed System", ConstantsWarehouse.changeRequestType.modify, bedConfigIdentity);
                        errorMessage.title = "Your requested change cannot be submitted.";
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                }, (errorresponse => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed System", ConstantsWarehouse.changeRequestType.modify, bedConfigIdentity);
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
