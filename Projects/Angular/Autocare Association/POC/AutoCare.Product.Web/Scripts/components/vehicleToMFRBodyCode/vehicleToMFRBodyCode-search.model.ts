import { IVehicleToMfrBodyCode } from "./vehicleToMfrBodyCode.model";
import { IMake } from "../make/make.model";
import { IModel } from "../model/model.model";
import { ISubModel } from "../subModel/subModel.model";
import { IVehicleType } from "../vehicleType/vehicleType.model";
import { IVehicleTypeGroup } from "../vehicleTypeGroup/vehicleTypeGroup.model";
import {IMfrBodyCode} from "../mfrBodyCode/mfrBodyCode.model"

export interface IVehicleToMfrBodyCodeSearchInputModel {
    mfrBodyCodeId?: number;  //NOTE: mfrBodyCodeId should be combined with other inputs in mfrBodyCode replace screen
    startYear: string,
    endYear: string,
    regions: string[],
    vehicleTypeGroups: string[],
    vehicleTypes: string[],
    makes: string[],
    models: string[],
    subModels: string[],
    mfrBodyCodes:string[],
}
export interface IVehicleToMfrBodyCodeSearchViewModel {
    facets?: IVehicleToMfrBodyCodeSearchFacets;
    result?: IVehicleToMfrBodyCodeSearchResult;
    totalCount?: number;
    searchType?: SearchType;
}

export interface IVehicleToMfrBodyCodeSearchFacets {
    startYears: string[],
    endYears: string[],
    regions: IFacet[],
    vehicleTypeGroups: IFacet[],
    vehicleTypes: IFacet[],
    makes: IFacet[],
    models: IFacet[],
    subModels: IFacet[],
    mfrBodyCodes:IFacet[],
}

export interface IFacet {
    id?: string;
    name: string;
    isSelected?: boolean;
}

export interface IVehicleToMfrBodyCodeSearchResult {
    mfrBodyCodes: IMfrBodyCode[],
    vehicleToMfrBodyCodes: IVehicleToMfrBodyCode[],
}

export enum SearchType {
    None,
    GeneralSearch,
    SearchByMfrBodyCodeId,
}