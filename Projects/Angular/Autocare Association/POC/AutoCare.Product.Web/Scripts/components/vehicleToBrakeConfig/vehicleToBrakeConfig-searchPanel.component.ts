import { Component, OnInit, ViewChild, Input,
    Output, EventEmitter }                              from "@angular/core";
import { ToastsManager }                                from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                                from "../shared/shared.service";
import { NavigationService }                            from "../shared/navigation.service";
import { ConstantsWarehouse }                           from "../constants-warehouse";
import { IVehicleToBrakeConfigSearchViewModel,
    SearchType,
    IVehicleToBrakeConfigSearchInputModel,
    IFacet }                                            from "../vehicleToBrakeConfig/vehicleToBrakeConfig-search.model";
import { VehicleToBrakeConfigService }                  from "../vehicleToBrakeConfig/vehicleToBrakeConfig.service";
import { IVehicleToBrakeConfig }                        from "../vehicleTobrakeConfig/vehicleTobrakeConfig.model";

@Component({
    selector: "vehicletobrakeconfig-searchpanel",
    templateUrl: "app/templates/vehicleToBrakeConfig/vehicleToBrakeConfig-searchPanel.component.html",
    providers: [VehicleToBrakeConfigService, SharedService],
})

export class VehicleToBrakeConfigSearchPanel {
    private makeQuery: string;
    private modelQuery: string;
    private subModelQuery: string;
    private regionFacet: IFacet[];
    private vehicleTypeFacet: IFacet[];
    private vehicleTypeGroupFacet: IFacet[] = [];
    private makeFacet: IFacet[];
    private modelFacet: IFacet[];
    private subModelFacet: IFacet[];
    private startYearFacet: string[];
    private endYearFacet: string[];
    private frontBrakeTypeFacet: IFacet[];
    private rearBrakeTypeFacet: IFacet[];
    private brakeAbsFacet: IFacet[];
    private brakeSystemFacet: IFacet[];
    private selectedStartYear: string;
    private selectedEndYear: string;
    private brakeConfigId: string;
    private vehicleToBrakeConfigSearchInputModel: IVehicleToBrakeConfigSearchInputModel;
    private isSelectAllBrakeSystems: boolean;
    private vehicleToBrakeConfigsRetrieved: IVehicleToBrakeConfig[] = [];
    private showLoadingGif: boolean = false;

    @Input("thresholdRecordCount") thresholdRecordCount: number;
    @Input("vehicleToBrakeConfigSearchViewModel") vehicleToBrakeConfigSearchViewModel: IVehicleToBrakeConfigSearchViewModel;
    @Input("vehicleToBrakeConfigsForSelectedBrake") vehicleToBrakeConfigsForSelectedBrake: IVehicleToBrakeConfig[];
    @Output("onSearchEvent") onSearchEvent = new EventEmitter<IVehicleToBrakeConfig[]>();

    constructor(private sharedService: SharedService,
        private toastr: ToastsManager, private vehicleToBrakeConfigService: VehicleToBrakeConfigService) {
    }

    ngOnInit() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.brakeConfigId = "";

