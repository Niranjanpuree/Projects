import {Injectable} from '@angular/core';
import {IEngineDesignation, IEngineDesignationChangeRequestReview} from './engineDesignation.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class EngineDesignationService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllEngineDesignations() {
        return this._httpHelper.get<IEngineDesignation[]>(ConstantsWarehouse.api.engineDesignation);
    }

    getEngineDesignationDetail(id: number) {
        debugger;
        return this._httpHelper.get<IEngineDesignation>(ConstantsWarehouse.api.engineDesignation + '/' + id);
    }

    getEngineDesignations(engineDesignationNameFilter: string) {
        return this._httpHelper.post(ConstantsWarehouse.api.engineDesignation + "/search", engineDesignationNameFilter);
    }

    getByEngineDesignationId(engineDesignationId: Number) {
        let urlSearch: string = "/engineDesignation/" + engineDesignationId;
        return this._httpHelper.get<IEngineDesignation[]>(ConstantsWarehouse.api.engineDesignation + urlSearch);
    }

    getByBedLengthId(engineDesignatioId: number) {
        let urlSearch: string = "/engineDesignationByBedLength/" + engineDesignatioId;
        return this._httpHelper.get<IEngineDesignation[]>(ConstantsWarehouse.api.engineDesignation + urlSearch);
    }
    addEngineDesignation(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.engineDesignation, data);
    }

    updateEngineDesignation(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.engineDesignation + '/' + id, data);
    }

    deleteEngineDesignation(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.engineDesignation + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IEngineDesignationChangeRequestReview>(ConstantsWarehouse.api.engineDesignation + '/changeRequestStaging/' + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.engineDesignation + '/changeRequestStaging/' + id, data);
    }

}