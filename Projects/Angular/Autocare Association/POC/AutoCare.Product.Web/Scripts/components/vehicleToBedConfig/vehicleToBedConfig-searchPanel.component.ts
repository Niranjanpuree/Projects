import { Component, OnInit, ViewChild, Input,
    Output, EventEmitter }                             from "@angular/core";
import { Router }                   from "@angular/router";
import { ToastsManager }                               from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                               from "../shared/shared.service";
import { NavigationService }                           from "../shared/navigation.service";
import { ConstantsWarehouse }                          from "../constants-warehouse";
import { IVehicleToBedConfigSearchViewModel,
    SearchType,
    IVehicleToBedConfigSearchInputModel, IFacet }            from "../vehicleToBedConfig/vehicleToBedConfig-search.model";
import { IVehicleToBedConfig }                       from "../vehicleTobedConfig/vehicleTobedConfig.model";
import { VehicleToBedConfigService }                       from "../vehicleTobedConfig/vehicleTobedConfig.service";

@Component({
    selector: "vehicletobedconfig-searchpanel",
    templateUrl: "app/templates/vehicleToBedConfig/vehicleToBedConfig-searchPanel.component.html",
    providers: [VehicleToBedConfigService],
})

export class VehicleToBedConfigSearchPanel {
    private makeQuery: string;
    private modelQuery: string;
    private subModelQuery: string;

    regionFacet: IFacet[];
    vehicleTypeFacet: IFacet[];
    vehicleTypeGroupFacet: IFacet[] = [];
    makeFacet: IFacet[];
    modelFacet: IFacet[];
    subModelFacet: IFacet[];
    startYearFacet: string[];
    endYearFacet: string[];
    bedTypeFacet: IFacet[];
    bedLengthFacet: IFacet[];
    selectedStartYear: string;
    selectedEndYear: string;
    bedConfigId: string;
    private vehicleToBedConfigSearchInputModel: IVehicleToBedConfigSearchInputModel;
    private isSelectAllBedSystems: boolean;
    private vehicleToBedConfigsRetrieved: IVehicleToBedConfig[] = [];
    private showLoadingGif: boolean = false;

    @Input("thresholdRecordCount") thresholdRecordCount: number;
    @Input("vehicleToBedConfigSearchViewModel") vehicleToBedConfigSearchViewModel: IVehicleToBedConfigSearchViewModel;
    @Input("vehicleToBedConfigsForSelectedBed") vehicleToBedConfigsForSelectedBed: IVehicleToBedConfig[];
    @Output("onSearchEvent") onSearchEvent = new EventEmitter<IVehicleToBedConfig[]>();

    constructor(private sharedService: SharedService, 
        private toastr: ToastsManager, private vehicleToBedConfigService: VehicleToBedConfigService) {
    }

    ngOnInit() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";

        if (this.sharedService.vehicleToBedConfigSearchViewModel != null) {
            this.vehicleToBedConfigSearchViewModel.facets = this.sharedService.vehicleToBedConfigSearchViewModel.facets;
            this.bedLengthFacet = this.vehicleToBedConfigSearchViewModel.facets.bedLengths.slice();
            this.bedTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.bedTypes.slice();
            this.regionFacet = this.vehicleToBedConfigSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleToBedConfigSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleToBedConfigSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleToBedConfigSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleToBedConfigSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleToBedConfigSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.slice();
            if (this.sharedService.vehicleToBedConfigSearchViewModel.searchType == SearchType.SearchByBedConfigId) {
                this.searchByBedConfigId();
            }
            else if (this.sharedService.vehicleToBedConfigSearchViewModel.searchType == SearchType.GeneralSearch)
            {
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

        if (this.vehicleToBedConfigSearchViewModel.facets.regions) {
            this.vehicleToBedConfigSearchViewModel.facets.regions.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.makes) {
            this.vehicleToBedConfigSearchViewModel.facets.makes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.models) {
            this.vehicleToBedConfigSearchViewModel.facets.models.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.subModels) {
            this.vehicleToBedConfigSearchViewModel.facets.subModels.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.bedTypes) {
            this.vehicleToBedConfigSearchViewModel.facets.bedTypes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.bedLengths) {
            this.vehicleToBedConfigSearchViewModel.facets.bedLengths.forEach(item => item.isSelected = false);
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
            bedTypes: [],
            bedLengths: []
        };
    }
    
    private refreshFacets() {
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToBedConfigSearchViewModel.facets.regions) {
            this.vehicleToBedConfigSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.makes) {
            this.vehicleToBedConfigSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.models) {
            this.vehicleToBedConfigSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.subModels) {
            this.vehicleToBedConfigSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(m => inputModel.subModels.push(m.name));
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.bedTypes) {
            this.vehicleToBedConfigSearchViewModel.facets.bedTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.bedTypes.push(m.name));
        }

