import { Component, OnInit, ViewChild, Input,
    Output, EventEmitter }                              from "@angular/core";
import { Router }                    from "@angular/router";
import { ToastsManager }                                from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                                from "../shared/shared.service";
import { NavigationService }                            from "../shared/navigation.service";
import { ConstantsWarehouse }                           from "../constants-warehouse";
import { IVehicleToDriveTypeSearchViewModel,
    SearchType,
    IVehicleToDriveTypeSearchInputModel,
    IFacet }                                            from "../vehicleToDriveType/vehicleToDriveType-search.model";
import { VehicleToDriveTypeService }                  from "../vehicleToDriveType/vehicleToDriveType.service";
import { IVehicleToDriveType }                        from "../vehicleToDriveType/vehicleToDriveType.model";

@Component({
    selector: "vehicletodrivetype-searchpanel",
    templateUrl: "app/templates/vehicleToDriveType/vehicleToDriveType-searchPanel.component.html",
    providers: [SharedService, VehicleToDriveTypeService],
})

export class VehicleToDriveTypeSearchPanel {
    private makeQuery: string;
    private modelQuery: string;
    private subModelQuery: string;
    private regionFacet: IFacet[];
    private vehicleTypeFacet: IFacet[];
    private vehicleTypeGroupFacet: IFacet[] = [];
    private driveTypeFacet: IFacet[];
    private makeFacet: IFacet[];
    private modelFacet: IFacet[];
    private subModelFacet: IFacet[];
    private startYearFacet: string[];
    private endYearFacet: string[];
    private isSelectAllDriveTypes: boolean;
    private selectedStartYear: string;
    private selectedEndYear: string;
    private driveTypeId: string;
    private vehicleToDriveTypesRetrieved : IVehicleToDriveType[]=[];
    private showLoadingGif: boolean = false;

    @Input("thresholdRecordCount") thresholdRecordCount: number;
    @Input("vehicleToDriveTypesSearchViewModel") vehicleToDriveTypeSearchViewModel: IVehicleToDriveTypeSearchViewModel;
    @Input("vehicleToDriveTypesForSelectedBrake") vehicleToDriveTypeForSelectedDriveType: IVehicleToDriveType[];
    @Output("onSearchEvent") onSearchEvent = new EventEmitter<IVehicleToDriveType[]>();

    constructor(private sharedService: SharedService,
        private toastr: ToastsManager, private vehicleToDriveTypesService: VehicleToDriveTypeService) {
    }

    ngOnInit() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.driveTypeId = "";

        if (this.sharedService.vehicleToDriveTypeSearchViewModel != null) {
            this.vehicleToDriveTypeSearchViewModel.facets = this.sharedService.vehicleToDriveTypeSearchViewModel.facets;
            this.regionFacet = this.vehicleToDriveTypeSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleToDriveTypeSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleToDriveTypeSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleToDriveTypeSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleToDriveTypeSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleToDriveTypeSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.slice();
            this.driveTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.slice();
            if (this.sharedService.vehicleToDriveTypeSearchViewModel.searchType == SearchType.SearchByDriveTypeId) {
                this.searchByDriveTypeId();
            }
            else if (this.sharedService.vehicleToDriveTypeSearchViewModel.searchType == SearchType.GeneralSearch) {
                //this.search();
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
        this.driveTypeId = "";

        if (this.vehicleToDriveTypeSearchViewModel.facets.regions) {
            this.vehicleToDriveTypeSearchViewModel.facets.regions.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToDriveTypeSearchViewModel.facets.makes) {
            this.vehicleToDriveTypeSearchViewModel.facets.makes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToDriveTypeSearchViewModel.facets.models) {
            this.vehicleToDriveTypeSearchViewModel.facets.models.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToDriveTypeSearchViewModel.facets.subModels) {
            this.vehicleToDriveTypeSearchViewModel.facets.subModels.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes) {
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToDriveTypeSearchViewModel.facets.driveTypes) {
            this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.forEach(item => item.isSelected = false);
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
            driveTypes: []
        };
    }

    private refreshFacets() {
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToDriveTypeSearchViewModel.facets.regions) {
            this.vehicleToDriveTypeSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }

        if (this.vehicleToDriveTypeSearchViewModel.facets.makes) {
            this.vehicleToDriveTypeSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }

        if (this.vehicleToDriveTypeSearchViewModel.facets.models) {
            this.vehicleToDriveTypeSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }

        if (this.vehicleToDriveTypeSearchViewModel.facets.subModels) {
            this.vehicleToDriveTypeSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(m => inputModel.subModels.push(m.name));
        }

        if (this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }

        if (this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes) {
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }

        if (this.vehicleToDriveTypeSearchViewModel.facets.driveTypes) {
            this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.driveTypes.push(m.name));
        }
     
