import { Component, OnInit, ViewChild }             from "@angular/core";
import { Router, ActivatedRoute }   from "@angular/router";
import { ToastsManager }                            from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { IBedConfig }                             from "./bedConfig.model";
import { BedConfigService }                       from "./bedConfig.service";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "bedConfig-delete-component",
    templateUrl: "app/templates/bedConfig/bedConfig-delete.component.html",
    providers: [BedConfigService],
})

export class BedConfigDeleteComponent implements OnInit {
    public bedConfig: IBedConfig;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;

    constructor(private bedConfigService: BedConfigService, private toastr: ToastsManager,
        private route: ActivatedRoute, private router: Router) {
    }

    ngOnInit() {
        this.showLoadingGif = true;
        // Load Existing bed config with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        this.bedConfigService.getBedConfig(id).subscribe(result => {
            this.bedConfig = result;
            this.bedConfig.vehicleToBedConfigs = [];
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
                this.bedConfig.attachments = uploadedFiles;
            }
            if (this.bedConfig.attachments) {
                this.bedConfig.attachments = this.bedConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.bedConfigService.deleteBedConfig(this.bedConfig.id, this.bedConfig).subscribe(response => {
                if (response) {

                        let successMessage = ConstantsWarehouse.notificationMessage.success("", ConstantsWarehouse.changeRequestType.remove, "BedConfigId: " + this.bedConfig.id);
                        successMessage.title = `You request to ${ConstantsWarehouse.changeRequestType.remove} BedConfigId ${this.bedConfig.id} change request ID "${response}" will be reviewed.`;
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("", ConstantsWarehouse.changeRequestType.remove, "BedConfigId: " + this.bedConfig.id);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                }                  
                },(errorresponse => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("", ConstantsWarehouse.changeRequestType.remove, "BedConfigId: " + this.bedConfig.id);
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