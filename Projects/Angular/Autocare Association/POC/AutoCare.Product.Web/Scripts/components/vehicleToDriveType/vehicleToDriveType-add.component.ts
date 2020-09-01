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
import { ISubModel }                            from "../subModel/subModel.model";
import { SubModelService }                      from "../subModel/subModel.service";
import { IVehicleToDriveType }                  from "../vehicleToDriveType/vehicleToDriveType.model";
import { VehicleToDriveTypeService }            from "../vehicleToDriveType/vehicleToDriveType.service";
import { IVehicle }                             from "../vehicle/vehicle.model";
import { VehicleService }                       from "../vehicle/vehicle.service";
import { IDriveType }                           from "../DriveType/DriveType.model";
import { DriveTypeService }                     from "../DriveType/DriveType.service";
import { HttpHelper }                           from "../httpHelper";
import { ModalComponent }     from "ng2-bs3-modal/ng2-bs3-modal";
import { SharedService }                        from "../shared/shared.service";
import { NavigationService }                    from "../shared/navigation.service";
import { ToastsManager }                        from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ConstantsWarehouse }                   from '../constants-warehouse';
import { AcFileUploader }                       from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { AcFile }                               from '../../lib/aclibs/ac-fileuploader/ac-fileuploader.model';
import { Observable }                           from 'rxjs/Observable';
import { IVehicleToDriveTypeSearchInputModel }  from "./vehicleToDriveType-search.model";


@Component({
    selector: "vehicleToDriveType-add-component",
    templateUrl: "app/templates/vehicleToDriveType/vehicleToDriveType-add.component.html",
    providers: [MakeService, ModelService, YearService, BaseVehicleService, SubModelService, RegionService, VehicleToDriveTypeService, DriveTypeService, HttpHelper]
})
export class VehicleToDriveTypeAddComponent {
    private commenttoadd: string;
    private makes: IMake[];
    private models: IModel[];
    private years: IYear[];
    private vehicleIdSearchText: string;
    private subModels: ISubModel[];
    private regions: IRegion[];
    private vehicle: IVehicle = { id: -1, baseVehicleId: -1, makeId: -1, yearId: -1, subModelId: -1, regionId: -1 };
    private vehicles: IVehicle[] = [];
    private driveType: IDriveType = { id: -1 };
    private driveTypes: IDriveType[] = [];
    private allDriveTypes: IDriveType[] = [];
    private selectedDriveType: IDriveType = { id: -1 }
    private driveTypeIdSearchText: string;
    private acFiles: AcFile[] = [];
    private proposedVehicleToDriveTypes: IVehicleToDriveType[];
    private existingVehicleToDriveTypes: IVehicleToDriveType[];
    private proposedVehicleToDriveTypesSelectionCount: number = 0;
    private popupVehicle: IVehicle;
    private popupDriveType: IDriveType;
    private showLoadingGif: boolean = false;
    private backNavigation: string;
    private backNavigationText: string;
    private selectAllChecked: boolean;

    @ViewChild(AcFileUploader) acFileUploader: AcFileUploader;
    @ViewChild('driveTypeAssociationsPopup') driveTypeAssociationsPopup: ModalComponent;
    @ViewChild('associationsPopup') associationsPopup: ModalComponent;

