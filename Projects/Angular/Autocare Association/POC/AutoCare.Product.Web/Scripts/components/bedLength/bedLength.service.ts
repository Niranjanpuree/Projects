import {Injectable} from '@angular/core';
import {IBedLength, IBedLengthChangeRequestReview} from './bedLength.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class BedLengthService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllBedLengths() {
        return this._httpHelper.get<IBedLength[]>(ConstantsWarehouse.api.bedLength);
    }

    getBedLengthDetail(id: number) {
        return this._httpHelper.get<IBedLength>(ConstantsWarehouse.api.bedLength + '/' + id);
    }

    getBedLength(bedLengthNameFilter: string) {
        return this._httpHelper.post(ConstantsWarehouse.api.bedLength + "/search", bedLengthNameFilter);
    }

    addBedLength(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.bedLength, data);
    }

    updateBedLength(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.bedLength + '/' + id, data);
    }

    deleteBedLength(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.bedLength + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IBedLengthChangeRequestReview>(ConstantsWarehouse.api.bedLength + '/changeRequestStaging/' + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.bedLength + '/changeRequestStaging/' + id, data);
    }
    getByBedLengthId(bedLengthId: Number) {
        let urlSearch: string = "/bedLength/" + bedLengthId;
        return this._httpHelper.get<IBedLength[]>(ConstantsWarehouse.api.bedLength + urlSearch);
    }
}