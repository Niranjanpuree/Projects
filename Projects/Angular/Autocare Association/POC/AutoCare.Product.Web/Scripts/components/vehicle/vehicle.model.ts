import {IVehicleToBrakeConfig} from "../vehicleToBrakeConfig/vehicleToBrakeConfig.model";
import {IVehicleToBedConfig} from "../vehicleToBedConfig/vehicleToBedConfig.model";
import {IVehicleToBodyStyleConfig } from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.model";
import {IVehicleToWheelBase } from "../vehicleToWheelBase/vehicleToWheelBase.model";
import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";
import {IVehicleToDriveType } from "../vehicleToDriveType/vehicleToDriveType.model";
import {IVehicleToMfrBodyCode} from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode.model";

export interface IVehicle {
    id?: number;
    baseVehicleId?: number;
    makeId?: number;
    makeName?: string;
    modelId?: number;
    modelName?: string;
    yearId?: number;
    subModelId?: number;
    subModelName?: string;
    regionId?: number;
    regionName?: string;
    sourceId?: number;
    sourceName?: string;
    publicationStageId?: number;
    publicationStageName?: string;
    publicationStageDate?: Date;
    publicationStageSource?: string;
    publicationEnvironment?: string;
    vehicleToBrakeConfigCount?: number;
    vehicleToBedConfigCount?: number;
    vehicleToBodyStyleConfigCount?: number;
    vehicleToMfrBodyCodeCount?:number;
    isSelected?: boolean;
    comment?: string;
    pendingChangeRequest?: boolean;
    changeRequestId?: number;
    vehicleToBrakeConfigs?: IVehicleToBrakeConfig[];
    vehicleToBedConfigs?: IVehicleToBedConfig[];
    vehicleToBodyStyleConfigs?: IVehicleToBodyStyleConfig[];
    isDelete?: boolean;
    attachments?: any[];
    vehicleToWheelBaseCount?: number;
    vehicleToWheelBases?: IVehicleToWheelBase[];
    vehicleToMfrBodyCodes?: IVehicleToMfrBodyCode[];
    vehicleToDriveTypeCount?: number;
    vehicleToDriveTypes?: IVehicleToDriveType[];
}

export interface IVehicleChangeRequestStagingReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IVehicle;
    entityCurrent?: IVehicle;
    //requestorComments?: Array<ICommentsStaging>;
    //reviewerComments?: Array<ICommentsStaging>;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}