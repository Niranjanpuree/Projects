import { Component, OnInit, ViewChild } from            "@angular/core";
import { Router, ActivatedRoute } from  "@angular/router";
import { ToastsManager } from                           "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ModalComponent } from        "ng2-bs3-modal/ng2-bs3-modal";
import { BaseVehicleService } from                      "./baseVehicle.service";
import { ConstantsWarehouse } from                      "../constants-warehouse";
import { HttpHelper } from                              "../httpHelper";
import { IBaseVehicle } from                            "./baseVehicle.model";
import { VehicleService } from                          "../vehicle/vehicle.service";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "baseVehicle-modify-component",
    templateUrl: "app/templates/baseVehicle/baseVehicle-delete.component.html",
    providers: [BaseVehicleService, VehicleService, HttpHelper]
})
export class BaseVehicleDeleteComponent implements OnInit {
    public deleteBaseVehicle: IBaseVehicle;
    // TODO: comment text, file attachment

    // popup
    @ViewChild("viewAffectedVehiclesModal")
    public vehiclePopup: ModalComponent;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;
    constructor(private baseVehicleService: BaseVehicleService, private _vehicleService: VehicleService,
        private route: ActivatedRoute, private toastr: ToastsManager, private _router: Router) {
    }

    ngOnInit() {
        this.showLoadingGif = true;
        // Load existing base vechile with reference from RouteParams
        let id = Number(this.route.snapshot.params['id']);
        this.baseVehicleService.getBaseVehicle(id).subscribe(
            result => {
                this.deleteBaseVehicle = result;
                this.showLoadingGif = false;
            },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });

    };

    // delete
    onDeleteBaseVehicle() {
        // make current base vehicle identity
        let baseVehicleIdentity: string = this.deleteBaseVehicle.makeName + ", "
            + this.deleteBaseVehicle.modelName;
        // delete base vehicle
        //Comment parameter is added on deleteBaseVehicle
        //post called for delete to pass whole object
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteBaseVehicle.attachments = uploadedFiles;
            }
            if (this.deleteBaseVehicle.attachments) {
                this.deleteBaseVehicle.attachments = this.deleteBaseVehicle.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.baseVehicleService.deleteBaseVehicle(this.deleteBaseVehicle.id, this.deleteBaseVehicle).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Base Vehicle", ConstantsWarehouse.changeRequestType.remove, baseVehicleIdentity);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + baseVehicleIdentity + "\" Base Vehicle requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this._router.navigateByUrl('vehicle/search');
                } else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Base Vehicle", ConstantsWarehouse.changeRequestType.remove, baseVehicleIdentity);
                    errorMessage.title = "Your requested change cannot be submitted";
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                this.showLoadingGif = false;
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Base Vehicle", ConstantsWarehouse.changeRequestType.remove, baseVehicleIdentity);
                errorMessage.title = "Your requested change cannot be submitted";
                this.toastr.warning(errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.acFileUploader.reset();
                this.showLoadingGif = false;

            });

        }, error => {
            this.acFileUploader.reset();
        });
    }

    // view affected vehicles
    onViewAffectedVehicles() {
        if (!this.deleteBaseVehicle) {
            return;
        }

        this.vehiclePopup.open("lg");
        if (!this.deleteBaseVehicle.vehicles) {
            this._vehicleService.getVehiclesByBaseVehicleId(this.deleteBaseVehicle.id).subscribe(m => {
                this.deleteBaseVehicle.vehicles = m;
                this.deleteBaseVehicle.vehicleCount = m.length;
            },
                error => { this.toastr.warning(<any>error) });
        }
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers();
    }
}
