import { Component, OnInit, ViewChild }           from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { ToastsManager }                          from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {  ModalComponent }       from "ng2-bs3-modal/ng2-bs3-modal";
import { IMfrBodyCode }                           from "./mfrBodyCode.model";
import { MfrBodyCodeService }                           from "./mfrBodyCode.service";
import { VehicleToMfrBodyCodeService }                           from "../vehicletomfrbodycode/vehicletomfrbodycode.service";
import { IVehicleToMfrBodyCode }                           from "../vehicletomfrbodycode/vehicletomfrbodycode.model";
import { IVehicleToMfrBodyCodeSearchViewModel, IFacet }            from "../vehicletomfrbodyCode/vehicletomfrbodyCode-search.model";
import { AcGridComponent }                        from '../../lib/aclibs/ac-grid/ac-grid';      
import { ConstantsWarehouse }                     from "../constants-warehouse";                           

@Component({
    selector: "mfrBodyCode-replace-component",
    templateUrl: "app/templates/mfrBodyCode/mfrBodyCode-replace.component.html",
    providers: [VehicleToMfrBodyCodeService]
})


export class MfrBodyCodeReplaceComponent implements OnInit {
    public existingMfrBodyCode: IMfrBodyCode;
    public replaceMfrBodyCode: IMfrBodyCode;
    public mfrBodyCodeIdSearchText: string;
    showLoadingGif: boolean = false;
    isSelectAllVehicleToMfrBodyCode: boolean;
    vehicleToMfrBodyCodeSearchViewModel: IVehicleToMfrBodyCodeSearchViewModel;
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
    private mfrBodyCodes: IMfrBodyCode[] = [];    
    private thresholdRecordCount: number = 100000;  //NOTE: keep this number large so that "select all" checkbox always appears
    @ViewChild("vehicleToMfrBodyCodeGrid") vehicleToMfrBodyCodeGrid: AcGridComponent;

    constructor(private mfrBodyCodeService: MfrBodyCodeService,
        private router: Router,
        private route: ActivatedRoute, private toastr: ToastsManager,
        private vehicleToMfrBodyCodeService: VehicleToMfrBodyCodeService) {
        // initialize empty MfrBOdyCode
        this.replaceMfrBodyCode = {
            id: -1
        };
    }

