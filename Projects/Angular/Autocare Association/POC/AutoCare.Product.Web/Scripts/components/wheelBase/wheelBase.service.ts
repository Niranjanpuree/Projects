import { Injectable } from '@angular/core';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";
import {IWheelBase} from './wheelBase.model';
import {IWheelBaseChangeRequestStagingReview} from './wheelBase.model';

@Injectable()
export class WheelBaseService {
    public existingWheelBase: IWheelBase;
    public replacementWheelBase: IWheelBase;

    constructor(private httpHelper: HttpHelper) {}

    getAllWheelBase() {
        return this.httpHelper.get<IWheelBase[]>(ConstantsWarehouse.api.wheelBase);
    }

    getWheelBaseDetail(id: number) {
        return this.httpHelper.get<IWheelBase>(ConstantsWarehouse.api.wheelBase + '/' + id);
    }

    getWheelBasebyid(id) {
        return this.httpHelper.get<IWheelBase>(ConstantsWarehouse.api.wheelBase + "/" + id);
    }

    getByBase(data: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this.httpHelper.post(ConstantsWarehouse.api.wheelBase + "/getByWheelBaseName", data);
    }

    getByChildNames(baseName: string, wheelBaseMetric: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        let data = {
            base: baseName, wheelBaseMetric: wheelBaseMetric
        }
        return this.httpHelper.post(ConstantsWarehouse.api.wheelBase + "/getByChildNames", data);
    }

    addWheelBase(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.wheelBase, data);
    }

    updateWheelBase(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.wheelBase + '/' + id, data);
    }

    replaceWheelBase(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.wheelBase + "/replace/" + id, data);
    }

    deleteWheelBase(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.wheelBase + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this.httpHelper.get<IWheelBaseChangeRequestStagingReview>(ConstantsWarehouse.api.wheelBase + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.wheelBase + '/changeRequestStaging/' + id, data);
    }
}