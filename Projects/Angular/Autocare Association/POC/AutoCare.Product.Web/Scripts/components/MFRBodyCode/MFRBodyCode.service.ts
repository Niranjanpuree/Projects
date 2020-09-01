import { Injectable }         from "@angular/core";
import { IMfrBodyCode, IMfrBodyCodeChangeRequestReview}       from "./mfrBodyCode.model";
import { IVehicleToMfrBodyCode } from '../vehicleToMfrBodyCode/vehicleToMfrBodyCode.model';
import { ConstantsWarehouse } from "../constants-warehouse";
import { HttpHelper }         from "../httpHelper";
import { IVehicleToMfrBodyCodeSearchViewModel } from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode-search.model";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class MfrBodyCodeService {
    public existingMfrBodyCode: IMfrBodyCode;
    public replacementMfrBodyCode: IMfrBodyCode
    constructor(private httpHelper: HttpHelper) { }
    getMfrBodyCodes() {
        return this.httpHelper.get<IMfrBodyCode[]>(ConstantsWarehouse.api.mfrBodyCode);
    }

    getMfrBodyCode(id) {
        return this.httpHelper.get<IMfrBodyCode>(ConstantsWarehouse.api.mfrBodyCode + "/" + id);
    }

    addMfrBodyCode(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.mfrBodyCode, data);
    }

    updateMfrBodyCode(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.mfrBodyCode + '/' + id, data);
    }

    replaceMfrBodyCode(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.mfrBodyCode + "/replace/" + id, data);
    }

    getPendingChangeRequests() {
        return this.httpHelper.get<IMfrBodyCode[]>(ConstantsWarehouse.api.mfrBodyCode + "/pendingChangeRequests");
    }
    
    deleteMfrBodyCode(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.mfrBodyCode + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: number) {
        return this.httpHelper.get<IMfrBodyCodeChangeRequestReview>(ConstantsWarehouse.api.mfrBodyCode + "/changeRequestStaging/" + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.mfrBodyCode + '/changeRequestStaging/' + id, data);
    }
}