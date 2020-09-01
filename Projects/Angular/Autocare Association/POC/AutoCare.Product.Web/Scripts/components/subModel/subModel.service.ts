import { Injectable } from "@angular/core";
import { ISubModel, ISubModelChangeRequestStagingReview } from "./subModel.model";
import { ConstantsWarehouse } from "../constants-warehouse";
import { HttpHelper } from "../httpHelper";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class SubModelService {
    constructor(private _httpHelper: HttpHelper) { }

    getAllSubModels() {
        return this._httpHelper.get<ISubModel[]>(ConstantsWarehouse.api.subModel);
    }

    getSubModels() {
        return this._httpHelper.get<ISubModel[]>(ConstantsWarehouse.api.subModel);
    }

    getSubModelDetail(id: number) {
        return this._httpHelper.get<ISubModel>(ConstantsWarehouse.api.subModel + "/" + id);
    }

    getFilteredSubModels(subModelNameFilter: string) {
        //NOTE: HTTP POST is used to allow valid special characters ('+', '.', '&')
        return this._httpHelper.post(ConstantsWarehouse.api.subModel + "/search", subModelNameFilter);
    }

    getSubModelsByBaseVehicleId(baseVehicleId: number) {
        return this._httpHelper.get<ISubModel[]>(ConstantsWarehouse.api.baseVehicle + "/" + baseVehicleId + "/subModels");
    }

    getSubModelsByMakeIdsAndModelIds(makeIds: number[], modelIds: number[]) {
        let makeIdFilter: string = ",";
        makeIds.forEach(item => makeIdFilter += item + ",");
        let modelIdFilter: string = ",";
        modelIds.forEach(item => modelIdFilter += item + ",");

        return this._httpHelper.get<ISubModel[]>(ConstantsWarehouse.api.make + "/" + makeIdFilter + "/models/" + modelIdFilter + "/subModels");
    }

    addSubModel(data) {
        return this._httpHelper.post(ConstantsWarehouse.api.subModel, data);
    }

    updateSubModel(id, data) {
        return this._httpHelper.put(ConstantsWarehouse.api.subModel + "/" + id, data);
    }

    deleteSubModel(id, data) {
        return this._httpHelper.post(ConstantsWarehouse.api.subModel + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this._httpHelper.get<ISubModelChangeRequestStagingReview>(ConstantsWarehouse.api.subModel + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(ConstantsWarehouse.api.subModel + '/changeRequestStaging/' + id, data);
    }
}