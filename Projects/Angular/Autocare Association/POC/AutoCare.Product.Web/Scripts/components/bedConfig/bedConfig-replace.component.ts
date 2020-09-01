import { Component, OnInit, ViewChild }           from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { ToastsManager }                          from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ModalComponent }       from "ng2-bs3-modal/ng2-bs3-modal";
import { IBedConfig }                           from "./bedConfig.model";
import { BedTypeService }                       from "../bedType/bedType.service";
import { IBedType }                             from "../bedType/bedType.model";
import { BedLengthService }                        from "../bedLength/bedLength.service";
import { IBedLength }                              from "../bedLength/bedLength.model";
import { BedConfigService }                     from "./bedConfig.service";
import { ConstantsWarehouse }                     from "../constants-warehouse";
import { VehicleToBedConfigService }            from "../vehicleToBedConfig/vehicleToBedConfig.service";
import { AcGridComponent }                        from '../../lib/aclibs/ac-grid/ac-grid';
import {IVehicleToBedConfig }                   from"../vehicleToBedConfig/vehicleToBedConfig.model"
import {
    IVehicleToBedConfigSearchInputModel,
    IVehicleToBedConfigSearchViewModel,
    IFacet }                                    from "../vehicleToBedConfig/vehicleToBedConfig-search.model";

@Component({
    selector: "bedConfig-replace-component",
    templateUrl: "app/templates/bedConfig/bedConfig-replace.component.html",
    providers: [VehicleToBedConfigService],
})

export class BedConfigReplaceComponent implements OnInit {
    public existingBedConfig: IBedConfig;
    public replaceBedConfig: IBedConfig;
    public bedTypes: IBedType[];
    public bedLengths: IBedLength[];
    public bedConfigIdSearchText: string;
    showLoadingGif: boolean = false;
    isSelectAllVehicleToBedConfig: boolean;

    vehicleToBedConfigSearchViewModel: IVehicleToBedConfigSearchViewModel;
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
    @ViewChild("vehicleToBedConfigGrid") vehicleToBedConfigGrid: AcGridComponent;

    constructor(private bedTypeService: BedTypeService, private bedConfigService: BedConfigService, private bedLengthService: BedLengthService,
        private router: Router,
        private route: ActivatedRoute, private toastr: ToastsManager,
        private vehicleToBedConfigService: VehicleToBedConfigService) {
        // initialize empty bed config
        this.replaceBedConfig = {
            id: 0,
            bedLengthId: -1,
            length: "",
            bedLengthMetric: "",
            bedTypeId: -1,
            bedTypeName: "",
            isSelected: false
        };
    }

