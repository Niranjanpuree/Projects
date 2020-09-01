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
import { IVehicleToMfrBodyCode } from        "./vehicleToMfrBodyCode.model";
import { VehicleToMfrBodyCodeService } from  "./vehicleToMfrBodyCode.service";
import { IVehicle } from                     "../vehicle/vehicle.model";
import { VehicleService } from               "../vehicle/vehicle.service";
import { IMfrBodyCode } from                 "../mfrBodyCode/mfrBodyCode.model";
import { MfrBodyCodeService } from           "../mfrBodyCode/mfrBodyCode.service";
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
    selector: "vehicleToMfrBodyCode-add-component",
    templateUrl: "app/templates/vehicleToMfrBodyCode/vehicleToMfrBodyCode-add.component.html",
    providers: [MakeService, ModelService, YearService, BaseVehicleService, SubModelService, RegionService, HttpHelper]
})
export class VehicleToMfrBodyCodeAddComponent {
    commenttoadd: string;
    makes: IMake[];
    models: IModel[];
    years: IYear[];
    vehicleIdSearchText: string;
    subModels: ISubModel[];
    regions: IRegion[];
    vehicle: IVehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
    vehicles: IVehicle[] = [];
    mfrBodyCode: IMfrBodyCode = { id: -1 };
    mfrBodyCodes: IMfrBodyCode[] = [];
    private mfrBodyCodesForSelection: IMfrBodyCode[] = [];
    mfrBodyCodeIdSearchText: string;
    @ViewChild(AcFileUploader)
    acFileUploader: AcFileUploader;

    private acFiles: AcFile[] = [];

    proposedVehicleToMfrBodyCodes: IVehicleToMfrBodyCode[];
    existingVehicleToMfrBodyCodes: IVehicleToMfrBodyCode[];
    proposedVehicleToMfrBodyCodesSelectionCount: number = 0;
    popupVehicle: IVehicle;
    popupMfrBodyCode: IMfrBodyCode;

    showLoadingGif: boolean = false;
    backNavigation: string;
    backNavigationText: string;
    selectAllChecked: boolean;

    @ViewChild('mfrBodyCodeAssociationsPopup')
    mfrBodyCodeAssociationsPopup: ModalComponent;
    @ViewChild('associationsPopup')
    associationsPopup: ModalComponent;

    constructor(private makeService: MakeService, private modelService: ModelService
        , private yearService: YearService, private baseVehicleService: BaseVehicleService, private subModelService: SubModelService
        , private regionService: RegionService, private vehicleToMfrBodyCodeService: VehicleToMfrBodyCodeService, private vehicleService: VehicleService
        , private mfrBodyCodeService: MfrBodyCodeService, private router: Router, private sharedService: SharedService, private toastr: ToastsManager
        , private navgationService: NavigationService) {

        if (this.sharedService.vehicles) {
            this.vehicles = this.sharedService.vehicles;
        }

        if (this.sharedService.mfrBodyCodes) {
            this.mfrBodyCodes = this.sharedService.mfrBodyCodes;
        }

        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('vehicle') > 0)
                this.backNavigationText = "Return to Mfr Body Code Search";
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

        this.mfrBodyCodes = [];

        this.yearService.getYears().subscribe(m => {
            this.years = m;
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });   //TODO: load all years that are attached to basevehicles

