import {Injectable} from '@angular/core';
import {IVehicleType, IVehicleTypeChangeRequestStagingReview} from './vehicleType.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class VehicleTypeService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllVehicleTypes() {
        return this._httpHelper.get<IVehicleType[]>(ConstantsWarehouse.api.vehicleType);
    }

    getVehicleTypeDetail(id: number) {
        return this._httpHelper.get<IVehicleType>(ConstantsWarehouse.api.vehicleType + '/' + id);
    }

    getVehicleTypes(vehicleTypeNameFilter: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(ConstantsWarehouse.api.vehicleType + '/search', vehicleTypeNameFilter);
    }

    addVehicleType(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.vehicleType, data);
    }

    updateVehicleType(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.vehicleType + '/' + id, data);
    }

    deleteVehicleTypePost(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.vehicleType + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IVehicleTypeChangeRequestStagingReview>(ConstantsWarehouse.api.vehicleType + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.vehicleType + '/changeRequestStaging/' + id, data);
    }
}