    constructor(private makeService: MakeService, private modelService: ModelService
        , private yearService: YearService, private baseVehicleService: BaseVehicleService, private subModelService: SubModelService
        , private regionService: RegionService, private vehicleToDriveTypeService: VehicleToDriveTypeService, private vehicleService: VehicleService
        , private driveTypeService: DriveTypeService, private router: Router, private sharedService: SharedService, private toastr: ToastsManager
        , private navgationService: NavigationService) {
        if (this.sharedService.vehicles) {
            this.vehicles = this.sharedService.vehicles;
        }
        if (this.sharedService.driveTypes) {
            this.driveTypes = this.sharedService.driveTypes;
        }
        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('vehicletodrivetype') > 0)
                this.backNavigationText = "Return to Drive Type System Search";
            else
                this.backNavigationText = "Return to Vehicle Search";
        }
    }

    private ngOnInit() {
        this.showLoadingGif = true;
        this.models = [];
        this.years = [];
        this.subModels = [];
        this.regions = [];
        this.yearService.getYears().subscribe(m => {
            this.years = m;
            this.driveTypeService.getDriveTypes().subscribe(d => {
                this.allDriveTypes = d;
                this.showLoadingGif = false;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });   //TODO: load all years that are attached to basevehicles
        this.selectAllChecked = false;
    }

    private onVehicleIdKeyPress(event) {
        if (event.keyCode == 13) {
            this.onVehicleIdSearch();
        }
    }

    private onVehicleIdSearch() {
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

    private onSelectYear() {
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

    private onSelectMake() {
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

    private onSelectModel() {
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

    private onSelectSubModel() {
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

    private onSelectRegion() {
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

    private onRemoveVehicle(vehicleId: number) {
        if (confirm("Remove Vehicle Id " + vehicleId + " from selection?")) {
            this.vehicles = this.vehicles.filter(item => item.id != vehicleId);
            this.refreshProposedVehicleToDriveTypes();
        }
    }

    private onAddVehicleToSelection() {
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
            this.refreshProposedVehicleToDriveTypes();
            this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        }
    }

    private onViewDriveTypeAssociations(vehicle) {
        this.popupVehicle = vehicle;
        this.driveTypeAssociationsPopup.open("lg");
        if (!this.popupVehicle.vehicleToDriveTypes) {
            this.vehicleToDriveTypeService.getByVehicleId(this.popupVehicle.id).subscribe(m => {
                this.popupVehicle.vehicleToDriveTypes = m;
                this.popupVehicle.vehicleToDriveTypeCount = m.length;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    private onDriveTypeIdKeyPress(event) {
        if (event.keyCode == 13) {
            this.onDriveTypeIdSearch();
        }
    }

    private onDriveTypeIdSearch() {
        let driveTypeId = Number(this.driveTypeIdSearchText);
        if (isNaN(driveTypeId)) {
            this.toastr.warning("Invalid Drive Type ID", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.driveType.id == driveTypeId) {
            return;
        }
        this.driveType = { id: -1 };

        this.showLoadingGif = true;
        this.driveTypeService.getDriveType(driveTypeId).subscribe(bc => {
            this.driveType = bc;
            this.showLoadingGif = false;
        }, error => {//id not found for example
            let errorMessage: string = JSON.parse(String(error)).message;
            this.toastr.warning(errorMessage, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    private onAddDriveTypeToSelection() {
        //TODO: do not allow addition if this item has an open CR
        if (this.driveType.id == -1) {
            this.toastr.warning("Drive Type ID not available.", ConstantsWarehouse.validationTitle);
            return;
        }

        let filteredDriveTypes = this.driveTypes.filter(item => item.id == this.driveType.id);
        if (filteredDriveTypes && filteredDriveTypes.length) {
            this.toastr.warning("Selected Drive Type ID already added", ConstantsWarehouse.validationTitle);
        }
        else {
            this.driveTypes.push(this.driveType);
            this.refreshProposedVehicleToDriveTypes();
            this.driveType = { id: -1 };
        }
        this.selectAllChecked = true;
    }

    private getDefaultInputModel(): IVehicleToDriveTypeSearchInputModel {
        return {
            driveTypeId: 0,
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
            driveTypes: []
        };
    }

    private onViewAssociations(driveType) {
        this.popupDriveType = driveType;
        this.associationsPopup.open("lg");
        if (!this.popupDriveType.vehicleToDriveTypes) {
            let inputModel = this.getDefaultInputModel();
            inputModel.driveTypeId = this.popupDriveType.id;
            this.vehicleToDriveTypeService.getAssociations(inputModel).subscribe(m => {
                this.popupDriveType.vehicleToDriveTypes = m;
                this.popupDriveType.vehicleToDriveTypeCount = m.length;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    private onRemoveDriveType(driveTypeId: number) {
        if (confirm("Remove Drive Type Id " + driveTypeId + " from selection?")) {
            this.driveTypes = this.driveTypes.filter(item => item.id != driveTypeId);
            this.refreshProposedVehicleToDriveTypes();
        }
    }

    private refreshProposedVehicleToDriveTypes() {
        if (this.vehicles.length == 0 || this.driveTypes.length == 0) {
            this.proposedVehicleToDriveTypes = [];
            return;
        }
        if (this.showLoadingGif) {
            return;
        }
        this.showLoadingGif = true;
        let allProposedVehicleToDriveTypes: IVehicleToDriveType[] = [];
        this.vehicles.forEach(v => {
            this.driveTypes.forEach(b => {
                allProposedVehicleToDriveTypes.push({
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
                        vehicleToDriveTypeCount: null,
                        isSelected: false
                    },
                    driveType: {
                        id: b.id,
                        name: b.name,
                        vehicleToDriveTypeCount: 0,
                    },
                    isSelected: true
                });
            });
        });

        let selectedVehicleIds = this.vehicles.map(x => x.id);
        let selectedDriveTypeIds = this.driveTypes.map(x => x.id);
        this.vehicleToDriveTypeService.getVehicleToDriveTypesByVehicleIdsAndDriveTypeIds(selectedVehicleIds, selectedDriveTypeIds)
            .subscribe(m => {
                this.proposedVehicleToDriveTypes = [];
                this.existingVehicleToDriveTypes = m;
                if (this.existingVehicleToDriveTypes == null || this.existingVehicleToDriveTypes.length == 0) {
                    this.proposedVehicleToDriveTypes = allProposedVehicleToDriveTypes;
                }
                else {

                    let existingVehicleIds = this.existingVehicleToDriveTypes.map(x => x.vehicle.id);
                    let existingDriveTypeIds = this.existingVehicleToDriveTypes.map(x => x.driveType.id);
                    this.proposedVehicleToDriveTypes = allProposedVehicleToDriveTypes.filter(item => existingVehicleIds.indexOf(item.vehicle.id) < 0 || existingDriveTypeIds.indexOf(item.driveType.id) < 0);
                }

                if (this.proposedVehicleToDriveTypes != null) {
                    this.proposedVehicleToDriveTypesSelectionCount = this.proposedVehicleToDriveTypes.filter(item => item.isSelected).length;
                }

                this.showLoadingGif = false;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
    }

    private onSelectAllProposedDriveTypeAssociations(event) {
        this.selectAllChecked = event;
        if (this.proposedVehicleToDriveTypes == null) {
            return;
        }
        this.proposedVehicleToDriveTypes.forEach(item => item.isSelected = event.target.checked);
        this.proposedVehicleToDriveTypesSelectionCount = this.proposedVehicleToDriveTypes.filter(item => item.isSelected).length;
    }

    private refreshProposedVehicleToDriveTypesSelectionCount(event, vehicleToDriveType: IVehicleToDriveType) {
        if (event.target.checked) {
            this.proposedVehicleToDriveTypesSelectionCount++;
            var excludedVehicle = this.proposedVehicleToDriveTypes.filter(item => item.isSelected);
            if (excludedVehicle.length == this.proposedVehicleToDriveTypes.length - 1) {
                this.selectAllChecked = true;
            }
        } else {
            this.proposedVehicleToDriveTypesSelectionCount--;
            this.selectAllChecked = false;
        }
    }

    private onSubmitAssociations() {
        if (!this.proposedVehicleToDriveTypes) {
            return;
        }
        let length: number = this.proposedVehicleToDriveTypes.length;
        if (this.proposedVehicleToDriveTypes.filter(item => item.isSelected).length == 0) {
            this.toastr.warning("No drive type associations selected", ConstantsWarehouse.errorTitle);
            return;
        }
        this.submitAssociations(length);
    }

    private getNextVehicleToDriveType(index): IVehicleToDriveType {
        if (!this.proposedVehicleToDriveTypes || this.proposedVehicleToDriveTypes.length == 0) {
            return null;
        }
        let nextConfig = this.proposedVehicleToDriveTypes[index];
        return nextConfig;
    }

    private submitAssociations(length: number) {
        this.showLoadingGif = true;
        length = length - 1;
        if (length >= 0) {
            let proposedVehicleToDriveType = <IVehicleToDriveType>this.getNextVehicleToDriveType(length);
            proposedVehicleToDriveType.comment = this.commenttoadd;
            let vehicleToDriveTypeIdentity: string = "(Vehicle ID: " + proposedVehicleToDriveType.vehicle.id + ", Drive Type ID: " + proposedVehicleToDriveType.driveType.id + ")"
            this.acFiles = this.acFileUploader.getAcFiles();
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    proposedVehicleToDriveType.attachments = this.sharedService.clone(uploadedFiles);
                }
                if (proposedVehicleToDriveType.attachments) {
                    proposedVehicleToDriveType.attachments = proposedVehicleToDriveType.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.vehicleToDriveTypeService.addVehicleToDriveType(proposedVehicleToDriveType).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle to Drive Type", ConstantsWarehouse.changeRequestType.add, vehicleToDriveTypeIdentity);
                        successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + vehicleToDriveTypeIdentity + "\" Vehicle to Drive Type change requestid  \"" + response + "\" will be reviewed.";
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.acFileUploader.reset();
                        this.acFileUploader.setAcFiles(this.acFiles);
                        this.submitAssociations(length);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle to Drive Type", ConstantsWarehouse.changeRequestType.add, vehicleToDriveTypeIdentity);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                        this.acFileUploader.reset();
                        this.acFileUploader.setAcFiles(this.acFiles);
                        this.submitAssociations(length);
                    }
                }, error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle to Drive Type", ConstantsWarehouse.changeRequestType.add, vehicleToDriveTypeIdentity);
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

    private cleanupComponent(): Observable<boolean> | boolean {
        return this.acFileUploader.cleanupAllTempContainers();
    }

    private onSelectDriveType() {
        //this.driveType.id = -1;
        //if (this.driveType.id == -1) {
        //    return;
        //}

        this.driveTypeService.getDriveType(this.driveType.id).subscribe(m => this.driveType = m,
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
    }
}