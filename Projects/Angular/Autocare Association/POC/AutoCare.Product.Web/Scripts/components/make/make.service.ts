import { Injectable } from '@angular/core';
import { IMake, IMakeChangeRequestStagingReview } from './make.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import { HttpHelper } from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class MakeService {

    constructor(private _httpHelper: HttpHelper) {
    }

    getAllMakes() {
        return this._httpHelper.get<IMake[]>(ConstantsWarehouse.api.make);
    }

    get() {
        return this._httpHelper.get<IMake[]>(ConstantsWarehouse.api.make /*+ '/count/20'*/);
    }

    getByFilter(makeNameFilter: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(ConstantsWarehouse.api.make + '/search', makeNameFilter);
    }

    getMakesByYearId(yearId: number) {
        return this._httpHelper.get<IMake[]>(ConstantsWarehouse.api.year + "/" + yearId + '/makes');
    }

    add(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.make, data);
    }

    update(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.make + '/' + id, data);
    }

    delete(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.make + '/delete/' + id, data);
    }

    getById(id) {
        return this._httpHelper.get<IMake>(ConstantsWarehouse.api.make + '/' + id);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IMakeChangeRequestStagingReview>(ConstantsWarehouse.api.make + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.make + '/changeRequestStaging/' + id, data);
    }
}