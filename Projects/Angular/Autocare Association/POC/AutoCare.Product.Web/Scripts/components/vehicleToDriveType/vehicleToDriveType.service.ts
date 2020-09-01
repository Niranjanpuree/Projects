import { Injectable } from              "@angular/core";
import { IVehicleToDriveType, IVehicleToDriveTypeChangeRequestStagingReview } from   "../vehicleToDriveType/vehicleToDriveType.model";
import { IVehicle } from                "../vehicle/vehicle.model";
import { ConstantsWarehouse } from      "../constants-warehouse";
import { HttpHelper } from              "../httpHelper";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";
import { IVehicleToDriveTypeSearchInputModel
    , IVehicleToDriveTypeSearchViewModel } from "../vehicleToDriveType/vehicleToDriveType-search.model";

@Injectable()
export class VehicleToDriveTypeService {
    payload: IVehicleToDriveType[];
    vehicles: IVehicle[];
    constructor(private httpHelper: HttpHelper) { }

    getVehicleToDriveTypes() {
        return this.httpHelper.get<IVehicleToDriveType[]>(ConstantsWarehouse.api.vehicleToDriveType);
    }

    getVehicleToDriveType(id) {
        return this.httpHelper.get<IVehicleToDriveType>(ConstantsWarehouse.api.vehicleToDriveType + "/" + id);
    }

    getByVehicleId(vehicleId: number) {
        return this.httpHelper.get<IVehicleToDriveType[]>(ConstantsWarehouse.api.vehicle + "/" + vehicleId + "/vehicletodrivetypes");
    }

    getVehicleToDriveTypesByVehicleIdsAndDriveTypeIds(vehicleIds?: Number[], driveTypeIds?: Number[]) {
        if (vehicleIds == null && driveTypeIds == null) {
            return <any>null;
        }
        let vehicleIdFilter: string = '/,';
        let driveTypeIdFilter: string = '/,';

        if (vehicleIds != null) {
            vehicleIds.forEach(item => vehicleIdFilter += item + ',');
        }
        if (driveTypeIds != null) {
            driveTypeIds.forEach(item => driveTypeIdFilter += item + ',');
        }

        return this.httpHelper.get<IVehicleToDriveType[]>(ConstantsWarehouse.api.vehicle + '/' + vehicleIdFilter + '/driveTypes/' + driveTypeIdFilter + '/vehicleToDriveTypes');
    }

    addVehicleToDriveType(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToDriveType, data);
    }

    //TODO: use getAssociations() which calls azure search
    getVehicleToDriveTypeByDriveTypeId(driveTypeId: Number) {
        let urlSearch: String = "/driveType/" + driveTypeId;
        return this.httpHelper.get<IVehicleToDriveType[]>(ConstantsWarehouse.api.vehicleToDriveType + urlSearch);
    }

    updateVehicleToDriveType(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.vehicleToDriveType + "/" + id, data);
    }

    deleteVehicleToDriveType(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToDriveType + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: Number) {
        return this.httpHelper.get<IVehicleToDriveTypeChangeRequestStagingReview>(ConstantsWarehouse.api.vehicleToDriveType + '/changeRequestStaging/' + id);
    }

    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToDriveType + '/changeRequestStaging/' + id, data);
    }

    search(vehicleToDriveTypeSearchInputModel: IVehicleToDriveTypeSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToDriveTypeSearch, vehicleToDriveTypeSearchInputModel);
    }

    searchByVehicleIds(vehicleIds: number[]) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToDriveTypeSearch + '/vehicle/', vehicleIds);
    }

    searchByDriveTypeId(driveTypeId: number) {
        return this.httpHelper.get<IVehicleToDriveTypeSearchViewModel>(ConstantsWarehouse.api.vehicleToDriveTypeSearch + "/driveType/" + driveTypeId);
    }

    getAssociations(vehicleToDriveTypeSearchInputModel: IVehicleToDriveTypeSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToDriveTypeSearch + "/associations", vehicleToDriveTypeSearchInputModel);
    }

    refreshFacets(vehicleToDriveTypeSearchInputModel: IVehicleToDriveTypeSearchInputModel) {
        return this.httpHelper.post(ConstantsWarehouse.api.vehicleToDriveTypeSearchFacets, vehicleToDriveTypeSearchInputModel);
    }

}