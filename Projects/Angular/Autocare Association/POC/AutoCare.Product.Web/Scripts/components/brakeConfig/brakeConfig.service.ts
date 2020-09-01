import { Injectable }         from "@angular/core";
import { IBrakeConfig, IBrakeConfigChangeRequestReview}       from "./brakeConfig.model";
import { IVehicleToBrakeConfig } from '../vehicleTobrakeConfig/vehicleTobrakeConfig.model';
import { ConstantsWarehouse } from "../constants-warehouse";
import { HttpHelper }         from "../httpHelper";
import { IVehicleToBrakeConfigSearchViewModel } from "../vehicleToBrakeConfig/vehicleToBrakeConfig-search.model";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class BrakeConfigService {
    public existingBrakeConfig: IBrakeConfig;
    public replacementBrakeConfig: IBrakeConfig;

    constructor(private httpHelper: HttpHelper) { }

    getBrakeConfigs() {
        return this.httpHelper.get<IBrakeConfig[]>(ConstantsWarehouse.api.brakeConfig);
    }

    getBrakeConfig(id) {
        return this.httpHelper.get<IBrakeConfig>(ConstantsWarehouse.api.brakeConfig + "/" + id);
    }

    addBrakeConfig(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.brakeConfig, data);
    }

    updateBrakeConfig(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.brakeConfig + '/' + id, data);
    }

    replaceBrakeConfig(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.brakeConfig + "/replace/" + id, data);
    }

    getPendingChangeRequests() {
        return this.httpHelper.get<IBrakeConfig[]>(ConstantsWarehouse.api.brakeConfig + "/pendingChangeRequests");
    }

    getByChildIds(frontBrakeTypeId: Number, rearBrakeTypeId: Number, brakeABSId: Number, brakeSystemId: Number) {
        let urlSearch: String = "/frontBrakeType/" + frontBrakeTypeId + "/rearBrakeType/" + rearBrakeTypeId + "/brakeABS/" + brakeABSId + "/brakeSystem/" + brakeSystemId;
        return this.httpHelper.get<IBrakeConfig>(ConstantsWarehouse.api.brakeConfig + urlSearch);
    }

    deleteBrakeConfig(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.brakeConfig + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: number) {
        return this.httpHelper.get<IBrakeConfigChangeRequestReview>(ConstantsWarehouse.api.brakeConfig + "/changeRequestStaging/" + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.brakeConfig + '/changeRequestStaging/' + id, data);
    }
}