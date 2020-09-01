import { Component, OnInit, ViewChild }                        from "@angular/core";
import { Router, ActivatedRoute }   from "@angular/router";
import { ToastsManager }                            from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { IBrakeConfig }                             from "./brakeConfig.model";
import { BrakeConfigService }                       from "./brakeConfig.service";
import { IBrakeType }                               from "../brakeType/brakeType.model";
import { BrakeTypeService }                         from "../brakeType/brakeType.service";
import { BrakeABSService }                          from "../brakeABS/brakeABS.service";
import { IBrakeABS }                                from "../brakeABS/brakeABS.model";
import { BrakeSystemService }                       from "../brakeSystem/brakeSystem.service";
import { IBrakeSystem }                             from "../brakeSystem/brakeSystem.model";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "brakeConfig-modify-component",
    templateUrl: "app/templates/brakeConfig/brakeConfig-modify.component.html",
    providers: [BrakeConfigService],
})

export class BrakeConfigModifyComponent implements OnInit {
    existingBrakeConfig: IBrakeConfig;
    modifiedBrakeConfig: IBrakeConfig;
    brakeTypes: IBrakeType[];
    brakeABSes: IBrakeABS[];
    brakeSystems: IBrakeSystem[];
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;

    constructor(private _brakeConfigService: BrakeConfigService,
        private _brakeAbsService: BrakeABSService,
        private _brakeSystemService: BrakeSystemService,
        private _brakeTypeSerivce: BrakeTypeService,
        private toastr: ToastsManager,
        private route: ActivatedRoute,
        private router: Router
    ) {
    }

    ngOnInit() {
        this.showLoadingGif = true;
        let id = Number(this.route.snapshot.params["id"]);
        this._brakeTypeSerivce.getAllBrakeTypes().subscribe(x => this.brakeTypes = x,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        this._brakeAbsService.getAllBrakeABSes().subscribe(x => this.brakeABSes = x,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        this._brakeSystemService.getAllBrakeSystems().subscribe(x => this.brakeSystems = x,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        this._brakeConfigService.getBrakeConfig(id).subscribe(x => {
            this.existingBrakeConfig = x;
            this.modifiedBrakeConfig = <IBrakeConfig>JSON.parse(JSON.stringify(this.existingBrakeConfig));
            this.showLoadingGif = false;
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
    }

    // validation
    private validateModifyBrakeConfig(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (Number(this.modifiedBrakeConfig.frontBrakeTypeId) === -1) {
            this.toastr.warning("Please select Front brake type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.modifiedBrakeConfig.rearBrakeTypeId) === -1) {
            this.toastr.warning("Please select Rear brake type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.modifiedBrakeConfig.brakeABSId) === -1) {
            this.toastr.warning("Please select Brake ABS.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.modifiedBrakeConfig.brakeSystemId) === -1) {
            this.toastr.warning("Please select Brake system.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onSubmitChangeRequests() {
        if (this.validateModifyBrakeConfig()) {
            let brakeConfigIdentity: string = this.modifiedBrakeConfig.frontBrakeTypeName + ','
                + this.modifiedBrakeConfig.rearBrakeTypeName + ',' + this.modifiedBrakeConfig.brakeABSName
                + ',' + this.modifiedBrakeConfig.brakeSystemName;
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    this.modifiedBrakeConfig.attachments = uploadedFiles;
                }
                if (this.modifiedBrakeConfig.attachments) {
                    this.modifiedBrakeConfig.attachments = this.modifiedBrakeConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this._brakeConfigService.updateBrakeConfig(this.modifiedBrakeConfig.id, this.modifiedBrakeConfig).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Brake System", ConstantsWarehouse.changeRequestType.modify, brakeConfigIdentity);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.modify} Brake System ${brakeConfigIdentity} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake System", ConstantsWarehouse.changeRequestType.modify, brakeConfigIdentity);
                        errorMessage.title = "Your requested change cannot be submitted.";
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                }, (errorresponse => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake System", ConstantsWarehouse.changeRequestType.modify, brakeConfigIdentity);
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
