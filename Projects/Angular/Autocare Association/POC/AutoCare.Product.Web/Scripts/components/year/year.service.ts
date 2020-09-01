import { Injectable } from '@angular/core';
import { IYear,IYearViewModel, IYearChangeRequestStagingReview } from './year.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class YearService {
    constructor(private _httpHelper: HttpHelper) { }

    getYears() {
        return this._httpHelper.get<IYear[]>(ConstantsWarehouse.api.year);
    }

    addYear(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.year, data);
    }

    getDependencies(id) {
        return this._httpHelper.get<IYear>(ConstantsWarehouse.api.year + '/dependencies/' + id);
    }

    delete(id) {
        return this._httpHelper.delete(ConstantsWarehouse.api.year + '/' + id);
    }

    deleteYear(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.year + '/delete/' + id, data);
    }
    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IYearChangeRequestStagingReview>(ConstantsWarehouse.api.year + '/changeRequestStaging/' + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.year + '/changeRequestStaging/' + id, data);
    }
}