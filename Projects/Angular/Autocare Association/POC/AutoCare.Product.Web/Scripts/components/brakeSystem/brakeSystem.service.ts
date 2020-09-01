import {Injectable} from '@angular/core';
import {IBrakeSystem, IBrakeSystemChangeRequestReview } from './brakeSystem.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class BrakeSystemService {
    constructor(private httpHelper: HttpHelper) { }

    getAllBrakeSystems() {
        return this.httpHelper.get<IBrakeSystem[]>(ConstantsWarehouse.api.brakeSystem);
    }

    getBrakeSystemDetail(id: number) {
        return this.httpHelper.get<IBrakeSystem>(ConstantsWarehouse.api.brakeSystem + '/' + id);
    }

    getBrakeSystems(brakeSystemNameFilter: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this.httpHelper.post(ConstantsWarehouse.api.brakeSystem + "/search", brakeSystemNameFilter);
    }

    getByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId(frontBrakeTypeId: Number, rearBrakeTypeId: Number, brakeABSId: Number) {
        let urlSearch: string = "/frontBrakeType/" + frontBrakeTypeId + "/rearBrakeType/" + rearBrakeTypeId + "/brakeABS/" + brakeABSId;
        return this.httpHelper.get<IBrakeSystem[]>(ConstantsWarehouse.api.brakeSystem + urlSearch);
    }

    addBrakeSystem(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.brakeSystem, data);
    }

    updateBrakeSystem(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.brakeSystem + '/' + id, data);
    }

    deleteBrakeSystem(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.brakeSystem + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: number) {
        return this.httpHelper.get<IBrakeSystemChangeRequestReview>(ConstantsWarehouse.api.brakeSystem + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.brakeSystem + '/changeRequestStaging/' + id, data);
    }
}