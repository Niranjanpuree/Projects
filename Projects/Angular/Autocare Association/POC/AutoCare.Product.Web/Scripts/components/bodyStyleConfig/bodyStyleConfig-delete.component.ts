import { Component, OnInit, ViewChild }             from "@angular/core";
import { Router, ActivatedRoute }   from "@angular/router";
import { ToastsManager }                            from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { IBodyStyleConfig }                             from "./bodyStyleConfig.model";
import { BodyStyleConfigService }                       from "./bodyStyleConfig.service";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "bodyStyleConfig-delete-component",
    templateUrl: "app/templates/bodyStyleConfig/bodyStyleConfig-delete.component.html",
    providers: [BodyStyleConfigService],
})

export class BodyStyleConfigDeleteComponent implements OnInit {
    public bodyStyleConfig: IBodyStyleConfig;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;

    constructor(private bodyStyleConfigService: BodyStyleConfigService, private toastr: ToastsManager,
        private route: ActivatedRoute, private router: Router) {
    }

    ngOnInit() {
        this.showLoadingGif = true;
        // Load Existing body style config with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        this.bodyStyleConfigService.getBodyStyleConfig(id).subscribe(result => {
            this.bodyStyleConfig = result;
            this.bodyStyleConfig.vehicleToBodyStyleConfigs = [];
            this.showLoadingGif = false;
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
    }

    //validation

    deleteSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.bodyStyleConfig.attachments = uploadedFiles;
            }
            if (this.bodyStyleConfig.attachments) {
                this.bodyStyleConfig.attachments = this.bodyStyleConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.bodyStyleConfigService.deleteBodyStyleConfig(this.bodyStyleConfig.id, this.bodyStyleConfig).subscribe(response => {
                if (response) {

                        let successMessage = ConstantsWarehouse.notificationMessage.success("", ConstantsWarehouse.changeRequestType.remove, "BodyStyleConfigId: " + this.bodyStyleConfig.id);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.remove} BodyStyleConfigId ${this.bodyStyleConfig.id} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("", ConstantsWarehouse.changeRequestType.remove, "BodyStyleConfigId: " + this.bodyStyleConfig.id);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                }                  
                },(errorresponse => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("", ConstantsWarehouse.changeRequestType.remove, "BodyStyleConfigId: " + this.bodyStyleConfig.id);
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