import { Injectable }                               from "@angular/core";
import { IBaseVehicle }                             from "./baseVehicle.model";
import { IModel }                                   from "../model/model.model";
import { ConstantsWarehouse }                       from "../constants-warehouse";
import { HttpHelper }                               from "../httpHelper";
import { IBaseVehicleChangeRequestStagingReview }   from "./baseVehicle.model";
import { IChangeRequestReview }                     from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class BaseVehicleService {
    existingBaseVehicle: IBaseVehicle;
    replacementBaseVehicle: IBaseVehicle;

    constructor(private httpHelper: HttpHelper) { }

    getBaseVehicles() {
        return this.httpHelper.get<IBaseVehicle[]>(ConstantsWarehouse.api.baseVehicle);
    }

    getBaseVehicle(id) {
        return this.httpHelper.get<IBaseVehicle>(ConstantsWarehouse.api.baseVehicle + "/" + id);
    }

    getModelsByYearIdAndMakeId(yearId: number, makeId: number) {
        return this.httpHelper.get<IModel[]>(ConstantsWarehouse.api.year + "/" + yearId + "/makes/" + makeId + "/models");
    }

    addBaseVehicle(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.baseVehicle, data);
    }

    getPendingChangeRequests() {
        return this.httpHelper.get<IBaseVehicle[]>(ConstantsWarehouse.api.baseVehicle + "/pendingChangeRequests"); //TODO: move to constants
    }

    updateBaseVehicle(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.baseVehicle + "/" + id, data);
    }

    replaceBaseVehicle(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.baseVehicle + "/replace/" + id, data);
    }

    deleteBaseVehicle(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.baseVehicle + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this.httpHelper.get<IBaseVehicleChangeRequestStagingReview>(ConstantsWarehouse.api.baseVehicle + "/changeRequestStaging/" + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.baseVehicle + '/changeRequestStaging/' + id, data);
    }

}