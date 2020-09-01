import { Component, OnInit, ViewChild }         from "@angular/core";
import { Router }            from "@angular/router";
import { BaseVehicleService }                   from "../baseVehicle/baseVehicle.service";
import { IMake }                                from "../make/make.model";
import { MakeService }                          from "../make/make.service";
import { IModel }                               from "../model/model.model";
import { ModelService }                         from "../model/model.service";
import { IYear }                                from "../year/year.model";
import { YearService }                          from "../year/year.service";
import { IRegion }                              from "../region/region.model";
import { RegionService }                        from "../region/region.service";
import { ISubModel }                            from "../submodel/submodel.model";
import { SubModelService }                      from "../submodel/submodel.service";
import { IVehicleToBedConfig }                  from "./vehicleToBedConfig.model";
import { VehicleToBedConfigService }            from "./vehicleToBedConfig.service";
import { IVehicle }                             from "../vehicle/vehicle.model";
import { VehicleService }                       from "../vehicle/vehicle.service";
import { IBedConfig }                           from "../bedConfig/bedConfig.model";
import { BedConfigService }                     from "../bedConfig/bedConfig.service";
import { IBedType }                             from "../bedType/bedType.model";
import { BedTypeService }                       from "../bedType/bedType.service";
import { IBedLength }                           from "../bedLength/bedLength.model";
import { BedLengthService }                     from "../bedLength/bedlength.service";
import { ModalComponent }     from "ng2-bs3-modal/ng2-bs3-modal";
import { SharedService }                        from "../shared/shared.service";
import { NavigationService }                    from "../shared/navigation.service";
import { ToastsManager }                        from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ConstantsWarehouse }                   from '../constants-warehouse';
import { AcFileUploader }                       from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { AcFile }                               from '../../lib/aclibs/ac-fileuploader/ac-fileuploader.model';
import { Observable }                           from 'rxjs/Observable';

@Component({
    selector: "vehicleToBedConfig-add-component",
    templateUrl: "app/templates/vehicleToBedConfig/vehicleToBedConfig-add.component.html",
    providers: [MakeService, ModelService, YearService, BaseVehicleService, SubModelService, RegionService, BedLengthService, BedTypeService]
})

export class VehicleToBedConfigAddComponent {
    commenttoadd: string;
    makes: IMake[];
    models: IModel[];
    years: IYear[];
    vehicleIdSearchText: string;
    subModels: ISubModel[];
    regions: IRegion[];
    vehicle: IVehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
    vehicles: IVehicle[] = [];
    bedConfig: IBedConfig = { id: -1, bedLengthId: -1, bedTypeId: -1 };
    bedConfigs: IBedConfig[] = [];
    bedTypes: IBedType[];
    bedLengths: IBedLength[];
    bedConfigIdSearchText: string;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;

    private acFiles: AcFile[] = [];

    proposedVehicleToBedConfigs: IVehicleToBedConfig[];
    existingVehicleToBedConfigs: IVehicleToBedConfig[];
    proposedVehicleToBedConfigsSelectionCount: number = 0;
    popupVehicle: IVehicle;
    popupBedConfig: IBedConfig;

    showLoadingGif: boolean = false;
    backNavigation: string;
    backNavigationText: string;
    selectAllChecked: boolean;

    @ViewChild('bedAssociationsPopup')
    bedAssociationsPopup: ModalComponent;
    @ViewChild('associationsPopup')
    associationsPopup: ModalComponent;

    constructor(private makeService: MakeService, private modelService: ModelService
        , private yearService: YearService, private baseVehicleService: BaseVehicleService, private subModelService: SubModelService
        , private regionService: RegionService, private vehicleToBedConfigService: VehicleToBedConfigService, private vehicleService: VehicleService
        , private bedTypeService: BedTypeService, private bedLengthService: BedLengthService
        , private bedConfigService: BedConfigService, private router: Router, private sharedService: SharedService, private toastr: ToastsManager
        , private navgationService: NavigationService) {

        if (this.sharedService.vehicles) {
            this.vehicles = this.sharedService.vehicles;
        }

        if (this.sharedService.bedConfigs) {
            this.bedConfigs = this.sharedService.bedConfigs;
        }

        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('vehicletobedconfig') > 0)
                this.backNavigationText = "Return to Bed System Search";
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

        this.bedTypes = [];

        this.yearService.getYears().subscribe(m => {
            this.years = m;
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });   //TODO: load all years that are attached to basevehicles

