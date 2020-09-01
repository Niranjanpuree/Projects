import { Injectable } from '@angular/core';
import { IFuelDeliverySubType, IFuelDeliverySubTypeChangeRequestStagingReview } from './fuelDeliverySubType.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import { HttpHelper } from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class FuelDeliverySubTypeService {

    constructor(private _httpHelper: HttpHelper) {
    }

    getAllFuelDeliverySubTypes() {
        return this._httpHelper.get<IFuelDeliverySubType[]>(ConstantsWarehouse.api.fuelDeliverySubType);
    }

    get() {
        return this._httpHelper.get<IFuelDeliverySubType[]>(ConstantsWarehouse.api.fuelDeliverySubType /*+ '/count/20'*/);
    }

    getByFilter(fuelDeliverySubTypeNameFilter: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(ConstantsWarehouse.api.fuelDeliverySubType + '/search', fuelDeliverySubTypeNameFilter);
    }
    add(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.fuelDeliverySubType, data);
    }

    update(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.fuelDeliverySubType + '/' + id, data);
    }

    delete(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.fuelDeliverySubType + '/delete/' + id, data);
    }

    getById(id) {
        return this._httpHelper.get<IFuelDeliverySubType>(ConstantsWarehouse.api.fuelDeliverySubType + '/' + id);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IFuelDeliverySubTypeChangeRequestStagingReview>(ConstantsWarehouse.api.fuelDeliverySubType + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.fuelDeliverySubType + '/changeRequestStaging/' + id, data);
    }
}