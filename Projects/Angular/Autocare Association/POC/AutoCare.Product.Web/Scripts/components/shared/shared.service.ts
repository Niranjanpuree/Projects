import { Injectable }              from "@angular/core";
import { IVehicle }                from "../vehicle/vehicle.model";
import { IBrakeConfig }            from "../brakeConfig/brakeConfig.model";
import { IBedConfig } from                 "../bedConfig/bedConfig.model";
import { IChangeRequestSearchViewModel }          from "../change/change-search.model";
import { JwtHelper } from '../jwtHelper';
import { IVehicleSearchViewModel } from "../vehicle/vehicle-search.model";
import { IVehicleToBrakeConfigSearchViewModel }    from "../vehicleToBrakeConfig/vehicleToBrakeConfig-search.model";
import { IVehicleToBedConfigSearchViewModel }    from "../vehicleToBedConfig/vehicleToBedConfig-search.model";
import {IMainHeader, IIdentityInfo, ITokenModel, Role} from './shared.model';
import { IVehicleToBodyStyleConfigSearchViewModel }            from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-search.model";
import { IBodyStyleConfig }            from "../bodyStyleConfig/bodyStyleConfig.model";
import {IVehicleToMfrBodyCodeSearchViewModel} from "../VehicleToMfrBodyCode/VehicleToMfrBodyCode-search.model";
import { IVehicleToWheelBaseSearchViewModel }            from "../vehicleToWheelBase/vehicleToWheelBase-search.model";
import { IWheelBase }            from "../WheelBase/WheelBase.model";

import {IMfrBodyCode} from "../mfrBodyCode/mfrBodyCode.model";
import {IDriveType} from "../driveType/driveType.model";
import {IVehicleToDriveTypeSearchViewModel} from "../VehicleToDriveType/VehicleToDriveType-search.model";

@Injectable()
export class SharedService {
    vehicles: IVehicle[];
    brakeConfigs: IBrakeConfig[];
    changeRequestSearchViewModel: IChangeRequestSearchViewModel;
    vehicleSearchViewModel: IVehicleSearchViewModel;
    // vehicle to brake config
    vehicleToBrakeConfigSearchViewModel: IVehicleToBrakeConfigSearchViewModel;
    vehicleToBedConfigSearchViewModel: IVehicleToBedConfigSearchViewModel;
    bedConfigs: IBedConfig[];
    mfrBodyCodes: IMfrBodyCode[];
    vehicleToMfrBodyCodeSearchViewModel: IVehicleToMfrBodyCodeSearchViewModel;
    driveTypes: IDriveType[];
    vehicleToDriveTypeSearchViewModel: IVehicleToDriveTypeSearchViewModel;
    vehicleToBrakeConfigRouteBack: string;
    referenceDataActiveSubMenuGroupSelected: string = '';
    // vehicle to body style config
    bodyStyleConfigs: IBodyStyleConfig[];
    vehicleToBodyStyleConfigSearchViewModel: IVehicleToBodyStyleConfigSearchViewModel;
    // vehicle to wheel base
    wheelBases: IWheelBase[];
    vehicleToWheelBaseSearchViewModel: IVehicleToWheelBaseSearchViewModel;
    systemMenubarSelected: string;

    private _token: ITokenModel;
    private _jwtHelper = new JwtHelper();
    private _identityInfo: IIdentityInfo = { customerId: "", email: "", isAdmin: false, isRequestor: false, isResearcher: false };

    //For Submenu accroding to mainMenuSelected
    selectedMenuHeaderItem: IMainHeader = { selectedMainHeaderMenuItem: "VCDB" };

    // getter and setter for token: ITokenModel
    public getTokenModel(): ITokenModel {
        // clear token
        this._token = null;

        // note: if user directly access url
        if (!this._token) {
            let sessionToken = sessionStorage.getItem("jwtToken");
            if (sessionToken) {
                this._token = this._jwtHelper.decodeToken(sessionToken);
            }
        }
        // todo: check session expire, this is don't when calling canActivate at authorize.service

        return this._token;
    }

    public setTokenModel(token: ITokenModel) {
        this._token = token;
    }

    public getIdentityInfo(submittedBy: string): IIdentityInfo {
        let token: ITokenModel = this.getTokenModel();
        // clear _identityInfo
        this._identityInfo = { customerId: "", email: "", isAdmin: false, isRequestor: false, isResearcher: false };

        if (token) {
            this._identityInfo.customerId = token.customer_id;
            this._identityInfo.email = token.email;

            // note: assumption: that admin/ researcher/ user all can be requestor.
            // if requestor, then this is precedence.
            // if current user == stagingItem.submittedby then requestor else visitor
            if (submittedBy == token.customer_id) {
                this._identityInfo.isRequestor = true;
            }

            //if (!this._identityInfo.isRequestor) {
            switch (token.role) {
                case <any>(Role[Role.admin]):
                    this._identityInfo.isAdmin = true;
                    break;
                case <any>(Role[Role.researcher]):
                    this._identityInfo.isResearcher = true;
                    break;
                case <any>(Role[Role.user]):
                    // if current user == stagingItem.submittedby then requestor else visitor
                    if (submittedBy == token.customer_id) {
                        this._identityInfo.isRequestor = true;
                    }
                    break;
                default:
                    break;
            }
            //}
        }
        return this._identityInfo;
    }

    // clone method : start
    public clone(source): any {
        var result = source, i, len;
        if (!source
            || source instanceof Number
            || source instanceof String
            || source instanceof Boolean) {
            return result;
        }
        else if (Object.prototype.toString.call(source).slice(8, -1) === 'Array') {
            result = [];
            var resultLen = 0;
            for (i = 0, len = source.length; i < len; i++) {
                result[resultLen++] = this.clone(source[i]);
            }
        }
        else if (typeof source == 'object') {
            result = {};
            for (i in source) {
                if (source.hasOwnProperty(i)) {
                    result[i] = this.clone(source[i]);
                }
            }
        }
        return result;
    };
    // clone method : end
}


