import {Injectable} from '@angular/core';
import {IBrakeType, IBrakeTypeChangeRequestReview} from './brakeType.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class BrakeTypeService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllBrakeTypes() {
        return this._httpHelper.get<IBrakeType[]>(ConstantsWarehouse.api.brakeType);
    }

    getBrakeTypeDetail(id: number) {
        return this._httpHelper.get<IBrakeType>(ConstantsWarehouse.api.brakeType + '/' + id);
    }

    getBrakeTypes(brakeTypeNameFilter: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(ConstantsWarehouse.api.brakeType + "/search", brakeTypeNameFilter);
    }

    getByFrontBrakeTypeId(frontBrakeTypeId: Number) {
        let urlSearch: string = "/frontBrakeType/" + frontBrakeTypeId;
        return this._httpHelper.get<IBrakeType[]>(ConstantsWarehouse.api.brakeType + urlSearch);
    }

    addBrakeType(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.brakeType, data);
    }

    updateBrakeType(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.brakeType + '/' + id, data);
    }

    deleteBrakeType(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.brakeType + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IBrakeTypeChangeRequestReview>(ConstantsWarehouse.api.brakeType + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.brakeType + '/changeRequestStaging/' + id, data);
    }

}