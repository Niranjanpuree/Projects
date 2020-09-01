import { Component, OnInit, ViewChild, Input,
    Output, EventEmitter }                              from "@angular/core";
import { Router }                    from "@angular/router";
import { ToastsManager }                                from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                                from "../shared/shared.service";
import { NavigationService }                            from "../shared/navigation.service";
import { ConstantsWarehouse }                           from "../constants-warehouse";
import { IVehicleToWheelBaseSearchViewModel,
    SearchType,
    IVehicleToWheelBaseSearchInputModel,
    IFacet }                                            from "../vehicleToWheelBase/vehicleToWheelBase-search.model";
import { VehicleToWheelBaseService }                  from "../vehicleTowheelBase/vehicleToWheelBase.service";
import { IVehicleToWheelBase }                        from "../vehicleTowheelBase/vehicleTowheelBase.model";

@Component({
    selector: "vehicletowheelbase-searchpanel",
    templateUrl: "app/templates/VehicleToWheelBase/vehicleToWheelBase-searchPanel.component.html",
    providers: [VehicleToWheelBaseService],
})

export class VehicleToWheelBaseSearchPanel {
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
    private selectedStartYear: string;
    private selectedEndYear: string;
    private wheelBaseId: string;
    private vehicleToWheelBaseSearchInputModel: IVehicleToWheelBaseSearchInputModel;
    private isSelectAllWheelBaseSystems: boolean;
    private vehicleToWheelBaseRetrieved: IVehicleToWheelBase[] = [];
    private showLoadingGif: boolean = false;

    @Input("thresholdRecordCount") thresholdRecordCount: number;
    @Input("vehicleToWheelBaseSearchViewModel") vehicleToWheelBaseSearchViewModel: IVehicleToWheelBaseSearchViewModel;
    @Input("vehicleToWheelBaseForSelectedWheelBase") vehicleToWheelBaseForSelectedWheelBase: IVehicleToWheelBase[];
    @Output("onSearchEvent") onSearchEvent = new EventEmitter<IVehicleToWheelBase[]>();

    constructor(private sharedService: SharedService,
        private toastr: ToastsManager, private vehicleToWheelBaseService: VehicleToWheelBaseService) {
    }

    ngOnInit() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.wheelBaseId = "";