        if (this.sharedService.vehicleToBrakeConfigSearchViewModel != null) {
            this.vehicleToBrakeConfigSearchViewModel.facets = this.sharedService.vehicleToBrakeConfigSearchViewModel.facets;
            this.frontBrakeTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.slice();
            this.rearBrakeTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.slice();
            this.brakeAbsFacet = this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.slice();
            this.brakeSystemFacet = this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.slice();
            this.regionFacet = this.vehicleToBrakeConfigSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleToBrakeConfigSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleToBrakeConfigSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.slice();
            if (this.sharedService.vehicleToBrakeConfigSearchViewModel.searchType == SearchType.SearchByBrakeConfigId) {
                this.searchByBrakeConfigId();
            }
            else if (this.sharedService.vehicleToBrakeConfigSearchViewModel.searchType == SearchType.GeneralSearch) {
                this.search();
            }
            else {
                this.showLoadingGif = false;
            }
        }
        else {
            this.refreshFacets();
        }
    }

    private onClearFilters() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.brakeConfigId = "";

        if (this.vehicleToBrakeConfigSearchViewModel.facets.regions) {
            this.vehicleToBrakeConfigSearchViewModel.facets.regions.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.makes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.makes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.models) {
            this.vehicleToBrakeConfigSearchViewModel.facets.models.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.subModels) {
            this.vehicleToBrakeConfigSearchViewModel.facets.subModels.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs) {
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems) {
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.forEach(item => item.isSelected = false);
        }

        this.refreshFacets();
    }

    private getDefaultInputModel() {
        return {
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

    private refreshFacets() {
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToBrakeConfigSearchViewModel.facets.regions) {
            this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.makes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.models) {
            this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.subModels) {
            this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(m => inputModel.subModels.push(m.name));
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.frontBrakeTypes.push(m.name));
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.rearBrakeTypes.push(m.name));
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs) {
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.filter(item => item.isSelected)
                .forEach(m => inputModel.brakeAbs.push(m.name));
        }

        if (this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems) {
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.filter(item => item.isSelected)
                .forEach(m => inputModel.brakeSystems.push(m.name));
        }

        this.showLoadingGif = true;
        this.vehicleToBrakeConfigService.refreshFacets(inputModel).subscribe(data => {
            this.updateRegionFacet(data.facets.regions);
            this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
            this.updateVehicleTypeFacet(data.facets.vehicleTypes);
            this.updateYearFacet(data.facets.years);
            this.updateMakeFacet(data.facets.makes);
            this.updateModelFacet(data.facets.models, "");
            this.updateSubModelFacet(data.facets.subModels, "");
            this.updateFrontBrakeTypeFacet(data.facets.frontBrakeTypes);
            this.updateRearBrakeTypeFacet(data.facets.rearBrakeTypes);
            this.updateBrakeAbsFacet(data.facets.brakeAbs);
            this.updateBrakeSystemFacet(data.facets.brakeSystems);

            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    private filterMakes($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.makes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.makeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterModels($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.models != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.modelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterSubModels($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.subModels != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.subModelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterRegions($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.regions != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.regionFacet = this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterVehicleTypeGroups($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeGroupFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterVehicleTypes($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterFrontBrakeTypes($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterRearBrakeTypes($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterBrakeAbs($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterBrakeSystems($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private onYearSelected() {
        this.refreshFacets();
    }

    private onItemSelected(event, facet: IFacet[]) {
        let isChecked = event.target.checked;
        let selectedItem = facet.filter(item => item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase())[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    }

    private updateRegionFacet(regions) {
        let existingSelectedRegions = this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBrakeConfigSearchViewModel.facets.regions = [];

        for (let item of regions) {
            let newItem = {
                name: item,
                isSelected: false
            };

            for (let existingSelectedRegion of existingSelectedRegions) {
                if (item === existingSelectedRegion) {
                    newItem.isSelected = true;
                }
            }

            this.vehicleToBrakeConfigSearchViewModel.facets.regions.push(newItem);
        }

        this.regionFacet = this.vehicleToBrakeConfigSearchViewModel.facets.regions.slice();
    }

    private updateYearFacet(years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    }

    private updateMakeFacet(makes) {
        let existingSelectedMakes = this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(make => make.isSelected).map(item => item.name);
        this.vehicleToBrakeConfigSearchViewModel.facets.makes = [];

        for (let make of makes) {
            let newMake = {
                name: make,
                isSelected: false
            };

            for (let existingSelectedMake of existingSelectedMakes) {
                if (make === existingSelectedMake) {
                    newMake.isSelected = true;
                }
            }

            this.vehicleToBrakeConfigSearchViewModel.facets.makes.push(newMake);
        }

        this.makeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.makes.slice();
    }

    //TODO: makeName is not used
    private updateModelFacet(models, makeName) {
        let existingSelectedModels = this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(model => model.isSelected)
            .map(item => item.name);

        this.vehicleToBrakeConfigSearchViewModel.facets.models = [];

        for (let model of models) {
            let newModel = {
                name: model,
                isSelected: false,
            };

            for (let existingSelectedModel of existingSelectedModels) {
                if (model === existingSelectedModel) {
                    newModel.isSelected = true;
                }
            }

            this.vehicleToBrakeConfigSearchViewModel.facets.models.push(newModel);
        }

        this.modelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.models.slice();
    }

    private updateSubModelFacet(subModels, modelName) {
        let existingSelectedSubModels = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(submodel => submodel.isSelected)
            .map(item => item.name);

        this.vehicleToBrakeConfigSearchViewModel.facets.subModels = [];

        for (let subModel of subModels) {
            let newSubModel = {
                name: subModel,
                isSelected: false,
            };

            for (let existingSelectedSubModel of existingSelectedSubModels) {
                if (subModel === existingSelectedSubModel) {
                    newSubModel.isSelected = true;
                }
            }

            this.vehicleToBrakeConfigSearchViewModel.facets.subModels.push(newSubModel);
        }

        this.subModelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.slice();
    }

    private updateVehicleTypeGroupFacet(vehicleTypeGroups) {
        let existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups = [];

        for (let item of vehicleTypeGroups) {
            let newItem = {
                name: item,
                isSelected: false
            };

            for (let existingSelectedItem of existingSelectedItems) {
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }

            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }

        this.vehicleTypeGroupFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.slice();
    }

    private updateVehicleTypeFacet(vehicleTypes) {
        let existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes = [];

        for (let item of vehicleTypes) {
            let newItem = {
                name: item,
                isSelected: false
            };

            for (let existingSelectedItem of existingSelectedItems) {
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }

            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.push(newItem);
        }

        this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.slice();
    }

    private updateFrontBrakeTypeFacet(frontBrakeTypes) {
        let existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes = [];

        for (let item of frontBrakeTypes) {
            let newItem = {
                name: item,
                isSelected: false
            };

            for (let existingSelectedItem of existingSelectedItems) {
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }

            this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.push(newItem);
        }

        this.frontBrakeTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.slice();
    }

    private updateRearBrakeTypeFacet(rearBrakeTypes) {
        let existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes = [];

        for (let item of rearBrakeTypes) {
            let newItem = {
                name: item,
                isSelected: false
            };

            for (let existingSelectedItem of existingSelectedItems) {
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }

            this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.push(newItem);
        }

        this.rearBrakeTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.slice();
    }

    private updateBrakeAbsFacet(brakeAbs) {
        let existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs = [];

        for (let item of brakeAbs) {
            let newItem = {
                name: item,
                isSelected: false
            };

            for (let existingSelectedItem of existingSelectedItems) {
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }

            this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.push(newItem);
        }

        this.brakeAbsFacet = this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.slice();
    }

    private updateBrakeSystemFacet(brakeSystems) {
        let existingSelectedItems = this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems = [];

        for (let item of brakeSystems) {
            let newItem = {
                name: item,
                isSelected: false
            };

            for (let existingSelectedItem of existingSelectedItems) {
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }

            this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.push(newItem);
        }

        this.brakeSystemFacet = this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.slice();
    }

    private searchByBrakeConfigId() {
        let brakeConfigId = Number(this.brakeConfigId);
        if (isNaN(brakeConfigId)) {
            this.toastr.warning("Invalid Break Config Id", ConstantsWarehouse.validationTitle);
            return;
        }

        this.vehicleToBrakeConfigSearchViewModel.searchType = SearchType.SearchByBrakeConfigId;
        this.showLoadingGif = true;
        this.vehicleToBrakeConfigService.searchByBrakeConfigId(brakeConfigId).subscribe(m => {
            if (m.result.brakeConfigs.length > 0) {
                this.getSearchResult(<any>m);
                this.showLoadingGif = false;
                $(".drawer-left").css('width', 15);
            }
            else {
                this.toastr.warning("The search yeilded no result", "No Record Found!!");
                this.showLoadingGif = false;
            }
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    private onBrakeConfigKeyPress(event) {
        if (event.keyCode == 13) {
            this.searchByBrakeConfigId();
        }
    }

    private getSearchResult(m: IVehicleToBrakeConfigSearchViewModel) {
        this.vehicleToBrakeConfigSearchViewModel.result = m.result;

        this.vehicleToBrakeConfigSearchViewModel.totalCount = m.totalCount;

        this.vehicleToBrakeConfigsForSelectedBrake = [];
        this.isSelectAllBrakeSystems = false;

        // note: select all when totalRecords <= threshold
        if (this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.length <= this.thresholdRecordCount) {
            this.vehicleToBrakeConfigSearchViewModel.result.brakeConfigs.forEach(item => {
                item.isSelected = true;
                this.refreshAssociationWithBrakeConfigId(item.id, item.isSelected);
            });
            this.isSelectAllBrakeSystems = true;
        }

        // callback emitter
        this.onSearchEvent.emit(this.vehicleToBrakeConfigsForSelectedBrake);
    }

    // todo: make is modular, this method is at two location
    private refreshAssociationWithBrakeConfigId(brakeConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToBrakeConfigsRetrieved = this.getVehicleToBrakeConfigsByBrakeConfigId(brakeConfigId);
            //TODO: number of associations which may be useful in add brake association screen?
            let temp = this.vehicleToBrakeConfigsForSelectedBrake || [];
            for (var vehicleToBrakeConfig of this.vehicleToBrakeConfigsRetrieved) {
                temp.push(vehicleToBrakeConfig);
            }
            this.vehicleToBrakeConfigsForSelectedBrake = temp;
        }
        else {
            let m = this.vehicleToBrakeConfigsForSelectedBrake.filter(x => x.brakeConfig.id != brakeConfigId);
            this.vehicleToBrakeConfigsForSelectedBrake = m;
        }
    }

    private getVehicleToBrakeConfigsByBrakeConfigId(id) {
        return this.vehicleToBrakeConfigSearchViewModel.result.vehicleToBrakeConfigs.filter(v => v.brakeConfig.id == id);
    }

    private search() {
        this.vehicleToBrakeConfigSearchViewModel.searchType = SearchType.GeneralSearch;
        this.showLoadingGif = true;
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.frontBrakeTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.frontBrakeTypes.push(m.name));
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.rearBrakeTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.rearBrakeTypes.push(m.name));
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs) {
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeAbs.filter(item => item.isSelected)
                .forEach(m => inputModel.brakeAbs.push(m.name));
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems) {
            this.vehicleToBrakeConfigSearchViewModel.facets.brakeSystems.filter(item => item.isSelected)
                .forEach(m => inputModel.brakeSystems.push(m.name));
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.makes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.models) {
            this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.subModels) {
            this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(s => inputModel.subModels.push(s.name));
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }
        if (this.vehicleToBrakeConfigSearchViewModel.facets.regions) {
            this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }
        this.vehicleToBrakeConfigService.search(inputModel).subscribe(m => {
            if (m.result.brakeConfigs.length > 0 || m.result.vehicleToBrakeConfigs.length > 0) {
                this.getSearchResult(<any>m);
                this.showLoadingGif = false;
                $(".drawer-left").css('width', 15);
            }
            else {
                this.toastr.warning("The search yeilded no result", "No Record Found!!");
                this.showLoadingGif = false;
            }
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    private clearFacet(facet: IFacet[], refresh: boolean = true) {
        if (facet) {
            facet.forEach(item => item.isSelected = false);
        }
        if (refresh) {
            this.refreshFacets();
        }
    }
}