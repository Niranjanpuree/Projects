import {Injectable} from '@angular/core';
import {IEngineVersion, IEngineVersionChangeRequestReview} from './engineVersion.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class EngineVersionService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllEngineVersions() {
        return this._httpHelper.get<IEngineVersion[]>(ConstantsWarehouse.api.engineVersion);
    }

    getEngineVersionDetail(id: number) {
        debugger;
        return this._httpHelper.get<IEngineVersion>(ConstantsWarehouse.api.engineVersion + '/' + id);
    }

    getEngineVersions(engineVersionNameFilter: string) {
        return this._httpHelper.post(ConstantsWarehouse.api.engineVersion + "/search", engineVersionNameFilter);
    }

    getByEngineVersionId(engineVersionId: Number) {
        let urlSearch: string = "/engineVersion/" + engineVersionId;
        return this._httpHelper.get<IEngineVersion[]>(ConstantsWarehouse.api.engineVersion + urlSearch);
    }

    getByBedLengthId(engineDesignatioId: number) {
        let urlSearch: string = "/engineVersionByBedLength/" + engineDesignatioId;
        return this._httpHelper.get<IEngineVersion[]>(ConstantsWarehouse.api.engineVersion + urlSearch);
    }
    addEngineVersion(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.engineVersion, data);
    }

    updateEngineVersion(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.engineVersion + '/' + id, data);
    }

    deleteEngineVersion(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.engineVersion + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IEngineVersionChangeRequestReview>(ConstantsWarehouse.api.engineVersion + '/changeRequestStaging/' + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.engineVersion + '/changeRequestStaging/' + id, data);
    }

}