import { Injectable } from              "@angular/core";
import { IVehicleToBodyStyleConfig, IVehicleToBodyStyleConfigChangeRequestStagingReview } from   "./vehicleToBodyStyleConfig.model";
import { IVehicle } from                "../vehicle/vehicle.model";
import { ConstantsWarehouse } from      "../constants-warehouse";
import { HttpHelper } from              "../httpHelper";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";
import { IVehicleToBodyStyleConfigSearchInputModel
    , IVehicleToBodyStyleConfigSearchViewModel } from "./vehicleToBodyStyleConfig-search.model";

@Injectable()
export class VehicleToBodyStyleConfigService {
    payload: IVehicleToBodyStyleConfig[];
    vehicles: IVehicle[];
    constructor(private httpHelper: HttpHelper) { }

    getVehicleToBodyStyleConfigs() {
        return this.httpHelper.get<IVehicleToBodyStyleConfig[]>(ConstantsWarehouse.api.vehicleToBodyStyleConfig);
    }

    getVehicleToBodyStyleConfig(id) {
        return this.httpHelper.get<IVehicleToBodyStyleConfig>(ConstantsWarehouse.api.vehicleToBodyStyleConfig + "/" + id);
    }

    getByVehicleId(vehicleId: number) {
        return this.httpHelper.get<IVehicleToBodyStyleConfig[]>(ConstantsWarehouse.api.vehicle + "/" + vehicleId + "/vehicletobodyStyleconfigs");
    }

    getVehicleToBodyStyleConfigsByVehicleIdsAndBodyStyleConfigIds(vehicleIds?: Number[], bodyStyleConfigIds?: Number[]) {
        if (vehicleIds == null && bodyStyleConfigIds == null) {
            return <any>null;
        }
        let vehicleIdFilter: string = '/,';
        let bedConfigIdFilter: string = '/,';

        if (vehicleIds != null) {
            vehicleIds.forEach(item => vehicleIdFilter += item + ',');
        }
        if (bodyStyleConfigIds != null) {
            bodyStyleConfigIds.forEach(item => bedConfigIdFilter += item + ',');
        }

        return this.httpHelper.get<IVehicleToBodyStyleConfig[]>(ConstantsWarehouse.api.vehicle + '/' + vehicleIdFilter + '/bodyStyleConfigs/' + bedConfigIdFilter + '/vehicleToBodyStyleConfigs');
    }

    addVehicleToBodyStyleConfig(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBodyStyleConfig, data);
    }

    //TODO: use getAssociations() which calls azure search
    getVehicleToBodyStyleConfigByBodyStyleConfigId(bodyStyleConfigId: Number) {
        let urlSearch: String = "/bodyStyleConfig/" + bodyStyleConfigId;
        return this.httpHelper.get<IVehicleToBodyStyleConfig[]>(ConstantsWarehouse.api.vehicleToBodyStyleConfig + urlSearch);
    }

    updateVehicleToBodyStyleConfig(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.vehicleToBodyStyleConfig + "/" + id, data);
    }

    deleteVehicleToBodyStyleConfig(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBodyStyleConfig + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this.httpHelper.get<IVehicleToBodyStyleConfigChangeRequestStagingReview>(ConstantsWarehouse.api.vehicleToBodyStyleConfig + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBodyStyleConfig + '/changeRequestStaging/' + id, data);
    }

    search(vehicleToBodyStyleConfigSearchInputModel: IVehicleToBodyStyleConfigSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBodyStyleConfigSearch, vehicleToBodyStyleConfigSearchInputModel);
    }

    searchByVehicleIds(vehicleIds: number[]) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBodyStyleConfigSearch + '/vehicle/', vehicleIds);
    }

    searchByBodyStyleConfigId(bodyStyleConfigId: number) {
        return this.httpHelper.get<IVehicleToBodyStyleConfigSearchViewModel>(ConstantsWarehouse.api.vehicleToBodyStyleConfigSearch + "/bodyStyleConfig/" + bodyStyleConfigId);
    }

    getAssociations(vehicleToBodyStyleConfigSearchInputModel: IVehicleToBodyStyleConfigSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBodyStyleConfigSearch + "/associations", vehicleToBodyStyleConfigSearchInputModel);
    }

    refreshFacets(vehicleToBodyStyleConfigSearchInputModel: IVehicleToBodyStyleConfigSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToBodyStyleConfigSearch + "/facets", vehicleToBodyStyleConfigSearchInputModel);
    }
}