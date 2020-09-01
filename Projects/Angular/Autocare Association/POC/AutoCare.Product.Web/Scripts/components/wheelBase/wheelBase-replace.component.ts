import { Component, OnInit, ViewChild }           from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { ToastsManager }                          from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { IWheelBase }                           from "./wheelBase.model";
import { WheelBaseService }                     from "./wheelBase.service";
import { ConstantsWarehouse }                     from "../constants-warehouse";
import { VehicleToWheelBaseService }            from "../vehicleToWheelBase/vehicleToWheelBase.service";
import { AcGridComponent }                        from '../../lib/aclibs/ac-grid/ac-grid';
import {IVehicleToWheelBase }                   from"../vehicleToWheelBase/vehicleToWheelBase.model"
import {
    IVehicleToWheelBaseSearchInputModel,
    IVehicleToWheelBaseSearchViewModel,
    IFacet }                                    from "../vehicleToWheelBase/vehicleToWheelBase-search.model";

@Component({
    selector: "wheelBase-replace-component",
    templateUrl: "app/templates/wheelBase/wheelBase-replace.component.html",
    providers: [VehicleToWheelBaseService],
})

export class WheelBaseReplaceComponent implements OnInit {
    public existingWheelBase: IWheelBase;
    public replacementWheelBase: IWheelBase;
    public wheelBases: IWheelBase[];
    public wheelBaseIdSearchText: string;
    showLoadingGif: boolean = false;
    isSelectAllVehicleToWheelBase: boolean;

    vehicleToWheelBaseSearchViewModel: IVehicleToWheelBaseSearchViewModel;
    regionFacet: IFacet[];
    vehicleTypeFacet: IFacet[];
    vehicleTypeGroupFacet: IFacet[] = [];
    makeFacet: IFacet[];
    modelFacet: IFacet[];
    subModelFacet: IFacet[];
    startYearFacet: string[];
    endYearFacet: string[];
    selectedStartYear: string;
    selectedEndYear: string;
    private thresholdRecordCount: number = 100000;  //NOTE: keep this number large so that "select all" checkbox always appears
    @ViewChild("vehicleToWheelBaseGrid") vehicleToWheelBaseGrid: AcGridComponent;

    constructor(private wheelBaseService: WheelBaseService, private router: Router,
        private route: ActivatedRoute, private toastr: ToastsManager,
        private vehicleToWheelBaseService: VehicleToWheelBaseService) {

        this.replacementWheelBase = {
            id: -1,
        };
    }

