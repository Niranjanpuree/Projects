import {Component, ViewChild} from '@angular/core';
import { Router, ActivatedRoute} from '@angular/router';
import {IBaseVehicle} from './baseVehicle.model';
import {BaseVehicleService} from './baseVehicle.service';
import {HttpHelper} from '../httpHelper';
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {ConstantsWarehouse} from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';
@Component({
    selector: 'baseVehicle-replaceConfirm-component',
    templateUrl: 'app/templates/baseVehicle/baseVehicle-replaceConfirm.component.html',
    providers: [HttpHelper]
})

export class BaseVehicleReplaceConfirmComponent {
    existingBaseVehicle: IBaseVehicle;
    newBaseVehicle: IBaseVehicle;
    comment: string = '';
    showLoadingGif: boolean = false;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;
    constructor(private baseVehicleService: BaseVehicleService,
        private route: ActivatedRoute, private toastr: ToastsManager, private _router: Router) {
        this.existingBaseVehicle = baseVehicleService.existingBaseVehicle;
        this.newBaseVehicle = baseVehicleService.replacementBaseVehicle;
    }

    onReplaceChecked() {
        //validation for comment
        if (!this.existingBaseVehicle || !this.existingBaseVehicle.vehicles) {
            return;
        }
        if (this.existingBaseVehicle.vehicles.filter(item => item.isSelected).length <= 0) {
            this.toastr.warning("No vehicles selected", ConstantsWarehouse.validationTitle);
            return;
        }
        this.showLoadingGif = true; 
        this.existingBaseVehicle.vehicles = this.existingBaseVehicle.vehicles.filter(item => item.isSelected);    
        this.existingBaseVehicle.vehicles.forEach(v => { v.baseVehicleId = this.newBaseVehicle.id; });    
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.existingBaseVehicle.attachments = uploadedFiles;
            }
            if (this.existingBaseVehicle.attachments) {
                this.existingBaseVehicle.attachments = this.existingBaseVehicle.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.baseVehicleService.replaceBaseVehicle(this.existingBaseVehicle.id, this.existingBaseVehicle).subscribe(response => {
            if (response) {
                let successMessage = ConstantsWarehouse.notificationMessage.success("Base Vehicle", ConstantsWarehouse.changeRequestType.modify, 'Base Vehicle id: ' + this.existingBaseVehicle.id);
                successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + this.existingBaseVehicle.id + "\" Base Vehicle requestid  \"" + response + "\" will be reviewed.";
                this.toastr.success(successMessage.body, successMessage.title);
                this._router.navigateByUrl('vehicle/search');
            }
            else {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Base Vehicle", ConstantsWarehouse.changeRequestType.modify, 'Base Vehicle id: ' + this.existingBaseVehicle.id);
                this.toastr.warning(errorMessage.body, errorMessage.title);
            }
            this.showLoadingGif = false;
        }
            , error => {
                let errorMessage = ConstantsWarehouse.notificationMessage.error("Base Vehicle", ConstantsWarehouse.changeRequestType.modify, 'Base Vehicle id: ' + this.existingBaseVehicle.id);
                this.toastr.warning(errorMessage.body, errorMessage.title);
                this.showLoadingGif = false;
            }, () => {
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
