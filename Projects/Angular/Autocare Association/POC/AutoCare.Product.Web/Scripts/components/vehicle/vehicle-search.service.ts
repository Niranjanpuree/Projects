import { Injectable} from "@angular/core";
import { IVehicleSearchInputModel } from "./vehicle-search.model";
import { HttpHelper } from "../httpHelper";
import { ConstantsWarehouse } from "../constants-warehouse";
import { IVehicleSearchViewModel } from "./vehicle-search.model";

@Injectable()
export class VehicleSearchService {

    constructor(private _httpHelper: HttpHelper) {
    }

    search(vehicleSearchInputModel: IVehicleSearchInputModel) {
        //TODO: <IVehicleSearchViewModel> return type would require Observable<T> in post() in httphelper.ts
        return this._httpHelper.post(ConstantsWarehouse.api.vehicleSearch, vehicleSearchInputModel);
    }

    searchByBaseVehicleId(baseVehicleId: number) {
        return this._httpHelper.get<IVehicleSearchViewModel>(ConstantsWarehouse.api.vehicleSearch + "/baseVehicle/" + baseVehicleId);
    }

    searchByVehicleId(vehicleId: number) {
        return this._httpHelper.get<IVehicleSearchViewModel>(ConstantsWarehouse.api.vehicleSearch + "/vehicle/" + vehicleId);
    }

    refreshFacets(vehicleSearchInputModel: IVehicleSearchInputModel) {
        //TODO: <IVehicleSearchViewModel> return type would require Observable<T> in post() in httphelper.ts
        return this._httpHelper.post(ConstantsWarehouse.api.refreshFacets, vehicleSearchInputModel);
    }
}