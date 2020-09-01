import { Component, OnInit, ViewChild }           from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { ToastsManager }                          from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { IBodyStyleConfig }                           from "./bodyStyleConfig.model";
import { IBodyType }           from "../bodyType/bodyType.model";
import { BodyTypeService }     from "../bodyType/bodyType.service";
import { BodyNumDoorsService }      from "../bodyNumDoors/bodyNumDoors.service";
import { IBodyNumDoors }            from "../bodyNumDoors/bodyNumDoors.model";
import { BodyStyleConfigService }                     from "./bodyStyleConfig.service";
import { ConstantsWarehouse }                     from "../constants-warehouse";
import { VehicleToBodyStyleConfigService }            from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.service";
import { AcGridComponent }                        from '../../lib/aclibs/ac-grid/ac-grid';
import {IVehicleToBodyStyleConfig }               from"../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.model";
import { IFacet }                           from "../shared/shared.model";
import {
    IVehicleToBodyStyleConfigSearchInputModel,
    IVehicleToBodyStyleConfigSearchViewModel }          from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-search.model";

@Component({
    selector: "bodyStyleConfig-replace-component",
    templateUrl: "app/templates/bodyStyleConfig/bodyStyleConfig-replace.component.html",
    providers: [VehicleToBodyStyleConfigService],
})

export class BodyStyleConfigReplaceComponent implements OnInit {
    public existingBodyStyleConfig: IBodyStyleConfig;
    public replaceBodyStyleConfig: IBodyStyleConfig;
    public bodyTypes: IBodyType[];
    public bodyNumDoors: IBodyNumDoors[];
    public bodyStyleConfigIdSearchText: string;
    showLoadingGif: boolean = false;
    isSelectAllVehicleToBodyStyleConfig: boolean;

    vehicleToBodyStyleConfigSearchViewModel: IVehicleToBodyStyleConfigSearchViewModel;
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
    @ViewChild("vehicleToBodyStyleConfigGrid") vehicleToBodyStyleConfigGrid: AcGridComponent;

    constructor(private bodyTypeService: BodyTypeService, private bodyStyleConfigService: BodyStyleConfigService, private bodyNumDoorsService: BodyNumDoorsService,
        private router: Router,
        private route: ActivatedRoute, private toastr: ToastsManager,
        private vehicleToBodyStyleConfigService: VehicleToBodyStyleConfigService) {
        // initialize empty bed config
        this.replaceBodyStyleConfig = {
            id: 0,
            bodyNumDoorsId: -1,
            numDoors: "",
            bodyTypeId: -1,
            bodyTypeName: "",
            isSelected: false
        };
    }

