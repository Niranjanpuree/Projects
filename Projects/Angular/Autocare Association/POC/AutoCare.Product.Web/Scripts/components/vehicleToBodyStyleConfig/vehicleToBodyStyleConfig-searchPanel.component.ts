import { Component, OnInit, ViewChild, Input,
    Output, EventEmitter }                              from "@angular/core";
import { Router }                    from "@angular/router";
import { ToastsManager }                                from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                                from "../shared/shared.service";
import { NavigationService }                            from "../shared/navigation.service";
import { ConstantsWarehouse }                           from "../constants-warehouse";
import { IVehicleToBodyStyleConfigSearchInputModel,
    IVehicleToBodyStyleConfigSearchViewModel }          from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-search.model";
import { VehicleToBodyStyleConfigService }              from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.service";
import { IFacet, SearchType }                           from "../shared/shared.model";
import { IVehicleToBodyStyleConfig }                    from "./vehicleToBodyStyleConfig.model";

@Component({
    selector: "vehicletobodystyleconfig-searchpanel",
    templateUrl: "app/templates/vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-searchPanel.component.html",
    providers: [VehicleToBodyStyleConfigService, SharedService],
})

export class VehicleToBodyStyleConfigSearchPanel implements OnInit {
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
    private bodyNumDoorsFacet: IFacet[];
    private bodyTypeFacet: IFacet[];
    private selectedStartYear: string;
    private selectedEndYear: string;
    private bodyStyleConfigId: string;
    private vehicleToBodyStyleConfigSearchInputModel: IVehicleToBodyStyleConfigSearchInputModel;
    private isSelectAllBodyStyleConfigs: boolean;
    private vehicleToBodyStyleConfigsRetrieved: IVehicleToBodyStyleConfig[] = [];
    private showLoadingGif: boolean = false;

    @Input("thresholdRecordCount") thresholdRecordCount: number;
    @Input("vehicleToBodyStyleConfigSearchViewModel") vehicleToBodyStyleConfigSearchViewModel: IVehicleToBodyStyleConfigSearchViewModel;
    @Input("vehicleToBodyStyleConfigsForSelectedBodyStyle") vehicleToBodyStyleConfigsForSelectedBodyStyle: IVehicleToBodyStyleConfig[];
    @Output("onSearchEvent") onSearchEvent = new EventEmitter<IVehicleToBodyStyleConfig[]>();

    constructor(private sharedService: SharedService,
        private toastr: ToastsManager, private vehicleToBodyStyleConfigService: VehicleToBodyStyleConfigService) {
    }

    ngOnInit() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.bodyStyleConfigId = "";

