import {Injectable} from '@angular/core';
import {IBedType, IBedTypeChangeRequestReview} from './bedType.model';
import { ConstantsWarehouse } from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class BedTypeService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllBedTypes() {
        return this._httpHelper.get<IBedType[]>(ConstantsWarehouse.api.bedType);
    }

    getBedTypeDetail(id: number) {
        return this._httpHelper.get<IBedType>(ConstantsWarehouse.api.bedType + '/' + id);
    }

    getBedTypes(bedTypeNameFilter: string) {
        return this._httpHelper.post(ConstantsWarehouse.api.bedType + "/search", bedTypeNameFilter);
    }

    getByBedTypeId(bedTypeId: Number) {
        let urlSearch: string = "/bedType/" + bedTypeId;
        return this._httpHelper.get<IBedType[]>(ConstantsWarehouse.api.bedType + urlSearch);
    }

    getByBedLengthId(bedLengthId: number) {
        let urlSearch: string = "/bedTypeByBedLength/" + bedLengthId;
        return this._httpHelper.get<IBedType[]>(ConstantsWarehouse.api.bedType + urlSearch);
    }
    addBedType(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.bedType, data);
    }

    updateBedType(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.bedType + '/' + id, data);
    }

    deleteBedType(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.bedType + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IBedTypeChangeRequestReview>(ConstantsWarehouse.api.bedType + '/changeRequestStaging/' + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.bedType + '/changeRequestStaging/' + id, data);
    }

}