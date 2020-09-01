import { Component, OnInit, ViewChild}                      from "@angular/core";
import { IMake }                                            from "../make/make.model";
import { IModel }                                           from "../model/model.model";
import { ISubModel }                                        from "../subModel/subModel.model";
import { Router }                        from "@angular/router";
import {
    IVehicleSearchInputModel,
    IVehicleSearchViewModel,
    IVehicleSearchFacets,
    ConfigurationSystems,
    SearchType,
    FacetType,
    IFacet }                                                from "./vehicle-search.model";
import { VehicleSearchService}                              from "./vehicle-search.service";
import { IBaseVehicle }                                     from "../baseVehicle/baseVehicle.model";
import { IVehicle }                                         from "./vehicle.model";
import { SharedService }                                    from "../shared/shared.service";
import { NavigationService }                                from "../shared/navigation.service";
import { IVehicleToBrakeConfigSearchViewModel }             from "../vehicleToBrakeConfig/vehicleToBrakeConfig-search.model";
import { IVehicleToBrakeConfig }                            from "../vehicleTobrakeConfig/vehicleTobrakeConfig.model";
import { VehicleToBrakeConfigService }                      from "../vehicleToBrakeConfig/vehicleToBrakeConfig.service";
import { VehicleToWheelBaseService }                      from "../vehicleToWheelBase/vehicleToWheelBase.service";
import { IVehicleToBedConfigSearchViewModel }             from "../vehicleToBedConfig/vehicleToBedConfig-search.model";
import { IVehicleToBedConfig }                            from "../vehicleToBedConfig/vehicleToBedConfig.model";
import { VehicleToBedConfigService }                      from "../vehicleToBedConfig/vehicleToBedConfig.service";
import { IVehicleToBodyStyleConfigSearchViewModel }             from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-search.model";
import { IVehicleToBodyStyleConfig }                            from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.model";
import { IVehicleToWheelBase }                            from "../vehicleToWheelBase/vehicleToWheelBase.model";
import { VehicleToBodyStyleConfigService }                      from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.service";
import { ModalComponent }                 from "ng2-bs3-modal/ng2-bs3-modal";
import { ToastsManager }                                    from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ConstantsWarehouse }                               from "../constants-warehouse";
import { AcGridComponent }             from '../../lib/aclibs/ac-grid/ac-grid';
import { AcFileUploader }                                   from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { IVehicleToDriveTypeSearchViewModel }             from "../vehicleToDriveType/vehicleToDriveType-search.model";
import { IVehicleToDriveType }                            from "../vehicleToDriveType/vehicleToDriveType.model";
import { VehicleToDriveTypeService }                      from "../vehicleToDriveType/vehicleToDriveType.service";
import { IVehicleToMfrBodyCodeSearchViewModel }             from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode-search.model";
import { IVehicleToMfrBodyCode }                            from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode.model";
import { VehicleToMfrBodyCodeService }                      from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode.service";

@Component({
    selector: 'vehicleSearch-comp',
    templateUrl: 'app/templates/vehicle/vehicle-search.component.html',
    providers: [VehicleToBrakeConfigService, VehicleToBedConfigService, VehicleToWheelBaseService, VehicleToBodyStyleConfigService, VehicleToDriveTypeService, VehicleToMfrBodyCodeService, VehicleSearchService, AcFileUploader]
})

export class VehicleSearchComponent implements OnInit {
    private isHide: boolean = false;
    private vehicleSearchViewModel: IVehicleSearchViewModel;
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
    private vehicles: IVehicle[] = [];
    private vehicleToBrakeConfigSearchViewModel: IVehicleToBrakeConfigSearchViewModel;
    private vehicleToBedConfigSearchViewModel: IVehicleToBedConfigSearchViewModel;
    private vehicleToBrakeConfigs: IVehicleToBrakeConfig[] = [];
    private vehicleToBedConfigs: IVehicleToBedConfig[] = [];
    private vehicleToBodyStyleConfigs: IVehicleToBodyStyleConfig[] = [];
    private vehicleToDriveTypes: IVehicleToDriveType[] = [];
    private vehicleToWheelBases: IVehicleToWheelBase[] = [];
    private vehicleToMfrBodyCodes: IVehicleToMfrBodyCode[] = [];
    private selectedSystemType: string;
    private isBaseVehiclesExpanded: boolean = true;
    private isVehiclesExpanded: boolean = true;
    private isSystemExpanded: boolean = true;
    private showSystemSelect: boolean = false;
    private selectedSystem: ConfigurationSystems;
    private showLoadingGif: boolean = false;
    private isSelectAllBaseVehicles: boolean;
    private isSelectAllVehicles: boolean;
    private configSystems;
    private deleteVehicleToBrakeConfig: IVehicleToBrakeConfig;
    private deleteVehicleToBedConfig: IVehicleToBedConfig;
    private deleteVehicleToBodyStyleConfig: IVehicleToBodyStyleConfig;
    private deleteVehicleToDriveType: IVehicleToDriveType;
    private deleteVehicleToWheelBase: IVehicleToWheelBase;
    private deleteVehicleToMfrBodyCode: IVehicleToMfrBodyCode;
    private thresholdRecordCount: number = 1000;    //NOTE: keep this number large so that "select all" checkbox for base vehicles always appears
    private thresholdRecordCountVehicle: number = 100;
    private baseVehicleId: string;
    private vehicleId: string;
    private selectedBaseVehicleIdForSearch: string;

    @ViewChild(AcFileUploader) acFileUploader: AcFileUploader;
    @ViewChild("baseVehicleGrid") baseVehicleGrid: AcGridComponent;
    @ViewChild("vehicleGrid") vehicleGrid: AcGridComponent;

    @ViewChild("vehicleToBrakeConfigGrid") vehicleToBrakeConfigGrid: AcGridComponent;
    @ViewChild("vehicleToBedConfigGrid") vehicleToBedConfigGrid: AcGridComponent;
    @ViewChild("vehicleToBodyStyleConfigGrid") vehicleToBodyStyleConfigGrid: AcGridComponent;
    @ViewChild("vehicleToDriveTypeGrid") vehicleToDriveTypeGrid: AcGridComponent;
    @ViewChild("vehicleToWheelBaseGrid") vehicleToWheelBaseGrid: AcGridComponent;
    @ViewChild("vehicleToMfrBodyCodeGrid") vehicleToMfrBodyCodeGrid: AcGridComponent;

    @ViewChild('deleteBrakeAssociationPopup') deleteBrakeAssociationPopup: ModalComponent;
    @ViewChild('deleteBedAssociationPopup') deleteBedAssociationPopup: ModalComponent;
    @ViewChild('deleteBodyStyleAssociationPopup') deleteBodyStyleAssociationPopup: ModalComponent;
    @ViewChild('deleteDriveTypeAssociationPopup') deleteDriveTypeAssociationPopup: ModalComponent;
    @ViewChild('deleteWheelBaseAssociationPopup') deleteWheelBaseAssociationPopup: ModalComponent;
    @ViewChild('deleteMfrBodyCodeAssociationPopup') deleteMfrBodyCodeAssociationPopup: ModalComponent;


