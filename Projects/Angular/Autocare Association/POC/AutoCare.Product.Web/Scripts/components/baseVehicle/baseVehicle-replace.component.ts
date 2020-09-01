import { Component, OnInit, ViewChild } from            "@angular/core";
import { Router, ActivatedRoute } from  "@angular/router";
import { ToastsManager } from                           "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { IBaseVehicle } from                            "./baseVehicle.model";
import { BaseVehicleService } from                      "./baseVehicle.service";
import { IMake } from                                   "../make/make.model";
import { MakeService } from                             "../make/make.service";
import { IModel } from                                  "../model/model.model";
import { YearService } from                            "../year/year.service";
import { IYear} from                                    "../year/year.model";
import { IVehicle } from                                "../vehicle/vehicle.model";
import { VehicleService } from                          "../vehicle/vehicle.service";
import { HttpHelper } from                              "../httpHelper";
import {ConstantsWarehouse} from '../constants-warehouse';

@Component({
    selector: "baseVehicle-replace-component",
    templateUrl: "app/templates/baseVehicle/baseVehicle-replace.component.html",
    providers: [YearService, MakeService, VehicleService, HttpHelper]
})

export class BaseVehicleReplaceComponent {
    existingBaseVehicle: IBaseVehicle;
    newBaseVehicle: IBaseVehicle = { id: -1, makeId: -1, makeName: "", modelId: -1, modelName: "", yearId: -1, vehicles: null };
    makes: IMake[];
    models: IModel[];
    years: IYear[];
    baseIdSearchText: string;
    showLoadingGif: boolean = false;
    isSelectAllVehiclesToReplace: boolean;

    constructor(private _makeService: MakeService, private yearService: YearService, private baseVehicleService: BaseVehicleService, private vehicleService: VehicleService
        , private route: ActivatedRoute, private toastr: ToastsManager, private router: Router) {
    }

    ngOnInit() {
        this.isSelectAllVehiclesToReplace = false;
        this.showLoadingGif = true;
        let id = this.route.snapshot.params['id'];
        this.makes = [];
        this.models = [];
        this.years = [];
        this.baseVehicleService.getBaseVehicle(id).subscribe(m => {
            this.existingBaseVehicle = m;
            this.showLoadingGif = false;
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });

        this.yearService.getYears().subscribe(m => this.years = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onBaseIdKeyPress(event) {
        if (event.keyCode == 13) {
            this.onBaseIdSearch();
        }
    }

    onBaseIdSearch() {
        let baseId = Number(this.baseIdSearchText);
        if (isNaN(baseId)) {
            this.toastr.warning("Invalid Base ID", "Message");
            return;
        }

        if (this.newBaseVehicle.id == baseId) {
            return;
        }

        this.newBaseVehicle = { id: -1, yearId: -1, makeId: -1, makeName: "", modelId: -1, modelName: "", vehicles: null };
        this.makes = null;
        this.models = null;

        this.showLoadingGif = true;
        this.baseVehicleService.getBaseVehicle(baseId).subscribe(b => {
            this._makeService.getMakesByYearId(b.yearId).subscribe(mks => {
                this.makes = mks;

                this.baseVehicleService.getModelsByYearIdAndMakeId(b.yearId, b.makeId).subscribe(mdls => {
                    this.models = mdls;
                    this.newBaseVehicle = b;

                    this.showLoadingGif = false;
                }, error => {
                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    this.showLoadingGif = false;
                });
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
        }, error => {
            let errorMessage: string = JSON.parse(String(error)).message;
            this.toastr.warning(errorMessage, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }


  
    onSelectYear() {
        this.newBaseVehicle.id = -1;
        this.models = [];
        this.newBaseVehicle.makeId = -1;

        if (this.newBaseVehicle.yearId == -1) {
            this.makes = [];
            return;
        }

        this.makes = null;

        this._makeService.getMakesByYearId(this.newBaseVehicle.yearId).subscribe(m => this.makes = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onSelectMake() {
        this.newBaseVehicle.id = -1;
        this.models = [];

        if (this.newBaseVehicle.makeId == -1) {
            this.models = [];
            return;
        }

        this.models = null;

        this.baseVehicleService.getModelsByYearIdAndMakeId(this.newBaseVehicle.yearId, this.newBaseVehicle.makeId).subscribe(m => this.models = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onViewAffectedVehicles() {
        if (this.existingBaseVehicle && !this.existingBaseVehicle.vehicles) {
            this.showLoadingGif = true;
            this.vehicleService.getVehiclesByBaseVehicleId(this.existingBaseVehicle.id).subscribe(m => {
                this.existingBaseVehicle.vehicles = m;
                this.existingBaseVehicle.vehicleCount = m.length;

                this.showLoadingGif = false;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
        }
    }

    onSelectAllVehicles(isSelected) {
        this.isSelectAllVehiclesToReplace = isSelected;
        if (this.existingBaseVehicle.vehicles == null) {
            return;
        }
        this.existingBaseVehicle.vehicles.forEach(item => item.isSelected = isSelected);
    }

    onVehiclesToReplaceSelect(vehicle: IVehicle) {
        if (vehicle.isSelected) {
            //unchecked
            this.isSelectAllVehiclesToReplace = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingBaseVehicle.vehicles.filter(item => item.id != vehicle.id);
            if (excludedVehicle.every(item => item.isSelected)) {
                this.isSelectAllVehiclesToReplace = true;
            }
        }
    }

    onContinue() {
        if (!this.newBaseVehicle || this.newBaseVehicle.id == -1) {
            this.toastr.warning("Please select a replacement base vehicle", "Message");
            return;
        }

        if (this.existingBaseVehicle.vehicles == null) {
            this.toastr.warning("No vehicles selected", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.newBaseVehicle.id == this.existingBaseVehicle.id) {
            this.toastr.warning("Nothing changed", ConstantsWarehouse.validationTitle);
            return;
        }

        if (this.existingBaseVehicle.vehicles.filter(item => item.isSelected).length <= 0) {
            this.toastr.warning("No vehicles selected", ConstantsWarehouse.validationTitle);
            return;
        }

        this.baseVehicleService.existingBaseVehicle = this.existingBaseVehicle;
        this.baseVehicleService.existingBaseVehicle.vehicles = this.existingBaseVehicle.vehicles.filter(item => item.isSelected);

        this.newBaseVehicle.makeName = this.makes.filter(item => item.id == this.newBaseVehicle.makeId)[0].name;

        let selectedModel: IModel = this.models.filter(item => item.baseVehicleId == this.newBaseVehicle.id)[0];
        this.newBaseVehicle.modelId = selectedModel.id;
        this.newBaseVehicle.modelName = selectedModel.name;

        this.baseVehicleService.replacementBaseVehicle = this.newBaseVehicle;

        this.router.navigateByUrl("/basevehicle/replace/" + this.existingBaseVehicle.id + "/confirm");
    }
}
