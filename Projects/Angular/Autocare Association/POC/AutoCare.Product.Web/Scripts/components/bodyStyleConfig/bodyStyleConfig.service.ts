import { Injectable }         from "@angular/core";
import { IBodyStyleConfig, IBodyStyleConfigChangeRequestReview}       from "./bodyStyleConfig.model";
import { IVehicleToBodyStyleConfig } from '../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.model';
import { ConstantsWarehouse } from "../constants-warehouse";
import { HttpHelper }         from "../httpHelper";
import { IVehicleToBodyStyleConfigSearchViewModel } from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-search.model";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class BodyStyleConfigService {
    public existingBodyStyleConfig: IBodyStyleConfig;
    public replacementBodyStyleConfig: IBodyStyleConfig;

    constructor(private httpHelper: HttpHelper) { }

    getBodyStyleConfigs() {
        return this.httpHelper.get<IBodyStyleConfig[]>(ConstantsWarehouse.api.bodyStyleConfig);
    }

    getBodyStyleConfig(id) {
        return this.httpHelper.get<IBodyStyleConfig>(ConstantsWarehouse.api.bodyStyleConfig + "/" + id);
    }

    addBodyStyleConfig(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.bodyStyleConfig, data);
    }

    updateBodyStyleConfig(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.bodyStyleConfig + '/' + id, data);
    }

    replaceBodyStyleConfig(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.bodyStyleConfig + "/replace/" + id, data);
    }

    getPendingChangeRequests() {
        return this.httpHelper.get<IBodyStyleConfig[]>(ConstantsWarehouse.api.bodyStyleConfig + "/pendingChangeRequests");
    }

    getByChildIds(bodyNumberDoorsId: Number, bodyTypeId: Number) {
        let urlSearch: String = "/bodyNumDoors/" + bodyNumberDoorsId + "/bodyType/" + bodyTypeId;
        return this.httpHelper.get<IBodyStyleConfig>(ConstantsWarehouse.api.bodyStyleConfig + urlSearch);
    }

    deleteBodyStyleConfig(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.bodyStyleConfig + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: number) {
        return this.httpHelper.get<IBodyStyleConfigChangeRequestReview>(ConstantsWarehouse.api.bodyStyleConfig + "/changeRequestStaging/" + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.bodyStyleConfig + '/changeRequestStaging/' + id, data);
    }
}