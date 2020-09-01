import {Component, OnInit, ViewChild} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {VehicleService } from './vehicle.service';
import { IVehicle } from './vehicle.model';
import {SubModelService} from '../subModel/subModel.service';
import {ISubModel} from '../subModel/subModel.model';
import {RegionService} from '../region/region.service';
import {IRegion} from '../region/region.model';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: 'vehicle-modify-component',
    templateUrl: 'app/templates/vehicle/vehicle-modify.component.html',
    providers: [VehicleService, SubModelService, RegionService]
})

export class VehicleModifyComponent implements OnInit {
    subModels: ISubModel[];
    regions: IRegion[];
    vehicle: IVehicle;
    modifiedVehicle: IVehicle;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    showLoadingGif: boolean = false;

    constructor(private subModelService: SubModelService, private regionService: RegionService,
        private vehicleService: VehicleService, private _router: Router,
        private toastr: ToastsManager, private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.showLoadingGif = true;
        let id = Number(this.route.snapshot.params["id"]);
        this.subModelService.getAllSubModels().subscribe(m => this.subModels = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        this.regionService.getRegion().subscribe(m => this.regions = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        this.vehicleService.getVehicle(id).subscribe(m => {
            this.vehicle = m;
            this.modifiedVehicle = <IVehicle>JSON.parse(JSON.stringify(this.vehicle));
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle)
            this.showLoadingGif = false;
        });
    }

    onModifySubmit() {
        if (this.modifiedVehicle.subModelId == -1) {
            this.toastr.warning('Please select submodel.', ConstantsWarehouse.validationTitle);
            return;
        }
        else if (this.modifiedVehicle.regionId == -1) {
            this.toastr.warning('Please select region.', ConstantsWarehouse.validationTitle);
            return;
        }
        else if (this.vehicle.subModelId == this.modifiedVehicle.subModelId && this.vehicle.regionId == this.modifiedVehicle.regionId) {
            this.toastr.warning('Nothing Changed', ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {

            if (uploadedFiles && uploadedFiles.length > 0) {
                this.modifiedVehicle.attachments = uploadedFiles;
            }
            if (this.modifiedVehicle.attachments) {
                this.modifiedVehicle.attachments = this.modifiedVehicle.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleService.updateVehicle(this.vehicle.id, this.modifiedVehicle).subscribe(response => {

                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle", ConstantsWarehouse.changeRequestType.modify, 'Vehicle ID: ' + this.vehicle.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " Vehicle ID \"" + this.vehicle.id + "\"  change request Id  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                    this._router.navigateByUrl('vehicle/search');
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle", ConstantsWarehouse.changeRequestType.modify, 'Vehicle ID: ' + this.vehicle.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                this.acFileUploader.reset();
                this.showLoadingGif = false;
            }, error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle", ConstantsWarehouse.changeRequestType.modify, 'Vehicle ID: ' + this.vehicle.id);
                this.toastr.warning(errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
                this.showLoadingGif = false;
                this.acFileUploader.reset(true);
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