        this.bedLengthService.getAllBedLengths().subscribe(m => this.bedLengths = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));

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
            this.refreshProposedVehicleToBedConfigs();
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

            this.refreshProposedVehicleToBedConfigs();
            this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        }
    }

    onViewBedAssociations(vehicle) {
        this.popupVehicle = vehicle;
        this.bedAssociationsPopup.open("lg");
        if (!this.popupVehicle.vehicleToBedConfigs) {
            this.vehicleToBedConfigService.getByVehicleId(this.popupVehicle.id).subscribe(m => {
                this.popupVehicle.vehicleToBedConfigs = m;
                this.popupVehicle.vehicleToBedConfigCount = m.length;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    onBedConfigIdKeyPress(event) {
        if (event.keyCode == 13) {
            this.onBedConfigIdSearch();
        }
    }

    onBedConfigIdSearch() {
        let bedConfigId = Number(this.bedConfigIdSearchText);
        if (isNaN(bedConfigId)) {
            this.toastr.warning("Invalid Bed Config ID", ConstantsWarehouse.validationTitle);
            return;
        }

        if (this.bedConfig.id == bedConfigId) {
            return;
        }

        this.bedConfig = { id: -1, bedTypeId: -1, bedLengthId: -1 };
        var savedBedTypes = this.bedTypes;
        var savedBedLengths = this.bedLengths;

        this.bedTypes = null;

        //TODO : may need to load front bed types related to the bed config ID

        this.showLoadingGif = true;

        this.bedConfigService.getBedConfig(bedConfigId).subscribe(bc => {
            this.bedTypeService.getByBedLengthId(bc.bedLengthId).subscribe(a => {
                this.bedTypes = a;
                this.bedConfig = bc;
                this.showLoadingGif = false;

            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
        }, error => {//id not found for example
            let errorMessage: string = JSON.parse(String(error)).message;
            this.toastr.warning(errorMessage, ConstantsWarehouse.errorTitle);
            this.bedLengths = savedBedLengths;
            this.bedTypes = savedBedTypes;
            this.showLoadingGif = false;
        });
    }
    onSelectBedLength() {
        this.bedConfig.id = -1;
        this.bedConfig.bedTypeId = -1;
        if (this.bedConfig.bedLengthId == -1) {
            this.bedTypes = [];
            return;
        }
        this.bedTypes = null;
        this.bedTypeService.getByBedLengthId(this.bedConfig.bedLengthId).subscribe(m => this.bedTypes = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onSelectBedType() {
        this.bedConfig.id = -1;
        if (this.bedConfig.bedTypeId == -1) {
            return;
        }

        this.bedConfigService.getByChildIds(this.bedConfig.bedLengthId, this.bedConfig.bedTypeId).subscribe(m => this.bedConfig = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onAddBedConfigToSelection() {
        //TODO: do not allow addition if this item has an open CR

        if (this.bedConfig.bedLengthId == -1) {
            this.toastr.warning("Please select Bed Length.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.bedConfig.bedTypeId == -1) {
            this.toastr.warning("Please select Bed Type.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.bedConfig.id == -1) {
            this.toastr.warning("Bed Config ID not available.", ConstantsWarehouse.validationTitle);
            return;
        }

        let filteredBedConfigs = this.bedConfigs.filter(item => item.id == this.bedConfig.id);
        if (filteredBedConfigs && filteredBedConfigs.length) {
            this.toastr.warning("Selected Bed Config ID already added", ConstantsWarehouse.validationTitle);
        }
        else {

            this.bedConfig.length = this.bedLengths.filter(item => item.id == this.bedConfig.bedLengthId)[0].length;
            this.bedConfig.bedTypeName = this.bedTypes.filter(item => item.id == this.bedConfig.bedTypeId)[0].name;
            this.bedConfigs.push(this.bedConfig);

            this.refreshProposedVehicleToBedConfigs();
            this.bedConfig = { id: -1, bedLengthId: -1, bedTypeId: -1 };
        }
        this.selectAllChecked = true;
    }

    onViewAssociations(bedConfig) {
        this.popupBedConfig = bedConfig;
        this.associationsPopup.open("lg");
        if (!this.popupBedConfig.vehicleToBedConfigs) {
            let inputModel = this.getDefaultInputModel();
            inputModel.bedConfigId = this.popupBedConfig.id;

            this.vehicleToBedConfigService.getAssociations(inputModel).subscribe(m => {
                this.popupBedConfig.vehicleToBedConfigs = m;
                this.popupBedConfig.vehicleToBedConfigCount = m.length;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    getDefaultInputModel() {
        return {
            bedConfigId: 0,
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
            bedTypes: [],
            bedLengths: []
        };
    }

    onRemoveBedConfig(bedConfigId: number) {
        if (confirm("Remove Bed Config Id " + bedConfigId + " from selection?")) {
            this.bedConfigs = this.bedConfigs.filter(item => item.id != bedConfigId);
            this.refreshProposedVehicleToBedConfigs();
        }
    }

    refreshProposedVehicleToBedConfigs() {
        if (this.vehicles.length == 0 || this.bedConfigs.length == 0) {
            return;
        }

        if (this.showLoadingGif) {
            return;
        }
        this.showLoadingGif = true;

        let allProposedVehicleToBedConfigs: IVehicleToBedConfig[] = [];

        this.vehicles.forEach(v => {
            this.bedConfigs.forEach(b => {
                allProposedVehicleToBedConfigs.push({
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
                        vehicleToBedConfigCount: null,
                        isSelected: false
                    },
                    bedConfig: {
                        id: b.id,
                        bedLengthId: b.bedLengthId,
                        length: b.length,
                        bedLengthMetric: b.bedLengthMetric,
                        bedTypeId: b.bedTypeId,
                        bedTypeName: b.bedTypeName,
                        vehicleToBedConfigCount: 0
                    },
                    numberOfBedAssociation: -1,
                    isSelected: true
                });
            });
        });

        let selectedVehicleIds = this.vehicles.map(x => x.id);
        let selectedBedConfigIds = this.bedConfigs.map(x => x.id);

        this.vehicleToBedConfigService.getVehicleToBedConfigsByVehicleIdsAndBedConfigIds(selectedVehicleIds, selectedBedConfigIds)
            .subscribe(m => {
                this.proposedVehicleToBedConfigs = [];
                this.existingVehicleToBedConfigs = m;
                if (this.existingVehicleToBedConfigs == null || this.existingVehicleToBedConfigs.length == 0) {
                    this.proposedVehicleToBedConfigs = allProposedVehicleToBedConfigs;
                }
                else {

                    let existingVehicleIds = this.existingVehicleToBedConfigs.map(x => x.vehicle.id);
                    let existingBedConfigIds = this.existingVehicleToBedConfigs.map(x => x.bedConfig.id);
                    this.proposedVehicleToBedConfigs = allProposedVehicleToBedConfigs.filter(item => existingVehicleIds.indexOf(item.vehicle.id) < 0 || existingBedConfigIds.indexOf(item.bedConfig.id) < 0);
                }

                if (this.proposedVehicleToBedConfigs != null) {
                    this.proposedVehicleToBedConfigsSelectionCount = this.proposedVehicleToBedConfigs.filter(item => item.isSelected).length;
                }

                this.showLoadingGif = false;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
    }

    onSelectAllProposedBedAssociations(event) {
        this.selectAllChecked = event;
        if (this.proposedVehicleToBedConfigs == null) {
            return;
        }
        this.proposedVehicleToBedConfigs.forEach(item => item.isSelected = event.target.checked);
        this.proposedVehicleToBedConfigsSelectionCount = this.proposedVehicleToBedConfigs.filter(item => item.isSelected).length;
    }

    refreshProposedVehicleToBedConfigsSelectionCount(event, vehicleTobedconfig: IVehicleToBedConfig) {
        if (event.target.checked) {
            this.proposedVehicleToBedConfigsSelectionCount++;
            var excludedVehicle = this.proposedVehicleToBedConfigs.filter(item => item.isSelected);
            if (excludedVehicle.length == this.proposedVehicleToBedConfigs.length - 1) {
                this.selectAllChecked = true;
            }

        } else {
            this.proposedVehicleToBedConfigsSelectionCount--;
            this.selectAllChecked = false;
        }
    }

    onSubmitAssociations() {
        if (!this.proposedVehicleToBedConfigs) {
            return;
        }
        let length: number = this.proposedVehicleToBedConfigs.length;
        if (this.proposedVehicleToBedConfigs.filter(item => item.isSelected).length == 0) {
            this.toastr.warning("No bed associations selected", ConstantsWarehouse.errorTitle);
            return;
        }
        this.submitAssociations(length);
    }

    private getNextVehicleToBedConfig(index): IVehicleToBedConfig {
        if (!this.proposedVehicleToBedConfigs || this.proposedVehicleToBedConfigs.length == 0) {
            return null;
        }
        let nextConfig = this.proposedVehicleToBedConfigs[index];
        return nextConfig;
    }

    private submitAssociations(length: number) {
        this.showLoadingGif = true;
        length = length - 1;
        if (length >= 0) {
            let proposedVehicleToBedConfig = <IVehicleToBedConfig>this.getNextVehicleToBedConfig(length);
            proposedVehicleToBedConfig.comment = this.commenttoadd;
            let vehicleToBedConfigIdentity: string = "(Vehicle ID: " + proposedVehicleToBedConfig.vehicle.id + ", Bed Config ID: " + proposedVehicleToBedConfig.bedConfig.id + ")"
            this.acFiles = this.acFileUploader.getAcFiles();
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    proposedVehicleToBedConfig.attachments = this.sharedService.clone(uploadedFiles);
                }
                if (proposedVehicleToBedConfig.attachments) {
                    proposedVehicleToBedConfig.attachments = proposedVehicleToBedConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.vehicleToBedConfigService.addVehicleToBedConfig(proposedVehicleToBedConfig).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle to Bed Config", ConstantsWarehouse.changeRequestType.add, vehicleToBedConfigIdentity);
                        successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + vehicleToBedConfigIdentity + "\" Vehicle to Bed Config change requestid  \"" + response + "\" will be reviewed.";
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.acFileUploader.reset();
                        this.acFileUploader.setAcFiles(this.acFiles);
                        this.submitAssociations(length);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle to Bed Config", ConstantsWarehouse.changeRequestType.add, vehicleToBedConfigIdentity);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                        this.acFileUploader.reset();
                        this.acFileUploader.setAcFiles(this.acFiles);
                        this.submitAssociations(length);
                    }
                }, error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle to Bed Config", ConstantsWarehouse.changeRequestType.add, vehicleToBedConfigIdentity);
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