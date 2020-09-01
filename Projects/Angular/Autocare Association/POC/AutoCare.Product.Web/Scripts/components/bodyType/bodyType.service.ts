import {Injectable} from '@angular/core';
import {IBodyType, IBodyTypeChangeRequestReview} from './bodyType.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class BodyTypeService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllBodyTypes() {
        return this._httpHelper.get<IBodyType[]>(ConstantsWarehouse.api.bodyType);
    }

    getByBodyTypeId(bodyTypeId: Number) {
       let urlSearch: string = "/bodyType/" + bodyTypeId;
        return this._httpHelper.get<IBodyType[]>(ConstantsWarehouse.api.bodyType + urlSearch);
    }

    getByBodyNumDoorsId(bodyNumDoorsId: number) {
        let urlSearch: string = "/bodyNumDoors/" + bodyNumDoorsId;
        return this._httpHelper.get<IBodyType[]>(ConstantsWarehouse.api.bodyType + urlSearch);
    }
    getBodyTypeDetail(id: number) {
        return this._httpHelper.get<IBodyType>(ConstantsWarehouse.api.bodyType + '/' + id);
    }

    getBodyTypes(bodyTypeNameFilter: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(ConstantsWarehouse.api.bodyType + "/search", bodyTypeNameFilter);
    }

    addBodyType(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.bodyType, data);
    }

    updateBodyType(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.bodyType + '/' + id, data);
    }

    deleteBodyType(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.bodyType + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IBodyTypeChangeRequestReview>(ConstantsWarehouse.api.bodyType + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.bodyType + '/changeRequestStaging/' + id, data);
    }
}