    ngOnInit() {
        this.showLoadingGif = true;
        this.isSelectAllVehicleToWheelBase = false;
        // Load existing wheel base with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        this.wheelBaseService.getWheelBaseDetail(id).subscribe(result => {
            this.existingWheelBase = result;
            this.wheelBaseService.getAllWheelBase().subscribe(m => {
                this.wheelBases = m;

                this.refreshFacets();
            });
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });

        this.vehicleToWheelBaseSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: []
            }
            , result: { wheelBases: [], vehicleToWheelBases: [] }
        };

        this.selectedStartYear = "0";
        this.selectedEndYear = "0";

        // Drawer right start
        var headerht = $('header').innerHeight();
        var navht = $('nav').innerHeight();
        var winht = $(window).height();
        var winwt = 800;

        $(".drawer-left").css('min-height', winht - headerht - navht);
        $(".drawer-left").css('width', 15); //start is collapsed state

        $(document).on('click', '.drawer-show', function (event) {
            $(".drawer-left").css('width', winwt);
        });

        $(".drawer-left span").on('click', function () {

            var drawerwt = $(".drawer-left").width();

            if (drawerwt == 15) {
                $(".drawer-left").css('width', winwt);
            }
            else {
                $(".drawer-left").css('width', 15);
            }
        });

        $(document).on('click', function (event) {
            if (!$(event.target).closest(".drawer-left").length) {
                // Hide the menus.
                var drawerwt = $(".drawer-left").width();
                if (drawerwt > 15) {
                    $(".drawer-left").css('width', 15);
                }
            }
        });
        // Drawer right end
    }

    // validation
    private validatereplacementWheelBase(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (Number(this.replacementWheelBase.id) < 1) {
            this.toastr.warning("Please select replacement Wheel Base.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingWheelBase.id) === Number(this.replacementWheelBase.id)) {
            this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingWheelBase.vehicleToWheelBases != undefined) {
            if (this.existingWheelBase.vehicleToWheelBases.filter(item => item.isSelected).length <= 0) {
                this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
                isValid = false;
            }
        }
        else if (this.existingWheelBase.vehicleToWheelBases == undefined) {
            this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    onWheelBaseIdKeyPress(event) {
        if (Number(event.keyCode) === 13) {
            this.onWheelBaseIdSearch();
        }
    }

    onWheelBaseIdSearch() {
        let wheelBaseId: number = Number(this.wheelBaseIdSearchText);
        if (isNaN(wheelBaseId) || wheelBaseId <= 0) {
            this.toastr.warning("Invalid Wheel Base Id.", ConstantsWarehouse.validationTitle);
            return;
        }

        this.replacementWheelBase.id = -1;

        this.showLoadingGif = true;
        this.wheelBaseService.getWheelBaseDetail(Number(this.wheelBaseIdSearchText)).subscribe(result => {
            this.replacementWheelBase = result;

            this.showLoadingGif = false;
        }, error => {
            let errorMessage: string = JSON.parse(String(error)).message;
            this.toastr.warning(errorMessage, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    onSelectAllVehicleToWheelBase(isSelected) {
        this.isSelectAllVehicleToWheelBase = isSelected;
        if (this.existingWheelBase.vehicleToWheelBases == null) {
            return;
        }
        this.existingWheelBase.vehicleToWheelBases.forEach(item => item.isSelected = isSelected);
    }

    onClearFilters() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.refreshFacets();
    }

    getDefaultInputModel(): IVehicleToWheelBaseSearchInputModel {
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

    refreshFacets() {
        let inputModel = this.getDefaultInputModel();
        inputModel.wheelBaseId = this.existingWheelBase.id;
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

    filterMakes($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.makes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.makeFacet = this.vehicleToWheelBaseSearchViewModel.facets.makes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterModels($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.models != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.modelFacet = this.vehicleToWheelBaseSearchViewModel.facets.models.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterSubModels($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.subModels != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.subModelFacet = this.vehicleToWheelBaseSearchViewModel.facets.subModels.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterRegions($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.regions != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.regionFacet = this.vehicleToWheelBaseSearchViewModel.facets.regions.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterVehicleTypeGroups($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeGroupFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypeGroups.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterVehicleTypes($event) {
        if (this.vehicleToWheelBaseSearchViewModel != null &&
            this.vehicleToWheelBaseSearchViewModel.facets != null &&
            this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToWheelBaseSearchViewModel.facets.vehicleTypes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    onYearSelected() {
        this.refreshFacets();
    }

    onItemSelected(event, facet: IFacet[]) {
        let isChecked = event.target.checked;
        let selectedItem = facet.filter(item => item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase())[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    }

    updateRegionFacet(regions) {
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

    updateYearFacet(years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    }

    updateMakeFacet(makes) {
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
    updateModelFacet(models, makeName) {
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

    updateSubModelFacet(subModels, modelName) {
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

    updateVehicleTypeGroupFacet(vehicleTypeGroups) {
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

    updateVehicleTypeFacet(vehicleTypes) {
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


    searchVehicleToWheelBases() {
        this.showLoadingGif = true;

        let inputModel = this.getDefaultInputModel();
        inputModel.wheelBaseId = this.existingWheelBase.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        //NOTE: inputModel.wheelBaseId = this.existingWheelBase.id; should be sufficient here

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

        this.vehicleToWheelBaseService.getAssociations(inputModel).subscribe(result => {
            if (result.length > 0) {
                this.existingWheelBase.vehicleToWheelBases = result;
                this.existingWheelBase.vehicleToWheelBaseCount = result.length;

                this.isSelectAllVehicleToWheelBase = false;

                if (this.vehicleToWheelBaseGrid)
                    this.vehicleToWheelBaseGrid.refresh();

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

    // event on view affected vehicle associations
    onViewAffectedAssociations() {
        $(".drawer-left").css('width', 800); //show the filter panel to enable search
    }

    clearAllFacets() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";

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

    clearFacet(facet: IFacet[], refresh: boolean = true) {
        if (facet) {
            facet.forEach(item => item.isSelected = false);
        }
        if (refresh) {
            this.refreshFacets();
        }
    }

    onVehicleToWheelBaseSelected(vechicleToWheelBase: IVehicleToWheelBase) {
        if (vechicleToWheelBase.isSelected) {
            //unchecked
            this.isSelectAllVehicleToWheelBase = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingWheelBase.vehicleToWheelBases.filter(item => item.id != vechicleToWheelBase.id);
            if (excludedVehicle.every(item => item.isSelected)) {
                this.isSelectAllVehicleToWheelBase = true;
            }
        }
    }

    // event on continue
    onContinue() {
        // validate
        if (this.validatereplacementWheelBase()) {
            // set data in factory/ service

            this.wheelBaseService.existingWheelBase = this.existingWheelBase;
            this.wheelBaseService.existingWheelBase.vehicleToWheelBases = this.existingWheelBase.vehicleToWheelBases.filter(item => item.isSelected);
            this.replacementWheelBase.base = this.wheelBases.filter(item => item.id === Number(this.replacementWheelBase.id))[0].base;
            this.replacementWheelBase.wheelBaseMetric = this.wheelBases.filter(item => item.id === Number(this.replacementWheelBase.id))[0].wheelBaseMetric;
            this.wheelBaseService.replacementWheelBase = this.replacementWheelBase;
            // redirect to Confirm page.
            this.router.navigateByUrl("/wheelbase/replace/confirm/" + this.existingWheelBase.id);

        } else {
            //console.log(this.errorMessage);
        }
    }
}