    ngOnInit() {
        this.showLoadingGif = true;
        this.isSelectAllVehicleToBodyStyleConfig = false;
        // Load existing body style config with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        this.bodyNumDoors = [];
        this.bodyTypes = [];

        this.bodyStyleConfigService.getBodyStyleConfig(id).subscribe(result => {
            this.existingBodyStyleConfig = result;
            this.bodyNumDoorsService.getAllBodyNumDoors().subscribe(bl => {
                this.bodyNumDoors = bl;

                this.refreshFacets();
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            });
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });

        this.vehicleToBodyStyleConfigSearchViewModel = {
            facets: {
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                startYears: [],
                endYears: [],
                makes: [],
                models: [],
                subModels: [],
                bodyNumDoors: [],
                bodyTypes: []
            }
            , result: { bodyStyleConfigs: [], vehicleToBodyStyleConfigs: [] }
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
    private validateReplaceBodyStyleConfig(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (Number(this.replaceBodyStyleConfig.bodyNumDoorsId) === -1) {
            this.toastr.warning("Please select Body num doors.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBodyStyleConfig.bodyTypeId) === -1) {
            this.toastr.warning("Please select Body type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }

        else if (Number(this.replaceBodyStyleConfig.id) < 1) {
            this.toastr.warning("Please select replacement Body style config system.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingBodyStyleConfig.id) === Number(this.replaceBodyStyleConfig.id)) {
            this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.filter(item => item.isSelected).length <= 0) {
            this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    // event on model change in selection of Bed Type
    onChangeBodyType(bodyTypeId: number, bodyNumDoorsId: number) {
        this.replaceBodyStyleConfig.id = null;
        if (bodyTypeId > 0 && bodyNumDoorsId > 0) {
            this.bodyStyleConfigService.getByChildIds(bodyNumDoorsId, bodyTypeId).subscribe(bc => {
                this.replaceBodyStyleConfig = bc;
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            });
        }
        else {
            this.replaceBodyStyleConfig.id = 0;
        }
    }

    // event on model change in selection of Bed Length
    onChangeBodyNumDoors(bodyNumDoorsId: number) {
        this.replaceBodyStyleConfig.bodyTypeId = -1;
        if (this.replaceBodyStyleConfig.bodyNumDoorsId == -1) {
            this.bodyTypes = [];
            this.replaceBodyStyleConfig.id = 0;
            return;
        }
        this.bodyTypes = null;
        this.bodyTypeService.getByBodyNumDoorsId(bodyNumDoorsId).subscribe(bt => {
            this.bodyTypes = bt;
        },
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            }); // bed types
    }

    onBodyStyleConfigIdKeyPress(event) {
        if (Number(event.keyCode) === 13) {
            this.onBodyStyleConfigIdSearch();
        }
    }

    // event on bed config id search
    onBodyStyleConfigIdSearch() {
        let bodyStyleConfigId: number = Number(this.bodyStyleConfigIdSearchText);
        if (isNaN(bodyStyleConfigId) || bodyStyleConfigId <= 0) {
            this.toastr.warning("Invalid Bed Config Id.", ConstantsWarehouse.validationTitle);
            return;
        }
        // empty replace bed config
        this.replaceBodyStyleConfig = {
            id: 0,
            bodyNumDoorsId: -1,
            numDoors: "",
            bodyTypeId: -1,
            bodyTypeName: "",
            isSelected: false
        };

        this.bodyTypes = null;
        this.bodyNumDoors = null;

        this.showLoadingGif = true;
        this.bodyStyleConfigService.getBodyStyleConfig(Number(this.bodyStyleConfigIdSearchText)).subscribe(result => {
            this.bodyTypeService.getByBodyTypeId(result.bodyTypeId).subscribe(bt => {
                this.bodyTypes = bt;

                this.bodyNumDoorsService.getByBodyNumDoorsId(result.bodyNumDoorsId).subscribe(bl => {
                    this.bodyNumDoors = bl;
                    this.bodyStyleConfigService.getByChildIds(result.bodyNumDoorsId, result.bodyTypeId).subscribe(result => {
                        this.replaceBodyStyleConfig = result;
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
    }

    onSelectAllVehicleToBodyStyleConfig(isSelected) {
        this.isSelectAllVehicleToBodyStyleConfig = isSelected;
        if (this.existingBodyStyleConfig.vehicleToBodyStyleConfigs == null) {
            return;
        }
        this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.forEach(item => item.isSelected = isSelected);
    }

    onClearFilters() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.refreshFacets();
    }

    getDefaultInputModel() {
        return {
            bodyStyleConfigId: 0,
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

    refreshFacets() {
        let inputModel = this.getDefaultInputModel();
        inputModel.bodyStyleConfigId = this.existingBodyStyleConfig.id;
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

        this.showLoadingGif = true;
        this.vehicleToBodyStyleConfigService.refreshFacets(inputModel).subscribe(data => {
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
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.makes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.makeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.makes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterModels($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.models != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.modelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterSubModels($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.subModelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterRegions($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.regions != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.regionFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.regions.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterVehicleTypeGroups($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeGroupFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterVehicleTypes($event) {
        if (this.vehicleToBodyStyleConfigSearchViewModel != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets != null &&
            this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.filter(
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

    updateYearFacet(years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    }

    updateMakeFacet(makes) {
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

    //TODO: makeName is not used
    updateModelFacet(models, makeName) {
        let existingSelectedModels = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.filter(model => model.isSelected)
            .map(item => item.name);

        this.vehicleToBodyStyleConfigSearchViewModel.facets.models = [];

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

            this.vehicleToBodyStyleConfigSearchViewModel.facets.models.push(newModel);
        }

        this.modelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.models.slice();
    }

    updateSubModelFacet(subModels, modelName) {
        let existingSelectedSubModels = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.filter(submodel => submodel.isSelected)
            .map(item => item.name);

        this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels = [];

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

            this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.push(newSubModel);
        }

        this.subModelFacet = this.vehicleToBodyStyleConfigSearchViewModel.facets.subModels.slice();
    }

    updateVehicleTypeGroupFacet(vehicleTypeGroups) {
        let existingSelectedItems = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected).map(item => item.name);
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

    updateVehicleTypeFacet(vehicleTypes) {
        let existingSelectedItems = this.vehicleToBodyStyleConfigSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected).map(item => item.name);
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


    searchVehicleToBodyStyleConfigs() {
        this.showLoadingGif = true;

        let inputModel = this.getDefaultInputModel();
        inputModel.bodyStyleConfigId = this.existingBodyStyleConfig.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        //NOTE: inputModel.bodyStyleConfigId = this.existingBodyStyleConfig.id; should be sufficient here

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

        this.vehicleToBodyStyleConfigService.getAssociations(inputModel).subscribe(result => {
            if (result.length > 0) {
                this.existingBodyStyleConfig.vehicleToBodyStyleConfigs = result;
                this.existingBodyStyleConfig.vehicleToBodyStyleConfigCount = result.length;

                this.isSelectAllVehicleToBodyStyleConfig = false;

                if (this.vehicleToBodyStyleConfigGrid)
                    this.vehicleToBodyStyleConfigGrid.refresh();

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

    onVehicleToBodyStyleConfigSelected(vechicleToBodyStyleConfig: IVehicleToBodyStyleConfig) {
        if (vechicleToBodyStyleConfig.isSelected) {
            //unchecked
            this.isSelectAllVehicleToBodyStyleConfig = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.filter(item => item.id != vechicleToBodyStyleConfig.id);
            if (excludedVehicle.every(item => item.isSelected)) {
                this.isSelectAllVehicleToBodyStyleConfig = true;
            }
        }
    }

    // event on continue
    onContinue() {
        if (this.validateReplaceBodyStyleConfig()) {

            this.bodyStyleConfigService.existingBodyStyleConfig = this.existingBodyStyleConfig;
            this.bodyStyleConfigService.existingBodyStyleConfig.vehicleToBodyStyleConfigs = this.existingBodyStyleConfig.vehicleToBodyStyleConfigs.filter(item => item.isSelected);

            this.replaceBodyStyleConfig.numDoors = this.bodyNumDoors.filter(item => item.id === Number(this.replaceBodyStyleConfig.bodyNumDoorsId))[0].numDoors;
            this.replaceBodyStyleConfig.bodyTypeName = this.bodyTypes.filter(item => item.id === Number(this.replaceBodyStyleConfig.bodyTypeId))[0].name;
            this.bodyStyleConfigService.replacementBodyStyleConfig = this.replaceBodyStyleConfig;
            this.router.navigateByUrl("/bodystyleconfig/replace/confirm/" + this.existingBodyStyleConfig.id);

        }
    }
}