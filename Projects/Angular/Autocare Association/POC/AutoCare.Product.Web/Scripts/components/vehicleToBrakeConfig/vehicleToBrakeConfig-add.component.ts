import { Component, OnInit, ViewChild } from "@angular/core";
import { Router } from    "@angular/router";
import { BaseVehicleService } from           "../baseVehicle/baseVehicle.service";
import { IMake } from                        "../make/make.model";
import { MakeService } from                  "../make/make.service";
import { IModel } from                       "../model/model.model";
import { ModelService } from                 "../model/model.service";
import { IYear } from                       "../year/year.model";
import { YearService } from                  "../year/year.service";
import { IRegion } from                      "../region/region.model";
import { RegionService } from                "../region/region.service";
import { ISubModel } from                    "../submodel/submodel.model";
import { SubModelService } from              "../submodel/submodel.service";
import { IVehicleToBrakeConfig } from        "./vehicleToBrakeConfig.model";
import { VehicleToBrakeConfigService } from  "./vehicleToBrakeConfig.service";
import { IVehicle } from                     "../vehicle/vehicle.model";
import { VehicleService } from               "../vehicle/vehicle.service";
import { IBrakeConfig } from                 "../brakeConfig/brakeConfig.model";
import { BrakeConfigService } from           "../brakeConfig/brakeConfig.service";
import { IBrakeType } from                   "../brakeType/brakeType.model";
import { BrakeTypeService } from             "../brakeType/brakeType.service";
import { IBrakeSystem } from                 "../brakeSystem/brakeSystem.model";
import { BrakeSystemService } from           "../brakeSystem/brakeSystem.service";
import { IBrakeABS } from                    "../brakeABS/brakeABS.model";
import { BrakeABSService } from              "../brakeABS/brakeABS.service";
import { HttpHelper } from                   "../httpHelper";
import { ModalComponent } from "ng2-bs3-modal/ng2-bs3-modal";
import { SharedService } from                "../shared/shared.service";
import { NavigationService } from            "../shared/navigation.service";
import { ToastsManager } from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ConstantsWarehouse } from '../constants-warehouse';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { AcFile } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader.model';
import { Observable }    from 'rxjs/Observable';

@Component({
    selector: "vehicleToBrakeConfig-add-component",
    templateUrl: "app/templates/vehicleToBrakeConfig/vehicleToBrakeConfig-add.component.html",
    providers: [MakeService, ModelService, YearService, BaseVehicleService, SubModelService, RegionService, HttpHelper]
})

export class VehicleToBrakeConfigAddComponent {
    commenttoadd: string;
    makes: IMake[];
    models: IModel[];
    years: IYear[];
    vehicleIdSearchText: string;
    subModels: ISubModel[];
    regions: IRegion[];
    vehicle: IVehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
    vehicles: IVehicle[] = [];
    brakeConfig: IBrakeConfig = { id: -1, frontBrakeTypeId: -1, rearBrakeTypeId: -1, brakeABSId: -1, brakeSystemId: -1 };
    brakeConfigs: IBrakeConfig[] = [];
    frontBrakeTypes: IBrakeType[];
    rearBrakeTypes: IBrakeType[];
    brakeABSes: IBrakeABS[];
    brakeSystems: IBrakeSystem[];
    brakeConfigIdSearchText: string;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;

    private acFiles: AcFile[] = [];

    proposedVehicleToBrakeConfigs: IVehicleToBrakeConfig[];
    existingVehicleToBrakeConfigs: IVehicleToBrakeConfig[];
    proposedVehicleToBrakeConfigsSelectionCount: number = 0;
    popupVehicle: IVehicle;
    popupBrakeConfig: IBrakeConfig;

    showLoadingGif: boolean = false;
    backNavigation: string;
    backNavigationText: string;
    selectAllChecked: boolean;

    @ViewChild('brakeAssociationsPopup')
    brakeAssociationsPopup: ModalComponent;
    @ViewChild('associationsPopup')
    associationsPopup: ModalComponent;

