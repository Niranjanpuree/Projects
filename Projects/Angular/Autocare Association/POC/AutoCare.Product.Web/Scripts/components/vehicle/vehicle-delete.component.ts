import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute} from '@angular/router';
import { VehicleService } from './vehicle.service';
import { IVehicle } from './vehicle.model';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ConstantsWarehouse } from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'vehicle-delete-component',
    templateUrl: 'app/templates/vehicle/vehicle-delete.component.html',
    providers: [VehicleService]
})

export class VehicleDeleteComponent implements OnInit {
    vehicle: IVehicle;
    comment: string = '';
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;

    constructor(private vehicleService: VehicleService, private router: Router,
        private toastr: ToastsManager, private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.showLoadingGif = true;
        let id = Number(this.route.snapshot.params["id"]);
        this.vehicleService.getVehicle(id).subscribe(m => {
            this.vehicle = m;
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    onDeleteSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.vehicle.attachments = uploadedFiles;
            }
            if (this.vehicle.attachments) {
                this.vehicle.attachments = this.vehicle.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleService.deleteVehicle(this.vehicle.id, this.vehicle).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle", ConstantsWarehouse.changeRequestType.remove, 'Vehicle ID: ' + this.vehicle.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " Vehicle ID \"" + this.vehicle.id + "\"  change request Id  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this.router.navigateByUrl('vehicle/search');
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle", ConstantsWarehouse.changeRequestType.remove, 'Vehicle ID: ' + this.vehicle.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                this.showLoadingGif = false;
            }
                , error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle", ConstantsWarehouse.changeRequestType.remove, 'Vehicle ID: ' + this.vehicle.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
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