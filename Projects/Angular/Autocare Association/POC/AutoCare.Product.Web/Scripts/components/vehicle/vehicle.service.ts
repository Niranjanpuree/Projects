import { Injectable }                           from "@angular/core";
import { IVehicle }                             from "./vehicle.model";
import { ConstantsWarehouse }                   from "../constants-warehouse";
import { URLSearchParams }                      from "@angular/http"
import { HttpHelper }                           from "../httpHelper";
import { IVehicleChangeRequestStagingReview }   from "./vehicle.model";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";


@Injectable()
export class VehicleService {
    constructor(private httpHelper: HttpHelper) {
    }

    getVehicles() {
        return this.httpHelper.get<IVehicle[]>(ConstantsWarehouse.api.vehicle);
    }

    getVehicle(id) {
        return this.httpHelper.get<IVehicle>(ConstantsWarehouse.api.vehicle + "/" + id);
    }

    getVehicleByBaseVehicleIdSubModelIdAndRegionId(baseVehicleId: number, subModelId: number, regionId: number) {
        return this.httpHelper.get<IVehicle>(ConstantsWarehouse.api.baseVehicle + "/" + baseVehicleId + "/subModels/" + subModelId + "/regions/" + regionId + "/vehicles");
    }

    getVehiclesByBaseVehicleId(baseVehicleId: number) {
        return this.httpHelper.get<IVehicle[]>(ConstantsWarehouse.api.baseVehicle + "/" + baseVehicleId + "/vehicles");
    }
    updateVehicle(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.vehicle + "/" + id, data);
    }

    deleteVehicle(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicle + "/delete/" + id, data);
    }

    getPendingChangeRequest(baseVehicleId) {

        return this.httpHelper.get<IVehicle[]>(ConstantsWarehouse.api.vehiclePendingChangeRequest + "/" + baseVehicleId);
    }

    
    createVehicleChangeRequests(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicle, data);
    }

    getChangeRequestStaging(id: Number) {
        return this.httpHelper.get<IVehicleChangeRequestStagingReview>(ConstantsWarehouse.api.vehicle + "/changeRequestStaging/" + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicle + '/changeRequestStaging/' + id, data);
    }
}