        if (this.vehicleToBedConfigSearchViewModel.facets.bedLengths) {
            this.vehicleToBedConfigSearchViewModel.facets.bedLengths.filter(item => item.isSelected)
                .forEach(m => inputModel.bedLengths.push(m.name));
        }
        
        this.showLoadingGif = true;
        this.vehicleToBedConfigService.refreshFacets(inputModel).subscribe(data => {
            this.updateRegionFacet(data.facets.regions);
            this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
            this.updateVehicleTypeFacet(data.facets.vehicleTypes);
            this.updateYearFacet(data.facets.years);
            this.updateMakeFacet(data.facets.makes);
            this.updateModelFacet(data.facets.models, "");
            this.updateSubModelFacet(data.facets.subModels, "");
            this.updateBedTypeFacet(data.facets.bedTypes);
            this.updateBedLengthFacet(data.facets.bedLengths);

            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    private filterMakes($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.makes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.makeFacet = this.vehicleToBedConfigSearchViewModel.facets.makes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterModels($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.models != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.modelFacet = this.vehicleToBedConfigSearchViewModel.facets.models.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterSubModels($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.subModels != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.subModelFacet = this.vehicleToBedConfigSearchViewModel.facets.subModels.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterRegions($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.regions != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.regionFacet = this.vehicleToBedConfigSearchViewModel.facets.regions.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterVehicleTypeGroups($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeGroupFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterVehicleTypes($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterBedTypes($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.bedTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.bedTypes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterBedLengths($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.bedLengths != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.bedLengths.filter(
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
        let existingSelectedRegions = this.vehicleToBedConfigSearchViewModel.facets.regions.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBedConfigSearchViewModel.facets.regions = [];

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

            this.vehicleToBedConfigSearchViewModel.facets.regions.push(newItem);
        }

        this.regionFacet = this.vehicleToBedConfigSearchViewModel.facets.regions.slice();
    }

    private updateYearFacet(years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    }

    private updateMakeFacet(makes) {
        let existingSelectedMakes = this.vehicleToBedConfigSearchViewModel.facets.makes.filter(make => make.isSelected).map(item => item.name);
        this.vehicleToBedConfigSearchViewModel.facets.makes = [];

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

            this.vehicleToBedConfigSearchViewModel.facets.makes.push(newMake);
        }

        this.makeFacet = this.vehicleToBedConfigSearchViewModel.facets.makes.slice();
    }

    //TODO: makeName is not used
    private updateModelFacet(models, makeName) {
        let existingSelectedModels = this.vehicleToBedConfigSearchViewModel.facets.models.filter(model => model.isSelected)
            .map(item => item.name);

        this.vehicleToBedConfigSearchViewModel.facets.models = [];

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

            this.vehicleToBedConfigSearchViewModel.facets.models.push(newModel);
        }

        this.modelFacet = this.vehicleToBedConfigSearchViewModel.facets.models.slice();
    }

    private updateSubModelFacet(subModels, modelName) {
        let existingSelectedSubModels = this.vehicleToBedConfigSearchViewModel.facets.subModels.filter(submodel => submodel.isSelected)
            .map(item => item.name);

        this.vehicleToBedConfigSearchViewModel.facets.subModels = [];

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

            this.vehicleToBedConfigSearchViewModel.facets.subModels.push(newSubModel);
        }

        this.subModelFacet = this.vehicleToBedConfigSearchViewModel.facets.subModels.slice();
    }

    private updateVehicleTypeGroupFacet(vehicleTypeGroups) {
        let existingSelectedItems = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups = [];

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

            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }

        this.vehicleTypeGroupFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.slice();
    }

    private updateVehicleTypeFacet(vehicleTypes) {
        let existingSelectedItems = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes = [];

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

            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.push(newItem);
        }

        this.vehicleTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.slice();
    }

    private updateBedTypeFacet(bedTypes) {
        let existingSelectedItems = this.vehicleToBedConfigSearchViewModel.facets.bedTypes.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBedConfigSearchViewModel.facets.bedTypes = [];

        for (let item of bedTypes) {
            let newItem = {
                name: item,
                isSelected: false
            };

            for (let existingSelectedItem of existingSelectedItems) {
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }

            this.vehicleToBedConfigSearchViewModel.facets.bedTypes.push(newItem);
        }

        this.bedTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.bedTypes.slice();
    }

    private updateBedLengthFacet(bedLengths) {
        let existingSelectedItems = this.vehicleToBedConfigSearchViewModel.facets.bedLengths.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBedConfigSearchViewModel.facets.bedLengths = [];

        for (let item of bedLengths) {
            let newItem = {
                name: item,
                isSelected: false
            };

            for (let existingSelectedItem of existingSelectedItems) {
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }

            this.vehicleToBedConfigSearchViewModel.facets.bedLengths.push(newItem);
        }

        this.bedLengthFacet = this.vehicleToBedConfigSearchViewModel.facets.bedLengths.slice();
    }

    private searchByBedConfigId() {
        let bedConfigId = Number(this.bedConfigId);
        if (isNaN(bedConfigId)) {
            this.toastr.warning("Invalid Bed Config Id", ConstantsWarehouse.validationTitle);
            return;
        }

        this.vehicleToBedConfigSearchViewModel.searchType = SearchType.SearchByBedConfigId;
        this.showLoadingGif = true;
        this.vehicleToBedConfigService.searchByBedConfigId(bedConfigId).subscribe(m => {
            if (m.result.bedConfigs.length > 0) {
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

    private onBedConfigKeyPress(event) {
        if (event.keyCode == 13) {
            this.searchByBedConfigId();
        }
    }

    private getSearchResult(m: IVehicleToBedConfigSearchViewModel) {
        this.vehicleToBedConfigSearchViewModel.result = m.result;
        this.vehicleToBedConfigSearchViewModel.totalCount = m.totalCount;

        this.vehicleToBedConfigsForSelectedBed= [];

        this.isSelectAllBedSystems = false;

        if (this.vehicleToBedConfigSearchViewModel.result.bedConfigs.length <= this.thresholdRecordCount) {
            this.vehicleToBedConfigSearchViewModel.result.bedConfigs.forEach(item => {
                item.isSelected = true;
                this.refreshAssociationWithBedConfigId(item.id, item.isSelected);
            });
            this.isSelectAllBedSystems = true;
        }

        // callback emitter
        this.onSearchEvent.emit(this.vehicleToBedConfigsForSelectedBed);
    }

    // todo: make is modular, this method is at two location
    private refreshAssociationWithBedConfigId(bedConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToBedConfigsRetrieved = this.getVehicleToBedConfigsByBedConfigId(bedConfigId);
            //TODO: number of associations which may be useful in add bed association screen?
            let temp = this.vehicleToBedConfigsForSelectedBed || [];
            for (var vehicleToBedConfig of this.vehicleToBedConfigsRetrieved) {
                temp.push(vehicleToBedConfig);
            }
            this.vehicleToBedConfigsForSelectedBed = temp;
        }
        else {
            let m = this.vehicleToBedConfigsForSelectedBed.filter(x => x.bedConfig.id != bedConfigId);
            this.vehicleToBedConfigsForSelectedBed = m;
        }
    }

    private getVehicleToBedConfigsByBedConfigId(id) {
        return this.vehicleToBedConfigSearchViewModel.result.vehicleToBedConfigs.filter(v => v.bedConfig.id == id);
    }

    private search() {
        this.vehicleToBedConfigSearchViewModel.searchType = SearchType.GeneralSearch;
        this.showLoadingGif = true;
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToBedConfigSearchViewModel.facets.bedTypes) {
            this.vehicleToBedConfigSearchViewModel.facets.bedTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.bedTypes.push(m.name));
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.bedLengths) {
            this.vehicleToBedConfigSearchViewModel.facets.bedLengths.filter(item => item.isSelected)
                .forEach(m => inputModel.bedLengths.push(m.name));
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.makes) {
            this.vehicleToBedConfigSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.models) {
            this.vehicleToBedConfigSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.subModels) {
            this.vehicleToBedConfigSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(s => inputModel.subModels.push(s.name));
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }
        if (this.vehicleToBedConfigSearchViewModel.facets.regions) {
            this.vehicleToBedConfigSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }
        this.vehicleToBedConfigService.search(inputModel).subscribe(m => {
            if (m.result.bedConfigs.length > 0 || m.result.vehicleToBedConfigs.length > 0) {
                this.getSearchResult(<any>m);
                this.showLoadingGif = false;
                $(".drawer-left").css('width', 15);
            }
            else {
                this.toastr.warning("The search yeilded no result", "No Record Found!!");
                this.showLoadingGif = false;
            }
        },
            error => {
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
