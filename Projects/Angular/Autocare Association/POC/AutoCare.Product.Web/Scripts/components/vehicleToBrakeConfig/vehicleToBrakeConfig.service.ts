import { Injectable } from              "@angular/core";
import { IVehicleToBrakeConfig, IVehicleToBrakeConfigChangeRequestStagingReview } from   "./vehicleToBrakeConfig.model";
import { IVehicle } from                "../vehicle/vehicle.model";
import { ConstantsWarehouse } from      "../constants-warehouse";
import { HttpHelper } from              "../httpHelper";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";
import { IVehicleToBrakeConfigSearchInputModel
    , IVehicleToBrakeConfigSearchViewModel } from "./vehicleToBrakeConfig-search.model";

@Injectable()
export class VehicleToBrakeConfigService {
    payload: IVehicleToBrakeConfig[];
    vehicles: IVehicle[];
    constructor(private httpHelper: HttpHelper) { }

    getVehicleToBrakeConfigs() {
        return this.httpHelper.get<IVehicleToBrakeConfig[]>(ConstantsWarehouse.api.vehicleToBrakeConfig);
    }

    getVehicleToBrakeConfig(id) {
        return this.httpHelper.get<IVehicleToBrakeConfig>(ConstantsWarehouse.api.vehicleToBrakeConfig + "/" + id);
    }

    getByVehicleId(vehicleId: number) {
        return this.httpHelper.get<IVehicleToBrakeConfig[]>(ConstantsWarehouse.api.vehicle + "/" + vehicleId + "/vehicletobrakeconfigs");
    }

    getVehicleToBrakeConfigsByVehicleIdsAndBrakeConfigIds(vehicleIds?: Number[], brakeConfigIds?: Number[]) {
        if (vehicleIds == null && brakeConfigIds == null) {
            return <any>null;
        }
        let vehicleIdFilter: string = '/,';
        let brakeConfigIdFilter: string = '/,';

        if (vehicleIds != null) {
            vehicleIds.forEach(item => vehicleIdFilter += item + ',');
        }
        if (brakeConfigIds != null) {
            brakeConfigIds.forEach(item => brakeConfigIdFilter += item + ',');
        }

        return this.httpHelper.get<IVehicleToBrakeConfig[]>(ConstantsWarehouse.api.vehicle + '/' + vehicleIdFilter + '/brakeConfigs/' + brakeConfigIdFilter + '/vehicleToBrakeConfigs');
    }

    addVehicleToBrakeConfig(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBrakeConfig, data);
    }

    //TODO: use getAssociations() which calls azure search
    getVehicleToBrakeConfigByBrakeConfigId(brakeConfigId: Number) {
        let urlSearch: String = "/brakeConfig/" + brakeConfigId;
        return this.httpHelper.get<IVehicleToBrakeConfig[]>(ConstantsWarehouse.api.vehicleToBrakeConfig + urlSearch);
    }

    updateVehicleToBrakeConfig(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.vehicleToBrakeConfig + "/" + id, data);
    }

    deleteVehicleToBrakeConfig(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBrakeConfig + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this.httpHelper.get<IVehicleToBrakeConfigChangeRequestStagingReview>(ConstantsWarehouse.api.vehicleToBrakeConfig + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBrakeConfig + '/changeRequestStaging/' + id, data);
    }

    search(vehicleToBrakeConfigSearchInputModel: IVehicleToBrakeConfigSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBrakeConfigSearch, vehicleToBrakeConfigSearchInputModel);
    }

    searchByVehicleIds(vehicleIds: number[]) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBrakeConfigSearch + '/vehicle/', vehicleIds);
    }

    searchByBrakeConfigId(brakeConfigId: number) {
        return this.httpHelper.get<IVehicleToBrakeConfigSearchViewModel>(ConstantsWarehouse.api.vehicleToBrakeConfigSearch + "/brakeConfig/" + brakeConfigId);
    }

    getAssociations(vehicleToBrakeConfigSearchInputModel: IVehicleToBrakeConfigSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBrakeConfigSearch + "/associations", vehicleToBrakeConfigSearchInputModel);
    }

    refreshFacets(vehicleToBrakeConfigSearchInputModel: IVehicleToBrakeConfigSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBrakeConfigSearchFacets, vehicleToBrakeConfigSearchInputModel);
    }

}