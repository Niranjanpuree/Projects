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
import { IVehicleToBodyStyleConfig }            from "./vehicleToBodyStyleConfig.model";
import { VehicleToBodyStyleConfigService }      from "./vehicleToBodyStyleConfig.service";
import { IVehicle }                             from "../vehicle/vehicle.model";
import { VehicleService }                       from "../vehicle/vehicle.service";
import { IBodyStyleConfig }                     from "../bodyStyleConfig/bodyStyleConfig.model";
import { BodyStyleConfigService }               from "../bodyStyleConfig/bodyStyleConfig.service";
import { IBodyType }                            from "../bodyType/bodyType.model";
import { IBodyNumDoors}                         from "../bodyNumDoors/bodyNumDoors.model";
import { BodyNumDoorsService }                  from "../bodyNumDoors/bodyNumDoors.service";
import { BodyTypeService }                      from "../bodyType/bodyType.service";
import { ModalComponent }     from "ng2-bs3-modal/ng2-bs3-modal";
import { SharedService }                        from "../shared/shared.service";
import { NavigationService }                    from "../shared/navigation.service";
import { ToastsManager }                        from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ConstantsWarehouse }                   from '../constants-warehouse';
import { AcFileUploader }                       from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { AcFile }                               from '../../lib/aclibs/ac-fileuploader/ac-fileuploader.model';
import { Observable }                           from 'rxjs/Observable';

@Component({
    selector: "vehicleToBodyStyleConfig-add-component",
    templateUrl: "app/templates/vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-add.component.html",
    providers: [MakeService, BodyNumDoorsService, ModelService, YearService, BaseVehicleService, SubModelService, RegionService, BodyTypeService]
})

export class VehicleToBodyStyleConfigAddComponent {
    commenttoadd: string;
    makes: IMake[];
    models: IModel[];
    years: IYear[];
    vehicleIdSearchText: string;
    subModels: ISubModel[];
    regions: IRegion[];
    vehicle: IVehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
    vehicles: IVehicle[] = [];
    bodyStyleConfig: IBodyStyleConfig = { id: -1, bodyNumDoorsId: -1, bodyTypeId: -1 };
    bodyStyleConfigs: IBodyStyleConfig[] = [];
    bodyTypes: IBodyType[];
    bodyNumDoors: IBodyNumDoors[];
    bodyStyleConfigIdSearchText: string;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;

    private acFiles: AcFile[] = [];

    proposedVehicleToBodyStyleConfigs: IVehicleToBodyStyleConfig[];
    existingVehicleToBodyStyleConfigs: IVehicleToBodyStyleConfig[];
    proposedVehicleToBodyStyleConfigsSelectionCount: number = 0;
    popupVehicle: IVehicle;
    popupBodyStyleConfig: IBodyStyleConfig;

    showLoadingGif: boolean = false;
    backNavigation: string;
    backNavigationText: string;
    selectAllChecked: boolean;

    @ViewChild('bodyAssociationsPopup')
    bodyAssociationsPopup: ModalComponent;
    @ViewChild('associationsPopup')
    associationsPopup: ModalComponent;

    constructor(private makeService: MakeService, private modelService: ModelService
        , private yearService: YearService, private baseVehicleService: BaseVehicleService, private subModelService: SubModelService
        , private regionService: RegionService, private vehicleToBodyStyleConfigService: VehicleToBodyStyleConfigService, private vehicleService: VehicleService
        , private bodyTypeService: BodyTypeService
        , private bodyStyleConfigService: BodyStyleConfigService, private bodyNumDoorsService: BodyNumDoorsService, private router: Router, private sharedService: SharedService, private toastr: ToastsManager
        , private navgationService: NavigationService) {

        if (this.sharedService.vehicles) {
            this.vehicles = this.sharedService.vehicles;
        }

        if (this.sharedService.bodyStyleConfigs) {
            this.bodyStyleConfigs = this.sharedService.bodyStyleConfigs;
        }
        this.backNavigation = this.navgationService.backRoute;
        this.backNavigationText = "Return To DashBoard";
        if (this.navgationService.backRoute) {
            var navigate = this.sharedService.systemMenubarSelected;
            if (this.backNavigation == '/vehicle/search') {
                this.backNavigationText = "Return To Vehicle Search";
            }
            if (navigate == 'Body') {
                this.backNavigationText = "Return To Body System Search";
            }
        }
    }

