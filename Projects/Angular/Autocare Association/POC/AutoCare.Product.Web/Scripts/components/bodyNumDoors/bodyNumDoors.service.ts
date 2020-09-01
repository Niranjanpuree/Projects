import {Injectable} from '@angular/core';
import {IBodyNumDoors, IBodyNumDoorsChangeRequestReview} from './bodyNumDoors.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class BodyNumDoorsService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllBodyNumDoors() {
        return this._httpHelper.get<IBodyNumDoors[]>(ConstantsWarehouse.api.bodyNumDoors);
    }

    getBodyNumDoorsDetail(id: number) {
        return this._httpHelper.get<IBodyNumDoors>(ConstantsWarehouse.api.bodyNumDoors + '/' + id);
    }

    getBodyNumDoors(bodyNumDoorsNameFilter: string) {
      return this._httpHelper.post(ConstantsWarehouse.api.bodyNumDoors + "/search", bodyNumDoorsNameFilter);
    }

    addBodyNumDoors(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.bodyNumDoors, data);
    }

    updateBodyNumDoors(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.bodyNumDoors + '/' + id, data);
    }

    deleteBodyNumDoors(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.bodyNumDoors + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IBodyNumDoorsChangeRequestReview>(ConstantsWarehouse.api.bodyNumDoors + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.bodyNumDoors + '/changeRequestStaging/' + id, data);
    }
    getByBodyNumDoorsId(bodyNumberDoorsId: Number) {
        let urlSearch: string = "/bodyNumDoors/" + bodyNumberDoorsId;
        return this._httpHelper.get<IBodyNumDoors[]>(ConstantsWarehouse.api.bodyNumDoors + urlSearch);
    }
}