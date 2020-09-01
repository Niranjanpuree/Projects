import { Component, OnInit, ViewChild, Input,
    Output, EventEmitter }                               from "@angular/core";
import { Router }                     from "@angular/router";
import { ToastsManager }                                 from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { SharedService }                                 from "../shared/shared.service";
import { NavigationService }                             from "../shared/navigation.service";
import { ConstantsWarehouse }                            from "../constants-warehouse";
import { SystemMenuBar }                                 from "./system-menubar.component";
import { IVehicleToBrakeConfigSearchViewModel,
    SearchType, IVehicleToBrakeConfigSearchInputModel }  from "../vehicleToBrakeConfig/vehicleToBrakeConfig-search.model";
import { VehicleToBrakeConfigSearchComponent }           from "../vehicleToBrakeConfig/vehicleToBrakeConfig-search.component";
import { VehicleToBedConfigSearchComponent }             from "../vehicleToBedConfig/vehicleToBedConfig-search.component";
import { IVehicleToBrakeConfig }                         from "../vehicleTobrakeConfig/vehicleTobrakeConfig.model";

import { IVehicleToBedConfigSearchViewModel,
    IVehicleToBedConfigSearchInputModel }               from "../vehicleToBedConfig/vehicleToBedConfig-search.model";
import { IVehicleToBedConfig }                           from "../vehicleToBedConfig/vehicleToBedConfig.model";
import { IVehicleToWheelBase }                           from "../vehicleToWheelBase/vehicleToWheelBase.model";
// vehicle to body style
import { IVehicleToWheelBaseSearchViewModel }      from "../vehicleToWheelBase/vehicleToWheelBase-search.model";
import { IVehicleToBodyStyleConfigSearchViewModel }      from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-search.model";
import { IVehicleToBodyStyleConfig }                     from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.model";

import { VehicleToBodyStyleConfigSearchComponent }       from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-search.component";
import { VehicleToWheelBaseSearchComponent }       from "../vehicleToWheelBase/vehicleToWheelBase-search.component";

import { IVehicleToMfrBodyCodeSearchViewModel }      from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode-search.model";
import { IVehicleToMfrBodyCode }                     from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode.model";
import {VehicleToMfrBodyCodeSearchComponent}  from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode-search.component";
import { IVehicleToDriveTypeSearchViewModel }      from "../vehicleToDriveType/vehicleToDriveType-search.model";
import { IVehicleToDriveType }                     from "../vehicleToDriveType/vehicleToDriveType.model";
import {VehicleToDriveTypeSearchComponent}  from "../vehicleToDriveType/vehicleToDriveType-search.component";

@Component({
    selector: "system-search",
    templateUrl: "app/templates/system/system-search.component.html",
})

export class SystemSearchComponent {
    private isSystemsMenuExpanded: boolean;
    private activeSubMenuGroup: string;
    private thresholdRecordCount: number = 100;
    // vehicle to brake config
    private vehicleToBrakeConfigSearchViewModel: IVehicleToBrakeConfigSearchViewModel;
    private vehicleToWheelBaseSearchViewModel: IVehicleToWheelBaseSearchViewModel;
    private vehicleToBrakeConfigsForSelectedBrake: IVehicleToBrakeConfig[];
    private vehicleToWheelBaseForSelectedWheelBase: IVehicleToWheelBase[];
    // vehicle to bed config
    private vehicleToBedConfigSearchViewModel: IVehicleToBedConfigSearchViewModel;
    private vehicleToBedConfigsForSelectedBed: IVehicleToBedConfig[];
    // vehicle to body style config
    private vehicleToBodyStyleConfigSearchViewModel: IVehicleToBodyStyleConfigSearchViewModel;
    private vehicleToBodyStyleConfigsForSelectedBodyStyle: IVehicleToBodyStyleConfig[];

    //Vehicle to MFRBodyCode
    private vehicleToMfrBodyCodeSearchViewModel: IVehicleToMfrBodyCodeSearchViewModel;
    private vehicleToMfrBodyCodesForSelectedMfrBodyCode: IVehicleToMfrBodyCode[];

    //Vehicle to DriveType
    private vehicleToDriveTypeSearchViewModel: IVehicleToDriveTypeSearchViewModel;
    private vehicleToDriveTypesForSelectedDriveType: IVehicleToDriveType[];

    @ViewChild("systemMenubarPanel") systemMenubarPanel: SystemMenuBar;
    @ViewChild("systemMenubarGrid") systemMenubarGrid: SystemMenuBar;
    @ViewChild("vehicletobrakeconfigsearch") vehicletobrakeconfigsearch: VehicleToBrakeConfigSearchComponent;
    @ViewChild("vehicletobedconfigsearch") vehicletobedconfigsearch: VehicleToBedConfigSearchComponent;
    @ViewChild("vehicletobodystyleconfigsearch") vehicletobodystyleconfigsearch: VehicleToBodyStyleConfigSearchComponent;
    @ViewChild("vehicletowheelbasesearch") vehicletowheelbasesearch: VehicleToWheelBaseSearchComponent;
    @ViewChild("vehicletomfrbodycodesearch") vehicletomfrbodycodesearch: VehicleToMfrBodyCodeSearchComponent;
    @ViewChild("vehicletodrivetypesearch") vehicletodrivetypesearch: VehicleToDriveTypeSearchComponent;

