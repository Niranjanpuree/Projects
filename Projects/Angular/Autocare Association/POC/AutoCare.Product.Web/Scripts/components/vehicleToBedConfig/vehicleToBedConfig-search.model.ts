import { IBedConfig } from "../bedConfig/bedConfig.model";
import { IVehicleToBedConfig } from "../vehicleToBedConfig/vehicleToBedConfig.model";

export interface IVehicleToBedConfigSearchInputModel {
    startYear: string,
    endYear: string,
    regions: string[],
    vehicleTypeGroups: string[],
    vehicleTypes: string[],
    bedLengths: string[],
    bedTypes: string[],
    makes: string[],
    models: string[],
    subModels: string[],
}

export interface IVehicleToBedConfigSearchViewModel {
    facets?: IVehicleToBedConfigSearchFacets,
    result?: IVehicleToBedConfigSearchResult,
    searchType?: SearchType;
    totalCount?: number;
}

export interface IVehicleToBedConfigSearchFacets {
    bedTypes: IFacet[],
    bedLengths: IFacet[],
    startYears: string[],
    endYears: string[],
    regions: IFacet[],
    vehicleTypeGroups: IFacet[],
    vehicleTypes: IFacet[],
    makes: IFacet[],
    models: IFacet[],
    subModels: IFacet[],
}

export interface IFacet {
    id?: string,
    name: string,
    isSelected?: boolean
}

export interface IVehicleToBedConfigSearchResult {
    bedConfigs: IBedConfig[],
    vehicleToBedConfigs: IVehicleToBedConfig[],
}

export enum SearchType {
    None,
    GeneralSearch,
    SearchByBedConfigId,
}