    ngOnInit() {
        this.showLoadingGif = true;
        this.years = [];
        this.makes = [];
        this.models = [];
        this.subModels = [];
        this.regions = [];

        this.bodyTypes = [];

        this.yearService.getYears().subscribe(m => {
            this.years = m;
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });   //TODO: load all years that are attached to basevehicles

        this.bodyNumDoorsService.getAllBodyNumDoors().subscribe(m => this.bodyNumDoors = m,
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
            this.refreshProposedVehicleToBodyStyleConfigs();
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
            this.vehicles.push(this.vehicle);

            this.refreshProposedVehicleToBodyStyleConfigs();
            this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        }
    }

    onViewBodyAssociations(vehicle) {
        this.popupVehicle = vehicle;
        this.bodyAssociationsPopup.open("lg");
        if (!this.popupVehicle.vehicleToBodyStyleConfigs) {
            this.vehicleToBodyStyleConfigService.getByVehicleId(this.popupVehicle.id).subscribe(m => {
                this.popupVehicle.vehicleToBodyStyleConfigs = m;
                this.popupVehicle.vehicleToBodyStyleConfigCount = m.length;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    onbodyStyleConfigIdKeyPress(event) {
        if (event.keyCode == 13) {
            this.onBodyStyleConfigIdSearch();
        }
    }

    onBodyStyleConfigIdSearch() {
        let bodyStyleConfigId = Number(this.bodyStyleConfigIdSearchText);
        if (isNaN(bodyStyleConfigId)) {
            this.toastr.warning("Invalid Body Style ID", ConstantsWarehouse.validationTitle);
            return;
        }

        if (this.bodyStyleConfig.id == bodyStyleConfigId) {
            return;
        }

        this.bodyStyleConfig = { id: -1, bodyNumDoorsId: -1, bodyTypeId: -1 };
        var savedBodyTypes = this.bodyTypes;
        var savedBodyNumDoors = this.bodyNumDoors;

        this.bodyTypes = null;
        this.showLoadingGif = true;

        this.bodyStyleConfigService.getBodyStyleConfig(bodyStyleConfigId).subscribe(bc => {
            this.bodyTypeService.getByBodyNumDoorsId(bc.bodyNumDoorsId).subscribe(r => {
                this.bodyTypes = r;
                this.bodyStyleConfig = bc;
                this.showLoadingGif = false;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
        }, error => {//id not found for example
            let errorMessage: string = JSON.parse(String(error)).message;
            this.toastr.warning(errorMessage, ConstantsWarehouse.errorTitle);
            this.bodyTypes = savedBodyTypes;
            this.bodyNumDoors = savedBodyNumDoors;
            this.showLoadingGif = false;
        });
    }
    onSelectBodyNumberDoors() {
        this.bodyStyleConfig.id = -1;
        this.bodyStyleConfig.bodyTypeId = -1;
        this.bodyTypes = [];
        if (this.bodyStyleConfig.bodyNumDoorsId == -1) {
            this.bodyTypes = [];
            return;
        }
        this.bodyTypes = null;
        this.bodyTypeService.getByBodyNumDoorsId(this.bodyStyleConfig.bodyNumDoorsId).subscribe(m => this.bodyTypes = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }

    onSelectBodyType() {
        this.bodyStyleConfig.id = -1;
        if (this.bodyStyleConfig.bodyTypeId == -1) {
            return;
        }
        this.bodyStyleConfigService.getByChildIds(this.bodyStyleConfig.bodyNumDoorsId, this.bodyStyleConfig.bodyTypeId).subscribe(m => this.bodyStyleConfig = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }





    onAddBodyStyleConfigToSelection() {
        //TODO: do not allow addition if this item has an open CR

        if (this.bodyStyleConfig.bodyNumDoorsId == -1) {
            this.toastr.warning("Please select BodyNum Doors.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.bodyStyleConfig.bodyTypeId == -1) {
            this.toastr.warning("Please select Body Type.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.bodyStyleConfig.id == -1) {
            this.toastr.warning("Body Style Config ID not available.", ConstantsWarehouse.validationTitle);
            return;
        }

        let filteredBodyStyleConfigs = this.bodyStyleConfigs.filter(item => item.id == this.bodyStyleConfig.id);
        if (filteredBodyStyleConfigs && filteredBodyStyleConfigs.length) {
            this.toastr.warning("Selected Body Style Config ID already added", ConstantsWarehouse.validationTitle);
        }
        else {

            this.bodyStyleConfig.numDoors = this.bodyNumDoors.filter(item => item.id == this.bodyStyleConfig.bodyNumDoorsId)[0].numDoors;
            this.bodyStyleConfig.bodyTypeName = this.bodyTypes.filter(item => item.id == this.bodyStyleConfig.bodyTypeId)[0].name;
            this.bodyStyleConfigs.push(this.bodyStyleConfig);
            this.refreshProposedVehicleToBodyStyleConfigs();
            this.bodyStyleConfig = { id: -1, bodyNumDoorsId: -1, bodyTypeId: -1 };
        }
        this.selectAllChecked = true;
    }

    onViewAssociations(bodyStyleConfig) {
        this.popupBodyStyleConfig = bodyStyleConfig;
        this.associationsPopup.open("lg");
        if (!this.popupBodyStyleConfig.vehicleToBodyStyleConfigs) {
            let inputModel = this.getDefaultInputModel();
            inputModel.bodyStyleConfigId = this.popupBodyStyleConfig.id;

            this.vehicleToBodyStyleConfigService.getAssociations(inputModel).subscribe(m => {
                this.popupBodyStyleConfig.vehicleToBodyStyleConfigs = m;
                this.popupBodyStyleConfig.vehicleToBodyStyleConfigCount = m.length;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    getDefaultInputModel() {
        return {
            bodyStyleConfigId: 0,
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
            bodyNumDoors: [],
            bodyTypes: []
        };
    }

    onRemoveBodyStyleConfig(bodyStyleConfigId: number) {
        if (confirm("Remove Body Style Config Id " + bodyStyleConfigId + " from selection?")) {
            this.bodyStyleConfigs = this.bodyStyleConfigs.filter(item => item.id != bodyStyleConfigId);
            this.refreshProposedVehicleToBodyStyleConfigs();
        }
    }

    refreshProposedVehicleToBodyStyleConfigs() {
        if (this.vehicles.length == 0 || this.bodyStyleConfigs.length == 0) {
            return;
        }

        if (this.showLoadingGif) {
            return;
        }
        this.showLoadingGif = true;

        let allProposedVehicleToBodyStyleConfigs: IVehicleToBodyStyleConfig[] = [];

        this.vehicles.forEach(v => {
            this.bodyStyleConfigs.forEach(b => {
                allProposedVehicleToBodyStyleConfigs.push({
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
                        vehicleToBodyStyleConfigCount: null,
                        isSelected: false
                    },
                    bodyStyleConfig: {
                        id: b.id,
                        bodyNumDoorsId: b.bodyNumDoorsId,
                        bodyTypeId: b.bodyTypeId,
                        bodyTypeName: b.bodyTypeName,
                        vehicleToBodyStyleConfigCount: b.vehicleToBodyStyleConfigCount,
                        numDoors: b.numDoors
                    },
                    numberOfBodyAssociation: -1,
                    isSelected: true
                });
            });
        });

        let selectedVehicleIds = this.vehicles.map(x => x.id);
        let selectedBodyStyleConfigStyleIds = this.bodyStyleConfigs.map(x => x.id);

        this.vehicleToBodyStyleConfigService.getVehicleToBodyStyleConfigsByVehicleIdsAndBodyStyleConfigIds(selectedVehicleIds, selectedBodyStyleConfigStyleIds)
            .subscribe(m => {
                this.proposedVehicleToBodyStyleConfigs = [];
                this.existingVehicleToBodyStyleConfigs = m;
                if (this.existingVehicleToBodyStyleConfigs == null || this.existingVehicleToBodyStyleConfigs.length == 0) {
                    this.proposedVehicleToBodyStyleConfigs = allProposedVehicleToBodyStyleConfigs;
                }
                else {

                    let existingVehicleIds = this.existingVehicleToBodyStyleConfigs.map(x => x.vehicle.id);
                    let existingBodyStyleConfigIds = this.existingVehicleToBodyStyleConfigs.map(x => x.bodyStyleConfig.id);
                    this.proposedVehicleToBodyStyleConfigs = allProposedVehicleToBodyStyleConfigs.filter(item => existingVehicleIds.indexOf(item.vehicle.id) < 0 || existingBodyStyleConfigIds.indexOf(item.bodyStyleConfig.id) < 0);
                }

                if (this.proposedVehicleToBodyStyleConfigs != null) {
                    this.proposedVehicleToBodyStyleConfigsSelectionCount = this.proposedVehicleToBodyStyleConfigs.filter(item => item.isSelected).length;
                }

                this.showLoadingGif = false;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
    }

    onSelectAllProposedBodyAssociations(event) {
        this.selectAllChecked = event;
        if (this.proposedVehicleToBodyStyleConfigs == null) {
            return;
        }
        this.proposedVehicleToBodyStyleConfigs.forEach(item => item.isSelected = event.target.checked);
        this.proposedVehicleToBodyStyleConfigsSelectionCount = this.proposedVehicleToBodyStyleConfigs.filter(item => item.isSelected).length;
    }

    refreshProposedVehicleToBodyStyleConfigsSelectionCount(event, vehicleToBodyStyleconfig: IVehicleToBodyStyleConfig) {
        if (event.target.checked) {
            this.proposedVehicleToBodyStyleConfigsSelectionCount++;
            var excludedVehicle = this.proposedVehicleToBodyStyleConfigs.filter(item => item.isSelected);
            if (excludedVehicle.length == this.proposedVehicleToBodyStyleConfigs.length - 1) {
                this.selectAllChecked = true;
            }

        } else {
            this.proposedVehicleToBodyStyleConfigsSelectionCount--;
            this.selectAllChecked = false;
        }
    }

    onSubmitAssociations() {
        if (!this.proposedVehicleToBodyStyleConfigs) {
            return;
        }
        let length: number = this.proposedVehicleToBodyStyleConfigs.length;
        if (this.proposedVehicleToBodyStyleConfigs.filter(item => item.isSelected).length == 0) {
            this.toastr.warning("No Body Associations Selected", ConstantsWarehouse.errorTitle);
            return;
        }
        this.submitAssociations(length);
    }

    private getNextVehicleToBodyStyleConfig(index): IVehicleToBodyStyleConfig {
        if (!this.proposedVehicleToBodyStyleConfigs || this.proposedVehicleToBodyStyleConfigs.length == 0) {
            return null;
        }
        let nextConfig = this.proposedVehicleToBodyStyleConfigs[index];
        return nextConfig;
    }

    private submitAssociations(length: number) {
        this.showLoadingGif = true;
        length = length - 1;
        if (length >= 0) {
            let proposedVehicleToBodyStyleConfig = <IVehicleToBodyStyleConfig>this.getNextVehicleToBodyStyleConfig(length);
            proposedVehicleToBodyStyleConfig.comment = this.commenttoadd;
            let vehicleToBodyStyleConfigIdentity: string = "(Vehicle ID: " + proposedVehicleToBodyStyleConfig.vehicle.id + ", Body Style Config ID: " + proposedVehicleToBodyStyleConfig.bodyStyleConfig.id + ")"
            this.acFiles = this.acFileUploader.getAcFiles();
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    proposedVehicleToBodyStyleConfig.attachments = this.sharedService.clone(uploadedFiles);
                }
                if (proposedVehicleToBodyStyleConfig.attachments) {
                    proposedVehicleToBodyStyleConfig.attachments = proposedVehicleToBodyStyleConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.vehicleToBodyStyleConfigService.addVehicleToBodyStyleConfig(proposedVehicleToBodyStyleConfig).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle to Body Style Config", ConstantsWarehouse.changeRequestType.add, proposedVehicleToBodyStyleConfig);
                        successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + vehicleToBodyStyleConfigIdentity + "\" Vehicle to Body Config change requestid  \"" + response + "\" will be reviewed.";
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.acFileUploader.reset();
                        this.acFileUploader.setAcFiles(this.acFiles);
                        this.submitAssociations(length);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle to Body Style Config", ConstantsWarehouse.changeRequestType.add, vehicleToBodyStyleConfigIdentity);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                        this.acFileUploader.reset();
                        this.acFileUploader.setAcFiles(this.acFiles);
                        this.submitAssociations(length);
                    }
                }, error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle to Body Style Config", ConstantsWarehouse.changeRequestType.add, vehicleToBodyStyleConfigIdentity);
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