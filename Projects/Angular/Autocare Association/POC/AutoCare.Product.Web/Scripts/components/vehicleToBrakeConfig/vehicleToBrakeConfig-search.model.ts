import { IBrakeConfig } from "../brakeConfig/brakeConfig.model";
import { IVehicleToBrakeConfig } from "../vehicleToBrakeConfig/vehicleToBrakeConfig.model";
import { IMake } from "../make/make.model";
import { IModel } from "../model/model.model";
import { ISubModel } from "../subModel/subModel.model";
import { IVehicleType } from "../vehicleType/vehicleType.model";
import { IVehicleTypeGroup } from "../vehicleTypeGroup/vehicleTypeGroup.model";
import { IBrakeABS } from "../brakeABS/brakeABS.model";
import { IBrakeSystem } from "../brakeSystem/brakeSystem.model";
import { IBrakeType } from "../brakeType/brakeType.model";
import { IRegion } from "../region/region.model";

export interface IVehicleToBrakeConfigSearchInputModel {
    brakeConfigId?: number;  //NOTE: brakeConfigId should be combined with other inputs in brake config replace screen
    startYear: string,
    endYear: string,
    regions: string[],
    vehicleTypeGroups: string[],
    vehicleTypes: string[],
    makes: string[],
    models: string[],
    subModels: string[],
    frontBrakeTypes: string[],
    rearBrakeTypes: string[],
    brakeAbs: string[],
    brakeSystems: string[],
}

export interface IVehicleToBrakeConfigSearchViewModel {
    facets?: IVehicleToBrakeConfigSearchFacets;
    result?: IVehicleToBrakeConfigSearchResult;
    totalCount?: number;
    searchType?: SearchType;
}

export interface IVehicleToBrakeConfigSearchFacets {
    startYears: string[],
    endYears: string[],
    regions: IFacet[],
    vehicleTypeGroups: IFacet[],
    vehicleTypes: IFacet[],
    makes: IFacet[],
    models: IFacet[],
    subModels: IFacet[],
    frontBrakeTypes: IFacet[],
    rearBrakeTypes: IFacet[],
    brakeAbs: IFacet[],
    brakeSystems: IFacet[],
}

export interface IFacet {
    id?: string;
    name: string;
    isSelected?: boolean;
}

export interface IVehicleToBrakeConfigSearchResult {
    brakeConfigs: IBrakeConfig[],
    vehicleToBrakeConfigs: IVehicleToBrakeConfig[],
}

export enum SearchType {
    None,
    GeneralSearch,
    SearchByBrakeConfigId,
}