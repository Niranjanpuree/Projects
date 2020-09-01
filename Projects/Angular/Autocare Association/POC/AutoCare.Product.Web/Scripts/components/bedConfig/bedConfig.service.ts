import { Injectable }         from "@angular/core";
import { IBedConfig, IBedConfigChangeRequestReview}       from "./bedConfig.model";
import { IVehicleToBedConfig } from '../vehicleTobedConfig/vehicleTobedConfig.model';
import { ConstantsWarehouse } from "../constants-warehouse";
import { HttpHelper }         from "../httpHelper";
import { IVehicleToBedConfigSearchViewModel } from "../vehicleToBedConfig/vehicleToBedConfig-search.model";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class BedConfigService {
    public existingBedConfig: IBedConfig;
    public replacementBedConfig: IBedConfig;

    constructor(private httpHelper: HttpHelper) { }

    getBedConfigs() {
        return this.httpHelper.get<IBedConfig[]>(ConstantsWarehouse.api.bedConfig);
    }

    getBedConfig(id) {
        return this.httpHelper.get<IBedConfig>(ConstantsWarehouse.api.bedConfig + "/" + id);
    }

    addBedConfig(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.bedConfig, data);
    }

    updateBedConfig(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.bedConfig + '/' + id, data);
    }

    replaceBedConfig(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.bedConfig + "/replace/" + id, data);
    }

    getPendingChangeRequests() {
        return this.httpHelper.get<IBedConfig[]>(ConstantsWarehouse.api.bedConfig + "/pendingChangeRequests");
    }

    getByChildIds(bedLengthId: Number, bedTypeId: Number) {
        let urlSearch: String = "/bedLength/" + bedLengthId + "/bedType/" + bedTypeId;
        return this.httpHelper.get<IBedConfig>(ConstantsWarehouse.api.bedConfig + urlSearch);
    }

    deleteBedConfig(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.bedConfig + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: number) {
        return this.httpHelper.get<IBedConfigChangeRequestReview>(ConstantsWarehouse.api.bedConfig + "/changeRequestStaging/" + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.bedConfig + '/changeRequestStaging/' + id, data);
    }
}