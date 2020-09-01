import {Injectable} from '@angular/core';
import {IBrakeABS, IBrakeABSChangeRequestReview} from './brakeABS.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class BrakeABSService {
    constructor(private httpHelper: HttpHelper) { }

    getAllBrakeABSes() {
        return this.httpHelper.get<IBrakeABS[]>(ConstantsWarehouse.api.brakeABS);
    }

    getBrakeABSDetail(id: number) {
        return this.httpHelper.get<IBrakeABS>(ConstantsWarehouse.api.brakeABS + '/' + id);
    }

    getBrakeABSs(brakeABSNameFilter: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this.httpHelper.post(ConstantsWarehouse.api.brakeABS + "/search", brakeABSNameFilter);
    }

    getByFrontBrakeTypeIdRearBrakeTypeId(frontBrakeTypeId: Number, rearBrakeTypeId: Number) {
        let urlSearch: string = "/frontBrakeType/" + frontBrakeTypeId + "/rearBrakeType/" + rearBrakeTypeId;
        return this.httpHelper.get<IBrakeABS[]>(ConstantsWarehouse.api.brakeABS + urlSearch);
    }

    addBrakeABS(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.brakeABS, data);
    }

    updateBrakeABS(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.brakeABS + '/' + id, data);
    }

    deleteBrakeABS(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.brakeABS + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: number) {
        return this.httpHelper.get<IBrakeABSChangeRequestReview>(ConstantsWarehouse.api.brakeABS + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.brakeABS + '/changeRequestStaging/' + id, data);
    }
}