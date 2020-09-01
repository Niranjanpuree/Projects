import { IVehicle } from "../vehicle/vehicle.model";
import { IBrakeConfig } from "../brakeConfig/brakeConfig.model";

export interface IVehicleToBrakeConfig {
    id?: number;
    numberOfBrakesAssociation?: number;
    changeRequestId?: number;
    isSelected?: boolean;
    vehicleId?: number;
    brakeConfigId?: number;
    brakeConfig?: IBrakeConfig;
    vehicle?: IVehicle;
    comment?: string;
    attachments?: any[]; 
    frontBrakeTypeId ?: number;
    frontBrakeTypeName ?: string;
    rearBrakeTypeId ?: number;
    rearBrakeTypeName ?: string;
    brakeSystemId ?: number;
    brakeSystemName ?: string;
    brakeABSId ?: number;
    brakeABSName ?: string;
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IVehicleToBrakeConfigChangeRequestStagingReview extends IReview  {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IVehicleToBrakeConfig;
    entityCurrent?: IVehicleToBrakeConfig;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