    ngOnInit() {
        this.showLoadingGif = true;
        this.isSelectAllVehicleToMfrBodyCode = false;
        // Load existing MfrBOdyCode config with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        this.mfrBodyCodes = [];
        this.mfrBodyCodeService.getMfrBodyCode(id).subscribe(result => {
            this.existingMfrBodyCode = result;
            this.mfrBodyCodeService.getMfrBodyCodes().subscribe(mbc => {
                this.mfrBodyCodes = mbc;
                this.refreshFacets();
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
        this.vehicleToMfrBodyCodeSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: [],
                mfrBodyCodes: [],
               
            }
            , result: { mfrBodyCodes: [], vehicleToMfrBodyCodes: [] }
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
    private validateReplaceMfrBodyCode(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (Number(this.replaceMfrBodyCode.id) === -1) {
            this.toastr.warning("Please select Mfr Body Code.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceMfrBodyCode.id) < 1) {
            this.toastr.warning("Please select replacement Mfr Body Code.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingMfrBodyCode.id) === Number(this.replaceMfrBodyCode.id)) {
            this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingMfrBodyCode.vehicleToMfrBodyCodes.filter(item => item.isSelected).length <= 0) {
            this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    // event on model change in selection of MfrBOdyCode
    onChangeMfrBodyCode(mfrBodyCodeId: number) {
        if (mfrBodyCodeId > 0)
            this.replaceMfrBodyCode.id = mfrBodyCodeId;
        else
            this.replaceMfrBodyCode.id = 0;
    }
   
    
    onMfrBodyCodeIdKeyPress(event) {
        if (Number(event.keyCode) === 13) {
            this.onMfrBodyCodeIdSearch();
        }
    }

    // event on MfrBodyCode config id search
    onMfrBodyCodeIdSearch() {
        let mfrBodyCodeId: number = Number(this.mfrBodyCodeIdSearchText);
        if (isNaN(mfrBodyCodeId) || mfrBodyCodeId <= 0) {
            this.toastr.warning("Invalid Mfr Body Code Id.", ConstantsWarehouse.validationTitle);
            return;
        }
        this.replaceMfrBodyCode.id = -1;
        this.showLoadingGif = true;
        this.mfrBodyCodeService.getMfrBodyCode(Number(this.mfrBodyCodeIdSearchText)).subscribe(result => {
            this.replaceMfrBodyCode = result
            this.showLoadingGif = false;
           
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    onSelectAllVehicleToMfrBodyCode(isSelected) {
        this.isSelectAllVehicleToMfrBodyCode = isSelected;
        if (this.existingMfrBodyCode.vehicleToMfrBodyCodes == null) {
            return;
        }
        this.existingMfrBodyCode.vehicleToMfrBodyCodes.forEach(item => item.isSelected = isSelected);
    }

    onClearFilters() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.refreshFacets();
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
            mfrBodyCodes:[]
        };
    }

    refreshFacets() {
        let inputModel = this.getDefaultInputModel();
        inputModel.mfrBodyCodeId = this.existingMfrBodyCode.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.regions) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.makes) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.models) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(m => inputModel.subModels.push(m.name));
        }

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }

        this.showLoadingGif = true;
        this.vehicleToMfrBodyCodeService.refreshFacets(inputModel).subscribe(data => {
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
        if (this.vehicleToMfrBodyCodeSearchViewModel != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets.makes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.makeFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterModels($event) {
        if (this.vehicleToMfrBodyCodeSearchViewModel != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets.models != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.modelFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.models.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterSubModels($event) {
        if (this.vehicleToMfrBodyCodeSearchViewModel != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.subModelFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterRegions($event) {
        if (this.vehicleToMfrBodyCodeSearchViewModel != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets.regions != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.regionFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterVehicleTypeGroups($event) {
        if (this.vehicleToMfrBodyCodeSearchViewModel != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeGroupFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterVehicleTypes($event) {
        if (this.vehicleToMfrBodyCodeSearchViewModel != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets != null &&
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.filter(
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
        let existingSelectedRegions = this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToMfrBodyCodeSearchViewModel.facets.regions = [];

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

            this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.push(newItem);
        }

        this.regionFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.slice();
    }

    updateYearFacet(years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    }

    updateMakeFacet(makes) {
        let existingSelectedMakes = this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.filter(make => make.isSelected).map(item => item.name);
        this.vehicleToMfrBodyCodeSearchViewModel.facets.makes = [];

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

            this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.push(newMake);
        }

        this.makeFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.slice();
    }

    //TODO: makeName is not used
    updateModelFacet(models, makeName) {
        let existingSelectedModels = this.vehicleToMfrBodyCodeSearchViewModel.facets.models.filter(model => model.isSelected)
            .map(item => item.name);

        this.vehicleToMfrBodyCodeSearchViewModel.facets.models = [];

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

            this.vehicleToMfrBodyCodeSearchViewModel.facets.models.push(newModel);
        }

        this.modelFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.models.slice();
    }

    updateSubModelFacet(subModels, modelName) {
        let existingSelectedSubModels = this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.filter(submodel => submodel.isSelected)
            .map(item => item.name);

        this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels = [];

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

            this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.push(newSubModel);
        }

        this.subModelFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.slice();
    }

    updateVehicleTypeGroupFacet(vehicleTypeGroups) {
        let existingSelectedItems = this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups = [];

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

            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }

        this.vehicleTypeGroupFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.slice();
    }

    updateVehicleTypeFacet(vehicleTypes) {
        let existingSelectedItems = this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected).map(item => item.name);
        this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes = [];

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

            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.push(newItem);
        }

        this.vehicleTypeFacet = this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.slice();
    }


    searchVehicleToMfrBodyCodes() {
        this.showLoadingGif = true;

        let inputModel = this.getDefaultInputModel();
        inputModel.mfrBodyCodeId = this.existingMfrBodyCode.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.makes) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.models) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(s => inputModel.subModels.push(s.name));
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }
        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.regions) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }

        this.vehicleToMfrBodyCodeService.getAssociations(inputModel).subscribe(result => {
            if (result.length > 0) {
                this.existingMfrBodyCode.vehicleToMfrBodyCodes = result;
                this.existingMfrBodyCode.vehicleToMfrBodyCodeCount = result.length;

                this.isSelectAllVehicleToMfrBodyCode= false;

                if (this.vehicleToMfrBodyCodeGrid)
                    this.vehicleToMfrBodyCodeGrid.refresh();

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

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.regions) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.regions.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.makes) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.makes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.models) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.models.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.subModels.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypeGroups.forEach(item => item.isSelected = false);
        }

        if (this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes) {
            this.vehicleToMfrBodyCodeSearchViewModel.facets.vehicleTypes.forEach(item => item.isSelected = false);
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

    onVehicleToMfrBodyCodeSelected(vechicleToMfrBodyCode: IVehicleToMfrBodyCode) {
        if (vechicleToMfrBodyCode.isSelected) {
            //unchecked
            this.isSelectAllVehicleToMfrBodyCode = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingMfrBodyCode.vehicleToMfrBodyCodes.filter(item => item.id != vechicleToMfrBodyCode.id);
            if (excludedVehicle.every(item => item.isSelected)) {
                this.isSelectAllVehicleToMfrBodyCode = true;
            }
        }
    }

    // event on continue
    onContinue() {
        if (this.validateReplaceMfrBodyCode()) {

            this.mfrBodyCodeService.existingMfrBodyCode = this.existingMfrBodyCode;
            this.mfrBodyCodeService.existingMfrBodyCode.vehicleToMfrBodyCodes = this.existingMfrBodyCode.vehicleToMfrBodyCodes.filter(item => item.isSelected);

            this.replaceMfrBodyCode.name = this.mfrBodyCodes.filter(item => item.id === Number(this.replaceMfrBodyCode.id))[0].name;
            this.mfrBodyCodeService.replacementMfrBodyCode = this.replaceMfrBodyCode;
            this.router.navigateByUrl("/mfrbodycode/replace/confirm/" + this.existingMfrBodyCode.id);
        }
    }
}