        this.mfrBodyCodeService.getMfrBodyCodes().subscribe(m => this.mfrBodyCodesForSelection = m,
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
            this.refreshProposedVehicleToMfrBodyCodes();
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

            this.refreshProposedVehicleToMfrBodyCodes();
            this.vehicle = { id: -1, baseVehicleId: -1, makeId: -1, modelId: -1, yearId: -1, subModelId: -1, regionId: -1 };
        }
    }

    onViewMfrBodyCodeAssociation(vehicle) {
        this.popupVehicle = vehicle;
        this.mfrBodyCodeAssociationsPopup.open("lg");
        if (!this.popupVehicle.vehicleToMfrBodyCodes) {
            this.vehicleToMfrBodyCodeService.getByVehicleId(this.popupVehicle.id).subscribe(m => {
                this.popupVehicle.vehicleToMfrBodyCodes = m;
                this.popupVehicle.vehicleToMfrBodyCodeCount = m.length;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    onMfrBodyCodeIdKeyPress(event) {
        if (event.keyCode == 13) {
            this.onMfrBodyCodeIdSearch();
        }
    }

    onMfrBodyCodeIdSearch() {
        let mfrBodyCodeId = Number(this.mfrBodyCodeIdSearchText);
        if (isNaN(mfrBodyCodeId)) {
            this.toastr.warning("Invalid Mfr Body Code ID", ConstantsWarehouse.validationTitle);
            return;
        }

        if (this.mfrBodyCode.id == mfrBodyCodeId) {
            return;
        }

        this.mfrBodyCode = { id: -1 };
        var savedMfrBodyCodes = this.mfrBodyCodes;

        this.showLoadingGif = true;

        this.mfrBodyCodeService.getMfrBodyCode(mfrBodyCodeId).subscribe(mbc => {

            this.mfrBodyCode = mbc;
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        }); error => {//id not found for example
            let errorMessage: string = JSON.parse(String(error)).message;
            this.toastr.warning(errorMessage, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        };
    }

    onAddMfrBodyCodeToSelection() {
        //TODO: do not allow addition if this item has an open CR
        if (this.mfrBodyCode.id == -1) {
            this.toastr.warning("Please select Mfr Body Code.", ConstantsWarehouse.validationTitle);
            return;
        }
        if (this.mfrBodyCode.id == -1) {
            this.toastr.warning("Please select Mfr Body Code.", ConstantsWarehouse.validationTitle);
            return;
        }
        let filteredMfrBodyCodes = this.mfrBodyCodes.filter(item => item.id == this.mfrBodyCode.id);
        if (filteredMfrBodyCodes && filteredMfrBodyCodes.length) {
            this.toastr.warning("Selected Mfr Body Code ID already added", ConstantsWarehouse.validationTitle);
        }
        else {
            this.mfrBodyCode.name = this.mfrBodyCodesForSelection.filter(item => item.id == this.mfrBodyCode.id)[0].name;
            this.mfrBodyCodes.push(this.mfrBodyCode);
            this.refreshProposedVehicleToMfrBodyCodes();
            this.mfrBodyCode = { id: -1 };
        }
        this.selectAllChecked = true;
    }

    onViewAssociations(mfrBodyCode) {
        this.popupMfrBodyCode = mfrBodyCode;
        this.associationsPopup.open("lg");
        if (!this.popupMfrBodyCode.vehicleToMfrBodyCodes) {

            let inputModel = this.getDefaultInputModel();
            inputModel.mfrBodyCodeId = this.popupMfrBodyCode.id;

            this.vehicleToMfrBodyCodeService.getAssociations(inputModel).subscribe(m => {
                this.popupMfrBodyCode.vehicleToMfrBodyCodes = m;
                this.popupMfrBodyCode.vehicleToMfrBodyCodeCount = m.length;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    getDefaultInputModel() {
        return {
            mfrBodyCodeId: 0,
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
            mfrBodyCodes: []
        };
    }

    onRemoveMfrBodyCode(mfrBodyCodeId: number) {
        if (confirm("Remove Mfr Body Code Id " + mfrBodyCodeId + " from selection?")) {
            this.mfrBodyCodes = this.mfrBodyCodes.filter(item => item.id != mfrBodyCodeId);
            this.refreshProposedVehicleToMfrBodyCodes();
        }
    }

    refreshProposedVehicleToMfrBodyCodes() {
        if (this.vehicles.length == 0 || this.mfrBodyCodes.length == 0) {
            return;
        }
        if (this.showLoadingGif) {
            return;
        }
        this.showLoadingGif = true;

        let allProposedVehicleToMfrBodyCodes: IVehicleToMfrBodyCode[] = [];

        this.vehicles.forEach(v => {
            this.mfrBodyCodes.forEach(b => {
                allProposedVehicleToMfrBodyCodes.push({
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
                        vehicleToMfrBodyCodeCount: null,
                        isSelected: false
                    },
                    mfrBodyCode: {
                        id: b.id,
                        name: b.name,
                        vehicleToMfrBodyCodeCount: 0
                    },
                    numberOfMfrBodyCodesAssociation: -1,
                    isSelected: true
                });
            });
        });

        let selectedVehicleIds = this.vehicles.map(x => x.id);
        let selectedMfrBodyCodeIds = this.mfrBodyCodes.map(x => x.id);

        this.vehicleToMfrBodyCodeService.getVehicleToMfrBodyCodesByVehicleIdsAndMfrBodyCodeIds(selectedVehicleIds, selectedMfrBodyCodeIds)
            .subscribe(m => {
                this.proposedVehicleToMfrBodyCodes = [];
                this.existingVehicleToMfrBodyCodes = m;
                if (this.existingVehicleToMfrBodyCodes == null || this.existingVehicleToMfrBodyCodes.length == 0) {
                    this.proposedVehicleToMfrBodyCodes = allProposedVehicleToMfrBodyCodes;
                }
                else {

                    let existingVehicleIds = this.existingVehicleToMfrBodyCodes.map(x => x.vehicle.id);
                    let existingMfrBodyCodeIds = this.existingVehicleToMfrBodyCodes.map(x => x.mfrBodyCode.id);
                    this.proposedVehicleToMfrBodyCodes = allProposedVehicleToMfrBodyCodes.filter(item => existingVehicleIds.indexOf(item.vehicle.id) < 0 || existingMfrBodyCodeIds.indexOf(item.mfrBodyCode.id) < 0);
                }

                if (this.proposedVehicleToMfrBodyCodes != null) {
                    this.proposedVehicleToMfrBodyCodesSelectionCount = this.proposedVehicleToMfrBodyCodes.filter(item => item.isSelected).length;
                }

                this.showLoadingGif = false;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
    }

    onSelectAllProposedMfrBodyCodeAssociation(event) {
        this.selectAllChecked = event;
        if (this.proposedVehicleToMfrBodyCodes == null) {
            return;
        }
        this.proposedVehicleToMfrBodyCodes.forEach(item => item.isSelected = event.target.checked);
        this.proposedVehicleToMfrBodyCodesSelectionCount = this.proposedVehicleToMfrBodyCodes.filter(item => item.isSelected).length;
    }

    refreshProposedVehicleToMfrBodyCodesSelectionCount(event, vehicleTomfrBodyCode: IVehicleToMfrBodyCode) {
        if (event.target.checked) {
            this.proposedVehicleToMfrBodyCodesSelectionCount++;
            var excludedVehicle = this.proposedVehicleToMfrBodyCodes.filter(item => item.isSelected);
            if (excludedVehicle.length == this.proposedVehicleToMfrBodyCodes.length - 1) {
                this.selectAllChecked = true;
            }

        } else {
            this.proposedVehicleToMfrBodyCodesSelectionCount--;
            this.selectAllChecked = false;
        }
    }

    onSubmitAssociations() {
        if (!this.proposedVehicleToMfrBodyCodes) {
            return;
        }
        let length: number = this.proposedVehicleToMfrBodyCodes.length;
        if (this.proposedVehicleToMfrBodyCodes.filter(item => item.isSelected).length == 0) {
            this.toastr.warning("No MFR Body Code associations selected", ConstantsWarehouse.errorTitle);
            return;
        }
        this.submitAssociations(length);
    }

    private getNextVehicleToMfrBodyCode(index): IVehicleToMfrBodyCode {
        if (!this.proposedVehicleToMfrBodyCodes || this.proposedVehicleToMfrBodyCodes.length == 0) {
            return null;
        }
        let nextConfig = this.proposedVehicleToMfrBodyCodes[index];
        return nextConfig;
    }

    private submitAssociations(length: number) {
        this.showLoadingGif = true;
        length = length - 1;
        if (length >= 0) {
            let proposedVehicleToMfrBodyCode = <IVehicleToMfrBodyCode>this.getNextVehicleToMfrBodyCode(length);
            proposedVehicleToMfrBodyCode.comment = this.commenttoadd;
            let vehicleToMfrBodyCodeIdentity: string = "(Vehicle ID: " + proposedVehicleToMfrBodyCode.vehicle.id + ", Mfr Body Code ID: " + proposedVehicleToMfrBodyCode.mfrBodyCode.id + ")"
            this.acFiles = this.acFileUploader.getAcFiles();
            this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
                if (uploadedFiles && uploadedFiles.length > 0) {
                    proposedVehicleToMfrBodyCode.attachments = this.sharedService.clone(uploadedFiles);
                }
                if (proposedVehicleToMfrBodyCode.attachments) {
                    proposedVehicleToMfrBodyCode.attachments = proposedVehicleToMfrBodyCode.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
                }
                this.vehicleToMfrBodyCodeService.addVehicleToMfrBodyCode(proposedVehicleToMfrBodyCode).subscribe(response => {
                    if (response) {
                        let successMessage = ConstantsWarehouse.notificationMessage.success("Vehicle to MFR Body Code", ConstantsWarehouse.changeRequestType.add, vehicleToMfrBodyCodeIdentity);
                        successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.add + " the \"" + vehicleToMfrBodyCodeIdentity + "\" Vehicle to MFR Body Code change requestid  \"" + response + "\" will be reviewed.";
                        this.toastr.success(successMessage.body, successMessage.title);
                        this.acFileUploader.reset();
                        this.acFileUploader.setAcFiles(this.acFiles);
                        this.submitAssociations(length);
                        this.router.navigateByUrl("/system/search");
                    } else {
                        let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle to Mfr Body Code", ConstantsWarehouse.changeRequestType.add, vehicleToMfrBodyCodeIdentity);
                        this.toastr.warning(errorMessage.body, errorMessage.title);
                        this.acFileUploader.reset();
                        this.acFileUploader.setAcFiles(this.acFiles);
                        this.submitAssociations(length);
                    }
                }, error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Vehicle to Mfr Body Code", ConstantsWarehouse.changeRequestType.add, vehicleToMfrBodyCodeIdentity);
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
