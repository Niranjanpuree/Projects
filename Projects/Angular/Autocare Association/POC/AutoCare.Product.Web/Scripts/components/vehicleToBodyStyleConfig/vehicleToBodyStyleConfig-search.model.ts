import { IBodyStyleConfig } from "../bodyStyleConfig/bodyStyleConfig.model";
import { IVehicleToBodyStyleConfig } from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.model";
import { IFacet, SearchType } from "../shared/shared.model";

export interface IVehicleToBodyStyleConfigSearchInputModel {
    makes: string[];
    models: string[];
    subModels: string[];
    startYear: string;
    endYear: string;
    regions: string[];
    bodyNumDoors: string[];
    bodyTypes: string[];
    vehicleTypeGroups: string[];
    vehicleTypes: string[];
}

export interface IVehicleToBodyStyleConfigSearchViewModel {
    facets?: IVehicleToBodyStylesConfigSearchFacets;
    result?: IVehicleToBodyStyleConfigSearchResult;
    totalCount?: number;
    // note: not from controller- used for navigation purpose
    searchType?: SearchType;
}

export interface IVehicleToBodyStylesConfigSearchFacets {
    makes: IFacet[];
    models: IFacet[];
    subModels: IFacet[];
    // todo: check if instead of two properties for startYears and endYears can be accomodated into 1 years.
    // note: not required.
    startYears: string[];
    endYears: string[];
    regions: IFacet[];
    vehicleTypeGroups: IFacet[];
    vehicleTypes: IFacet[];
    bodyNumDoors: IFacet[];
    bodyTypes: IFacet[];
}

export interface IVehicleToBodyStyleConfigSearchResult {
    // todo: check if bodyStyleConfigId and vehicleId are also required.
    bodyStyleConfigs: IBodyStyleConfig[];
    vehicleToBodyStyleConfigs: IVehicleToBodyStyleConfig[];
}

//export enum SearchType {
//    None,
//    GeneralSearch,
//    SearchByBedConfigId,
//}