    constructor(private sharedService: SharedService, private router: Router, private toastr: ToastsManager,
        private navigationService: NavigationService) {
    }

    ngOnInit() {
        //this.activeSubMenuGroup = this.systemMenubar.activeSubMenuGroup;
        if (this.sharedService.systemMenubarSelected != null) {
            this.activeSubMenuGroup = this.sharedService.systemMenubarSelected;
        }
        else {
            this.activeSubMenuGroup = "Brake";//load brake system first
        }
        this.isSystemsMenuExpanded = true;
        // initialize vehicleToBrakeConfigSearchModel. this property will be passed to searchPanel component.
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
            , searchType: SearchType.None
        };
        // vehicle to bed config
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
                bedLengths: [],
                bedTypes: [],
            }
            , result: { bedConfigs: [], vehicleToBedConfigs: [] }
        };
        //this.vehicleToBedConfigsForSelectedBed = [];

        // vehicle to body style config
        this.vehicleToBodyStyleConfigSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: [],
                bodyNumDoors: [],
                bodyTypes: []
            },
            result: { bodyStyleConfigs: [], vehicleToBodyStyleConfigs: [] },
        };
        //vehicle to wheelbase 
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
            },
            result: { wheelBases: [], vehicleToWheelBases: [] },
        };
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
                mfrBodyCodes: []
            },
            result: { mfrBodyCodes: [], vehicleToMfrBodyCodes: [] },
        };
        this.vehicleToDriveTypeSearchViewModel = {
            facets: {
                startYears: [],
                endYears: [],
                regions: [],
                vehicleTypeGroups: [],
                vehicleTypes: [],
                makes: [],
                models: [],
                subModels: [],
                driveTypes: []
            },
            result: { driveTypes: [], vehicleToDriveTypes: [] },
        };
    }

    private onToggleMenuBarEvent(isSystemsMenuExpanded: boolean) {
        this.isSystemsMenuExpanded = isSystemsMenuExpanded;
    }

    private onSearchEvent(vehicleToBrakeConfigsForSelectedBrake: IVehicleToBrakeConfig[]) {
        this.vehicleToBrakeConfigsForSelectedBrake = vehicleToBrakeConfigsForSelectedBrake;
        this.vehicletobrakeconfigsearch.refreshGrids();
    }

    private onBedSearchEvent(vehicleToBedConfigsForSelectedBed: IVehicleToBedConfig[]) {
        this.vehicleToBedConfigsForSelectedBed = vehicleToBedConfigsForSelectedBed;
        this.vehicletobedconfigsearch.refreshGrids();
    }

    private onSelectedSubMenuGroupEvent(subMenuGroup: string) {
        this.activeSubMenuGroup = subMenuGroup;
        this.systemMenubarGrid.activeSubMenuGroup = subMenuGroup;
        this.systemMenubarPanel.activeSubMenuGroup = subMenuGroup;
        this.sharedService.systemMenubarSelected = subMenuGroup;
    }

    private onBodyStyleConfigSearchEvent(vehicleToBodyStyleConfigsForSelectedBodyStyle: IVehicleToBodyStyleConfig[]) {
        this.vehicleToBodyStyleConfigsForSelectedBodyStyle = vehicleToBodyStyleConfigsForSelectedBodyStyle;
        this.vehicletobodystyleconfigsearch.refreshGrids();
    }

    private onWheelBaseSearchEvent(vehicleToWheelBaseForSelectedWheelBase: IVehicleToWheelBase[]) {
        this.vehicleToWheelBaseForSelectedWheelBase = vehicleToWheelBaseForSelectedWheelBase;
        this.vehicletowheelbasesearch.refreshGrids();
    }

    private onMfrBodyCodeSearchEvent(vehicleToMfrBodyCodesForSelectedMfrBodyCode: IVehicleToMfrBodyCode[]) {
        this.vehicleToMfrBodyCodesForSelectedMfrBodyCode = vehicleToMfrBodyCodesForSelectedMfrBodyCode;
        this.vehicletomfrbodycodesearch.refreshGrids();
    }

    private onDriveTypeSearchEvent(vehicleToDriveTypesForSelectedDriveType: IVehicleToDriveType[]) {
        this.vehicleToDriveTypesForSelectedDriveType = vehicleToDriveTypesForSelectedDriveType;
        this.vehicletodrivetypesearch.refreshGrids();
    }
}