    ngOnInit() {
        this.showLoadingGif = true;
        this.isSelectAllVehicleToBedConfig = false;
        // Load existing bed config with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        this.bedLengths = [];
        this.bedTypes = [];

        this.bedConfigService.getBedConfig(id).subscribe(result => {
            this.existingBedConfig = result;
            this.bedLengthService.getAllBedLengths().subscribe(bl => {
                this.bedLengths = bl;

                this.refreshFacets();
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });//bed length
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
            });//bed config

        this.vehicleToBedConfigSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: [],
                bedTypes: [],
                bedLengths: [],
            }
            , result: { bedConfigs: [], vehicleToBedConfigs: [] }
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
    private validateReplaceBedConfig(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (Number(this.replaceBedConfig.bedLengthId) === -1) {
            this.toastr.warning("Please select Bed length.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBedConfig.bedTypeId) === -1) {
            this.toastr.warning("Please select Bed type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }

        else if (Number(this.replaceBedConfig.id) < 1) {
            this.toastr.warning("Please select replacement Bed config system.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingBedConfig.id) === Number(this.replaceBedConfig.id)) {
            this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingBedConfig.vehicleToBedConfigs.filter(item => item.isSelected).length <= 0) {
            this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    // event on model change in selection of Bed Type
    onChangeBedType(bedTypeId: number, bedLengthId: number) {
        this.replaceBedConfig.id = null;
        if (bedTypeId > 0 && bedLengthId > 0) {
            this.bedConfigService.getByChildIds(bedLengthId, bedTypeId).subscribe(bc => {
                this.replaceBedConfig = bc;
            },
                error => {
                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                });
        }
        else {
            this.replaceBedConfig.id = 0;
        }

    }

    // event on model change in selection of Bed Length
    onChangeBedLength(bedLengthId: number) {
        this.replaceBedConfig.bedTypeId = -1;
        if (this.replaceBedConfig.bedLengthId == -1) {
            this.bedTypes = [];
            this.replaceBedConfig.id = 0;
            return;
        }
        this.bedTypes = null;
        this.bedTypeService.getByBedLengthId(bedLengthId).subscribe(bt => {
            this.bedTypes = bt;
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            }); // bed types
    }

    onBedConfigIdKeyPress(event) {
        if (Number(event.keyCode) === 13) {
            this.onBedConfigIdSearch();
        }
    }

    // event on bed config id search
    onBedConfigIdSearch() {
        let bedConfigId: number = Number(this.bedConfigIdSearchText);
        if (isNaN(bedConfigId) || bedConfigId <= 0) {
            this.toastr.warning("Invalid Bed Config Id.", ConstantsWarehouse.validationTitle);
            return;
        }
        // empty replace bed config
        this.replaceBedConfig = {
            id: 0,
            bedLengthId: -1,
            length: "",
            bedLengthMetric: "",
            bedTypeId: -1,
            bedTypeName: "",
            isSelected: false
        };

        this.bedTypes = null;
   

        this.showLoadingGif = true;
        this.bedConfigService.getBedConfig(Number(this.bedConfigIdSearchText)).subscribe(result => {

            this.bedTypeService.getByBedLengthId(result.bedLengthId).subscribe(a => {
                    this.bedTypes = a;
                    this.bedConfigService.getByChildIds(result.bedLengthId, result.bedTypeId).subscribe(result => {
                        this.replaceBedConfig = result;
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
    }

    onSelectAllVehicleToBedConfig(isSelected) {
        this.isSelectAllVehicleToBedConfig = isSelected;
        if (this.existingBedConfig.vehicleToBedConfigs == null) {
            return;
        }
        this.existingBedConfig.vehicleToBedConfigs.forEach(item => item.isSelected = isSelected);
    }

    onClearFilters() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.refreshFacets();
    }

    getDefaultInputModel() {
        return {
            bedConfigId: 0,
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

    refreshFacets() {
        let inputModel = this.getDefaultInputModel();
        inputModel.bedConfigId = this.existingBedConfig.id;
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

        this.showLoadingGif = true;
        this.vehicleToBedConfigService.refreshFacets(inputModel).subscribe(data => {
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
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.makes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.makeFacet = this.vehicleToBedConfigSearchViewModel.facets.makes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterModels($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.models != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.modelFacet = this.vehicleToBedConfigSearchViewModel.facets.models.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterSubModels($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.subModels != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.subModelFacet = this.vehicleToBedConfigSearchViewModel.facets.subModels.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterRegions($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.regions != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.regionFacet = this.vehicleToBedConfigSearchViewModel.facets.regions.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterVehicleTypeGroups($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeGroupFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypeGroups.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterVehicleTypes($event) {
        if (this.vehicleToBedConfigSearchViewModel != null &&
            this.vehicleToBedConfigSearchViewModel.facets != null &&
            this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBedConfigSearchViewModel.facets.vehicleTypes.filter(
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

    updateYearFacet(years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    }

    updateMakeFacet(makes) {
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
    updateModelFacet(models, makeName) {
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

    updateSubModelFacet(subModels, modelName) {
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

    updateVehicleTypeGroupFacet(vehicleTypeGroups) {
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

    updateVehicleTypeFacet(vehicleTypes) {
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


    searchVehicleToBedConfigs() {
        this.showLoadingGif = true;

        let inputModel = this.getDefaultInputModel();
        inputModel.bedConfigId = this.existingBedConfig.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        //NOTE: inputModel.bedConfigId = this.existingBedConfig.id; should be sufficient here

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

        this.vehicleToBedConfigService.getAssociations(inputModel).subscribe(result => {
            if (result.length > 0) {
                this.existingBedConfig.vehicleToBedConfigs = result;
                this.existingBedConfig.vehicleToBedConfigCount = result.length;

                this.isSelectAllVehicleToBedConfig = false;

                if (this.vehicleToBedConfigGrid)
                    this.vehicleToBedConfigGrid.refresh();

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

    onVehicleToBedConfigSelected(vechicleToBedConfig: IVehicleToBedConfig) {
        if (vechicleToBedConfig.isSelected) {
            //unchecked
            this.isSelectAllVehicleToBedConfig = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingBedConfig.vehicleToBedConfigs.filter(item => item.id != vechicleToBedConfig.id);
            if (excludedVehicle.every(item => item.isSelected)) {
                this.isSelectAllVehicleToBedConfig = true;
            }
        }
    }

    // event on continue
    onContinue() {
        if (this.validateReplaceBedConfig()) {

            this.bedConfigService.existingBedConfig = this.existingBedConfig;
            this.bedConfigService.existingBedConfig.vehicleToBedConfigs = this.existingBedConfig.vehicleToBedConfigs.filter(item => item.isSelected);

            this.replaceBedConfig.length = this.bedLengths.filter(item => item.id === Number(this.replaceBedConfig.bedLengthId))[0].name;
            this.replaceBedConfig.bedTypeName = this.bedTypes.filter(item => item.id === Number(this.replaceBedConfig.bedTypeId))[0].name;
            this.bedConfigService.replacementBedConfig = this.replaceBedConfig;
            this.router.navigateByUrl("/bedconfig/replace/confirm/" + this.existingBedConfig.id);
        }
    }
}