        if (this.sharedService.vehicleToWheelBaseSearchViewModel != null) {
            this.vehicleToWheelBaseSearchViewModel.facets = this.sharedService.vehicleToWheelBaseSearchViewModel.facets;
            this.regionFacet = this.vehicleToWheelBaseSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleToWheelBaseSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleToWheelBaseSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleToWheelBaseSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleToWheelBaseSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleToWheelBaseSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.slice();
            if (this.sharedService.vehicleToWheelBaseSearchViewModel.searchType == SearchType.SearchByWheelBaseId) {
                this.searchByWheelBaseId();
            }
            else if (this.sharedService.vehicleToWheelBaseSearchViewModel.searchType == SearchType.GeneralSearch) {
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
        this.wheelBaseId = "";

        if (this.vehicleToWheelBaseSearchViewModel.facets.regions) {
            this.vehicleToWheelBaseSearchViewModel.facets.regions.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToWheelBaseSearchViewModel.facets.makes) {
            this.vehicleToWheelBaseSearchViewModel.facets.makes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToWheelBaseSearchViewModel.facets.models) {
            this.vehicleToWheelBaseSearchViewModel.facets.models.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToWheelBaseSearchViewModel.facets.subModels) {
            this.vehicleToWheelBaseSearchViewModel.facets.subModels.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes) {
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.forEach(item => item.isSelected = false);
        }

        this.refreshFacets();
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

    private refreshFacets() {
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToWheelBaseSearchViewModel.facets.regions) {
            this.vehicleToWheelBaseSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }

        if (this.vehicleToWheelBaseSearchViewModel.facets.makes) {
            this.vehicleToWheelBaseSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }

        if (this.vehicleToWheelBaseSearchViewModel.facets.models) {
            this.vehicleToWheelBaseSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }

        if (this.vehicleToWheelBaseSearchViewModel.facets.subModels) {
            this.vehicleToWheelBaseSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(m => inputModel.subModels.push(m.name));
        }

        if (this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }

        if (this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes) {
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }

        this.showLoadingGif = true;
        this.vehicleToWheelBaseService.refreshFacets(inputModel).subscribe(data => {
            this.updateRegionFacet(data.facets.regions);
            this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
            this.updateVehicleTypeFacet(data.facets.vehicleTypes);
            this.updateYearFacet(data.facets.years);
            this.updateMakeFacet(data.facets.makes);
            this.updateModelFacet(data.facets.models, "");
            this.updateSubModelFacet(data.facets.subModels, "");

            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    private filterMakes($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.makes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.makeFacet = this.vehicleToWheelBaseSearchViewModel.facets.makes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterModels($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.models != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.modelFacet = this.vehicleToWheelBaseSearchViewModel.facets.models.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterSubModels($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.subModels != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.subModelFacet = this.vehicleToWheelBaseSearchViewModel.facets.subModels.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterRegions($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.regions != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.regionFacet = this.vehicleToWheelBaseSearchViewModel.facets.regions.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterVehicleTypeGroups($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeGroupFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterVehicleTypes($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.filter(
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
        let existingSelectedRegions = this.vehicleToWheelBaseSearchViewModel.facets.regions.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToWheelBaseSearchViewModel.facets.regions = [];

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

            this.vehicleToWheelBaseSearchViewModel.facets.regions.push(newItem);
        }

        this.regionFacet = this.vehicleToWheelBaseSearchViewModel.facets.regions.slice();
    }

    private updateYearFacet(years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    }

    private updateMakeFacet(makes) {
        let existingSelectedMakes = this.vehicleToWheelBaseSearchViewModel.facets.makes.filter(make => make.isSelected).map(item => item.name);
        this.vehicleToWheelBaseSearchViewModel.facets.makes = [];

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

            this.vehicleToWheelBaseSearchViewModel.facets.makes.push(newMake);
        }

        this.makeFacet = this.vehicleToWheelBaseSearchViewModel.facets.makes.slice();
    }

    //TODO: makeName is not used
    private updateModelFacet(models, makeName) {
        let existingSelectedModels = this.vehicleToWheelBaseSearchViewModel.facets.models.filter(model => model.isSelected)
            .map(item => item.name);

        this.vehicleToWheelBaseSearchViewModel.facets.models = [];

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

            this.vehicleToWheelBaseSearchViewModel.facets.models.push(newModel);
        }

        this.modelFacet = this.vehicleToWheelBaseSearchViewModel.facets.models.slice();
    }

    private updateSubModelFacet(subModels, modelName) {
        let existingSelectedSubModels = this.vehicleToWheelBaseSearchViewModel.facets.subModels.filter(submodel => submodel.isSelected)
            .map(item => item.name);

        this.vehicleToWheelBaseSearchViewModel.facets.subModels = [];

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

            this.vehicleToWheelBaseSearchViewModel.facets.subModels.push(newSubModel);
        }

        this.subModelFacet = this.vehicleToWheelBaseSearchViewModel.facets.subModels.slice();
    }

    private updateVehicleTypeGroupFacet(vehicleTypeGroups) {
        let existingSelectedItems = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups = [];

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

            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }

        this.vehicleTypeGroupFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.slice();
    }

    private updateVehicleTypeFacet(vehicleTypes) {
        let existingSelectedItems = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes = [];

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

            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.push(newItem);
        }

        this.vehicleTypeFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.slice();
    }

    private searchByWheelBaseId() {
        let wheelBaseId = Number(this.wheelBaseId);
        if (isNaN(wheelBaseId)) {
            this.toastr.warning("Invalid Wheel Base Id", ConstantsWarehouse.validationTitle);
            return;
        }

        this.vehicleToWheelBaseSearchViewModel.searchType = SearchType.SearchByWheelBaseId;
        this.showLoadingGif = true;
        this.vehicleToWheelBaseService.searchByWheelBaseId(wheelBaseId).subscribe(m => {
            if (m.result.wheelBases.length > 0) {
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

    private onWheelBaseKeyPress(event) {
        if (event.keyCode == 13) {
            this.searchByWheelBaseId();
        }
    }

    private getSearchResult(m: IVehicleToWheelBaseSearchViewModel) {
        this.vehicleToWheelBaseSearchViewModel.result = m.result;

        this.vehicleToWheelBaseSearchViewModel.totalCount = m.totalCount;

        this.vehicleToWheelBaseForSelectedWheelBase = [];
        this.isSelectAllWheelBaseSystems = false;

        // note: select all when totalRecords <= threshold
        if (this.vehicleToWheelBaseSearchViewModel.result.wheelBases.length <= this.thresholdRecordCount) {
            this.vehicleToWheelBaseSearchViewModel.result.wheelBases.forEach(item => {
                item.isSelected = true;
                this.refreshAssociationWithWheelBaseId(item.id, item.isSelected);
            });
            this.isSelectAllWheelBaseSystems = true;
        }

        // callback emitter
        this.onSearchEvent.emit(this.vehicleToWheelBaseForSelectedWheelBase);
    }

    // todo: make is modular, this method is at two location
    private refreshAssociationWithWheelBaseId(wheelBaseConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToWheelBaseRetrieved = this.getVehicleToWheelBaseByWheelBaseId(wheelBaseConfigId);
            //TODO: number of associations which may be useful in add Wheel BAse association screen?
            let temp = this.vehicleToWheelBaseForSelectedWheelBase || [];
            for (var vehicleToWheelBase of this.vehicleToWheelBaseRetrieved) {
                temp.push(vehicleToWheelBase);
            }
            this.vehicleToWheelBaseForSelectedWheelBase = temp;
        }
        else {
            let m = this.vehicleToWheelBaseForSelectedWheelBase.filter(x => x.wheelBaseId != wheelBaseConfigId);
            this.vehicleToWheelBaseForSelectedWheelBase = m;
        }
    }

    private getVehicleToWheelBaseByWheelBaseId(id) {
        return this.vehicleToWheelBaseSearchViewModel.result.vehicleToWheelBases.filter(v => v.wheelBaseId == id);
    }

    private search() {
        this.vehicleToWheelBaseSearchViewModel.searchType = SearchType.GeneralSearch;
        this.showLoadingGif = true;
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToWheelBaseSearchViewModel.facets.makes) {
            this.vehicleToWheelBaseSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.models) {
            this.vehicleToWheelBaseSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.subModels) {
            this.vehicleToWheelBaseSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(s => inputModel.subModels.push(s.name));
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes) {
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }
        if (this.vehicleToWheelBaseSearchViewModel.facets.regions) {
            this.vehicleToWheelBaseSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }
        this.vehicleToWheelBaseService.search(inputModel).subscribe(m => {
            if (m.result.wheelBases.length > 0 || m.result.vehicleToWheelBase.length > 0) {
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