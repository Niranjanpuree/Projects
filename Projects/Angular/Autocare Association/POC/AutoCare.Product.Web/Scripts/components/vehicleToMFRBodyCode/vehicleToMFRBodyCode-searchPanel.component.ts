import { Component, OnInit, ViewChild, Input,
    Output, EventEmitter }                              from "@angular/core";
import { Router }                    from "@angular/router";
import { ToastsManager }                                from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                                from "../shared/shared.service";
import { NavigationService }                            from "../shared/navigation.service";
import { ConstantsWarehouse }                           from "../constants-warehouse";
import { IVehicleToMfrBodyCodeSearchViewModel,
    SearchType,
    IVehicleToMfrBodyCodeSearchInputModel,
    IFacet }                                            from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode-search.model";
import { VehicleToMfrBodyCodeService }                  from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode.service";
import { IVehicleToMfrBodyCode }                        from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode.model";

@Component({
    selector: "vehicletomfrbodycode-searchpanel",
    templateUrl: "app/templates/vehicleToMfrBodyCode/vehicleToMfrBodyCode-searchPanel.component.html",
    providers: [VehicleToMfrBodyCodeService],
})

export class VehicleToMfrBodyCodeSearchPanel {
    private makeQuery: string;
    private modelQuery: string;
    private subModelQuery: string;
    private regionFacet: IFacet[];
    private vehicleTypeFacet: IFacet[];
    private vehicleTypeGroupFacet: IFacet[] = [];
    //private mfrBodyCodeFacet: IFacet[];
    private makeFacet: IFacet[];
    private modelFacet: IFacet[];
    private subModelFacet: IFacet[];
    private startYearFacet: string[];
    private endYearFacet: string[];
    private isSelectAllMfrBodyCodes: boolean;
    private selectedStartYear: string;
    private selectedEndYear: string;
    private mfrBodyCodeId: string;
    private vehicleToMfrBodyCodesRetrieved: IVehicleToMfrBodyCode[]=[];
    private showLoadingGif: boolean = false;

    @Input("thresholdRecordCount")
    thresholdRecordCount: number;
    @Input("vehicleToMfrBodyCodesSearchViewModel")
    vehicleToMfrBodyCodesSearchViewModel: IVehicleToMfrBodyCodeSearchViewModel; 
    @Input("vehicleToMfrBodyCodesForSelectedMfrBodyCode")
    vehicleToMfrBodyCodesForSelectedMfrBodyCode: IVehicleToMfrBodyCode[];
    @Output("onSearchEvent")
    onSearchEvent = new EventEmitter<IVehicleToMfrBodyCode[]>();

    constructor(private sharedService: SharedService,
        private toastr: ToastsManager, private vehicleToMfrBodyCodesService: VehicleToMfrBodyCodeService) {
    }

    ngOnInit() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.mfrBodyCodeId = "";

