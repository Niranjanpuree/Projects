import { Injectable } from '@angular/core';
import { IModel, IModelChangeRequestReview } from './model.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import { HttpHelper } from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class ModelService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllModels() {
        return this._httpHelper.get<IModel[]>(ConstantsWarehouse.api.model);
    }

    getModels() {
        return this._httpHelper.get<IModel[]>(ConstantsWarehouse.api.model);
    }

    getFilteredModels(modelNameFilter: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(ConstantsWarehouse.api.model + '/search', modelNameFilter);
    }

    getModelsByMakeIds(makeIds?: number[]) {
        if (makeIds != null) {
            let makeIdFilter: string = '/,';    //note: if makeIds is empty then send a comma (,) so that /api/makes//models/, is hit
            makeIds.forEach(item => makeIdFilter += item + ',');

            return this._httpHelper.get<IModel[]>(ConstantsWarehouse.api.make + makeIdFilter + '/models');
        }
    }

    getModelsByYearIdAndMakeId(yearId: number, makeId: number) {
        return this._httpHelper.get<IModel[]>(ConstantsWarehouse.api.year + "/" + yearId + "/makes" + makeId + '/models');
    }

    getModelDetail(id) {
        return this._httpHelper.get<IModel>(ConstantsWarehouse.api.model + '/' + id);
    }

    addModel(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.model, data);
    }

    updateModel(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.model + '/' + id, data);
    }

    deleteModel(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.model + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IModelChangeRequestReview>(ConstantsWarehouse.api.model + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.model + '/changeRequestStaging/' + id, data);
    }
}