import {Injectable} from '@angular/core';
import {IEngineVin, IEngineVinChangeRequestReview} from './engineVin.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class EngineVinService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllEngineVins() {
        return this._httpHelper.get<IEngineVin[]>(ConstantsWarehouse.api.engineVin);
    }

    getEngineVinDetail(id: number) {
        debugger;
        return this._httpHelper.get<IEngineVin>(ConstantsWarehouse.api.engineVin + '/' + id);
    }

    getEngineVins(engineVinNameFilter: string) {
        return this._httpHelper.post(ConstantsWarehouse.api.engineVin + "/search", engineVinNameFilter);
    }

    getByEngineVinId(engineVinId: Number) {
        let urlSearch: string = "/engineVin/" + engineVinId;
        return this._httpHelper.get<IEngineVin[]>(ConstantsWarehouse.api.engineVin + urlSearch);
    }

    getByBedLengthId(engineDesignatioId: number) {
        let urlSearch: string = "/engineVinByBedLength/" + engineDesignatioId;
        return this._httpHelper.get<IEngineVin[]>(ConstantsWarehouse.api.engineVin + urlSearch);
    }
    addEngineVin(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.engineVin, data);
    }

    updateEngineVin(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.engineVin + '/' + id, data);
    }

    deleteEngineVin(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.engineVin + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IEngineVinChangeRequestReview>(ConstantsWarehouse.api.engineVin + '/changeRequestStaging/' + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.engineVin + '/changeRequestStaging/' + id, data);
    }

}