        this.showLoadingGif = true;
        this.vehicleToDriveTypesService.refreshFacets(inputModel).subscribe(data => {
            this.updateRegionFacet(data.facets.regions);
            this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
            this.updateVehicleTypeFacet(data.facets.vehicleTypes);
            this.updateYearFacet(data.facets.years);
            this.updateMakeFacet(data.facets.makes);
            this.updateModelFacet(data.facets.models, "");
            this.updateSubModelFacet(data.facets.subModels, "");
            this.updateDriveTypeFacet(data.facets.driveTypes);
           this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    private filterMakes($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.makes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.makeFacet = this.vehicleToDriveTypeSearchViewModel.facets.makes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterModels($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.models != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.modelFacet = this.vehicleToDriveTypeSearchViewModel.facets.models.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterSubModels($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.subModels != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.subModelFacet = this.vehicleToDriveTypeSearchViewModel.facets.subModels.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterRegions($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.regions != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.regionFacet = this.vehicleToDriveTypeSearchViewModel.facets.regions.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterVehicleTypeGroups($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeGroupFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterVehicleTypes($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterDriveType($event) {
        if (this.vehicleToDriveTypeSearchViewModel != null &&
            this.vehicleToDriveTypeSearchViewModel.facets != null &&
            this.vehicleToDriveTypeSearchViewModel.facets.driveTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.driveTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.filter(
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
        let existingSelectedRegions = this.vehicleToDriveTypeSearchViewModel.facets.regions.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToDriveTypeSearchViewModel.facets.regions = [];

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

            this.vehicleToDriveTypeSearchViewModel.facets.regions.push(newItem);
        }

        this.regionFacet = this.vehicleToDriveTypeSearchViewModel.facets.regions.slice();
    }

    private updateYearFacet(years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    }

    private updateMakeFacet(makes) {
        let existingSelectedMakes = this.vehicleToDriveTypeSearchViewModel.facets.makes.filter(make => make.isSelected).map(item => item.name);
        this.vehicleToDriveTypeSearchViewModel.facets.makes = [];

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

            this.vehicleToDriveTypeSearchViewModel.facets.makes.push(newMake);
        }

        this.makeFacet = this.vehicleToDriveTypeSearchViewModel.facets.makes.slice();
    }

    //TODO: makeName is not used
    private updateModelFacet(models, makeName) {
        let existingSelectedModels = this.vehicleToDriveTypeSearchViewModel.facets.models.filter(model => model.isSelected)
            .map(item => item.name);

        this.vehicleToDriveTypeSearchViewModel.facets.models = [];

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

            this.vehicleToDriveTypeSearchViewModel.facets.models.push(newModel);
        }

        this.modelFacet = this.vehicleToDriveTypeSearchViewModel.facets.models.slice();
    }

    private updateSubModelFacet(subModels, modelName) {
        let existingSelectedSubModels = this.vehicleToDriveTypeSearchViewModel.facets.subModels.filter(submodel => submodel.isSelected)
            .map(item => item.name);

        this.vehicleToDriveTypeSearchViewModel.facets.subModels = [];

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

            this.vehicleToDriveTypeSearchViewModel.facets.subModels.push(newSubModel);
        }

        this.subModelFacet = this.vehicleToDriveTypeSearchViewModel.facets.subModels.slice();
    }

    private updateVehicleTypeGroupFacet(vehicleTypeGroups) {
        let existingSelectedItems = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups = [];

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

            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }

        this.vehicleTypeGroupFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.slice();
    }

    private updateVehicleTypeFacet(vehicleTypes) {
        let existingSelectedItems = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes = [];

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

            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.push(newItem);
        }

        this.vehicleTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.slice();
    }

    private updateDriveTypeFacet(frontBrakeTypes) {
        let existingSelectedItems = this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToDriveTypeSearchViewModel.facets.driveTypes = [];

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

            this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.push(newItem);
        }

        this.driveTypeFacet = this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.slice();
    }

    private searchByDriveTypeId() {
        let driveTypeId = Number(this.driveTypeId);
        if (isNaN(driveTypeId)) {
            this.toastr.warning("Invalid Drive TypeIdId", ConstantsWarehouse.validationTitle);
            return;
        }

        this.vehicleToDriveTypeSearchViewModel.searchType = SearchType.SearchByDriveTypeId;
        this.showLoadingGif = true;
        this.vehicleToDriveTypesService.searchByDriveTypeId(driveTypeId).subscribe(m => {
            if (m.result.driveTypes.length > 0) {
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

    private onDriveTypesKeyPress(event) {
        if (event.keyCode == 13) {
            this.searchByDriveTypeId();
        }
    }

    private getSearchResult(m: IVehicleToDriveTypeSearchViewModel) {
        this.vehicleToDriveTypeSearchViewModel.result = m.result;

        this.vehicleToDriveTypeSearchViewModel.totalCount = m.totalCount;

        this.vehicleToDriveTypeForSelectedDriveType = [];
        this.isSelectAllDriveTypes = false;

        // note: select all when totalRecords <= threshold
        if (this.vehicleToDriveTypeSearchViewModel.result.driveTypes.length <= this.thresholdRecordCount) {
            this.vehicleToDriveTypeSearchViewModel.result.driveTypes.forEach(item => {
                item.isSelected = true;
                this.refreshAssociationWithDriveTypesId(item.id, item.isSelected);
            });
            this.isSelectAllDriveTypes = true;
        }

        // callback emitter
        this.onSearchEvent.emit(this.vehicleToDriveTypeForSelectedDriveType);
    }

    // todo: make is modular, this method is at two location
    private refreshAssociationWithDriveTypesId(brakeConfigId, isSelected) {
        if (isSelected) {
            this.vehicleToDriveTypesRetrieved = this.getVehicleToDriveTypesByDriveTypesId(brakeConfigId);
            //TODO: number of associations which may be useful in add brake association screen?
            let temp = this.vehicleToDriveTypeForSelectedDriveType || [];
            for (var vehicleToDriveTypes of this.vehicleToDriveTypesRetrieved) {
                temp.push(vehicleToDriveTypes);
            }
            this.vehicleToDriveTypeForSelectedDriveType = temp;
        }
        else {
            let m = this.vehicleToDriveTypeForSelectedDriveType.filter(x => x.driveType.id != brakeConfigId);
            this.vehicleToDriveTypeForSelectedDriveType = m;
        }
    }

    private getVehicleToDriveTypesByDriveTypesId(id) {
        return this.vehicleToDriveTypeSearchViewModel.result.vehicleToDriveTypes.filter(v => v.driveType.id == id);
    }

    private search() {
        this.vehicleToDriveTypeSearchViewModel.searchType = SearchType.GeneralSearch;
        this.showLoadingGif = true;
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToDriveTypeSearchViewModel.facets.driveTypes) {
            this.vehicleToDriveTypeSearchViewModel.facets.driveTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.driveTypes.push(m.name));
        }
      
        if (this.vehicleToDriveTypeSearchViewModel.facets.makes) {
            this.vehicleToDriveTypeSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.models) {
            this.vehicleToDriveTypeSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.subModels) {
            this.vehicleToDriveTypeSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(s => inputModel.subModels.push(s.name));
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes) {
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToDriveTypeSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }
        if (this.vehicleToDriveTypeSearchViewModel.facets.regions) {
            this.vehicleToDriveTypeSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }
        this.vehicleToDriveTypesService.search(inputModel).subscribe(m => {
            if (m.result.driveTypes.length > 0 || m.result.vehicleToDriveTypes.length > 0) {
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
