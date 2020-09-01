import { IBaseVehicle } from "../baseVehicle/baseVehicle.model"
import { IVehicle } from "./vehicle.model"

export interface IVehicleSearchInputModel {
    startYear: string,
    endYear: string,
    regions: string[],
    vehicleTypeGroups: string[],
    vehicleTypes: string[],
    makes: string[],
    models: string[],
    subModels: string[],
}

export interface IVehicleSearchViewModel {
    facets?: IVehicleSearchFacets,
    result?: IVehicleSearchResult,
    totalCount?: number,
    searchType: SearchType;
}

export interface IVehicleSearchFacets {
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

export interface IVehicleSearchResult {
    baseVehicles: IBaseVehicle[],
    vehicles: IVehicle[],
}

export enum ConfigurationSystems {
    Select,
    Brake,
    Bed,
    Body,
    Engine,
    Wheel,
    Drive,
    MFR
}

export enum SearchType {
    None,
    GeneralSearch,
    SearchByBaseVehicleId,
    SearchByVehicleId,
}

export enum FacetType {
    Region,
    VehicleType,
    VehicleTypeGroup,
    Year,
    Make,
    Model,
    SubModel
}