        if (this.sharedService.vehicleToMfrBodyCodeSearchViewModel != null) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets = this.sharedService.vehicleToMfrBodyCodeSearchViewModel.facets;
            this.regionFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.slice();
            if (this.sharedService.vehicleToMfrBodyCodeSearchViewModel.searchType == SearchType.SearchByMfrBodyCodeId) {
                this.searchByMfrBodyCodeId();
            } else if (this.sharedService.vehicleToMfrBodyCodeSearchViewModel.searchType == SearchType.GeneralSearch) {
                 this.search();
            } else {
                this.showLoadingGif = false;
            }
        } else {
            this.refreshFacets();
        }
    }

    private onClearFilters() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.mfrBodyCodeId = "";

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.regions) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.makes) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.models) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.models.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.forEach(item => item.isSelected = false);
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
            mfrBodyCodes: [],
        };
    }

    private refreshFacets() {
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.regions) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.makes) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.models) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(m => inputModel.subModels.push(m.name));
        }

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }


        this.showLoadingGif = true;
        this.vehicleToMfrBodyCodesService.refreshFacets(inputModel).subscribe(data => {
            this.updateRegionFacet(data.facets.regions);
            this.updateVehicleTypeGroupFacet(data.facets.vehicleTypeGroups);
            this.updateVehicleTypeFacet(data.facets.vehicleTypes);
            this.updateYearFacet(data.facets.years);
            this.updateMakeFacet(data.facets.makes);
            this.updateModelFacet(data.facets.models, "");
            this.updateSubModelFacet(data.facets.subModels, "");
            //this.updateMfrBodyCodeFacet(data.facets.mfrBodyCodes);
            this.showLoadingGif = false;
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    private filterMakes($event) {
        if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets.makes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.makeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.filter(
                item => item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterModels($event) {
        if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets.models != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.modelFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.models.filter(
                item => item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterSubModels($event) {
        if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.subModelFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.filter(
                item => item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterRegions($event) {
        if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets.regions != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.regionFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.filter(
                item => item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterVehicleTypeGroups($event) {
        if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeGroupFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.filter(
                item => item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    private filterVehicleTypes($event) {
        if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.filter(
                item => item.name.toLocaleLowerCase()
                .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    //private filterMfrBodyCode($event) {
    //    if (this.vehicleToMfrBodyCodesSearchViewModel != null &&
    //        this.vehicleToMfrBodyCodesSearchViewModel.facets != null &&
    //        this.vehicleToMfrBodyCodesSearchViewModel.facets.mfrBodyCodes != null) {
    //        var inputElement = <HTMLInputElement>$event.target;
    //        this.mfrBodyCodeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.mfrBodyCodes.filter(
    //            item => item.name.toLocaleLowerCase()
    //            .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
    //    }
    //}


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
        let existingSelectedRegions = this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToMfrBodyCodesSearchViewModel.facets.regions = [];

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

            this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.push(newItem);
        }

        this.regionFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.slice();
    }

    private updateYearFacet(years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    }

    private updateMakeFacet(makes) {
        let existingSelectedMakes = this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.filter(make => make.isSelected).map(item => item.name);
        this.vehicleToMfrBodyCodesSearchViewModel.facets.makes = [];

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

            this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.push(newMake);
        }

        this.makeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.slice();
    }

    //TODO: makeName is not used
    private updateModelFacet(models, makeName) {
        let existingSelectedModels = this.vehicleToMfrBodyCodesSearchViewModel.facets.models.filter(model => model.isSelected)
            .map(item => item.name);

        this.vehicleToMfrBodyCodesSearchViewModel.facets.models = [];

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

            this.vehicleToMfrBodyCodesSearchViewModel.facets.models.push(newModel);
        }

        this.modelFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.models.slice();
    }

    private updateSubModelFacet(subModels, modelName) {
        let existingSelectedSubModels = this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.filter(submodel => submodel.isSelected)
            .map(item => item.name);

        this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels = [];

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

            this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.push(newSubModel);
        }

        this.subModelFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.slice();
    }

    private updateVehicleTypeGroupFacet(vehicleTypeGroups) {
        let existingSelectedItems = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups = [];

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

            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }

        this.vehicleTypeGroupFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.slice();
    }

    private updateVehicleTypeFacet(vehicleTypes) {
        let existingSelectedItems = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes = [];

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

            this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.push(newItem);
        }

        this.vehicleTypeFacet = this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.slice();
    }
    private searchByMfrBodyCodeId() {
        let mfrBodyCodeId = Number(this.mfrBodyCodeId);
        if (isNaN(mfrBodyCodeId)) {
            this.toastr.warning("Invalid Mfr Body Code Id", ConstantsWarehouse.validationTitle);
            return;
        }

        this.vehicleToMfrBodyCodesSearchViewModel.searchType = SearchType.SearchByMfrBodyCodeId;
        this.showLoadingGif = true;
        this.vehicleToMfrBodyCodesService.searchByMfrBodyCodeId(mfrBodyCodeId).subscribe(m => {
            if (m.result.mfrBodyCodes.length > 0) {
                this.getSearchResult(<any>m);
                this.showLoadingGif = false;
                $(".drawer-left").css('width', 15);
            } else {
                this.toastr.warning("The search yeilded no result", "No Record Found!!");
                this.showLoadingGif = false;
            }
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    private onMfrBodyCodesKeyPress(event) {
        if (event.keyCode == 13) {
            this.searchByMfrBodyCodeId();
        }
    }
    private getSearchResult(m: IVehicleToMfrBodyCodeSearchViewModel) {
        this.vehicleToMfrBodyCodesSearchViewModel.result = m.result;

        this.vehicleToMfrBodyCodesSearchViewModel.totalCount = m.totalCount;

        this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = [];
        this.isSelectAllMfrBodyCodes = false;

        // note: select all when totalRecords <= threshold
        if (this.vehicleToMfrBodyCodesSearchViewModel.result.mfrBodyCodes.length <= this.thresholdRecordCount) {
            this.vehicleToMfrBodyCodesSearchViewModel.result.mfrBodyCodes.forEach(item => {
                item.isSelected = true;
                this.refreshAssociationWithMfrBodyCodeId(item.id, item.isSelected);
            });
            this.isSelectAllMfrBodyCodes = true;
        }

        // callback emitter
        this.onSearchEvent.emit(this.vehicleToMfrBodyCodesForSelectedMfrBodyCode);
    }

    private refreshAssociationWithMfrBodyCodeId(mfrBodyCodeId, isSelected) {
        if (isSelected) {
            this.vehicleToMfrBodyCodesRetrieved = this.getVehicleToMfrBodyCodesByMfrBodyCodeId(mfrBodyCodeId);
            //TODO: number of associations which may be useful in add brake association screen?
            let temp = this.vehicleToMfrBodyCodesForSelectedMfrBodyCode || [];
            for (var vehicleToMfrBodyCode of this.vehicleToMfrBodyCodesRetrieved) {
                temp.push(vehicleToMfrBodyCode);
            }
            this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = temp;
        }
        else {
            let m = this.vehicleToMfrBodyCodesForSelectedMfrBodyCode.filter(x => x.mfrBodyCode.id != mfrBodyCodeId);
            this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = m;
        }
    }
    private getVehicleToMfrBodyCodesByMfrBodyCodeId(id) {
        return this.vehicleToMfrBodyCodesSearchViewModel.result.vehicleToMfrBodyCodes.filter(v => v.mfrBodyCode.id == id);
    }
    private search() {
        this.vehicleToMfrBodyCodesSearchViewModel.searchType = SearchType.GeneralSearch;
        this.showLoadingGif = true;
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToMfrBodyCodesSearchViewModel.facets.mfrBodyCodes) {
            this.vehicleToMfrBodyCodesSearchViewModel.facets.mfrBodyCodes.filter(item => item.isSelected)
                .forEach(m => inputModel.mfrBodyCodes.push(m.name));

            if (this.vehicleToMfrBodyCodesSearchViewModel.facets.makes) {
                this.vehicleToMfrBodyCodesSearchViewModel.facets.makes.filter(item => item.isSelected)
                    .forEach(m => inputModel.makes.push(m.name));
            }
            if (this.vehicleToMfrBodyCodesSearchViewModel.facets.models) {
                this.vehicleToMfrBodyCodesSearchViewModel.facets.models.filter(item => item.isSelected)
                    .forEach(m => inputModel.models.push(m.name));
            }
            if (this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels) {
                this.vehicleToMfrBodyCodesSearchViewModel.facets.subModels.filter(item => item.isSelected)
                    .forEach(s => inputModel.subModels.push(s.name));
            }
            if (this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes) {
                this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                    .forEach(m => inputModel.vehicleTypes.push(m.name));
            }
            if (this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups) {
                this.vehicleToMfrBodyCodesSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                    .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
            }
            if (this.vehicleToMfrBodyCodesSearchViewModel.facets.regions) {
                this.vehicleToMfrBodyCodesSearchViewModel.facets.regions.filter(item => item.isSelected)
                    .forEach(m => inputModel.regions.push(m.name));
            }
            this.vehicleToMfrBodyCodesService.search(inputModel).subscribe(m => {
                if (m.result.mfrBodyCodes.length > 0 || m.result.vehicleToMfrBodyCodes.length > 0) {
                    this.getSearchResult(<any>m);
                    this.showLoadingGif = false;
                    $(".drawer-left").css('width', 15);
                } else {
                    this.toastr.warning("The search yeilded no result", "No Record Found!!");
                    this.showLoadingGif = false;
                }
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
        }
    }

    private clearFacet(facet: IFacet[], refresh: boolean = true) {
        if (facet) {
            facet.forEach(item => item.isSelected = false);
        }
        if (refresh) {
            this.refreshFacets();
        }
    }
    private onMfrBodyCodeKeyPress(event) {
        if (event.keyCode == 13) {
            this.searchByMfrBodyCodeId();
        }
    }
}