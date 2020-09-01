import { Component, OnInit, ViewChild }                      from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { ToastsManager }                    from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ModalComponent } from "ng2-bs3-modal/ng2-bs3-modal";

import { IBaseVehicle }       from "./baseVehicle.model";
import { BaseVehicleService } from "./baseVehicle.service";
import { HttpHelper }         from "../httpHelper";
import { IMake }              from "../make/make.model";
import { IModel }             from "../model/model.model";
import { IYear }              from "../year/year.model";
import { MakeService }        from "../make/make.service";
import { ModelService }       from "../model/model.service";
import { YearService }        from "../year/year.service";
import { ConstantsWarehouse } from "../constants-warehouse";
import { VehicleService }     from "../vehicle/vehicle.service";
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "baseVehicle-modify-component",
    templateUrl: "app/templates/baseVehicle/baseVehicle-modify.component.html",
    providers: [BaseVehicleService, VehicleService, HttpHelper],
})
export class BaseVehicleModifyComponent implements OnInit {
    public existingBaseVehicle: IBaseVehicle;
    public changeBaseVehicle: IBaseVehicle;
    public makes: IMake[];
    public models: IModel[];
    public years: IYear[];
    // TODO: comment text, file attachment

    // popup
    @ViewChild("viewAffectedVehiclesModal")
    public vehiclePopup: ModalComponent;

    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;

    showLoadingGif: boolean = false;

    constructor(private baseVehicleService: BaseVehicleService, private _vehicleService: VehicleService,
        private route: ActivatedRoute, private toastr: ToastsManager,
        private makeService: MakeService, private modelService: ModelService, private yearService: YearService, private _router: Router) {
        // initalize empty changeBaseVehicle
        this.changeBaseVehicle = { id: 0, makeId: -1, makeName: '', modelId: -1, modelName: '', yearId: -1, vehicles: null };
    }

    ngOnInit() {
        this.showLoadingGif = true;
        // Load existing base vechile with reference from RouteParams
        let id = Number(this.route.snapshot.params['id']);
        this.baseVehicleService.getBaseVehicle(id).subscribe(
            result => {
                this.existingBaseVehicle = result;
            },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
        // Load select options for change.
        this.makeService.getAllMakes().subscribe(mks => {
            this.makes = mks;
            this.modelService.getAllModels().subscribe(mdls => {
                this.models = mdls;
                this.changeBaseVehicle = <IBaseVehicle>JSON.parse(JSON.stringify(this.existingBaseVehicle));
                this.showLoadingGif = false;
            },
                error => {
                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    this.showLoadingGif = false;
                }); // models
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            }); // makes
        this.yearService.getYears().subscribe(m => this.years = m,
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            }); // years

    }

    // Save changes
    onModifyBaseVehicle() {
        // validate change information
        if (this.validateChangeBaseVehicle()) {
            // make current base vehicle identity
            let baseVehicleIdentity: string = this.makes.filter(item => item.id === Number(this.changeBaseVehicle.makeId))[0].name + ", "
                + this.models.filter(item => item.id === Number(this.changeBaseVehicle.modelId))[0].name;
            // modify base vehicle
            this.changeBaseVehicle.id = this.existingBaseVehicle.id;
            //NOTE: save numberOfVehicles in payload string, will be used in change review screen
            this.changeBaseVehicle.vehicleCount = this.existingBaseVehicle.vehicleCount;
            this.showLoadingGif = true;
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    this.changeBaseVehicle.attachments = uploadedFiles;
                }
                if (this.changeBaseVehicle.attachments) {
                    this.changeBaseVehicle.attachments = this.changeBaseVehicle.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.baseVehicleService.updateBaseVehicle(this.existingBaseVehicle.id, this.changeBaseVehicle).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Base Vehicle", ConstantsWarehouse.changeRequestType.modify, baseVehicleIdentity);
                        successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.modify + " the \"" + baseVehicleIdentity + "\" Base Vehicle requestid  \"" + response + "\" will be reviewed.";
                        this.toastr.success(successMessage.body, successMessage.title);
                        this._router.navigateByUrl('vehicle/search');
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Base Vehicle", ConstantsWarehouse.changeRequestType.modify, baseVehicleIdentity);
                        errorMessage.title = "Your requested change cannot be submitted.";
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                    }
                    this.showLoadingGif = false;
                }, error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Base Vehicle", ConstantsWarehouse.changeRequestType.modify, baseVehicleIdentity);
                    errorMessage.title = "Your requested change cannot be submitted.";
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
    }

    // validate base vehicle
    validateChangeBaseVehicle(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (Number(this.changeBaseVehicle.makeId) === -1) {
            this.toastr.warning("Please select Make.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.changeBaseVehicle.modelId) === -1) {
            this.toastr.warning("Please select Model.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.changeBaseVehicle.yearId) === -1) {
            this.toastr.warning("Please select Year.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        // check if existing and change base vehicle information exactly same
        else if (this.existingBaseVehicle.makeId === Number(this.changeBaseVehicle.makeId)
            && this.existingBaseVehicle.modelId === Number(this.changeBaseVehicle.modelId)
            && this.existingBaseVehicle.yearId === Number(this.changeBaseVehicle.yearId)
        ) {
            this.toastr.warning("Existing and Change information are exactly same.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    // view affected vehicles
    onViewAffectedVehicles() {
        if (!this.existingBaseVehicle) {
            return;
        }

        this.vehiclePopup.open("lg");
        if (!this.existingBaseVehicle.vehicles) {
            this._vehicleService.getVehiclesByBaseVehicleId(this.existingBaseVehicle.id).subscribe(m => {
                this.existingBaseVehicle.vehicles = m;
                this.existingBaseVehicle.vehicleCount = m.length;
            },
                error => {
                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                });
        }
    }
    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers();
    }
}