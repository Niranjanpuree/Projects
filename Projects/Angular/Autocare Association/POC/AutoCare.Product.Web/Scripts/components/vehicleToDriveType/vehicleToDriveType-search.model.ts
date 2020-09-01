import { IVehicleToDriveType} from "./vehicleToDriveType.model";
import { IMake } from "../make/make.model";
import { IModel } from "../model/model.model";
import { ISubModel } from "../subModel/subModel.model";
import { IVehicleType } from "../vehicleType/vehicleType.model";
import { IVehicleTypeGroup } from "../vehicleTypeGroup/vehicleTypeGroup.model";
import {IDriveType } from "../driveType/driveType.model"

export interface IVehicleToDriveTypeSearchInputModel {
    driveTypeId?: number;  //NOTE: driveTypeId should be combined with other inputs in drive type replace screen
    startYear: string,
    endYear: string,
    regions: string[],
    vehicleTypeGroups: string[],
    vehicleTypes: string[],
    makes: string[],
    models: string[],
    subModels: string[],
    driveTypes:string[],
}
export interface IVehicleToDriveTypeSearchViewModel {
    facets?: IVehicleToDriveTypeSearchFacets;
    result?: IVehicleToDriveTypeSearchResult;
    totalCount?: number;
    searchType?: SearchType;
}

export interface IVehicleToDriveTypeSearchFacets {
    startYears: string[],
    endYears: string[],
    regions: IFacet[],
    vehicleTypeGroups: IFacet[],
    vehicleTypes: IFacet[],
    makes: IFacet[],
    models: IFacet[],
    subModels: IFacet[],
    driveTypes:IFacet[],
}

export interface IFacet {
    id?: string;
    name: string;
    isSelected?: boolean;
}

export interface IVehicleToDriveTypeSearchResult {
    driveTypes: IDriveType[],
    vehicleToDriveTypes: IVehicleToDriveType[],
}

export enum SearchType {
    None,
    GeneralSearch,
    SearchByDriveTypeId,
}