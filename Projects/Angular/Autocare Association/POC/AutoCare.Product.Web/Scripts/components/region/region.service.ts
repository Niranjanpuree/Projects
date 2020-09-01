import {Injectable} from '@angular/core';
import {IRegion, IParentRegion, IRegionViewModel, IRegionChangeRequestStagingReview } from './region.model';
import 'rxjs/Rx';
import {Http, Response} from '@angular/http';
import {ConstantsWarehouse} from '../constants-warehouse';
import {HttpHelper} from '../httpHelper';
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class RegionService {
    constructor(private _httpHelper: HttpHelper) { }

    getRegion() {
        return this._httpHelper.get<IRegion[]>(ConstantsWarehouse.api.region);
    }

    getRegionDetail(id: number) {
        return this._httpHelper.get<IRegion>(ConstantsWarehouse.api.region + '/' + id);
    }

    getRegionByNameFilter(regionNameFilter: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(ConstantsWarehouse.api.region + '/search', regionNameFilter);
    }

    getRegionsByBaseVehicleIdAndSubModelId(baseVehicleId: number, subModelId: number) {
        return this._httpHelper.get<IRegion[]>(ConstantsWarehouse.api.baseVehicle + '/' + baseVehicleId + '/subModels/' + subModelId + '/regions');
    }

    addRegion(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.region, data);
    }

    updateRegion(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.region + '/' + id, data);
    }

    deleteRegionPost(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.region + '/delete/' + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<IRegionChangeRequestStagingReview>(ConstantsWarehouse.api.region + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.region + '/changeRequestStaging/' + id, data);
    }
}