    constructor(private vehicleToBrakeConfigService: VehicleToBrakeConfigService
        , private vehicleSearchService: VehicleSearchService, private sharedService: SharedService
        , private router: Router, private toastr: ToastsManager, private navigationService: NavigationService
        , private vehicleToBedConfigService: VehicleToBedConfigService, private vehicleToBodyStyleConfigService: VehicleToBodyStyleConfigService, private vehicleToDriveTypeService: VehicleToDriveTypeService
        , private vehicleToWheelBaseService: VehicleToWheelBaseService, private vehicleToMfrBodyCodeService: VehicleToMfrBodyCodeService) {
    }

    ngOnInit() {
        this.vehicleSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: [],
            },
            result: { baseVehicles: [], vehicles: [] },
            searchType: SearchType.None
        };

        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.baseVehicleId = "";
        this.vehicleId = "";

        if (this.sharedService.vehicleSearchViewModel != null) {
            this.vehicleSearchViewModel.facets = this.sharedService.vehicleSearchViewModel.facets;

            this.regionFacet = this.vehicleSearchViewModel.facets.regions.slice();
            this.startYearFacet = this.vehicleSearchViewModel.facets.startYears.slice();
            this.endYearFacet = this.vehicleSearchViewModel.facets.endYears.slice();
            this.makeFacet = this.vehicleSearchViewModel.facets.makes.slice();
            this.modelFacet = this.vehicleSearchViewModel.facets.models.slice();
            this.subModelFacet = this.vehicleSearchViewModel.facets.subModels.slice();
            this.vehicleTypeGroupFacet = this.vehicleSearchViewModel.facets.vehicleTypeGroups.slice();
            this.vehicleTypeFacet = this.vehicleSearchViewModel.facets.vehicleTypes.slice();

            if (this.sharedService.vehicleSearchViewModel.searchType == SearchType.SearchByBaseVehicleId) {
                this.baseVehicleId = this.sharedService.vehicleSearchViewModel.result.baseVehicles[0].id.toString();
                this.searchByBaseVehicleId();
            }
            else if (this.sharedService.vehicleSearchViewModel.searchType == SearchType.SearchByVehicleId) {
                this.searchByVehicleId();
            }
            else if (this.sharedService.vehicleSearchViewModel.searchType == SearchType.GeneralSearch) {
                this.searchVehicle();
            }
            else {
                this.showLoadingGif = false;
            }
        }
        else {
            this.refreshFacets();
        }

        //this.isSelectAllBaseVehicles = true;
        this.isSelectAllVehicles = false;
        this.selectedSystem = 0;

        this.sharedService.vehicles = null;        //clear old selections
        this.sharedService.brakeConfigs = null;    //clear old selections

        // Drawer right start
        var headerht = $('header').innerHeight();
        var navht = $('nav').innerHeight();
        var winht = $(window).height();
        var winwt = 800;

        $(".drawer-left").css('min-height', winht - headerht - navht);
        $(".drawer-left").css('width', winwt);

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
        //$("#main").on('click', function() {
        //    var drawerwt = $(".drawer-left").width();
        //    if (drawerwt > 15) {
        //        $(".drawer-left").css('width', 15);
        //    } 
        //});
        // Drawer right end
        this.configSystems = ConfigurationSystems;
    }

    clearFilters() {
        this.selectedStartYear = "0";
        this.selectedEndYear = "0";
        this.baseVehicleId = "";
        this.vehicleId = "";

        if (this.vehicleSearchViewModel.facets.regions) {
            this.vehicleSearchViewModel.facets.regions.forEach(item => item.isSelected = false);
        }

        if (this.vehicleSearchViewModel.facets.makes) {
            this.vehicleSearchViewModel.facets.makes.forEach(item => item.isSelected = false);
        }

        if (this.vehicleSearchViewModel.facets.models) {
            this.vehicleSearchViewModel.facets.models.forEach(item => item.isSelected = false);
        }

        if (this.vehicleSearchViewModel.facets.subModels) {
            this.vehicleSearchViewModel.facets.subModels.forEach(item => item.isSelected = false);
        }

        if (this.vehicleSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleSearchViewModel.facets.vehicleTypeGroups.forEach(item => item.isSelected = false);
        }

        if (this.vehicleSearchViewModel.facets.vehicleTypes) {
            this.vehicleSearchViewModel.facets.vehicleTypes.forEach(item => item.isSelected = false);
        }

        this.refreshFacets();
    }

    getDefaultInputModel() {
        return {
            startYear: "0",
            endYear: "0",
            regions: [],
            vehicleTypeGroups: [],
            vehicleTypes: [],
            makes: [],
            models: [],
            subModels: [],
        };
    }

    refreshFacets() {
        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleSearchViewModel.facets.regions) {
            this.vehicleSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }

        if (this.vehicleSearchViewModel.facets.makes) {
            this.vehicleSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }

        if (this.vehicleSearchViewModel.facets.models) {
            this.vehicleSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }

        if (this.vehicleSearchViewModel.facets.subModels) {
            this.vehicleSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(m => inputModel.subModels.push(m.name));
        }

        if (this.vehicleSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }

        if (this.vehicleSearchViewModel.facets.vehicleTypes) {
            this.vehicleSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }

        this.showLoadingGif = true;
        this.vehicleSearchService.refreshFacets(inputModel).subscribe(data => {
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
        if (this.vehicleSearchViewModel != null &&
            this.vehicleSearchViewModel.facets != null &&
            this.vehicleSearchViewModel.facets.makes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.makeFacet = this.vehicleSearchViewModel.facets.makes.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterModels($event) {
        if (this.vehicleSearchViewModel != null &&
            this.vehicleSearchViewModel.facets != null &&
            this.vehicleSearchViewModel.facets.models != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.modelFacet = this.vehicleSearchViewModel.facets.models.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterSubModels($event) {
        if (this.vehicleSearchViewModel != null &&
            this.vehicleSearchViewModel.facets != null &&
            this.vehicleSearchViewModel.facets.subModels != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.subModelFacet = this.vehicleSearchViewModel.facets.subModels.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterRegions($event) {
        if (this.vehicleSearchViewModel != null &&
            this.vehicleSearchViewModel.facets != null &&
            this.vehicleSearchViewModel.facets.regions != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.regionFacet = this.vehicleSearchViewModel.facets.regions.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterVehicleTypeGroups($event) {
        if (this.vehicleSearchViewModel != null &&
            this.vehicleSearchViewModel.facets != null &&
            this.vehicleSearchViewModel.facets.vehicleTypeGroups != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeGroupFacet = this.vehicleSearchViewModel.facets.vehicleTypeGroups.filter(
                item => item.name.toLocaleLowerCase()
                    .indexOf(inputElement.value.toLocaleLowerCase()) !== -1);
        }
    }

    filterVehicleTypes($event) {
        if (this.vehicleSearchViewModel != null &&
            this.vehicleSearchViewModel.facets != null &&
            this.vehicleSearchViewModel.facets.vehicleTypes != null) {
            var inputElement = <HTMLInputElement>$event.target;
            this.vehicleTypeFacet = this.vehicleSearchViewModel.facets.vehicleTypes.filter(
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
        let existingSelectedRegions = this.vehicleSearchViewModel.facets.regions.filter(item => item.isSelected).map(item => item.name);
        this.vehicleSearchViewModel.facets.regions = [];

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

            this.vehicleSearchViewModel.facets.regions.push(newItem);
        }

        this.regionFacet = this.vehicleSearchViewModel.facets.regions.slice();
    }

    updateYearFacet(years) {
        this.vehicleSearchViewModel.facets.startYears = years.slice();
        this.vehicleSearchViewModel.facets.endYears = years.slice();
        this.startYearFacet = years.slice();
        this.endYearFacet = years.slice();
    }

    updateMakeFacet(makes) {
        let existingSelectedMakes = this.vehicleSearchViewModel.facets.makes.filter(make => make.isSelected).map(item => item.name);
        this.vehicleSearchViewModel.facets.makes = [];

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

            this.vehicleSearchViewModel.facets.makes.push(newMake);
        }

        this.makeFacet = this.vehicleSearchViewModel.facets.makes.slice();
    }

    updateModelFacet(models, makeName) {
        let existingSelectedModels = this.vehicleSearchViewModel.facets.models.filter(model => model.isSelected)
            .map(item => item.name);

        this.vehicleSearchViewModel.facets.models = [];

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

            this.vehicleSearchViewModel.facets.models.push(newModel);
        }

        this.modelFacet = this.vehicleSearchViewModel.facets.models.slice();
    }

    updateSubModelFacet(subModels, modelName) {
        let existingSelectedSubModels = this.vehicleSearchViewModel.facets.subModels.filter(submodel => submodel.isSelected)
            .map(item => item.name);

        this.vehicleSearchViewModel.facets.subModels = [];

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

            this.vehicleSearchViewModel.facets.subModels.push(newSubModel);
        }

        this.subModelFacet = this.vehicleSearchViewModel.facets.subModels.slice();
    }

    updateVehicleTypeGroupFacet(vehicleTypeGroups) {
        let existingSelectedItems = this.vehicleSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected).map(item => item.name);
        this.vehicleSearchViewModel.facets.vehicleTypeGroups = [];

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

            this.vehicleSearchViewModel.facets.vehicleTypeGroups.push(newItem);
        }

        this.vehicleTypeGroupFacet = this.vehicleSearchViewModel.facets.vehicleTypeGroups.slice();
    }

    updateVehicleTypeFacet(vehicleTypes) {
        let existingSelectedItems = this.vehicleSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected).map(item => item.name);
        this.vehicleSearchViewModel.facets.vehicleTypes = [];

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

            this.vehicleSearchViewModel.facets.vehicleTypes.push(newItem);
        }

        this.vehicleTypeFacet = this.vehicleSearchViewModel.facets.vehicleTypes.slice();
    }

    searchVehicle() {
        this.vehicleSearchViewModel.searchType = SearchType.GeneralSearch;
        this.vehicles = [];
        this.vehicleToBrakeConfigs = [];
        this.isSelectAllVehicles = false;

        let inputModel = this.getDefaultInputModel();
        inputModel.startYear = this.selectedStartYear;
        inputModel.endYear = this.selectedEndYear;

        if (this.vehicleSearchViewModel.facets.regions) {
            this.vehicleSearchViewModel.facets.regions.filter(item => item.isSelected)
                .forEach(m => inputModel.regions.push(m.name));
        }

        if (this.vehicleSearchViewModel.facets.makes) {
            this.vehicleSearchViewModel.facets.makes.filter(item => item.isSelected)
                .forEach(m => inputModel.makes.push(m.name));
        }

        if (this.vehicleSearchViewModel.facets.models) {
            this.vehicleSearchViewModel.facets.models.filter(item => item.isSelected)
                .forEach(m => inputModel.models.push(m.name));
        }

        if (this.vehicleSearchViewModel.facets.subModels) {
            this.vehicleSearchViewModel.facets.subModels.filter(item => item.isSelected)
                .forEach(m => inputModel.subModels.push(m.name));
        }

        if (this.vehicleSearchViewModel.facets.vehicleTypeGroups) {
            this.vehicleSearchViewModel.facets.vehicleTypeGroups.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypeGroups.push(m.name));
        }

        if (this.vehicleSearchViewModel.facets.vehicleTypes) {
            this.vehicleSearchViewModel.facets.vehicleTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.vehicleTypes.push(m.name));
        }

        this.showLoadingGif = true;
        this.vehicleSearchService.search(inputModel).subscribe(m => {
            if (m.result.baseVehicles.length > 0 || m.result.vehicles.length > 0) {
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

    onBaseIdKeyPress(event) {
        if (event.keyCode == 13) {
            this.searchByBaseVehicleId();
        }
    }

    searchByBaseVehicleId() {
        let baseId = Number(this.baseVehicleId);
        if (isNaN(baseId)) {
            this.toastr.warning("Invalid Base ID", ConstantsWarehouse.validationTitle);
            return;
        }

        this.vehicleSearchViewModel.searchType = SearchType.SearchByBaseVehicleId;
        this.showLoadingGif = true;
        this.vehicles = [];
        this.vehicleToBrakeConfigs = [];
        this.isSelectAllVehicles = false;

        this.vehicleSearchService.searchByBaseVehicleId(baseId).subscribe(m => {
            if (m.result.baseVehicles.length > 0 || m.result.vehicles.length > 0) {
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

    onVehicleIdKeyPress(event) {
        if (event.keyCode == 13) {
            this.searchByVehicleId();
        }
    }

    searchByVehicleId() {
        let vehicleId = Number(this.vehicleId);
        if (isNaN(vehicleId)) {
            this.toastr.warning("Invalid Vehicle ID", ConstantsWarehouse.validationTitle);
            return;
        }

        this.vehicleSearchViewModel.searchType = SearchType.SearchByVehicleId;
        this.showLoadingGif = true;
        this.vehicles = [];
        this.vehicleToBrakeConfigs = [];
        this.isSelectAllVehicles = false;

        this.vehicleSearchService.searchByVehicleId(vehicleId).subscribe(m => {
            if (m.result.baseVehicles.length > 0 || m.result.vehicles.length > 0) {
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

    // wrapper privates
    private onSelectAllBaseVehicle(selected) {
        this.isSelectAllBaseVehicles = selected;
        if (!selected) {
            this.isSelectAllVehicles = false; // uncheck slect all vehicle from vehicles when select all base vehicle unchecked
            this.vehicleSearchViewModel.result.vehicles.forEach(x => x.isSelected = false);
        }

        if (this.vehicleSearchViewModel == null) {
            return;
        }

        this.vehicles = [];
        this.vehicleToBrakeConfigs = [];

        this.vehicleSearchViewModel.result.baseVehicles.forEach(item => {
            item.isSelected = selected;
            this.refreshVehiclesWithBaseVehicleId(item.id, item.isSelected);
        });

        // refresh grids
        if (this.vehicleGrid)
            this.vehicleGrid.refresh();
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
    }

    private onBaseVehicleSelected(baseVehicle: IBaseVehicle) {
        this.refreshVehiclesWithBaseVehicleId(baseVehicle.id, !baseVehicle.isSelected);
        if (baseVehicle.isSelected) {
            //unchecked
            this.isSelectAllBaseVehicles = false;
        }
        else {
            //checked
            var excludedBaseVehicle = this.vehicleSearchViewModel.result.baseVehicles.filter(item => item.id != baseVehicle.id);
            if (excludedBaseVehicle.every(item => item.isSelected)) {
                this.isSelectAllBaseVehicles = true;
            }
            this.isSelectAllVehicles = false;
        }

        // check if all vehicles are selected for less then equal to threshold of vehicle.
        if (this.vehicles && this.vehicles.length > 0 &&
            this.vehicles.length <= this.thresholdRecordCountVehicle && this.vehicles.every(item => item.isSelected)) {
            this.isSelectAllVehicles = true;
        } else {
            this.isSelectAllVehicles = false;
        }

        // refresh grids
        if (this.vehicleGrid)
            this.vehicleGrid.refresh();
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
    }

    refreshVehiclesWithBaseVehicleId(baseVehicleId, isSelected) {
        if (isSelected) {
            var vehiclesRetrieved = this.getVehiclesByBaseVehicleId(baseVehicleId);
            //add this vehicles to vehiclesGrid array
            for (var vehicle of vehiclesRetrieved) {
                this.vehicles.push(vehicle);
            }
        }
        else {
            var removedVehicles = this.vehicles.filter(x => x.baseVehicleId == baseVehicleId);
            removedVehicles.forEach(item => item.isSelected = false);
            //refresh brakes
            this.vehicleToBrakeConfigs = this.vehicleToBrakeConfigs.filter(item => removedVehicles.map(v => v.id).indexOf(item.vehicleId) < 0);

            this.vehicles = this.vehicles.filter(x => x.baseVehicleId != baseVehicleId);
        }
    }

    getVehiclesByBaseVehicleId(id) {
        return this.vehicleSearchViewModel.result.vehicles.filter(v => v.baseVehicleId == id);
    }

    getSystemInfoByVehicleId(id) {
        this.selectedSystemType = "brake";
        if (this.selectedSystemType == "brake") {
            //get information of 
        }
        else if (this.selectedSystemType == "engine") {

        }
        else {
            //systemType not recognized
        }
    }

    filterMenu() {
        if (this.isHide) {
            return "";
        }
        else {
            return "activate";
        }
    }

    onSelectAllVehicle(selected: boolean) {
        this.isSelectAllVehicles = selected;
        if (this.vehicles == null) {
            return;
        }

        this.vehicles.forEach(item => item.isSelected = selected);
        if (this.selectedSystem === ConfigurationSystems.Brake) {
            this.vehicleToBrakeConfigs = [];
            if (selected) {
                this.showLoadingGif = true;
                this.vehicleToBrakeConfigService.searchByVehicleIds(this.vehicles.filter(item => item.isSelected).map(item => item.id)).subscribe(m => {
                    this.vehicles.forEach(v => {
                        v.vehicleToBrakeConfigs = m.filter(item => item.vehicle.id == v.id);
                        this.vehicleToBrakeConfigs = this.vehicleToBrakeConfigs.concat(v.vehicleToBrakeConfigs);
                    });

                    this.showLoadingGif = false;
                },
                    error => {
                        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                        this.showLoadingGif = false;
                    });
            }
        }

        // refresh grids
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();

    }

    onAddVehicleToBrakeConfigs() {
        this.sharedService.vehicles = this.vehicles.filter(item => item.isSelected);
        this.sharedService.vehicles.forEach(item => {
            if (item.vehicleToBrakeConfigs) {
                item.vehicleToBrakeConfigCount = item.vehicleToBrakeConfigs.length;
            }
            else {
                item.vehicleToBrakeConfigCount = 0;
            }
        });
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletobrakeconfig/add"]);
    }

    onAddVehicleToBedConfigs() {
        this.sharedService.vehicles = this.vehicles.filter(item => item.isSelected);
        this.sharedService.vehicles.forEach(item => {
            if (item.vehicleToBedConfigs) {
                item.vehicleToBedConfigCount = item.vehicleToBedConfigs.length;
            }
            else {
                item.vehicleToBedConfigCount = 0;
            }
        });
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletobedconfig/add"]);
    }

    onAddVehicleToBodyStyleConfigs() {
        this.sharedService.vehicles = this.vehicles.filter(item => item.isSelected);
        this.sharedService.vehicles.forEach(item => {
            if (item.vehicleToBodyStyleConfigs) {
                item.vehicleToBodyStyleConfigCount = item.vehicleToBodyStyleConfigs.length;
            }
            else {
                item.vehicleToBodyStyleConfigCount = 0;
            }
        });
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletobodystyleconfig/add"]);
    }

    onAddVehicleToDriveTypes() {
        this.sharedService.vehicles = this.vehicles.filter(item => item.isSelected);
        this.sharedService.vehicles.forEach(item => {
            if (item.vehicleToDriveTypes) {
                item.vehicleToDriveTypeCount = item.vehicleToDriveTypes.length;
            }
            else {
                item.vehicleToDriveTypeCount = 0;
            }
        });
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletodrivetype/add"]);
    }

    onAddVehicleToMfrBodyCodes() {
        this.sharedService.vehicles = this.vehicles.filter(item => item.isSelected);
        this.sharedService.vehicles.forEach(item => {
            if (item.vehicleToMfrBodyCodes) {
                item.vehicleToMfrBodyCodeCount = item.vehicleToMfrBodyCodes.length;
            }
            else {
                item.vehicleToMfrBodyCodeCount = 0;
            }
        });
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        this.router.navigate(["/vehicletomfrbodycode/add"]);
    }

    getSearchResult(m: IVehicleSearchViewModel) {
        //NOTE: do not replace this.vehicleSearchViewModel.facets = m.facets;
        //NOTE: do not replace this.vehicleSearchViewModel.filters
        this.vehicleSearchViewModel.result = m.result;
        this.vehicleSearchViewModel.totalCount = m.totalCount;

        this.vehicles = [];
        this.vehicleToBrakeConfigs = [];

        this.isSelectAllBaseVehicles = false;
        // note: select all only if totalRecords <= thresholdRecordCount
        // else no selection.
        if (this.vehicleSearchViewModel.result.baseVehicles.length <= this.thresholdRecordCount) {
            this.vehicleSearchViewModel.result.baseVehicles.forEach(item => {
                item.isSelected = true;
                this.refreshVehiclesWithBaseVehicleId(item.id, item.isSelected);
            });
            this.isSelectAllBaseVehicles = true;
        }

        // refresh grid
        if (this.baseVehicleGrid)
            this.baseVehicleGrid.refresh();
        if (this.vehicleGrid)
            this.vehicleGrid.refresh();
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
        if (this.vehicleToMfrBodyCodeGrid)
            this.vehicleToMfrBodyCodeGrid.refresh();
    }

    onVehicleSelected(vehicle: IVehicle, event) {
        if (event.target.checked) {
            switch (this.selectedSystem) {
                case ConfigurationSystems.Brake:
                    this.loadVehicleToBrakeSystem(vehicle);
                    break;
                case ConfigurationSystems.Bed:
                    this.loadVehicleToBedSystem(vehicle);
                    break;
                case ConfigurationSystems.Body:
                    this.loadVehicleToBodyStyleSystem(vehicle);
                    break;
                case ConfigurationSystems.Drive:
                    this.loadVehicleToDriveSystem(vehicle);
                    break;
                case ConfigurationSystems.Wheel:
                    this.loadVehicleToWheelBaseSystem(vehicle);
                    break;
                case ConfigurationSystems.MFR:
                    this.loadVehicleToMfrBodyCodeSystem(vehicle);
                    break;
                default:
                    break;
            }
        }
        else {
            this.vehicleToBrakeConfigs = this.vehicleToBrakeConfigs.filter(item => item.vehicleId != vehicle.id);
            this.vehicleToBedConfigs = this.vehicleToBedConfigs.filter(item => item.vehicleId != vehicle.id);
            this.vehicleToBodyStyleConfigs = this.vehicleToBodyStyleConfigs.filter(item => item.vehicleId != vehicle.id);
            this.vehicleToDriveTypes = this.vehicleToDriveTypes.filter(item => item.vehicleId != vehicle.id);
            this.vehicleToWheelBases = this.vehicleToWheelBases.filter(item => item.vehicleId != vehicle.id);
            this.vehicleToMfrBodyCodes = this.vehicleToMfrBodyCodes.filter(item => item.vehicleId != vehicle.id);
        }

        if (vehicle.isSelected) {
            //unchecked
            this.isSelectAllVehicles = false;
        }
        else {
            //checked
            var excludedVehicle = this.vehicles.filter(item => item.id != vehicle.id);
            if (excludedVehicle.every(item => item.isSelected)) {
                this.isSelectAllVehicles = true;
            }
        }

        // refresh grids
        if (this.vehicleToBrakeConfigGrid)
            this.vehicleToBrakeConfigGrid.refresh();
        if (this.vehicleToBedConfigGrid)
            this.vehicleToBedConfigGrid.refresh();
        if (this.vehicleToBodyStyleConfigGrid)
            this.vehicleToBodyStyleConfigGrid.refresh();
        if (this.vehicleToDriveTypeGrid)
            this.vehicleToDriveTypeGrid.refresh();
        if (this.vehicleToWheelBaseGrid)
            this.vehicleToWheelBaseGrid.refresh();
        if (this.vehicleToMfrBodyCodeGrid)
            this.vehicleToMfrBodyCodeGrid.refresh();
    }

    private loadVehicleToBrakeSystem(vehicle: IVehicle) {
        if (vehicle.vehicleToBrakeConfigs == null || vehicle.vehicleToBrakeConfigs.length == 0) {
            this.showLoadingGif = true;
            this.vehicleToBrakeConfigService.searchByVehicleIds([vehicle.id]).subscribe(m => {
                vehicle.vehicleToBrakeConfigs = m;
                this.vehicleToBrakeConfigs = this.vehicleToBrakeConfigs.concat(m);
                this.showLoadingGif = false;
            },
                error => {
                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    this.showLoadingGif = false;
                });
        } else {
            this.vehicleToBrakeConfigs = this.vehicleToBrakeConfigs.concat(vehicle.vehicleToBrakeConfigs);
        }
    }

    private loadVehicleToBedSystem(vehicle: IVehicle) {
        if (vehicle.vehicleToBedConfigs == null || vehicle.vehicleToBedConfigs.length == 0) {
            this.showLoadingGif = true;
            this.vehicleToBedConfigService.searchByVehicleIds([vehicle.id]).subscribe(m => {
                vehicle.vehicleToBedConfigs = m;
                this.vehicleToBedConfigs = this.vehicleToBedConfigs.concat(m);
                this.showLoadingGif = false;
            },
                error => {
                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    this.showLoadingGif = false;
                });
        } else {
            this.vehicleToBedConfigs = this.vehicleToBedConfigs.concat(vehicle.vehicleToBedConfigs);
        }
    }

    private loadVehicleToBodyStyleSystem(vehicle: IVehicle) {
        if (vehicle.vehicleToBodyStyleConfigs == null || vehicle.vehicleToBodyStyleConfigs.length == 0) {
            this.showLoadingGif = true;
            this.vehicleToBodyStyleConfigService.searchByVehicleIds([vehicle.id]).subscribe(m => {
                vehicle.vehicleToBodyStyleConfigs = m;
                this.vehicleToBodyStyleConfigs = this.vehicleToBodyStyleConfigs.concat(m);
                this.showLoadingGif = false;
            },
                error => {
                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    this.showLoadingGif = false;
                });
        } else {
            this.vehicleToBodyStyleConfigs = this.vehicleToBodyStyleConfigs.concat(vehicle.vehicleToBodyStyleConfigs);
        }
    }

    private loadVehicleToDriveSystem(vehicle: IVehicle) {
        if (vehicle.vehicleToDriveTypes == null || vehicle.vehicleToDriveTypes.length == 0) {
            this.showLoadingGif = true;
            this.vehicleToDriveTypeService.searchByVehicleIds([vehicle.id]).subscribe(m => {
                vehicle.vehicleToDriveTypes = m;
                this.vehicleToDriveTypes = this.vehicleToDriveTypes.concat(m);
                this.showLoadingGif = false;
            },
                error => {
                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    this.showLoadingGif = false;
                });
        } else {
            this.vehicleToDriveTypes = this.vehicleToDriveTypes.concat(vehicle.vehicleToDriveTypes);
        }
    }

    private loadVehicleToWheelBaseSystem(vehicle: IVehicle) {
        if (vehicle.vehicleToWheelBases == null || vehicle.vehicleToWheelBases.length == 0) {
            this.showLoadingGif = true;
            this.vehicleToWheelBaseService.searchByVehicleIds([vehicle.id]).subscribe(m => {
                vehicle.vehicleToWheelBases = m;
                this.vehicleToWheelBases = this.vehicleToWheelBases.concat(m);
                this.showLoadingGif = false;
            },
                error => {
                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    this.showLoadingGif = false;
                });
        } else {
            this.vehicleToWheelBases = this.vehicleToWheelBases.concat(vehicle.vehicleToWheelBases);
        }
    }

    private loadVehicleToMfrBodyCodeSystem(vehicle: IVehicle) {
        if (vehicle.vehicleToMfrBodyCodes == null || vehicle.vehicleToMfrBodyCodes.length == 0) {
            this.showLoadingGif = true;
            this.vehicleToMfrBodyCodeService.searchByVehicleIds([vehicle.id]).subscribe(m => {
                vehicle.vehicleToMfrBodyCodes = m;
                this.vehicleToMfrBodyCodes = this.vehicleToMfrBodyCodes.concat(m);
                this.showLoadingGif = false;
            },
                error => {
                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    this.showLoadingGif = false;
                });
        } else {
            this.vehicleToMfrBodyCodes = this.vehicleToMfrBodyCodes.concat(vehicle.vehicleToMfrBodyCodes);
        }
    }

    onDeleteVehicleToBrakeConfig(vehicleToBrakeConfig: IVehicleToBrakeConfig) {
        this.deleteVehicleToBrakeConfig = vehicleToBrakeConfig;

        this.deleteBrakeAssociationPopup.open("sm");
    }

    onDeleteVehicleToBedConfig(vehicleToBedConfig: IVehicleToBedConfig) {
        this.deleteVehicleToBedConfig = vehicleToBedConfig;

        this.deleteBedAssociationPopup.open("sm");
    }

    onDeleteVehicleToBodyStyleConfig(vehicleToBodyStyleConfig: IVehicleToBodyStyleConfig) {
        this.deleteVehicleToBodyStyleConfig = vehicleToBodyStyleConfig;

        this.deleteBodyStyleAssociationPopup.open("sm");
    }

    onDeleteVehicleToDriveType(vehicleToDriveType: IVehicleToDriveType) {
        this.deleteVehicleToDriveType = vehicleToDriveType;

        this.deleteDriveTypeAssociationPopup.open("sm");
    }

    onDeleteVehicleToWheelBase(vehicleToWheelBase: IVehicleToWheelBase) {
        this.deleteVehicleToWheelBase = vehicleToWheelBase;

        this.deleteWheelBaseAssociationPopup.open("sm");
    }

    onDeleteVehicleToMfrBodyCode(vehicleToMfrBodyCode: IVehicleToMfrBodyCode) {
        this.deleteVehicleToMfrBodyCode = vehicleToMfrBodyCode;

        this.deleteMfrBodyCodeAssociationPopup.open("sm");
    }

    onDeleteVehicleToBrakeConfigSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteVehicleToBrakeConfig.attachments = uploadedFiles;
            }
            if (this.deleteVehicleToBrakeConfig.attachments) {
                this.deleteVehicleToBrakeConfig.attachments = this.deleteVehicleToBrakeConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleToBrakeConfigService.deleteVehicleToBrakeConfig(this.deleteVehicleToBrakeConfig.id, this.deleteVehicleToBrakeConfig).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Brake Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBrakeConfig.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.deleteVehicleToBrakeConfig.id + "\" Brake Association change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBrakeConfig.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            },
                error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Brake Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBrakeConfig.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                    this.showLoadingGif = false;
                }, () => {
                    this.acFileUploader.reset(true);
                    this.deleteBrakeAssociationPopup.close();
                    this.showLoadingGif = false;
                });
        }, error => {
            this.acFileUploader.reset(true);
            this.deleteBrakeAssociationPopup.close();
            this.showLoadingGif = false;
        });
    }

    onDeleteVehicleToBedConfigSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteVehicleToBedConfig.attachments = uploadedFiles;
            }
            if (this.deleteVehicleToBedConfig.attachments) {
                this.deleteVehicleToBedConfig.attachments = this.deleteVehicleToBedConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleToBedConfigService.deleteVehicleToBedConfig(this.deleteVehicleToBedConfig.id, this.deleteVehicleToBedConfig).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Bed Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBedConfig.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.deleteVehicleToBedConfig.id + "\" Bed Association change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBedConfig.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            },
                error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Bed Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBedConfig.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                    this.showLoadingGif = false;
                }, () => {
                    this.acFileUploader.reset(true);
                    this.deleteBedAssociationPopup.close();
                    this.showLoadingGif = false;
                });
        }, error => {
            this.acFileUploader.reset(true);
            this.deleteBedAssociationPopup.close();
            this.showLoadingGif = false;
        });
    }

    onDeleteVehicleToBodyStyleConfigSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteVehicleToBodyStyleConfig.attachments = uploadedFiles;
            }
            if (this.deleteVehicleToBodyStyleConfig.attachments) {
                this.deleteVehicleToBodyStyleConfig.attachments = this.deleteVehicleToBodyStyleConfig.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleToBodyStyleConfigService.deleteVehicleToBodyStyleConfig(this.deleteVehicleToBodyStyleConfig.id, this.deleteVehicleToBodyStyleConfig).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Body Style Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBodyStyleConfig.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.deleteVehicleToBodyStyleConfig.id + "\" Body Style Association change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Body Style Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBodyStyleConfig.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            },
                error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Body Style Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToBodyStyleConfig.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                    this.showLoadingGif = false;
                }, () => {
                    this.acFileUploader.reset(true);
                    this.deleteBodyStyleAssociationPopup.close();
                    this.showLoadingGif = false;
                });
        }, error => {
            this.acFileUploader.reset(true);
            this.deleteBodyStyleAssociationPopup.close();
            this.showLoadingGif = false;
        });
    }

    onDeleteVehicleToDriveTypeSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteVehicleToDriveType.attachments = uploadedFiles;
            }
            if (this.deleteVehicleToDriveType.attachments) {
                this.deleteVehicleToDriveType.attachments = this.deleteVehicleToDriveType.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleToDriveTypeService.deleteVehicleToDriveType(this.deleteVehicleToDriveType.id, this.deleteVehicleToDriveType).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Drive Type Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToDriveType.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.deleteVehicleToDriveType.id + "\" Drive Type Association change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Drive Type Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToDriveType.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            },
                error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Drive Type Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToDriveType.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                    this.showLoadingGif = false;
                }, () => {
                    this.acFileUploader.reset(true);
                    this.deleteDriveTypeAssociationPopup.close();
                    this.showLoadingGif = false;
                });
        }, error => {
            this.acFileUploader.reset(true);
            this.deleteDriveTypeAssociationPopup.close();
            this.showLoadingGif = false;
        });
    }

    onDeleteVehicleToWheelBaseSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteVehicleToWheelBase.attachments = uploadedFiles;
            }
            if (this.deleteVehicleToWheelBase.attachments) {
                this.deleteVehicleToWheelBase.attachments = this.deleteVehicleToWheelBase.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleToWheelBaseService.deleteVehicleToWheelBase(this.deleteVehicleToWheelBase.id, this.deleteVehicleToWheelBase).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("Wheel Base Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToWheelBase.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.deleteVehicleToWheelBase.id + "\" Wheel Base Association change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Wheel Base Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToWheelBase.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            },
                error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("Wheel Base Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToWheelBase.id);
                    this.toastr.warning(error, errorMessage.title);
                    this.showLoadingGif = false;
                }, () => {
                    this.acFileUploader.reset(true);
                    this.deleteWheelBaseAssociationPopup.close();
                    this.showLoadingGif = false;
                });
        }, error => {
            this.acFileUploader.reset(true);
            this.deleteWheelBaseAssociationPopup.close();
            this.showLoadingGif = false;
        });
    }

    onDeleteVehicleToMfrBodyCodeSubmit() {
        this.showLoadingGif = true;
        this.acFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.deleteVehicleToMfrBodyCode.attachments = uploadedFiles;
            }
            if (this.deleteVehicleToMfrBodyCode.attachments) {
                this.deleteVehicleToMfrBodyCode.attachments = this.deleteVehicleToMfrBodyCode.attachments.concat(this.acFileUploader.getFilesMarkedToDelete());
            }
            this.vehicleToMfrBodyCodeService.deleteVehicleToMfrBodyCode(this.deleteVehicleToMfrBodyCode.id, this.deleteVehicleToMfrBodyCode).subscribe(response => {
                if (response) {
                    let successMessage = ConstantsWarehouse.notificationMessage.success("MFR Body Code Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToMfrBodyCode.id);
                    successMessage.title = "Your request to " + ConstantsWarehouse.changeRequestType.remove + " the \"" + this.deleteVehicleToMfrBodyCode.id + "\" MFR Body Code Association change requestid  \"" + response + "\" will be reviewed.";
                    this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("MFR Body Code Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToMfrBodyCode.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            },
                error => {
                    let errorMessage = ConstantsWarehouse.notificationMessage.error("MFR Body Code Association", ConstantsWarehouse.changeRequestType.remove, this.deleteVehicleToMfrBodyCode.id);
                    this.toastr.warning(errorMessage.body, errorMessage.title);
                    this.showLoadingGif = false;
                }, () => {
                    this.acFileUploader.reset(true);
                    this.deleteMfrBodyCodeAssociationPopup.close();
                    this.showLoadingGif = false;
                });
        }, error => {
            this.acFileUploader.reset(true);
            this.deleteMfrBodyCodeAssociationPopup.close();
            this.showLoadingGif = false;
        });
    }

    systemSelect(selectedSystem: ConfigurationSystems) {
        this.showSystemSelect = !this.showSystemSelect;
        this.selectedSystem = selectedSystem;
        this.vehicleToBrakeConfigs = [];
        switch (this.selectedSystem) {
            case ConfigurationSystems.Brake:
                this.loadVehicleToBrakeSystemOnSystemSelect();
                break;
            case ConfigurationSystems.Bed:
                this.loadVehicleToBedSystemOnSystemSelect();
                break;
            case ConfigurationSystems.Body:
                this.loadVehicleToBodyStyleSystemOnSystemSelect();
                break;
            case ConfigurationSystems.Drive:
                this.loadVehicleToDriveTypeSystemOnSystemSelect();
                break;
            case ConfigurationSystems.Wheel:
                this.vehicleToWheelBases = [];
                this.loadVehicleToWheelSystemOnSystemSelect();
                break;
            case ConfigurationSystems.MFR:
                this.loadVehicleToMFRSystemOnSystemSelect();
                break;
            default:
                break;
        }

    }

    private loadVehicleToBrakeSystemOnSystemSelect() {
        if (this.vehicles.filter(x => x.isSelected).length) {
            var selectedTemp = this.vehicles.filter(x => x.isSelected);
            this.vehicles.filter(x => x.isSelected).forEach(vehicle => {
                if (vehicle.vehicleToBrakeConfigs) {
                    this.vehicleToBrakeConfigs = this.vehicleToBrakeConfigs.concat(vehicle.vehicleToBrakeConfigs);
                    selectedTemp = selectedTemp.filter(x => x.id !== vehicle.id);
                }
            });

            if (selectedTemp.length > 0) {
                this.showLoadingGif = true;
                this.vehicleToBrakeConfigService.searchByVehicleIds(selectedTemp.map(item => item.id)).subscribe(m => {
                    this.vehicles.forEach(v => {
                        v.vehicleToBrakeConfigs = m.filter(item => item.vehicle.id === v.id);
                        this.vehicleToBrakeConfigs = this.vehicleToBrakeConfigs.concat(v.vehicleToBrakeConfigs);
                    });

                    this.showLoadingGif = false;
                },
                    error => {
                        this.showLoadingGif = false;
                        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    });
            }
        }
    }

    private loadVehicleToBedSystemOnSystemSelect() {
        if (this.vehicles.filter(x => x.isSelected).length) {
            var selectedTemp = this.vehicles.filter(x => x.isSelected);
            this.vehicles.filter(x => x.isSelected).forEach(vehicle => {
                if (vehicle.vehicleToBedConfigs) {
                    this.vehicleToBedConfigs = this.vehicleToBedConfigs.concat(vehicle.vehicleToBedConfigs);
                    selectedTemp = selectedTemp.filter(x => x.id !== vehicle.id);
                }
            });

            if (selectedTemp.length > 0) {
                this.showLoadingGif = true;
                this.vehicleToBedConfigService.searchByVehicleIds(selectedTemp.map(item => item.id)).subscribe(m => {
                    this.vehicles.forEach(v => {
                        v.vehicleToBedConfigs = m.filter(item => item.vehicle.id === v.id);
                        this.vehicleToBedConfigs = this.vehicleToBedConfigs.concat(v.vehicleToBedConfigs);
                    });

                    this.showLoadingGif = false;
                },
                    error => {
                        this.showLoadingGif = false;
                        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    });
            }
        }
    }

    private loadVehicleToBodyStyleSystemOnSystemSelect() {
        if (this.vehicles.filter(x => x.isSelected).length) {
            var selectedTemp = this.vehicles.filter(x => x.isSelected);
            this.vehicles.filter(x => x.isSelected).forEach(vehicle => {
                if (vehicle.vehicleToBodyStyleConfigs) {
                    this.vehicleToBodyStyleConfigs = this.vehicleToBodyStyleConfigs.concat(vehicle.vehicleToBodyStyleConfigs);
                    selectedTemp = selectedTemp.filter(x => x.id !== vehicle.id);
                }
            });

            if (selectedTemp.length > 0) {
                this.showLoadingGif = true;
                this.vehicleToBodyStyleConfigService.searchByVehicleIds(selectedTemp.map(item => item.id)).subscribe(m => {
                    this.vehicles.forEach(v => {
                        v.vehicleToBodyStyleConfigs = m.filter(item => item.vehicle.id === v.id);
                        this.vehicleToBodyStyleConfigs = this.vehicleToBodyStyleConfigs.concat(v.vehicleToBodyStyleConfigs);
                    });

                    this.showLoadingGif = false;
                },
                    error => {
                        this.showLoadingGif = false;
                        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    });
            }
        }
    }

    private loadVehicleToDriveTypeSystemOnSystemSelect() {
        if (this.vehicles.filter(x => x.isSelected).length) {
            var selectedTemp = this.vehicles.filter(x => x.isSelected);
            this.vehicleToDriveTypes = [];
            this.vehicles.filter(x => x.isSelected).forEach(vehicle => {
                if (vehicle.vehicleToDriveTypes) {
                    this.vehicleToDriveTypes = this.vehicleToDriveTypes.concat(vehicle.vehicleToDriveTypes);
                    selectedTemp = selectedTemp.filter(x => x.id !== vehicle.id);
                }
            });

            if (selectedTemp.length > 0) {
                this.showLoadingGif = true;
                this.vehicleToDriveTypeService.searchByVehicleIds(selectedTemp.map(item => item.id)).subscribe(m => {
                    this.vehicles.forEach(v => {
                        v.vehicleToDriveTypes = m.filter(item => item.vehicle.id === v.id);
                        this.vehicleToDriveTypes = this.vehicleToDriveTypes.concat(v.vehicleToDriveTypes);
                    });

                    this.showLoadingGif = false;
                },
                    error => {
                        this.showLoadingGif = false;
                        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    });
            }
        }
    }

    private loadVehicleToWheelSystemOnSystemSelect() {
        if (this.vehicles.filter(x => x.isSelected).length) {
            var selectedTemp = this.vehicles.filter(x => x.isSelected);
            this.vehicles.filter(x => x.isSelected).forEach(vehicle => {
                if (vehicle.vehicleToWheelBases) {
                    this.vehicleToWheelBases = this.vehicleToWheelBases.concat(vehicle.vehicleToWheelBases);
                    selectedTemp = selectedTemp.filter(x => x.id !== vehicle.id);
                }
            });

            if (selectedTemp.length > 0) {
                this.showLoadingGif = true;
                this.vehicleToWheelBaseService.searchByVehicleIds(selectedTemp.map(item => item.id)).subscribe(m => {
                    this.vehicles.forEach(v => {
                        v.vehicleToWheelBases = m.filter(item => item.vehicle.id === v.id);
                        this.vehicleToWheelBases = this.vehicleToWheelBases.concat(v.vehicleToWheelBases);
                    });

                    this.showLoadingGif = false;
                },
                    error => {
                        this.showLoadingGif = false;
                        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    });
            }
        }
    }

    private loadVehicleToMFRSystemOnSystemSelect() {
        if (this.vehicles.filter(x => x.isSelected).length) {
            var selectedTemp = this.vehicles.filter(x => x.isSelected);
            this.vehicles.filter(x => x.isSelected).forEach(vehicle => {
                if (vehicle.vehicleToMfrBodyCodes) {
                    this.vehicleToMfrBodyCodes = this.vehicleToMfrBodyCodes.concat(vehicle.vehicleToMfrBodyCodes);
                    selectedTemp = selectedTemp.filter(x => x.id !== vehicle.id);
                }
            });

            if (selectedTemp.length > 0) {
                this.showLoadingGif = true;
                this.vehicleToMfrBodyCodeService.searchByVehicleIds(selectedTemp.map(item => item.id)).subscribe(m => {
                    this.vehicles.forEach(v => {
                        v.vehicleToMfrBodyCodes = m.filter(item => item.vehicle.id === v.id);
                        this.vehicleToMfrBodyCodes = this.vehicleToMfrBodyCodes.concat(v.vehicleToMfrBodyCodes);
                    });

                    this.showLoadingGif = false;
                },
                    error => {
                        this.showLoadingGif = false;
                        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                    });
            }
        }
    }

    iconClassReturn() {
        var returnclass = "";
        if (this.selectedSystem === ConfigurationSystems.Brake)
            returnclass = "brake-system";
        else if (this.selectedSystem === ConfigurationSystems.Engine)
            returnclass = "engine-system";
        else if (this.selectedSystem === ConfigurationSystems.Wheel)
            returnclass = "wheel-system";
        return returnclass;
    }

    routerLinkRedirect(route: string, id: number) {
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        var routeToTraverse = route;
        if (id !== 0) {
            routeToTraverse = routeToTraverse + id.toString();
        }
        this.router.navigateByUrl(routeToTraverse);
    }

    onViewBaseVehicleChangeRequest(baseVehicleVm: IBaseVehicle) {
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/basevehicle/" + baseVehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    onViewVehicleCr(vehicleVm: IVehicle) {
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/vehicle/" + vehicleVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    }

    onViewAssociatedVehiclesCr(associatedVehicleVm: IVehicleToBrakeConfig) {
        this.sharedService.vehicleSearchViewModel = this.vehicleSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        switch (this.selectedSystem) {
            case ConfigurationSystems.Brake:
                var changeRequestLink = "/change/review/vehicletobrakeconfig/" + associatedVehicleVm.changeRequestId;
                break;
            case ConfigurationSystems.Bed:
                var changeRequestLink = "/change/review/vehicletobedconfig/" + associatedVehicleVm.changeRequestId;
                break;
            case ConfigurationSystems.Body:
                var changeRequestLink = "/change/review/vehicletobodystyleconfig/" + associatedVehicleVm.changeRequestId;
                break;
            case ConfigurationSystems.Drive:
                var changeRequestLink = "/change/review/vehicletodrivetype/" + associatedVehicleVm.changeRequestId;
                break;
            case ConfigurationSystems.MFR:
                var changeRequestLink = "/change/review/vehicletomfrbodycode/" + associatedVehicleVm.changeRequestId;
                break;
            default:
                break;
        }

        this.router.navigateByUrl(changeRequestLink);
    }

    clearFacet(facet: IFacet[], refresh: boolean = true) {
        if (facet) {
            facet.forEach(item => item.isSelected = false);
        }
        if (refresh) {
            this.refreshFacets();
        }
    }

    onSystemAssociationClick() {
        switch (this.selectedSystem) {
            case ConfigurationSystems.Select:
                this.toastr.warning("Please select an assoication to add.", "No association selected!!");
                break;

            case ConfigurationSystems.Brake:
                this.onAddVehicleToBrakeConfigs();
                break;
            case ConfigurationSystems.Bed:
                this.onAddVehicleToBedConfigs();
                break;
            case ConfigurationSystems.Body:
                this.onAddVehicleToBodyStyleConfigs();
                break;
            case ConfigurationSystems.Drive:
                this.onAddVehicleToDriveTypes();
                break;
            case ConfigurationSystems.MFR:
                this.onAddVehicleToMfrBodyCodes();
                break;

            default:
                this.toastr.warning("", "No Method for this Vehicle to System Association");
                break;
        }
    }
}