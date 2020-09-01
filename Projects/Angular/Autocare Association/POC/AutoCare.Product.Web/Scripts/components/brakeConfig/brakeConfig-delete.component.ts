import { Component, OnInit, ViewChild }             from "@angular/core";
import { Router, ActivatedRoute }   from "@angular/router";
import { ToastsManager }                            from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { IBrakeConfig }                             from "./brakeConfig.model";
import { BrakeConfigService }                       from "./brakeConfig.service";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "brakeConfig-delete-component",
    templateUrl: "app/templates/brakeConfig/brakeConfig-delete.component.html",
    providers: [BrakeConfigService],
})

export class BrakeConfigDeleteComponent implements OnInit {
    public brakeConfig: IBrakeConfig;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;

    constructor(private brakeConfigService: BrakeConfigService, private toastr: ToastsManager,
        private route: ActivatedRoute, private router: Router) {
    }

    ngOnInit() {
        this.showLoadingGif = true;
        // Load Existing brake config with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        this.brakeConfigService.getBrakeConfig(id).subscribe(result => {
            this.brakeConfig = result;
            this.brakeConfig.vehicleToBrakeConfigs = [];
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    deleteSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.brakeConfig.attachments = uploadedFiles;
            }
            if (this.brakeConfig.attachments) {
                this.brakeConfig.attachments = this.brakeConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.brakeConfigService.deleteBrakeConfig(this.brakeConfig.id, this.brakeConfig).subscribe(response => {
                if (response) {

                    let successMessage = ConstantsWarehouse.notificationMessage.success("", ConstantsWarehouse.changeRequestType.remove, "BrakeConfigId: " + this.brakeConfig.id);
                    successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.remove} BrakeConfigId ${this.brakeConfig.id} change request ID "${response}" will be reviewed.`;
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.router.navigateByUrl("/system/search");
                } else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("", ConstantsWarehouse.changeRequestType.remove, "BrakeConfigId: " + this.brakeConfig.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (errorresponse => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("", ConstantsWarehouse.changeRequestType.remove, "BrakeConfigId: " + this.brakeConfig.id);
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

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers();
    }
}