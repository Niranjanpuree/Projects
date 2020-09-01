import { IVehicleToWheelBase } from "../vehicleToWheelBase/vehicleToWheelBase.model";
import { IMake } from "../make/make.model";
import { IModel } from "../model/model.model";
import { ISubModel } from "../subModel/subModel.model";
import { IVehicleType } from "../vehicleType/vehicleType.model";
import { IVehicleTypeGroup } from "../vehicleTypeGroup/vehicleTypeGroup.model";
import { IRegion } from "../region/region.model";
import { IWheelBase } from "../wheelBase/wheelBase.model";

export interface IVehicleToWheelBaseSearchInputModel {
    wheelBaseId?: number; 
    startYear: string,
    endYear: string,
    regions: string[],
    vehicleTypeGroups: string[],
    vehicleTypes: string[],
    makes: string[],
    models: string[],
    subModels: string[],
  }

export interface IVehicleToWheelBaseSearchViewModel {
    facets?: IVehicleToWheelBaseSearchFacets;
    result?: IVehicleToWheelBaseSearchResult;
    totalCount?: number;
    searchType?: SearchType;
}

export interface IVehicleToWheelBaseSearchFacets {
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
    id?: string;
    name: string;
    isSelected?: boolean;
}
export interface IWheelBaseFacet {
    id?: string;
    wheelBaseMetric:string;
    isSelected?: boolean;
}

export interface IVehicleToWheelBaseSearchResult {
    wheelBases: IWheelBase[],
    vehicleToWheelBases: IVehicleToWheelBase[],
}

export enum SearchType {
    None,
    GeneralSearch,
    SearchByWheelBaseId,
}