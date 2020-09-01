import { Injectable } from              "@angular/core";
import { IVehicleToWheelBase, IVehicleToWheelBaseChangeRequestStagingReview } from   "./vehicleToWheelBase.model";
import { IVehicle } from                "../vehicle/vehicle.model";
import { ConstantsWarehouse } from      "../constants-warehouse";
import { HttpHelper } from              "../httpHelper";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";
import { IVehicleToWheelBaseSearchInputModel
    , IVehicleToWheelBaseSearchViewModel } from "./vehicleToWheelBase-search.model";

@Injectable()
export class VehicleToWheelBaseService {
    payload: IVehicleToWheelBase[];
    vehicles: IVehicle[];
    constructor(private httpHelper: HttpHelper) { }

    getVehicleToWheelBases() {
        return this.httpHelper.get<IVehicleToWheelBase[]>(ConstantsWarehouse.api.vehicleToWheelBase);
    }

    getVehicleToWheelBase(id) {
        return this.httpHelper.get<IVehicleToWheelBase>(ConstantsWarehouse.api.vehicleToWheelBase + "/" + id);
    }

    getByVehicleId(vehicleId: number) {
        return this.httpHelper.get<IVehicleToWheelBase[]>(ConstantsWarehouse.api.vehicle + "/" + vehicleId + "/vehicleToWheelBases");
    }

    getVehicleToWheelBasesByVehicleIdsAndWheelBaseIds(vehicleIds?: Number[], wheelBaseIds?: Number[]) {
        if (vehicleIds == null && wheelBaseIds == null) {
            return <any>null;
        }
        let vehicleIdFilter: string = '/,';
        let wheelBaseIdsFilter: string = '/,';

        if (vehicleIds != null) {
            vehicleIds.forEach(item => vehicleIdFilter += item + ',');
        }
        if (wheelBaseIds != null) {
            wheelBaseIds.forEach(item => wheelBaseIdsFilter += item + ',');
        }

        return this.httpHelper.get<IVehicleToWheelBase[]>(ConstantsWarehouse.api.vehicle + '/' + vehicleIdFilter + '/wheelBases/' + wheelBaseIdsFilter + '/vehicleToWheelBases');
    }

    addVehicleToWheelBase(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToWheelBase, data);
    }

    //TODO: use getAssociations() which calls azure search
    getVehicleToWheelBaseByWheelBaseId(wheelBaseId: Number) {
        let urlSearch: String = "/wheelBase/" + wheelBaseId;
        return this.httpHelper.get<IVehicleToWheelBase[]>(ConstantsWarehouse.api.vehicleToWheelBase + urlSearch);
    }

    updateVehicleToWheelBase(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.vehicleToWheelBase + "/" + id, data);
    }

    deleteVehicleToWheelBase(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToWheelBase + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this.httpHelper.get<IVehicleToWheelBaseChangeRequestStagingReview>(ConstantsWarehouse.api.vehicleToWheelBase + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToWheelBase + '/changeRequestStaging/' + id, data);
    }

    search(vehicleToWheelBaseSearchInputModel: IVehicleToWheelBaseSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToWheelBaseSearch, vehicleToWheelBaseSearchInputModel);
    }

    searchByVehicleIds(vehicleIds: number[]) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToWheelBaseSearch + '/vehicle/', vehicleIds);
    }

    searchByWheelBaseId(wheelBaseId: number) {
        return this.httpHelper.get<IVehicleToWheelBaseSearchViewModel>(ConstantsWarehouse.api.vehicleToWheelBaseSearch + "/wheelBase/" + wheelBaseId);
    }

    getAssociations(vehicleToWheelBaseSearchInputModel: IVehicleToWheelBaseSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToWheelBaseSearch + "/associations", vehicleToWheelBaseSearchInputModel);
    }

    refreshFacets(vehicleToWheelBaseSearchInputModel: IVehicleToWheelBaseSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToWheelBaseSearch + "/facets", vehicleToWheelBaseSearchInputModel);
    }

}