import { Component, OnInit, ViewChild }           from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { ToastsManager }                          from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { IBrakeConfig }                           from "./brakeConfig.model";
import { BrakeTypeService }                       from "../brakeType/brakeType.service";
import { IBrakeType }                             from "../brakeType/brakeType.model";
import { BrakeABSService }                        from "../brakeABS/brakeABS.service";
import { IBrakeABS }                              from "../brakeABS/brakeABS.model";
import { BrakeSystemService }                     from "../brakeSystem/brakeSystem.service";
import { IBrakeSystem }                           from "../brakeSystem/brakeSystem.model";
import { BrakeConfigService }                     from "./brakeConfig.service";
import { ConstantsWarehouse }                     from "../constants-warehouse";
import { VehicleToBrakeConfigService }            from "../vehicleToBrakeConfig/vehicleToBrakeConfig.service";
import { AcGridComponent }                        from '../../lib/aclibs/ac-grid/ac-grid';
import {IVehicleToBrakeConfig }                   from"../vehicleToBrakeConfig/vehicleToBrakeConfig.model"
import {
    IVehicleToBrakeConfigSearchInputModel,
    IVehicleToBrakeConfigSearchViewModel,
    IFacet }                                    from "../vehicleToBrakeConfig/vehicleToBrakeConfig-search.model";

@Component({
    selector: "brakeConfig-replace-component",
    templateUrl: "app/templates/brakeConfig/brakeConfig-replace.component.html",
    providers: [VehicleToBrakeConfigService],
})

export class BrakeConfigReplaceComponent implements OnInit {
    public existingBrakeConfig: IBrakeConfig;
    public replaceBrakeConfig: IBrakeConfig;
    public frontBrakeTypes: IBrakeType[];
    public rearBrakeTypes: IBrakeType[];
    public brakeABSes: IBrakeABS[];
    public brakeSystems: IBrakeSystem[];
    public brakeConfigIdSearchText: string;
    showLoadingGif: boolean = false;
    isSelectAllVehicleToBrakeConfig: boolean;

    vehicleToBrakeConfigSearchViewModel: IVehicleToBrakeConfigSearchViewModel;
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
    @ViewChild("vehicleToBrakeConfigGrid") vehicleToBrakeConfigGrid: AcGridComponent;

    constructor(private brakeABSService: BrakeABSService, private brakeConfigService: BrakeConfigService, private brakeSystemService: BrakeSystemService,
        private brakeTypeSerivce: BrakeTypeService, private router: Router,
        private route: ActivatedRoute, private toastr: ToastsManager,
        private vehicleToBrakeConfigService: VehicleToBrakeConfigService) {
        // initialize empty brake config
        this.replaceBrakeConfig = {
            id: null,
            frontBrakeTypeId: -1,
            frontBrakeTypeName: "",
            rearBrakeTypeId: -1,
            rearBrakeTypeName: "",
            brakeABSId: -1,
            brakeABSName: "",
            brakeSystemId: -1,
            brakeSystemName: "",
            isSelected: false
        };
    }

    ngOnInit() {
        this.showLoadingGif = true;
        this.isSelectAllVehicleToBrakeConfig = false;
        // Load existing brake config with reference from RouteParams
        let id = Number(this.route.snapshot.params["id"]);
        this.brakeConfigService.getBrakeConfig(id).subscribe(result => {
            this.existingBrakeConfig = result;

            // Load select options for replace.
            this.brakeTypeSerivce.getAllBrakeTypes().subscribe(bt => {
                this.frontBrakeTypes = bt;

                this.refreshFacets();
            }, error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                this.showLoadingGif = false;
            }); // front brake types
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });

        this.rearBrakeTypes = [];
        this.brakeABSes = [];
        this.brakeSystems = [];

        this.vehicleToBrakeConfigSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
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
            }
            , result: { brakeConfigs: [], vehicleToBrakeConfigs: [] }
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
    private validateReplaceBrakeConfig(): Boolean {
        let isValid: Boolean = true;
        // check required fields
        if (Number(this.replaceBrakeConfig.frontBrakeTypeId) === -1) {
            this.toastr.warning("Please select Front brake type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBrakeConfig.rearBrakeTypeId) === -1) {
            this.toastr.warning("Please select Rear brake type.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBrakeConfig.brakeABSId) === -1) {
            this.toastr.warning("Please select Brake ABS.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBrakeConfig.brakeSystemId) === -1) {
            this.toastr.warning("Please select Brake system.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.replaceBrakeConfig.id) < 1) {
            this.toastr.warning("Please select replacement Brake config system.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (Number(this.existingBrakeConfig.id) === Number(this.replaceBrakeConfig.id)) {
            this.toastr.warning("Nothing has changed.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        else if (this.existingBrakeConfig.vehicleToBrakeConfigs != undefined) {
            if (this.existingBrakeConfig.vehicleToBrakeConfigs.filter(item => item.isSelected).length <= 0) {
                this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
                isValid = false;
            }
        }
        else if (this.existingBrakeConfig.vehicleToBrakeConfigs == undefined) {
            this.toastr.warning("No Associations selected.", ConstantsWarehouse.validationTitle);
            isValid = false;
        }
        return isValid;
    }

    // event on model change in selection of Front brake type
    onChangeFrontBrakeType(frontBrakeTypeId: Number) {
        this.replaceBrakeConfig.brakeABSId = -1;
        this.brakeABSes = [];
        this.replaceBrakeConfig.brakeSystemId = -1;
        this.brakeSystems = [];
        this.replaceBrakeConfig.rearBrakeTypeId = -1;

        if (this.replaceBrakeConfig.frontBrakeTypeId == -1) {
            this.rearBrakeTypes = [];
            return;
        }

        this.rearBrakeTypes = null;

        this.brakeTypeSerivce.getByFrontBrakeTypeId(frontBrakeTypeId).subscribe(bt => {
            this.rearBrakeTypes = bt;
        },
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle)); // rear brake types
    }

    // event on model change in selection of Rear brake type
    onChangeRearBrakeType(frontBrakeTypeId: Number, rearBrakeTypeId: Number) {
        this.replaceBrakeConfig.brakeSystemId = -1;
        this.brakeSystems = [];
        this.replaceBrakeConfig.brakeABSId = -1;

        if (this.replaceBrakeConfig.rearBrakeTypeId == -1) {
            this.brakeABSes = [];
            return;
        }

        this.brakeABSes = null;

        this.brakeABSService.getByFrontBrakeTypeIdRearBrakeTypeId(frontBrakeTypeId, rearBrakeTypeId).subscribe(babs => {
            this.brakeABSes = babs;
        },
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle)); // brake ABSes
    }

    // event on model change in selection of Brake Abs
    onChangeBrakeABS(frontBrakeTypeId: Number, rearBrakeTypeId: Number, brakeABSId: Number) {
        this.replaceBrakeConfig.brakeSystemId = -1;

        if (this.replaceBrakeConfig.brakeABSId == -1) {
            this.brakeSystems = [];
            return;
        }

        this.brakeSystems = null;

        this.brakeSystemService.getByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId(frontBrakeTypeId, rearBrakeTypeId, brakeABSId).subscribe(bs => {
            this.brakeSystems = bs;
        },
            error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle)); // brake ABSes
    }

    // event on model change in selection of Brake system
    onChangeBrakeSystem(frontBrakeTypeId: Number, rearBrakeTypeId: Number, brakeABSId: Number, brakeSystemId: Number) {
        this.replaceBrakeConfig.id = null;

        // get brake config system info
        if (frontBrakeTypeId > 0 && rearBrakeTypeId > 0 && brakeABSId > 0 && brakeSystemId > 0) {
            this.brakeConfigService.getByChildIds(frontBrakeTypeId, rearBrakeTypeId, brakeABSId, brakeSystemId).subscribe(result => {
                this.replaceBrakeConfig = result;
            }, error => this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle));
        }
    }

    // event on brake config id quick search
    onBrakeConfigIdKeyPress(event) {
        if (Number(event.keyCode) === 13) {
            this.onBrakeConfigIdSearch();
        }
    }

    // event on brake config id search
    onBrakeConfigIdSearch() {
        let brakeConfigId: number = Number(this.brakeConfigIdSearchText);
        if (isNaN(brakeConfigId) || brakeConfigId <= 0) {
            this.toastr.warning("Invalid Brake Config Id.", ConstantsWarehouse.validationTitle);
            return;
        }
        // empty replace brake config
        this.replaceBrakeConfig = {
            id: 0,
            frontBrakeTypeId: -1,
            frontBrakeTypeName: "",
            rearBrakeTypeId: -1,
            rearBrakeTypeName: "",
            brakeABSId: -1,
            brakeABSName: "",
            brakeSystemId: -1,
            brakeSystemName: "",
            isSelected: false
        };

        this.rearBrakeTypes = null;
        this.brakeABSes = null;
        this.brakeSystems = null;

        this.showLoadingGif = true;
        this.brakeConfigService.getBrakeConfig(Number(this.brakeConfigIdSearchText)).subscribe(result => {
            this.brakeTypeSerivce.getByFrontBrakeTypeId(result.frontBrakeTypeId).subscribe(bt => {
                this.rearBrakeTypes = bt;

                this.brakeABSService.getByFrontBrakeTypeIdRearBrakeTypeId(result.frontBrakeTypeId, result.rearBrakeTypeId).subscribe(babs => {
                    this.brakeABSes = babs;

                    this.brakeSystemService.getByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId(result.frontBrakeTypeId, result.rearBrakeTypeId, result.brakeABSId).subscribe(bs => {
                        this.brakeSystems = bs;

                        this.brakeConfigService.getByChildIds(result.frontBrakeTypeId, result.rearBrakeTypeId, result.brakeABSId, result.brakeSystemId).subscribe(result => {
                            this.replaceBrakeConfig = result;

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
        }, error => {
            let errorMessage: string = JSON.parse(String(error)).message;
            this.toastr.warning(errorMessage, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    onSelectAllVehicleToBrakeConfig(isSelected) {
        this.isSelectAllVehicleToBrakeConfig = isSelected;
        if (this.existingBrakeConfig.vehicleToBrakeConfigs == null) {
            return;
        }
        this.existingBrakeConfig.vehicleToBrakeConfigs.forEach(item => item.isSelected = isSelected);
    }

    onClearFilters() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.refreshFacets();
    }

    getDefaultInputModel() {
        return {
            brakeConfigId: 0,
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

    refreshFacets() {
        let inputModel = this.getDefaultInputModel();
        inputModel.brakeConfigId = this.existingBrakeConfig.id;
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

        this.showLoadingGif = true;
        this.vehicleToBrakeConfigService.refreshFacets(inputModel).subscribe(data => {
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
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.makes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.makeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.makes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterModels($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.models != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.modelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.models.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterSubModels($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.subModels != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.subModelFacet = this.vehicleToBrakeConfigSearchViewModel.facets.subModels.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterRegions($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.regions != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.regionFacet = this.vehicleToBrakeConfigSearchViewModel.facets.regions.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterVehicleTypeGroups($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeGroupFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypeGroups.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterVehicleTypes($event) {
        if (this.vehicleToBrakeConfigSearchViewModel != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets != null &&
            this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleToBrakeConfigSearchViewModel.facets.vehicleTypes.filter(
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

    updateYearFacet(years) {
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    }

    updateMakeFacet(makes) {
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
    updateModelFacet(models, makeName) {
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

    updateSubModelFacet(subModels, modelName) {
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

    updateVehicleTypeGroupFacet(vehicleTypeGroups) {
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

    updateVehicleTypeFacet(vehicleTypes) {
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


    searchVehicleToBrakeConfigs() {
        this.showLoadingGif = true;

        let inputModel = this.getDefaultInputModel();
        inputModel.brakeConfigId = this.existingBrakeConfig.id;
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        //NOTE: inputModel.brakeConfigId = this.existingBrakeConfig.id; should be sufficient here
        //inputModel.frontBrakeTypes.push(this.existingBrakeConfig.frontBrakeTypeName);
        //inputModel.rearBrakeTypes.push(this.existingBrakeConfig.rearBrakeTypeName);
        //inputModel.brakeAbs.push(this.existingBrakeConfig.brakeABSName);
        //inputModel.brakeSystems.push(this.existingBrakeConfig.brakeSystemName);

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

        this.vehicleToBrakeConfigService.getAssociations(inputModel).subscribe(result => {
            if (result.length > 0) {
                this.existingBrakeConfig.vehicleToBrakeConfigs = result;
                this.existingBrakeConfig.vehicleToBrakeConfigCount = result.length;

                this.isSelectAllVehicleToBrakeConfig = false;

                if (this.vehicleToBrakeConfigGrid)
                    this.vehicleToBrakeConfigGrid.refresh();

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

    onVehicleToBrakeConfigSelected(vechicleToBrakeConfig: IVehicleToBrakeConfig) {
        if (vechicleToBrakeConfig.isSelected) {
            //unchecked
            this.isSelectAllVehicleToBrakeConfig = false;
        }
        else {
            //checked
            var excludedVehicle = this.existingBrakeConfig.vehicleToBrakeConfigs.filter(item => item.id != vechicleToBrakeConfig.id);
            if (excludedVehicle.every(item => item.isSelected)) {
                this.isSelectAllVehicleToBrakeConfig = true;
            }
        }
    }

    // event on continue
    onContinue() {
        // validate
        if (this.validateReplaceBrakeConfig()) {
            // set data in factory/ service

            this.brakeConfigService.existingBrakeConfig = this.existingBrakeConfig;
            this.brakeConfigService.existingBrakeConfig.vehicleToBrakeConfigs = this.existingBrakeConfig.vehicleToBrakeConfigs.filter(item => item.isSelected);

            this.replaceBrakeConfig.frontBrakeTypeName = this.frontBrakeTypes.filter(item => item.id === Number(this.replaceBrakeConfig.frontBrakeTypeId))[0].name;
            this.replaceBrakeConfig.rearBrakeTypeName = this.rearBrakeTypes.filter(item => item.id === Number(this.replaceBrakeConfig.rearBrakeTypeId))[0].name;
            this.replaceBrakeConfig.brakeABSName = this.brakeABSes.filter(item => item.id === Number(this.replaceBrakeConfig.brakeABSId))[0].name;
            this.replaceBrakeConfig.brakeSystemName = this.brakeSystems.filter(item => item.id === Number(this.replaceBrakeConfig.brakeSystemId))[0].name;
            this.brakeConfigService.replacementBrakeConfig = this.replaceBrakeConfig;
            // redirect to Confirm page.
            this.router.navigateByUrl("/brakeconfig/replace/confirm/" + this.existingBrakeConfig.id);

        } else {
            //console.log(this.errorMessage);
        }
    }
}