        if (this.sharedService.vehicleToBodyStyleConfigSearchViewModel != null) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets = this.sharedService.vehicleToBodyStyleConfigSearchViewModel.facets;
            this.bodyNumDoorsFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.slice();
            this.bodyTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.slice();
            this.regionFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.slice();
            if (this.sharedService.vehicleToBodyStyleConfigSearchViewModel.searchType == SearchType.SearchByConfigId) {
                this.searchByBodyStyleConfigId();
            }
            else if (this.sharedService.vehicleToBodyStyleConfigSearchViewModel.searchType == SearchType.GeneralSearch) {
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

    onClearFilters() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.bodyStyleConfigId = "";

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.regions) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.makes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.models) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.forEach(item => item.isSelected = false);
        }

        this.refreshFacets();
    }

    refreshFacets() {
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.regions) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.makes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.models) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(m => inputModel.subModels.push(m.name));
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.filter(item => item.isSelected)
                .forEach(m => inputModel.bodyNumDoors.push(m.name));
        }

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.bodyTypes.push(m.name));
        }

        this.showLoadingGif = true;
        this.vehicleToBodyStyleConfigService.refreshFacets(inputModel).subscribe(data => {
            this.updateRegionFacet(data.facets.regions);
            this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
            this.updateVehicleTypeFacet(data.facets.vehicleTypes);
            this.updateYearFacet(data.facets.years);
            this.updateMakeFacet(data.facets.makes);
            this.updateModelFacet(data.facets.models);
            this.updateSubModelFacet(data.facets.subModels);
            this.updateBodyNumDoorsFacet(data.facets.bodyNumDoors);
            this.updateBodyTypeFacet(data.facets.bodyTypes);
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
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
            bodyNumDoors: [],
            bodyTypes: []
        };
    }

    private updateRegionFacet(regions) {
        let existingSelectedRegions = this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBodyStyleConfigSearchViewModel.facets.regions = [];
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
            this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.push(newItem);
        }
        this.regionFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.slice();
    }

    private updateVehicleTypeGroupFacet(vehicleTypeGroups) {
        let existingSelectedItems = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
            .map(item => item.name);
        this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups = [];
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
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }
        this.vehicleTypeGroupFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.slice();
    }

    private updateVehicleTypeFacet(vehicleTypes) {
        let existingSelectedItems = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
            .map(item => item.name);
        this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes = [];
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
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.push(newItem);
        }
        this.vehicleTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.slice();
    }

    private updateYearFacet(years) {
        this.vehicleToBodyStyleConfigSearchViewModel.facets.startYears = years.slice();
        this.vehicleToBodyStyleConfigSearchViewModel.facets.endYears = years.slice();
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    }

    private updateMakeFacet(makes) {
        let existingSelectedMakes = this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.filter(make => make.isSelected).map(item => item.name);
        this.vehicleToBodyStyleConfigSearchViewModel.facets.makes = [];
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
            this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.push(newMake);
        }
        this.makeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.slice();
    }

    private updateModelFacet(models) {
        let existingSelectedModels = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.filter(model => model.isSelected)
            .map(item => item.name);
        this.vehicleToBodyStyleConfigSearchViewModel.facets.models = [];
        for (let model of models) {
            let newModel = {
                name: model,
                isSelected: false
            };
            for (let existingSelectedModel of existingSelectedModels) {
                if (model === existingSelectedModel) {
                    newModel.isSelected = true;
                }
            }
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models.push(newModel);
        }
        this.modelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.slice();
    }

    private updateSubModelFacet(subModels) {
        let existingSelectedSubModels = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.filter(submodel => submodel.isSelected)
            .map(item => item.name);
        this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels = [];
        for (let subModel of subModels) {
            let newSubModel = {
                name: subModel,
                isSelected: false
            };
            for (let existingSelectedSubModel of existingSelectedSubModels) {
                if (subModel === existingSelectedSubModel) {
                    newSubModel.isSelected = true;
                }
            }
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.push(newSubModel);
        }
        this.subModelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.slice();
    }

    private updateBodyNumDoorsFacet(bodyNumDoors) {
        let existingSelectedItems = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors = [];
        for (let item of bodyNumDoors) {
            let newItem = {
                name: item,
                isSelected: false
            };
            for (let existingSelectedItem of existingSelectedItems) {
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.push(newItem);
        }
        this.bodyNumDoorsFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.slice();
    }

    private updateBodyTypeFacet(bodyTypes) {
        let existingSelectedItems = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes = [];
        for (let item of bodyTypes) {
            let newItem = {
                name: item,
                isSelected: false
            };
            for (let existingSelectedItem of existingSelectedItems) {
                if (item === existingSelectedItem) {
                    newItem.isSelected = true;
                }
            }
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.push(newItem);
        }
        this.bodyTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.slice();
    }

    // filters
    private onYearSelected() {
        this.refreshFacets();
    }

    private filterMakes($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.makes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.makeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterModels($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.modelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterSubModels($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.subModelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterRegions($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.regions != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.regionFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterVehicleTypeGroups($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeGroupFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterVehicleTypes($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterBodyNumDoors($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterBodyTypes($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private onItemSelected(event, facet: IFacet[]) {
        let isChecked = event.target.checked;
        let selectedItem = facet.filter(item => item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase())[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    }

    private searchByBodyStyleConfigId() {
        let bodyStyleConfigId = Number(this.bodyStyleConfigId);
        if (isNaN(bodyStyleConfigId)) {
            this.toastr.warning("Invalid Body Style Config Id", ConstantsWarehouse.validationTitle);
            return;
        }

        this.vehicleToBodyStyleConfigSearchViewModel.searchType = SearchType.SearchByConfigId;
        this.showLoadingGif = true;
        this.vehicleToBodyStyleConfigService.searchByBodyStyleConfigId(bodyStyleConfigId).subscribe(m => {
            if (m.result.bodyStyleConfigs.length > 0) {
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

    private getSearchResult(m: IVehicleToBodyStyleConfigSearchViewModel) {
        this.vehicleToBodyStyleConfigSearchViewModel.result = m.result;
        this.vehicleToBodyStyleConfigSearchViewModel.totalCount = m.totalCount;

        this.vehicleToBodyStyleConfigsForSelectedBodyStyle = [];
        this.isSelectAllBodyStyleConfigs = false;

        // note: select all only if totalCount <= threshold
        if (this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.length <= this.thresholdRecordCount) {
            this.vehicleToBodyStyleConfigSearchViewModel.result.bodyStyleConfigs.forEach(item => {
                item.isSelected = true;
                this.refreshAssociationWithBodyStyleConfigId(item.id, item.isSelected);
            });
            this.isSelectAllBodyStyleConfigs = true;
        }

        // callback emitter
        this.onSearchEvent.emit(this.vehicleToBodyStyleConfigsForSelectedBodyStyle);
    }

    private refreshAssociationWithBodyStyleConfigId(bodyStyleConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToBodyStyleConfigsRetrieved = this.getVehicleToBodyStyleConfigsByBodyStyleConfigId(bodyStyleConfigId);
            //TODO: number of associations which may be useful in add BodyStyle association screen?
            let temp = this.vehicleToBodyStyleConfigsForSelectedBodyStyle || [];
            for (var vehicleToBodyStyleConfig of this.vehicleToBodyStyleConfigsRetrieved) {
                temp.push(vehicleToBodyStyleConfig);
            }
            this.vehicleToBodyStyleConfigsForSelectedBodyStyle = temp;
        }
        else {
            let m = this.vehicleToBodyStyleConfigsForSelectedBodyStyle.filter(x => x.bodyStyleConfig.id != bodyStyleConfigId);
            this.vehicleToBodyStyleConfigsForSelectedBodyStyle = m;
        }
    }

    private getVehicleToBodyStyleConfigsByBodyStyleConfigId(id) {
        return this.vehicleToBodyStyleConfigSearchViewModel.result.vehicleToBodyStyleConfigs.filter(v => v.bodyStyleConfig.id == id);
    }

    private onBodyStyleConfigKeyPress(event) {
        if (event.keyCode == 13) {
            this.searchByBodyStyleConfigId();
        }
    }

    search() {
        this.vehicleToBodyStyleConfigSearchViewModel.searchType = SearchType.GeneralSearch;
        this.showLoadingGif = true;
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyNumDoors.filter(item => item.isSelected)
                .forEach(m => inputModel.bodyNumDoors.push(m.name));
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.bodyTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.bodyTypes.push(m.name));
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.makes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.models) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(s => inputModel.subModels.push(s.name));
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }
        if (this.vehicleToBodyStyleConfigSearchViewModel.facets.regions) {
            this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }
        this.vehicleToBodyStyleConfigService.search(inputModel).subscribe(m => {
            if (m.result.bodyStyleConfigs.length > 0 || m.result.vehicleToBodyStyleConfigs.length > 0) {
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