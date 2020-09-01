import { Injectable }         from "@angular/core";
import { IDriveType, IDriveTypeChangeRequestReview}       from "./driveType.model";
import { IVehicleToDriveType } from '../vehicleToDriveType/vehicleToDriveType.model';
import { ConstantsWarehouse } from "../constants-warehouse";
import { HttpHelper }         from "../httpHelper";
import { IVehicleToDriveTypeSearchViewModel } from "../vehicleToDriveType/vehicleToDriveType-search.model";
import { IChangeRequestReview } from "../changeRequestReview/changeRequestReview.model";

@Injectable()
export class DriveTypeService {
    public existingDriveType: IDriveType;
    public replacementDriveType: IDriveType;
    constructor(private httpHelper: HttpHelper) { }
    getDriveTypes() {
        return this.httpHelper.get<IDriveType[]>(ConstantsWarehouse.api.driveType);
    }

    getDriveType(id) {
        return this.httpHelper.get<IDriveType>(ConstantsWarehouse.api.driveType + "/" + id);
    }

    addDriveType(data) {
        return this.httpHelper.post(ConstantsWarehouse.api.driveType, data);
    }

    updateDriveType(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.driveType + '/' + id, data);
    }

    replaceDriveTypeConfig(id, data) {
        return this.httpHelper.put(ConstantsWarehouse.api.driveType + "/replace/" + id, data);
    }

    getPendingChangeRequests() {
        return this.httpHelper.get<IDriveType[]>(ConstantsWarehouse.api.driveType + "/pendingChangeRequests");
    }
    
    deleteDriveType(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.driveType + "/delete/" + id, data);
    }

    getChangeRequestStaging(id: number) {
        return this.httpHelper.get<IDriveTypeChangeRequestReview>(ConstantsWarehouse.api.driveType + "/changeRequestStaging/" + id);
    }
    submitChangeRequestReview(id: Number, data: IChangeRequestReview) {
        return this.httpHelper.post(ConstantsWarehouse.api.driveType + '/changeRequestStaging/' + id, data);
    }
}