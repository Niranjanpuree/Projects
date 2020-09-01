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
import { IVehicleToWheelBase }                  from "../vehicleToWheelBase/vehicleToWheelBase.model";
import { VehicleToWheelBaseService }            from "../vehicleToWheelBase/vehicleToWheelBase.service";
import { IVehicle }                             from "../vehicle/vehicle.model";
import { VehicleService }                       from "../vehicle/vehicle.service";
import { IWheelBase }                           from "../WheelBase/WheelBase.model";
import { WheelBaseService }                     from "../WheelBase/WheelBase.service";
import { ModalComponent }     from "ng2-bs3-modal/ng2-bs3-modal";
import { SharedService }                        from "../shared/shared.service";
import { NavigationService }                    from "../shared/navigation.service";
import { ToastsManager }                        from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ConstantsWarehouse }                   from '../constants-warehouse';
import { AcFileUploader }                       from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { AcFile }                               from '../../lib/aclibs/ac-fileuploader/ac-fileuploader.model';
import { Observable }                           from 'rxjs/Observable';
import { IVehicleToWheelBaseSearchInputModel }  from "./vehicleToWheelBase-search.model";

@Component({
    selector: "vehicleToWheelBase-add-component",
    templateUrl: "app/templates/vehicleToWheelBase/vehicleToWheelBase-add.component.html",
    providers: [MakeService, ModelService, YearService, BaseVehicleService, SubModelService, RegionService, VehicleToWheelBaseService, WheelBaseService]
})

export class VehicleToWheelBaseAddComponent {
    private commenttoadd: string;
    private makes: IMake[];
    private models: IModel[];
    private years: IYear[];
    private vehicleIdSearchText: string;
    private subModels: ISubModel[];
    private regions: IRegion[];
    private vehicle: IVehicle = { id: -1, baseVehicleId: -1, makeId: -1, yearId: -1, subModelId: -1, regionId: -1 };
    private vehicles: IVehicle[] = [];
    private wheelBase: IWheelBase = { id: -1, base: "-1", wheelBaseMetric: "-1" };
    private wheelBases: IWheelBase[] = [];
    private allWheelBases: IWheelBase[] = [];
    private wheelBaseMetrics: IWheelBase[] = [];
    private wheelBaseIdSearchText: string;
    private acFiles: AcFile[] = [];
    private proposedVehicleToWheelBases: IVehicleToWheelBase[];
    private existingVehicleToWheelBases: IVehicleToWheelBase[];
    private proposedVehicleToWheelBasesSelectionCount: number = 0;
    private popupVehicle: IVehicle;
    private popupWheelBase: IWheelBase;
    private showLoadingGif: boolean = false;
    private backNavigation: string;
    private backNavigationText: string;
    private selectAllChecked: boolean;

    @ViewChild(AcFileUploader) acFileUploader: AcFileUploader;
    @ViewChild('wheelBaseAssociationsPopup') wheelBaseAssociationsPopup: ModalComponent;
    @ViewChild('associationsPopup') associationsPopup: ModalComponent;