    constructor(private makeService: MakeService, private modelService: ModelService
        , private yearService: YearService, private baseVehicleService: BaseVehicleService, private subModelService: SubModelService
        , private regionService: RegionService, private vehicleToBrakeConfigService: VehicleToBrakeConfigService, private vehicleService: VehicleService
        , private brakeTypeService: BrakeTypeService, private brakeSystemService: BrakeSystemService, private brakeABSService: BrakeABSService
        , private brakeConfigService: BrakeConfigService, private router: Router, private sharedService: SharedService, private toastr: ToastsManager
        , private navgationService: NavigationService) {

        if (this.sharedService.vehicles) {
            this.vehicles = this.sharedService.vehicles;
        }

        if (this.sharedService.brakeConfigs) {
            this.brakeConfigs = this.sharedService.brakeConfigs;
        }

        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('vehicletobrakeconfig') > 0)
                this.backNavigationText = "Return to Brake System Search";
            else
                this.backNavigationText = "Return to Vehicle Search";
        }
    }

    ngOnInit() {
        this.showLoadingGif = true;
        this.years = [];
        this.makes = [];
        this.models = [];
        this.subModels = [];
        this.regions = [];

        this.rearBrakeTypes = [];
        this.brakeSystems = [];
        this.brakeABSes = [];

        this.yearService.getYears().subscribe(m => {
            this.years = m;
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });   //TODO: load all years that are attached to basevehicles

        this.brakeTypeService.getAllBrakeTypes().subscribe(m => this.frontBrakeTypes = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));    //TODO: load all frontbraketypes that are attached to brakeconfigs

        this.selectAllChecked = false;
    }

    onVehicleIdKeyPress(event) {
        if (event.keyCode == 13) {
            this.onVehicleIdSearch();
        }
    }

    onVehicleIdSearch() {
        let vehicleId = Number(this.vehicleIdSearchText);
        if (isNaN(vehicleId)) {
            this.toastr.warning("Invalid Vehicle ID", ConstantsWarehouse.validationTitle);
            return;
        }

        if (this.vehicle.id == vehicleId) {
            return;
        }

        this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        this.makes = null;
        this.models = null;
        this.subModels = null;
        this.regions = null;

        //TODO : may need to load makes related to the base ID

        this.showLoadingGif = true;
        this.vehicleService.getVehicle(vehicleId).subscribe(v => {

            this.makeService.getMakesByYearId(v.yearId).subscribe(mks => {
                this.makes = mks;

                this.baseVehicleService.getModelsByYearIdAndMakeId(v.yearId, v.makeId).subscribe(mdls => {
                    this.models = mdls;

                    this.subModelService.getSubModelsByBaseVehicleId(v.baseVehicleId).subscribe(subMdls => {
                        this.subModels = subMdls;

                        this.regionService.getRegionsByBaseVehicleIdAndSubModelId(v.baseVehicleId, v.subModelId).subscribe(r => {
                            this.regions = r;
                            this.vehicle = v;
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
        this.vehicle.id = -1;
        this.vehicle.makeId = -1;
        this.vehicle.baseVehicleId = -1;
        this.models = [];
        this.vehicle.subModelId = -1;
        this.subModels = [];
        this.vehicle.regionId = -1;
        this.regions = [];

        if (this.vehicle.yearId == -1) {
            this.makes = [];
            return;
        }

        this.makes = null;

        this.makeService.getMakesByYearId(this.vehicle.yearId).subscribe(m => this.makes = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }


    onSelectMake() {
        this.vehicle.id = -1;

        this.vehicle.baseVehicleId = -1;
        this.vehicle.subModelId = -1;
        this.subModels = [];
        this.vehicle.regionId = -1;
        this.regions = [];

        if (this.vehicle.makeId == -1) {
            this.models = [];
            return;
        }
        this.models = null;

        this.baseVehicleService.getModelsByYearIdAndMakeId(this.vehicle.yearId, this.vehicle.makeId).subscribe(m => this.models = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onSelectModel() {
        this.vehicle.id = -1;

        this.vehicle.subModelId = -1;
        this.vehicle.regionId = -1;
        this.regions = [];

        if (this.vehicle.baseVehicleId == -1) {
            this.subModels = [];
            return;
        }

        this.subModels = null;

        this.subModelService.getSubModelsByBaseVehicleId(this.vehicle.baseVehicleId).subscribe(m => this.subModels = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onSelectSubModel() {
        this.vehicle.id = -1;

        this.vehicle.regionId = -1;

        if (this.vehicle.subModelId == -1) {
            this.regions = [];
            return;
        }

        this.regions = null;

        this.regionService.getRegionsByBaseVehicleIdAndSubModelId(this.vehicle.baseVehicleId, this.vehicle.subModelId).subscribe(m => this.regions = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onSelectRegion() {
        this.vehicle.id = -1;

        if (this.vehicle.regionId == -1) {
            return;
        }

        this.showLoadingGif = true;
        this.vehicleService.getVehicleByBaseVehicleIdSubModelIdAndRegionId(this.vehicle.baseVehicleId, this.vehicle.subModelId, this.vehicle.regionId).subscribe(m => {
            this.vehicle = m;
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    onRemoveVehicle(vehicleId: number) {
        if (confirm("Remove Vehicle Id " + vehicleId + " from selection?")) {
            this.vehicles = this.vehicles.filter(item => item.id != vehicleId);
            this.refreshProposedVehicleToBrakeConfigs();
        }
    }

    onAddVehicleToSelection() {
        //TODO: do not allow addition if this item has an open CR
        if (this.vehicle.makeId == -1) {
            this.toastr.warning("Please select Make.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.vehicle.modelId == -1) {
            this.toastr.warning("Please select Model.", ConstantsWarehouse.validationTitle);
            return;
        }

        if (this.vehicle.baseVehicleId == -1) {
            this.toastr.warning("Please select Years.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.vehicle.subModelId == -1) {
            this.toastr.warning("Please select Sub-Model.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.vehicle.regionId == -1) {
            this.toastr.warning("Please select Region.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.vehicle.id == -1) {
            this.toastr.warning("Vehicle ID not available.", ConstantsWarehouse.validationTitle);
            return;
        }

        let filteredVehiclses = this.vehicles.filter(item => item.id == this.vehicle.id);
        if (filteredVehiclses && filteredVehiclses.length) {
            this.toastr.warning("Selected Vehicle already added", ConstantsWarehouse.validationTitle);
        }
        else {
            //NOTE: names are already available
            //this.vehicle.makeName = this.makes.filter(item => item.id == this.vehicle.makeId)[0].name;
            //this.vehicle.modelName = this.models.filter(item => item.id == this.vehicle.modelId)[0].name;
            //this.vehicle.subModelName = this.subModels.filter(item => item.id == this.vehicle.subModelId)[0].name;
            this.vehicles.push(this.vehicle);

            this.refreshProposedVehicleToBrakeConfigs();
            this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        }
    }

    onViewBrakeAssociations(vehicle) {
        this.popupVehicle = vehicle;
        this.brakeAssociationsPopup.open("lg");
        if (!this.popupVehicle.vehicleToBrakeConfigs) {
            this.vehicleToBrakeConfigService.getByVehicleId(this.popupVehicle.id).subscribe(m => {
                this.popupVehicle.vehicleToBrakeConfigs = m;
                this.popupVehicle.vehicleToBrakeConfigCount = m.length;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    onBrakeConfigIdKeyPress(event) {
        if (event.keyCode == 13) {
            this.onBrakeConfigIdSearch();
        }
    }

    onBrakeConfigIdSearch() {
        let brakeConfigId = Number(this.brakeConfigIdSearchText);
        if (isNaN(brakeConfigId)) {
            this.toastr.warning("Invalid Brake ID", ConstantsWarehouse.validationTitle);
            return;
        }

        if (this.brakeConfig.id == brakeConfigId) {
            return;
        }

        this.brakeConfig = { id: -1, frontBrakeTypeId: -1, rearBrakeTypeId: -1, brakeABSId: -1, brakeSystemId: -1 };
        var savedRearBrakeTypes = this.rearBrakeTypes;
        var savedBrakeABSes = this.brakeABSes;
        var savedBrakeSystems = this.brakeSystems;
        this.rearBrakeTypes = null;
        this.brakeABSes = null;
        this.brakeSystems = null;

        //TODO : may need to load front brake types related to the brake config ID

        this.showLoadingGif = true;

        this.brakeConfigService.getBrakeConfig(brakeConfigId).subscribe(bc => {
            this.brakeTypeService.getByFrontBrakeTypeId(bc.frontBrakeTypeId).subscribe(r => {
                this.rearBrakeTypes = r;

                this.brakeABSService.getByFrontBrakeTypeIdRearBrakeTypeId(bc.frontBrakeTypeId, bc.rearBrakeTypeId).subscribe(a => {
                    this.brakeABSes = a;

                    this.brakeSystemService.getByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId(bc.frontBrakeTypeId, bc.rearBrakeTypeId, bc.brakeABSId).subscribe(s => {
                        this.brakeSystems = s;
                        this.brakeConfig = bc;
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
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
        }, error => {//id not found for example
            let errorMessage: string = JSON.parse(String(error)).message;
            this.toastr.warning(errorMessage, ConstantsWarehouse.errorTitle);
            this.rearBrakeTypes = savedRearBrakeTypes;
            this.brakeABSes = savedBrakeABSes;
            this.brakeSystems = savedBrakeSystems;
            this.showLoadingGif = false;
        });
    }

    onSelectFrontBrakeType() {
        this.brakeConfig.id = -1;

        this.brakeConfig.rearBrakeTypeId = -1;
        this.brakeConfig.brakeABSId = -1;
        this.brakeABSes = [];
        this.brakeConfig.brakeSystemId = -1;
        this.brakeSystems = [];

        if (this.brakeConfig.frontBrakeTypeId == -1) {
            this.rearBrakeTypes = [];
            return;
        }

        this.rearBrakeTypes = null;

        this.brakeTypeService.getByFrontBrakeTypeId(this.brakeConfig.frontBrakeTypeId).subscribe(m => this.rearBrakeTypes = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onSelectRearBrakeType() {
        this.brakeConfig.id = -1;

        this.brakeConfig.brakeABSId = -1;
        this.brakeConfig.brakeSystemId = -1;
        this.brakeSystems = [];

        if (this.brakeConfig.rearBrakeTypeId == -1) {
            this.brakeABSes = [];
            return;
        }

        this.brakeABSes = null;

        this.brakeABSService.getByFrontBrakeTypeIdRearBrakeTypeId(this.brakeConfig.frontBrakeTypeId, this.brakeConfig.rearBrakeTypeId).subscribe(m => this.brakeABSes = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onSelectBrakeABS() {
        this.brakeConfig.id = -1;

        this.brakeConfig.brakeSystemId = -1;

        if (this.brakeConfig.brakeABSId == -1) {
            this.brakeSystems = [];
            return;
        }

        this.brakeSystems = null;

        this.brakeSystemService.getByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId(this.brakeConfig.frontBrakeTypeId, this.brakeConfig.rearBrakeTypeId, this.brakeConfig.brakeABSId).subscribe(m => this.brakeSystems = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onSelectBrakeSystem() {
        this.brakeConfig.id = -1;

        if (this.brakeConfig.brakeSystemId == -1) {
            return;
        }

        this.brakeConfigService.getByChildIds(this.brakeConfig.frontBrakeTypeId, this.brakeConfig.rearBrakeTypeId, this.brakeConfig.brakeABSId, this.brakeConfig.brakeSystemId).subscribe(m => this.brakeConfig = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onAddBrakeConfigToSelection() {
        //TODO: do not allow addition if this item has an open CR
        if (this.brakeConfig.frontBrakeTypeId == -1) {
            this.toastr.warning("Please select Front Brake Type.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.brakeConfig.rearBrakeTypeId == -1) {
            this.toastr.warning("Please select Rear Brake Type.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.brakeConfig.brakeABSId == -1) {
            this.toastr.warning("Please select Brake ABS.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.brakeConfig.brakeSystemId == -1) {
            this.toastr.warning("Please select Brake System.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.brakeConfig.id == -1) {
            this.toastr.warning("Brake ID not available.", ConstantsWarehouse.validationTitle);
            return;
        }

        let filteredBrakeConfigs = this.brakeConfigs.filter(item => item.id == this.brakeConfig.id);
        if (filteredBrakeConfigs && filteredBrakeConfigs.length) {
            this.toastr.warning("Selected Brake ID already added", ConstantsWarehouse.validationTitle);
        }
        else {
            this.brakeConfig.frontBrakeTypeName = this.frontBrakeTypes.filter(item => item.id == this.brakeConfig.frontBrakeTypeId)[0].name;
            this.brakeConfig.rearBrakeTypeName = this.rearBrakeTypes.filter(item => item.id == this.brakeConfig.rearBrakeTypeId)[0].name;
            this.brakeConfig.brakeABSName = this.brakeABSes.filter(item => item.id == this.brakeConfig.brakeABSId)[0].name;
            this.brakeConfig.brakeSystemName = this.brakeSystems.filter(item => item.id == this.brakeConfig.brakeSystemId)[0].name;
            this.brakeConfigs.push(this.brakeConfig);

            this.refreshProposedVehicleToBrakeConfigs();
            this.brakeConfig = { id: -1, frontBrakeTypeId: -1, rearBrakeTypeId: -1, brakeABSId: -1, brakeSystemId: -1 };
        }
        this.selectAllChecked = true;
    }

    onViewAssociations(brakeConfig) {
        this.popupBrakeConfig = brakeConfig;
        this.associationsPopup.open("lg");
        if (!this.popupBrakeConfig.vehicleToBrakeConfigs) {

            let inputModel = this.getDefaultInputModel();
            inputModel.brakeConfigId = this.popupBrakeConfig.id;

            this.vehicleToBrakeConfigService.getAssociations(inputModel).subscribe(m => {
                this.popupBrakeConfig.vehicleToBrakeConfigs = m;
                this.popupBrakeConfig.vehicleToBrakeConfigCount = m.length;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    getDefaultInputModel() {
        return {
            brakeConfigId: 0,
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
            frontBrakeTypes: [],
            rearBrakeTypes: [],
            brakeAbs: [],
            brakeSystems: [],
        };
    }

    onRemoveBrakeConfig(brakeConfigId: number) {
        if (confirm("Remove Brake Id " + brakeConfigId + " from selection?")) {
            this.brakeConfigs = this.brakeConfigs.filter(item => item.id != brakeConfigId);
            this.refreshProposedVehicleToBrakeConfigs();
        }
    }

    refreshProposedVehicleToBrakeConfigs() {
        if (this.vehicles.length == 0 || this.brakeConfigs.length == 0) {
            return;
        }

        if (this.showLoadingGif) {
            return;
        }
        this.showLoadingGif = true;

        let allProposedVehicleToBrakeConfigs: IVehicleToBrakeConfig[] = [];

        this.vehicles.forEach(v => {
            this.brakeConfigs.forEach(b => {
                allProposedVehicleToBrakeConfigs.push({
                    vehicle: {
                        id: v.id,
                        baseVehicleId: v.baseVehicleId,
                        makeId: null,
                        makeName: v.makeName,
                        modelId: null,
                        modelName: v.modelName,
                        yearId: v.yearId,
                        subModelId: null,
                        subModelName: v.subModelName,
                        regionId: null,
                        regionName: v.regionName,
                        sourceId: null,
                        sourceName: '',
                        publicationStageId: null,
                        publicationStageName: '',
                        publicationStageDate: null,
                        publicationStageSource: '',
                        publicationEnvironment: '',
                        vehicleToBrakeConfigCount: null,
                        isSelected: false
                    },
                    brakeConfig: {
                        id: b.id,
                        frontBrakeTypeId: b.frontBrakeTypeId,
                        frontBrakeTypeName: b.frontBrakeTypeName,
                        rearBrakeTypeId: b.rearBrakeTypeId,
                        rearBrakeTypeName: b.rearBrakeTypeName,
                        brakeSystemId: b.brakeSystemId,
                        brakeSystemName: b.brakeSystemName,
                        brakeABSId: b.brakeABSId,
                        brakeABSName: b.brakeABSName,
                        vehicleToBrakeConfigCount: 0
                    },
                    numberOfBrakesAssociation: -1,
                    isSelected: true
                });
            });
        });

        let selectedVehicleIds = this.vehicles.map(x => x.id);
        let selectedBrakeConfigIds = this.brakeConfigs.map(x => x.id);

        this.vehicleToBrakeConfigService.getVehicleToBrakeConfigsByVehicleIdsAndBrakeConfigIds(selectedVehicleIds, selectedBrakeConfigIds)
            .subscribe(m => {
                this.proposedVehicleToBrakeConfigs = [];
                this.existingVehicleToBrakeConfigs = m;
                if (this.existingVehicleToBrakeConfigs == null || this.existingVehicleToBrakeConfigs.length == 0) {
                    this.proposedVehicleToBrakeConfigs = allProposedVehicleToBrakeConfigs;
                }
                else {

                    let existingVehicleIds = this.existingVehicleToBrakeConfigs.map(x => x.vehicle.id);
                    let existingBrakeConfigIds = this.existingVehicleToBrakeConfigs.map(x => x.brakeConfig.id);
                    this.proposedVehicleToBrakeConfigs = allProposedVehicleToBrakeConfigs.filter(item => existingVehicleIds.indexOf(item.vehicle.id) < 0 || existingBrakeConfigIds.indexOf(item.brakeConfig.id) < 0);
                }

                if (this.proposedVehicleToBrakeConfigs != null) {
                    this.proposedVehicleToBrakeConfigsSelectionCount = this.proposedVehicleToBrakeConfigs.filter(item => item.isSelected).length;
                }

                this.showLoadingGif = false;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
    }

    onSelectAllProposedBrakeAssociations(event) {
        this.selectAllChecked = event;
        if (this.proposedVehicleToBrakeConfigs == null) {
            return;
        }
        this.proposedVehicleToBrakeConfigs.forEach(item => item.isSelected = event.target.checked);
        this.proposedVehicleToBrakeConfigsSelectionCount = this.proposedVehicleToBrakeConfigs.filter(item => item.isSelected).length;
    }

    refreshProposedVehicleToBrakeConfigsSelectionCount(event, vehicleTobrakeconfig: IVehicleToBrakeConfig) {
        if (event.target.checked) {
            this.proposedVehicleToBrakeConfigsSelectionCount++;
            var excludedVehicle = this.proposedVehicleToBrakeConfigs.filter(item => item.isSelected);
            if (excludedVehicle.length == this.proposedVehicleToBrakeConfigs.length - 1) {
                this.selectAllChecked = true;
            }

        } else {
            this.proposedVehicleToBrakeConfigsSelectionCount--;
            this.selectAllChecked = false;
        }
    }

    onSubmitAssociations() {
        if (!this.proposedVehicleToBrakeConfigs) {
            return;
        }
        let length: number = this.proposedVehicleToBrakeConfigs.length;
        if (this.proposedVehicleToBrakeConfigs.filter(item => item.isSelected).length == 0) {
            this.toastr.warning("No brake associations selected", ConstantsWarehouse.errorTitle);
            return;
        }
        this.submitAssociations(length);
    }

    private getNextVehicleToBrakeConfig(index): IVehicleToBrakeConfig {
        if (!this.proposedVehicleToBrakeConfigs || this.proposedVehicleToBrakeConfigs.length == 0) {
            return null;
        }
        let nextConfig = this.proposedVehicleToBrakeConfigs[index];
        return nextConfig;
    }

    private submitAssociations(length: number) {
        this.showLoadingGif = true;
        length = length - 1;
        if (length >= 0) {
            let proposedVehicleToBrakeConfig = <IVehicleToBrakeConfig>this.getNextVehicleToBrakeConfig(length);
            proposedVehicleToBrakeConfig.comment = this.commenttoadd;
            let vehicleToBrakeConfigIdentity: string = "(Vehicle ID: " + proposedVehicleToBrakeConfig.vehicle.id + ", Brake Config ID: " + proposedVehicleToBrakeConfig.brakeConfig.id + ")"
            this.acFiles = this.acFileUploader.getAcFiles();
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    proposedVehicleToBrakeConfig.attachments = this.sharedService.clone(uploadedFiles);
                }
                if (proposedVehicleToBrakeConfig.attachments) {
                    proposedVehicleToBrakeConfig.attachments = proposedVehicleToBrakeConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.vehicleToBrakeConfigService.addVehicleToBrakeConfig(proposedVehicleToBrakeConfig).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle to Brake Config", ConstantsWarehouse.changeRequestType.add, vehicleToBrakeConfigIdentity);
                        successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + vehicleToBrakeConfigIdentity + "\" Vehicle to Brake Config change requestid  \"" + response + "\" will be reviewed.";
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.acFileUploader.reset();
                        this.acFileUploader.setAcFiles(this.acFiles);
                        this.submitAssociations(length);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle to Brake Config", ConstantsWarehouse.changeRequestType.add, vehicleToBrakeConfigIdentity);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                        this.acFileUploader.reset();
                        this.acFileUploader.setAcFiles(this.acFiles);
                        this.submitAssociations(length);
                    }
                }, error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle to Brake Config", ConstantsWarehouse.changeRequestType.add, vehicleToBrakeConfigIdentity);
                    this.toastr.warning(error ? error : errorMessage.body, errorMessage.title);
                    this.acFileUploader.reset();
                    this.acFileUploader.setAcFiles(this.acFiles);
                    this.submitAssociations(length);
                });
            }, error => {
                this.showLoadingGif = false;
            });
        } else {
            this.acFileUploader.reset();
            this.showLoadingGif = false;
        }
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers();
    }
}