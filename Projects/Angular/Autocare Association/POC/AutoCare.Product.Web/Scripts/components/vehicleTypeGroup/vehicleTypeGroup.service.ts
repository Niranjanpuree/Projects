import { Injectable } from '@angular/core';
import {IVehicleTypeGroup, IVehicleTypeGroupChangeRequestStagingReview} from './vehicleTypeGroup.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {  HttpHelper } from '../httpHelper';
import { IChangeRequestReview }              from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class VehicleTypeGroupService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllVehicleTypeGroups() {
        return this._httpHelper.get<IVehicleTypeGroup[]>(ConstantsWarehouse.api.vehicleTypeGroup);
    }

    getVehicleTypeGroupDetail(id: number) {
        return this._httpHelper.get<IVehicleTypeGroup>(ConstantsWarehouse.api.vehicleTypeGroup + '/' + id);
    }

    getVehicleTypeGroups(vehicleTypeGroupNameFilter: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(ConstantsWarehouse.api.vehicleTypeGroup + '/search', vehicleTypeGroupNameFilter);
    }

    addVehicleTypeGroup(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.vehicleTypeGroup, data);
    }

    updateVehicleTypeGroup(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.vehicleTypeGroup + '/' + id, data);
    }

    deleteVehicleTypeGroupPost(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.vehicleTypeGroup + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IVehicleTypeGroupChangeRequestStagingReview>(ConstantsWarehouse.api.vehicleTypeGroup + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.vehicleTypeGroup + '/changeRequestStaging/' + id, data);
    }
}