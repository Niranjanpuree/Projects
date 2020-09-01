import { Injectable } from              "@angular/core";
import { IVehicleToBedConfig, IVehicleToBedConfigChangeRequestStagingReview } from   "./vehicleToBedConfig.model";
import { IVehicle } from                "../vehicle/vehicle.model";
import { ConstantsWarehouse } from      "../constants-warehouse";
import { HttpHelper } from              "../httpHelper";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";
import { IVehicleToBedConfigSearchInputModel
    , IVehicleToBedConfigSearchViewModel } from "./vehicleToBedConfig-search.model";

@Injectable()
export class VehicleToBedConfigService {
    payload: IVehicleToBedConfig[];
    vehicles: IVehicle[];
    constructor(private httpHelper: HttpHelper) { }

    getVehicleToBedConfigs() {
        return this.httpHelper.get<IVehicleToBedConfig[]>(ConstantsWarehouse.api.vehicleToBedConfig);
    }

    getVehicleToBedConfig(id) {
        return this.httpHelper.get<IVehicleToBedConfig>(ConstantsWarehouse.api.vehicleToBedConfig + "/" + id);
    }

    getByVehicleId(vehicleId: number) {
        return this.httpHelper.get<IVehicleToBedConfig[]>(ConstantsWarehouse.api.vehicle + "/" + vehicleId + "/vehicletobedconfigs");
    }

    getVehicleToBedConfigsByVehicleIdsAndBedConfigIds(vehicleIds?: Number[], bedConfigIds?: Number[]) {
        if (vehicleIds == null && bedConfigIds == null) {
            return <any>null;
        }
        let vehicleIdFilter: string = '/,';
        let bedConfigIdFilter: string = '/,';

        if (vehicleIds != null) {
            vehicleIds.forEach(item => vehicleIdFilter += item + ',');
        }
        if (bedConfigIds != null) {
            bedConfigIds.forEach(item => bedConfigIdFilter += item + ',');
        }

        return this.httpHelper.get<IVehicleToBedConfig[]>(ConstantsWarehouse.api.vehicle + '/' + vehicleIdFilter + '/bedConfigs/' + bedConfigIdFilter + '/vehicleToBedConfigs');
    }

    addVehicleToBedConfig(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBedConfig, data);
    }

    //TODO: use getAssociations() which calls azure search
    getVehicleToBedConfigByBedConfigId(bedConfigId: Number) {
        let urlSearch: String = "/bedConfig/" + bedConfigId;
        return this.httpHelper.get<IVehicleToBedConfig[]>(ConstantsWarehouse.api.vehicleToBedConfig + urlSearch);
    }

    updateVehicleToBedConfig(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.vehicleToBedConfig + "/" + id, data);
    }

    deleteVehicleToBedConfig(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBedConfig + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this.httpHelper.get<IVehicleToBedConfigChangeRequestStagingReview>(ConstantsWarehouse.api.vehicleToBedConfig + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBedConfig + '/changeRequestStaging/' + id, data);
    }

    search(vehicleToBedConfigSearchInputModel: IVehicleToBedConfigSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBedConfigSearch, vehicleToBedConfigSearchInputModel);
    }

    searchByBaseVehicleId(baseVehicleId: number) {
        return this.httpHelper.get<IVehicleToBedConfigSearchViewModel>(ConstantsWarehouse.api.vehicleToBedConfigSearch + "/baseVehicle/" + baseVehicleId);
    }

    searchByVehicleId(vehicleId: number) {
        return this.httpHelper.get<IVehicleToBedConfigSearchViewModel>(ConstantsWarehouse.api.vehicleToBedConfigSearch + "/vehicle/" + vehicleId);
    }

    searchByVehicleIds(vehicleIds: number[]) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBedConfigSearch + '/vehicle/', vehicleIds);
    }

    searchByBedConfigId(bedConfigId: number) {
        return this.httpHelper.get<IVehicleToBedConfigSearchViewModel>(ConstantsWarehouse.api.vehicleToBedConfigSearch + "/bedConfig/" + bedConfigId);
    }

    getAssociations(vehicleToBedConfigSearchInputModel: IVehicleToBedConfigSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBedConfigSearch + "/associations", vehicleToBedConfigSearchInputModel);
    }

    refreshFacets(vehicleToBedConfigSearchInputModel: IVehicleToBedConfigSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBedConfigSearchFacets, vehicleToBedConfigSearchInputModel);
    }

}