import { Injectable } from              "@angular/core";
import { IVehicleToMfrBodyCode, IVehicleToMfrBodyCodeChangeRequestStagingReview } from   "../vehicleToMfrBodyCode/vehicleToMfrBodyCode.model";
import { IVehicle } from                "../vehicle/vehicle.model";
import { ConstantsWarehouse } from      "../constants-warehouse";
import { HttpHelper } from              "../httpHelper";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";
import { IVehicleToMfrBodyCodeSearchInputModel
    , IVehicleToMfrBodyCodeSearchViewModel } from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode-search.model";

@Injectable()
export class VehicleToMfrBodyCodeService {
    payload: IVehicleToMfrBodyCode[];
    vehicles: IVehicle[];
    constructor(private httpHelper: HttpHelper) { }

    getVehicleToMfrBodyCodes() {
        return this.httpHelper.get<IVehicleToMfrBodyCode[]>(ConstantsWarehouse.api.vehicleToMfrBodyCode);
    }

    getVehicleToMfrBodyCode(id) {
        return this.httpHelper.get<IVehicleToMfrBodyCode>(ConstantsWarehouse.api.vehicleToMfrBodyCode + "/" + id);
    }

    getByVehicleId(vehicleId: number) {
        return this.httpHelper.get<IVehicleToMfrBodyCode[]>(ConstantsWarehouse.api.vehicle + "/" + vehicleId + "/vehicletobrakeconfigs");
    }

    getVehicleToMfrBodyCodesByVehicleIdsAndMfrBodyCodeIds(vehicleIds?: Number[], mfrBodyCodeIds?: Number[]) {
        if (vehicleIds == null && mfrBodyCodeIds == null) {
            return <any>null;
        }
        let vehicleIdFilter: string = '/,';
        let mfrBodyCodeIdFilter: string = '/,';

        if (vehicleIds != null) {
            vehicleIds.forEach(item => vehicleIdFilter += item + ',');
        }
        if (mfrBodyCodeIds != null) {
            mfrBodyCodeIds.forEach(item => mfrBodyCodeIdFilter += item + ',');
        }

        return this.httpHelper.get<IVehicleToMfrBodyCode[]>(ConstantsWarehouse.api.vehicle + '/' + vehicleIdFilter + '/mfrBodyCodes/' + mfrBodyCodeIdFilter + '/vehicleToMfrBodyCodes');
    }

    addVehicleToMfrBodyCode(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToMfrBodyCode, data);
    }

    //TODO: use getAssociations() which calls azure search
    getVehicleToMfrBodyCodeByMfrBodyCodeId(mfrBodyCodeId: Number) {
        let urlSearch: String = "/mfrBodyCode/" + mfrBodyCodeId;
        return this.httpHelper.get<IVehicleToMfrBodyCode[]>(ConstantsWarehouse.api.vehicleToMfrBodyCode + urlSearch);
    }

    updateVehicleToMfrBodyCode(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.vehicleToMfrBodyCode + "/" + id, data);
    }

    deleteVehicleToMfrBodyCode(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToMfrBodyCode + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this.httpHelper.get<IVehicleToMfrBodyCodeChangeRequestStagingReview>(ConstantsWarehouse.api.vehicleToMfrBodyCode + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToMfrBodyCode + '/changeRequestStaging/' + id, data);
    }

    search(vehicleToMfrBodyCodeSearchInputModel: IVehicleToMfrBodyCodeSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToMfrBodyCodeSearch, vehicleToMfrBodyCodeSearchInputModel);
    }

    searchByVehicleIds(vehicleIds: number[]) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToMfrBodyCodeSearch + '/vehicle/', vehicleIds);
    }

    searchByMfrBodyCodeId(mfrBodyCodeId: number) {
        return this.httpHelper.get<IVehicleToMfrBodyCodeSearchViewModel>(ConstantsWarehouse.api.vehicleToMfrBodyCodeSearch + "/mfrBodyCode/" + mfrBodyCodeId);
    }

    getAssociations(vehicleToMfrBodyCodeSearchInputModel: IVehicleToMfrBodyCodeSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToMfrBodyCodeSearch + "/associations", vehicleToMfrBodyCodeSearchInputModel);
    }

    refreshFacets(vehicleToMfrBodyCodeSearchInputModel: IVehicleToMfrBodyCodeSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToMfrBodyCodeSearchFacets, vehicleToMfrBodyCodeSearchInputModel);
    }

}