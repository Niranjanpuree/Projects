import {Injectable} from '@angular/core';
import {IFuelType, IFuelTypeChangeRequestReview} from './fuelType.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class FuelTypeService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllFuelTypes() {
        return this._httpHelper.get<IFuelType[]>(ConstantsWarehouse.api.fuelType);
    }

    getFuelTypeDetail(id: number) {
        debugger;
        return this._httpHelper.get<IFuelType>(ConstantsWarehouse.api.fuelType + '/' + id);
    }

    getFuelTypes(fuelTypeNameFilter: string) {
        return this._httpHelper.post(ConstantsWarehouse.api.fuelType + "/search", fuelTypeNameFilter);
    }

    getByFuelTypeId(fuelTypeId: Number) {
        let urlSearch: string = "/fuelType/" + fuelTypeId;
        return this._httpHelper.get<IFuelType[]>(ConstantsWarehouse.api.fuelType + urlSearch);
    }

    addFuelType(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.fuelType, data);
    }

    updateFuelType(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.fuelType + '/' + id, data);
    }

    deleteFuelType(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.fuelType + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IFuelTypeChangeRequestReview>(ConstantsWarehouse.api.fuelType + '/changeRequestStaging/' + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.fuelType + '/changeRequestStaging/' + id, data);
    }

}