    constructor(private makeService: MakeService, private modelService: ModelService
        , private yearService: YearService, private baseVehicleService: BaseVehicleService, private subModelService: SubModelService
        , private regionService: RegionService, private vehicleToWheelBaseService: VehicleToWheelBaseService, private vehicleService: VehicleService
        , private wheelBaseService: WheelBaseService, private router: Router, private sharedService: SharedService, private toastr: ToastsManager
        , private navgationService: NavigationService) {
        if (this.sharedService.vehicles) {
            this.vehicles = this.sharedService.vehicles;
        }
        if (this.sharedService.wheelBases) {
            this.wheelBases = this.sharedService.wheelBases;
        }
        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('vehicletowheelbase') > 0)
                this.backNavigationText = "Return to Wheel Base Search";
            else
                this.backNavigationText = "Return to Vehicle Search";
        }
    }

    private ngOnInit() {
        this.showLoadingGif = true;
        this.years = [];
        this.makes = [];
        this.models = [];
        this.subModels = [];
        this.regions = [];
        this.wheelBase = { id: -1 };

        this.yearService.getYears().subscribe(m => {
            this.years = m;
            this.wheelBaseService.getAllWheelBase().subscribe(m => {
                this.allWheelBases = m;
                this.showLoadingGif = false;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            });
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });

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
        this.subModelService.getSubModelsByBaseVehicleId(this.vehicle.baseVehicleId).subscribe(m => {
            this.subModels = m;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle)
        });
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
            this.refreshProposedVehicleToWheelBases();
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
            this.refreshProposedVehicleToWheelBases();
            this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        }
    }

    private onViewWheelBaseAssociations(vehicle) {
        this.popupVehicle = vehicle;
        this.wheelBaseAssociationsPopup.open("lg");
        if (!this.popupVehicle.vehicleToWheelBases) {
            this.vehicleToWheelBaseService.getByVehicleId(this.popupVehicle.id).subscribe(m => {
                this.popupVehicle.vehicleToWheelBases = m;
                this.popupVehicle.vehicleToWheelBaseCount = m.length;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    private onWheelBaseIdKeyPress(event) {
        if (event.keyCode == 13) {
            this.onWheelBaseIdSearch();
        }
    }

    private onWheelBaseIdSearch() {
        let wheelBaseId = Number(this.wheelBaseIdSearchText);
        if (isNaN(wheelBaseId)) {
            this.toastr.warning("Invalid Wheel Base ID", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.wheelBase.id == wheelBaseId) {
            return;
        }
        this.wheelBase = { id: -1 };
        //this.wheelBases = null;
        //this.wheelBaseMetrics = null;

        this.showLoadingGif = true;
        this.wheelBaseService.getWheelBaseDetail(wheelBaseId).subscribe(wb => {
            this.wheelBase = wb;
            this.showLoadingGif = false;
        }, error => {//id not found for example
            let errorMessage: string = JSON.parse(String(error)).message;
            this.toastr.warning(errorMessage, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    private onAddWheelBaseToSelection() {
        //TODO: do not allow addition if this item has an open CR
        if (this.wheelBase.id == -1) {
            this.toastr.warning("Wheel Base ID not available.", ConstantsWarehouse.validationTitle);
            return;
        }

        let filteredWheelBases = this.wheelBases.filter(item => item.id == this.wheelBase.id);
        if (filteredWheelBases && filteredWheelBases.length) {
            this.toastr.warning("Selected Wheel Base ID already added", ConstantsWarehouse.validationTitle);
        }
        else {
            this.wheelBase.base = this.allWheelBases.filter(item => item.id == this.wheelBase.id)[0].base;
            this.wheelBase.wheelBaseMetric = this.allWheelBases.filter(item => item.id == this.wheelBase.id)[0].wheelBaseMetric;
            this.wheelBase.vehicleToWheelBaseCount = this.allWheelBases.filter(item => item.id == this.wheelBase.id)[0].vehicleToWheelBaseCount;
            this.wheelBases.push(this.wheelBase);
            this.refreshProposedVehicleToWheelBases();
            this.wheelBase = { id: -1 };
        }
        this.selectAllChecked = true;
    }

    private getDefaultInputModel(): IVehicleToWheelBaseSearchInputModel {
        return {
            wheelBaseId: 0,
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: []
        };
    }

    private onViewAssociations(wheelBase) {
        this.popupWheelBase = wheelBase;
        this.associationsPopup.open("lg");
        if (!this.popupWheelBase.vehicleToWheelBases) {
            let inputModel = this.getDefaultInputModel();
            inputModel.wheelBaseId = this.popupWheelBase.id;
            this.vehicleToWheelBaseService.getAssociations(inputModel).subscribe(m => {
                this.popupWheelBase.vehicleToWheelBases = m;
                this.popupWheelBase.vehicleToWheelBaseCount = m.length;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    private onRemoveWheelBase(wheelBaseId: number) {
           if (confirm("Remove Wheel Base Id " + wheelBaseId + " from selection?")) {
            this.wheelBases = this.wheelBases.filter(item => item.id != wheelBaseId);
            this.refreshProposedVehicleToWheelBases();
        }
    }

    private refreshProposedVehicleToWheelBases() {
        if (this.vehicles.length == 0 || this.wheelBases.length == 0) {
            return;
        }
        if (this.showLoadingGif) {
            return;
        }
        this.showLoadingGif = true;
        let allProposedVehicleToWheelBases: IVehicleToWheelBase[] = [];
        this.vehicles.forEach(v => {
            this.wheelBases.forEach(b => {
                allProposedVehicleToWheelBases.push({
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
                        vehicleToWheelBaseCount: null,
                        isSelected: false
                    },
                    wheelBase: {
                        id: b.id,
                        base: b.base,
                        wheelBaseMetric: b.wheelBaseMetric,
                        vehicleToWheelBaseCount: 0,
                    },
                    isSelected: true
                });
            });
        });

        let selectedVehicleIds = this.vehicles.map(x => x.id);
        let selectedWheelBaseIds = this.wheelBases.map(x => x.id);
        this.vehicleToWheelBaseService.getVehicleToWheelBasesByVehicleIdsAndWheelBaseIds(selectedVehicleIds, selectedWheelBaseIds)
            .subscribe(m => {
                this.proposedVehicleToWheelBases = [];
                this.existingVehicleToWheelBases = m;
                if (this.existingVehicleToWheelBases == null || this.existingVehicleToWheelBases.length == 0) {
                    this.proposedVehicleToWheelBases = allProposedVehicleToWheelBases;
                }
                else {
                    let existingVehicleIds = this.existingVehicleToWheelBases.map(x => x.vehicle.id);
                    let existingWheelBaseIds = this.existingVehicleToWheelBases.map(x => x.wheelBase.id);
                    this.proposedVehicleToWheelBases = allProposedVehicleToWheelBases.filter(item => existingVehicleIds.indexOf(item.vehicle.id) < 0 || existingWheelBaseIds.indexOf(item.wheelBase.id) < 0);
                }

                if (this.proposedVehicleToWheelBases != null) {
                    this.proposedVehicleToWheelBasesSelectionCount = this.proposedVehicleToWheelBases.filter(item => item.isSelected).length;
                }

                this.showLoadingGif = false;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
    }

    private onSelectAllProposedWheelBaseAssociations(event) {
        this.selectAllChecked = event;
        if (this.proposedVehicleToWheelBases == null) {
            return;
        }
        this.proposedVehicleToWheelBases.forEach(item => item.isSelected = event.target.checked);
        this.proposedVehicleToWheelBasesSelectionCount = this.proposedVehicleToWheelBases.filter(item => item.isSelected).length;
    }

    private refreshProposedVehicleToWheelBasesSelectionCount(event, vehicleToWheelBase: IVehicleToWheelBase) {
        if (event.target.checked) {
            this.proposedVehicleToWheelBasesSelectionCount++;
            var excludedVehicle = this.proposedVehicleToWheelBases.filter(item => item.isSelected);
            if (excludedVehicle.length == this.proposedVehicleToWheelBases.length - 1) {
                this.selectAllChecked = true;
            }
        } else {
            this.proposedVehicleToWheelBasesSelectionCount--;
            this.selectAllChecked = false;
        }
    }

    private onSubmitAssociations() {
        if (!this.proposedVehicleToWheelBases) {
            return;
        }
        let length: number = this.proposedVehicleToWheelBases.length;
        if (this.proposedVehicleToWheelBases.filter(item => item.isSelected).length == 0) {
            this.toastr.warning("No wheel base associations selected", ConstantsWarehouse.errorTitle);
            return;
        }
        this.submitAssociations(length);
    }

    private getNextVehicleToWheelBase(index): IVehicleToWheelBase {
        if (!this.proposedVehicleToWheelBases || this.proposedVehicleToWheelBases.length == 0) {
            return null;
        }
        let nextConfig = this.proposedVehicleToWheelBases[index];
        return nextConfig;
    }

    private submitAssociations(length: number) {
        this.showLoadingGif = true;
        length = length - 1;
        if (length >= 0) {
            let proposedVehicleToWheelBase = <IVehicleToWheelBase>this.getNextVehicleToWheelBase(length);
            proposedVehicleToWheelBase.comment = this.commenttoadd;
            let vehicleToWheelBaseIdentity: string = "(Vehicle ID: " + proposedVehicleToWheelBase.vehicle.id + ", Wheel Base ID: " + proposedVehicleToWheelBase.wheelBase.id + ")"
            this.acFiles = this.acFileUploader.getAcFiles();
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    proposedVehicleToWheelBase.attachments = this.sharedService.clone(uploadedFiles);
                }
                if (proposedVehicleToWheelBase.attachments) {
                    proposedVehicleToWheelBase.attachments = proposedVehicleToWheelBase.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.vehicleToWheelBaseService.addVehicleToWheelBase(proposedVehicleToWheelBase).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle to Wheel Base", ConstantsWarehouse.changeRequestType.add, vehicleToWheelBaseIdentity);
                        successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + vehicleToWheelBaseIdentity + "\" Vehicle to Wheel Base change requestid  \"" + response + "\" will be reviewed.";
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.acFileUploader.reset();
                        this.acFileUploader.setAcFiles(this.acFiles);
                        this.submitAssociations(length);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle to Wheel Base", ConstantsWarehouse.changeRequestType.add, vehicleToWheelBaseIdentity);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                        this.acFileUploader.reset();
                        this.acFileUploader.setAcFiles(this.acFiles);
                        this.submitAssociations(length);
                    }
                }, error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle to Wheel Base", ConstantsWarehouse.changeRequestType.add, vehicleToWheelBaseIdentity);
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
}